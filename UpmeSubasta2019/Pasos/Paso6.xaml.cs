using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for Paso3.xaml
    /// </summary>
    public partial class Paso6 : UserControl
    {
        bool isExecuting;
        string Archivobat1;
        int oplProcessId;
        public static string LogOfe = null;
        public static string Mensaje = null;
        String Ejecucion = null;
        public Paso6()
        {
            DataContext = new Paso6ViewModel();
            InitializeComponent();
        }


        private void ExecutionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (isExecuting == true)
            {
                isExecuting = false;
                MessageBox.Show("INFO.ExecutionCompleted");
                MessageBox.Show(Ejecucion);
            }
        }


        public void EjecutarSubasta(object sender, RoutedEventArgs ex)
        {
            //Sub ejecutarSubasta()

            //Dim ruta  As String
            //Dim archivoOUT As String


            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += ExecuteOPL;
            //worker.RunWorkerCompleted += ExecutionCompleted;
            //worker.RunWorkerAsync();

            //ExecuteOPL();
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
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message + "\r\n";
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
                    Exportar.ExportaPDF(ReporteGeneradores, archivopdf);
                }
                else
                {
                    ReporteComercializadores.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.OfertasVenta.rdlc";
                    ReporteComercializadores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteComercializadores.LocalReport.DataSources.Add(ds);
                    ReporteComercializadores.RefreshReport();
                    Exportar.ExportaPDF(ReporteComercializadores, archivopdf);
                }
            }
            else
            {
                Mensaje = "No existen datos de la consulta de datos resumen de las ofertas ..." + "\r\n";
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Resumen de Ofertas Venta", "Resumen de Ofertas Venta");
            }

        }


        public void MostrarAsignacionesCompra()
        {
            string Query1 = "exec AsignacionesCompra Mecanismo";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.AsignacionCompra.rdlc", "AsignacionesCompra");
        }

        public void MostrarAsignacionesVenta()
        {
            string Query1 = "exec AsignacionesVenta Mecanismo";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.AsignacionVenta.rdlc", "AsignacionesVenta");
        }

        private void Opl_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Ejecucion += e.Data + Environment.NewLine;
                //EjecucionStatus.Text = Ejecucion.ToString();
                // EjecucionStatus.Text += e.Data + Environment.NewLine.ToString();
                //EjecucionStatus.Text = Ejecucion.ToString();
                //MessageBox.Show(Ejecucion);
            }
        }

        private void Opl_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                Ejecucion += e.Data + Environment.NewLine;
            //EjecucionStatus.Text += e.Data + Environment.NewLine.ToString();
            //MessageBox.Show(Ejecucion);

        }

        private void EjecucionStatus_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            EjecucionStatus.ScrollToEnd();
        }

        private void armarbat()
        {
            string ruta, archivoOUT, archivoBAT;

            Mensaje = "Configurando la ejecución del modelo ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Ejecución Modelo Matematico", "Configurando Rutas");
            //ruta = "C:\\Users\\eagud\\source\\repos\\Upme\\Subasta\\UpmeSubasta2019";
            ruta = "C:\\Upme\\subastaCLPE_5_";
            archivoBAT = ruta + "\\subastaCLPE.bat";                       //  '.bat del modelo de optimización
            //archivoOUT = ruta + "\\SubastaCLPE_salidas.xlsx";           //    'archivo de resultados

            //if (File.Exists(archivoBAT))
            //{
            //    File.Delete(archivoBAT);
            //}

            Mensaje = "Creando el archivo por lotes para la ejecución del modelo ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Ejecución Modelo Matematico", "Creando Archivo Rutas");

            //using (StreamWriter Filebat = File.AppendText(archivoBAT))         //se crea el archivo
            //{
            //    string Line1 = "CD " + ruta;
            //    //string Line1 = ruta;
            //    string Line2 = " python subastaCLPE.py";
            //    //string Line3 = "pause";
            //    Filebat.WriteLine(Line1);
            //    Filebat.WriteLine(Line2);
            //    // Filebat.WriteLine(Line3);
            //    Filebat.Close();
            //}

            Archivobat1 = archivoBAT;
            Mensaje = "Archivo por lotes creado para la ejecución del modelo ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Ejecución Modelo Matematico", "Archivo y Ruta Creada:" + archivoBAT);

        }

        private void MostrarGraficoAsignaciones()
        {
            {
                // Instantiate window
                var asignacionGraficoModalWindow = new AsignacionGrafico();


                // Show window modally
                // NOTE: Returns only when window is closed
                Nullable<bool> dialogResult = asignacionGraficoModalWindow.ShowDialog();
            }
        }


        // private void ExecuteOPL(object sender, DoWorkEventArgs e)
        private void ExecuteOPL(object sender, RoutedEventArgs ex)
        {
            isExecuting = true;
            armarbat();           

            Mensaje = "Creando hilo de proceso de ejecución del modelo ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Ejecución Modelo Matematico", "Creando proceso en S.O:");
            //ProcessStartInfo startInfo = new ProcessStartInfo(Archivobat1)
            //{
            //    CreateNoWindow = true,
            //    UseShellExecute = false,
            //    RedirectStandardOutput = true,
            //    RedirectStandardError = true
            //};

            Mensaje = "Ejecutando el modelo ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Ejecución Modelo Matematico", "Proceso en ejecución");

            //Process opl = new Process();
            //opl.StartInfo = startInfo;
            //opl.OutputDataReceived += new DataReceivedEventHandler(Opl_OutputDataReceived);
            //opl.ErrorDataReceived += new DataReceivedEventHandler(Opl_ErrorDataReceived);
            //opl.Start();
            //oplProcessId = opl.Id;
            //opl.BeginErrorReadLine();
            //opl.BeginOutputReadLine();
            //opl.WaitForExit();

            Mensaje = "Ejecutando el modelo ...";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Ejecución Modelo Matematico", "Proceso Terminado");

            System.Threading.Thread.Sleep(1000);
            if (!string.IsNullOrEmpty(Ejecucion))
                EjecucionStatus.Text = Ejecucion.ToString();

            Mensaje = "Ejecutando el modelo ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Ejecución Modelo Matematico", "Proceso Terminado");

            Mensaje = "Construyendo Reportes de Salida ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Reportes de Salida", "Reportes de Salida de Compra");
            MostrarAsignacionesCompra();
            Mensaje = "Construyendo Reportes de Salida ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Reportes de Salida", "Reportes de Salida de Venta");
            MostrarAsignacionesVenta();
            Mensaje = "Reportes de Salida ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Reportes de Salida", "Reportes de Salida Generados");
            MostrarGraficoAsignaciones();

            //}


        }

    }



}
