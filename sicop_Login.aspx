<%@ Page Language="C#" AutoEventWireup="true" Inherits="sicop_Login" EnableSessionState="True" CodeFile="sicop_Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>.: INTERFAZ SICOP vs BPro :.</title>
    <link href ="Estilos/Style.css"  rel="stylesheet" type="text/css" />
    <style>
        .body{background: url("Imagenes/backgrnd_hoja.gif") fixed}
    </style>
</head>
<form id=form runat=server action="" method=post>
<body class="body">    
    <div>
    <table border="0" cellpadding=0 cellspacing = 0 width=100%> 
       <tr>
         <td colspan=2 align=left> 
             <img src="Imagenes/image2993.png" />         
         </td>
          <td align=right> &nbsp; </td>
       </tr>  
      
       <tr><td><br/></td></tr>
       <tr><td><br/></td></tr>  
       <tr><td><br/></td></tr> 
         
       <tr >
       <td >
        <table border=0 align=center cellpadding=0 cellspacing=0 width=100%>
             <tr><td align="right"><font class="ETIQUETA"  style="color:#0000FF" >*Usuario</font></td><td>
                              <select class="SelectBox" name="cboUsuarios" id="cboUsuarios" onchanges="Deselecciona2(this);">
                                                              
                              </select>                              
                                </td></tr>
             <tr><td><br/></td></tr>
             <tr><td align="right"><font class="ETIQUETA" style="color:#0000FF" >*Password</font></td><td><input type="password" class="TextBox" name="pas" maxlength="20" value="" ONKEYPRESS="if ((event.keyCode == 13)) enviar();"/></td> </tr>
             <tr><td><br/></td></tr>             
             <tr><td><br/></td></tr>
             <TR>
                <TD Colspan=2 Align=middle>
                    <img src="Imagenes/b_Entrar.gif" style="cursor:hand" onclick="enviar()">
                    <input type="hidden" name="hIntentos" id="hIntentos" value="0" />
                    &nbsp;<a class="liga" id="anclaRecuperarP" name="anclaRecuperarP" href = "pcRecuperarContrasena.aspx" style="display:none";>No recuerdo mi Login o Password deseo recuperarlos</a>
                </TD>
            </TR>
            <tr><td><br /></td></tr>
        </table>                      
       </td>
       </tr> 
       
       <TR>
    <TD width=50% align=left>
    <table border=0 cellpadding=0 cellspacing=0>
       <tr>
         <td><font size=1 color=003366 face=verdana>Cualquier duda o comentario comunicate con nosotros</font></td>
       </tr>
       <tr>
         <td><font size=1 color=003366 face=verdana>Teléfono: 10857019</font></td>
       </tr>
       <tr>
         <td><font size=1 color=003366 face=verdana>Correo electrónico: ignacio.jimenez@grupoandrade.com.mx</font></td>
       </tr>
    </table>
    </TD>
    <td valign=top>
      <table width=50% cellpadding=0 cellspacing=0 border=0>
        <tr>		  
          <td align=left>
          <table width=100% cellpadding=0 cellspacing=0 border=0 ID="Table1">
            <tr><td></td><tr>            
            <tr><td align=center><font size=1 color=003366 face=verdana>Accesos desde 01/04/2013</font></td></tr>
            <tr><td><br></td></tr>
            <tr><td align=center>
                <table cellpadding=0 cellspacing=0 border=0>
                  <tr>
                    <td><img src="Imagenes/DIGITOS/ANIMADOS/<%=siguiente.Substring(0,1)%>.gif"> </td>
                    <td><img src="Imagenes/DIGITOS/ANIMADOS/<%=siguiente.Substring(1,1)%>.gif"> </td>
                    <td><img src="Imagenes/DIGITOS/ANIMADOS/<%=siguiente.Substring(2,1)%>.gif"> </td>
                    <td><img src="Imagenes/DIGITOS/ANIMADOS/<%=siguiente.Substring(3,1)%>.gif"> </td>
					<td><img src="Imagenes/DIGITOS/ANIMADOS/<%=siguiente.Substring(4,1)%>.gif"> </td>
                    <td><img src="Imagenes/DIGITOS/ANIMADOS/<%=siguiente.Substring(5,1)%>.gif"> </td>
                    <td><img src="Imagenes/DIGITOS/ANIMADOS/<%=siguiente.Substring(6,1)%>.gif"> </td>
                    <td><img src="Imagenes/DIGITOS/ANIMADOS/<%=siguiente.Substring(7,1)%>.gif"> </td>
                  </tr>
                </table>
                </td></tr>
            <tr><td><font size=1 color=f0f0f0>&nbsp;</font></td></tr>
          </table>
          </td>
          <!--<td align=right><img src="pics/conteni_img.gif"></td>-->
        </tr>
      </table>
    </td>
  </TR>
             
    </table>    
    </div> 
</body>
</html>
<script language="javascript">   
          ConsultaUsuarios();

        function ConsultaUsuarios()
        {        
           sicop_Login.ConsultaUsuarios(ConsultaUsuarios_CallBack);
        }
        
        function ConsultaUsuarios_CallBack(response)
          {
            var ds = response.value;            
              if(ds != null && typeof(ds) == "object" && ds.Tables != null){
			            for(var i=0; i<ds.Tables[0].Rows.length; i++){                                      
                                 if (i==0){
                                   document.all.cboUsuarios.length=0;                       
                                   var option0 = new Option("  ------Seleccione su Usuario ------  ","0");                       
                                   eval("document.all.cboUsuarios.options[document.all.cboUsuarios.length]=option0");                       
                                   }
                               var option0 = new Option(ds.Tables[0].Rows[i].texto,ds.Tables[0].Rows[i].valor);                                      
                               eval("document.all.cboUsuarios.options[document.all.cboUsuarios.length]=option0");                   
                               }                   
                    }  
          }
        

   
   
   
   function registrar()
   { 
   var url = "sicop_Login.aspx?Accion=Registro";  
   window.location=url;
   }

  function enviar()
    {     
       var login = document.all.cboUsuarios.value;
       var password = document.all.pas.value;
       if (FncisEmpty(login) || FncisEmpty(password))
          {
          alert(" Proporcione ambos campos usuario y password");
          return;
          }
        else{
            if (login=="0")
              {
              alert(" Seleccione su Usuario");
               return;
              }
                
         var cad = login + "|" + password;
         var intentos = parseInt(document.all.hIntentos.value,10);
         intentos = intentos+1;
          document.all.hIntentos.value=intentos;                  
          sicop_Login.Valida(cad,enviar_CallBack);
        }         
    }
    
    function enviar_CallBack(response)
    {
         var ds = response.value;    
        
         if(ds != null && typeof(ds) == "object" && ds.Tables != null)
           { 
            var mensaje = ds.Tables[0].Rows[0].mensaje;              
                var id_usuario=ds.Tables[0].Rows[0].id_usuario;
                var rol = ds.Tables[0].Rows[0].rol;
                var nombre_usuario=ds.Tables[0].Rows[0].nombre_usuario;
                var url="";
                var id_agencia="";
                
            if (mensaje.indexOf("encontrado")>-1)
             {                                 
                if (mensaje.indexOf("variasagencias")>-1)
                   id_agencia="variasagencias";
                else
                   id_agencia=mensaje.substring(mensaje.indexOf("|")+1);    
                
                if (id_agencia=="variasagencias")
                  url = "sicop_SeleccionaAgencia.aspx?sicop_Id_usuario=" + id_usuario + "&sicop_rol_usuario=" + rol + "&sicop_id_agencia=" + id_agencia + "&sicop_nombre_usuario=" + nombre_usuario; 
                else   
                  url = "sicop_Inicio.aspx?sicop_Id_usuario=" + id_usuario + "&sicop_rol_usuario=" + rol + "&sicop_id_agencia=" + id_agencia + "&sicop_nombre_usuario=" + nombre_usuario;                                
                
                window.location=url;
             }
            else{                
                 if (mensaje=="No se encontró un usuario con los datos proporcionados.")
                    {
                    var intentos = parseInt(document.all.hIntentos.value,10);
                        if (intentos==3)
                           {
                           document.all.anclaRecuperarP.style.display="";
                           }
                    }
                    alert(mensaje);
                }
            }            
    }
     
   function FncisEmpty(s)
   {
      return ((s == null) || (s.length == 0))
   }
</script>

</form>