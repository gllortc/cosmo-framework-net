using System;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa una excepción para la recepción de mensajes del servicio Smtp.
   /// </summary>
   public class SmtpReceiveException : SmtpException
   {
      /// <summary>
      /// Devuelve una instancia de SmtpReceiveException.
      /// </summary>
      public SmtpReceiveException() { }

      /// <summary>
      /// Devuelve una instancia de SmtpReceiveException.
      /// </summary>
      public SmtpReceiveException(String inMessage) : base(inMessage) { }
   }

}
