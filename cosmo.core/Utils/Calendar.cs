using System;

namespace Cosmo.Utils
{
   /// <summary>
   /// Summary description for CSUtils
   /// </summary>
   public class Calendar
   {
      /// <summary>
      /// Enumera los tipos de intervalo soportados por la clase Calendar.
      /// </summary>
      public enum DateInterval
      {
         /// <summary>Segundos</summary>
         Second,
         /// <summary>Minutos</summary>
         Minute,
         /// <summary>Horas</summary>
         Hour,
         /// <summary>Dias</summary>
         Day,
         /// <summary>Semanas</summary>
         Week,
         /// <summary>Meses</summary>
         Month,
         /// <summary>Trimestres</summary>
         Quarter,
         /// <summary>Años</summary>
         Year
      }

      /// <summary>
      /// Permite quantificar la diferencia entre dos fechas/horas
      /// </summary>
      /// <param name="Interval">Medida de la diferencia entre las dos fechas</param>
      /// <param name="StartDate">Fecha/hora inicial</param>
      /// <param name="EndDate">Fecha/hora final</param>
      /// <returns>El número de unidades (determinado por el parámetro Interval) que marcan la diferencia</returns>
      public static long DateDiff(DateInterval Interval, System.DateTime StartDate, System.DateTime EndDate)
      {
         long lngDateDiffValue = 0;

         System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
         switch (Interval)
         {
            case DateInterval.Day:
               lngDateDiffValue = (long)TS.Days;
               break;
            case DateInterval.Hour:
               lngDateDiffValue = (long)TS.TotalHours;
               break;
            case DateInterval.Minute:
               lngDateDiffValue = (long)TS.TotalMinutes;
               break;
            case DateInterval.Month:
               lngDateDiffValue = (long)(TS.Days / 30);
               break;
            case DateInterval.Quarter:
               lngDateDiffValue = (long)((TS.Days / 30) / 3);
               break;
            case DateInterval.Second:
               lngDateDiffValue = (long)TS.TotalSeconds;
               break;
            case DateInterval.Week:
               lngDateDiffValue = (long)(TS.Days / 7);
               break;
            case DateInterval.Year:
               lngDateDiffValue = (long)(TS.Days / 365);
               break;
         }

         return (lngDateDiffValue);
      }

      /// <summary>
      /// Determina si una instancia es una hora/fecha.
      /// </summary>
      /// <param name="objectType">Tipo de la instancia a evaluar.</param>
      /// <returns><c>true</c> si la instancia pertenece a un valor de hora o fecha o <c>false</c> en cualquier otro caso.</returns>
      public static bool IsDateType(Type objectType)
      {
         if (objectType == typeof(DateTime)) return true;

         return false;
      }
   }
}
