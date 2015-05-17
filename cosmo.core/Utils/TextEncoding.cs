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
      /// Reemplaza los carácteres especiales por entidades HTML.
      /// </summary>
      /// <param name="originalString">Texto original.</param>
      /// <param name="carryReturn">Carácter o cadena a reemplazar por los saltos de línea del texto.</param>
      /// <returns>La cadena compatible HTML.</returns>
      public string HtmlEncode(string originalString, string carryReturn)
      {
         originalString = originalString.Replace("&", "&amp;");
         originalString = originalString.Replace("<", "&lt;");
         originalString = originalString.Replace(">", "&gt;");
         originalString = originalString.Replace("¡", "&iexcl;");
         originalString = originalString.Replace("¢", "&cent;");
         originalString = originalString.Replace("£", "&pound;");
         originalString = originalString.Replace("¥", "&yen;");
         originalString = originalString.Replace("§", "&sect;");
         originalString = originalString.Replace("¨", "&uml;");
         originalString = originalString.Replace("©", "&copy;");
         originalString = originalString.Replace("«", "&laquo;");
         originalString = originalString.Replace("¬", "&not;");
         originalString = originalString.Replace("®", "&reg;");
         originalString = originalString.Replace("°", "&deg;");
         originalString = originalString.Replace("±", "&plusmn;");
         originalString = originalString.Replace("´", "&acute;");
         originalString = originalString.Replace("µ", "&micro;");
         originalString = originalString.Replace("¶", "&para;");
         originalString = originalString.Replace("·", "&middot;");
         originalString = originalString.Replace("¸", "&cedil;");
         originalString = originalString.Replace("»", "&raquo;");
         originalString = originalString.Replace("¿", "&iquest;");
         originalString = originalString.Replace("À", "&Agrave;");
         originalString = originalString.Replace("Á", "&Aacute;");
         originalString = originalString.Replace("Â", "&Acirc;");
         originalString = originalString.Replace("Ã", "&Atilde;");
         originalString = originalString.Replace("Ä", "&Auml;");
         originalString = originalString.Replace("Å", "&Aring;");
         originalString = originalString.Replace("Æ", "&AElig;");
         originalString = originalString.Replace("Ç", "&Ccedil;");
         originalString = originalString.Replace("È", "&Egrave;");
         originalString = originalString.Replace("É", "&Eacute;");
         originalString = originalString.Replace("Ê", "&Ecirc;");
         originalString = originalString.Replace("Ë", "&Euml;");
         originalString = originalString.Replace("Ì", "&Igrave;");
         originalString = originalString.Replace("Í", "&Iacute;");
         originalString = originalString.Replace("Î", "&Icirc;");
         originalString = originalString.Replace("Ï", "&Iuml;");
         originalString = originalString.Replace("Ñ", "&Ntilde;");
         originalString = originalString.Replace("Ò", "&Ograve;");
         originalString = originalString.Replace("Ó", "&Oacute;");
         originalString = originalString.Replace("Ô", "&Ocirc;");
         originalString = originalString.Replace("Õ", "&Otilde;");
         originalString = originalString.Replace("Ö", "&Ouml;");
         originalString = originalString.Replace("Ø", "&Oslash;");
         originalString = originalString.Replace("Ù", "&Ugrave;");
         originalString = originalString.Replace("Ú", "&Uacute;");
         originalString = originalString.Replace("Û", "&Ucirc;");
         originalString = originalString.Replace("Ü", "&Uuml;");
         originalString = originalString.Replace("ß", "&szlig;");
         originalString = originalString.Replace("à", "&agrave;");
         originalString = originalString.Replace("á", "&aacute;");
         originalString = originalString.Replace("â", "&acirc;");
         originalString = originalString.Replace("ã", "&atilde;");
         originalString = originalString.Replace("ä", "&auml;");
         originalString = originalString.Replace("å", "&aring;");
         originalString = originalString.Replace("æ", "&aelig;");
         originalString = originalString.Replace("ç", "&ccedil;");
         originalString = originalString.Replace("è", "&egrave;");
         originalString = originalString.Replace("é", "&eacute;");
         originalString = originalString.Replace("ê", "&ecirc;");
         originalString = originalString.Replace("ë", "&euml;");
         originalString = originalString.Replace("ì", "&igrave;");
         originalString = originalString.Replace("í", "&iacute;");
         originalString = originalString.Replace("î", "&icirc;");
         originalString = originalString.Replace("ï", "&iuml;");
         originalString = originalString.Replace("ñ", "&ntilde;");
         originalString = originalString.Replace("ò", "&ograve;");
         originalString = originalString.Replace("ó", "&oacute;");
         originalString = originalString.Replace("ô", "&ocirc;");
         originalString = originalString.Replace("õ", "&otilde;");
         originalString = originalString.Replace("ö", "&ouml;");
         originalString = originalString.Replace("÷", "&divide;");
         originalString = originalString.Replace("ø", "&oslash;");
         originalString = originalString.Replace("ù", "&ugrave;");
         originalString = originalString.Replace("ú", "&uacute;");
         originalString = originalString.Replace("û", "&ucirc;");
         originalString = originalString.Replace("ü", "&uuml;");
         originalString = originalString.Replace("ÿ", "&yuml;");
         originalString = originalString.Replace("‚", "&#130;");
         originalString = originalString.Replace("ƒ", "&#131;");
         originalString = originalString.Replace("„", "&#132;");
         originalString = originalString.Replace("…", "&#133;");
         originalString = originalString.Replace("†", "&#134;");
         originalString = originalString.Replace("‡", "&#135;");
         originalString = originalString.Replace("ˆ", "&#136;");
         originalString = originalString.Replace("‰", "&#137;");
         originalString = originalString.Replace("‹", "&#139;");
         originalString = originalString.Replace("Œ", "&#140;");
         originalString = originalString.Replace("‘", "&#145;");
         originalString = originalString.Replace("’", "&#146;");
         originalString = originalString.Replace("“", "&#147;");
         originalString = originalString.Replace("”", "&#148;");
         originalString = originalString.Replace("•", "&#149;");
         originalString = originalString.Replace("–", "&#150;");
         originalString = originalString.Replace("—", "&#151;");
         originalString = originalString.Replace("˜", "&#152;");
         originalString = originalString.Replace("™", "&#153;");
         originalString = originalString.Replace("›", "&#155;");
         originalString = originalString.Replace("œ", "&#156;");
         originalString = originalString.Replace("Ÿ", "&#159;");

         if (!carryReturn.Equals(""))
            originalString = originalString.Replace(Environment.NewLine, carryReturn + Environment.NewLine);

         return originalString;
      }

      /// <summary>
      /// Reemplaza los carácteres especiales por entidades HTML.
      /// </summary>
      /// <param name="originalString">Texto original.</param>
      /// <returns>La cadena compatible HTML.</returns>
      public string HtmlEncode(string originalString)
      {
         return HtmlEncode(originalString, "");
      }

      /// <summary>
      /// Reemplaza las entidades HTML a carácteres UTF-8.
      /// </summary>
      /// <param name="originalString">Texto HTML original.</param>
      /// <param name="replaceCR">Indica si se deben reemplazar los saltos de línea BR a su equivalente UTF-8.</param>
      /// <returns>El texto UTF-8.</returns>
      public string HtmlDecode(string originalString, bool replaceCR)
      {
         originalString = originalString.Replace("&amp;", "&");
         originalString = originalString.Replace("&lt;", "<");
         originalString = originalString.Replace("&gt;", ">");
         originalString = originalString.Replace("&iexcl;", "¡");
         originalString = originalString.Replace("&cent;", "¢");
         originalString = originalString.Replace("&pound;", "£");
         originalString = originalString.Replace("&yen;", "¥");
         originalString = originalString.Replace("&sect;", "§");
         originalString = originalString.Replace("&uml;", "¨");
         originalString = originalString.Replace("&copy;", "©");
         originalString = originalString.Replace("&laquo;", "«");
         originalString = originalString.Replace("&not;", "¬");
         originalString = originalString.Replace("&reg;", "®");
         originalString = originalString.Replace("&deg;", "°");
         originalString = originalString.Replace("&plusmn;", "±");
         originalString = originalString.Replace("&acute;", "´");
         originalString = originalString.Replace("&micro;", "µ");
         originalString = originalString.Replace("&para;", "¶");
         originalString = originalString.Replace("&middot;", "·");
         originalString = originalString.Replace("&cedil;", "¸");
         originalString = originalString.Replace("&raquo;", "»");
         originalString = originalString.Replace("&iquest;", "¿");
         originalString = originalString.Replace("&Agrave;", "À");
         originalString = originalString.Replace("&Aacute;", "Á");
         originalString = originalString.Replace("&Acirc;", "Â");
         originalString = originalString.Replace("&Atilde;", "Ã");
         originalString = originalString.Replace("&Auml;", "Ä");
         originalString = originalString.Replace("&Aring;", "Å");
         originalString = originalString.Replace("&AElig;", "Æ");
         originalString = originalString.Replace("&Ccedil;", "Ç");
         originalString = originalString.Replace("&Egrave;", "È");
         originalString = originalString.Replace("&Eacute;", "É");
         originalString = originalString.Replace("&Ecirc;", "Ê");
         originalString = originalString.Replace("&Euml;", "Ë");
         originalString = originalString.Replace("&Igrave;", "Ì");
         originalString = originalString.Replace("&Iacute;", "Í");
         originalString = originalString.Replace("&Icirc;", "Î");
         originalString = originalString.Replace("&Iuml;", "Ï");
         originalString = originalString.Replace("&Ntilde;", "Ñ");
         originalString = originalString.Replace("&Ograve;", "Ò");
         originalString = originalString.Replace("&Oacute;", "Ó");
         originalString = originalString.Replace("&Ocirc;", "Ô");
         originalString = originalString.Replace("&Otilde;", "Õ");
         originalString = originalString.Replace("&Ouml;", "Ö");
         originalString = originalString.Replace("&Oslash;", "Ø");
         originalString = originalString.Replace("&Ugrave;", "Ù");
         originalString = originalString.Replace("&Uacute;", "Ú");
         originalString = originalString.Replace("&Ucirc;", "Û");
         originalString = originalString.Replace("&Uuml;", "Ü");
         originalString = originalString.Replace("&szlig;", "ß");
         originalString = originalString.Replace("&agrave;", "à");
         originalString = originalString.Replace("&aacute;", "á");
         originalString = originalString.Replace("&acirc;", "â");
         originalString = originalString.Replace("&atilde;", "ã");
         originalString = originalString.Replace("&auml;", "ä");
         originalString = originalString.Replace("&aring;", "å");
         originalString = originalString.Replace("&aelig;", "æ");
         originalString = originalString.Replace("&ccedil;", "ç");
         originalString = originalString.Replace("&egrave;", "è");
         originalString = originalString.Replace("&eacute;", "é");
         originalString = originalString.Replace("&ecirc;", "ê");
         originalString = originalString.Replace("&euml;", "ë");
         originalString = originalString.Replace("&igrave;", "ì");
         originalString = originalString.Replace("&iacute;", "í");
         originalString = originalString.Replace("&icirc;", "î");
         originalString = originalString.Replace("&iuml;", "ï");
         originalString = originalString.Replace("&ntilde;", "ñ");
         originalString = originalString.Replace("&ograve;", "ò");
         originalString = originalString.Replace("&oacute;", "ó");
         originalString = originalString.Replace("&ocirc;", "ô");
         originalString = originalString.Replace("&otilde;", "õ");
         originalString = originalString.Replace("&ouml;", "ö");
         originalString = originalString.Replace("&divide;", "÷");
         originalString = originalString.Replace("&oslash;", "ø");
         originalString = originalString.Replace("&ugrave;", "ù");
         originalString = originalString.Replace("&uacute;", "ú");
         originalString = originalString.Replace("&ucirc;", "û");
         originalString = originalString.Replace("&uuml;", "ü");
         originalString = originalString.Replace("&yuml;", "ÿ");
         originalString = originalString.Replace("&#130;", "‚");
         originalString = originalString.Replace("&#131;", "ƒ");
         originalString = originalString.Replace("&#132;", "„");
         originalString = originalString.Replace("&#133;", "…");
         originalString = originalString.Replace("&#134;", "†");
         originalString = originalString.Replace("&#135;", "‡");
         originalString = originalString.Replace("&#136;", "ˆ");
         originalString = originalString.Replace("&#137;", "‰");
         originalString = originalString.Replace("&#139;", "‹");
         originalString = originalString.Replace("&#140;", "Œ");
         originalString = originalString.Replace("&#145;", "‘");
         originalString = originalString.Replace("&#146;", "’");
         originalString = originalString.Replace("&#147;", "“");
         originalString = originalString.Replace("&#148;", "”");
         originalString = originalString.Replace("&#149;", "•");
         originalString = originalString.Replace("&#150;", "–");
         originalString = originalString.Replace("&#151;", "—");
         originalString = originalString.Replace("&#152;", "˜");
         originalString = originalString.Replace("&#153;", "™");
         originalString = originalString.Replace("&#155;", "›");
         originalString = originalString.Replace("&#156;", "œ");
         originalString = originalString.Replace("&#159;", "Ÿ");

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
      /// Reemplaza las carácteres especiales MIME a UTF-8.
      /// </summary>
      /// <param name="originalString">Texto original con carácteres MIME.</param>
      /// <param name="replaceHTMLEntities">Indica si se deben reemplazar los indicadores de entidad HTML.</param>
      /// <returns>De3vuelve el texto en UTF-8.</returns>
      public string MimeDecode(string originalString, bool replaceHTMLEntities)
      {
         // Traduce los códigos 858
         originalString = originalString.Replace("=20", " ");
         originalString = originalString.Replace("=C7", "Ç");
         originalString = originalString.Replace("=FC", "ü");
         originalString = originalString.Replace("=E9", "é");
         originalString = originalString.Replace("=E2", "â");
         originalString = originalString.Replace("=E4", "ä");
         originalString = originalString.Replace("=E0", "à");
         originalString = originalString.Replace("=E7", "ç");
         originalString = originalString.Replace("=EA", "ê");
         originalString = originalString.Replace("=EB", "ë");
         originalString = originalString.Replace("=E8", "è");
         originalString = originalString.Replace("=EF", "ï");
         originalString = originalString.Replace("=EE", "î");
         originalString = originalString.Replace("=EC", "ì");
         originalString = originalString.Replace("=C4", "Ä");
         originalString = originalString.Replace("=C9", "É");
         originalString = originalString.Replace("=F4", "ô");
         originalString = originalString.Replace("=F6", "ö");
         originalString = originalString.Replace("=F2", "ò");
         originalString = originalString.Replace("=FB", "û");
         originalString = originalString.Replace("=F9", "ù");
         originalString = originalString.Replace("=A3", "£");
         originalString = originalString.Replace("=E1", "á");
         originalString = originalString.Replace("=ED", "í");
         originalString = originalString.Replace("=F3", "ó");
         originalString = originalString.Replace("=FA", "ú");
         originalString = originalString.Replace("=F1", "ñ");
         originalString = originalString.Replace("=D1", "Ñ");
         originalString = originalString.Replace("=AB", "«");
         originalString = originalString.Replace("=BB", "»");
         originalString = originalString.Replace("=C1", "Á");
         originalString = originalString.Replace("=C2", "Â");
         originalString = originalString.Replace("=C0", "À");
         originalString = originalString.Replace("=A5", "¥");
         originalString = originalString.Replace("=CA", "Ê");
         originalString = originalString.Replace("=CB", "Ë");
         originalString = originalString.Replace("=C8", "È");
         originalString = originalString.Replace("=20AC", "€");
         originalString = originalString.Replace("=CD", "Í");
         originalString = originalString.Replace("=CE", "Î");
         originalString = originalString.Replace("=CF", "Ï");
         originalString = originalString.Replace("=CC", "Ì");
         originalString = originalString.Replace("=A6", "|");
         originalString = originalString.Replace("=D3", "Ó");
         originalString = originalString.Replace("=D4", "Ô");
         originalString = originalString.Replace("=D2", "Ò");
         originalString = originalString.Replace("=DA", "Ú");
         originalString = originalString.Replace("=DB", "Û");
         originalString = originalString.Replace("=D9", "Ù");
         originalString = originalString.Replace("=B4", "'");
         originalString = originalString.Replace("=92", "'");
         originalString = originalString.Replace("=B8", ",");
         originalString = originalString.Replace("=AD", " ");
         originalString = originalString.Replace("=BF", "¿");
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
   /// Implementa un parser para código BBCode.
   /// </summary>
   public static class BBCodeParser
   {
      /// <summary>
      /// Parsea un texto que contenga códigos BBCode y lo convierte en XHTML válido W3C.
      /// </summary>
      /// <param name="text">Texto original a parsear.</param>
      /// <returns>El código XHTML resultante.</returns>
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

         // Itálica (formato: [i]emphatized text[/i])
         regex = new Regex("\\[i\\]([^\\]]+)\\[\\/i\\]", RegexOptions.IgnoreCase);
         final = regex.Replace(final, "<em>$1</em>");

         // Negrita (formato: [b]emphatized text[/b])
         regex = new Regex("\\[b\\]([^\\]]+)\\[\\/b\\]", RegexOptions.IgnoreCase);
         final = regex.Replace(final, "<strong>$1</strong>");

         // Imágenes (formato: [img]http://www.google.com/intl/en_ALL/images/logo.gif[/img])
         regex = new Regex("\\[img\\]([^\\]]+)\\[\\/img\\]", RegexOptions.IgnoreCase);
         final = regex.Replace(final, "<img src=\"$1\" alt=\"Imágen incrustada\" />");

         // Enlaces (formato: [url=http://www.google.com/intl/en_ALL/images/logo.gif]Click here[/url])
         regex = new Regex("\\[url=([^\\]]+)\\]([^\\]]+)\\[\\/url\\]", RegexOptions.IgnoreCase);
         final = regex.Replace(final, "<a href=\"$1\" target=\"_blank\">$2</a>");

         //=============================================
         // Detecta enlaces y correos electrónicos
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

            // Añade un control de detección de URLs más fino que el propuesto
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
