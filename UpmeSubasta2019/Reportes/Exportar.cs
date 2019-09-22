using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UpmeSubasta2019.Reportes
{
    public class Exportar
    {
        public static void ExportaPDF(ReportViewer repor,string nombrearchivo)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            ReportViewer reportViewer = new ReportViewer();
//            reportViewer

//ReporteComercializadores.Reset();
//            ReportDataSource ds = new ReportDataSource("ResumenSalidas", dt);
//            //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.OfertasVenta.rdlc";
//            ReporteComercializadores.LocalReport.ReportEmbeddedResource = Reporte;
//            ReporteComercializadores.LocalReport.DataSources.Add(ds);
//            ReporteComercializadores.RefreshReport();


            //ReportViewer1.LocalReport.Render(
            byte[] bytes =  repor.LocalReport.Render(

               "PDF", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            FileStream fs = new FileStream(@"d:\" + nombrearchivo+".pdf",
             FileMode.Create);
            // FileMode.Create)
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();


            byte[] bytes1 = repor.LocalReport.Render(

              "Excel", null, out mimeType, out encoding,
               out extension,
              out streamids, out warnings);

            FileStream fs1 = new FileStream(@"d:\" + nombrearchivo + ".xls",
             FileMode.Create);
            // FileMode.Create)
            fs1.Write(bytes1, 0, bytes1.Length);
            fs1.Close();


      

            //MessageBox.Show("Report exported to:);


        }

    }
}
