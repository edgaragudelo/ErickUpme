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
    /// Lógica de interacción para frm_OfertasCompra.xaml
    /// </summary>
    public partial class frm_OfertasCompra : Window
    {
        public frm_OfertasCompra()
        {
            InitializeComponent();
            RecuperarOfertaCompra();
            RecuperarOfertaVenta();



        } // fin metodo frm_OfertasCompra

        private void RecuperarOfertaCompra()
        {
            string tabla = "dbo.OfertasCompra";
            string query = "SELECT nombre,ID_oferta,energiaMax,precio from ";
            query = query + tabla;
            DataTable Datosmostrar;
            ConexionBD cnx2 = new ConexionBD();
            Datosmostrar = cnx2.ObtenerTabla(query);

            if (Datosmostrar == null)
                ShowStatus("Error al cargar los datos de Oferta",1);
            else
            {
                dtg_OfertasCompra.ItemsSource = Datosmostrar.DefaultView;
                ShowStatus("Datos de Oferta Compra",1);

            }

        }  // fin metodo RecuperarOfertaCompra


        private void RecuperarOfertaVenta()
        {
            string tabla = "dbo.ofertasVenta";
            string query = "SELECT nombre,ID_oferta,bloque,numPaquetesMax,numPaquetesMin,precio,simultanea,excluyente,dependiente from ";
            query = query + tabla;
            DataTable Datosmostrar;
            ConexionBD cnx2 = new ConexionBD();
            Datosmostrar = cnx2.ObtenerTabla(query);

            
    


            if (Datosmostrar == null)
                ShowStatus("Error al cargar los datos de Oferta",1);
            else
            {
                dtg_OfertasVenta.ItemsSource = Datosmostrar.DefaultView;
                ShowStatus("Datos de Oferta Venta",2);

            }

        }


        private void ShowStatus(string info,int tipo)
        {
            // tipo es la bandera para el tipo de dato, si es oferta compra o oferta venta
            if (tipo==1)
            Status.Content = info;
            else
                StatusVenta.Content = info;


        }
    }
}
