<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sicop_SeleccionaAgencia.aspx.cs" Inherits="PortalClientes.sicop_SeleccionaAgencia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>.:INTERFACE SICOP vs BPro:.</title>
        <link href ="Estilos/Style.css"  rel="stylesheet" type="text/css" />
    <style>
        .body{background: url("Imagenes/fondo.bmp") fixed}
    </style>
</head>
<body class="body">
    <form id="form1" runat="server">
    <div>
       <table cellpadding=0 cellspacing=0 width=100%>           
        <tr>
         <td colspan=2 align=left> 
             <img src="Imagenes/image2993.png" />         
         </td>
          <td align=right> &nbsp; 
          <input type=hidden name=hid_usuario id=hid_usuario value="<%=id_usuario%>" />
          <input type=hidden name=hAccion id=hAccion value="<%=Accion%>" />
          <input type=hidden name=hrol_usuario id=hrol_usuario value="<%=rol%>" />
          <input type=hidden name=hnombre_usuario id=hnombre_usuario value="<%=nombre_usuario%>" />
          </td>
       </tr>  
      
       <tr><td><br/></td></tr>
       <tr><td><br/></td></tr>  
       <tr><td><br/></td></tr>
       <tr>
         <td align=center>
           <table cellpadding=0 cellspacing=0>
          <tr>
            <td><font class="ETIQUETA"  style="color:#0000FF" >AGENCIAS REGISTRADAS</font></td>
          </tr>
          <tr>  
            <td>                              
                              <select class="SelectBox" name="cboAgencias" id="cboAgencias" onchanges="Deselecciona2(this);">
                                                              
                              </select>  
            </td>            
          </tr>
            
           </table>         
         </td>             
       </tr>       
       <tr><td><br/></td></tr>
       <tr><td><br/></td></tr>  
    
            <tr><td><input type=button name=cmdEnviar id=cmdEnviar onclick="Entrar();" class="toolbutton" value="Entrar"</td></tr>          
          
       </table>
    </div>
    </form>
</body>
<script language="javascript">   
        ConsultaAgencias();

        function ConsultaAgencias()
        {        
           var id_usuario=document.all.hid_usuario.value;
           sicop_SeleccionaAgencia.ConsultaAgencias(id_usuario,ConsultaAgencias_CallBack);
        }
        
        function ConsultaAgencias_CallBack(response)
          {
            var ds = response.value;            
              if(ds != null && typeof(ds) == "object" && ds.Tables != null){
			            for(var i=0; i<ds.Tables[0].Rows.length; i++){                                      
                                 if (i==0){
                                   document.all.cboAgencias.length=0;                       
                                   var option0 = new Option("  ------Seleccione la Agencia ------  ","0");                       
                                   eval("document.all.cboAgencias.options[document.all.cboAgencias.length]=option0");                       
                                   }
                               var option0 = new Option(ds.Tables[0].Rows[i].texto,ds.Tables[0].Rows[i].valor);                                      
                               eval("document.all.cboAgencias.options[document.all.cboAgencias.length]=option0");                   
                               }                   
                    }  
          }
        
        function Entrar()
        {
           var id_agencia=document.all.cboAgencias.value;
           var id_usuario = document.all.hid_usuario.value;
           var rol = document.all.hrol_usuario.value;
           var accion = document.all.hAccion.value;
           var nombre_usuario=document.all.hnombre_usuario.value; 
                                
           if (id_agencia=="0")
             {
               alert("Seleccione una agencia");
               return;
             }
             
           var url = "sicop_Inicio.aspx?sicop_Id_usuario=" + id_usuario + "&sicop_rol_usuario=" + rol + "&sicop_id_agencia=" + id_agencia + "&sicop_nombre_usuario=" + nombre_usuario;                                                
           //alert(url);
           window.location=url;             
        }
        
</script>
</html>
