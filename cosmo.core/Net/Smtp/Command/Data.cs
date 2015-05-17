﻿using System;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el comando DATA.
   /// </summary>
   public class Data : SmtpCommand
   {
      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Data"; }
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
