<%@ Page Language="C#" MasterPageFile="~/fltscan_Principal.master" EnableSessionState="True" AutoEventWireup="true" CodeFile="fltscan_r_escaneo.aspx.cs" Title="REGISTRO DE SALIDA DE UNIDADES" Inherits="fltscan_r_escaneo" %>
<%@ Import Namespace="System.Messaging" %> 

<asp:Content ID="ContentPlaceHolder2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server"> 
        				        
<!--
/********************************************ENCABEZADO**************************************************************
 * Programa:    chk2_r_t_asistencia.aspx
 * Desarrollor: Luis Bonnet
 * Fecha:       20120123
 * Descripcion: pagina que se encarga de consultar en chk2 la TARJETA DE ASISTENCIA DE LOS EMPLEADOS SELECCIONADOS
 *              
 * 20120123 16:45 
*********************************************************************************************************************/
-->
<script language="JavaScript" src="../Scripts/number-functions.js" type="text/javascript"></script>
<script language="JavaScript" src="../Scripts/Calendar/calendar.js" type="text/javascript"></script> 
<script LANGUAGE="JavaScript" SRC="../Scripts/Calendar/IsDate.js"></script>

<html>
  <head>
         <title>.: REGISTRO DE SALIDA DE UNIDADES :. </title>
        <link href ="Estilos/Style.css"  rel="stylesheet" type="text/css" />        
    <style>
        .body{background: url("Imagenes/andradedifuso5s.png") fixed}
    </style>

  </head>
 <body class="body">
 <form id=form1 runat=server> <!-- si no esta todo contenido en un form que corra en el servidor el Ajax no funciona-->
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
     <tr>
       <td colspan=2 align="center"><h3>REGISTRO DE SALIDAS DE UNIDADES</h3></td>
       <td> <input type="hidden" id="hid_usuario" name="hid_usuario" value="<%=fltscan_id_usuario%>"/> </td>
     </tr> 
     <tr>
                           <td align="left">  
                             <font class = "ETIQUETA">SELECCIONE LA SUCURSAL DONDE SE ENCUENTRA LA UNIDAD A REGISTRAR:</font>
                          </td> 
                          
                           <td align="left">  

                          </td>                               
     </tr>    
     <tr>
       <td align=left>
                              <select class="SelectBox" name="cboSucursales" id="cboSucursales" onchange="Deselecciona2(this);">
                                                              
                              </select>
       </td>     
       <td align=left>

       </td>
     </tr>  
     
   <tr>
     <td>
       <table cellpadding=0 cellspacing=0 width=100% border="0">       
       <tr class="titulotabla">
						<td class="fondotabla" align="left"><font class = "ETIQUETA"> *Proporcione el Número de Serie de la unidad a registrar su salida:</font></td>
	   </tr>
       <tr class="titulotabla">	
           <td class="fondotabla" align="left">
                            <input id="txtVIN" style="WIDTH: 200px; HEIGHT: 20px" size=17 class="textogris" maxlength="17" name="txtVIN" value="" />
						</td>
<!--
						<td class="fondotabla" align="right">*FechaFinal:</td>
						<td class="fondotabla" align="left">
						<INPUT id=txtFechFin style="WIDTH: 72px; HEIGHT: 20px" size=7 class="textogris" maxLength="10" name=txtFechFin value="" onBlur="validaFecha(this)" onkeyup="vbscript:agregaSlash(txtFechFin)" >
						<A onClick="javascript:setDateField(txtFechFin);top.newWin=window.open('../Scripts/calendar/calendar.html','cal','WIDTH=208,HEIGHT=230'); doNothing();"><IMG style="LEFT: 1px; TOP: 43px; cursor:hand;" height=16 src="../imagenes/calendar.gif" width=16 align=bottom  border=0  X-CLARIS-USEIMAGEHEIGHT X-CLARIS-USEIMAGEWIDTH></A>
						</td>

    -->
					</tr>
					<tr class="titulotabla" >
                        <td><br /></td>
					</tr>
       </table> 
       </td>
    <td align=left valign=top>

    </td>
    </tr>
               
     <tr><td><input type="hidden" name="hidagenciacliente" value="" /> <input type="hidden" name="hagenciacliente" value="" /><br /></td></tr>        
     <tr>
       <td colspan=2 align="center">                 
                    <input id="cmdRegistrar" type="button" title="Registra la salida de la unidad" value="REGISTRAR" onclick="RegistrarSalida();"  />                                 
		</td>
     </tr>

     <tr  bgcolor="silver"><td colspan=2 height="1px;"><img src="Imagenes\spacer.gif" width="1px;" height="1px;" /></td></tr>
     <tr><td><br/></td></tr>
     <tr><td colspan=2 align=left></td></tr>
     <tr ><td colspan=2 align="center">  
     <form name="formLB" id="formLB" method="post">
                         <!-- En este layer es donde se mostrarán los resultados de la interaccion con el servidor mediante Ajax-->
   					<div id="Etiqueta" style="width:800px;" align="center" >

					</div>     
     </form>    					
     </td></tr>
    </table> 
    
    </form>          
 </body>
</html>		

<script language="vbscript">
sub agregaSlash(txtFecha)
		if len(txtFecha.value)=2 or len(txtFecha.value)=5 then	
			txtFecha.value=txtFecha.value & "/"				
		end if		
	end sub
</script>

<script type="text/javascript" language="javascript">
        ConsultaSucursales();
        //ConsultaUOs();
        //ConsultaGpos();
        //PonFechaHoy();
      

        function ConsultaGpos()
        {
           var id_usuario = document.all.hid_usuario.value;
           if (!FncisEmpty(id_usuario))
           {
           chk2_r_t_asistencia.Gpos(id_usuario,ConsultaGpos_CallBack);
           }                
        }
        
        function ConsultaGpos_CallBack(response)
        {
          var ds = response.value;
            
          if(ds != null && typeof(ds) == "object" && ds.Tables != null){
			            for(var i=0; i<ds.Tables[0].Rows.length; i++){                                      
                                 if (i==0){
                                   document.all.cboGpos.length=0;                       
                                   var option0 = new Option("  ------Seleccione un Grupo ------  ","0");                       
                                   eval("document.all.cboGpos.options[document.all.cboGpos.length]=option0");                       
                                   }
                               var option0 = new Option(ds.Tables[0].Rows[i].texto,ds.Tables[0].Rows[i].valor);                                      
                               eval("document.all.cboGpos.options[document.all.cboGpos.length]=option0");                   
                               }                   
                    }            
        }

        function ConsultaSucursales()
        {
           var id_usuario = document.all.hid_usuario.value;
           if (!FncisEmpty(id_usuario))
           {
           fltscan_r_escaneo.ConsultaSucursales(id_usuario,ConsultaSucursales_CallBack);
           }        
        }
        
        function ConsultaSucursales_CallBack(response)
          {                    
              var ds = response.value;            
              if(ds != null && typeof(ds) == "object" && ds.Tables != null){
			            for(var i=0; i<ds.Tables[0].Rows.length; i++){                                      
                                 if (i==0){
                                     document.all.cboSucursales.length = 0;
                                   var option0 = new Option("  ------Seleccione una Sucursal ------  ","0");                       
                                   eval("document.all.cboSucursales.options[document.all.cboSucursales.length]=option0");
                                   }
                               var option0 = new Option(ds.Tables[0].Rows[i].texto,ds.Tables[0].Rows[i].valor);                                      
                               eval("document.all.cboSucursales.options[document.all.cboSucursales.length]=option0");
                               }                   
                    }                      
          }
                             
        
        function RegistrarSalida()
        {
                   
           var vin = "";           
           var id_usuario = document.all.hid_usuario.value;           
           var id_agencia = document.all.cboSucursales.value;                                    
           var cbosucursales = document.getElementById("cboSucursales");
                      
           var nombreagencia = cbosucursales.options[cbosucursales.selectedIndex].text;
           //alert(nombreagencia);
         
           if (id_agencia == 0)
           {
               alert("Seleccione la sucursal en donde se registrará la salida de la unidad");
               return;
           }

           vin = document.all.txtVIN.value;
           //alert(vin.length);
           //alert(vin);

           if (document.all.txtVIN.value == "" || vin.length != 17)               
            {
               alert("Proporcione un número de VIN Válido");
               return;
           }
            
           document.all.Etiqueta.innerHTML = "<font class=ETIQUETA>Registrando Salida de la unidad: " + vin + "  para la sucursal: " + id_agencia + " -- " + nombreagencia + " espere un momento...</font>&nbsp;<img src='../Imagenes/espere.gif'/>";
                    //document.all.cmdRegistrar.disabled=true;
                    fltscan_r_escaneo.RegistrarSalida(id_agencia, vin, id_usuario, RegistraSalida_CallBack);
        }
        
        
        function RegistraSalida_CallBack(response)
        {
           var ds = response.value;
           document.all.cmdRegistrar.disabled=false;
           var cadHTML = "";           

           if (ds != null && typeof (ds) == "object" && ds.Tables != null) {
               if (ds.Tables[0].Rows.length > 0) {
                   var mensaje = ds.Tables[0].Rows[0].mensaje;
                   if (mensaje != "") {                       
                       var vin = ds.Tables[0].Rows[0].vin;
                       mensaje += "\n\r" + " La salida de la unidad: " + vin + " No ha quedado Registrada!!!"
                       alert(mensaje);
                       cadHTML = "<table bgcolor='yellow' cellpadding=0 cellspacing=0>";
                       cadHTML += "<tr><td><font face=VERDANA color=000000 size=3><b> " + mensaje + "</b></font></td></tr>";
                       cadHTML += "</table>";
                       document.all.Etiqueta.innerHTML = cadHTML;
                       return;
                   }
               }
           }

            /*
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
            */


           cadHTML = "<Table width=100% cellpadding=0 cellspacing=0 border=0 >";
           //cadHTML += "<tr><td><img src='Imagenes/ExportExcel.bmp' title='Exportar a Excel' onclick=ExportaAExcel();></td></tr>"; 
           cadHTML +="<tr><td>";
           cadHTML +="<font class=ETIQUETA><b>REGISTROS DE SALIDA DE LA UNIDAD </b></font>";           
           cadHTML +="</td></tr>";
           cadHTML +="<tr><td>";
           cadHTML +="<br>";           
           cadHTML +="</td></tr>";           
           cadHTML +="<tr><td align=center>";
           cadHTML += "<Table cellpadding=0 cellspacing=0 border=0 bordercolor=000000>";
           //cadHTML +="<tr><td align=right><img src='Imagenes/sidemenutopline.gif'></td></tr>";                      
           cadHTML +="<tr><td>";
           cadHTML += "<Table width=100% cellpadding=1 cellspacing=2 border=0 bordercolor=000000>"; 
           //cadHTML +="<tr><td colspan=8 align=right><img src='Imagenes/sidemenutopline.gif'></td></tr>";                      
           //cadHTML +="<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td rowspan=100 width='1px;' align=left bgcolor='003366'><img width='1px;' src='Imagenes/sidemenurightline.gif'></td></tr>";
            //(fecha, que, quien, aquien,id_agencia,centralizado)
           cadHTML += "<tr bgcolor=Yellow>";
           cadHTML +="<td><font face=VERDANA color=003366 size=2><b>FECHA&nbsp;&nbsp;</b></td>";  
           cadHTML +="<td><font face=VERDANA color=003366 size=2><b>HORA&nbsp;&nbsp;</b></td>";  
           cadHTML +="<td><font face=VERDANA color=003366 size=2><b>USUARIO REGISTRO&nbsp;&nbsp;</b></td>";  
           cadHTML +="<td><font face=VERDANA color=003366 size=2><b>NUMERO DE SERIE&nbsp;&nbsp;</b></td>";  
           cadHTML +="<td><font face=VERDANA color=003366 size=2><b>ID AGENCIA&nbsp;&nbsp;</b></td>";
           cadHTML +="<td><font face=VERDANA color=003366 size=2><b>AGENCIA REGISTRO&nbsp;&nbsp;</b></td>";
           cadHTML +="</tr>";           

           if(ds != null && typeof(ds) == "object" && ds.Tables != null)
           {                          
             if(ds.Tables[0].Rows.length>0)
             {                
               for (var ContIndice=0;ContIndice<ds.Tables[0].Rows.length;ContIndice++)
                   {                                 
                   cadHTML +="<tr align=center bgcolor=ffffff>";
		           cadHTML +="<td>";
                   cadHTML +="<font face=VERDANA color=000000 size=1>" + ds.Tables[0].Rows[ContIndice].fecha + "</font>"; 
				   cadHTML +="</td>";
		           cadHTML +="<td>";
                   cadHTML +="<font face=VERDANA color=000000 size=1>" + ds.Tables[0].Rows[ContIndice].hora + "</font>"; 
				   cadHTML +="</td>";
				   cadHTML +="<td>";
                   cadHTML +="<font face=VERDANA color=000000 size=1>" + ds.Tables[0].Rows[ContIndice].quien + "</font>"; 
                   cadHTML += "</td>";
                   cadHTML += "<td>";
                   cadHTML += "<font face=VERDANA color=000000 size=1>" + ds.Tables[0].Rows[ContIndice].vin + "</font>";
                   cadHTML += "</td>";
                   cadHTML +="<td >";
                   cadHTML +="<font face=VERDANA color=000000 size=1>" + ds.Tables[0].Rows[ContIndice].id_agencia + "</font>"; 
                   cadHTML +="</td>";
				   cadHTML +="<td >";
				   cadHTML +="<font face=VERDANA color=000000 size=1>" + ds.Tables[0].Rows[ContIndice].nombre_agencia + "</font>"; 
				   cadHTML +="</td>";				   				   
				   cadHTML +="</tr>";				        
				   }
				        			   					                                          
                   cadHTML +="</table>";                  
                   cadHTML +="</td></tr>";
                   //cadHTML +="<tr><td align=right><img src='Imagenes/sidemenubotcurve.gif'></td></tr>";
                   cadHTML +="</table>";                   
                   cadHTML +="</td></tr></table>";                        
                   
                   document.all.Etiqueta.innerHTML = cadHTML;
                }
                else{
                            //Quiere decir que no encontró ningún registro con los parámetros proporcionados
                            cadHTML = "<table bgcolor='yellow' cellpadding=0 cellspacing=0>";
                            cadHTML +="<tr><td><font face=VERDANA color=000000 size=3><b> NO SE ENCONTRARON REGISTROS CON LOS PARAMETROS PROPORCIONADOS</b></font></td></tr>";
                            cadHTML +="</table>";
                            document.all.Etiqueta.innerHTML = cadHTML;
                }
           }                      
        }
                
       function Deselecciona(obj)
       {
       //alert(obj.checked);
          if (obj.checked)
            {
              document.all.cboEmpleados.value=0;
              document.all.cboUOs.value=0;
              document.all.cboGpos.value=0;
            }            
       }
        
       function Deselecciona2(obj)
       {
        //alert(obj.name);
         if (obj.value!="0")
         {
           document.all.chktodos.checked=false;           
         }
         
         if (obj.name=="cboUOs")
         {
           document.all.cboEmpleados.value=0;
           document.all.cboGpos.value=0;
         }
         
         if (obj.name=="cboEmpleados")
         {
           document.all.cboUOs.value=0;
           document.all.cboGpos.value=0;
         }
         
         if (obj.name=="cboGpos")
         {
           document.all.cboEmpleados.value=0;
           document.all.cboUOs.value=0;
         }                   
       } 
   
   function FncisEmpty(s)
   {
      return ((s == null) || (s.length == 0))
   }
        
        function PonFechaHoy()
{
var time=new Date();
var lmonth=time.getMonth() + 1;
if(lmonth<10)
 lmonth="0" + lmonth;
var date=time.getDate();
if(date<10)
date="0" + date;
var year=time.getYear();
if (year < 2000)
year = year + 1900;
document.getElementById("txtFechIni").value= date + '/' + lmonth + '/' + year;
document.getElementById("txtFechFin").value= date + '/' + lmonth + '/' + year;
return  
}

	// validación de fecha, formato dd/mm/aaaa (se requiere otra validación 
// si el formato es distinto, como dd-mm-aaaa, o mm-dd-aaaa...) 
function validaFecha(txt) { 
    
    if(txt.value=="")
       return false;
    
    var arrayDiasMes=[31,28,31,30,31,30,31,31,30,31,30,31]; 
    var re=/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/; 
    if (re.test(txt.value)) { 
    var arrayFecha=re.exec(txt.value); 
    var dia=parseInt(arrayFecha[1],10); 
    var mes=parseInt(arrayFecha[2],10); 
    var anno=parseInt(arrayFecha[3],10); 
    if ((anno%4==0 && anno%100!=0) || (anno%100==0 && anno%400==0)) arrayDiasMes[1]=29; 
    if (mes>=1 && mes<=12) { 
        if (dia>=1 && dia<=arrayDiasMes[mes-1]) return true; 
        } 
    } 
    txt.value=''; 
    alert('Fecha incorrecta: debe ser una fecha válida en el formato dd/mm/aaaa.'); 
    //txt.focus(); 
    return false; 
} 
</script> 

</asp:Content>