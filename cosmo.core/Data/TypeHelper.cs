using System;
using System.Globalization;

namespace Cosmo.Data
{
   /// <summary>
   /// <see cref="Type"/> helper class.
   /// </summary>
   public static class TypeHelper
   {

      #region Methods

      /// <summary>
      /// Formats a type in the official type description like [typename], [assemblyname].
      /// </summary>
      /// <param name="assembly">Assembly.</param>
      /// <param name="type">Type.</param>
      /// <returns>Type name like [typename], [assemblyname].</returns>
      public static string FormatType(string assembly, string type)
      {
         return string.Format(CultureInfo.InvariantCulture, "{0}, {1}", type, assembly);
      }

      /// <summary>
      /// Formats multiple inner types into one string.
      /// </summary>
      /// <returns>String representing a combination of all inner types.</returns>
      public static string FormatInnerTypes(string[] innerTypes)
      {
         // Declare variables
         string result = string.Empty;

         // Loop all inner types
         for (int i = 0; i < innerTypes.Length; i++)
         {
            // Add type
            result += string.Format(CultureInfo.InvariantCulture, "[{0}]", innerTypes[i]);

            // Postfix a comma if this is not the last
            if (i < innerTypes.Length - 1) result += ",";
         }

         // Return result
         return result;
      }

      #endregion

   }
}
