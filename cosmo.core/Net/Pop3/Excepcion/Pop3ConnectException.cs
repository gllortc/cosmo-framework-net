using System;

namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// The exception that is thrown when connection error occurs.
   /// </summary>
   public class Pop3ConnectException : Pop3Exception
   {
      /// <summary>
      /// Devuelve una instancia de Pop3ConnectException.
      /// </summary>
      public Pop3ConnectException() { }

      /// <summary>
      /// Devuelve una instancia de Pop3ConnectException.
      /// </summary>
      public Pop3ConnectException(String inMessage) : base(inMessage) { }
   }

}
