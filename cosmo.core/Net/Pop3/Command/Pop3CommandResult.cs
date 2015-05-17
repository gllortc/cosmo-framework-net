using System;
using Cosmo.Net.Mail;

namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// Implementa el resultado de un comando del servicio Pop3.
   /// </summary>
   public class Pop3CommandResult
   {
      private String zText = string.Empty;
      private Boolean zOk = true;

      /// <summary>
      /// Devuelve una instancia de Pop3CommandResult.
      /// </summary>
      public Pop3CommandResult(String inText)
      {
         this.zOk = MailParser.IsResponseOk(inText);
         this.zText = inText;
      }

      /// <summary>
      /// Texto (mensaje).
      /// </summary>
      public String Text
      {
         get { return this.zText; }
      }

      /// <summary>
      /// Indica si el comando ha sido ejecutado con éxito.
      /// </summary>
      public Boolean Ok
      {
         get { return this.zOk; }
      }
   }

}
