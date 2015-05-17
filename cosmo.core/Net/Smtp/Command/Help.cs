using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el ocmando HELP.
   /// </summary>
   public class Help : SmtpCommand
   {
      private String zCommandName = string.Empty;

      /// <summary>
      /// Devuelve una instancia de Help.
      /// </summary> 
      public Help() { }

      /// <summary>
      /// Devuelve una instancia de Help.
      /// </summary> 
      public Help(String inCommandName)
      {
         this.zCommandName = inCommandName;
      }

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Help"; }
      }

      /// <summary>
      /// Nombre del comando para el que se desea obtener ayuda.
      /// </summary>
      public String CommandName
      {
         get { return this.zCommandName; }
         set { this.zCommandName = value; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return String.Format("{0} {1}", this.Name, this.CommandName);
      }
   }

}
