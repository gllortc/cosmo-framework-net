using System.Text.RegularExpressions;

namespace Cosmo.Utils.Html.Parsers
{
   /// <summary>
   /// Parser para mensajes formateados con BBCodes.
   /// </summary>
   /// <remarks>
   /// Basado en el <a href="http://eksith.wordpress.com/2009/01/14/aspnet-bbcode-c/">artículo siguiente</a>.
   /// </remarks>
   public class BBCodeParserOld
   {
      /// <summary>Nombre de la clase que se aplica por defecto a las imágenes incluidas en los mensajes del foro.</summary>
      public static string IMG_CLASS = "msg-img";

      /// <summary>Establece el número máximo de niveles de citación (quote).</summary>
      private static int MAX_QUOTE_LEVELS = 3;

      /// <summary>
      /// Converts the input plain-text BBCode to HTML output and replacing carriage returns
      /// and spaces with <br /> and   etc...
      /// Recommended: Use this function only during storage and updates.
      /// Keep a seperate field in your database for HTML formatted content and raw text.
      /// An optional third, plain text field, with no formatting info will make full text searching
      /// more accurate.
      /// E.G. BodyText(with BBCode for textarea/WYSIWYG), BodyPlain(plain text for searching),
      /// BodyHtml(formatted HTML for output pages)
      /// </summary>
      public static string Parse(string content)
      {
         // Basic tag stripping for this example (PLEASE EXTEND THIS!)   
         content = StripTags(content);

         // Formatea saltos de línea
         content = content.Replace("\r", "").
                           Replace("\n\n", "</p><p style=\"padding-top:10px;\">").
                           Replace("\n", "</p><p style=\"padding-top:10px;\">").
                           Replace("\t", "   ");

         content = MatchReplace(@"\[b(?:\s*)\]((.|\n)*?)\[/b(?:\s*)\]", "<strong>$1</strong>", content);
         content = MatchReplace(@"\[i(?:\s*)\]((.|\n)*?)\[/i(?:\s*)\]", "<em>$1</em>", content);
         content = MatchReplace(@"\[u(?:\s*)\]((.|\n)*?)\[/u(?:\s*)\]", "<span style=\"text-decoration:underline;\">$1</span>", content);         
         content = MatchReplace(@"\[del\]([^\]]+)\[\/del\]", "<span style=\"text-decoration:line-through\">$1</span>", content);
        
         // Colors and sizes   
         // content = MatchReplace(@"\[color=(#[0-9a-fA-F]{6}|[a-z-]+)]([^\]]+)\[\/color\]", "<span style=\"color:$1;\">$2</span>", content);   
         // content = MatchReplace(@"\[size=([2-5])]([^\]]+)\[\/size\]", "<span style=\"font-size:$1em; font-weight:normal;\">$2</span>", content);
         content = MatchReplace(@"\[color=(#[0-9a-fA-F]{6}|[a-z-]+)]([^\]]+)\[\/color\]", "$2", content);
         content = MatchReplace(@"\[size=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/size(?:\s*)\]", "$3", content);
        
         // Text alignment   
         // content = MatchReplace(@"\[left\]([^\]]+)\[\/left\]", "<span style=\"text-align:left\">$1</span>", content);   
         // content = MatchReplace(@"\[right\]([^\]]+)\[\/right\]", "<span style=\"text-align:right\">$1</span>", content);   
         // content = MatchReplace(@"\[center\]([^\]]+)\[\/center\]", "<span style=\"text-align:center\">$1</span>", content);   
         // content = MatchReplace(@"\[justify\]([^\]]+)\[\/justify\]", "<span style=\"text-align:justify\">$1</span>", content);   
        
         // HTML Links   
         content = MatchReplace(@"\[url(?:\s*)\]www\.(.*?)\[/url(?:\s*)\]", "<a class=\"link-ext\" href=\"http://www.$1\" target=\"_blank\" title=\"Abrir enlace\">$1</a> ", content);
         content = MatchReplace(@"\[url(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=\"link-ext\" href=\"$1\" target=\"_blank\" title=\"Abrir enlace\">$1</a>", content);
         content = MatchReplace(@"\[url=""((.|\n)*?)(?:\s*)""\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=\"link-ext\" href=\"$1\" target=\"_blank\" title=\"Abrir enlace\">$3</a> ", content);
         content = MatchReplace(@"\[url=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=\"link-ext\" href=\"$1\" target=\"_blank\" title=\"Abrir enlace\">$3</a> ", content);
         content = MatchReplace(@"\[link(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a class=\"link-ext\" href=\"$1\" target=\"_blank\" title=\"Abrir enlace\">$1</a> ", content);
         content = MatchReplace(@"\[link=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a class=\"link-ext\" href=\"$1\" target=\"_blank\" title=\"Abrir enlace\">$3</a> ", content);
         content = MatchReplace(@"\[email(?:\s*)\]((.|\n)*?)\[/email(?:\s*)\]", "<a class=\"mail\" href=\"mailto:$1\">$1</a> ", content);

         // Images   
         content = MatchReplace(@"\[img(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img class=\"" + BBCodeParserOld.IMG_CLASS + "\" src=\"$1\" border=\"0\" alt=\"Imagen externa\" />", content);
         content = MatchReplace(@"\[img align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img class=\"" + BBCodeParserOld.IMG_CLASS + "\" src=\"$3\" border=\"0\" align=\"$1\" alt=\"Imagen externa\" />", content);
         content = MatchReplace(@"\[img=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img class=\"" + BBCodeParserOld.IMG_CLASS + "\" width=\"$1\" height=\"$3\" src=\"$5\" border=\"0\" alt=\"Imagen externa\" />", content);

         // Lists   
         // content = MatchReplace(@"\[*\]([^\[]+)", "<li>$1</li>", content);   
         // content = MatchReplace(@"\[list\]([^\]]+)\[\/list\]", "<ul>$1</ul><p>", content);   
         // content = MatchReplace(@"\[list=1\]([^\]]+)\[\/list\]", "</p><ol>$1</ol><p>", content);   
        
         // Headers   
         // content = MatchReplace(@"\[h1\]([^\]]+)\[\/h1\]", "<h1>$1</h1>", content);   
         // content = MatchReplace(@"\[h2\]([^\]]+)\[\/h2\]", "<h2>$1</h2>", content);   
         // content = MatchReplace(@"\[h3\]([^\]]+)\[\/h3\]", "<h3>$1</h3>", content);   
         // content = MatchReplace(@"\[h4\]([^\]]+)\[\/h4\]", "<h4>$1</h4>", content);   
         // content = MatchReplace(@"\[h5\]([^\]]+)\[\/h5\]", "<h5>$1</h5>", content);   
         // content = MatchReplace(@"\[h6\]([^\]]+)\[\/h6\]", "<h6>$1</h6>", content);   

         // Horizontal rule
         content = MatchReplace(@"\[hr\]", "<hr />", content);
        
         // Set a maximum quote depth (In this case, hard coded to 3)   
         for (int i = 1; i < MAX_QUOTE_LEVELS; i++)   
         {   
             // Quotes
             content = MatchReplace(@"\[quote=([^\]]+)@([^\]]+)|([^\]]+)]([^\]]+)\[\/quote\]", 
                                     "</p><div class=\"block\"><blockquote><cite>$1 <a href=\"$3\" target=\"_blank\">wrote</a> on $2</cite><hr /><p>$4</p></blockquote></div></p><p>", content);
             content = MatchReplace(@"\[quote=([^\]]+)@([^\]]+)]([^\]]+)\[\/quote\]", 
                                     "</p><div class=\"block\"><blockquote><cite>$1 wrote on $2</cite><hr /><p>$3</p></blockquote></div><p>", content);
             content = MatchReplace(@"\[quote=([^\]]+)]([^\]]+)\[\/quote\]", 
                                     "</p><div class=\"block\"><blockquote><cite>$1 wrote</cite><hr /><p>$2</p></blockquote></div><p>", content);
             content = MatchReplace(@"\[quote\]([^\]]+)\[\/quote\]", 
                                     "</p><div class=\"block\"><blockquote><p>$1</p></blockquote></div><p>", content);
         }  

         // The following markup is for embedded video -->

         // YouTube
         /*content = MatchReplace(@"\[video\]http:\/\/([a-zA-Z]+.)youtube.com\/watch\?v=([a-zA-Z0-9_\-]+)\[\/video\]",
                                 "<object width=\"605\" height=\"405\">\n" +
                                 "  <param name=\"movie\" value=\"http://www.youtube.com/v/$2\" />\n" +
                                 "  <param name=\"allowFullScreen\" value=\"true\" />\n" + 
                                 "  <embed src=\"http://www.youtube.com/v/$2\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"605\" height=\"405\"></embed>\n" + 
                                 "</object>\n", content);*/

         content = MatchReplace(@"\[video\]http:\/\/([a-zA-Z]+.)youtube.com\/watch\?v=([a-zA-Z0-9_\-]+)\[\/video\]",
                                 "<object type=\"application/x-shockwave-flash\" style=\"width:605px;height:405px\" data=\"http://www.youtube.com/v/$2\" >" +
                                 "  <param name=\"movie\" value=\"http://www.youtube.com/v/$2\" />" +
                                 "</object>", content);

         content = MatchReplace(@"\[youtube\]http:\/\/([a-zA-Z]+.)youtube.com\/watch\?v=([a-zA-Z0-9_\-]+)\[\/youtube\]",
                                 "<object type=\"application/x-shockwave-flash\" style=\"width:605px;height:405px\" data=\"http://www.youtube.com/v/$2\" >" +
                                 "  <param name=\"movie\" value=\"http://www.youtube.com/v/$2\" />" +
                                 "</object>", content);

         // LiveVideo
         content = MatchReplace(@"\[video\]http:\/\/([a-zA-Z]+.)livevideo.com\/video\/([a-zA-Z0-9_\-]+)\/([a-zA-Z0-9]+)\/([a-zA-Z0-9_\-]+).aspx\[\/video\]",
                                 "<object width=\"605\" height=\"405\">\n" + 
                                 "  <embed src=\"http://www.livevideo.com/flvplayer/embed/$3\" type=\"application/x-shockwave-flash\" quality=\"high\" width=\"605\" height=\"405\" wmode=\"transparent\"></embed>" + 
                                 "</object>", content);

         // LiveVideo (There are two types of links for LV)   
         content = MatchReplace(@"\[video\]http:\/\/([a-zA-Z]+.)livevideo.com\/video\/([a-zA-Z0-9]+)\/([a-zA-Z0-9_\-]+).aspx\[\/video\]",
                                 "<object width=\"605\" height=\"405\">" + 
                                 "  <embed src=\"http://www.livevideo.com/flvplayer/embed/$2&autostart=0\" type=\"application/x-shockwave-flash\" quality=\"high\" width=\"605\" height=\"405\" wmode=\"transparent\"></embed>" + 
                                 "</object>", content);   

         // Metacafe
         content = MatchReplace(@"\[video\]http\:\/\/([a-zA-Z]+.)metacafe.com\/watch\/([0-9]+)\/([a-zA-Z0-9_]+)/\[\/video\]",
                                 "<object width=\"605\" height=\"405\">" + 
                                 "  <embed flashVars=\"playerVars=showStats=no|autoPlay=no|\" src=\"http://www.metacafe.com/fplayer/$2/$3.swf\" width=\"605\" height=\"405\" wmode=\"transparent\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\"></embed>" + 
                                 "</object>", content);

         // Google and Wikipedia container links   
         content = MatchReplace(@"\[google\]([^\]]+)\[\/google\]", "<a class=\"link-google\" href=\"http://www.google.com/search?q=$1\" target=\"_blank\" title=\"Búsqueda en Google\">$1</a>", content);
         content = MatchReplace(@"\[wikipedia\]([^\]]+)\[\/wikipedia\]", "<a class=\"link-wiki\" href=\"http://www.wikipedia.org/wiki/$1\" target=\"_blank\" title=\"Búsqueda en Wikipedia (EN)\">$1</a>", content);   

         // Put the content in a paragraph
         // content = "</p><p>" + content + "</p>";

         // Clean up a few potential markup problems
         /*content = content.Replace("t", "    ")   
             .Replace("  ", "  ")   
             .Replace("<br />", "")   
             .Replace("<p><br />", "</p><p>")   
             .Replace("</p><p><blockquote>", "<blockquote>")   
             .Replace("</blockquote></blockquote></p>", "")   
             .Replace("<p></p>", "")   
             .Replace("<p><ul></ul></p>", "<ul>")   
             .Replace("<p></p></ul>", "")   
             .Replace("<p><ol></ol></p>", "<ol>")   
             .Replace("<p></p></ol>", "")   
             .Replace("<p><li>", "</li><li><p>")   
             .Replace("</p></li></p>", "");   */

         //-----------------------------------------------------------------------
         // Emoticons
         //-----------------------------------------------------------------------
         content = content.Replace(":-)", "<img src=\"templates/shared/eico_01.png\" alt=\"Emoticon\" />").
                           Replace(":)", "<img src=\"templates/shared/eico_01.png\" alt=\"Emoticon\" />").
                           Replace(":-D", "<img src=\"templates/shared/eico_02.png\" alt=\"Emoticon\" />").
                           Replace(":D", "<img src=\"templates/shared/eico_02.png\" alt=\"Emoticon\" />").
                           Replace(";-)", "<img src=\"templates/shared/eico_03.png\" alt=\"Emoticon\" />").
                           Replace(";)", "<img src=\"templates/shared/eico_03.png\" alt=\"Emoticon\" />").
                           Replace(":-O", "<img src=\"templates/shared/eico_04.png\" alt=\"Emoticon\" />").
                           Replace(":O", "<img src=\"templates/shared/eico_04.png\" alt=\"Emoticon\" />").
                           Replace(":-P", "<img src=\"templates/shared/eico_05.png\" alt=\"Emoticon\" />").
                           Replace(":P", "<img src=\"templates/shared/eico_05.png\" alt=\"Emoticon\" />").
                           Replace(":-(", "<img src=\"templates/shared/eico_06.png\" alt=\"Emoticon\" />").
                           Replace(":(", "<img src=\"templates/shared/eico_06.png\" alt=\"Emoticon\" />");

         return content; 
      }

      #region Private Members

      /// <summary>
      /// Elimina cualquier posible TAG HTML.
      /// </summary>
      /// <param name="content">Cadena de texto a tratar.</param>
      /// <returns>Una cadena sin ningún TAG.</returns>
      private static string StripTags(string content)
      {
         Regex regex = null;

         // Elimina código XHTML/CSS de Word 2007
         regex = new Regex(@"<!--([\s\S]*?)-->|&amp;lt;!--([\s\S]*?)--&amp;gt;|<style>[\s\S]*?<\/style>/g", RegexOptions.Multiline);
         content = regex.Replace(content, string.Empty);

         // Elimina TAGs HTML
         // @"< [^>]+>"
         regex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);
         content = regex.Replace(content, string.Empty);

         return content;
      }

      private static string MatchReplace(string pattern, string match, string content)
      {
	      return MatchReplace(pattern, match, content, false, false, false);
      }

      private static string MatchReplace(string pattern, string match, string content, bool multi)
      {
	      return MatchReplace(pattern, match, content, multi, false, false);
      }

      private static string MatchReplace(string pattern, string match, string content, bool multi, bool white)
      {
	      return MatchReplace(pattern, match, content, multi, white);
      }

      /// <summary>
      /// Match and replace a specific pattern with formatted text
      /// </summary>
      /// <param name="pattern">Regular expression pattern</param>
      /// <param name="match">Match replacement</param>
      /// <param name="content">Text to format</param>
      /// <param name="multi">Multiline text (optional)</param>
      /// <param name="white">Ignore white space (optional)</param>
      /// <param name="cult">Indica si el reemplazo no es dependiente d ela cultura.</param>
      /// <returns>HTML Formatted from the original BBCode</returns>
      private static string MatchReplace(string pattern, string match, string content, bool multi, bool white, bool cult)
      {
	      if (multi && white && cult)
		      return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
	      else if (multi && white)
		      return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnoreCase);
	      else if (multi && cult)
		      return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
	      else if (white && cult)
		      return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);
	      else if (multi)
		      return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline);
	      else if (white)
		      return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
	      else if (cult)
		      return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	      // Default
	      return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase);
      }

      #endregion

   }
}
