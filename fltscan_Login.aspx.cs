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
using System.Data.SqlClient;
using System.Diagnostics;


public partial class fltscan_Login : System.Web.UI.Page
{

    private string CadenaConexion = "Data Source=192.168.20.5;Initial Catalog=PortalClientes;UID=sa;PWD=S0p0rt3"; //ConfigurationManager.AppSettings["ConnectionString"].ToString(); //es la cadena de conexión a la base de datos principal
    SqlConnection conexion = new SqlConnection(); //objeto Conexion a la BD principal
    private SqlDataAdapter objAdapter;    //Objeto adaptador de datos de la BD principal

    public string siguiente = "00000000";

    protected void Page_Load(object sender, EventArgs e)
    {        
        Ajax.Utility.RegisterTypeForAjax(typeof(fltscan_Login));
        if (!IsPostBack)
        { 
        string IP=Request.ServerVariables["REMOTE_ADDR"];

        string  Q = "Select Isnull(Max(ACCESO),0)+1 as SIGUIENTE from CONTADOR_ACCESOS";            
			ConexionBDchkDos objcon = new ConexionBDchkDos(this.CadenaConexion);  
            this.siguiente = ConsultaUnSoloCampo(Q);

            Q = "Insert into CONTADOR_ACCESOS (ACCESO,FECHA,IP)";
            Q += " values (" + siguiente + ",getdate(),'" + IP.Trim() + "')";
            objcon.EjecUnaInstruccion(Q); 
            if (this.siguiente.Length>0)
                this.siguiente = "00000000".Substring(0,8-this.siguiente.Length) + this.siguiente;               
        }
    }
    /// <summary>
    /// Encargada de validar si el usuario tiene acceso o no a la aplicación
    /// </summary>
    /// <param name="Datos">Puede ser el RFC o bien puede ser tanto login|password, separados por | si es por RFC siempre se busca en la tabla de Clientes
    ///  sin embargo si es por login y password, ademas hay que buscar en ambas tablas, primero en la de Clientes y luego en la de usuarios.
    /// </param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public DataSet Valida(string Datos)
    {
        DataSet res = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("mensaje");
        dt.Columns.Add("id_usuario");
        dt.Columns.Add("rol");
        dt.Columns.Add("nombreusuario");

        string NombreTabla = "";
        string Q = "";
        string[] Dat = Datos.Split('|');

        string usrencontrado="";

        ConexionBDchkDos objcon = new ConexionBDchkDos(this.CadenaConexion);
                       
            Q = "Select id_usr from SCAN_USR where contrasena='" + Dat[0].ToString() + "' and status='True'";
            usrencontrado = objcon.ConsultaUnSoloCampo(Q);            
        
        DataRow reg = dt.NewRow(); 
        if (usrencontrado.Trim() != "")
        {            
            try
            {                                                      
                            string Id_usuario = usrencontrado.Trim();
                            
                            Q = "Select id_rol from SCAN_USR where id_usr=Convert(int," + usrencontrado.Trim() + ")" ;
                            string Rol = objcon.ConsultaUnSoloCampo(Q);
                            string nombreusuario = objcon.ConsultaUnSoloCampo("Select nombre from SCAN_USR where id_usr=Convert(int," + usrencontrado.Trim() + ")");
                            reg["id_usuario"] = Id_usuario;
                            reg["rol"] = Rol;                            
                            reg["mensaje"] = "usuario encontrado";
                            reg["nombreusuario"] = nombreusuario;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                //this.conexion.Close();
            }
        }
        else {            
            reg["mensaje"] = "No se encontró un usuario con los datos proporcionados.";        
        }
        dt.Rows.Add(reg);
        res.Tables.Add(dt); 
        return res;
    }
	
	    public string ConsultaUnSoloCampo(string Q)
    {
        string res = String.Empty;
        DataSet ds = new DataSet();
        try
        {
            if (this.conexion.State.ToString() != "Open")
            {
                this.conexion.ConnectionString = this.CadenaConexion.Trim();
                this.conexion.Open();
            }
            System.Data.SqlClient.SqlDataAdapter objAdaptador = new System.Data.SqlClient.SqlDataAdapter(Q, this.CadenaConexion);
            objAdaptador.Fill(ds, "Resultados");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {//no importa cuantos registros traiga siempre regresará solo la primer columna y del primer registro. 
                    res = ds.Tables[0].Rows[0][0].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally {
            this.conexion.Close();
        }
        return res;
    }



        [Ajax.AjaxMethod()]        
        public DataSet ConsultaUsuarios()
        {
            DataSet resultados = new DataSet();
            string Q = "";
            Q = "Select Cod as valor, Usuario as texto FROM Usuarios order by 2";
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
