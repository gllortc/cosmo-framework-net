using System;

namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// Representa un comando para el servicio Pop3.
   /// </summary>
   public abstract class Pop3Command
   {
      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public abstract String Name { get; }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public abstract String GetCommandString();
   }

}
