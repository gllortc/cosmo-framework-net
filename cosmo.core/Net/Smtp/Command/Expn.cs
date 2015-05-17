using System;

namespace Cosmo.Net.Smtp.Command
{

   /// <summary>
   /// Representa el comando EXPN.
   /// </summary>
   public class Expn : SmtpCommand
   {
      private String zMailingList = string.Empty;

      /// <summary>
      /// Devuelve una instancia de Expn.
      /// </summary>
      public Expn(String inMailingList)
      {
         this.zMailingList = inMailingList;
      }

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Expn"; }
      }

      /// <summary>
      /// MailingList.
      /// </summary>
      public String MailingList
      {
         get { return this.zMailingList; }
         set { this.zMailingList = value; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return String.Format("{0} {1}", this.Name, this.MailingList);
      }
   }

}
