using System.Text.RegularExpressions;

namespace Cosmo.Cms.Forums.Parsers
{
   /// <summary>
   /// Parser para mensajes de texto ASCII.
   /// </summary>
   public class CSForumPlainTextParser
   {
      /// <summary>
      /// Transforma los códigos BBCode de un mensaje a XHTML
      /// </summary>
      /// <param name="msgtext">Texto original del mensaje</param>
      /// <returns>Una cadena XHTML que contiene el mensaje a visualizar</returns>
      public static string Parse(string content)
      {
         // Entidades HTML (evita errores de seguridad)
         content = content.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");

         //-----------------------------------------------------------------------
         // Saltos de línea
         //-----------------------------------------------------------------------
         content = content.Replace("\n", "<br />\n");

         //-----------------------------------------------------------------------
         // Enlaces
         //-----------------------------------------------------------------------
         content = content.Replace("http", " http");
         content = content.Replace("HTTP", " http");
         content = content.Replace("https", " https");
         content = content.Replace("HTTPS", " https");
         content = (" " + content);

         string pattern = "(^|[\\n ])([\\w]+?://[^ ,\"\\s<]*)";
         content = Regex.Replace(content, pattern, new MatchEvaluator(SimpleLinkMatchEvaluator));

         pattern = "(^|[\\n ])((www|ftp)\\.[^ ,\"\\s<]*)";
         content = Regex.Replace(content, pattern, new MatchEvaluator(LinkMatchEvaluator));

         pattern = "(^|[\\n ])([a-z0-9&\\-_.]+?)@([\\w\\-]+\\.([\\w\\-\\.]+\\.)*[\\w]+)";
         content = Regex.Replace(content, pattern, new MatchEvaluator(MailMatchEvaluator));

         //-----------------------------------------------------------------------
         // Emoticons
         //-----------------------------------------------------------------------
         content = content.Replace(":-)", "<img src=\"images/eico_01.png\" alt=\"Emoticon\" />").
                           Replace(":-D", "<img src=\"images/eico_02.png\" alt=\"Emoticon\" />").
                           Replace(";-)", "<img src=\"images/eico_03.png\" alt=\"Emoticon\" />").
                           Replace(":-O", "<img src=\"images/eico_04.png\" alt=\"Emoticon\" />").
                           Replace(":-P", "<img src=\"images/eico_05.png\" alt=\"Emoticon\" />").
                           Replace(":-(", "<img src=\"images/eico_06.png\" alt=\"Emoticon\" />");

         return content;
      }

      #region Private Members

      static string SimpleLinkMatchEvaluator(Match match)
      {
         // if (match.Groups["code"].Length <= 0) return "";

         string xhtml = "";
         xhtml += match.Groups[1].Value + "<a href=\"" + match.Groups[2].Value + "\" target=\"_blank\">" + match.Groups[2].Value + "</a>";

         return xhtml;
      }

      static string LinkMatchEvaluator(Match match)
      {
         // if (match.Groups["code"].Length <= 0) return "";

         string xhtml = "";
         xhtml += match.Groups[1].Value + "<a href=\"http://" + match.Groups[2].Value + "\" target=\"_blank\">" + match.Groups[2].Value + "</a>";

         return xhtml;
      }

      static string MailMatchEvaluator(Match match)
      {
         // if (match.Groups["code"].Length <= 0) return "";

         string xhtml = "";
         xhtml += match.Groups[1].Value + "<a href=\"mailto:" + Cosmo.Net.Mail.Obfuscate(match.Groups[2].Value + "@" + match.Groups[3].Value) + "\" target=\"_blank\">" + Cosmo.Net.Mail.Obfuscate(match.Groups[2].Value + "@" + match.Groups[3].Value) + "</a>";

         return xhtml;
      }

      #endregion

   }
}
