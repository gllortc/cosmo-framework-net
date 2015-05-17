using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Class contains "lot" of static methods tools for this library.
   /// </summary>
   internal static class FaxTools
   {
      /// <summary>
      /// Check if a <see cref="String"/> is null or empty by trimming his space on left and right.
      /// </summary>
      /// <param name="str">String to be check.</param>
      /// <returns>True if the string is null or empty, false in otherwise.</returns>
      public static bool IsNullOrEmpty(string str)
      {
         if (str == null)
            return true;

         str = str.Trim();

         return string.IsNullOrEmpty(str);
      }


      /// <summary>
      /// Create a fax exception with a message with a inner <see cref="Win32Exception"/>.
      /// </summary>
      /// <param name="message">Message associate with the new exception.</param>
      /// <returns>A new <see cref="FaxException"/> with the <see cref="Win32Exception"/> inner exception
      /// if <see cref="Marshal.GetLastWin32Error()"/> return a value different to 0.</returns>
      public static FaxException CreateFaxException(string message)
      {
         if (Marshal.GetLastWin32Error() != 0)
         {
            return new FaxException(message, new Win32Exception(Marshal.GetLastWin32Error()));
         }

         return new FaxException(message);
      }
   }

}
