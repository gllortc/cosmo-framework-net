using System;
using System.Net;

namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// The exception that is represent pop3 exception and is thrown when pop3 error occurs.
   /// </summary>
   public class Pop3Exception : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de Pop3Exception.
      /// </summary>
      public Pop3Exception() { }

      /// <summary>
      /// Devuelve una instancia de Pop3Exception.
      /// </summary>
      public Pop3Exception(String inMessage) : base(inMessage) { }
   }

}