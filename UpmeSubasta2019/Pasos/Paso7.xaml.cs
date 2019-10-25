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
using System.Diagnostics;

namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for Paso3.xaml
    /// </summary>
    public partial class Paso7 : UserControl
    {
        public Paso7()
        {
            DataContext = new Paso7ViewModel();
            InitializeComponent();
            MostrarResumenSalidas();
            MostrarContratosASIC();
            MostrarLogs();
        }


        public void MostrarOfertasTodas(string Query1, int Proceso, string Reporte, string archivopdf)
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

            if (dt.Rows.Count != 0)
            {
                if (Proceso == 2)
                {
                    ReporteGeneradores.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    ////ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.ContratosASIC.rdlc";
                    ReporteGeneradores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteGeneradores.LocalReport.DataSources.Add(ds);
                    ReporteGeneradores.RefreshReport();
                    Exportar.ExportaPDF(ReporteGeneradores, archivopdf);
                }
                if (Proceso == 1)
                {
                    ReporteComercializadores.Reset();
                    ReportDataSource ds = new ReportDataSource("ResumenSalidas", dt);
                    //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.OfertasVenta.rdlc";
                    ReporteComercializadores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteComercializadores.LocalReport.DataSources.Add(ds);
                    ReporteComercializadores.RefreshReport();
                    Exportar.ExportaPDF(ReporteComercializadores, archivopdf);
                }
                if (Proceso == 3)
                {
                    ReporteLogs.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.OfertasVenta.rdlc";
                    ReporteLogs.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteLogs.LocalReport.DataSources.Add(ds);
                    ReporteLogs.RefreshReport();
                    Exportar.ExportaPDF(ReporteLogs, archivopdf);
                }
            }
            else
            {
                string Mensaje = "No existen datos de la consulta de datos resumen de las ofertas ..." + "\r\n";
                MessageBox.Show(Mensaje, "Error en la consulta de datos de las ofertas");

                //LogOfe = LogOfe + Mensaje;
                //DAL.InsertarLog(Mensaje, "Resumen de Ofertas Venta", "Resumen de Ofertas Venta");
            }

        }

        public void Backupbd(object sender, RoutedEventArgs ex)
        {

            ////Exportar(ReporteComercializadores);

            //Exportar.ExportaPDF(ReporteComercializadores, "Resumensalidas");
            //Exportar.ExportaPDF(ReporteGeneradores, "ContratosASIC");
            string Comandobackup = "c:\\Upme\\BAckup\\EjecutaBackup.bat";
            ProcessStartInfo startInfo = new ProcessStartInfo(Comandobackup)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process backup = new Process();
            backup.StartInfo = startInfo;
            backup.Start();



        }
        public void MostrarResumenSalidas()
        {
            string Query1 = "exec dbo.ResumenSalidas 'Mecanismo'";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.ResumenSalidas.rdlc", "ResumenSalidasMecanismo");

        }

        public void MostrarContratosASIC()
        {
            string Query1 = "exec dbo.ConsultaDatosContratosASIC 'Mecanismo'";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.ContratosASIC.rdlc", "ContratosASICMecanismo");

        }

        public void MostrarLogs()
        {
            string Query1 = "SELECT [Valor],[Tipo],[FechaProceso],[Proceso],[UsuarioMaquina] FROM [dbo].[LogProcesos] order by FechaProceso desc";
            MostrarOfertasTodas(Query1, 3, "UpmeSubasta2019.Reportes.LogProcesos.rdlc", "LogsEjecucionAplicativo");

        }
    }
}
