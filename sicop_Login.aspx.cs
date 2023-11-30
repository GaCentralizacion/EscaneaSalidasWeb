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


public partial class sicop_Login : System.Web.UI.Page
{    
    private static string CadenaConexion = ConfigurationManager.AppSettings["sicop_ConnectionString"].ToString(); //es la cadena de conexión a la base de datos principal //"Data Source=192.168.20.8;Initial Catalog=PortalClientes;UID=sa;PWD=mama"; 
    SqlConnection conexion = new SqlConnection(); //objeto Conexion a la BD principal
    private SqlDataAdapter objAdapter;    //Objeto adaptador de datos de la BD principal
    ConexionBDchkDos objDB = new ConexionBDchkDos(CadenaConexion);

    public string siguiente = "00000000";

    protected void Page_Load(object sender, EventArgs e)
    {        
        Ajax.Utility.RegisterTypeForAjax(typeof(sicop_Login));
        if (!IsPostBack)
        { 
        string IP=Request.ServerVariables["REMOTE_ADDR"];

        string  Q = "Select Isnull(Max(ACCESO),0)+1 as SIGUIENTE from CONTADOR_ACCESOS";
            this.objDB = new ConexionBDchkDos(CadenaConexion);  
            this.siguiente = this.objDB.ConsultaUnSoloCampo(Q);

            Q = "Insert into CONTADOR_ACCESOS (ACCESO,FECHA,IP)";
            Q += " values (" + siguiente + ",getdate(),'" + IP.Trim() + "')";
            this.objDB.EjecUnaInstruccion(Q); 
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
        dt.Columns.Add("nombre_usuario");
        dt.Columns.Add("rol");
        

        string NombreTabla = "";
        string Q = "";
        string[] Dat = Datos.Split('|');

        string usrencontrado="";

        ConexionBDchkDos objcon = new ConexionBDchkDos(CadenaConexion);

            Q = "Select id_usr from BP_USR_SALIDAS where cve_usuario='" + Dat[0].ToString() + "' and contrasena='" + Dat[1].ToString() + "'";
            usrencontrado = objcon.ConsultaUnSoloCampo(Q);            
        
        DataRow reg = dt.NewRow(); 
        if (usrencontrado.Trim() != "")
        {            
            try
            {                                                      
                            string Id_usuario = usrencontrado.Trim();
                            string Rol = objcon.ConsultaUnSoloCampo("Select id_rol from BP_USR_SALIDAS where id_usr=" + Id_usuario);
                            string NombreUsr = objcon.ConsultaUnSoloCampo("Select nombre from BP_USR_SALIDAS where id_usr=" + Id_usuario); 
                            reg["id_usuario"] = Id_usuario;
                            reg["nombre_usuario"] = NombreUsr.Trim();
                            reg["rol"] = Rol;            
                            reg["mensaje"] = "usuario encontrado";

                            //validamos si el usuario puede ingresar a más de una agencia 
                            Q = "Select id_agencia from BP_USR_AGENCIAS where id_usr = " + Id_usuario;
                            DataSet dsagexusuario = objcon.Consulta(Q);
                            if (dsagexusuario != null && dsagexusuario.Tables.Count > 0 && dsagexusuario.Tables[0].Rows.Count > 0)
                            {
                                if (dsagexusuario.Tables[0].Rows.Count == 1)
                                {   //el usuario solo puede ingresar a una sola agencia, indicamos cual es esta agencia.
                                    reg["mensaje"] = "usuario encontrado" + "|" + dsagexusuario.Tables[0].Rows[0]["id_agencia"].ToString();
                                }
                                else { 
                                    //el usuario puede ingresar a varias agencias.
                                    reg["mensaje"] = "usuario encontrado" + "|variasagencias";
                                }
                            }
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
		 
        [Ajax.AjaxMethod()]        
        public DataSet ConsultaUsuarios()
        {
            DataSet resultados = new DataSet();
            string Q = "";
            Q = "Select cve_usuario as valor, nombre as texto FROM BP_USR_SALIDAS order by 2";
            try
            {
                if (CadenaConexion != "")
                {
                    if (conexion.State.ToString().ToUpper() == "CLOSED")
                    {
                        conexion.ConnectionString = CadenaConexion;
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
