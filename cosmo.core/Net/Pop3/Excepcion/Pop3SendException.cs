using System;

namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// The exception that is thrown when send request is fail.
   /// </summary>
   public class Pop3SendException : Pop3Exception
   {
      /// <summary>
      /// Devuelve una instancia de Pop3SendException.
      /// </summary>
      public Pop3SendException() { }

      /// <summary>
      /// Devuelve una instancia de Pop3SendException.
      /// </summary>
      public Pop3SendException(String inMessage) : base(inMessage) { }
   }

}
