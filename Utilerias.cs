using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections.Specialized;
using System.Data;
using ICSharpCode.SharpZipLib.Zip;

public class Utilerias
    {        
        
        public Utilerias()
        { 
          
        }

        

        
        //Dada un diccionario y un valor regresa la llave si la encuentra
        //si no regresa la cadena vacia
        public string DameLLave(StringDictionary coleccion, string valor)
        {
            string res = "";
            foreach (string llave in coleccion.Keys)
            {
                string valorcol = coleccion[llave].Trim();
                if (valorcol.Equals(valor))
                {
                    res = llave;
                    return res;
                }
            }
            return res;
        }

        public static string DameLlave(NameValueCollection coleccion, string valor)
        {
            string res = "";
            foreach (string llave in coleccion.Keys)
            {
                string valorcol = coleccion[llave].Trim();
                if (valorcol.Equals(valor))
                {
                    res = llave;
                    return res;
                }
            }
            return res;
        }

        /// <summary>
        ///"lee toda la cadena n�mero que se le pasa como  argumento y se queda �nicamente con los caracteres que son d�gitos - o ." 
        /// </summary>
        public static string A_Numero(string numero)
        {
            string res = numero.Trim();
            numero = numero.Trim();
            if (numero.Length > 0)
            {
                //char[] cant = new char[numero.Length]; 
                int i = 0;
                int numdigitos = 0;
                string digito = "";
                string cantidad = "";
                string digitos = "-0123456789.,";
                while (i < numero.Length)
                {
                    digito = numero.Substring(i, 1);
                    if (digitos.IndexOf(digito) != -1)
                    {
                        cantidad += numero[i].ToString();
                        numdigitos++;
                    }
                    i++;
                }
                //ya tenemos en cantidad el n�mero con s�lo d�gitos 
                //ahora hacemos la conversion
                res = cantidad.Trim();
            }
            return res;
        }
        /// <summary>
        /// Busca cualquiera de los 10 caracteres: 0123456789
        /// </summary>
        /// <param name="cadena">donde se buscar�n los d�gitos</param>
        /// <returns>verdadero si encontr� aunque sea uno solo de los 10 d�gitos, falso si no encontro ninguno</returns>
        public static bool ContieneDigitos(string cadena)
        {
            bool res = false;
            int i = 0;
            string digito = "";
            string digitos = "0123456789";

            while (i < cadena.Length && res == false)
            {
                digito = cadena.Substring(i, 1);
                if (digitos.IndexOf(digito) > -1) //en cuanto esta el caracter entre los d�gitos devuelve verdadero
                {
                        res = true;
                }
                i++;
            }

            return res;
        }

        //regresa true si lo que recibe no contiene letras o caracteres que no pertenezcan al formato #,.
        //donde el # es cualquier cantidad de d�gitos.
        public static bool EsNumero(string numero)
        {
            bool res = true;
            int i = 0;
            string digito = "";
            string digitos = "-0123456789.,";
            while (i < numero.Length && res == true)
            {
                digito = numero.Substring(i, 1);
                if (i == 0 && digito == "-" && numero.Length < 2) //solo es el signo - , no un n�mero negativo
                    res = false;
                if (i == 0 && digito == "," && numero.Length < 2) //solo es el signo ',' , no un n�mero
                    res = false;
                if (i == 0 && digito == "." && numero.Length < 2) //solo es el signo '.' , no un n�mero
                    res = false;

                if (res != false)
                {
                    if (digitos.IndexOf(digito) == -1) //en cuanto no esta el caracter entre los d�gitos devuelve falso
                    {
                        res = false;
                    }
                }
                i++;
            }
            return res;
        }

        public static string A_Fecha(string fecha)
        {

            DateTime fechaaux;
            try
            {
                if (fecha.Length == 6)
                { //al a�o le faltan 2 d�gitos esperamos que el formato sea yyMMdd
                    string anioaux = fecha.Substring(0, 2);
                    int anio = A_Entero(anioaux);
                    if (anio >= 20 && anio <= 99) //la fecha esta en el siglo pasado
                    {
                        anio = 1900 + anio;
                    }
                    else if (anio >= 0 && anio < 20) //LJBA este codigo solo servir� hasta el 2019
                    {
                        anio = 2000 + anio;
                    }
                    fecha = anio.ToString() + "/" + fecha.Substring(2, 2) + "/" + fecha.Substring(4, 2);
                }

                fechaaux = Convert.ToDateTime(fecha);
                fecha = fechaaux.ToString("dd/MM/yyyy");
            }
            catch (Exception e)
            {
                string a = e.Message;
                return fecha;
            }
            return fecha;
        }

        public static bool EsFecha(string fecha)
        {
            bool res = false;
            DateTime fechaaux;
            if (fecha.IndexOf("/") == -1)
                return res;
            try
            {
                fechaaux = Convert.ToDateTime(fecha);
                res = true;
            }
            catch (Exception e)
            {
                string al = e.Message;
                return false;
            }
            return res;
        }
        /// <summary>
        ///Recibe una cadena que supone tiene inmerso un n�mero p.e. "12r4a"
        ///Regresa el n�mero entero que se encuentra en ella --> 124
        ///si no hay un n�mero entero, entonces regresa un 0 
        /// </summary>
        public static int A_Entero(string numero)
        {
            int res = 0;
            numero = numero.Trim();
            if (numero.Length > 0)
            {
                //char[] cant = new char[numero.Length]; 
                int i = 0;
                int numdigitos = 0;
                string digito = "";
                string cantidad = "";
                string digitos = "-0123456789";
                while (i < numero.Length)
                {
                    digito = numero.Substring(i, 1);
                    if (digitos.IndexOf(digito) != -1)
                    {
                        cantidad += numero[i].ToString();
                        numdigitos++;
                    }
                    i++;
                }
                //ya tenemos en cantidad el n�mero con s�lo d�gitos 
                //ahora hacemos la conversion
                if (cantidad.Length > 0)
                    res = Convert.ToInt16(cantidad);
            }
            return res;
        }

        /// <summary>
        /// Dado un N�mero, lo regresa con el formato de moneda local sin el signo de pesos
        /// </summary>
        /// <param name="Numero"></param>        
        /// <returns></returns>

        public static string FormateaNumero(string Numero)
        {
            try
            {
                double num = Convert.ToDouble(Numero);
                Numero = num.ToString("c");
                Numero = Numero.Replace("$", ""); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message); 
            }
            return Numero;
        }



        #region Para implementar El Select Distinct en una DataTable
        private static bool ColumnEqual(object A, object B)
        {

            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }

        public static DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
        {
            DataTable dt = new DataTable(TableName);
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            //if (ds != null)
            //    ds.Tables.Add(dt);
            return dt;
        }


        #endregion



        #region Escribir en un archivo de texto el log.
        public static bool LimpiaArchivoLog(string rutaArchivo)
        {
            bool res = false;
            FileStream fs = null;
            try
            {
                fs = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write, FileShare.None);
                fs.Close();
                res = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return res;
        }


        public static void WriteToLog(string message, string Desde, string rutaArchivo)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    fs = new FileStream(rutaArchivo, FileMode.Append, FileAccess.Write, FileShare.None);
                    sw = new StreamWriter(fs, Encoding.ASCII);
                    sw.WriteLine(message.Trim() + ", desde: " + Desde + "," + DateTime.Now.ToString());
                    sw.Close();
                    fs.Close();
                }
                else
                {//El archivo no existe por lo tanto lo creamos y escribimos en el 
                    fs = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write, FileShare.None);
                    sw = new StreamWriter(fs, Encoding.ASCII);
                    sw.WriteLine(message.Trim() + ", desde: " + Desde + "," + DateTime.Now.ToString());
                    sw.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
        }
        #endregion

        #region Utilerias de Compresion y Descompresion de Archivos

        public static string DescomprimirZip(string sRuta, string ArchZip)
        {
            string res = "";
            try
            {
                FastZip fZip = new FastZip();
                fZip.ExtractZip(ArchZip, sRuta, "");
                res = sRuta;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return res;
        }


        public static string ComprimirZip(string sRuta, string ArchZip, string filtro)
        {
            string res = "";
            bool comprimio = false;
            try
            {
                string[] arrfiles = Directory.GetFiles(sRuta, filtro);
                if (arrfiles.Length > 0)
                {
                    ZipOutputStream zipOut = new ZipOutputStream(File.Create(ArchZip));
                    foreach (string fName in arrfiles)
                    {
                        FileInfo fi = new FileInfo(fName);
                        ZipEntry entry = new ZipEntry(fi.Name);
                        FileStream sReader = File.OpenRead(fName);
                        byte[] buff = new byte[Convert.ToInt32(sReader.Length)];
                        sReader.Read(buff, 0, (int)sReader.Length);
                        entry.DateTime = fi.LastWriteTime;
                        entry.Size = sReader.Length;
                        sReader.Close();
                        zipOut.PutNextEntry(entry);
                        zipOut.Write(buff, 0, buff.Length);
                        comprimio = true;
                    }
                    zipOut.Finish();
                    zipOut.Close();
                    if (File.Exists(ArchZip) && comprimio)
                        res = ArchZip.Trim();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return res;
        }

        #endregion

    }

