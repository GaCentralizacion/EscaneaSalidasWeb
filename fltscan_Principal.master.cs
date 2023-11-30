using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;


public partial class fltscan_Principal : System.Web.UI.MasterPage
{

    string CadenaConexion = "Data Source=192.168.20.5;Initial Catalog=PortalClientes;UID=sa;PWD=S0p0rt3"; //ConfigurationManager.AppSettings["ConnectionString"].ToString(); //es la cadena de conexión a la base de datos principal
    DataTable dtMenuItems1 = new DataTable();
    public string strMenu = "";
    //protected System.Web.UI.WebControls.PlaceHolder placeHolder1;
    public string strNombreUsuario = "";

    private void Page_Init(object sender, System.EventArgs e)
    {        
                
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                // IDENTIFICAR EL ROL Y HACER CONSULTA PARA MENU            
                String Id_Rol = "";
                int ContTab = 0;
				
                Id_Rol = Convert.ToString(Session["fltscan_rol_usuario"]);

                this.strNombreUsuario = Convert.ToString(Session["fltscan_nombre_usuario"]);

                string id_usuario = "";
                id_usuario = Convert.ToString(Session["fltscan_id_usuario"]);

                string Accion = Convert.ToString(Session["fltscan_Accion"]);

                //if (!usuario.FncGetUsuarioActualizoPassword(id_usuario) && Accion=="")
                //{
                      //hay que redireccionar a la pagina del Cambio de Password
                //    Session["fltscan_Accion"] = "CambioPassword";
                //    Response.Redirect("fltscan_CambioPassword.aspx"); 
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); 
            }        
        }        
    }    
	
}
