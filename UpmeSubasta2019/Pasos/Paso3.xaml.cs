using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Reporting.WinForms;
using System.Data;
using UpmeSubasta2019.Data;
using UpmeSubasta2019.Reportes;

namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for Paso2.xaml
    /// </summary>
    public partial class Paso3 : UserControl
    {
        //public static string LogOfertas.Text = null;
        public static string Mensaje = null;
        bool OfertasOk = true;
        public static int CerrarPaso3 = 0;
        public Paso3()
        {
            DataContext = new Paso3ViewModel();
            InitializeComponent();
        }

        public void MostrarOfertasTodas(string query, int proceso, string reporte, string archivopdf)
        {
            // Proceso: 1 = Compras, 2 = Ventas
            DataTable dt = null;

            try
            {
                dt = DAL.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en la consulta de datos de las ofertas");
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex.Message + "\r\n";
                LogOfertas.Text = LogOfertas.Text + Mensaje;
                DAL.InsertarLog(Mensaje, "Datos Ofertas Venta", "Datos Ofertas Venta");
                //throw;
            }

            if (dt.Rows.Count != 0)
            {
                switch (proceso)
                {
                    case 1:
                        ActualizarReporte(ReporteGeneradores, dt, reporte, archivopdf);
                        break;
                    case 2:
                        ActualizarReporte(ReporteComercializadores, dt, reporte, archivopdf);
                        break;
                }
            }
            else
            {
                Mensaje = "No existen datos de la consulta de datos ..." + "\r\n";
                LogOfertas.Text = LogOfertas.Text + Mensaje;
                DAL.InsertarLog(Mensaje, "Ofertas reporte " + archivopdf, "Ofertas reporte " + archivopdf);
            }

        }

        // Actualiza los datos del reporte
        private void ActualizarReporte(ReportViewer reporteViewer, DataTable dt, string reporte, string archivopdf)
        {
            reporteViewer.Reset();
            ReportDataSource ds = new ReportDataSource("DataSet1", dt);
            reporteViewer.LocalReport.ReportEmbeddedResource = reporte;
            reporteViewer.LocalReport.DataSources.Add(ds);
            reporteViewer.RefreshReport();
            Exportar.ExportaPDF(reporteViewer, archivopdf);
        }

        private void LogOfertasTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            LogOfertas.ScrollToEnd();
        }

        public bool CargarOfertasDePostgresql()
        {
            bool resultado = false;
            var nombreConsulta = string.Empty;
            string errorTipo = "Carga de Ofertas Upme Sobre 2";

            try
            {
                var mensaje = "---------------------------------------------\r\n";
                mensaje += "Iniciando proceso de lectura de datos del Sobre 2.\r\n";
                LogOfertas.Text += mensaje;
                DAL.InsertarLog(mensaje, "", "Sobre2");

                var dt = DAL.ExecuteQuery("SELECT * FROM ConsultasBD WHERE (Operacion = 'query' OR Operacion = 'grabar') AND Proceso = 'sobre2'");
                var consultas = Helper.ConvertDataTableToList<ConsultasBd>(dt);
                var consultasPostgresql = consultas.Where(c => c.Operacion == "query");

                foreach (var consultaBd in consultasPostgresql)
                {
                    nombreConsulta = consultaBd.Nombre;
                    var dtDatos = DAL.ExecuteQueryPostgres(consultaBd.Sql);
                    int numRegistrosLeidos = dtDatos.Rows.Count;
                    if (numRegistrosLeidos > 0)
                    {
                        var spCarga = consultas.First(c => c.Operacion == "grabar" && c.Tipo == consultaBd.Tipo && c.Proceso == consultaBd.Proceso);
                        int numRegistrosInsertados = DAL.ExecuteQueryParametro(spCarga.Sql, spCarga.Parametro, dtDatos);
                        if (numRegistrosLeidos != numRegistrosInsertados)
                        {
                            mensaje = string.Format("Numero de registros leidos de {0} no es igual a los insertados... \r\n Nombre de la consulta: {1}", consultaBd.Tipo, nombreConsulta);
                            LogError(mensaje, consultaBd.Tipo, consultaBd.Proceso);
                            return false;
                        }
                    }
                    mensaje = string.Format("Se cargaron los datos de {0} de {1}\r\n", consultaBd.Tipo, consultaBd.Proceso);
                    LogOfertas.Text += mensaje;
                    DAL.InsertarLog(mensaje, consultaBd.Tipo, consultaBd.Proceso);
                }

                mensaje = "El proceso de lectura de datos del Sobre 2 finalizo correctamente.\r\n";
                LogOfertas.Text += mensaje;
                DAL.InsertarLog(mensaje, "", "Sobre2");
                resultado = true;
            }
            catch (Exception error)
            {
                var mensaje = string.Format("Error en la lectura de las ofertas... {0}\r\nNombre de la consulta: {1}", error.Message, nombreConsulta);
                LogOfertas.Text += mensaje;
                LogError(mensaje, errorTipo, errorTipo, true);
                resultado = false;
            }
            return resultado;
        }

        public bool LimpiarTablas()
        {
            string errorTipo = "Borrado de datos Upme Sobre 2";
            try
            {
                var consultasBorrar = DAL.ExecuteQuery("SELECT * FROM ConsultasBD WHERE Operacion = 'delete' AND Proceso = 'sobre2' AND BD =  'sql'");
                var consultasBorrarTablasSql = Helper.ConvertDataTableToList<ConsultasBd>(consultasBorrar);
                foreach (var consultaBd in consultasBorrarTablasSql)
                {
                    DAL.ExecuteQueryNormal(consultaBd.Sql);
                }
                return true;
            }
            catch (Exception error)
            {
                var mensaje = string.Format("Error en borrado de las ofertas... {0}\r\n", error.Message);
                LogError(mensaje, errorTipo, errorTipo, true);
                return false;
            }
        }

        private void LogError(string mensaje, string tipo, string proceso, bool messageBox = false)
        {
            LogOfertas.Text += mensaje;
            DAL.InsertarLog(mensaje, tipo, proceso);
            if (messageBox) MessageBox.Show(mensaje, "Error");
        }

        public void CargaOfertas(object sender, RoutedEventArgs ex)
        {
            bool Validar = ConsultarPasos();
            string errorTipo = "Carga de Ofertas Upme Sobre 2";

            if (Validar && CerrarPaso3 == 0)
            {
                var resultado = LimpiarTablas();
                if (resultado)
                {
                    resultado = CargarOfertasDePostgresql();
                    if (resultado)
                    {
                        MostrarResumenOfertasCompra();
                        MostrarResumenOfertasVenta();
                    }
                }
                else
                {
                    var mensaje = string.Format("Error en la lectura de las ofertas...Limpiando tablas\r\n");
                    LogError(mensaje, errorTipo, errorTipo, true);
                }
            }
            else
            {
                MessageBox.Show("El paso ya fue cerrado, los datos de ofertas de sobre 2 ya fueron cargados y validados", "Cierre de pasos");
                MostrarOfertasTodas("exec [dbo].[ConsultaDatosOfertaCompra] 2, Subasta", 1, "UpmeSubasta2019.Reportes.OfertasCompra.rdlc", "OfertasCompra");
                MostrarOfertasTodas("exec [dbo].[ConsultaDatosOfertaVenta] 2, Subasta", 2, "UpmeSubasta2019.Reportes.OfertasVenta.rdlc", "OfertasVenta");
            }
        }

        private void CerrarPaso(object sender, RoutedEventArgs e)
        {
            // Validar si el paso ya no fue cerrado
            bool Validar = ConsultarPasos();
            if (Validar)
            {
                if (OfertasOk)
                {
                    Mensaje = "Cierre paso 3 exitoso." + "\r\n";
                    LogOfertas.Text = LogOfertas.Text + Mensaje;
                    DAL.InsertarLog(Mensaje, "Cierre paso 3", "Cierre Pasos");
                    MessageBox.Show(Mensaje, "Cierre de pasos");
                    CerrarPaso3 = 1;
                }
                else
                {
                    Mensaje = "Cierre paso 3 fallido. Aún no ha sido realizado el proceso de carga de las ofertas del Sobre 2" + "\r\n";
                    LogOfertas.Text = LogOfertas.Text + Mensaje;
                    DAL.InsertarLog(Mensaje, "Cierre paso 3", "Cierre Pasos");
                    MessageBox.Show(Mensaje, "Cierre de pasos");
                    CerrarPaso3 = 0;
                }
            }
            else
            {
                MessageBox.Show("El paso ya fue cerrado, los datos de ofertas de sobre 2 ya fueron cargados y validados", "Cierre de pasos");
            }
        }

        public bool ConsultarPasos()
        {
            bool ValidarPaso = false;
            DataTable dt = null;
            string Query1 = "EXEC DBO.ConsultaDatosPasos 'Cierre Pasos', 'Cierre paso 3', 'Cierre paso 3 exitoso.'";
            try
            {
                dt = DAL.ExecuteQuery(Query1);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Error en la consulta de datos de los pasos de las ofertas");
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message + "\r\n";
                LogOfertas.Text = LogOfertas.Text + Mensaje;
                DAL.InsertarLog(Mensaje, "Datos Ofertas Venta", "Datos Ofertas Venta");
                //throw;

            }

            if (dt.Rows.Count == 0)
                ValidarPaso = true;
            else
                ValidarPaso = false;

            return ValidarPaso;
        }

        private void MostrarOfertas(object sender, RoutedEventArgs e)
        {
            //MostrarOfertasTodas("exec [dbo].[ConsultaDatosOfertaCompra] 2, Subasta", 1, "UpmeSubasta2019.Reportes.OfertasCompra.rdlc", "OfertasCompra");
            //MostrarOfertasTodas("exec [dbo].[ConsultaDatosOfertaVenta] 2, Subasta", 2, "UpmeSubasta2019.Reportes.OfertasVenta.rdlc", "OfertaVenta");

            MostrarOfertasCompra();
            MostrarOfertasVenta();


        }

        public void MostrarOfertasCompra()
        {
            string Query1 = "exec [dbo].[ConsultaDatosOfertaCompra] 2, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.OfertasCompra.rdlc", "OfertasCompra");
        }

        public void MostrarResumenOfertasVenta()
        {
            string Query1 = "exec [dbo].[ResumenOfertasVenta] 2, Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.ResumenOfertaVenta.rdlc", "ResumenVenta");
        }


        public void MostrarResumenOfertasCompra()
        {
            string Query1 = "exec [dbo].[ResumenOfertasCompra] 2, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.ResumenOfertaCompra.rdlc", "ResumenCompra");
        }

        public void MostrarOfertasVenta()
        {
            string Query1 = "exec [dbo].[ConsultaDatosOfertaVenta] 2, Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.OfertasVenta.rdlc", "OfertaVenta");
        }
    }
}

