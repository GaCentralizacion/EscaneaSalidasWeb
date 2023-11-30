using System;
using System.Collections;
using System.Collections.Generic; 
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
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Security;
using ICSharpCode.SharpZipLib.Zip;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using System.Web.SessionState;

public partial class fltscan_r_escaneo : System.Web.UI.Page
{

    private string CadenaConexion = "Data Source=192.168.20.5;Initial Catalog=PortalClientes;UID=sa;PWD=S0p0rt3"; //ConfigurationManager.AppSettings["ConnectionString"].ToString(); //es la cadena de conexión a la base de datos principal
    SqlConnection conexion = new SqlConnection(); //objeto Conexion a la BD principal
    private SqlDataAdapter objAdapter;    //Objeto adaptador de datos de la BD principal

    ConexionBDchkDos objDB = null;

    public string fltscan_id_usuario = "";  //Request.QueryString["chk2_Id_usuario"] == null ? "" : Request.QueryString["chk2_Id_usuario"].ToString().Trim();
    public string fltscan_rol_usuario = ""; //Request.QueryString["chk2_rol_usuario"] == null ? "" : Request.QueryString["chk2_rol_usuario"].ToString().Trim();

    string RutaLocal = HttpContext.Current.Server.MapPath("~");

    string ConnectionStringGPS = "Data Source=192.168.20.3;Initial Catalog=Middleware; Persist Security Info=True; User ID=centralizacionGA;Password=S0p0rt3";

    protected void Page_Load(object sender, EventArgs e)
    {
            this.fltscan_id_usuario = Convert.ToString(Session["fltscan_id_usuario"]);  //Request.QueryString["chk2_Id_usuario"] == null ? "" : Request.QueryString["chk2_Id_usuario"].ToString().Trim();
            this.fltscan_rol_usuario = Convert.ToString(Session["fltscan_rol_usuario"]); //Request.QueryString["chk2_rol_usuario"] == null ? "" : Request.QueryString["chk2_rol_usuario"].ToString().Trim();
            
        //para poder utilizar Ajax en cada página donde lo vayas a usar debes
        //poner la siguiente declaración: en donde el nombre de la clase debe corresponder con el tipo.
            Ajax.Utility.RegisterTypeForAjax(typeof(fltscan_r_escaneo));

        if (!IsPostBack)
        {
            this.objDB = new ConexionBDchkDos(this.CadenaConexion);            
        }
    }


    [Ajax.AjaxMethod()]
    //fltscan_r_escaneo.RegistrarSalida(id_agencia,vin,RegistraSalida_CallBack);

    public DataSet RegistrarSalida(string id_agencia, string vin,string id_usuario)
    {
        string Q = "";
        DataSet res = new DataSet();          
        DataTable dt = new DataTable();        
        dt.Columns.Add("mensaje");
        dt.Columns.Add("vin");

        this.fltscan_id_usuario = id_usuario; 
            
        this.objDB = new ConexionBDchkDos(this.CadenaConexion);

        string id_maquina = objDB.ConsultaUnSoloCampo("Select id_maquina from SICOPCONFIGXMAQUINA where numero_sucursal='" + id_agencia.Trim() + "'");
        string ValidaInstalacionGPS = objDB.ConsultaUnSoloCampo("Select ValidaInstalacionGPS from SICOPCONFIGXMAQUINA where id_maquina=" + id_maquina.Trim());

                #region [20210513] Inicia Validacion de retiro del GPS
                if (ValidaInstalacionGPS.ToUpper().Trim() == "TRUE")
                {
                    string resultadoValidacionGPS = ValidaRetiroGPS(vin.Trim(), id_agencia);
                    if (resultadoValidacionGPS.Trim() == "UNIDAD CON GPS")
                    {
                        DataRow reg = dt.NewRow(); 
                        resultadoValidacionGPS = "La unidad: " + vin.Trim() + " aún tiene instalado el dispositivo GPS. Realice el procedimiento de retiro del dispositivo GPS. La fecha de salida NO HA SIDO REGISTRADA";
                        reg["mensaje"] = resultadoValidacionGPS;
                        reg["vin"] = vin;
                        dt.Rows.Add(reg);
                        res.Tables.Add(dt); 
                        return res;                    
                    }
                }
                #endregion

                #region [20200917] Inicia Validacion Impresion de Salida
                string resultadoValidacion = ValidaImpresionSalida(vin.Trim(),id_agencia);
                    if (resultadoValidacion.IndexOf("YA SE IMPRIMIO LA SALIDA EN")== -1) 
                    {
                        DataRow reg = dt.NewRow();                         
                        reg["mensaje"] = resultadoValidacion.Trim();
                        reg["vin"] = vin;
                        dt.Rows.Add(reg);
                        res.Tables.Add(dt); 
                        return res;                    
                    }
                #endregion

                string NombreUsuario = this.objDB.ConsultaUnSoloCampo("Select nombre from SCAN_USR where id_usr=" + this.fltscan_id_usuario);  
                string quien = "id_maquina=Sistema FlotillaWEB " + this.fltscan_id_usuario  + " " + NombreUsuario.Trim();
                
                Q = " Insert into SICOP_BITACORA (fecha, que, quien, aquien,id_agencia,centralizado)";
                Q += " values (getdate(),'Actualizacion exitosa! Id Prospecto:','" + quien.Trim() + "','" + vin.Trim() + "','" + id_agencia + "','True')";
                Q += " Select @@IDENTITY";
                                              
                string regafectados = this.objDB.ConsultaUnSoloCampo(Q).Trim(); 
                if (regafectados == "")
                {
                        DataRow reg = dt.NewRow();                         
                        reg["mensaje"] = "Sucedio un error la Salida no ha quedado registrada!!!";
                        reg["vin"] = vin;    
                        dt.Rows.Add(reg);
                        res.Tables.Add(dt); 
                        return res;                    
                }
                else
                {
                    Q = "Select distinct Convert(char(10),fecha,103) as fecha, ";
                    Q += " Convert(char(10),fecha,108) as hora,";
                    Q += " quien, que, aquien, aquien as vin, substring(que,patindex('%!%',que)+1,len(que)) as idsicop, ";
                    Q += " isnull(factura,'') as factura, isnull(tipo_venta,'') as tipo_venta, isnull(tipo_auto,'') as tipo_auto, ";
                    Q += " b.id_agencia,";
                    Q += " alias as nombre_agencia,";
                    Q += " '' as mensaje";
                    Q += " FROM SICOP_BITACORA b, AGENCIAS a where ";
                    Q += " b.id_agencia = a.id_agencia";
                    Q += " and id_bitacora > 0 ";
                    Q += " and aquien  = '" + vin.Trim() + "'";
                    Q += " order by 1";
                }

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
                this.objAdapter.Fill(res, "Salidas");
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
        return res;
    }

   
    /// <summary>
    ///     Consulta todos los empleados que administra un determinado id_usuario
    /// </summary>
    /// <param name="id_usuario"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    //(id_usuario,ConsultaEmpleados_CallBack);
    public DataSet ConsultaSucursales(string id_usuario)
    {
        DataSet resultados = new DataSet();
        string Q = "";

        this.objDB = new ConexionBDchkDos(this.CadenaConexion);

        Q = "Select distinct id_agencia as valor, alias as texto FROM Agencias where id_agencia in (select id_agencia from SCAN_USR_AGENCIAS where id_usr=" + id_usuario.Trim() + ") order by 2";
                
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
                this.objAdapter.Fill(resultados, "EMPLEADOS");
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


    //20210513
    public string ValidaRetiroGPS(string CodigoLeido, string id_agencia)
    {
        string res = "";

        try
        {
                if (this.ConnectionStringGPS.Trim() != "")
                {
                    try
                    {                        
                        try
                        {
                            System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection();

                            if (sqlConnection1.State.ToString().ToUpper().Trim() != "OPEN")
                            {
                                sqlConnection1.ConnectionString = this.ConnectionStringGPS.Trim();
                                sqlConnection1.Open();
                            }

                            string spq = "gps.SEL_GPS_VALIDA_INSTALACION_SP";
                            //spq += " '" + CodigoEscaneado.Trim() + "',1,'0'"; 

                            SqlCommand sqlCommand1 = new SqlCommand(spq, sqlConnection1);
                            sqlCommand1.CommandType = CommandType.StoredProcedure;

                            SqlParameter param = new SqlParameter("@vin", SqlDbType.VarChar, 17);
                            param.Direction = ParameterDirection.Input;
                            param.Value = CodigoLeido.Trim();
                            sqlCommand1.Parameters.Add(param);

                            SqlParameter param1 = new SqlParameter("@idUsuario", SqlDbType.Int, 4);
                            param1.Direction = ParameterDirection.Input;
                            param1.Value = 1;
                            sqlCommand1.Parameters.Add(param1);

                            SqlParameter param2 = new SqlParameter("@err", SqlDbType.VarChar, 250);
                            param2.Direction = ParameterDirection.Output;
                            param2.Value = "0";
                            sqlCommand1.Parameters.Add(param2);

                            SqlDataReader reader = sqlCommand1.ExecuteReader();
                            if (reader.HasRows)
                            {
                                int contador = 0;
                                while (reader.Read())
                                {//la unidad con vin XXXXX  aun tiene instalado el GPS ?
                                    if (reader[0].ToString().ToUpper().Trim() == "FALSE")
                                    {
                                        res = "UNIDAD SIN GPS";
                                    }
                                    if (reader[0].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        res = "UNIDAD CON GPS";
                                    }

                                    Debug.WriteLine(reader[0].ToString().Trim());
                                    contador++;
                                }
                            }
                            reader.Close();
                            sqlCommand1.Dispose();
                            sqlConnection1.Close();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            //Utilerias.WriteToLog(ex.Message, "ValidaRetiroGPS", Application.StartupPath + "\\Log.txt");
                        }

                    }
                    catch (Exception ex1)
                    {
                        Debug.WriteLine(ex1.Message);
                        //Utilerias.WriteToLog(ex1.Message, "ValidaRetiroGPS_1", Application.StartupPath + "\\Log.txt");
                    }
                }            
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            //Utilerias.WriteToLog(ex.Message, "ValidaRetiroGPS_2", Application.StartupPath + "\\Log.txt");
        }

        return res;
    }


    public string ValidaImpresionSalida(string CodigoLeido, string NumeroSucursal)
    {
        string res = "";
        string Q = "";

        try
        {
            //primero analizamos la cadena capturada y si tiene el formato requerido la parseamos.
            string CodigoBarras = CodigoLeido.Trim();
            if (CodigoBarras.Length > 0)
            {
                string vin = CodigoLeido.Trim();
                string strconexionABussinesPro = "";
                //Consultamos los datos para poder firmarnos en la base de datos.
                #region Consulta de los datos para el Logueo en el Servidor Remoto

                //conociendo el id_agencia procedemos a consultar los datos de conexion en la tabla transferencia
                Q = "Select ip,usr_bd,pass_bd,nombre_bd,bd_alterna, dir_remoto_xml, dir_remoto_pdf,usr_remoto,pass_remoto, ip_almacen_archivos, smtpserverhost, smtpport, usrcredential, usrpassword ";
                Q += " From SICOP_TRASMISION where id_agencia='" + NumeroSucursal + "'";

                DataSet ds = this.objDB.Consulta(Q);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow regConexion = ds.Tables[0].Rows[0];
                    strconexionABussinesPro = string.Format("Data Source={0};Initial Catalog={1}; Persist Security Info=True; User ID={2};Password={3}", regConexion["ip"].ToString(), regConexion["bd_alterna"].ToString(), regConexion["usr_bd"].ToString(), regConexion["pass_bd"].ToString());
                }
                else
                {
                    res = "Error: No se encontraron datos para la coneccion con BPRO. numero sucursal: " + NumeroSucursal.Trim();
                    return res;
                }
                //this.objDB = null; 
                if (strconexionABussinesPro.Trim() != "")
                {
                    ConexionBDchkDos objBDBP = new ConexionBDchkDos(strconexionABussinesPro);
                    Q = "Select VEH_SITUACION FROM SER_VEHICULO WHERE VEH_NUMSERIE = '" + CodigoLeido.Trim() + "'";
                    string situacion = objBDBP.ConsultaUnSoloCampo(Q);
                    if (situacion.Trim() != "")
                    {
                        string fechasalida = "";
                        if (situacion.Substring(0, 1).ToUpper() == "S")
                        { //Se trata de un seminuevo. 3N1AB7AD3GL627235
                            Q = "select PMS_FECHAENTREGA from USN_PEDIDO where PMS_NUMSERIE =  '" + CodigoLeido.Trim() + "'";
                            Q += "and PMS_STATUS = 'I'";
                            fechasalida = objBDBP.ConsultaUnSoloCampo(Q).Trim();
                        }
                        else
                        { //Se trata de un nuevo.  3N1CN7AD5KK414950
                            Q = "Select PEN_FECHAENTREGA from UNI_PEDIUNI where PEN_NUMSERIE =  '" + CodigoLeido.Trim() + "'  and PEN_STATUS = 'I'";
                            fechasalida = objBDBP.ConsultaUnSoloCampo(Q).Trim();
                        }

                        if (fechasalida.Trim() != "")
                        {
                            res = "YA SE IMPRIMIO LA SALIDA EN : " + fechasalida.Trim();
                        }
                        else
                        {
                            res = "NO SE HA IMPRESO LA HOJA DE SALIDA";
                        }
                    }
                    else
                    {
                        res = "LA UNIDAD NO SE ENCUENTRA EN EL INVENTARIO DE ESTA SUCURSAL. NO SE REGISTRA SALIDA";
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            //Utilerias.WriteToLog(ex.Message, "ValidaImpresionSalida", Application.StartupPath + "\\Log.txt");
        }

        return res;
    }



}
