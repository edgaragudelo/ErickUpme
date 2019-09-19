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
using System.Windows.Shapes;
using System.Data;

namespace Subasta
{
    /// <summary>
    /// Lógica de interacción para Parametros.xaml
    /// </summary>
    public partial class frm_Parametros : Window
    {
        public frm_Parametros()
        {
            InitializeComponent();
            RecuperarParametros();
        }


        private void RecuperarParametros()
        {
            string tabla = "dbo.parametrosConfiguracion";
            string query = "SELECT idParametro,nombreParametro,valorParametro from ";
            query = query + tabla;
            DataTable Datosmostrar;
            ConexionBD cnx2 = new ConexionBD();
            Datosmostrar = cnx2.ObtenerTabla(query);

            if (Datosmostrar == null)
                ShowStatus("Error al cargar los datos de los parametros ", 1);
            else
            {
                dtg_Parametros.ItemsSource = Datosmostrar.DefaultView;
                ShowStatus("Datos de Parametros de Configuración", 1);

            }

        }  // fin metodo RecuperarOfertaCompra


        private void ShowStatus(string info, int tipo)
        {
            // tipo es la bandera para el tipo de dato, si es oferta compra o oferta venta
            if (tipo == 1)
                Status.Content = info;
          


        }


    }

   

}
