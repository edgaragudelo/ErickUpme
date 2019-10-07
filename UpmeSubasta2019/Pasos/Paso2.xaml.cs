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
    public partial class Paso2 : UserControl
    {
        public static string LogOfe = null;
        public static string Mensaje = null;
        bool OfertasOk = true;
        public static int CerrarPaso2 = 0;
        public Paso2()
        {
            DataContext = new Paso2ViewModel();
            InitializeComponent();
        }


        public void MostrarOfertasTodas(string Query1,int Proceso,string Reporte,string archivopdf)
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
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message + "\r\n"; 
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
                    ReporteGeneradores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteGeneradores.LocalReport.DataSources.Add(ds);
                    ReporteGeneradores.RefreshReport();
                    Exportar.ExportaPDF(ReporteGeneradores,archivopdf);
                }
                if (Proceso == 1)
                {
                    ReporteComercializadores.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    ReporteComercializadores.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteComercializadores.LocalReport.DataSources.Add(ds);
                    ReporteComercializadores.RefreshReport();
                    Exportar.ExportaPDF(ReporteComercializadores,archivopdf);
                }
                if (Proceso == 3)
                {
                    ReporteCriterios.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    ReporteCriterios.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteCriterios.LocalReport.DataSources.Add(ds);
                    ReporteCriterios.RefreshReport();
                    Exportar.ExportaPDF(ReporteCriterios, archivopdf);
                    
                }
                if (Proceso == 4)
                {
                    ReporteCompravsGene.Reset();
                    ReportDataSource ds = new ReportDataSource("DataSet1", dt);
                    ReporteCompravsGene.LocalReport.ReportEmbeddedResource = Reporte;
                    ReporteCompravsGene.LocalReport.DataSources.Add(ds);
                    ReporteCompravsGene.RefreshReport();
                    Exportar.ExportaPDF(ReporteCompravsGene, archivopdf);
                }

            }
            else
            {
                Mensaje = "No existen datos de la consulta de datos resumen de las ofertas ..." + "\r\n"; 
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Resumen de Ofertas Venta", "Resumen de Ofertas Venta");
            }

        }



        public void MostrarOfertasCompra()
        {
            string Query1 = "exec [dbo].[ConsultaDatosOfertaCompra] 1, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.OfertaSobre1Compra.rdlc", "ComercializadoresSobre1");
        }


        private void LogOfertasTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            LogOfertas.ScrollToEnd();
        }

        public void MostrarResumenOfertasVenta()
        {
            string Query1 = "exec [dbo].[ResumenOfertasVenta] 1, Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.ResumenOfertaVenta.rdlc","ResumenVenta");
        }


        public void MostrarResumenOfertasCompra()
        {
            string Query1 = "exec [dbo].[ResumenOfertasCompra] 1, Subasta";
            MostrarOfertasTodas(Query1, 1, "UpmeSubasta2019.Reportes.ResumenOfertaCompra.rdlc","ResumenCompra");
        }

        

        public void MostrarOfertasVenta()
        {           
            string Query1 = "exec [dbo].[ConsultaDatosProyectos] Subasta";
            MostrarOfertasTodas(Query1, 2, "UpmeSubasta2019.Reportes.OfertasSobre1Proyectos.rdlc", "GeneradoresSobre1");           
        }

        public void MostrarGrafico()
        {
            string Query1 = "exec [dbo].[ComprasvsGeneracion] 1,Subasta";
            MostrarOfertasTodas(Query1, 4, "UpmeSubasta2019.Reportes.ComprasvsGenera.rdlc", "GraficoComprasvsGeneracion");
        }


        public void CargaOfertas(object sender, RoutedEventArgs ex)
        {

            string QueryPostCompra = null;
            string QueryCargaCompra = null;
            string QueryPostVenta = null;
            string QueryPostProyectos = null;
            string QueryPostBloques = null;
            string QueryPostParametros = null;
            string QueryCargaVenta = null;
            string QueryCargaProyectos = null;
            string QueryCargaBloques = null;
            string QueryCargaParametros = null;
            string QueryBorrarCompra = null;
            string QueryBorrarVenta = null;
            string QueryBorrarProyectos = null;
            string QueryBorrarBloques = null;
            string QueryBorrarParametros = null;
            string QueryBorrarLog = null;
            


           
            DataTable dtcompra = null;
            DataTable dtventa = null;
            DataTable dtproyectos = null;
            DataTable dtbloques = null;
            DataTable dtparametros = null;

            bool Validar = ConsultarPasos();
            if (Validar)
            {
            
                if (CerrarPaso2 == 0)
                {

                    // Proceso de carga de ofertas de comercializadores -- OfertaCompra

                    QueryBorrarLog = "DELETE FROM [dbo].[LogProcesos]";

                    //QueryPostCompra = "SELECT *,22.3 EnergiaMin FROM public.\"ofertasCompra\" where sobre=1 and \"Proceso\"='Subasta'"; 

                    //QueryPostCompra = "SELECT Com.\"NombreCorto\" nombre,OfeCom.\"Codigo\" ID_oferta,OfeCom.\"CantidadMaxima\" energiaMax,COALESCE(OfeCom.\"PrecioOferta\", 0) precio, row_number() over (order by OfeCom.\"HoraRegistroPrecio\" desc) ordenllegada,1 Sobre,'Subasta' Proceso,23 energiaMin";
                    //QueryPostCompra = QueryPostCompra + " FROM public.\"OfertasComercializador\" OfeCom, public.\"Comercializadores\" Com where OfeCom.\"Comercializador_id\" = Com.\"IdComercializador\"";

                    QueryPostCompra = "	SELECT Com.\"NombreCorto\" nombre,OfeCom.\"Codigo\" ID_oferta,OfeCom.\"CantidadMaxima\" energiaMax,COALESCE(OfeCom.\"PrecioOferta\", 0) precio, ";
                    QueryPostCompra = QueryPostCompra + "row_number() over(order by OfeCom.\"HoraRegistroPrecio\" desc) ordenllegada,1 Sobre,'Subasta' Proceso,COALESCE(Mec.\"Minimo\",0) energiaMin ";
                    QueryPostCompra = QueryPostCompra + " FROM public.\"OfertasComercializador\" OfeCom, public.\"Comercializadores\" Com, public.\"Oferentes\" Ofe, public.\"MecanismoComplementario\" Mec";
                    QueryPostCompra = QueryPostCompra + " where OfeCom.\"Comercializador_id\" = Ofe.\"IdOferente\" and Ofe.\"IdComercializador_id\" = Com.\"IdComercializador\" ";
                    QueryPostCompra = QueryPostCompra + " and Mec.\"Comercializador_id\" = Ofe.\"IdComercializador_id\" --and Mec.\"CodigoOferta\" = OfeCom.\"Codigo\"";


                    QueryPostBloques = "SELECT \"Bloque\",\"Pi\",\"Pf\" FROM public.\"Convocatoria_Bloques\"";

                    QueryCargaCompra = "dbo.GrabarOfertas";
                    QueryBorrarCompra = "DELETE FROM [dbo].[ofertasCompra] where sobre=1 and \"Proceso\"='Subasta'";

                    // Proceso de carga de ofertas de generadores -- OfertaVenta
                    //QueryPostVenta = "SELECT * FROM public.\"ofertasVenta\" where sobre=1 and \"Proceso\"='Subasta'";


                    QueryPostVenta = "SELECT OfePro.\"Codigo\" nombre, OfePro.\"IdOferta\" ID_oferta, Blo.\"Bloque\" bloque, OfePro.\"MaxPaquetes\" numPaquetesMax, OfePro.\"MinPaquetes\" numPaquetesMin,";
                    QueryPostVenta = QueryPostVenta + "COALESCE(OfePro.\"PrecioOferta\", 0) precio,(select (CASE WHEN OfePro.\"RestriccionOferta\" = 'SIM' THEN (select OfeP1.\"Codigo\" from public.\"OfertasProyectos\" OfeP1 where OfeP1.\"IdOferta\" = OfePro.\"OfertaRestriccion_id\" )";
                    QueryPostVenta = QueryPostVenta + "END)) as simultanea, (select (CASE WHEN OfePro.\"RestriccionOferta\" = 'EXC' THEN";
                    QueryPostVenta = QueryPostVenta + " (select OfeP1.\"Codigo\" from public.\"OfertasProyectos\" OfeP1 where OfeP1.\"IdOferta\" = OfePro.\"OfertaRestriccion_id\" ) END)) as excluyente, ";
                    QueryPostVenta = QueryPostVenta + "(select (CASE WHEN OfePro.\"RestriccionOferta\" = 'DEP' THEN";
                    QueryPostVenta = QueryPostVenta + "(select OfeP1.\"Codigo\" from public.\"OfertasProyectos\" OfeP1 where OfeP1.\"IdOferta\" = OfePro.\"OfertaRestriccion_id\" )";
                    QueryPostVenta = QueryPostVenta + "END)) as dependiente,row_number() over (order by OfePro.\"HoraRegistroPrecio\" desc) ordenllegada,1 Sobre,'Subasta' Proceso";
                    QueryPostVenta = QueryPostVenta + " FROM public.\"OfertasProyectos\" OfePro,public.\"Convocatoria_Bloques\" Blo";
                    QueryPostVenta = QueryPostVenta + " Where OfePro.\"Bloque_id\" = Blo.\"IdBloque\"";                 

                    //QueryPostProyectos = "SELECT Pry.\"Codigo\" nombre,ReqT.\"CapacidadEfectivaTotal\" capacidadMaxima,Fue.\"Factor\" factorPlanta,null empresa,'Subasta' Proceso";
                    //QueryPostProyectos = QueryPostProyectos + " FROM public.\"Proyectos\" Pry, public.\"RequisitosTecnicos\" ReqT, public.\"Fuentes_Energia\" Fue";
                    //QueryPostProyectos = QueryPostProyectos + " where Pry.\"RequisitoTecnico_id\" = ReqT.\"IdRequisitoTecnico\" and Pry.\"Fuente_id\" = Fue.\"IdFuenteEnergia\";";

                    QueryPostProyectos = "SELECT Pry.\"Codigo\" nombre,ReqT.\"CapacidadEfectivaTotal\" capacidadMaxima,Fue.\"Factor\" factorPlanta,Oferentes.\"RazonSocial\" empresa,'Subasta' Proceso";
                    QueryPostProyectos = QueryPostProyectos + " FROM public.\"Proyectos\" Pry, public.\"RequisitosTecnicos\" ReqT, public.\"Fuentes_Energia\" Fue, public.\"VinculosEconomicos\" Vin, public.\"Oferentes\" Oferentes";
                    QueryPostProyectos = QueryPostProyectos + " where Pry.\"RequisitoTecnico_id\" = ReqT.\"IdRequisitoTecnico\" and Pry.\"Fuente_id\" = Fue.\"IdFuenteEnergia\"";
                    QueryPostProyectos = QueryPostProyectos + " and Oferentes.\"IdOferente\" = Pry.\"Generador_id\" and Vin.\"Declarante_id\" = Oferentes.\"IdOferente\" and Pry.\"Estado\"='CAL_GAR'";

                    QueryPostParametros = "SELECT \"IdParametroSubasta\",\"DemandaObjetivo\",\"TopeMaximoPromedio\", \"TopeMaximoIndividual\", \"TamanoPaquete\",\"HoraRegistro\" FROM public.\"ParametrosSubasta\"";
                    QueryBorrarParametros = "DELETE FROM [dbo].[ParametrosSubasta]";

                    QueryCargaVenta = "dbo.GrabarOfertasVenta";
                    QueryBorrarVenta = "DELETE FROM [dbo].[ofertasVenta] where sobre=1 and proceso='Subasta'";

                    QueryCargaProyectos = "dbo.GrabarProyectos";
                    QueryBorrarProyectos = "DELETE FROM [dbo].[proyectosGeneracion] where Proceso='Subasta'";

                    QueryCargaBloques = "dbo.GrabarBloques";
                    QueryBorrarBloques = "DELETE FROM [dbo].[Bloques] ";

                    QueryCargaParametros = "dbo.[GrabarParametros]";


                    // Proceso de lectura de ofertas desde la bd fuente -- POSTGRES
                    try
                    {
                        int RegsaborradosLog = DAL.ExecuteQueryNormal(QueryBorrarLog);

                        Mensaje = "Inicia Proceso de Carga de Ofertas Sobre No 1...." + "\r\n";
                        LogOfe = Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        int RegsaborradosCompra = DAL.ExecuteQueryNormal(QueryBorrarCompra);
                        int RegsaborradosVenta = DAL.ExecuteQueryNormal(QueryBorrarVenta);
                        int RegsaborradosProyectos = DAL.ExecuteQueryNormal(QueryBorrarProyectos);
                        int RegsaborradosBloques = DAL.ExecuteQueryNormal(QueryBorrarBloques);
                        int RegsaborradosParametros = DAL.ExecuteQueryNormal(QueryBorrarParametros);

                        Mensaje = "Conectando a la B.D de Ofertas de La UPME...." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        dtcompra = DAL.ExecuteQueryPostgres(QueryPostCompra);

                        Mensaje = "Lectura de datos de Comercializadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");

                        Mensaje = "Lectura de datos de Generadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        dtventa = DAL.ExecuteQueryPostgres(QueryPostVenta);
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");

                        Mensaje = "Lectura de datos de Proyectos de Generación de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        dtproyectos = DAL.ExecuteQueryPostgres(QueryPostProyectos);
                        DAL.InsertarLog(Mensaje, "Carga de Proyectos de Generación Sobre 1", "Carga de Proyectos de Generación Sobre 1");

                        Mensaje = "Lectura de datos de Bloques de Ofertas de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        dtbloques = DAL.ExecuteQueryPostgres(QueryPostBloques);
                        DAL.InsertarLog(Mensaje, "Carga de Bloques de ofertas Sobre 1", "Carga de Bloques de ofertas Sobre 1");

                        Mensaje = "Lectura de datos de Parametros de Subasta de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        dtparametros = DAL.ExecuteQueryPostgres(QueryPostParametros);
                        DAL.InsertarLog(Mensaje, "Carga de Parametros de la subasta Sobre 1", "Carga de Parametros de la subasta Sobre 1");
                        
                    }
                    catch (Exception ex1)
                    {
                        OfertasOk = false;
                        MessageBox.Show(ex1.Message, "Error en la lectura de las ofertas");
                        Mensaje = "Error en la lectura de las ofertas ..." + ex1.Message + "\r\n"; 
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Upme Sobre 1", "Carga de Ofertas Upme Sobre 1");
                        //throw;

                    }

                    if (dtcompra.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de Comercializadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        return;
                    }

                    if (dtventa.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de Generadores de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        return;
                    }

                    if (dtproyectos.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de Proyectos de Generación de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Proyectos de Generación Sobre 1", "Carga de Proyectos de Generación Sobre 1");
                        return;
                    }

                    if (dtbloques.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de bloques de ofertas de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de bloques de ofertas Sobre 1", "Carga de bloques de ofertas Sobre 1");
                        return;
                    }

                    if (dtparametros.Rows.Count == 0)
                    {
                        Mensaje = "No cargo datos de Parametros de la subasta de la UPME..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Parametros de la subasta Sobre 1", "Carga de Parametros de la subasta Sobre 1");
                        return;
                    }

                    try
                    {
                        Mensaje = "Cargando datos de ofertas de Comercializadores..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        int Regscompra = DAL.ExecuteQueryParametro(QueryCargaCompra, "@OfertasCompra", dtcompra);
                        if (!ValidarRegistrosCargados.ValidarCargas(dtcompra.Rows.Count, Regscompra, Mensaje))
                        {
                            Mensaje = "Numero de registros leidos de Comercializadores no es igual a los insertados..." + "\r\n";
                            LogOfe = LogOfe + Mensaje;
                            DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        }

                        Mensaje = "Cargando datos de ofertas de Generadores..." + "\r\n";
                        LogOfe = LogOfe + Mensaje;                        
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas", "Carga de Ofertas");
                        int Regsventa = DAL.ExecuteQueryParametro(QueryCargaVenta, "@OfertasVenta", dtventa);
                        if (!ValidarRegistrosCargados.ValidarCargas(dtventa.Rows.Count, Regsventa, Mensaje))
                        {
                            Mensaje = "Numero de registros leidos de Generadores no es igual a los insertados..." + "\r\n";
                            LogOfe = LogOfe + Mensaje;
                            DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        }


                        Mensaje = "Cargando datos de Proyectos de Generación..." + "\r\n";                       
                        LogOfe = LogOfe + Mensaje;
                        DAL.InsertarLog(Mensaje, "Carga de Proyectos de Generación", "Carga de Ofertas Sobre 1");

                        int Regsproyectos = DAL.ExecuteQueryParametro(QueryCargaProyectos, "@Proyectos", dtproyectos);
                        if (!ValidarRegistrosCargados.ValidarCargas(dtproyectos.Rows.Count, Regsproyectos, Mensaje))
                        {
                            Mensaje = "Numero de registros leidos de Proyectos de Generación no es igual a los insertados..." + "\r\n";
                            LogOfe = LogOfe + Mensaje;
                            DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        }

                        LogOfe = LogOfe + Mensaje;

                        Mensaje = "Cargando datos de Bloques de Ofertas" + "\r\n";
                        DAL.InsertarLog(Mensaje, "Carga de Bloques de Ofertas", "Carga de Ofertas Sobre 1");
                        int Regsbloques = DAL.ExecuteQueryParametro(QueryCargaBloques, "@Bloques", dtbloques);
                        if (!ValidarRegistrosCargados.ValidarCargas(dtbloques.Rows.Count, Regsbloques, Mensaje))
                        {
                            Mensaje = "Numero de registros leidos de Bloques de Ofertas no es igual a los insertados..." + "\r\n";
                            LogOfe = LogOfe + Mensaje;
                            DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        }

                        Mensaje = "Cargando datos de Parametros de la subasta" + "\r\n";
                        DAL.InsertarLog(Mensaje, "Carga de Parametros de la subasta", "Carga de Parametros de la subasta Sobre 1");
                        int Regsparametros = DAL.ExecuteQueryParametro(QueryCargaBloques, "@Bloques", dtparametros);
                        if (!ValidarRegistrosCargados.ValidarCargas(dtparametros.Rows.Count, Regsparametros, Mensaje))
                        {
                            Mensaje = "Numero de registros leidos de Parametros de la subasta no es igual a los insertados..." + "\r\n";
                            LogOfe = LogOfe + Mensaje;
                            DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        }


                        LogOfe = LogOfe + Mensaje;

                        Mensaje = "Carga de datos exitosa..." + "\r\n";
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
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
                        DAL.InsertarLog(Mensaje, "Carga de Ofertas Sobre 1", "Carga de Ofertas Sobre 1");
                        LogOfertas.Text = LogOfe.ToString();
                        MostrarResumenOfertasCompra();
                        MostrarResumenOfertasVenta();
                        LogOfertas.Text = LogOfe.ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("El paso ya fue cerrado, los datos de ofertas de sobre 1 ya fueron cargados y validados","Cierre de pasos");
                MostrarOfertasCompra();
                MostrarOfertasVenta();
                MostrarGrafico();
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
                    Mensaje = "Cierre paso 2 exitoso." + "\r\n"; 
                    LogOfe = LogOfe + Mensaje;
                    DAL.InsertarLog(Mensaje, "Cierre paso 2", "Cierre Pasos");
                    MessageBox.Show(Mensaje, "Cierre de pasos");
                    CerrarPaso2 = 1;
                }
                else
                {
                    Mensaje = "Cierre paso 2 fallido. Aún no ha sido realizado el proceso de carga de las ofertas del Sobre 1" + "\r\n";
                    LogOfe = LogOfe + Mensaje;
                    DAL.InsertarLog(Mensaje, "Cierre paso 2", "Cierre Pasos");
                    MessageBox.Show(Mensaje, "Cierre de pasos");
                    CerrarPaso2 = 0;

                }

            }
            else
            {
                  MessageBox.Show("El paso ya fue cerrado, los datos de ofertas de sobre 1 ya fueron cargados y validados", "Cierre de pasos");
            }            
        }

        private void CriterioCompetencia(object sender, RoutedEventArgs e)
        {
            // Validar Criterio de competencia
            DataTable dt = null;
            int Cumple = 0;

            Mensaje = "Ejecutando Criterio de Competencia ..." + "\r\n";
            LogOfe = LogOfe + Mensaje;
            DAL.InsertarLog(Mensaje, "Criterio de Competencia", "Criterio de Competencia");
            String QueryCriterios = "exec VerificarCriterioCompetencia 1, Subasta";          

            try
            {
                dt = DAL.ExecuteQuery(QueryCriterios);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Error en el proceso de calculo de los criterios de competencia");
                Mensaje = "Error en el proceso de calculo de los criterios de competencia..." + ex1.Message + "\r\n";
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Criterio de Competencia", "Criterio de Competencia");
                //throw;

            }

            if (dt.Rows.Count == 0)
            {

                MessageBox.Show("Criterio de Competencia", "Error en la consulta de datos de los criterios de competencia");
                Mensaje = "Error en la consulta de datos de criterios de competencia, no se generaron datos" + "\r\n";
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Criterio de Competencia", "Criterio de Competencia");
            }
            else
            {
                Mensaje = "Criterios de Competencia Ejecutados ..." + "\r\n";
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Criterios de Competencia", "Criterios de Competencia");
                DataRow[] results = dt.Select(@"Cumple= 'No Cumple'");
                Cumple = results.Count(); 
                //Cumple = dt.Rows.IndexOf(dt.Select(@"Cumple= 'No Cumple'")[0]);
                if (Cumple != 0)
                {
                    Mensaje = "Criterios de Competencia Resultados ... NO CUMPLE" + "\r\n";
                    LogOfe = LogOfe + Mensaje;
                    DAL.InsertarLog(Mensaje, "Criterios de Competencia", "Criterios de Competencia");
                    MessageBox.Show("No Cumple Criterio de Competencia ", "Criterio de Competencia");
                    /// deshabilitar los pasos siguientes
                 }
                else
                 {
                    Mensaje = "Criterios de Competencia Resultados ... CUMPLE" + "\r\n";
                    LogOfe = LogOfe + Mensaje;
                    DAL.InsertarLog(Mensaje, "Criterios de Competencia", "Criterios de Competencia");
                    MessageBox.Show("Criterio de Competencia", "Cumple Criterio de Competencia " );

                }
                MostrarOfertasTodas(QueryCriterios, 3, "UpmeSubasta2019.Reportes.CiterioCompetencia.rdlc", "CrietrioCompetenciaSobre1");
                LogOfertas.Text = LogOfe;
            }

        }


        public static bool ConsultarPasos()
        {
            bool ValidarPaso = false;
            DataTable dt = null;
            string Query1 = "EXEC DBO.ConsultaDatosPasos 'Cierre Pasos', 'Cierre paso 2', 'Cierre paso 2 exitoso.'";
            try
            {
                dt = DAL.ExecuteQuery(Query1);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Error en la consulta de datos de los pasos de las ofertas");
                Mensaje = "Error en la consulta de datos de las ofertas ..." + ex1.Message + "\r\n";
                LogOfe = LogOfe + Mensaje;
                DAL.InsertarLog(Mensaje, "Datos Ofertas Venta", "Datos Ofertas Venta");
                //throw;

            }

            if (dt.Rows.Count ==0)
                ValidarPaso = true;
            else
                ValidarPaso = false;

            return ValidarPaso;
        }


       
        private void MostrarOfertas(object sender, RoutedEventArgs e)
        {
            MostrarOfertasCompra();
            MostrarOfertasVenta();
            MostrarGrafico();
        }
    }
}
