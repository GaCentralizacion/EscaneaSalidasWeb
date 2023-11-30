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

public partial class fltscan_Inicio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.AppendHeader("Pragma", "no-cache");
        Response.AppendHeader("Cache-Control", "no-store");
        Response.AppendHeader("Expires", "-1");

        Session["fltscan_Accion"]      = Request.QueryString["Accion"] == null?"":Request.QueryString["Accion"].ToString().Trim();
        Session["fltscan_id_usuario"]  = Request.QueryString["fltscan_id_usuario"] == null ? "" : Request.QueryString["fltscan_id_usuario"].ToString().Trim();        
        Session["fltscan_rol_usuario"] = Request.QueryString["fltscan_rol_usuario"] == null ? "" : Request.QueryString["fltscan_rol_usuario"].ToString().Trim();
        Session["fltscan_nombre_usuario"] = Request.QueryString["fltscan_nombre_usuario"] == null ? "" : Request.QueryString["fltscan_nombre_usuario"].ToString().Trim();

        if ((Convert.ToString(Session["fltscan_id_usuario"]) == "") && (Convert.ToString(Session["fltscan_rol_usuario"])==""))
        {
            Response.Redirect("fltscan_Login.aspx");
        }
		
    }
}
