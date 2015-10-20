using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmo.Utils
{
   public class BooleanUtils
   {
      public static bool ToBoolean(string value)
      {
         return ToBoolean(value, false);
      }

      /// <summary>
      /// Convert a string value to boolean.
      /// </summary>
      /// <param name="value">String to convert.</param>
      /// <param name="defaultValue">The default valur to return in case that teh convert process fail.</param>
      /// <returns>A boolean value.</returns>
      public static bool ToBoolean(string value, bool defaultValue)
      {
         if (string.IsNullOrWhiteSpace(value))
         {
            return defaultValue;
         }

         try
         {
            switch (value.Trim().ToLower())
            {
               case "0":
               case "false":
               case "f":
                  return false;

               case "1":
               case "true":
               case "t":
                  return true;

               default:
                  return bool.Parse(value);
            }
         }
         catch
         {
            return defaultValue;
         }
      }

      /// <summary>
      /// Check if an instance is boolean.
      /// </summary>
      /// <param name="objectType">Type to check.</param>
      /// <returns><c>true</c> if the instance if boolean or <c>false</c> in all other cases.</returns>
      public static bool IsBooleanType(Type objectType)
      {
         if (objectType == typeof(Boolean)) return true;

         return false;
      }
   }
}
