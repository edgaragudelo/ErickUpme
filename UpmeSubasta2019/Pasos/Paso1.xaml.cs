using System;
using System.Windows;
using System.Windows.Controls;
using UpmeSubasta2019.Data;
using System.Data;
using System.Linq;

namespace UpmeSubasta2019
{
    /// <summary>
    /// Paso 1
    /// </summary>
    public partial class Paso1 : UserControl
    {
        public Paso1()
        {
            DataContext = new Paso1ViewModel();
            InitializeComponent();
            mensajeErrorLabel.Content = "";

            LimpiarTablaParametros();
            CargarParametros();
            MostrarDatos();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate window
            var asignacionGraficoModalWindow = new AsignacionGrafico();

            // Show window modally
            // NOTE: Returns only when window is closed
            Nullable<bool> dialogResult = asignacionGraficoModalWindow.ShowDialog();
        }

        public bool LimpiarTablaParametros()
        {
            string errorTipo = "Carga de Parámetros Upme";
            try
            {
                var consultasBorrar = DAL.ExecuteQuery("SELECT * FROM ConsultasBD WHERE Operacion = 'delete' AND Proceso = 'paso1' AND BD =  'sql'");
                var consultasBorrarTablasSql = Helper.ConvertDataTableToList<ConsultasBd>(consultasBorrar);
                foreach (var consultaBd in consultasBorrarTablasSql)
                {
                    DAL.ExecuteQueryNormal(consultaBd.Sql);
                }
                return true;
            }
            catch (Exception error)
            {
                var mensaje = string.Format("Error en borrado de los parametros... {0}\r\n", error.Message);
                LogError(mensaje, errorTipo, errorTipo, true);
                return false;
            }
        }

        private bool CargarParametros()
        {
            var nombreConsulta = string.Empty;
            var mensaje = string.Empty; string errorTipo = "Carga de Parámetros Upme";

            try
            {
                var dt = DAL.ExecuteQuery("SELECT * FROM ConsultasBD WHERE (Proceso = 'paso1')");
                var consultas = Helper.ConvertDataTableToList<ConsultasBd>(dt);
                var consultasPostgresql = consultas.First(c => c.Operacion == "query");

                nombreConsulta = consultasPostgresql.Nombre;
                var dtDatos = DAL.ExecuteQueryPostgres(consultasPostgresql.Sql);
                int numRegistrosLeidos = dtDatos.Rows.Count;
                if (numRegistrosLeidos > 0)
                {
                    var spCarga = consultas.First(c => c.Operacion == "grabar" && c.Tipo == consultasPostgresql.Tipo && c.Proceso == consultasPostgresql.Proceso);
                    int numRegistrosInsertados = DAL.ExecuteQueryParametro(spCarga.Sql, spCarga.Parametro, dtDatos);
                    if (numRegistrosLeidos != numRegistrosInsertados)
                    {
                        mensaje = string.Format("Numero de registros leidos de {0} no es igual a los insertados... \r\n Nombre de la consulta: {1}", consultasPostgresql.Tipo, nombreConsulta);
                        LogError(mensaje, consultasPostgresql.Tipo, consultasPostgresql.Proceso);
                        return false;
                    }
                }
                mensaje = string.Format("Se cargaron los datos de {0} de {1}\r\n", consultasPostgresql.Tipo, consultasPostgresql.Proceso);
                DAL.InsertarLog(mensaje, consultasPostgresql.Tipo, consultasPostgresql.Proceso);
                return true;
            }
            catch (Exception error)
            {
                mensaje = string.Format("Error en la lectura de los parametros... {0}\r\nNombre de la consulta: {1}", error.Message, nombreConsulta);
                LogError(mensaje, errorTipo, errorTipo, true);
                return false;
            }
        }

        private void MostrarDatos()
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

        private void LogError(string mensaje, string tipo, string proceso, bool messageBox = false)
        {
            DAL.InsertarLog(mensaje, tipo, proceso);
            if (messageBox) MessageBox.Show(mensaje, "Error");
        }
    }
}
