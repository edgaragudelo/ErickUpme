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


namespace Subasta
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_cnx_Click(object sender, RoutedEventArgs e)
        {
            ConexionBD cnx1 = new ConexionBD();
            if (cnx1.conectar())
            {
                MessageBox.Show("Conexion OK");
            }
            else
            {
                MessageBox.Show("Fallo Conexion a bd");

            }
        }

        private void Btn_MostrarOferta_Click(object sender, RoutedEventArgs e)
        {
            ConexionBD cnx1 = new ConexionBD();
            if (cnx1.conectar())
            {
                cnx1.MostrarDatos("dbo.agentesMercado",1);
            }
        }

        private void Btn_MostraParametros_Click(object sender, RoutedEventArgs e)
        {
            ConexionBD cnx1 = new ConexionBD();
            if (cnx1.conectar())
            {
                cnx1.MostrarDatos("dbo.parametrosConfiguracion",2);
            }
        }

        private void Btn_MostrarProyectos_Click(object sender, RoutedEventArgs e)
        {
            ConexionBD cnx1 = new ConexionBD();
            if (cnx1.conectar())
            {
                cnx1.MostrarDatos("dbo.proyectosGeneracion", 3);
            }
        }

        private void Btn_Ejecutar_Click(object sender, RoutedEventArgs e)
        {
            ConexionBD cnx1 = new ConexionBD();
            if (cnx1.conectar())
            {
                cnx1.MostrarDatos("dbo.proyectosGeneracion", 3);
            }
        }


    }
}
