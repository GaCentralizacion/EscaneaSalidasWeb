using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace PortalClientes
{
    public partial class sicop_ConsultaSalidas : System.Web.UI.Page
    {
        private string CadenaConexion = ConfigurationManager.AppSettings["sicop_ConnectionString"].ToString(); //es la cadena de conexión a la base de datos principal
        private string Ruta = ConfigurationManager.AppSettings["ruta"].ToString();
        SqlConnection conexion = new SqlConnection(); //objeto Conexion a la BD principal
        private SqlDataAdapter objAdapter;    //Objeto adaptador de datos de la BD principal
        public string id_agencia = "";
        public string sicop_nombre_usuario = "";
        public string nombre_agencia = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.id_agencia = Convert.ToString(Session["sicop_id_agencia"]);
            this.sicop_nombre_usuario = Convert.ToString(Session["sicop_nombre_usuario"]);            
            Ajax.Utility.RegisterTypeForAjax(typeof(sicop_ConsultaSalidas));
            ConexionBDchkDos objDB = new ConexionBDchkDos(this.CadenaConexion);
            this.nombre_agencia = objDB.ConsultaUnSoloCampo("Select alias from AGENCIAS where id_agencia=" + this.id_agencia); 
        }

        [Ajax.AjaxMethod()]		
		//sicop_ConsultaSalidas.ConsultaBitacora(textoBuscar,ordenx,opcbusqueda,id_agencia,CallBackConsultaBitacora);
        public DataSet ConsultaBitacora(string textoBuscar, string ordenx, string opcbusqueda, string id_agencia)
        {
 	    DataSet res = new DataSet();

            ConexionBDchkDos objDB = new ConexionBDchkDos(this.CadenaConexion);
            
            //buscamos el grupo de agencias al que pertenece la agencia.
            string strCadAgencias = "";
            string Q = "select id_agencia from BP_AGENCIAS_X_EMPRESA ";
                   Q += " where id_empresa = (Select id_empresa from BP_AGENCIAS_X_EMPRESA where id_agencia='" + id_agencia.Trim() + "')";

                   DataSet dsaux = objDB.Consulta(Q);
                   if (dsaux != null && dsaux.Tables.Count > 0 && dsaux.Tables[0].Rows.Count > 0)
                   {
                       foreach (DataRow dragencia in dsaux.Tables[0].Rows)
                       {
                           strCadAgencias += "'" + dragencia["id_agencia"] + "',";                       
                       }

                       if (strCadAgencias.Trim() != "")
                       {
                           strCadAgencias = strCadAgencias.Substring(0, strCadAgencias.LastIndexOf(','));  
                       }
                   }


                    Q = "Select distinct Convert(char(10),fecha,103) as fecha, ";
                    Q += " Convert(char(10),fecha,108) as hora,";
                    Q += " quien, que, aquien, aquien as codebar, substring(que,patindex('%!%',que)+1,len(que)) as idsicop, ";
                    Q += " isnull(factura,'') as factura, isnull(tipo_venta,'') as tipo_venta, isnull(tipo_auto,'') as tipo_auto, ";
                    Q += " isnull(Convert(char(10),fecha_factura,103),'') as fecha_factura ";
                    Q += " FROM SICOP_BITACORA where id_bitacora > 0 ";
                    if (opcbusqueda.ToUpper().Trim() == "AQUIEN" && textoBuscar.Trim() != "")                    
                        Q += " and aquien  like '%" + textoBuscar.Trim() + "%'";
                    
                    if (opcbusqueda.ToUpper().Trim() == "QUIEN" && textoBuscar.Trim() != "")
                        Q += " and quien like '%" + textoBuscar.Trim() + "%'";

                    if (opcbusqueda.ToUpper().Trim() == "FACTURA" && textoBuscar.Trim() != "")
                        Q += " and factura like '%" + textoBuscar.Trim() + "%'";

                    if (opcbusqueda.ToUpper().Trim() == "FECHA" && textoBuscar.Trim() != "")
                    {
                        DateTime fechaaux;
                        try
                        {
                            //fechaaux = Convert.ToDateTime(textoBuscar.Trim());
                            int anio = Convert.ToInt16(textoBuscar.Substring(textoBuscar.LastIndexOf('/')+1));
                            anio = anio < 2000 ? 2000+anio : anio;
                            int mes = Convert.ToInt16(textoBuscar.Substring(textoBuscar.IndexOf('/')+1,2));
                            int dia = Convert.ToInt16(textoBuscar.Substring(0,2));
                            fechaaux = new DateTime(anio, mes, dia);
                            textoBuscar = fechaaux.ToString("yyyyMMdd");
                        }
                        catch (Exception exfecha)
                        {
                            Debug.WriteLine(exfecha.Message);
                            textoBuscar = DateTime.Now.ToString("yyyyMMdd");  
                        }
                        Q += " and Convert(char(8),fecha,112) = '" + textoBuscar.Trim() + "'";
                    }
                      


                    if (opcbusqueda.ToUpper().Trim() == "RANGO" && textoBuscar.Trim() != "")
                    {
                        DateTime fechaauxi;
                        DateTime fechaauxf;
                        try
                        {
                            string fini = textoBuscar.Substring(0, textoBuscar.IndexOf('-')).Trim();
                            fini = fini.Replace("-", "");
                            string ffin = textoBuscar.Substring(textoBuscar.IndexOf('-')+1).Trim();
                            int anioi = Convert.ToInt16(fini.Substring(fini.LastIndexOf('/') + 1));
                            int aniof = Convert.ToInt16(ffin.Substring(ffin.LastIndexOf('/') + 1));
                            
                            anioi = anioi < 2000 ? 2000 + anioi : anioi;
                            aniof = aniof < 2000 ? 2000 + aniof : aniof;

                            int mesi = Convert.ToInt16(fini.Substring(fini.IndexOf('/') + 1, 2));
                            int mesf = Convert.ToInt16(ffin.Substring(ffin.IndexOf('/') + 1, 2));

                            int diai = Convert.ToInt16(fini.Substring(0, 2));
                            int diaf = Convert.ToInt16(ffin.Substring(0, 2));

                            fechaauxi = new DateTime(anioi, mesi, diai);
                            fechaauxf = new DateTime(aniof, mesf, diaf);                                                    
                        }
                        catch (Exception exfecha)
                        {
                            Debug.WriteLine(exfecha.Message);
                            fechaauxi = DateTime.Now;
                            fechaauxf = DateTime.Now;
                        }
                        
                        Q += " and Convert(char(8),fecha,112) between '" + fechaauxi.ToString("yyyyMMdd") + "' and '" + fechaauxf.ToString("yyyyMMdd") + "'";
                    }

                    if (opcbusqueda.ToUpper().Trim() == "IDPROSPECTO" && textoBuscar.Trim() != "")
                    {//Actualizacion exitosa! Id Prospecto:
                        Q += " and que like '%Id Prospecto: " + textoBuscar.Trim() + "%'";
                    }

		    strCadAgencias=""; //20141114 para que solo muestre las de la agencia que se logueo.

                    if (strCadAgencias.Trim() != "")
                    {
                        Q += " and id_agencia in (" + strCadAgencias + ")";
                    }
					else
					{
					     Q += " and id_agencia in ('" + id_agencia.Trim() + "')";
					}
                    if (opcbusqueda.ToUpper().Trim() != "IDPROSPECTO")
                    {
                        Q += " and que like 'Actualizacion exitosa!%'";
                    }    
                    Q += " order by fecha";

                    res = objDB.Consulta(Q);  
		    res = GeneraCodigosBarras(res);
					
            return res;
        }
      
	    public DataSet GeneraCodigosBarras(DataSet ds)
		{
		
			foreach(DataRow reg in ds.Tables[0].Rows)
              {

                  try
                  {
                      System.Drawing.Graphics g = Graphics.FromImage(new Bitmap(1, 1));
                      string codigovin = reg["aquien"].ToString().ToUpper().Trim();

                      BarCode39.Code39 code = new BarCode39.Code39(codigovin);
                      code.Paint().Save( this.Ruta + "\\barcodes\\" + codigovin + ".png", ImageFormat.Png);
                      reg["codebar"] = "\\barcodes\\" + codigovin + ".png";
                  }
                  catch (Exception ex)
                  {
                      Debug.WriteLine(ex.Message); 
                  }
              }			            							
			
		    return ds;
		}
	  
    }
}
