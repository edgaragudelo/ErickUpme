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
            //WindowState = WindowState.Maximized;
            InitializeComponent();

            GenerarReporte();
        }

        private void GenerarReporte()
        {
            var dt = DAL.ExecuteQuery("select * from asignaciones order by energia");

            AsignacionGraficoReport.Reset();
            ReportDataSource ds = new ReportDataSource("AsignacionGraficoDataSet", dt);
            AsignacionGraficoReport.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.ReportesGraficos.Reportes.AsignacionGrafico.rdlc";
            AsignacionGraficoReport.LocalReport.DataSources.Add(ds);
            AsignacionGraficoReport.RefreshReport();
            Exportar.ExportaPDF(AsignacionGraficoReport, "AsignacionGraficoReporte");
        }
    }
}
