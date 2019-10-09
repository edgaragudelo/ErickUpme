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
using UpmeSubasta2019.Data;
using System.Data;
namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for Paso1.xaml
    /// </summary>
    /// //Hola 3
    /// //Hola 2
    public partial class Paso1 : UserControl
    {
        public Paso1()
        {
            DataContext = new Paso1ViewModel();
            InitializeComponent();
            mensajeErrorLabel.Content = "";
            ConsultarDatos();
            //Leer de la Base de datos

            // Asignar valor a cada textbox
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate window
            var asignacionGraficoModalWindow = new AsignacionGrafico();

            // Show window modally
            // NOTE: Returns only when window is closed
            Nullable<bool> dialogResult = asignacionGraficoModalWindow.ShowDialog();
        }

        private void ConsultarDatos()
        {
            string QueryParametros = null;
            string Mensaje = null;
            DataTable dt = null;
            QueryParametros = "SELECT [IdParametroSubasta],[DemandaObjetivo],[TopeMaximoPromedio],[TopeMaximoIndividual],[TamanoPaquete] FROM[dbo].[ParametrosSubasta]";

            try
            {
                dt = DAL.ExecuteQuery(QueryParametros);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Error en la consulta de datos de Parametros");
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message + "\r\n";
                //LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Datos Ofertas Venta", "Datos Ofertas Venta");
                //throw;

            }


            foreach (DataRow row in dt.Rows)
            {
                tamanoPaqueteTextBox.Text = row[4].ToString();
                TopeMaximoPromedioTextBox.Text = row[2].ToString();
                TopeMaximoIndividualTextBox.Text = row[3].ToString();
                demandaObjetivoTextBox.Text = row[1].ToString();

            }


        }

        private void GuardarParametros(object sender, RoutedEventArgs e)
        {
            double tamanoPaquete;
            double precioTope;
            double precioMaximo;
            double demandaObjetivo;
            string msg = string.Empty;
            

                if (!double.TryParse(tamanoPaqueteTextBox.Text, out tamanoPaquete))
            {
                msg += "Error: El valor en el campo Tamaño Paquete debe ser numérico." + System.Environment.NewLine;
            }
            if (!double.TryParse(TopeMaximoPromedioTextBox.Text, out precioTope))
            {
                msg += "Error: El valor en el campo Precio Tope debe ser numérico." + System.Environment.NewLine;
            }
            if (!double.TryParse(TopeMaximoIndividualTextBox.Text, out precioMaximo))
            {
                msg += "Error: El valor en el campo Precio Máximo de venta debe ser numérico." + System.Environment.NewLine;
            }
            if (!double.TryParse(demandaObjetivoTextBox.Text, out demandaObjetivo))
            {
                msg += "Error: El valor en el campoDemanda Objetivo debe ser numérico." + System.Environment.NewLine;
            }

            mensajeErrorLabel.Content = msg;
        }
    }
}
