using System;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el ocmando MAIL.
   /// </summary>
   public class Mail : SmtpCommand
   {
      private String zReversePath = string.Empty;

      /// <summary>
      /// Devuelve una instancia de Mail.
      /// </summary>
      public Mail(String inReversePath)
      {
         this.zReversePath = inReversePath;
      }

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Mail From:"; }
      }

      /// <summary>
      /// ReversePath.
      /// </summary>
      public String ReversePath
      {
         get { return this.zReversePath; }
         set { this.zReversePath = value; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return String.Format("{0}{1}", this.Name, this.ReversePath);
      }
   }

}
