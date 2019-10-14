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
    public partial class Paso2 : UserControl
    {
        //public static string LogOfertas.Text = null;
        private static string Mensaje = null;
        bool OfertasOk = true;
        private static int CerrarPaso2 = 0;

        public Paso2()
        {
            DataContext = new Paso2ViewModel();
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
                    case 3:
                        ActualizarReporte(ReporteCriterios, dt, reporte, archivopdf);
                        break;
                    case 4:
                        ActualizarReporte(ReporteCompravsGene, dt, reporte, archivopdf);
                        break;
                }
            }
            else
            {
                Mensaje = "No existen datos de la consulta de datos resumen de las ofertas ..." + "\r\n";
                LogOfertas.Text = LogOfertas.Text + Mensaje;
                DAL.InsertarLog(Mensaje, "Resumen de Ofertas Venta", "Resumen de Ofertas Venta");
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

        public bool CargarOfertasDePostgresql(string QueryaCargar)
        {
            // QueryaCargar = Tendra la condicion que determina cuales tablas va a cargar
            // PErmitir carga una tabla especifica o todas
            bool resultado = false;
            var nombreConsulta = string.Empty;
            string errorTipo = "Carga de Ofertas Upme Sobre 1";

            try
            {
                var mensaje = "---------------------------------------------\r\n";
                mensaje += "Iniciando proceso de lectura de datos del Sobre 1.\r\n";
                LogOfertas.Text += mensaje;
                DAL.InsertarLog(mensaje, "", "Sobre1");

                //var dt = DAL.ExecuteQuery("SELECT * FROM ConsultasBD WHERE (Operacion = 'query' OR Operacion = 'grabar') AND Proceso = 'sobre1'");
                var dt = DAL.ExecuteQuery(QueryaCargar);
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
                             mensaje = string.Format("Numero de registros leidos de Generadores no es igual a los insertados... \r\n Nombre de la consulta: {0}", nombreConsulta);
                            LogError(mensaje, consultaBd.Tipo, consultaBd.Proceso);
                            return false;
                        }
                    }
                    mensaje = string.Format("Se cargaron los datos de {0} de {1}\r\n", consultaBd.Tipo, consultaBd.Proceso);
                    LogOfertas.Text += mensaje;
                    DAL.InsertarLog(mensaje, consultaBd.Tipo, consultaBd.Proceso);
                }

                mensaje = "El proceso de lectura de datos del Sobre 1 finalizo correctamente.\r\n";
                LogOfertas.Text += mensaje;
                DAL.InsertarLog(mensaje, "", "Sobre1");
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
            string errorTipo = "Carga de Ofertas Upme Sobre 1";
            try
            {
                var consultasBorrar = DAL.ExecuteQuery("SELECT * FROM ConsultasBD WHERE Operacion = 'delete' AND Proceso = 'sobre1' AND BD =  'sql'");
                var consultasBorrarTablasSql = Helper.ConvertDataTableToList<ConsultasBd>(consultasBorrar);
                foreach (var consultaBd in consultasBorrarTablasSql)
                {
                    DAL.ExecuteQueryNormal(consultaBd.Sql);
                }
                return true;
            }
            catch (Exception error)
            {
                var mensaje = string.Format("Error en la lectura de las ofertas... {0}\r\n", error.Message);
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
            string errorTipo = "Carga de Ofertas Upme Sobre 1";

            if (Validar && CerrarPaso2 == 0)
            {
                var resultado = LimpiarTablas();
                if (resultado)
                {
                    resultado = CargarOfertasDePostgresql("SELECT * FROM ConsultasBD WHERE (Operacion = 'query' OR Operacion = 'grabar') AND Proceso = 'sobre1'");
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
                MessageBox.Show("El paso ya fue cerrado, los datos de ofertas de sobre 1 ya fueron cargados y validados", "Cierre de pasos");
                MostrarOfertasTodas("exec [dbo].[ConsultaDatosOfertaCompra] 1, Subasta", 1, "UpmeSubasta2019.Reportes.OfertaSobre1Compra.rdlc", "ComercializadoresSobre1");
                MostrarOfertasTodas("exec [dbo].[ConsultaDatosProyectos] Subasta", 2, "UpmeSubasta2019.Reportes.OfertasSobre1Proyectos.rdlc", "GeneradoresSobre1");
                MostrarOfertasTodas("exec [dbo].[ComprasvsGeneracion] 1,Subasta", 4, "UpmeSubasta2019.Reportes.ComprasvsGenera.rdlc", "GraficoComprasvsGeneracion");
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
                    Mensaje = "Cierre paso 2 exitoso." + "\r\n";
                    LogOfertas.Text = LogOfertas.Text + Mensaje;
                    DAL.InsertarLog(Mensaje, "Cierre paso 2", "Cierre Pasos");
                    MessageBox.Show(Mensaje, "Cierre de pasos");
                    CerrarPaso2 = 1;
                }
                else
                {
                    Mensaje = "Cierre paso 2 fallido. Aún no ha sido realizado el proceso de carga de las ofertas del Sobre 1" + "\r\n";
                    LogOfertas.Text = LogOfertas.Text + Mensaje;
                    DAL.InsertarLog(Mensaje, "Cierre paso 2", "Cierre Pasos");
                    MessageBox.Show(Mensaje, "Cierre de pasos");
                    CerrarPaso2 = 0;
                }
            }
            else
            {
                MessageBox.Show("El paso ya fue cerrado, los datos de ofertas de sobre 1 ya fueron cargados y validados", "Cierre de pasos");
            }
        }

        private void CriterioCompetencia(object sender, RoutedEventArgs e)
        {
            // Validar Criterio de competencia
            DataTable dt = null;
            int Cumple = 0;

            Mensaje = "Ejecutando Criterio de Competencia ..." + "\r\n";
            LogOfertas.Text = LogOfertas.Text + Mensaje;
            DAL.InsertarLog(Mensaje, "Criterio de Competencia", "Criterio de Competencia");
            String QueryCriterios = "exec VerificarCriterioCompetencia 1, Subasta";

            try
            {
                dt = DAL.ExecuteQuery(QueryCriterios);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Error en el proceso de calculo de los criterios de competencia");
                Mensaje = "Error en el proceso de calculo de los criterios de competencia..." + ex1.Message + "\r\n";
                LogOfertas.Text = LogOfertas.Text + Mensaje;
                DAL.InsertarLog(Mensaje, "Criterio de Competencia", "Criterio de Competencia");
                //throw;

            }

            if (dt.Rows.Count == 0)
            {

                MessageBox.Show("Criterio de Competencia", "Error en la consulta de datos de los criterios de competencia");
                Mensaje = "Error en la consulta de datos de criterios de competencia, no se generaron datos" + "\r\n";
                LogOfertas.Text = LogOfertas.Text + Mensaje;
                DAL.InsertarLog(Mensaje, "Criterio de Competencia", "Criterio de Competencia");
            }
            else
            {
                Mensaje = "Criterios de Competencia Ejecutados ..." + "\r\n";
                LogOfertas.Text = LogOfertas.Text + Mensaje;
                DAL.InsertarLog(Mensaje, "Criterios de Competencia", "Criterios de Competencia");
                DataRow[] results = dt.Select(@"Cumple= 'No Cumple'");
                Cumple = results.Count();
                //Cumple = dt.Rows.IndexOf(dt.Select(@"Cumple= 'No Cumple'")[0]);
                if (Cumple != 0)
                {
                    Mensaje = "Criterios de Competencia Resultados ... NO CUMPLE" + "\r\n";
                    LogOfertas.Text = LogOfertas.Text + Mensaje;
                    DAL.InsertarLog(Mensaje, "Criterios de Competencia", "Criterios de Competencia");
                    MessageBox.Show("No Cumple Criterio de Competencia ", "Criterio de Competencia");
                    /// deshabilitar los pasos siguientes
                }
                else
                {
                    Mensaje = "Criterios de Competencia Resultados ... CUMPLE" + "\r\n";
                    LogOfertas.Text = LogOfertas.Text + Mensaje;
                    DAL.InsertarLog(Mensaje, "Criterios de Competencia", "Criterios de Competencia");
                    MessageBox.Show("Criterio de Competencia", "Cumple Criterio de Competencia ");

                }
                MostrarOfertasTodas(QueryCriterios, 3, "UpmeSubasta2019.Reportes.CiterioCompetencia.rdlc", "CrietrioCompetenciaSobre1");
            }
        }

        public bool ConsultarPasos()
        {
            bool ValidarPaso = false;
            DataTable dt = null;
            string Query1 = "EXEC DBO.ConsultaDatosPasos 'Cierre Pasos', 'Cierre paso 2', 'Cierre paso 2 exitoso.'";
            try
            {
                dt = DAL.ExecuteQuery(Query1);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Error en la consulta de datos de los pasos de las ofertas");
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message + "\r\n";
                LogOfertas.Text = Mensaje;
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
            MostrarOfertasTodas("exec [dbo].[ConsultaDatosOfertaCompra] 1, Subasta", 1, "UpmeSubasta2019.Reportes.OfertaSobre1Compra.rdlc", "ComercializadoresSobre1");
            MostrarOfertasTodas("exec [dbo].[ConsultaDatosProyectos] Subasta", 2, "UpmeSubasta2019.Reportes.OfertasSobre1Proyectos.rdlc", "GeneradoresSobre1");
            MostrarOfertasTodas("exec [dbo].[ComprasvsGeneracion] 1,Subasta", 4, "UpmeSubasta2019.Reportes.ComprasvsGenera.rdlc", "GraficoComprasvsGeneracion");
        }

        public void MostrarOfertasCompra()
        {
            string Query1 = "exec [dbo].[ConsultaDatosOfertaCompra] 1, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.OfertaSobre1Compra.rdlc", "ComercializadoresSobre1");
        }

        public void MostrarResumenOfertasVenta()
        {
            string Query1 = "exec [dbo].[ResumenOfertasVenta] 1, Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.ResumenOfertaVenta.rdlc", "ResumenVenta");
        }


        public void MostrarResumenOfertasCompra()
        {
            string Query1 = "exec [dbo].[ResumenOfertasCompra] 1, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.ResumenOfertaCompra.rdlc", "ResumenCompra");
        }



        public void MostrarOfertasVenta()
        {
            string Query1 = "exec [dbo].[ConsultaDatosProyectos] Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.OfertasSobre1Proyectos.rdlc", "GeneradoresSobre1");
        }

        public void MostrarGrafico()
        {
            string Query1 = "exec [dbo].[ComprasvsGeneracion] 1,Subasta";
            MostrarOfertasTodas(Query1, 4, "UpmeSubasta2019.Reportes.ComprasvsGenera.rdlc", "GraficoComprasvsGeneracion");
        }
    }
}
