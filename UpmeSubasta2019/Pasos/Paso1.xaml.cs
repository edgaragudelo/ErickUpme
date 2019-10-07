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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate window
            var asignacionGraficoModalWindow = new AsignacionGrafico();

            // Show window modally
            // NOTE: Returns only when window is closed
            Nullable<bool> dialogResult = asignacionGraficoModalWindow.ShowDialog();
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
            if (!double.TryParse(precioTopeTextBox.Text, out precioTope))
            {
                msg += "Error: El valor en el campo Precio Tope debe ser numérico." + System.Environment.NewLine;
            }
            if (!double.TryParse(PrecioMaximoVentaTextBox.Text, out precioMaximo))
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
