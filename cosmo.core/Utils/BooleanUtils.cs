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

      public static bool ToBoolean(string value, bool defaultValue)
      {
         if (value == null) return defaultValue;

         if (value.Trim().ToLower().Equals("false") || value.Trim().Equals("0")) return false;
         if (value.Trim().ToLower().Equals("true") || value.Trim().Equals("1")) return true;

         return defaultValue;
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
