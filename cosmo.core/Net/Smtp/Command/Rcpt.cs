using System;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el comando RCPT.
   /// </summary>
   public class Rcpt : SmtpCommand
   {
      private String zForwardPath = "";

      /// <summary>
      /// Devuelve una instancia de Rcpt.
      /// </summary>
      public Rcpt(String inForwardPath)
      {
         this.zForwardPath = inForwardPath;
      }

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Rcpt To:"; }
      }

      /// <summary>
      /// ForwardPath.
      /// </summary>
      public String ForwardPath
      {
         get { return this.zForwardPath; }
         set { this.zForwardPath = value; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return String.Format("{0}{1}", this.Name, this.ForwardPath);
      }
   }

}
