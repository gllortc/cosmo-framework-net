using System;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el comando RESET.
   /// </summary>
   public class Reset : SmtpCommand
   {
      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Reset"; }
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
