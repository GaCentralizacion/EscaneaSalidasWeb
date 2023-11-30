using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Diagnostics;

       
public    class clsCodigoBarrasTicket
    {
        ArrayList headerLines = new ArrayList();

        int count = 0; //número de líneas que se han impreso

        int maxChar = 35;
        int maxCharDescription = 20;

        int imageHeight = 0;

        float leftMargin = 0; //0
        float topMargin = 0; //3

        string fontName = "Lucida Console";
        int fontSize = 9;

        Font printFont = null;
        SolidBrush myBrush = new SolidBrush(Color.Black);

        Font printFontBold = null;

        Graphics gfx = null;

        string line = null;

        bool ImpHeader = true; //indica si se desea que se incluya en la imagen el encabezado (en este caso la fecha)
        bool ImpClave = true; //indica si se desea que se incluya en la imagen (debajo del codigo de barras), la clave o lo que representan las barras)
        bool ImpPie = true; //indica si se desea que se incluya en la imagen el pie de imagen en este caso el nombre del producto y su descripcion

        string Codigo = String.Empty; //es la clave que se transforma al codigo de Barras
        string ProducName = String.Empty; //son el pie de la imagen.
        string DescProduc = String.Empty;
        int GrosorBarras = 2;
        Image CodeBarImage = null;

        public clsCodigoBarrasTicket()
        {
        }

        #region establecimiento de propiedades
        public bool ImprimirHeader
        {
            get { return this.ImpHeader;}
            set { if (value != ImpHeader) ImpHeader = value; }
        }
        public bool ImprimirClave
        {
            get { return this.ImpClave; }
            set { if (value != ImpClave) ImpClave = value; }
        }
        public bool ImprimirPie
        {
            get { return this.ImpPie; }
            set { if (value != ImpPie) ImpPie = value; }
        }


        public int BarrasGrosor
        {
            get { return this.GrosorBarras; }
            set { if (value != GrosorBarras) GrosorBarras = value; } 
        }
        public Image CodeBarImagen
        {
            get {return this.CodeBarImage;}
        }

        public string CodigoBarras
        {
            get { return this.Codigo; }
            set { if (value != Codigo) Codigo = value; }          
        }

        public string NombreProducto
        {
            get { return this.ProducName;}
            set { if (value != ProducName) ProducName = value; }
        }

        public string DescripcionProducto
        {
            get { return this.DescProduc; }
            set { if (value != DescProduc) DescProduc = value; }
        }

        public int MaxChar
        {
            get { return maxChar; }
            set { if (value != maxChar) maxChar = value; }
        }

        public int MaxCharDescription
        {
            get { return maxCharDescription; }
            set { if (value != maxCharDescription) maxCharDescription = value; }
        }

        public int FontSize
        {
            get { return fontSize; }
            set { if (value != fontSize) fontSize = value; }
        }

        public string FontName
        {
            get { return fontName; }
            set { if (value != fontName) fontName = value; }
        }
        #endregion

        #region complementos
        public void AddHeaderLine(string line)
        {
            headerLines.Add(line);
        }
        /// <summary>
        /// Alínea el texto a la derecha.
        /// </summary>
        /// <param name="lenght">longitud del texto a alinear</param>
        /// <returns>los espacios necesarios suficientes en el lado izquierdo para alinear a la derecha el texto</returns>
        private string AlignRightText(int lenght)
        {
            string espacios = "";
            int spaces = maxChar - lenght;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }
        /// <summary>
        /// Crea una línea de *
        /// </summary>
        /// <returns>regresa la línea de *</returns>
        private string DottedLine()
        {
            string dotted = "";
            for (int x = 0; x < maxChar; x++)
                dotted += "*";
            return dotted;
        }
        #endregion
        #region eventos de impresion
        public bool PrinterExists(string impresora)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (impresora == strPrinter)
                    return true;
            }
            return false;
        }

        private void ObtenImagenCodigoBarras()
        {
            if (this.CodigoBarras.Trim() != "" && this.CodigoBarras.Length>1) //no lo hace con un solo caracter
            {
                CodeBarImage = Code128Rendering.MakeBarcodeImage(this.Codigo, this.GrosorBarras, true);

            }
        }

        public Image ObtenImagenSoloCodigoBarras()
        {
            ObtenImagenCodigoBarras();
            return CodeBarImage; 
        }

        public Image ObtenPrevisualizacion()
        {
            printFont = new Font(fontName, fontSize, FontStyle.Regular);
            printFontBold = new Font(fontName, fontSize, FontStyle.Bold); //LJBA es necesario primero obtener el alto y ancho de la imagen del Código de Barras            
            ObtenImagenCodigoBarras();

            Image previa = new System.Drawing.Bitmap(CodeBarImage.Width, CodeBarImage.Height+100); //LJBA hard code
            try
            {
                using (gfx = Graphics.FromImage(previa))
                {
                    DrawHeader(); //Agrega al dibujo la fecha  [opcional]          
                    DrawCodeBar("");//Agrega al dibujo el código de barras.
                    DrawPieCodeBar(); //Agrega al dibujo la cadena que es el codigo de barras [opcional]
                    DrawNombreDescProd(); //Agrega al dibujo el Nombre del producto y su descripcion  [opcional]                  
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return previa;
        }

        public void PrintTicket(string impresora)
        {
            printFont = new Font(fontName, fontSize, FontStyle.Regular);
            printFontBold = new Font(fontName, fontSize, FontStyle.Bold);
            PrintDocument pr = new PrintDocument();
            pr.PrinterSettings.PrinterName = impresora;
            pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
            pr.Print();
        }

        private void pr_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            gfx = e.Graphics;
            if (CodeBarImage == null)
                ObtenImagenCodigoBarras();

            DrawHeader(); //Agrega al dibujo la fecha
            DrawCodeBar("Impresion");//Agrega al dibujo el código de barras.
            DrawPieCodeBar(); //Agrega al dibujo la cadena que es el codigo de barras [opcional]
            DrawNombreDescProd(); //Agrega al dibujo el Nombre del producto y su descripcion
            if (CodeBarImage != null)
            {
                CodeBarImage.Dispose();                
            }            
        }

        #endregion

        #region Construccion de la imagen a imprimir (Todo lo que se va a imprimir es tratado como una imagen
        /// <summary>
        /// Suma (en pixeles) el margen superior +  El espacio que ocupan las líneas de texto impresas + el alto de la imagen
        /// </summary>
        /// <returns>La posicion en pixeles del cursor en la imagen de la coordenada Y</returns>        
        private float YPosition()
        {
            float res = 0;
            res = topMargin + (count * printFont.GetHeight(gfx) + imageHeight);
            return res;
        }

        private void DrawEspacio()
        {
            line = "";

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

            count++;
        }


        /// <summary>
        /// Agrega al dibujo los encabezados definidos para el Ticket en este caso solo la fecha en que se imprime
        /// hace el formateo para cuando el texto sobrepasa el ancho del papel lo imprima en una siguiente línea.
        /// al final agrega una linea en blanco
        /// </summary>
        private void DrawHeader()
        {
            if (this.ImpHeader == true)
            {
                foreach (string header in headerLines)
                {
                    if (header.Length > maxChar)
                    {
                        int currentChar = 0;
                        int headerLenght = header.Length;

                        while (headerLenght > maxChar)
                        {
                            line = header.Substring(currentChar, maxChar);
                            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                            count++;
                            currentChar += maxChar;
                            headerLenght -= maxChar;
                        }
                        line = header;
                        gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                        count++;
                    }
                    else
                    {
                        line = header;
                        gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                    }
                }
                DrawEspacio();
            }
        }



        private void DrawCodeBar(string Tipo)
        {
            try
            {
                
                if (CodeBarImage != null)
                {
                    try
                    {
                        gfx.DrawImage(CodeBarImage, new Point((int)leftMargin, (int)YPosition()));
                        if (Tipo == "Impresion") //LJBA: cuando se manda a imprimir (a la impresora valga la redundancia por claridad),  
                        {                        //la altura de la imagen debe reducirse, de otra manera al ser la unidad de impresión milimetros, 
                                                //agrega un margen indeseable debajo y después de la impresion de la imagen en la hoja térmica.
                            double height = ((double)CodeBarImage.Height / 160) * 15; //((double)headerImage.Height / 58) * 15;
                            imageHeight = (int)Math.Round(height) + 5;
                        }
                        else {//cuando la salida es a la pantalla no es necesario modificar la altura de la imagen.
                            double height = ((double)CodeBarImage.Height);
                            imageHeight = (int)Math.Round(height) + 3;
                        }
                    }
                    catch (Exception ex1)
                    {
                        Debug.WriteLine(ex1.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Coloca en la imagen al pie del codigo de barras la clave o bien lo que representa el código de barras.
        /// </summary>
        private void DrawPieCodeBar()
        {
            try
            {                
                if (CodeBarImage != null && this.ImpClave == true)
                {
                    try
                    {
                        float ancho = ((float)CodeBarImage.Width);
                        //ancho = leftMargin + (ancho / 2);
                        ancho = 0;
                        string header = this.CodigoBarras.Trim();
                        if (header.Length > maxChar)
                        {
                            int currentChar = 0;
                            int headerLenght = header.Length;

                            while (headerLenght > maxChar)
                            {
                                line = header.Substring(currentChar, maxChar);
                                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                                count++;
                                currentChar += maxChar;
                                headerLenght -= maxChar;
                            }
                            line = header;
                            gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, ancho, YPosition(), new StringFormat());
                            count++;
                        }
                        else
                        {
                            line = header;
                            gfx.DrawString(line, printFont, myBrush, ancho, YPosition(), new StringFormat());

                            count++;
                        }
                        DrawEspacio();
                    }
                    catch (Exception)
                    {
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        private void DrawNombreDescProd()
        {
            if (this.ImpPie == true)
            {
                ArrayList footerLines = new ArrayList();
                footerLines.Add(this.ProducName);
                footerLines.Add(this.DescProduc);

                foreach (string footer in footerLines)
                {
                    if (footer.Length > maxChar)
                    {
                        int currentChar = 0;
                        int footerLenght = footer.Length;

                        while (footerLenght > maxChar)
                        {
                            line = footer;
                            gfx.DrawString(line.Substring(currentChar, maxChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                            count++;
                            currentChar += maxChar;
                            footerLenght -= maxChar;
                        }
                        line = footer;
                        gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                        count++;
                    }
                    else
                    {
                        line = footer;
                        gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                    }
                }
                leftMargin = 0;
                //DrawEspacio();
            }
        }
        #endregion

    }

