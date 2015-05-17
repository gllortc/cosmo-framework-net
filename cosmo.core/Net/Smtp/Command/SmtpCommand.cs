using System;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa un interface para los comandos del servicio Smtp.
   /// </summary>
   public abstract class SmtpCommand
   {
      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public abstract String Name { get; }
      
      /// <summary>
      /// Obtiene el texto de ejecuci�n del comando.
      /// </summary>
      public abstract String GetCommandString();
   }

}
