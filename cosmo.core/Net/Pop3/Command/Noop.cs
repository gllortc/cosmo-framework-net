using System;

namespace Cosmo.Net.Pop3.Command
{

   /// <summary>
   /// Representa el comando NOOP.
   /// </summary>
   public class Noop : Pop3Command
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
