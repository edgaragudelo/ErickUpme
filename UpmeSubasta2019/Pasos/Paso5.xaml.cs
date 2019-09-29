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
using System.IO;
using UpmeSubasta2019.Reportes;



namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for Paso3.xaml
    /// </summary>
    public partial class Paso5 : UserControl
    {
        public Paso5()
        {
            DataContext = new Paso5ViewModel();
            InitializeComponent();
            MostrarResumenSalidas();
            MostrarContratosASIC();
        }


        public void MostrarOfertasTodas(string Query1, int Proceso, string Reporte)
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
                //Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message;
                //LogOfe = LogOfe + Mensaje;
                //DAL.InsertarLog(Mensaje, "Datos Ofertas Venta", "Datos Ofertas Venta");
                //throw;

            }

            if (dt.Rows.Count !=0)
            {
                if (Proceso == 2)
                {
                    ReporteGeneradores.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    ////ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.ContratosASIC.rdlc";
                    ReporteGeneradores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteGeneradores.LocalReport.DataSources.Add(ds);
                    ReporteGeneradores.RefreshReport();
                }
                else
                {
                    ReporteComercializadores.Reset();
                    ReportDataSource ds = new ReportDataSource("ResumenSalidas", dt);
                    //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.OfertasVenta.rdlc";
                    ReporteComercializadores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteComercializadores.LocalReport.DataSources.Add(ds);
                    ReporteComercializadores.RefreshReport();
                }
            }
            else
            {
                string Mensaje = "No existen datos de la consulta de datos resumen de las ofertas ...";
                MessageBox.Show(Mensaje, "Error en la consulta de datos de las ofertas");
                
                //LogOfe = LogOfe + Mensaje;
                //DAL.InsertarLog(Mensaje, "Resumen de Ofertas Venta", "Resumen de Ofertas Venta");
            }

        }

        public void ExportarPDF(object sender, RoutedEventArgs ex)
        {

            //Exportar(ReporteComercializadores);

            Exportar.ExportaPDF(ReporteComercializadores,"Resumensalidas");
            Exportar.ExportaPDF(ReporteGeneradores, "ContratosASIC");




        }
        public void MostrarResumenSalidas()
        {
            string Query1 = "exec dbo.ResumenSalidas 'Subasta'";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.ResumenSalidas.rdlc");

        }

        public void MostrarContratosASIC()
        {
            string Query1 = "exec dbo.ConsultaDatosContratosASIC";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.ContratosASIC.rdlc");

        }
    }
}
