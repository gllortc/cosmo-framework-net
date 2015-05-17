using System;
using System.Collections.Generic;
using Cosmo.Net.Mail;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa la respuesta de un envio de correo.
   /// </summary>
   public class SendMailResult
   {
      private Boolean zSendSuccessful = false;
      private List<MailAddress> zInvalidMailAddressList = new List<MailAddress>();

      /// <summary>
      /// Devuelve una instancia de SendMailResult.
      /// </summary>
      public SendMailResult() { }

      /// <summary>
      /// Devuelve una instancia de SendMailResult.
      /// </summary>
      /// <param name="inSendSuccessful">Indica si el envío ha sido satisfactório.</param>
      public SendMailResult(Boolean inSendSuccessful)
      {
         this.zSendSuccessful = inSendSuccessful;
      }
      
      /// <summary>
      /// Indica si el envío ha sido satisfactório.
      /// </summary>
      public Boolean SendSuccessful
      {
         get { return this.zSendSuccessful; }
         set { this.zSendSuccessful = value; }
      }

      /// <summary>
      /// Contiene la lista de direcciones no válidas.
      /// </summary>
      public List<MailAddress> InvalidMailAddressList
      {
         get { return this.zInvalidMailAddressList; }
      }
   }

}
