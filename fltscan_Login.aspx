<%@ Page Language="C#" AutoEventWireup="true" Inherits="fltscan_Login" EnableSessionState="True" CodeFile="fltscan_Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>.: ESCANEO UNIDADES WEB :.</title>
    <link href ="Estilos/Style.css"  rel="stylesheet" type="text/css" />
    <style>
        .body{background: url("Imagenes/fondos.png") fixed}
    </style>
</head>
<form id=form runat=server action="" method=post>
<body class="body">    
    <div>
    <table border="0" cellpadding=0 cellspacing = 0 width=100%> 
       <tr>
         <td align=left> 
             <img src="Imagenes/image2993.png" />         
         </td>
           
          <td align=right> &nbsp; </td>
       </tr>  
      
       <tr><td align="center" colspan ="2"><font size=4 style="font-weight:bold" color=003366 face=verdana>REGISTRO DE SALIDA DE UNIDADES</font> </td></tr>
       <tr><td><br/></td></tr>  
       <tr><td><br/></td></tr> 
         
       <tr >
       <td >
        <table border=0 align=center cellpadding=0 cellspacing=0 width=100%>
             
             <tr><td align="right"><font class="ETIQUETA" style="color:#808080" >*Password</font></td><td><input type="password" class="TextBox" name="pas" maxlength="20" value="" ONKEYPRESS="if ((event.keyCode == 13)) enviar();"/></td> </tr>
             <tr><td><br/></td></tr>             
             <tr><td><br/></td></tr>
             <TR>
                <TD Colspan=2 Align=middle>
                    <img src="Imagenes/b_Entrar.gif"  onclick="enviar()">
                    <input type="hidden" name="hIntentos" id="hIntentos" value="0" />
                    &nbsp;<a class="liga" id="anclaRecuperarP" name="anclaRecuperarP" href = "pcRecuperarContrasena.aspx" style="display:none";>No recuerdo mi Login o Password deseo recuperarlos</a>
                </TD>
            </TR>
            <tr><td><br /></td></tr>
            <!--
            <tr><td colspan=3><font style="size:15px; color:#000000; font-family:Verdana; font-weight:bold;" >Si no cuentas con Login y password, regístrate y obtenlos, es gratuito y rápido.</font></td></tr>
            <TR>
                <TD Colspan=3 Align=middle>
                    <img src="Imagenes/b_registrar.gif" style="cursor:hand" onclick="registrar()" title="Registrarme en el portal">
                </TD>
            </TR>
            -->
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
            <tr><td align=center><font size=1 color=003366 face=verdana>Accesos desde 24/01/2012</font></td></tr>
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
                    <td><img src="Imagenes/DIGITOS/ANIMADOS/<%=siguiente.Substring(7,1)%>_a.gif"> </td>
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
          //ConsultaUsuarios();

        function ConsultaUsuarios()
        {        
           fltscan_Login.ConsultaUsuarios(ConsultaUsuarios_CallBack);
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
   var url = "Inicio.aspx?Accion=Registro";  
   window.location=url;
   }

  function enviar()
    {          
       var password = document.all.pas.value;
       if (FncisEmpty(password))
          {
          alert(" Proporcione el campo password para poder continuar");
          return;
          }
        else{
               
         var cad = password;
         var intentos = parseInt(document.all.hIntentos.value,10);
         intentos = intentos+1;
         document.all.hIntentos.value=intentos;                  
          fltscan_Login.Valida(cad,enviar_CallBack);
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
                var nombreusuario = ds.Tables[0].Rows[0].nombreusuario;
                var url="";
    
            if (mensaje.indexOf("encontrado")>-1)
             {                                  
                url = "fltscan_Inicio.aspx?fltscan_Id_usuario=" + id_usuario + "&fltscan_rol_usuario=" + rol + "&fltscan_nombre_usuario=" + nombreusuario;
				//alert(url);
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