﻿using System;
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
using Microsoft.Reporting.WinForms;
using System.Data;
using UpmeSubasta2019.Data;
using System.Data.SqlClient;
using UpmeSubasta2019.Reportes;

namespace UpmeSubasta2019
{
    /// <summary>
    /// Interaction logic for Paso2.xaml
    /// </summary>
    public partial class Paso3 : UserControl
    {
        public static string LogOfe = null;
        public static string Mensaje = null;
        bool OfertasOk = true;
        public static int CerrarPaso3 = 0;
        public Paso3()
        {
            DataContext = new Paso3ViewModel();
            InitializeComponent();
        }


        public void MostrarOfertasTodas(string Query1, int Proceso, string Reporte, string archivopdf)
        {
            // Proceso 1: Compras 2:Ventas

            DataTable dt = null;
            //     string Query1 = "exec [dbo].[ConsultaDatosOfertaVenta]";
            try
            {
                dt = DAL.ExecuteQuery(Query1);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Error en la consulta de datos de las ofertas");
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message;
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Datos Ofertas Venta", "Datos Ofertas Venta");
                //throw;

            }

            if (dt.Rows.Count != 0)
            {
                if (Proceso == 2)
                {
                    ReporteGeneradores.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.OfertasVenta.rdlc";
                    ReporteGeneradores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteGeneradores.LocalReport.DataSources.Add(ds);
                    ReporteGeneradores.RefreshReport();
                    Exportar.ExportaPDF(ReporteGeneradores, archivopdf);
                }
                else
                {
                    ReporteComercializadores.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    //ReporteGeneradores.LocalReport.ReportEmbeddedResource = "UpmeSubasta2019.Reportes.OfertasVenta.rdlc";
                    ReporteComercializadores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteComercializadores.LocalReport.DataSources.Add(ds);
                    ReporteComercializadores.RefreshReport();
                    Exportar.ExportaPDF(ReporteComercializadores, archivopdf);
                }
            }
            else
            {
                Mensaje = "No existen datos de la consulta de datos resumen de las ofertas ...";
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Resumen de Ofertas Venta", "Resumen de Ofertas Venta");
            }

        }



        public void MostrarOfertasCompra()
        {
            string Query1 = "exec [dbo].[ConsultaDatosOfertaCompra] 2, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.OfertasCompra.rdlc", "OfertasCompra");
        }


        private void LogOfertasTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            LogOfertas.ScrollToEnd();
        }

        public void MostrarResumenOfertasVenta()
        {
            string Query1 = "exec [dbo].[ResumenOfertasVenta] 2, Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.ResumenOfertaVenta.rdlc", "ResumenVenta");           
        }


        public void MostrarResumenOfertasCompra()
        {
            string Query1 = "exec [dbo].[ResumenOfertasCompra] 2, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.ResumenOfertaCompra.rdlc", "ResumenCompra");
        }



        public void MostrarOfertasVenta()
        {
            string Query1 = "exec [dbo].[ConsultaDatosOfertaVenta] 2, Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.OfertasVenta.rdlc", "OfertaVenta");
        }

        public void CargaOfertas(object sender, RoutedEventArgs ex)
        {

            string QueryPostCompra = null;
            string QueryCargaCompra = null;
            string QueryPostVenta = null;
            string QueryPostProyectos = null;
            string QueryCargaVenta = null;
            string QueryCargaProyectos = null;
            string QueryBorrarCompra = null;
            string QueryBorrarVenta = null;
            string QueryBorrarProyectos = null;
            string QueryBorrarLog = null;



            DataTable dtcompra = null;
            DataTable dtventa = null;
            DataTable dtproyectos = null;

            bool Validar = ConsultarPasos();
            if (Validar)
            {
                if (CerrarPaso3 == 0)
                {

                    // Proceso de carga de ofertas de comercializadores -- OfertaCompra

                    QueryBorrarLog = "DELETE FROM [dbo].[LogProcesos] where proceso like '%Sobre 2%'";

                    //QueryPostCompra = "SELECT *,22.3 EnergiaMin FROM public.\"ofertasCompra\" where sobre=2 and \"Proceso\"='Subasta'";

                    QueryPostCompra = "SELECT Com.\"NombreCorto\" nombre,OfeCom.\"Codigo\" ID_oferta,OfeCom.\"CantidadMaxima\" energiaMax,COALESCE(OfeCom.\"PrecioOferta\", 0) precio, row_number() over (order by OfeCom.\"HoraRegistroPrecio\" desc) ordenllegada,2 Sobre,'Subasta' Proceso,23 energiaMin";
                    QueryPostCompra = QueryPostCompra + " FROM public.\"OfertasComercializador\" OfeCom, public.\"Comercializadores\" Com where OfeCom.\"Comercializador_id\" = Com.\"IdComercializador\"";


                    QueryCargaCompra = "dbo.GrabarOfertas";
                    QueryBorrarCompra = "DELETE FROM [dbo].[ofertasCompra] where sobre=2 and \"Proceso\"='Subasta'";

                    // Proceso de carga de ofertas de generadores -- OfertaVenta
                    //QueryPostVenta = "SELECT * FROM public.\"ofertasVenta\" where sobre=2 and \"Proceso\"='Subasta'";


                    QueryPostVenta = "SELECT OfePro.\"Codigo\" nombre, OfePro.\"IdOferta\" ID_oferta, Blo.\"Bloque\" bloque, OfePro.\"MaxPaquetes\" numPaquetesMax, OfePro.\"MinPaquetes\" numPaquetesMin,";
                    QueryPostVenta = QueryPostVenta + "COALESCE(OfePro.\"PrecioOferta\", 0) precio,(select (CASE WHEN OfePro.\"RestriccionOferta\" = 'SIM' THEN (select OfeP1.\"Codigo\" from public.\"OfertasProyectos\" OfeP1 where OfeP1.\"IdOferta\" = OfePro.\"OfertaRestriccion_id\" )";
                    QueryPostVenta = QueryPostVenta + "END)) as simultanea, (select (CASE WHEN OfePro.\"RestriccionOferta\" = 'EXC' THEN";
                    QueryPostVenta = QueryPostVenta + " (select OfeP1.\"Codigo\" from public.\"OfertasProyectos\" OfeP1 where OfeP1.\"IdOferta\" = OfePro.\"OfertaRestriccion_id\" ) END)) as excluyente, ";
                    QueryPostVenta = QueryPostVenta + "(select (CASE WHEN OfePro.\"RestriccionOferta\" = 'DEP' THEN";
                    QueryPostVenta = QueryPostVenta + "(select OfeP1.\"Codigo\" from public.\"OfertasProyectos\" OfeP1 where OfeP1.\"IdOferta\" = OfePro.\"OfertaRestriccion_id\" )";
                    QueryPostVenta = QueryPostVenta + "END)) as dependiente,row_number() over (order by OfePro.\"HoraRegistroPrecio\" desc) ordenllegada,2 Sobre,'Subasta' Proceso";
                    QueryPostVenta = QueryPostVenta + " FROM public.\"OfertasProyectos\" OfePro,public.\"Convocatoria_Bloques\" Blo";
                    QueryPostVenta = QueryPostVenta + " Where OfePro.\"Bloque_id\" = Blo.\"IdBloque\"";

                    QueryPostProyectos = "SELECT Pry.\"Codigo\" nombre,ReqT.\"CapacidadEfectivaTotal\" capacidadMaxima,Fue.\"Factor\" factorPlanta,null empresa,'Subasta' Proceso";
                    QueryPostProyectos = QueryPostProyectos + " FROM public.\"Proyectos\" Pry, public.\"RequisitosTecnicos\" ReqT, public.\"Fuentes_Energia\" Fue";
                    QueryPostProyectos = QueryPostProyectos + " where Pry.\"RequisitoTecnico_id\" = ReqT.\"IdRequisitoTecnico\" and Pry.\"Fuente_id\" = Fue.\"IdFuenteEnergia\";";

                    QueryCargaVenta = "dbo.GrabarOfertasVenta";
                    QueryBorrarVenta = "DELETE FROM [dbo].[ofertasVenta] where sobre=2 and \"Proceso\"='Subasta'";

                    QueryCargaProyectos = "dbo.GrabarProyectos";
                    QueryBorrarProyectos = "DELETE FROM [dbo].[proyectosGeneracion] where Proceso='Subasta'";

                    // Proceso de lectura de ofertas desde la bd fuente -- POSTGRES
                    try
                    {
                        int RegsaborradosLog = DAL.ExecuteQueryNormal(QueryBorrarLog);

                        Mensaje = "Inicia Proceso de Carga de Ofertas Sobre No 2...." + "\r\n";
                        LogOfe = Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");
                        int RegsaborradosCompra = DAL.ExecuteQueryNormal(QueryBorrarCompra);
                        int RegsaborradosVenta = DAL.ExecuteQueryNormal(QueryBorrarVenta);
                        int RegsaborradosProyectos = DAL.ExecuteQueryNormal(QueryBorrarProyectos);

                        Mensaje = "Conectando a la B.D de Ofertas de La UPME...." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");
                        dtcompra = DAL.ExecuteQueryPostgres(QueryPostCompra);

                        Mensaje = "Lectura de datos de Comercializadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");

                        Mensaje = "Lectura de datos de Generadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        dtventa = DAL.ExecuteQueryPostgres(QueryPostVenta);

                        Mensaje = "Lectura de datos de Proyectos de Generación de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        dtproyectos = DAL.ExecuteQueryPostgres(QueryPostProyectos);
                        DAL.InsertarLog(Mensaje, "Carga de Proyectos de Generación Sobre 2", "Carga de Proyectos de Generación Sobre 2");

                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");

                    }
                    catch (Exception ex1)
                    {
                        OfertasOk = false;
                        MessageBox.Show(ex1.Message, "Error en la lectura de las ofertas");
                        Mensaje = "Error en la lectura de las ofertas ..." + ex1.Message;
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Upme Sobre 2", "Carga de Ofertas Upme Sobre 2");
                        //throw;

                    }

                    if (dtcompra.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de Comercializadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");
                        return;
                    }

                    if (dtventa.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de Generadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");
                        return;
                    }

                    if (dtproyectos.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de Proyectos de Generación de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Proyectos de Generación Sobre 2", "Carga de Proyectos de Generación Sobre 2");
                        return;
                    }

                    try
                    {
                        Mensaje = "Cargando datos de ofertas de Comercializadores..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");
                        int Regscompra = DAL.ExecuteQueryParametro(QueryCargaCompra, "@OfertasCompra", dtcompra);
                        Mensaje = "Cargando datos de ofertas de Generadores..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;

                        DAL.InsertarLog(Mensaje, "Carga de Ofertas", "Carga de Ofertas");
                        int Regsventa = DAL.ExecuteQueryParametro(QueryCargaVenta, "@OfertasVenta", dtventa);
                      
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");

                        LogOfe = LogOfe + Mensaje;

                        DAL.InsertarLog(Mensaje, "Carga de Proyectos de Generación", "Carga de Ofertas");
                        int Regsproyectos = DAL.ExecuteQueryParametro(QueryCargaProyectos, "@Proyectos", dtproyectos);
                        Mensaje = "Carga de datos exitosa..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;


                        Mensaje = "Carga de datos exitosa..." + "\r\n";
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");

                    }
                    catch (Exception ex2)
                    {
                        OfertasOk = false;
                        MessageBox.Show(ex2.Message, "Error en el proceso de grabado de las ofertas");
                    }



                    // Proceso de lectura y carga de los datos

                    if (OfertasOk)
                    {
                        // Ejecutar el resumen de las ofertas de comercializadores y generadores
                        Mensaje = "Proceso de carga finalizado..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 2", "Carga de Ofertas Sobre 2");
                        LogOfertas.Text = LogOfe.ToString();
                        MostrarResumenOfertasCompra();
                        MostrarResumenOfertasVenta();
                        LogOfertas.Text = LogOfe.ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("El paso ya fue cerrado, los datos de ofertas de sobre 2 ya fueron cargados y validados", "Cierre de pasos");
                MostrarOfertasCompra();
                MostrarOfertasVenta();
            }
        }

        private void CerrarPaso(object sender, RoutedEventArgs e)
        {
            // Validar si el paso ya no fue cerrado
            bool Validar = ConsultarPasos();
            if (Validar)
            {
                if (OfertasOk)
                {
                    Mensaje = "Cierre paso 3 exitoso.";
                    LogOfe = LogOfe + Mensaje;
                    DAL.InsertarLog(Mensaje, "Cierre paso 3", "Cierre Pasos");
                    MessageBox.Show(Mensaje, "Cierre de pasos");
                    CerrarPaso3 = 1;
                }
                else
                {
                    Mensaje = "Cierre paso 3 fallido. Aún no ha sido realizado el proceso de carga de las ofertas del Sobre 3";
                    LogOfe = LogOfe + Mensaje;
                    DAL.InsertarLog(Mensaje, "Cierre paso 3", "Cierre Pasos");
                    MessageBox.Show(Mensaje, "Cierre de pasos");
                    CerrarPaso3 = 0;

                }

            }
            else
            {
                MessageBox.Show("El paso ya fue cerrado, los datos de ofertas de sobre 3 ya fueron cargados y validados", "Cierre de pasos");
            }
        }

        public static bool ConsultarPasos()
        {
            bool ValidarPaso = false;
            DataTable dt = null;
            string Query1 = "EXEC DBO.ConsultaDatosPasos 'Cierre Pasos', 'Cierre paso 3', 'Cierre paso 3 exitoso.'";
            try
            {
                dt = DAL.ExecuteQuery(Query1);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Error en la consulta de datos de los pasos de las ofertas");
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message;
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Datos Ofertas Venta Sobre 2", "Datos Ofertas Venta Sobre 2");
                //throw;

            }

            if (dt.Rows.Count == 0)
                ValidarPaso = true;
            else
                ValidarPaso = false;

            return ValidarPaso;

        }


        private void MostrarOfertas(object sender, RoutedEventArgs e)
        {
            MostrarOfertasCompra();
            MostrarOfertasVenta();
        }
    }
}

