using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa una excepción para la conexión del servicio Smtp.
   /// </summary>
   public class SmtpConnectException : SmtpException
   {
      /// <summary>
      /// Devuelve una instancia de SmtpConnectException.
      /// </summary>
      public SmtpConnectException() { }

      /// <summary>
      /// Devuelve una instancia de SmtpConnectException.
      /// </summary>
      public SmtpConnectException(String inMessage) : base(inMessage) { }
   }

}
