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

namespace PortalClientes
{
    public partial class sicop_SeleccionaAgencia : System.Web.UI.Page
    {
        private string CadenaConexion = ConfigurationManager.AppSettings["sicop_ConnectionString"].ToString(); //es la cadena de conexión a la base de datos principal //"Data Source=192.168.20.8;Initial Catalog=PortalClientes;UID=sa;PWD=mama"; 
        SqlConnection conexion = new SqlConnection(); //objeto Conexion a la BD principal
        private SqlDataAdapter objAdapter;    //Objeto adaptador de datos de la BD principal
        ConexionBDchkDos objDB = null;

        public string id_usuario = "";
        public string Accion = "";
        public string rol = "";
        public string nombre_usuario = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            Ajax.Utility.RegisterTypeForAjax(typeof(sicop_SeleccionaAgencia));
            if (!IsPostBack)
            {
                Session["sicop_Accion"] = Request.QueryString["Accion"] == null ? "" : Request.QueryString["Accion"].ToString().Trim();
                Session["sicop_id_usuario"] = Request.QueryString["sicop_id_usuario"] == null ? "" : Request.QueryString["sicop_id_usuario"].ToString().Trim();
                Session["sicop_rol_usuario"] = Request.QueryString["sicop_rol_usuario"] == null ? "" : Request.QueryString["sicop_rol_usuario"].ToString().Trim();
                Session["sicop_id_agencia"] = Request.QueryString["sicop_id_agencia"] == null ? "" : Request.QueryString["sicop_id_agencia"].ToString().Trim();
                Session["sicop_nombre_usuario"] = Request.QueryString["sicop_nombre_usuario"] == null ? "" : Request.QueryString["sicop_nombre_usuario"].ToString().Trim();

                this.id_usuario = Convert.ToString(Session["sicop_id_usuario"]);
                this.nombre_usuario = Convert.ToString(Session["sicop_nombre_usuario"]);
                this.Accion = Convert.ToString(Session["sicop_Accion"]);
                this.rol = Convert.ToString(Session["sicop_rol_usuario"]);
 
            }
        }

        [Ajax.AjaxMethod()]
        public DataSet ConsultaAgencias(string id_usuario)
        {
            DataSet resultados = new DataSet();
            string Q = "";
            Q = "Select a.id_agencia as valor, a.nombre as texto FROM AGENCIAS a, BP_USR_AGENCIAS b where a.id_agencia=b.id_agencia and b.id_usr=" + id_usuario  + " order by 2";
            try
            {
                if (this.CadenaConexion != "")
                {
                    if (conexion.State.ToString().ToUpper() == "CLOSED")
                    {
                        conexion.ConnectionString = this.CadenaConexion;
                        this.conexion.Open();
                    }

                    this.objAdapter = new SqlDataAdapter(Q, this.conexion);
                    this.objAdapter.Fill(resultados, "USUARIOS");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                this.conexion.Close();
            }
            return resultados;
        }

    }
}
