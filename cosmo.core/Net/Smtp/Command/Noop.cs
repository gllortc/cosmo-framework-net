using System;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el comando NOOP.
   /// </summary>
   public class Noop : SmtpCommand
   {
      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Noop"; }
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
