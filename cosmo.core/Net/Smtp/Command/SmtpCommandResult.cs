using System;
using System.Text;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa una clase que representa el resultado de un comando Smtp.
   /// </summary>
   public class SmtpCommandResult
   {
      private SmtpCommandResultCode zStatusCode = SmtpCommandResultCode.None;
      private String zMessage = "";

      /// <summary>
      /// Devuelve una instancia de SmtpCommandResult.
      /// </summary>
      public SmtpCommandResult(SmtpCommandResultLine[] inLines)
      {
         StringBuilder sb = new StringBuilder();
         if (inLines.Length == 0)
         {
            throw new ArgumentException("inLine must not be zero length.");
         }

         this.zStatusCode = inLines[0].StatusCode;
         for (int i = 0; i < inLines.Length; i++)
         {
            sb.Append(inLines[i].Message);
            sb.AppendLine();
         }
         this.zMessage = sb.ToString();
      }

      /// <summary>
      /// Devuelve el código de estado.
      /// </summary>
      public SmtpCommandResultCode StatusCode
      {
         get { return this.zStatusCode; }
      }

      /// <summary>
      /// Devuelve el mensaje.
      /// </summary>
      public String Message
      {
         get { return this.zMessage; }
      }
   }

}
