using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

    public enum DateInterval 
    { 
        Day, 
        DayOfYear, 
        Hour, 
        Minute, 
        Month, 
        Quarter, 
        Second, 
        Weekday, 
        WeekOfYear, 
        Year 
    } 
 
    public class DateAndTime 
    {
        //public int NumPeriodoActual = 0;
        public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2) 
        { 
            return DateDiff(interval, dt1, dt2, System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek); 
        } 
 
        private static int GetQuarter(int nMonth) 
        { 
            if (nMonth <= 3) 
                return 1; 
            if (nMonth <= 6) 
                return 2; 
            if (nMonth <= 9) 
                return 3; 
            return 4; 
        } 
    
        /// <summary>
        /// Regresa la diferencia de 2 fechas, apartir de un tipo de periodo, semanal, mensual, etc
        /// </summary>
        /// <param name="interval">es el intervalo de tiempo que sirve para calcular los periodos</param>
        /// <param name="dt1"> es la fecha inicial de los periodos</param>
        /// <param name="dt2">es la fecha final de los periodos</param>
        /// <param name="eFirstDayOfWeek"> es el primer dia de la semana</param>
        /// <returns>regresa en cantidad numerica, el número de peridos </returns>
      
        public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2, DayOfWeek eFirstDayOfWeek) 
        { 
            if (interval == DateInterval.Year) 
                return dt2.Year - dt1.Year; 
 
            if (interval == DateInterval.Month) 
                return (dt2.Month - dt1.Month) + (12 * (dt2.Year - dt1.Year)); 
 
            TimeSpan ts = dt2 - dt1; 
             
            if (interval == DateInterval.Day || interval == DateInterval.DayOfYear) 
                return Round(ts.TotalDays); 
             
            if (interval == DateInterval.Hour) 
                return Round(ts.TotalHours); 
 
            if (interval == DateInterval.Minute) 
                return Round(ts.TotalMinutes); 
 
            if (interval == DateInterval.Second) 
                return Round(ts.TotalSeconds); 
 
            if (interval == DateInterval.Weekday ) 
            { 
                return Round(ts.TotalDays / 7.0); 
            } 
 
            if (interval == DateInterval.WeekOfYear) 
            { 
                while (dt2.DayOfWeek != eFirstDayOfWeek) 
                    dt2 = dt2.AddDays(-1); 
                while (dt1.DayOfWeek != eFirstDayOfWeek) 
                    dt1 = dt1.AddDays(-1); 
                ts = dt2 - dt1; 
                return Round(ts.TotalDays / 7.0); 
            } 
 
            if (interval == DateInterval.Quarter) 
            { 
                double d1Quarter = GetQuarter(dt1.Month); 
                double d2Quarter = GetQuarter(dt2.Month); 
                double d1 = d2Quarter - d1Quarter; 
                double d2 = (4 * (dt2.Year - dt1.Year)); 
                return Round(d1 + d2); 
            } 
 
            return 0; 
 
        } 
 
        private static long Round(double dVal) 
        { 
            if (dVal >= 0) 
                return (long)Math.Floor(dVal); 
            return (long)Math.Ceiling(dVal); 
        }
    

        /// <summary>
        /// Esta funcion regresa periodos de fechas divididos por semanas
        /// </summary>
        /// <param name="Periodos">es un ArrayList que contendra los rangos de las fechas semanales </param>
        /// <param name="fechaInicio"> fecha de inicio de los periodos</param>
        /// <param name="fechaFinal">fecha final de los periodos</param>
        /// <returns>regresa un ArrayList con elementos del tipo 15_17/jul/2009</returns>

        public ArrayList prcPeriodosSemanas(ArrayList Periodos, DateTime fechaInicio, DateTime fechaFinal)
        {
            DateTime semana;
            string rango1 = "";
            string rango2 = "";
            string Fecha = "";
            int cont_periodos;
       
            long diferencia = DateAndTime.DateDiff(DateInterval.Day, fechaInicio, fechaFinal);
            semana = fechaInicio;
            diferencia = diferencia / 7;
            

            for (cont_periodos = 0; cont_periodos <= diferencia; cont_periodos ++)
                {     
                        if (semana.DayOfWeek == DayOfWeek.Monday)
                        {
                            rango1 = semana.ToString().Substring(0, 2);
                            semana = semana.AddDays(7);
                            rango2 = semana.AddDays(-3).ToString().Substring(0, 2);
                            semana = semana.AddDays(-3);

                        }
                        else if (semana.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            rango1 = semana.ToString().Substring(0, 2);
                            semana = semana.AddDays(6);
                            rango2 = semana.AddDays(-3).ToString().Substring(0, 2);
                            semana = semana.AddDays(-3);
                        }

                        else if (semana.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            rango1 = semana.ToString().Substring(0, 2);
                            semana = semana.AddDays(5);
                            rango2 = semana.AddDays(-3).ToString().Substring(0, 2);
                            semana = semana.AddDays(-3);
                        }
                        else if (semana.DayOfWeek == DayOfWeek.Thursday)
                        {
                            rango1 = semana.ToString().Substring(0, 2);
                            semana = semana.AddDays(4);
                            rango2 = semana.AddDays(-3).ToString().Substring(0, 2);
                            semana = semana.AddDays(-3);
                        }
                        else if (semana.DayOfWeek == DayOfWeek.Friday)
                        {
                            rango1 = semana.ToString().Substring(0, 2);
                            semana = semana.AddDays(3);
                            rango2 = semana.AddDays(-3).ToString().Substring(0, 2);
                            semana = semana.AddDays(-3);
                        }


                        if (cont_periodos == diferencia)
                        {
                            if (semana > fechaFinal)
                            {
                                long dif;
                                dif = DateAndTime.DateDiff(DateInterval.Day, semana, fechaFinal);
                                semana = semana.AddDays(dif);
                                rango2 = semana.ToString().Substring(0, 2);
                            }

                        }

                        Fecha = rango1 + "-" + rango2 + "/" + fncMes(semana.Month) + "/" + semana.Year.ToString();
                        semana = semana.AddDays(3);
                        Periodos.Add(string.Format(Fecha));
            }
            return Periodos;
        }
    
        /// <summary>
        /// Esta funcion regresa periodos de fechas divididos por meses en un arreglo
        /// </summary>
        /// <param name="Periodos"> es un ArrayList que contendra los periodos de las fechas</param>
        /// <param name="fechaInicio">es la fecha de partida</param>
        /// <param name="fechaFinal">es la fecha final</param>
        /// <returns>regresa un ArrayList con cadenas del tipo ENE/2009, FEB/2009, etc </returns>
        public ArrayList prcPeriodosMeses( ArrayList Periodos, DateTime fechaInicio, DateTime fechaFinal)
        {
            DateTime mes;
            string Fecha;
         
            int cont_periodos = 0;
            int NumPeriodoActual=0;
                       
            long diferencia = DateAndTime.DateDiff(DateInterval.Month, fechaInicio, fechaFinal);
            mes = Convert.ToDateTime(fechaInicio);
            
                for (cont_periodos = 0; cont_periodos <= diferencia; cont_periodos ++)
                {
                     if (cont_periodos == 0)
                        {
                            mes.AddMonths(0);
                        }
                     else
                        {
                            mes = mes.AddMonths(1);
                        }
                        if ((mes.Month == DateTime.Now.Month) & (mes.Year == DateTime.Now.Year))
                        {
                            NumPeriodoActual = cont_periodos;
                        }
                        Fecha = fncMes(mes.Month);
                        Fecha = Fecha + "/" + mes.Year.ToString();
                        Periodos.Add(Fecha);
               }
          return Periodos;
       }

        /// <summary>
        /// Esta funcion regresa el Número de periodo que representa la fecha actual con respecto a un proyecto en especifico, y valida que
        /// este dentro del rango permitido de modificacion 
        /// </summary>
        /// <param name="tipoPeriodo">Es la forma en como ha sido dividido el proyecto, puede ser SEMANA ó MES</param>
        /// <param name="fechaInicio">fecha de inicio del proyecto</param>
        /// <param name="fechaFinal">fecha final del proyecto</param>
        /// <returns>regresa el número (entero) del periodo que representa la fecha actual en el SERVIDOR con respecto del proyecto</returns>
        public int prcRegresaNumPer(string tipoPeriodo, DateTime fechaInicio, DateTime fechaFinal)
        {
            int NumPeriodoActual = 0;
            DateTime mes;
            int cont_periodos = 0;
        
            if (tipoPeriodo == "MES")
            {
                long diferencia = DateAndTime.DateDiff(DateInterval.Month, fechaInicio, fechaFinal);
                mes = Convert.ToDateTime(fechaInicio);

                for (cont_periodos = 1; cont_periodos <= diferencia; cont_periodos++)
                {
                    if (cont_periodos == 0)
                    {
                        mes.AddMonths(0);
                    }
                    else
                    {
                        mes = mes.AddMonths(1);
                    }

                    if ((mes.Month == DateTime.Now.Month) & (mes.Year == DateTime.Now.Year))
                    {
                        NumPeriodoActual = cont_periodos;
                        return NumPeriodoActual;
                        break;
                    }
                }

                if ((fechaFinal.Month > DateTime.Now.Month) & (fechaFinal.Year < DateTime.Now.Year))
                {
                    NumPeriodoActual = 1000;
                }

                else if ((fechaInicio.Month < DateTime.Now.Month) & (fechaInicio.Year <= DateTime.Now.Year))
                {
                    NumPeriodoActual = 1000;

                }
                else if((fechaInicio.Month < DateTime.Now.Month)& (fechaInicio.Year>=DateTime.Now.Year))
                
                {
                    NumPeriodoActual = 0;
                
                }




                return NumPeriodoActual;
            }

            if (tipoPeriodo == "SEMANA")
            {
      
                //semana representa la fecha variante, que inicia con la fecha inicial del proyecto, y va incrementandose
                //semana tras semana, hasta llegar a la fecha final
                DateTime semana;

                // Rango1 es el numero de semana que representa la fecha inicial del proyecto
                int rango1 = fncObtieneNumeroSemana(fechaInicio);
               
                //Rango2 Es el numero de semana que representa la fecha final del proyecto
                int rango2 = fncObtieneNumeroSemana(fechaFinal);


                //Aqui guardamos el periodo actual, segun la fecha del servidor
                int periodo_actual = fncObtieneNumeroSemana(DateTime.Now.Date);

                             
                long diferencia = DateAndTime.DateDiff(DateInterval.Day, fechaInicio, fechaFinal);
                semana = fechaInicio;
                                
                diferencia = diferencia / 7;

                for (cont_periodos = 0; cont_periodos <= diferencia; cont_periodos++)
                {
                    if (semana.DayOfWeek == DayOfWeek.Monday)
                    {
                       semana = semana.AddDays(7);
                    }
                    else if (semana.DayOfWeek == DayOfWeek.Tuesday)
                    {
                       semana = semana.AddDays(6);
                    }
                    else if (semana.DayOfWeek == DayOfWeek.Wednesday)
                    {
                       semana = semana.AddDays(5);
                    }
                    else if (semana.DayOfWeek == DayOfWeek.Thursday)
                    {
                       semana = semana.AddDays(4);
                    }
                    else if (semana.DayOfWeek == DayOfWeek.Friday)
                    {
                       semana = semana.AddDays(3);
                    }

                    if ((rango1 + cont_periodos == periodo_actual) &(fechaInicio.Year==DateTime.Now.Date.Year))
                    {
                        return cont_periodos;
                        break;
                    }
                }

                    if (((rango1 + cont_periodos)> rango2) &(fechaFinal.Year< DateTime.Now.Date.Year))
                    {
                        NumPeriodoActual = 1000;
                    }

                    else if (((rango1 + cont_periodos) > rango2) & (fechaFinal.Year > DateTime.Now.Date.Year))
                    {
                        NumPeriodoActual = 0;
                    }
                    else if (((rango1 + cont_periodos) > rango2) & (fechaFinal.Year == DateTime.Now.Date.Year))
                    {
                        NumPeriodoActual = 1000;
                    }
           }
                   return NumPeriodoActual;
        }

           
        /// <summary>
        /// Esta funcion regresa las tres primeras letras del nombre de mes pasado como número
        /// </summary>
        /// <param name="Num_mes"> Es el número del mes que se pasara a su abreviacion en letras</param>
        /// <returns>regresa una cadena de caracteres. Ejemplo:
        /// si Num_mes = 1..... regresar Ene
        /// si Num_mes = 2......regresa Feb
        /// </returns>

        string fncMes(int Num_mes)
        {
            string mes = "";

            if (Num_mes == 1)
            {
                mes = "ENE";

            }
            else if (Num_mes == 2)
            {
                mes = "FEB";

            }
            else if (Num_mes == 3)
            {
                mes = "MAR";

            }
            else if (Num_mes == 4)
            {
                mes = "ABR";

            }
            else if (Num_mes == 5)
            {
                mes = "MAY";

            }
            else if (Num_mes == 6)
            {
                mes = "JUN";

            }
            else if (Num_mes == 7)
            {
                mes = "JUL";

            }
            else if (Num_mes == 8)
            {
                mes = "AGO";

            }
            else if (Num_mes == 9)
            {
                mes = "SEP";

            }
            else if (Num_mes == 10)
            {
                mes = "OCT";

            }
            else if (Num_mes == 11)
            {
                mes = "NOV";

            }
            else if (Num_mes == 12)
            {
                mes = "DIC";

            }
            return mes;
        }
        
         /// <summary>
        /// Esta función regresa el numero de la semana, que representa la fecha ingresada
        /// </summary>
        /// <param name="Fecha">fecha completa, de la cual se sabra el numero de semana</param>
        /// <returns>regresa un entero (maximo 2 cifras) que representa en numero de la semana de la fecha ingresada</returns>
        private static int fncObtieneNumeroSemana(DateTime Fecha)
        {
            System.Globalization.GregorianCalendar calendar = new System.Globalization.GregorianCalendar();
            return calendar.GetWeekOfYear(Fecha, System.Globalization.DateTimeFormatInfo.CurrentInfo.CalendarWeekRule, DayOfWeek.Monday);

        }

    } 
 