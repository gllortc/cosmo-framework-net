using System;
using System.Text.RegularExpressions;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa el resultado de un comando Smtp.
   /// </summary>
   public class SmtpCommandResultLine
   {
      private Boolean zInvalidFormat = false;
      private Int32 zStatusCodeNumber = 0;
      private SmtpCommandResultCode zStatusCode = SmtpCommandResultCode.None;
      private Boolean zHasNextLine = false;
      private String zMessage = "";

      /// <summary>
      /// Devuelve una instancia de SmtpCommandResultLine.
      /// </summary>
      /// <param name="inLine"></param>
      public SmtpCommandResultLine(String inLine)
      {
         Match m = Regex.Match(inLine, @"(?<StatusCode>[0-9]{3})(?<HasNextLine>[\s-]{0,1})(?<Message>.*)", RegexOptions.IgnoreCase);
         this.zInvalidFormat = !Int32.TryParse(m.Groups["StatusCode"].Value, out this.zStatusCodeNumber);
         this.zStatusCode = (SmtpCommandResultCode)this.zStatusCodeNumber;
         this.zHasNextLine = m.Groups["HasNextLine"].Value == "-";
         this.zMessage = m.Groups["Message"].Value;
      }

      /// <summary>
      /// Indica si el formato no es válido.
      /// </summary>
      public Boolean InvalidFormat
      {
         get { return this.zInvalidFormat; }
      }

      /// <summary>
      /// Código del resultado del comando.
      /// </summary>
      public Int32 CodeNumber
      {
         get { return this.zStatusCodeNumber; }
      }

      /// <summary>
      /// Código de estado del comando.
      /// </summary>
      public SmtpCommandResultCode StatusCode
      {
         get { return this.zStatusCode; }
      }

      /// <summary>
      /// Indica si exista otra línea de comando.
      /// </summary>
      public Boolean HasNextLine
      {
         get { return this.zHasNextLine; }
      }

      /// <summary>
      /// Mensaje.
      /// </summary>
      public String Message
      {
         get { return this.zMessage; }
      }

      /// <summary>
      /// SMTPƒT[ƒo[‚©‚ç‚ÌƒŒƒXƒ|ƒ“ƒX‚ÌŒ‹‰Ê‚ğ¦‚·ƒRƒ}ƒ“ƒh‚Ì‘®‚ª³‚µ‚¢‚©‚Ç‚¤‚©‚ğ¦‚·’l‚ğæ“¾‚µ‚Ü‚·B
      /// </summary>
      /// <param name="inLine"></param>
      /// <returns></returns>
      public static Boolean CheckFormat(String inLine)
      {
         Match m = Regex.Match(inLine, @"(?<Code>[0-9]{3})(?<HasNextLine>[\s-]{1})(?<Message>.*)", RegexOptions.IgnoreCase);
         return m.Success;
      }
   }

}
