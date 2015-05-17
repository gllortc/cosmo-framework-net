using System;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el comando HELO.
   /// </summary>
   public class Helo : SmtpCommand
   {
      private String zDomain = string.Empty;

      /// <summary>
      /// Devuelve una instancia de Helo.
      /// </summary>
      public Helo(String inDomain)
      {
         this.zDomain = inDomain;
      }

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Ehlo"; }
      }

      /// <summary>
      /// Dominio.
      /// </summary>
      public String Domain
      {
         get { return this.zDomain; }
         set { this.zDomain = value; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return String.Format("{0} {1}", this.Name, this.Domain);
      }
   }

}
