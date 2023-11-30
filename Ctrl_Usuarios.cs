//****************ENCABEZADO************** 
//Programa: Ctrl_Usuarios.vb 
//Desarrollador: Iris Gómez Gómez 
//Fecha: 
//Descripción: Clase que actua como clase de negocio para la interfaz de usuarios y su correspondiente 
// en clase a acceso a datos 
//**************************************** 

using Microsoft.VisualBasic;
using System.Data;


/// <summary> 
/// ''' Clase Controladora que sirve como conexion entre la interfaz y el acceso a datos 
/// </summary> 
/// <remarks></remarks> 
/// 

public class Ctrl_Usuarios
{

    Usuarios_Dao Usuarios_Dao = new Usuarios_Dao();
    Password Psw = new Password();

    public string PrcObtenerMd5(string Hash)
    {
        Password Password = new Password();
        return Password.PrcObtenerMd5(Hash);
    }

    public object FncVerificaLogIn(string Usuario, string Password)
    {
        Usuarios Usr = new Usuarios(Usuario, Password);
        return Usr;
    }

    public bool FncRegistraUsr(string Nombre, string APaterno, string AMaterno, string Correo, string Login, string Contraseña, string Pregunta, string Respuesta, string Activo)
    {
        return Usuarios_Dao.FncRegistraUsr(Nombre, APaterno, AMaterno, Correo, Login, Contraseña, Pregunta, Respuesta, Activo);
    }

    public DataSet FncGetUsuarios()
    {
        return Usuarios_Dao.FncGetUsuarios();
    }

  

    public bool FncActualizaUsr(Usuarios Usuario)
    {
        return Usuarios_Dao.FncActualizaUsr(Usuario);
    } 

    public DataSet FncGetUsuariosbyNombre(string Nombre)
    {
        return Usuarios_Dao.FncGetUsuariosbyNombre(Nombre);
    }

    public DataSet FncGetUsuariosbyAPaterno(string APaterno)
    {
        return Usuarios_Dao.FncGetUsuariosbyAPaterno(APaterno);
    }

    public DataSet FncGetUsuariosbyAMaterno(string AMaterno)
    {
        return Usuarios_Dao.FncGetUsuariosbyAMaterno(AMaterno);
    }

    public DataSet FncGetUsuariosbyTodos()
    {
        return Usuarios_Dao.FncGetUsuariosbyTodos();
    }

    public DataSet FncGetAccesosbyPerfil(string Id_Perfil)
    {
        return Usuarios_Dao.FncGetAccesosbyPerfil(Id_Perfil);
    }

    public bool FncRecupContraseña(string Usuario, string Correo)
    {
        return Usuarios_Dao.FncRecupContraseña(Usuario, Correo);
    }

    public DataSet FncGetPregResp(string Usuario, string Correo)
    {
        return Usuarios_Dao.FncGetPregResp(Usuario, Correo);
    }

    public DataSet FncGetUsrCont(string Pregunta, string Respuesta)
    {
        return Usuarios_Dao.FncGetUsrCont(Pregunta, Respuesta);
    }


    public DataSet FncGetUsrById(int Id_Usuario)
    {
        return Usuarios_Dao.FncGetUsrById(Id_Usuario);
    }

    public bool FncActualizaCont(int Id_Usuario, string Contraseña)
    {
        return Usuarios_Dao.FncActualizaCont(Id_Usuario, Contraseña);
    }

    public bool PrcValidaUsr(string Usuario)
    {
        return Usuarios_Dao.PrcValidaUsr(Usuario);
    }

    public bool PrcValidaUsrCom(string Nombre, string ApelPat, string ApelMat, string Usuario)
    {
        return Usuarios_Dao.PrcValidaUsrComp(Nombre, ApelPat, ApelMat, Usuario);
    }

    public DataTable FncGetPermUsr(int Id_Usuario)
    {
        return Usuarios_Dao.FncGetPermisosUsr(Id_Usuario);
    }

    public string FncGeneraPass(Usuarios Usuario)
    {
        return Psw.FncGeneraPass(Usuario);
    }

    public DataSet FncGetUsrByPerfil(string Id_Perfil)
    {
        return Usuarios_Dao.FncGetUsrByPerfil(Id_Perfil);
    }

    public DataSet FncGetUsrbyTodosbyPerf()
    {
        return Usuarios_Dao.FncGetUsrbyTodosbyPerf();
    }

    public DataSet FncGetUsuariosbyNombreT(string Nombre)
    {
        return Usuarios_Dao.FncGetUsuariosbyNombreT(Nombre);
    }

    public DataSet FncGetUsuariosbyAPaternoT(string APaterno)
    {
        return Usuarios_Dao.FncGetUsuariosbyAPaternoT(APaterno);
    }

    public DataSet FncGetUsuariosbyAMaternoT(string AMaterno)
    {
        return Usuarios_Dao.FncGetUsuariosbyAMaternoT(AMaterno);
    }

    public bool FncEliminaUsrPer(int Id_Usuario)
    {
        return Usuarios_Dao.FncEliminaUsrPer(Id_Usuario);
    }

    public bool FncActualizaUsrPerf(Usuarios Usuario)
    {
        return Usuarios_Dao.FncActualizaUsrPer(Usuario);
    }

    public bool FncInsertaUsrPerf(Usuarios Usuario)
    {
        return Usuarios_Dao.FncInsertaUsrPerf(Usuario);
    }

    public bool FncDeleteUsuarioPerf(int Id_Usuario)
    {
        return Usuarios_Dao.FncDeleteUsuarioPerf(Id_Usuario);
    }

    public bool FncVerificaPerfUsr(int Id_Usuario)
    {
        return Usuarios_Dao.FncVerificaPerfUsr(Id_Usuario);
    }

    public DataSet FncGetUsrbywithPerf()
    {
        return Usuarios_Dao.FncGetUsrbywithPerf();
    }

    public DataSet FncGetUsuariosbyNombreB(string Nombre)
    {
        return Usuarios_Dao.FncGetUsuariosbyNombreB(Nombre);
    }

    public DataSet FncGetUsuariosbyAPaternoB(string APaterno)
    {
        return Usuarios_Dao.FncGetUsuariosbyAPaternoB(APaterno);
    }

    public DataSet FncGetUsuariosbyAMaternoB(string AMaterno)
    {
        return Usuarios_Dao.FncGetUsuariosbyAMaternoB(AMaterno);
    }

} 

