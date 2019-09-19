using System;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using UpmeSubasta2019.Data;

namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for GraficoOfertas.xaml
    /// </summary>
    public partial class GraficoOfertas : UserControl
    {
        public GraficoOfertas()
        {
            InitializeComponent();
            //string query = "SELECT nombre,ID_oferta,energiaMax,precio,ordenllegada FROM Subasta.dbo.ofertasCompra";

            //var table =  DAL.ExecuteQuery(query);


            //string[] labels = table.AsEnumerable().Select(r => r.Field<string>("nombre")).ToArray();

            //string[] valx = table.AsEnumerable().Select(r => r.Field<string>("precio")).ToArray();

            //decimal[] val = Array.ConvertAll(valx, s => decimal.Parse(s));

            //IChartValues values = new ChartValues<object>();
            //values.InsertRange(0, val);

            //// var results = table.AsEnumerable().Where(x => x.Field<int>("ordenllegada") == 1);

            SeriesCollection = new SeriesCollection
            {
                //new ColumnSeries
                new StepLineSeries
                {
                    Title = "Ofertas",
                    Values = new ChartValues<double> { 1,2,3,4,5},
                    //Values = values,
                    PointGeometry = null
                }
            };

            Labels = new[] { "Ofe01", "Ofe02", "Ofe03", "Ofe04", "Ofe05" };
            //Labels = labels;
            YFormatter = value => value.ToString("C");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
