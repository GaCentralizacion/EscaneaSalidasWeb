<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sicop_ConsultaSalidas.aspx.cs" Inherits="PortalClientes.sicop_ConsultaSalidas" %>



<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
    <title>.: GRUPO ANDRADE SALIDAS DE UNIDADES :.</title>
    <link href ="Estilos/Style.css"  rel="stylesheet" type="text/css" />
    <style>        
        .pg-normal {
                    color: #000000;
                    font-size: 15px;
                    cursor: pointer;
                    background: #D0B389;
                    padding: 2px 4px 2px 4px;
                    }

      .pg-selected {
                    color: #fff;
                    font-size: 15px;
                    background: #000000;
                    padding: 2px 4px 2px 4px;
                    }      
                    
       .TextBoxResultadoMin
        {	
        BACKGROUND-COLOR: #FFFFFF;
        COLOR: #FF0000;
        FONT-FAMILY: verdana;
        FONT-SIZE: 11px;
        FONT-WEIGHT: bold;
        BORDER-RIGHT: #FFcc66 1px solid;
        BORDER-TOP: #FFcc66 1px solid;
        BORDER-LEFT: #FFcc66 1px solid;
        BORDER-BOTTOM: #FFcc66 1px solid
        }                                 
    </style>
    
</head>
<!--
/********************************************ENCABEZADO**************************************************************
 * Programa:    sicop_ConsultaSalidas.aspx
 * Desarrollor: Luis Bonnet
 * Fecha:       20141031
 * Descripcion: Buscará en base de datos el registro de los VIN registrados como salida, generará el reporte con código de barras.

 * Lo interesante es que la paginacion se hace localmente en el Browser con JavaScript, ocultando por páginas y solo mostrando los registros de la pagina que se desea.
 * Es mucho más rápido en cuanto a la navegacion entre paginas, pero puede ser lento al traer el resulset inicial, pues trae todos los registros.
 
 *********************************************************************************************************************/
-->

<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hid_agencia" name="hid_agencia" value="<%=id_agencia%>"/>
    <input type="hidden" id="hsicop_nombre_usuario" name="hsicop_nombre_usuario" value="<%=sicop_nombre_usuario%>"/>
    <input type="hidden" id="hnombre_agencia" name="hnombre_agencia" value="<%=nombre_agencia%>"/>            

    <div>
     <table border="0" cellpadding="0" cellspacing="0" width="100%">
                      <tr>
                       <td align="center"><h3>CONSULTA DE LA BITACORA DE SALIDAS DE UNIDAD</h3></td>
                       <td align="center">
                            <h4><%=nombre_agencia%>  &nbsp;&nbsp;&nbsp;&nbsp;  <%=sicop_nombre_usuario%></h4>
                       </td>
                     </tr> 
 <tr>
   <td align=center>   
                 <table border="0" cellpadding="0" cellspacing="0" width="50%">
                     <tr>
                       <td align=right>
                         <font class = "ETIQUETA">Buscar por:</font>
                       </td>
                       <td align=left colspan=2> 
                                                     <select class="SelectBox" name="cboBuscarPor" id="cboBuscarPor">
                                                           <option value="AQUIEN">V.I.N.</option>
                                                           <option selected value="FECHA">Fecha (DD/MM/YYYY)</option>
                                                           <option value="RANGO">Periodo de Fechas (DD/MM/YYYY) - (DD/MM/YYYY) </option>
                                                           <option value="IDPROSPECTO">IdProspecto SICOP</option>
                                                           <option value="FACTURA">Factura SerieFolio</option>
                                                     </select>
                                                     &nbsp;
                                                     <input type=text style="width:250px;" class="TextBoxResultadoMin" id="txtTextoBuscar"  value="" maxlength=50/>
                       </td>
                       <td align=left>
                          
                       </td>
                     </tr>
                     
                     <tr>
										<td colspan=5>
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="cmdConsulta" type="button" title="Consulta los registros de la BITACORA" value="Buscar" onclick="ConsultaBitacora();" style="cursor:hand;" />
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font class = "Rojo10">** NOTA **</font>&nbsp;&nbsp;<font class = "AzulMarino9">Se reporta el ID SICOP en las salidas registradas a partir del </font> &nbsp; <font class = "Rojo10"> 07 / 11 / 2014 </font>
                                        </td>
                                                                         
                     </tr>    
                     </table>
   </td>
   </tr>             
                                
                     <tr ><td colspan=2 align="center">     
                         <!-- En este layer es donde se mostrarán los resultados de la interaccion con el servidor mediante Ajax-->
   					<div id="Etiqueta" styles="width:1000px;" align="center" >

					</div>     
                    </td></tr>     
     </table>
    </div>
    </form>
</body>
</html>

<script language=javascript>
        
        function ConsultaBitacora()
        {
          var ordenx = "";                            
          var textoBuscar = document.all.txtTextoBuscar.value;
          var opcbusqueda = document.all.cboBuscarPor.value;          
          var id_agencia=document.all.hid_agencia.value;
          
          document.all.cmdConsulta.disabled=true;    
          document.all.Etiqueta.innerHTML="";  
          document.all.Etiqueta.innerHTML="<font class=ETIQUETA>Cosultando VIN´s espere un momento...</font>&nbsp;<img src='Imagenes/espere.gif'/>";
            
          sicop_ConsultaSalidas.ConsultaBitacora(textoBuscar,ordenx,opcbusqueda,id_agencia,CallBackConsultaBitacora);        
        }
        
        function CallBackConsultaBitacora(response)
        {
        var ds = response.value;           
           
           document.all.cmdConsulta.disabled=false;
           
           var cadHTML = "";           
           cadHTML = "<Table width=100% cellpadding=0 cellspacing=0 border=0 >";
           //cadHTML +="<tr><td align=center>";
           //cadHTML +="<font class=ETIQUETA><b>Listado de VINS:</b></font>";           
           //cadHTML +="</td></tr>";
           cadHTML +="<tr><td>";
           cadHTML +="<br>";           
           cadHTML +="</td></tr>";           
           cadHTML +="<tr><td align=center>";
           cadHTML += "<Table cellpadding=0 cellspacing=0 border=0 bordercolor=000000>";
           
           //el renglon de la paginacion                                            
                   cadHTML +="<tr align=center>";
		           cadHTML +="<td name='pageNavPosition' id='pageNavPosition' colspan=10>";		            
				   cadHTML +="</td>";				   
     			   cadHTML +="</tr>";
     			              
           cadHTML +="<tr><td align=left><font class=ETIQUETA><b>Listado de Números de Identifacion Vehicular encontrados:</b></font></td></tr>";                      
           cadHTML +="<tr><td>";
           cadHTML += "<Table name='myworklist' id='myworklist' width=100% cellpadding=0 cellspacing=0 border=0>"; 
           //cadHTML +="<tr><td colspan=7 align=right><img src='Imagenes/sidemenutopline.gif'></td></tr>";                      
           //cadHTML +="<tr><td></td><td></td><td></td><td></td><td></td><td></td><td rowspan=1000 width='1px;' align=left bgcolor='003366'><img width='1px;' src='Imagenes/sidemenurightline.gif'></td></tr>";
           cadHTML +="<tr><td colspan=10 bgcolor='003366'><img src='Imagenes/spacer.gif'></td></tr>";
           cadHTML +="<tr bgcolor=ffffff>";           
           cadHTML += "<td align=center><font face=VERDANA color=003366 size=1><b>&nbsp;&nbsp;</b></td>";
           cadHTML += "<td align=center><font face=VERDANA color=003366 size=1><b>FECHA ESCANEO&nbsp;&nbsp;</b></td>";
           cadHTML += "<td align=center><font face=VERDANA color=003366 size=1><b>HORA ESCANEO&nbsp;&nbsp;</b></td>";
           cadHTML += "<td align=center><font face=VERDANA color=003366 size=1><b>FACTURA&nbsp;&nbsp;</b></td>";
           cadHTML += "<td align=center><font face=VERDANA color=003366 size=1><b>FECHA FACTURA&nbsp;&nbsp;</b></td>";
           cadHTML += "<td align=center><font face=VERDANA color=003366 size=1><b>IDSICOP&nbsp;&nbsp;</b></td>";
           cadHTML += "<td align=center><font face=VERDANA color=003366 size=1><b>TIPO VENTA&nbsp;&nbsp;</b></td>";
           cadHTML += "<td align=center><font face=VERDANA color=003366 size=1><b>VIN&nbsp;&nbsp;</b></td>";
           cadHTML += "<td align=center><font face=VERDANA color=003366 size=1><b>TIPO AUTO&nbsp;&nbsp;</b></td>";
           cadHTML +=" <td align=center><font face=VERDANA color=003366 size=1><b>CODIGO BARRAS&nbsp;&nbsp;</b></td>";  
           cadHTML +="</tr>";
           cadHTML +="<tr><td colspan=10 bgcolor='003366'><img src='Imagenes/spacer.gif'></td></tr>";           
           cadHTML +="<tr><td colspan=10>&nbsp;</td></tr>";
           if(ds != null && typeof(ds) == "object" && ds.Tables != null && ds.Tables.length>0)
           {                                            
             if(ds.Tables[0].Rows.length>0)
             {             
               var contadorfacturas = 0;
               var bgcolor="ffffff";
               
               for (var ContIndice=0;ContIndice<ds.Tables[0].Rows.length;ContIndice++)
                   {
                   if (bgcolor=="ffffff")
                     bgcolor="f0f0f0";
                   else
                     bgcolor="ffffff";  
                   
                   cadHTML +="<tr align=center bgcolor=" + bgcolor  + ">";
		           cadHTML +="<td>";
		           cadHTML +="<font face=VERDANA color=000000 size=1><b>&nbsp;</b></font>";
		           cadHTML +="</td>";
		           cadHTML +="<td>";
		           cadHTML +="<font face=VERDANA color=000000 size=1><b>" + ds.Tables[0].Rows[ContIndice].fecha + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b></font>";
		           cadHTML += "</td>";		           
		           cadHTML +="<td>";
		           cadHTML +="<font face=VERDANA color=000000 size=1><b>" + ds.Tables[0].Rows[ContIndice].hora + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b></font>";
		           cadHTML +="</td>";
		           cadHTML +="<td>";
                   cadHTML +="<font face=VERDANA color=000000 size=1><b>" + ds.Tables[0].Rows[ContIndice].factura + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b></font>"; 
                   cadHTML += "</td>";
                   cadHTML += "<td>";
                   cadHTML += "<font face=VERDANA color=000000 size=1><b>" + ds.Tables[0].Rows[ContIndice].fecha_factura + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b></font>";
                   cadHTML += "</td>";
				   cadHTML +="<td align=left>";
                   cadHTML +="<font face=VERDANA color=000000 size=1><b>" + ds.Tables[0].Rows[ContIndice].idsicop + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b></font>"; 
				   cadHTML +="</td>";
				   cadHTML += "<td align=left>";
				   cadHTML += "<font face=VERDANA color=000000 size=1><b>" + ds.Tables[0].Rows[ContIndice].tipo_venta + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b></font>";
				   cadHTML += "</td>";

				   cadHTML += "<td align=left>";
				   cadHTML += "<font face=VERDANA color=000000 size=1><b>" + ds.Tables[0].Rows[ContIndice].aquien + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b></font>";
				   cadHTML += "</td>";

				   cadHTML += "<td align=left>";
				   cadHTML += "<font face=VERDANA color=000000 size=1><b>" + ds.Tables[0].Rows[ContIndice].tipo_auto + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b></font>";
				   cadHTML += "</td>";

		           cadHTML +="<td align=left>";
                   cadHTML +="<img src='" + ds.Tables[0].Rows[ContIndice].codebar + "'>"; 
				   cadHTML +="</td>";				   
     			   cadHTML +="</tr>";	
     			   contadorfacturas++;				                           			   
                   }//del for        
                   
                                                                                                                             
                   cadHTML +="</table>";                  
                   cadHTML +="</td></tr>";
                   //cadHTML +="<tr><td align=right><img src='Imagenes/sidemenubotcurve.gif'></td></tr>";
                   cadHTML +="</table>";                   
                   cadHTML +="</td></tr></table>";                        
                   
                   document.all.Etiqueta.innerHTML = cadHTML;
                                      
                   //antes de mostrar el layer puedes colocarlo donde tu quieras en la pantalla
                   //accediendo a sus propiedades top y left
                   //document.getElementById("Etiqueta").style.top = "150px";
                   //document.getElementById("Etiqueta").style.left = "50px";
                   //document.getElementById("Etiqueta").style.display = "";    
                   
                   //************************PAGINANDO EL RESULTADO******************************//
                   /*
                   var pager = new Pager('myworklist', 15);
                       pager.init();
                       pager.showPageNav(pager, 'pageNavPosition');
                       pager.showPage(1);      
				  */
                                                                                                      
                }
                else{
                              //Quiere decir que no encontró ninguna factura con esa empresa:
                            cadHTML = "<table bgcolor='yellow' cellpadding=0 cellspacing=0>";
                            cadHTML +="<tr><td><br></td></tr>";
                            cadHTML +="<tr><td><font face=VERDANA color=000000 size=3><b> NO SE ENCONTRARON REGISTROS con los parametros de consulta proporcionados</b></font></td></tr>";
                            cadHTML +="</table>";
                            document.all.Etiqueta.innerHTML = cadHTML;
                }
           }
           else{
              //Quiere decir que no encontró ninguna factura con esa empresa:
              cadHTML = "<table bgcolor='yellow' cellpadding=0 cellspacing=0>";
              cadHTML +="<tr><td><br></td></tr>";
              cadHTML +="<tr><td><font face=VERDANA color=000000 size=3><b> NO SE ENCONTRARON REGISTROS </b></font></td></tr>";
              cadHTML +="</table>";
              document.all.Etiqueta.innerHTML = cadHTML;
           }      
        
        }
                                      


//***********INICIA PAGINACION******
var _pagerObj;

function Pager(tableName, itemsPerPage) {
     this.tableName = tableName;
     this.itemsPerPage = itemsPerPage;
     this.currentPage = 1;
     this.pages = 0;
     this.inited = false;

     this.showRecords = function(from, to) {
         var rows = document.getElementById(tableName).rows;
         // i starts from 1 to skip table header row
         for (var i = 4; i < rows.length; i++) {
             if (i < from || i > to)
                 rows[i].style.display = 'none';
             else
                 rows[i].style.display = '';
         }
     }

     this.showPage = function(pageNumber) {
         if (!this.inited) {
             alert("not inited");
             return;
         }
         var oldPageAnchor = document.getElementById('pg' + this.currentPage);
         oldPageAnchor.className = 'pg-normal';
         this.currentPage = pageNumber;
         var newPageAnchor = document.getElementById('pg' + this.currentPage);
         newPageAnchor.className = 'pg-selected';
         var from = (pageNumber - 1) * itemsPerPage + 1;
         var to = from + itemsPerPage - 1;
         this.showRecords(from, to);
     }

     this.prev = function() {
         if (this.currentPage > 1)
             this.showPage(this.currentPage - 1);
     }
     
     this.next = function() {
         if (this.currentPage < this.pages) {
             this.showPage(this.currentPage + 1);
         }
     }

     this.init = function() {
         var rows = document.getElementById(tableName).rows;
         //var rows = 50;
         var records = (rows.length - 1);
//         var records =50;         
         this.pages = Math.ceil(records / itemsPerPage);         
         this.inited = true;
     }

     this.showPageNav = function(pagerObj, positionId) {
     _pagerObj = pagerObj;
         if (!this.inited) {
             alert("not inited");
             return;
         }

         var element = document.getElementById(positionId);               
         var pagerHtml = '<font face=VERDANA color=0000aa size=1 style="cursor:hand"; title="ir a la página " onclick="_pagerObj.prev();" class="pg-normal"> &#171  Prev </font> ';
         for (var page = 1; page <= this.pages; page++)
             pagerHtml += '<font face=VERDANA color=0000aa size=1 style="cursor:hand" id="pg' + page + '" class="pg-normal" onclick="_pagerObj.showPage(' + page + ');">' + page + '</font> ';

         pagerHtml += '<font face=VERDANA color=0000aa size=1 style="cursor:hand" onclick="_pagerObj.next();" class="pg-normal"> Next &#187;</font>';
         element.innerHTML = pagerHtml;
     }
 }
 
 /* USO
var pager = new Pager('myworklist', 10);
                pager.init();
                pager.showPageNav(pager, 'pageNavPosition');
                pager.showPage(1);
*/                
//********* Termina objeto de la paginacion *******

</script> 
