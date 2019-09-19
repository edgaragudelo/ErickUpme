using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace Subasta
{
    class ConexionBD
    {
        SqlConnection cnx;

        //Metodos

        //Metodo conectar()

        // Metodo desconectar()

        // Metodo ObtenerTabla()


        public bool conectar()
        {
            try
            {
                SqlConnectionStringBuilder cnxstring = new SqlConnectionStringBuilder();
                cnx = new SqlConnection();
                cnxstring.IntegratedSecurity = true;
                cnxstring.PersistSecurityInfo = false;
                cnxstring.DataSource = "DESKTOP-BEOBC7Q\\SQLEXPRESS";
                cnxstring.InitialCatalog = "Subasta";

                cnx.ConnectionString = cnxstring.ConnectionString.ToString();
                cnx.Open();
                return true;


            }
            catch(SqlException sqlexepcion)
            {
                return false;
            }

        }  // Fin conectar


        public void desconectar()
        {
            cnx.Close();

        }

        public DataTable ObtenerTabla(String sqlsentencia)
        {
            DataTable Datos = new DataTable();
            if (conectar())
            {
                SqlCommand sqlcom = new SqlCommand(sqlsentencia, cnx);
                SqlDataAdapter DataAdap = new SqlDataAdapter(sqlcom);
                DataAdap.Fill(Datos);
                desconectar();
            }
            return Datos;
        } // fin ObtenerTabla


        public int comando(string sqlsentencia)
        {
            int resultado = -1;

            try
            {
                SqlCommand comando = new SqlCommand(sqlsentencia, cnx);
                comando.ExecuteNonQuery();
                resultado = 0;
                desconectar();
            }
            catch (SqlException sqlexepcion)
            {
                resultado = sqlexepcion.Number;
            }
            return resultado;
        } // fin comando

        public void MostrarDatos(string tabla,int pantalla)
        {
           
            frm_OfertasCompra Ofertas = new frm_OfertasCompra();
            frm_Parametros Parame = new frm_Parametros();
            frm_ProyectosGeneracion Proyectos = new frm_ProyectosGeneracion();

            if (pantalla ==1) Ofertas.ShowDialog();
            if (pantalla==2) Parame.ShowDialog();
            if (pantalla == 3) Proyectos.ShowDialog();
        }


        public void Ejecutar(string tabla, int pantalla)
        {


            string ruta, archivoOUT;

                //ruta = ThisWorkbook.Path                              'Ruta del archivo de ejecucion
                //archivoBAT = ruta & "\subastaCLPE.bat"                         '.bat del modelo de optimización
                //archivoOUT = ruta & "\SubastaCLPE_salidas.xlsx"                 'archivo de resultados
                //Dim NombreDeArchivo() As String

                //Application.DisplayAlerts = False

                //ActiveWorkbook.Save

                //If IsFileOpen(archivoOUT) Then
                //    Workbooks("SubastaCLPE_salidas.xlsx").Close
                //End If

                //'Crear archivo .bat
                //If Dir(archivoBAT) <> "" Then Kill(archivoBAT)
                //Open archivoBAT For Output As #1

                //Print #1, "CD /D " & ruta
                //Print #1, " python subastaCLPE.py"
                //Print #1, "pause"

                //Close #1

                //'Ejecutar Modelo DHOG
                //Dim wsh As Object
                //Set wsh = VBA.CreateObject("WScript.Shell")
                //Dim waitOnReturn As Boolean: waitOnReturn = True
                //Dim windowStyle As Integer: windowStyle = 3
                //wsh.Run archivoBAT, windowStyle, waitOnReturn

                //Workbooks.Open filename:= archivoOUT

                //Call formatoSalida

                //Call graficar

                //End Sub
        }

    }
}
