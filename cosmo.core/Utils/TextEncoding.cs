using System;
using System.Text.RegularExpressions;

namespace Cosmo.Utils
{
   /// <summary>
   /// Implementa una clase con utilidades para Http.
   /// </summary>
   public class TextEncoding
   {
      /// <summary>
      /// Codifica una cadena de texto en base64.
      /// </summary>
      /// <param name="originalString">Cadena de texto a codificar.</param>
      /// <returns>Cadena codificada.</returns>
      public string Base64Encode(string originalString)
      {
         try
         {
            byte[] encData_byte = new byte[originalString.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(originalString);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Descodifica una cadena base64.
      /// </summary>
      /// <param name="encodedString">Cadena codificada.</param>
      /// <returns>Texto original.</returns>
      public string Base64Decode(string encodedString)
      {
         try
         {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(encodedString);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Reemplaza los car�cteres especiales por entidades HTML.
      /// </summary>
      /// <param name="originalString">Texto original.</param>
      /// <param name="carryReturn">Car�cter o cadena a reemplazar por los saltos de l�nea del texto.</param>
      /// <returns>La cadena compatible HTML.</returns>
      public string HtmlEncode(string originalString, string carryReturn)
      {
         originalString = originalString.Replace("&", "&amp;");
         originalString = originalString.Replace("<", "&lt;");
         originalString = originalString.Replace(">", "&gt;");
         originalString = originalString.Replace("�", "&iexcl;");
         originalString = originalString.Replace("�", "&cent;");
         originalString = originalString.Replace("�", "&pound;");
         originalString = originalString.Replace("�", "&yen;");
         originalString = originalString.Replace("�", "&sect;");
         originalString = originalString.Replace("�", "&uml;");
         originalString = originalString.Replace("�", "&copy;");
         originalString = originalString.Replace("�", "&laquo;");
         originalString = originalString.Replace("�", "&not;");
         originalString = originalString.Replace("�", "&reg;");
         originalString = originalString.Replace("�", "&deg;");
         originalString = originalString.Replace("�", "&plusmn;");
         originalString = originalString.Replace("�", "&acute;");
         originalString = originalString.Replace("�", "&micro;");
         originalString = originalString.Replace("�", "&para;");
         originalString = originalString.Replace("�", "&middot;");
         originalString = originalString.Replace("�", "&cedil;");
         originalString = originalString.Replace("�", "&raquo;");
         originalString = originalString.Replace("�", "&iquest;");
         originalString = originalString.Replace("�", "&Agrave;");
         originalString = originalString.Replace("�", "&Aacute;");
         originalString = originalString.Replace("�", "&Acirc;");
         originalString = originalString.Replace("�", "&Atilde;");
         originalString = originalString.Replace("�", "&Auml;");
         originalString = originalString.Replace("�", "&Aring;");
         originalString = originalString.Replace("�", "&AElig;");
         originalString = originalString.Replace("�", "&Ccedil;");
         originalString = originalString.Replace("�", "&Egrave;");
         originalString = originalString.Replace("�", "&Eacute;");
         originalString = originalString.Replace("�", "&Ecirc;");
         originalString = originalString.Replace("�", "&Euml;");
         originalString = originalString.Replace("�", "&Igrave;");
         originalString = originalString.Replace("�", "&Iacute;");
         originalString = originalString.Replace("�", "&Icirc;");
         originalString = originalString.Replace("�", "&Iuml;");
         originalString = originalString.Replace("�", "&Ntilde;");
         originalString = originalString.Replace("�", "&Ograve;");
         originalString = originalString.Replace("�", "&Oacute;");
         originalString = originalString.Replace("�", "&Ocirc;");
         originalString = originalString.Replace("�", "&Otilde;");
         originalString = originalString.Replace("�", "&Ouml;");
         originalString = originalString.Replace("�", "&Oslash;");
         originalString = originalString.Replace("�", "&Ugrave;");
         originalString = originalString.Replace("�", "&Uacute;");
         originalString = originalString.Replace("�", "&Ucirc;");
         originalString = originalString.Replace("�", "&Uuml;");
         originalString = originalString.Replace("�", "&szlig;");
         originalString = originalString.Replace("�", "&agrave;");
         originalString = originalString.Replace("�", "&aacute;");
         originalString = originalString.Replace("�", "&acirc;");
         originalString = originalString.Replace("�", "&atilde;");
         originalString = originalString.Replace("�", "&auml;");
         originalString = originalString.Replace("�", "&aring;");
         originalString = originalString.Replace("�", "&aelig;");
         originalString = originalString.Replace("�", "&ccedil;");
         originalString = originalString.Replace("�", "&egrave;");
         originalString = originalString.Replace("�", "&eacute;");
         originalString = originalString.Replace("�", "&ecirc;");
         originalString = originalString.Replace("�", "&euml;");
         originalString = originalString.Replace("�", "&igrave;");
         originalString = originalString.Replace("�", "&iacute;");
         originalString = originalString.Replace("�", "&icirc;");
         originalString = originalString.Replace("�", "&iuml;");
         originalString = originalString.Replace("�", "&ntilde;");
         originalString = originalString.Replace("�", "&ograve;");
         originalString = originalString.Replace("�", "&oacute;");
         originalString = originalString.Replace("�", "&ocirc;");
         originalString = originalString.Replace("�", "&otilde;");
         originalString = originalString.Replace("�", "&ouml;");
         originalString = originalString.Replace("�", "&divide;");
         originalString = originalString.Replace("�", "&oslash;");
         originalString = originalString.Replace("�", "&ugrave;");
         originalString = originalString.Replace("�", "&uacute;");
         originalString = originalString.Replace("�", "&ucirc;");
         originalString = originalString.Replace("�", "&uuml;");
         originalString = originalString.Replace("�", "&yuml;");
         originalString = originalString.Replace("�", "&#130;");
         originalString = originalString.Replace("�", "&#131;");
         originalString = originalString.Replace("�", "&#132;");
         originalString = originalString.Replace("�", "&#133;");
         originalString = originalString.Replace("�", "&#134;");
         originalString = originalString.Replace("�", "&#135;");
         originalString = originalString.Replace("�", "&#136;");
         originalString = originalString.Replace("�", "&#137;");
         originalString = originalString.Replace("�", "&#139;");
         originalString = originalString.Replace("�", "&#140;");
         originalString = originalString.Replace("�", "&#145;");
         originalString = originalString.Replace("�", "&#146;");
         originalString = originalString.Replace("�", "&#147;");
         originalString = originalString.Replace("�", "&#148;");
         originalString = originalString.Replace("�", "&#149;");
         originalString = originalString.Replace("�", "&#150;");
         originalString = originalString.Replace("�", "&#151;");
         originalString = originalString.Replace("�", "&#152;");
         originalString = originalString.Replace("�", "&#153;");
         originalString = originalString.Replace("�", "&#155;");
         originalString = originalString.Replace("�", "&#156;");
         originalString = originalString.Replace("�", "&#159;");

         if (!carryReturn.Equals(""))
            originalString = originalString.Replace(Environment.NewLine, carryReturn + Environment.NewLine);

         return originalString;
      }

      /// <summary>
      /// Reemplaza los car�cteres especiales por entidades HTML.
      /// </summary>
      /// <param name="originalString">Texto original.</param>
      /// <returns>La cadena compatible HTML.</returns>
      public string HtmlEncode(string originalString)
      {
         return HtmlEncode(originalString, "");
      }

      /// <summary>
      /// Reemplaza las entidades HTML a car�cteres UTF-8.
      /// </summary>
      /// <param name="originalString">Texto HTML original.</param>
      /// <param name="replaceCR">Indica si se deben reemplazar los saltos de l�nea BR a su equivalente UTF-8.</param>
      /// <returns>El texto UTF-8.</returns>
      public string HtmlDecode(string originalString, bool replaceCR)
      {
         originalString = originalString.Replace("&amp;", "&");
         originalString = originalString.Replace("&lt;", "<");
         originalString = originalString.Replace("&gt;", ">");
         originalString = originalString.Replace("&iexcl;", "�");
         originalString = originalString.Replace("&cent;", "�");
         originalString = originalString.Replace("&pound;", "�");
         originalString = originalString.Replace("&yen;", "�");
         originalString = originalString.Replace("&sect;", "�");
         originalString = originalString.Replace("&uml;", "�");
         originalString = originalString.Replace("&copy;", "�");
         originalString = originalString.Replace("&laquo;", "�");
         originalString = originalString.Replace("&not;", "�");
         originalString = originalString.Replace("&reg;", "�");
         originalString = originalString.Replace("&deg;", "�");
         originalString = originalString.Replace("&plusmn;", "�");
         originalString = originalString.Replace("&acute;", "�");
         originalString = originalString.Replace("&micro;", "�");
         originalString = originalString.Replace("&para;", "�");
         originalString = originalString.Replace("&middot;", "�");
         originalString = originalString.Replace("&cedil;", "�");
         originalString = originalString.Replace("&raquo;", "�");
         originalString = originalString.Replace("&iquest;", "�");
         originalString = originalString.Replace("&Agrave;", "�");
         originalString = originalString.Replace("&Aacute;", "�");
         originalString = originalString.Replace("&Acirc;", "�");
         originalString = originalString.Replace("&Atilde;", "�");
         originalString = originalString.Replace("&Auml;", "�");
         originalString = originalString.Replace("&Aring;", "�");
         originalString = originalString.Replace("&AElig;", "�");
         originalString = originalString.Replace("&Ccedil;", "�");
         originalString = originalString.Replace("&Egrave;", "�");
         originalString = originalString.Replace("&Eacute;", "�");
         originalString = originalString.Replace("&Ecirc;", "�");
         originalString = originalString.Replace("&Euml;", "�");
         originalString = originalString.Replace("&Igrave;", "�");
         originalString = originalString.Replace("&Iacute;", "�");
         originalString = originalString.Replace("&Icirc;", "�");
         originalString = originalString.Replace("&Iuml;", "�");
         originalString = originalString.Replace("&Ntilde;", "�");
         originalString = originalString.Replace("&Ograve;", "�");
         originalString = originalString.Replace("&Oacute;", "�");
         originalString = originalString.Replace("&Ocirc;", "�");
         originalString = originalString.Replace("&Otilde;", "�");
         originalString = originalString.Replace("&Ouml;", "�");
         originalString = originalString.Replace("&Oslash;", "�");
         originalString = originalString.Replace("&Ugrave;", "�");
         originalString = originalString.Replace("&Uacute;", "�");
         originalString = originalString.Replace("&Ucirc;", "�");
         originalString = originalString.Replace("&Uuml;", "�");
         originalString = originalString.Replace("&szlig;", "�");
         originalString = originalString.Replace("&agrave;", "�");
         originalString = originalString.Replace("&aacute;", "�");
         originalString = originalString.Replace("&acirc;", "�");
         originalString = originalString.Replace("&atilde;", "�");
         originalString = originalString.Replace("&auml;", "�");
         originalString = originalString.Replace("&aring;", "�");
         originalString = originalString.Replace("&aelig;", "�");
         originalString = originalString.Replace("&ccedil;", "�");
         originalString = originalString.Replace("&egrave;", "�");
         originalString = originalString.Replace("&eacute;", "�");
         originalString = originalString.Replace("&ecirc;", "�");
         originalString = originalString.Replace("&euml;", "�");
         originalString = originalString.Replace("&igrave;", "�");
         originalString = originalString.Replace("&iacute;", "�");
         originalString = originalString.Replace("&icirc;", "�");
         originalString = originalString.Replace("&iuml;", "�");
         originalString = originalString.Replace("&ntilde;", "�");
         originalString = originalString.Replace("&ograve;", "�");
         originalString = originalString.Replace("&oacute;", "�");
         originalString = originalString.Replace("&ocirc;", "�");
         originalString = originalString.Replace("&otilde;", "�");
         originalString = originalString.Replace("&ouml;", "�");
         originalString = originalString.Replace("&divide;", "�");
         originalString = originalString.Replace("&oslash;", "�");
         originalString = originalString.Replace("&ugrave;", "�");
         originalString = originalString.Replace("&uacute;", "�");
         originalString = originalString.Replace("&ucirc;", "�");
         originalString = originalString.Replace("&uuml;", "�");
         originalString = originalString.Replace("&yuml;", "�");
         originalString = originalString.Replace("&#130;", "�");
         originalString = originalString.Replace("&#131;", "�");
         originalString = originalString.Replace("&#132;", "�");
         originalString = originalString.Replace("&#133;", "�");
         originalString = originalString.Replace("&#134;", "�");
         originalString = originalString.Replace("&#135;", "�");
         originalString = originalString.Replace("&#136;", "�");
         originalString = originalString.Replace("&#137;", "�");
         originalString = originalString.Replace("&#139;", "�");
         originalString = originalString.Replace("&#140;", "�");
         originalString = originalString.Replace("&#145;", "�");
         originalString = originalString.Replace("&#146;", "�");
         originalString = originalString.Replace("&#147;", "�");
         originalString = originalString.Replace("&#148;", "�");
         originalString = originalString.Replace("&#149;", "�");
         originalString = originalString.Replace("&#150;", "�");
         originalString = originalString.Replace("&#151;", "�");
         originalString = originalString.Replace("&#152;", "�");
         originalString = originalString.Replace("&#153;", "�");
         originalString = originalString.Replace("&#155;", "�");
         originalString = originalString.Replace("&#156;", "�");
         originalString = originalString.Replace("&#159;", "�");

         if (replaceCR)
         {
            originalString = originalString.Replace("<br>", Environment.NewLine);
            originalString = originalString.Replace("<BR>", Environment.NewLine);
            originalString = originalString.Replace("<br/>", Environment.NewLine);
            originalString = originalString.Replace("<BR/>", Environment.NewLine);
            originalString = originalString.Replace("<br />", Environment.NewLine);
            originalString = originalString.Replace("<BR />", Environment.NewLine);
         }

         return originalString;
      }

      /// <summary>
      /// Reemplaza las car�cteres especiales MIME a UTF-8.
      /// </summary>
      /// <param name="originalString">Texto original con car�cteres MIME.</param>
      /// <param name="replaceHTMLEntities">Indica si se deben reemplazar los indicadores de entidad HTML.</param>
      /// <returns>De3vuelve el texto en UTF-8.</returns>
      public string MimeDecode(string originalString, bool replaceHTMLEntities)
      {
         // Traduce los c�digos 858
         originalString = originalString.Replace("=20", " ");
         originalString = originalString.Replace("=C7", "�");
         originalString = originalString.Replace("=FC", "�");
         originalString = originalString.Replace("=E9", "�");
         originalString = originalString.Replace("=E2", "�");
         originalString = originalString.Replace("=E4", "�");
         originalString = originalString.Replace("=E0", "�");
         originalString = originalString.Replace("=E7", "�");
         originalString = originalString.Replace("=EA", "�");
         originalString = originalString.Replace("=EB", "�");
         originalString = originalString.Replace("=E8", "�");
         originalString = originalString.Replace("=EF", "�");
         originalString = originalString.Replace("=EE", "�");
         originalString = originalString.Replace("=EC", "�");
         originalString = originalString.Replace("=C4", "�");
         originalString = originalString.Replace("=C9", "�");
         originalString = originalString.Replace("=F4", "�");
         originalString = originalString.Replace("=F6", "�");
         originalString = originalString.Replace("=F2", "�");
         originalString = originalString.Replace("=FB", "�");
         originalString = originalString.Replace("=F9", "�");
         originalString = originalString.Replace("=A3", "�");
         originalString = originalString.Replace("=E1", "�");
         originalString = originalString.Replace("=ED", "�");
         originalString = originalString.Replace("=F3", "�");
         originalString = originalString.Replace("=FA", "�");
         originalString = originalString.Replace("=F1", "�");
         originalString = originalString.Replace("=D1", "�");
         originalString = originalString.Replace("=AB", "�");
         originalString = originalString.Replace("=BB", "�");
         originalString = originalString.Replace("=C1", "�");
         originalString = originalString.Replace("=C2", "�");
         originalString = originalString.Replace("=C0", "�");
         originalString = originalString.Replace("=A5", "�");
         originalString = originalString.Replace("=CA", "�");
         originalString = originalString.Replace("=CB", "�");
         originalString = originalString.Replace("=C8", "�");
         originalString = originalString.Replace("=20AC", "�");
         originalString = originalString.Replace("=CD", "�");
         originalString = originalString.Replace("=CE", "�");
         originalString = originalString.Replace("=CF", "�");
         originalString = originalString.Replace("=CC", "�");
         originalString = originalString.Replace("=A6", "|");
         originalString = originalString.Replace("=D3", "�");
         originalString = originalString.Replace("=D4", "�");
         originalString = originalString.Replace("=D2", "�");
         originalString = originalString.Replace("=DA", "�");
         originalString = originalString.Replace("=DB", "�");
         originalString = originalString.Replace("=D9", "�");
         originalString = originalString.Replace("=B4", "'");
         originalString = originalString.Replace("=92", "'");
         originalString = originalString.Replace("=B8", ",");
         originalString = originalString.Replace("=AD", " ");
         originalString = originalString.Replace("=BF", "�");
         originalString = originalString.Replace("=85", "...");
         originalString = originalString.Replace("=93", "\"");
         originalString = originalString.Replace("=94", "\"");

         if (replaceHTMLEntities)
         {
            originalString = originalString.Replace(">", "&gt;");
            originalString = originalString.Replace("<", "&lt;");
            originalString = originalString.Replace("\"", "&quot;");
         }

         return originalString;
      }
   }

   #region class BBCodeParser

   /// <summary>
   /// Implementa un parser para c�digo BBCode.
   /// </summary>
   public static class BBCodeParser
   {
      /// <summary>
      /// Parsea un texto que contenga c�digos BBCode y lo convierte en XHTML v�lido W3C.
      /// </summary>
      /// <param name="text">Texto original a parsear.</param>
      /// <returns>El c�digo XHTML resultante.</returns>
      public static string Parse(string text)
      {
         int checkNoLink = 0;
         int checkATAG = 0;
         int checkIMG = 0;
         string final = text;
         string section = String.Empty;
         Regex regex;
         MatchCollection theMatches;

         // Reemplaza las entidades XHTML
         final = final.Replace("<", "&lt;");
         final = final.Replace(">", "&gt;");
         final = final.Replace("&", "&amp;");
         final = final.Replace("\n", "<br /> ");
         final = final.Replace(":-)", "<img src=\"images/eico_01.gif\" alt=\"Emoticon\" />");
         final = final.Replace(":-D", "<img src=\"images/eico_02.gif\" alt=\"Emoticon\" />");
         final = final.Replace(";-)", "<img src=\"images/eico_03.gif\" alt=\"Emoticon\" />");
         final = final.Replace(":-O", "<img src=\"images/eico_04.gif\" alt=\"Emoticon\" />");
         final = final.Replace(":-P", "<img src=\"images/eico_05.gif\" alt=\"Emoticon\" />");
         final = final.Replace(":-(", "<img src=\"images/eico_06.gif\" alt=\"Emoticon\" />");

         //=============================================
         // BBCodes
         //=============================================

         // It�lica (formato: [i]emphatized text[/i])
         regex = new Regex("\\[i\\]([^\\]]+)\\[\\/i\\]", RegexOptions.IgnoreCase);
         final = regex.Replace(final, "<em>$1</em>");

         // Negrita (formato: [b]emphatized text[/b])
         regex = new Regex("\\[b\\]([^\\]]+)\\[\\/b\\]", RegexOptions.IgnoreCase);
         final = regex.Replace(final, "<strong>$1</strong>");

         // Im�genes (formato: [img]http://www.google.com/intl/en_ALL/images/logo.gif[/img])
         regex = new Regex("\\[img\\]([^\\]]+)\\[\\/img\\]", RegexOptions.IgnoreCase);
         final = regex.Replace(final, "<img src=\"$1\" alt=\"Im�gen incrustada\" />");

         // Enlaces (formato: [url=http://www.google.com/intl/en_ALL/images/logo.gif]Click here[/url])
         regex = new Regex("\\[url=([^\\]]+)\\]([^\\]]+)\\[\\/url\\]", RegexOptions.IgnoreCase);
         final = regex.Replace(final, "<a href=\"$1\" target=\"_blank\">$2</a>");

         //=============================================
         // Detecta enlaces y correos electr�nicos
         //=============================================

         // Switch out periods within a <nolink> region to prevent processing
         if (final.Contains("nolink>")) checkNoLink = 1;
         if (checkNoLink == 1)
         {
            regex = new Regex(@"<nolink>(.*?)</nolink>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            theMatches = regex.Matches(final);
            for (int index = 0; index < theMatches.Count; index++)
            {
               final = final.Replace(theMatches[index].ToString(), theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));
            }
         }

         // Make email addresses mailto links
         if (final.Contains("@"))
         {
            regex = new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            theMatches = regex.Matches(final);
            if (theMatches.Count > 0) checkATAG = 1;
            for (int index = 0; index < theMatches.Count; index++)
            {
               final = final.Replace(theMatches[index].ToString(), "<a href=\"mailto:" + theMatches[index].ToString() + "\">" + theMatches[index].ToString() + "</a>");
            }
         }

         if (checkATAG == 0)
         {
            if (final.Contains("<a")) checkATAG = 1;
         }

         // Switch out periods within a <a> region to prevent processing
         if (checkATAG == 1)
         {
            regex = new Regex(@"<a(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            theMatches = regex.Matches(final);
            for (int index = 0; index < theMatches.Count; index++)
            {
               final = final.Replace(theMatches[index].ToString(), theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));
            }
         }
         if (final.Contains("<img ")) checkIMG = 1;

         // Switch out periods within a <img> tags to prevent processing
         if (checkIMG == 1)
         {
            regex = new Regex(@"<img (.*?)>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            theMatches = regex.Matches(final);
            for (int index = 0; index < theMatches.Count; index++)
            {
               final = final.Replace(theMatches[index].ToString(), theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));
            }
         }

         // Switch out periods within numeric values that appear to be valid domain names
         Regex tags = new Regex(@"[^\.\da-zA-Z\-\?\&\=]{1,}[^\.\d\?\&\=]*([\d]*\.[\d]{1,}([\.][\d]{1,})*)[^\.a-zA-Z\-]", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
         theMatches = tags.Matches(final);
         if (theMatches.Count > 0) checkATAG = 1;
         for (int index = 0; index < theMatches.Count; index++)
         {
            final = final.Replace(theMatches[index].ToString(), theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));
         }

         // Identify all potential URLs and domain names and make them hyperlinks
         tags = new Regex(@"([a-zA-Z0-9\:/\-]*[a-zA-Z0-9\-_]\.[a-zA-Z0-9\-_][a-zA-Z0-9\-_][a-zA-Z0-9\?\=&#_\-/\.]*[^<>,;\.\s\)\(\]\[\""])", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
         theMatches = tags.Matches(final);
         if (theMatches.Count > 0) checkATAG = 1;
         for (int index = 0; index < theMatches.Count; index++)
         {
            // Reemplaza los enlaces
            section = theMatches[index].ToString();
            if (!section.Contains("://")) section = "http://" + section;

            // A�ade un control de detecci�n de URLs m�s fino que el propuesto
            if (Cosmo.Net.Url.IsValid(section))
            {
               final = final.Replace(theMatches[index].ToString(), "<a href=\"" + section.Replace(".", "[[[pk:period]]]") + "\" target=\"_blank\">" + theMatches[index].ToString().Replace(".", "[[[pk:period]]]") + "</a>");
            }
         }

         // Clear out escape sequences and <nolink></nolink> tags
         if (checkATAG == 1 || checkIMG == 1) final = final.Replace("[[[pk:period]]]", ".");
         if (checkNoLink == 1)
         {
            final = final.Replace("<nolink>", string.Empty);
            final = final.Replace("</nolink>", string.Empty);
         }
         return final;
      }
   }

   #endregion

}
