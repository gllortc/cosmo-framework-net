using System;
using System.Text.RegularExpressions;

namespace Cosmo.Utils
{
   /// <summary>
   /// Implementa una clase con utilidades para tratar números
   /// </summary>
   public class Number
   {
      /// <summary>
      /// Indica si una cadena de texto es numérica.
      /// </summary>
      /// <param name="number">Cadena a comprobar.</param>
      /// <returns>True/False dependiendo del tipo de cadena.</returns>
      public static bool IsNumeric(string number)
      {
         try
         {
            Int32.Parse(number);
         }
         catch
         {
            return false;
         }

         return true;
      }

      /// <summary>
      /// Convierte una cadéna de texto numérica en un valor entero.
      /// </summary>
      /// <param name="number">Cadena a convertir.</param>
      /// <returns>Un valor entero o -1 si falla la conversion.</returns>
      public static int StrToInt(string number)
      {
         try
         {
            return Int32.Parse(number);
         }
         catch
         {
            return -1;
         }
      }

      /// <summary>
      /// Retorna la parte numérica de una cadena de texto.
      /// </summary>
      /// <param name="text">Cadena a transformar.</param>
      /// <returns>Un valor entero.</returns>
      public static int Val(string text)
      {
         return StrToInt(Regex.Match(text, @"\d+").Value);
      }

      /// <summary>
      /// Determina si una instancia es un entero.
      /// </summary>
      /// <param name="objectType">Tipo de la instancia a evaluar.</param>
      /// <returns><c>true</c> si la instancia pertenece a un valor entero o <c>false</c> en cualquier otro caso.</returns>
      public static bool IsIntegerType(Type objectType)
      {
         if (objectType == typeof(uint)) return true;
         if (objectType == typeof(int)) return true;
         if (objectType == typeof(byte)) return true;
         if (objectType == typeof(sbyte)) return true;
         if (objectType == typeof(short)) return true;
         if (objectType == typeof(long)) return true;
         if (objectType == typeof(Int16)) return true;
         if (objectType == typeof(Int32)) return true;
         if (objectType == typeof(Int64)) return true;
         if (objectType == typeof(SByte)) return true;
         if (objectType == typeof(Byte)) return true;

         return false;
      }

      /// <summary>
      /// Determina si una instancia es un valor decimal.
      /// </summary>
      /// <param name="objectType">Tipo de la instancia a evaluar.</param>
      /// <returns><c>true</c> si la instancia pertenece a un valor decimal o <c>false</c> en cualquier otro caso.</returns>
      public static bool IsDecimalType(Type objectType)
      {
         if (objectType == typeof(double)) return true;
         if (objectType == typeof(decimal)) return true;
         if (objectType == typeof(float)) return true;
         if (objectType == typeof(Decimal)) return true;
         if (objectType == typeof(Double)) return true;
         if (objectType == typeof(Single)) return true;

         return false;
      }
   }
}
