using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Reporting.WinForms;
using System.Data;
using UpmeSubasta2019.Data;
using System.Data.SqlClient;
using UpmeSubasta2019.Reportes;

namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for Paso2.xaml
    /// </summary>
    public partial class Paso2 : UserControl
    {
        public static string LogOfe = null;
        public static string Mensaje = null;
        bool OfertasOk = true;
        public static int CerrarPaso2 = 0;
        public Paso2()
        {
            DataContext = new Paso2ViewModel();
            InitializeComponent();
        }


        public void MostrarOfertasTodas(string Query1,int Proceso,string Reporte,string archivopdf)
        {
            // Proceso 1: Compras 2:Ventas

            DataTable dt = null;
       //     string Query1 = "exec [dbo].[ConsultaDatosOfertaVenta]";
            try
            {
                dt = DAL.ExecuteQuery(Query1);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Error en la consulta de datos de las ofertas");
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message;
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Datos Ofertas Venta", "Datos Ofertas Venta");
                //throw;

            }
                 
            if (dt.Rows.Count != 0)
            {
                if (Proceso == 2)
                {
                    ReporteGeneradores.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.OfertasVenta.rdlc";
                    ReporteGeneradores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteGeneradores.LocalReport.DataSources.Add(ds);
                    ReporteGeneradores.RefreshReport();
                    Exportar.ExportaPDF(ReporteGeneradores,archivopdf);
                }
                else
                {
                    ReporteComercializadores.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.OfertasVenta.rdlc";
                    ReporteComercializadores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteComercializadores.LocalReport.DataSources.Add(ds);
                    ReporteComercializadores.RefreshReport();
                    Exportar.ExportaPDF(ReporteComercializadores,archivopdf);
                }
            }
            else
            {
                Mensaje = "No existen datos de la consulta de datos resumen de las ofertas ...";
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Resumen de Ofertas Venta", "Resumen de Ofertas Venta");
            }

        }



        public void MostrarOfertasCompra()
        {
            string Query1 = "exec [dbo].[ConsultaDatosOfertaCompra] 1, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.OfertasCompra.rdlc","OfertasCompra");

            //DataTable dt = DAL.ExecuteQuery(Query1);
            //ReporteComercializadores.Reset();
            //ReportDataSource ds = new ReportDataSource("DataSet1", dt);
            //ReporteComercializadores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.Report1.rdlc";
            //ReporteComercializadores.LocalReport.DataSources.Add(ds);
            //ReporteComercializadores.RefreshReport();
        }


        private void LogOfertasTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            LogOfertas.ScrollToEnd();
        }

        public void MostrarResumenOfertasVenta()
        {
            string Query1 = "exec [dbo].[ResumenOfertasVenta] 1, Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.ResumenOfertaVenta.rdlc","ResumenVenta");

            //DataTable dt = DAL.ExecuteQuery(Query1);           

            //LogOfertas.Text = LogOfe.ToString();
            //ReporteGeneradores.Reset();
            //ReportDataSource ds = new ReportDataSource("DataSet1", dt);
            //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.ResumenOfertaVenta.rdlc";
            //ReporteGeneradores.LocalReport.DataSources.Add(ds);
            //ReporteGeneradores.RefreshReport();
        }


        public void MostrarResumenOfertasCompra()
        {
            string Query1 = "exec [dbo].[ResumenOfertasCompra] 1, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.ResumenOfertaCompra.rdlc","ResumenCompra");

            //DataTable dt = DAL.ExecuteQuery(Query1);
            //ReporteComercializadores.Reset();
            //ReportDataSource ds = new ReportDataSource("DataSet1", dt);
            //ReporteComercializadores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.ResumenOfertaCompra.rdlc";
            //ReporteComercializadores.LocalReport.DataSources.Add(ds);
            //ReporteComercializadores.RefreshReport();
        }

        

        public void MostrarOfertasVenta()
        {           
            string Query1 = "exec [dbo].[ConsultaDatosOfertaVenta] 1, Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.OfertasVenta.rdlc","OfertaVenta");           
        }

        public void CargaOfertas(object sender, RoutedEventArgs ex)
        {

            string QueryPostCompra = null;
            string QueryCargaCompra = null;
            string QueryPostVenta = null;
            string QueryCargaVenta = null;
            string QueryBorrarCompra = null;
            string QueryBorrarVenta = null;
            string QueryBorrarLog = null;


           
            DataTable dtcompra = null;
            DataTable dtventa = null;

            bool Validar = ConsultarPasos();
            if (Validar)
            {
            
                if (CerrarPaso2 == 0)
                {

                    // Proceso de carga de ofertas de comercializadores -- OfertaCompra

                    QueryBorrarLog = "DELETE FROM [dbo].[LogProcesos]";

                    QueryPostCompra = "SELECT * FROM public.\"ofertasCompra\" where sobre=1 and \"Proceso\"='Subasta'"; 

                    // QueryPostCompra = "SELECT \"Comercializador_id\",\"IdOferta\",\"CantMax\",\"PrecioOferta\", 0\"orden llegada" +
                    // "FROM public.\"OfertasComercializador\" Of,  public.\"Comercializadores\" CO" +
                    // "where Of.\"Comercializador_id\" = CO.\"IdComercializador";

                    QueryCargaCompra = "dbo.GrabarOfertas";
                    QueryBorrarCompra = "DELETE FROM [dbo].[ofertasCompra] where sobre=1 and \"Proceso\"='Subasta'";

                    // Proceso de carga de ofertas de generadores -- OfertaVenta
                    QueryPostVenta = "SELECT * FROM public.\"ofertasVenta\" where sobre=1 and \"Proceso\"='Subasta'";


                    //QueryPostVenta = "SELECT Po.\"Codigo\",\"IdOferta\",\"Bloque\",\"MaxPaquetes\",\"MinPaquetes\",\"PrecioOferta\",\'dato' simultanea, 'dato1' excluyente, 'dato2' dependiente, 0 ordenllegada";
                    //QueryPostVenta = QueryPostVenta + "FROM public.\"OfertasProyectos\" Op,public.\"Convocatoria_Bloques\" Bo ,public.\"Proyectos\" Po ";
                    //QueryPostVenta = QueryPostVenta + "where Op.\"Bloque_id\" = Bo.\"IdBloque\" and Po.\"IdProyecto\" = Op.\"IdProyecto";

                    QueryCargaVenta = "dbo.GrabarOfertasVenta";
                    QueryBorrarVenta = "DELETE FROM [dbo].[ofertasVenta] where sobre=1 and proceso='Subasta'"; 

                    // Proceso de lectura de ofertas desde la bd fuente -- POSTGRES
                    try
                    {
                        int RegsaborradosLog = DAL.ExecuteQueryNormal(QueryBorrarLog);

                        Mensaje = "Inicia Proceso de Carga de Ofertas Sobre No 1...." + "\r\n";
                        LogOfe = Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        int RegsaborradosCompra = DAL.ExecuteQueryNormal(QueryBorrarCompra);
                        int RegsaborradosVenta = DAL.ExecuteQueryNormal(QueryBorrarVenta);

                        Mensaje = "Conectando a la B.D de Ofertas de La UPME...." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        dtcompra = DAL.ExecuteQueryPostgres(QueryPostCompra);

                        Mensaje = "Lectura de datos de Comercializadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");

                        Mensaje = "Lectura de datos de Generadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        dtventa = DAL.ExecuteQueryPostgres(QueryPostVenta);
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");

                    }
                    catch (Exception ex1)
                    {
                        OfertasOk = false;
                        MessageBox.Show(ex1.Message, "Error en la lectura de las ofertas");
                        Mensaje = "Error en la lectura de las ofertas ..." + ex1.Message;
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Upme Sobre 1", "Carga de Ofertas Upme Sobre 1");
                        //throw;

                    }

                    if (dtcompra.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de Comercializadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        return;
                    }

                    if (dtventa.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de Generadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        return;
                    }

                    try
                    {
                        Mensaje = "Cargando datos de ofertas de Comercializadores..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        int Regscompra = DAL.ExecuteQueryParametro(QueryCargaCompra, "@OfertasCompra", dtcompra);
                        Mensaje = "Cargando datos de ofertas de Generadores..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;

                        DAL.InsertarLog(Mensaje, "Carga de Ofertas", "Carga de Ofertas");
                        int Regsventa = DAL.ExecuteQueryParametro(QueryCargaVenta, "@OfertasVenta", dtventa);
                        Mensaje = "Carga de datos exitosa..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                    }
                    catch (Exception ex2)
                    {
                        OfertasOk = false;
                        MessageBox.Show(ex2.Message, "Error en el proceso de grabado de las ofertas");
                    }



                    // Proceso de lectura y carga de los datos

                    if (OfertasOk)
                    {
                        // Ejecutar el resumen de las ofertas de comercializadores y generadores
                        Mensaje = "Proceso de carga finalizado..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        LogOfertas.Text = LogOfe.ToString();
                        MostrarResumenOfertasCompra();
                        MostrarResumenOfertasVenta();
                        LogOfertas.Text = LogOfe.ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("El paso ya fue cerrado, los datos de ofertas de sobre 1 ya fueron cargados y validados","Cierre de pasos");
                MostrarOfertasCompra();
                MostrarOfertasVenta();
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
                    Mensaje = "Cierre paso 2 exitoso.";
                    LogOfe = LogOfe + Mensaje;
                    DAL.InsertarLog(Mensaje, "Cierre paso 2", "Cierre Pasos");
                    MessageBox.Show(Mensaje, "Cierre de pasos");
                    CerrarPaso2 = 1;
                }
                else
                {
                    Mensaje = "Cierre paso 2 fallido. Aún no ha sido realizado el proceso de carga de las ofertas del Sobre 1";
                    LogOfe = LogOfe + Mensaje;
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

        public static bool ConsultarPasos()
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
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message;
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Datos Ofertas Venta", "Datos Ofertas Venta");
                //throw;

            }

            if (dt.Rows.Count ==0)
                ValidarPaso = true;
            else
                ValidarPaso = false;

            return ValidarPaso;

        }


        private void MostrarOfertas(object sender, RoutedEventArgs e)
        {
            MostrarOfertasCompra();
            MostrarOfertasVenta();
        }
    }
}
