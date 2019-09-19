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
    public partial class Paso4 : UserControl
    {
        bool isExecuting;
        string Archivobat1;
        int oplProcessId;
        String Ejecucion = null;
        public Paso4()
        {
            DataContext = new Paso4ViewModel();
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

            //ruta = ThisWorkbook.Path                              'Ruta del archivo de ejecucion
            //ruta = "D:\\Edgar\\ModeloSubasta";
            ruta = "C:\\Users\\eagud\\source\\repos\\Upme\\Subasta\\UpmeSubasta2019";
            archivoBAT = ruta + "\\subastaCLPE.bat";                       //  '.bat del modelo de optimización
            archivoOUT = ruta + "\\SubastaCLPE_salidas.xlsx";           //    'archivo de resultados

            if (File.Exists(archivoBAT))
            {
                File.Delete(archivoBAT);
            }

            using (StreamWriter Filebat = File.AppendText(archivoBAT))         //se crea el archivo
            {
                string Line1 = "CD " + ruta;
                //string Line1 = ruta;
                string Line2 = " python subastaCLPE.py";
                //string Line3 = "pause";
                Filebat.WriteLine(Line1);
                Filebat.WriteLine(Line2);
                // Filebat.WriteLine(Line3);
                Filebat.Close();
            }

            Archivobat1 = archivoBAT;

        }



        // private void ExecuteOPL(object sender, DoWorkEventArgs e)
        private void ExecuteOPL(object sender, RoutedEventArgs ex)
        {
            isExecuting = true;
            armarbat();
            //executionParametersViewModel.ExecutionStatus = "";
            //if (Existerutamodelo)
            //{
            // ProcessStartInfo startInfo = new ProcessStartInfo(Archivobat1 + "\\DHOG.bat")
            ProcessStartInfo startInfo = new ProcessStartInfo(Archivobat1)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            Process opl = new Process();
            opl.StartInfo = startInfo;
            opl.OutputDataReceived += new DataReceivedEventHandler(Opl_OutputDataReceived);
            opl.ErrorDataReceived += new DataReceivedEventHandler(Opl_ErrorDataReceived);


            opl.Start();

            oplProcessId = opl.Id;

            opl.BeginErrorReadLine();

            opl.BeginOutputReadLine();


            opl.WaitForExit();

            System.Threading.Thread.Sleep(1000);
            if (!string.IsNullOrEmpty(Ejecucion))
                EjecucionStatus.Text = Ejecucion.ToString();

            //}
        }



    }



}
