using System;
using System.Data;
using System.Data.SqlClient;
using NpgsqlTypes;
using Npgsql;
using System.Windows;
using System.Configuration;
using System.Net;
using System.Net.Sockets;

namespace UpmeSubasta2019.Data
{
    public class DAL
    {
        public static SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["UpmeSubasta2019.Properties.Settings.SubastaConnectionString"].ConnectionString);
        public static string myConnString1 = "Server=localhost;Port=5432;User id=postgres;Password=Mariajose;Database = Upme;";
        public static NpgsqlConnection posConn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["UpmeSubasta2019.Properties.Settings.SubastaUpmePostGresConnectionString"].ConnectionString);
    


        public static DataTable ExecuteQuery(string query)
        {
            var dataset = new DataSet();

            //var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Subasta;Integrated Security=True";
            //var connectionString = @"Data Source=SRVTESTSUBASTA\SQLEXPRESS;Initial Catalog=Subasta;Integrated Security=True";

            //using (var sqlConn = new SqlConnection(connectionString))
            //{
                using (var sqlCommand = new SqlCommand(query, sqlConn))
                {
                    var da = new SqlDataAdapter(sqlCommand);
                    da.Fill(dataset);
                }
            //}

            return dataset.Tables[0];
        }


        public static int ExecuteQueryNormal(string query)
        {
            int da = 0;

           // var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Subasta;Integrated Security=True";
            //var connectionString = @"Data Source=SRVTESTSUBASTA\SQLEXPRESS;Initial Catalog=Subasta;Integrated Security=True";

            //using (var sqlConn = new SqlConnection(connectionString))
            //using (var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["UpmeSubasta2019.Properties.Settings.SubastaConnectionString"].ConnectionString))
            //{
                using (var sqlCommand = new SqlCommand(query, sqlConn))
                {
                    sqlConn.Open();
                    da = sqlCommand.ExecuteNonQuery();
                    sqlConn.Close();
                }
            //}

            return da;
        }


        public static string LocalIPAddress()
        {
            IPHostEntry host; string localIP = ""; host = 
            Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                { localIP = ip.ToString();
                    break;
                }
            } return localIP;
        }

   

        public static void InsertarLog(string Valor, string Tipo, string Proceso)
        {
            string QueryInsert = null;
            String Fecha;
            Fecha = DateTime.Now.ToString(@"yyyy-MM-dd hh:mm");
            string Maquina = Environment.MachineName;
            String User = Environment.UserName;
            String DirIp = LocalIPAddress();

            Maquina = "Maquina:" + Maquina + " Usuario:" + User + " Dir IP:" + DirIp;

            QueryInsert = "INSERT INTO[dbo].[LogProcesos] ([Valor],[Tipo],[FechaProceso],[Proceso],[UsuarioMaquina]) VALUES ('" + Valor + "','" + Tipo + "','" + Fecha + "','" + Proceso + "','"+Maquina+"')";
            int RegsLogs = DAL.ExecuteQueryNormal(QueryInsert);
            if (RegsLogs == -1)
            {
                MessageBox.Show("Error Insertando registro en el log del proceso", "Error Log");
            }

        }


        public static int ExecuteQueryParametro(string query, string vble, DataTable Registros)
        {
            var dataset = new DataSet();
            int da = 0;
            //var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Subasta;Integrated Security=True";
            //var connectionString = @"Data Source=SRVTESTSUBASTA\SQLEXPRESS;Initial Catalog=Subasta;Integrated Security=True";
            //using (var sqlConn = new SqlConnection(connectionString))
            //{
                using (var sqlCommand = new SqlCommand(query, sqlConn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = vble;
                    param.Value = Registros;
                    sqlCommand.Parameters.Add(param);
                    sqlConn.Open();
                    //try
                    //{
                      da = sqlCommand.ExecuteNonQuery(); //  new SqlDataAdapter(sqlCommand);
                    //}
                    //catch (Exception Ex2)
                    //{
                    //String Mensaje = "Error en la ejecucion del proceso de guardado en SQl Server..." + "\r\n" + Ex2.Message + "\r\n";
                    ////LogOfe = Mensaje;
                    //InsertarLog(Mensaje, "Carga de Ofertas UPME", "Carga de Ofertas");
                    //MessageBox.Show(Ex2.Message, "Error Conexion BD");
                    //sqlConn.Close();
                }
                
                    //da.Fill(dataset);
                    sqlConn.Close();
                //}
            //}

            return da;
        }


        public static DataTable ExecuteQueryPostgres(string query)
        {
            var dataset = new DataSet();
            //string myConnString1 = "Server=localhost;Port=5432;User id=postgres;Password=Mariajose;Database = Upme;";   
            //using (var sqlConn = new NpgsqlConnection(myConnString1))
            //{
                using (var sqlCommand = new NpgsqlCommand(query, posConn))
                {
                    var da = new NpgsqlDataAdapter(sqlCommand);
                    try
                    {
                        da.Fill(dataset);
                    }
                    catch (Exception Ex2)
                    {
                        String Mensaje = "Error en la conexion a Postgres..." + "\r\n" + Ex2.Message + "\r\n";
                    //LogOfe = Mensaje;
                    InsertarLog(Mensaje, "Carga de Ofertas UPME", "Carga de Ofertas");
                        MessageBox.Show(Ex2.Message, "Error Conexion BD");                      
                    }

                //}
            }

            return dataset.Tables[0];
        }



    }
}