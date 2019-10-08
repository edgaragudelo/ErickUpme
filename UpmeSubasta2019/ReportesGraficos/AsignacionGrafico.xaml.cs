using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using UpmeSubasta2019.Data;

using Microsoft.Reporting.WinForms;
using UpmeSubasta2019.Reportes;


namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for AsignacionGrafico.xaml
    /// </summary>
    public partial class AsignacionGrafico : Window
    {
        public AsignacionGrafico()
        {
            InitializeComponent();

            // WindowState = WindowState.Maximized;

            // Centrar la ventana
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);

            // Reporte
            GenerarReporte();
        }

        private void GenerarReporte()
        {
            string ConsultaAsignacionesGraficosQuery = "exec [dbo].[ConsultaAsignacionesGraficos] Subasta";
            var dt = DAL.ExecuteQuery(ConsultaAsignacionesGraficosQuery);

            // var dt = DAL.ExecuteQuery("select * from asignaciones where proceso='Subasta' order by energia");

            AsignacionGraficoReport.Reset();
            ReportDataSource ds = new ReportDataSource("AsignacionGraficoDataSet", dt);
            AsignacionGraficoReport.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.ReportesGraficos.Reportes.AsignacionGrafico.rdlc";
            AsignacionGraficoReport.LocalReport.DataSources.Add(ds);
            AsignacionGraficoReport.RefreshReport();
            Exportar.ExportaPDF(AsignacionGraficoReport, "AsignacionGraficoReporte");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
