using System;
using System.Collections.Generic;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el comando EHLO.
   /// </summary>
   public class Ehlo : SmtpCommand
   {
      private String zDomain = string.Empty;

      /// <summary>
      /// Devuelve una instancia de Ehlo.
      /// </summary>
      public Ehlo(String inDomain)
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

      /// <summary>
      /// Implementa el resultado de la ejecución del comando.
      /// </summary>
      public class Result
      {
         private String zKeyword = string.Empty;
         private List<String> zParameters = new List<string>();

         /// <summary>
         /// Devuelve una instancia de Result.
         /// </summary>
         public Result() { }

         /// <summary>
         /// Keyword.
         /// </summary>
         public String Keyword
         {
            get { return this.zKeyword; }
            set { this.zKeyword = value; }
         }

         /// <summary>
         /// Parámetros.
         /// </summary>
         public List<String> Parameters
         {
            get { return this.zParameters; }
         }
      }
   }

}
