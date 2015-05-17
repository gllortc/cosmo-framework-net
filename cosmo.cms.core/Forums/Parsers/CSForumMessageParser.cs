using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace Cosmo.Cms.Forums.Parsers
{

   /// <summary>
   /// BBCode Helper allows formatting of text
   /// without the need to use Html
   /// </summary>
   [Obsolete()]
   public class CSForumMessageParser
   {
      static List<IHtmlFormatter> _formatters;

      #region Data Structures

      interface IHtmlFormatter
      {
         string Format(string data);
      }

      protected class RegexFormatter : IHtmlFormatter
      {
         private string _replace;
         private Regex _regex;

         public RegexFormatter(string pattern, string replace) : this(pattern, replace, true) { }

         public RegexFormatter(string pattern, string replace, bool ignoreCase)
         {
            RegexOptions options = RegexOptions.Compiled;

            if (ignoreCase)
            {
               options |= RegexOptions.IgnoreCase;
            }

            _replace = replace;
            _regex = new Regex(pattern, options);
         }

         public string Format(string data)
         {
            return _regex.Replace(data, _replace);
         }
      }

      protected class SearchReplaceFormatter : IHtmlFormatter
      {
         private string _pattern;
         private string _replace;

         public SearchReplaceFormatter(string pattern, string replace)
         {
            _pattern = pattern;
            _replace = replace;
         }

         public string Format(string data)
         {
            return data.Replace(_pattern, _replace);
         }
      }

      #endregion

      static CSForumMessageParser()
      {
         _formatters = new List<IHtmlFormatter>();

         _formatters.Add(new RegexFormatter(@"<(.|\n)*?>", string.Empty));

         _formatters.Add(new SearchReplaceFormatter("\r", ""));
         _formatters.Add(new SearchReplaceFormatter("\n\n", "</p><p style=\"padding-top:10px;\">"));
         _formatters.Add(new SearchReplaceFormatter("\n", "</p><p style=\"padding-top:10px;\">"));
         // _formatters.Add(new SearchReplaceFormatter("\n", "<br />"));

         _formatters.Add(new RegexFormatter(@"\[b(?:\s*)\]((.|\n)*?)\[/b(?:\s*)\]", "<strong>$1</strong>"));
         _formatters.Add(new RegexFormatter(@"\[i(?:\s*)\]((.|\n)*?)\[/i(?:\s*)\]", "<em>$1</em>"));
         _formatters.Add(new RegexFormatter(@"\[u(?:\s*)\]((.|\n)*?)\[/u(?:\s*)\]", "<span style=\"text-decoration:underline;\">$1</span>"));

         _formatters.Add(new RegexFormatter(@"\[left(?:\s*)\]((.|\n)*?)\[/left(?:\s*)]", "<div style=\"text-align:left\">$1</div>"));
         _formatters.Add(new RegexFormatter(@"\[center(?:\s*)\]((.|\n)*?)\[/center(?:\s*)]", "<div style=\"text-align:center\">$1</div>"));
         _formatters.Add(new RegexFormatter(@"\[right(?:\s*)\]((.|\n)*?)\[/right(?:\s*)]", "<div style=\"text-align:right\">$1</div>"));

         string quoteStart = "<blockquote><strong>$1 said:</strong></p><p>";
         string quoteEmptyStart = "<blockquote>";
         string quoteEnd = "</blockquote>";

         _formatters.Add(new RegexFormatter(@"\[quote=((.|\n)*?)(?:\s*)\]", quoteStart));
         _formatters.Add(new RegexFormatter(@"\[quote(?:\s*)\]", quoteEmptyStart));
         _formatters.Add(new RegexFormatter(@"\[/quote(?:\s*)\]", quoteEnd));

         _formatters.Add(new RegexFormatter(@"\[url(?:\s*)\]www\.(.*?)\[/url(?:\s*)\]", "<a class=\"bbcode-link\" href=\"http://www.$1\" target=\"_blank\" title=\"$1\">$1</a>"));
         _formatters.Add(new RegexFormatter(@"\[url(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$1</a>"));
         _formatters.Add(new RegexFormatter(@"\[url=""((.|\n)*?)(?:\s*)""\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>"));
         _formatters.Add(new RegexFormatter(@"\[url=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>"));
         _formatters.Add(new RegexFormatter(@"\[link(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$1</a>"));
         _formatters.Add(new RegexFormatter(@"\[link=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>"));

         _formatters.Add(new RegexFormatter(@"\[img(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img src=\"$1\" border=\"0\" alt=\"\" />"));
         _formatters.Add(new RegexFormatter(@"\[img align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img src=\"$3\" border=\"0\" align=\"$1\" alt=\"\" />"));
         _formatters.Add(new RegexFormatter(@"\[img=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img width=\"$1\" height=\"$3\" src=\"$5\" border=\"0\" alt=\"\" />"));

         _formatters.Add(new RegexFormatter(@"\[color=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/color(?:\s*)\]", "<span style=\"color=$1;\">$3</span>"));

         _formatters.Add(new RegexFormatter(@"\[hr(?:\s*)\]", "<hr />"));

         _formatters.Add(new RegexFormatter(@"\[email(?:\s*)\]((.|\n)*?)\[/email(?:\s*)\]", "<a href=\"mailto:$1\">$1</a>"));

         _formatters.Add(new RegexFormatter(@"\[size=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/size(?:\s*)\]", "<span style=\"font-size:$1\">$3</span>"));
         _formatters.Add(new RegexFormatter(@"\[font=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/font(?:\s*)\]", "<span style=\"font-family:$1;\">$3</span>"));
         _formatters.Add(new RegexFormatter(@"\[align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/align(?:\s*)\]", "<span style=\"text-align:$1;\">$3</span>"));
         _formatters.Add(new RegexFormatter(@"\[float=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/float(?:\s*)\]", "<span style=\"float:$1;\">$3</div>"));

         string sListFormat = "<ol class=\"bbcode-list\" style=\"list-style:{0};\">$1</ol>";

         _formatters.Add(new RegexFormatter(@"\[\*(?:\s*)]\s*([^\[]*)", "<li>$1</li>"));
         _formatters.Add(new RegexFormatter(@"\[list(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", "<ul class=\"bbcode-list\">$1</ul>"));
         _formatters.Add(new RegexFormatter(@"\[list=1(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "decimal"), false));
         _formatters.Add(new RegexFormatter(@"\[list=i(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "lower-roman"), false));
         _formatters.Add(new RegexFormatter(@"\[list=I(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "upper-roman"), false));
         _formatters.Add(new RegexFormatter(@"\[list=a(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "lower-alpha"), false));
         _formatters.Add(new RegexFormatter(@"\[list=A(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "upper-alpha"), false));

         // YouTube

         string ytobject = "";

         // XHTML compatible
         /*ytobject = "";
         ytobject += "<object type=\"application/x-shockwave-flash\" maxWidth=\"450\" maxHeight=\"366\" style=\"maxWidth:450px;maxHeight:366px;\" data=\"http://www.youtube.com/v/$1&hl=en&fs=1&rel=0&color1=0x2b405b&color2=0x6b8ab6\">\n";
         ytobject += "<param name=\"movie\" value=\"http://www.youtube.com/v/$1&hl=en&fs=1&rel=0&color1=0x2b405b&color2=0x6b8ab6\" />\n";
         ytobject += "<param name=\"allowFullScreen\" value=\"true\" />\n";
         ytobject += "<param name=\"allowscriptaccess\" value=\"always\" />\n";
         ytobject += "<img src=\"images/cs_video_broken.png\" border=\"0\" alt=\"Video (Objeto multimedia)\" />\n";
         ytobject += "</object>\n";*/

         // Estándard
         ytobject = "";
         ytobject += "<object width=\"605\" height=\"405\">\n";
         ytobject += "<param name=\"movie\" value=\"http://www.youtube-nocookie.com/v/$1&amp;hl=en&amp;fs=1&amp;rel=0&amp;color1=0x2b405b&amp;color2=0x6b8ab6&border=1\"></param>\n";
         ytobject += "<param name=\"allowFullScreen\" value=\"true\"></param>\n";
         ytobject += "<param name=\"allowscriptaccess\" value=\"always\"></param>\n";
         ytobject += "<embed src=\"http://www.youtube-nocookie.com/v/$1&amp;hl=en&amp;fs=1&amp;rel=0&amp;color1=0x2b405b&amp;color2=0x6b8ab6&border=0\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" allowfullscreen=\"true\" width=\"605\" height=\"405\"></embed>\n";
         ytobject += "</object>\n";

         _formatters.Add(new RegexFormatter(@"\[youtube\]" +                        // opening [youtube]
                                            @"(?:\. )?" +                           // optional dot-space
                                            @"(?<code>[^\s(!]+)" +                  // presume this is the src
                                            @"\s?" +                                // optional space
                                            @"\[/youtube\]" +                       // closing [/youtube]
                                            @"(?=\s|\.|,|;|\)|\||$)",               // lookahead: space or simple punctuation or end of string
                                            ytobject));
         /*_formatters.Add(new RegexFormatter(@"\[youtube\]" +               
                                            @"/(?<=\?v=)([a-zA-Z0-9_-])+/g" +       // optional dot-space
                                            @"\[/youtube\]" +                       // closing [/youtube]
                                            @"(?=\s|\.|,|;|\)|\||$)",               // lookahead: space or simple punctuation or end of string
                                            ytobject));*/
      }

      #region Static Members

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

      /// <summary>
      /// Transforma los códigos BBCode de un mensaje a XHTML
      /// </summary>
      /// <param name="msgtext">Texto original del mensaje</param>
      /// <param name="useBBcodes">Indica si debe detectar BBCodes o sólo texto</param>
      /// <returns>Una cadena XHTML que contiene el mensaje a visualizar</returns>
      public static string Format(string msgtext, bool useBBcodes)
      {
         if (useBBcodes)
         {
            foreach (IHtmlFormatter formatter in _formatters)
            {
               msgtext = formatter.Format(msgtext);
            }
         }
         else
         {
            // Entidades HTML (evita errores de seguridad)
            msgtext = msgtext.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");

            //-----------------------------------------------------------------------
            // Saltos de línea
            //-----------------------------------------------------------------------
            msgtext = msgtext.Replace("\n", "<br />\n");

            //-----------------------------------------------------------------------
            // Enlaces
            //-----------------------------------------------------------------------
            msgtext = msgtext.Replace("http", " http");
            msgtext = msgtext.Replace("HTTP", " http");
            msgtext = msgtext.Replace("https", " https");
            msgtext = msgtext.Replace("HTTPS", " https");
            msgtext = (" " + msgtext);

            string pattern = "(^|[\\n ])([\\w]+?://[^ ,\"\\s<]*)";
            msgtext = Regex.Replace(msgtext, pattern, new MatchEvaluator(SimpleLinkMatchEvaluator));

            pattern = "(^|[\\n ])((www|ftp)\\.[^ ,\"\\s<]*)";
            msgtext = Regex.Replace(msgtext, pattern, new MatchEvaluator(LinkMatchEvaluator));

            pattern = "(^|[\\n ])([a-z0-9&\\-_.]+?)@([\\w\\-]+\\.([\\w\\-\\.]+\\.)*[\\w]+)";
            msgtext = Regex.Replace(msgtext, pattern, new MatchEvaluator(MailMatchEvaluator));
         }

         //-----------------------------------------------------------------------
         // Emoticons
         //-----------------------------------------------------------------------
         msgtext = msgtext.Replace(":-)", "<img src=\"images/eico_01.gif\" alt=\"Emoticon\" />");
         msgtext = msgtext.Replace(":-D", "<img src=\"images/eico_02.gif\" alt=\"Emoticon\" />");
         msgtext = msgtext.Replace(";-)", "<img src=\"images/eico_03.gif\" alt=\"Emoticon\" />");
         msgtext = msgtext.Replace(":-O", "<img src=\"images/eico_04.gif\" alt=\"Emoticon\" />");
         msgtext = msgtext.Replace(":-P", "<img src=\"images/eico_05.gif\" alt=\"Emoticon\" />");
         msgtext = msgtext.Replace(":-(", "<img src=\"images/eico_06.gif\" alt=\"Emoticon\" />");

         return msgtext;
      }

      #endregion

   }
}
