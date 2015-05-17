using System;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el comando QUIT.
   /// </summary>
   public class Quit : SmtpCommand
   {
      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Quit"; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return this.Name;
      }
   }

}
