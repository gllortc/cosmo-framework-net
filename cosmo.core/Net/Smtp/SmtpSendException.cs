using System;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa una excepción para el envio de mensajes del servicio Smtp.
   /// </summary>
   public class SmtpSendException : SmtpException
   {
      /// <summary>
      /// Devuelve una instancia de SmtpSendException.
      /// </summary>
      public SmtpSendException() { }

      /// <summary>
      /// Devuelve una instancia de SmtpSendException.
      /// </summary>
      public SmtpSendException(String inMessage) : base(inMessage) { }
   }

}
