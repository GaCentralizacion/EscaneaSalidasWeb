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

public partial class sicop_Inicio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.AppendHeader("Pragma", "no-cache");
        Response.AppendHeader("Cache-Control", "no-store");
        Response.AppendHeader("Expires", "-1");

        Session["sicop_accion"]      = Request.QueryString["sicop_accion"] == null?"":Request.QueryString["sicop_accion"].ToString().Trim();
        Session["sicop_id_usuario"]  = Request.QueryString["sicop_id_usuario"] == null ? "" : Request.QueryString["sicop_id_usuario"].ToString().Trim();        
        Session["sicop_rol_usuario"] = Request.QueryString["sicop_rol_usuario"] == null ? "" : Request.QueryString["sicop_rol_usuario"].ToString().Trim();
        Session["sicop_id_agencia"] =  Request.QueryString["sicop_id_agencia"] == null ? "" : Request.QueryString["sicop_id_agencia"].ToString().Trim();
        Session["sicop_nombre_usuario"] = Request.QueryString["sicop_nombre_usuario"] == null ? "" : Request.QueryString["sicop_nombre_usuario"].ToString().Trim();

        if ((Convert.ToString(Session["sicop_id_usuario"]) == ""))
        {
            Response.Redirect("sicop_Login.aspx");
        }
        else {
            Response.Redirect("sicop_ConsultaSalidas.aspx");
        }		
    }
}
