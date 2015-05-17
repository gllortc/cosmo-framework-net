using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// The exception that is thrown when receive response error occurs.
   /// </summary>
   public class Pop3ReceiveException : Pop3Exception
   {
      /// <summary>
      /// Devuelve una instancia de Pop3ReceiveException.
      /// </summary>
      public Pop3ReceiveException() { }

      /// <summary>
      /// Devuelve una instancia de Pop3ReceiveException.
      /// </summary>
      public Pop3ReceiveException(String inMessage) : base(inMessage) { }
   }

}
