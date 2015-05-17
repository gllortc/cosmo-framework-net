using System;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el comando VRFY.
   /// </summary>
   public class Vrfy : SmtpCommand
   {
      private String zUserName = "";

      /// <summary>
      /// Devuelve una instancia de Vrfy.
      /// </summary>
      public Vrfy(String inUserName)
      {
         this.zUserName = inUserName;
      }

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Vrfy"; }
      }

      /// <summary>
      /// Nombre del usuario que se desea verificar.
      /// </summary>
      public String UserName
      {
         get { return this.zUserName; }
         set { this.zUserName = value; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return String.Format("{0} {1}", this.Name, this.UserName);
      }
   }

}
