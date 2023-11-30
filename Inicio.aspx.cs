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

public partial class Inicio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.AppendHeader("Pragma", "no-cache");
        Response.AppendHeader("Cache-Control", "no-store");
        Response.AppendHeader("Expires", "-1");

        Session["Accion"]      = Request.QueryString["Accion"] == null?"":Request.QueryString["Accion"].ToString().Trim();
        Session["Id_Cliente"]  = Request.QueryString["Id_Cliente"] == null ? "" : Request.QueryString["Id_Cliente"].ToString().Trim();
        Session["Id_Personal"] = Request.QueryString["Id_Personal"] == null ? "" : Request.QueryString["Id_Personal"].ToString().Trim();
        Session["Rol"]         = Request.QueryString["Rol"] == null ? "" : Request.QueryString["Rol"].ToString().Trim();
        Session["RFCCliente"]  = Request.QueryString["RFCCliente"] == null ? "" : Request.QueryString["RFCCliente"].ToString().Trim();
        

        if ((Convert.ToString(Session["Accion"]) == "Registro"))
        {
            Response.Redirect("pcMantDatosPersonal.aspx"); 
        }

        if ((Convert.ToString(Session["Accion"]) == "ActivarCuenta"))
        {
            string md51 = Request.QueryString["clv"] == null ? "" : Request.QueryString["clv"].ToString().Trim();
            Response.Redirect("pcActivarCuenta.aspx?clv=" + md51);
        }

        if ((Convert.ToString(Session["Id_Cliente"]) == "") && (Convert.ToString(Session["Id_Personal"])==""))
        {
            Response.Redirect("Login.aspx");
        }

        if ((Convert.ToString(Session["Id_Cliente"]) != "") && (Convert.ToString(Session["Rol"]) == "2"))
        {
            Response.Redirect("pcConsultaFacturasVIP.aspx");
        }
    }
}
