using CodeKicker.BBCode;
using System.Collections.Generic;

namespace Cosmo.Utils.Html
{

   /// <summary>
   /// BBCode parser.
   /// </summary>
   public class BBCodeTextParser
   {

      #region Methods

      /// <summary>
      /// Convierte un texto con tags bbCodes a HTML.
      /// </summary>
      /// <param name="bbCodeText">Texto original que contiene códigos bbCodes.</param>
      /// <returns>Una cadena HTML.</returns>
      public string ParseText(string bbCodeText)
      {
         string xhtml = bbCodeText.Trim();
         var parser = new CodeKicker.BBCode.BBCodeParser(new[]
                {
                    new CodeKicker.BBCode.BBTag("b", "<b>", "</b>"), 
                    new CodeKicker.BBCode.BBTag("i", "<span style=\"font-style:italic;\">", "</span>"), 
                    new CodeKicker.BBCode.BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"), 
                    new CodeKicker.BBCode.BBTag("code", "<pre class=\"prettyprint\">", "</pre>"), 
                    new CodeKicker.BBCode.BBTag("img", "<img src=\"${content}\" />", "", false, true), 
                    new CodeKicker.BBCode.BBTag("quote", "<blockquote>", "</blockquote>"),
                    new CodeKicker.BBCode.BBTag("cr", "<br/>", string.Empty),
                    // new CodeKicker.BBCode.BBTag("list", "<ul>", "</ul>"), 
                    // new CodeKicker.BBCode.BBTag("*", "<li>", "</li>", true, false), 
                    new CodeKicker.BBCode.BBTag("url", "<a href=\"${href}\" target=\"_blank\" class=\"link-ext\">", "</a>", new CodeKicker.BBCode.BBAttribute("href", string.Empty, GetUrlTagHrefAttributeValue), new CodeKicker.BBCode.BBAttribute("href", "href", GetUrlTagHrefAttributeValue)), 
                    new CodeKicker.BBCode.BBTag("youtube", "<iframe width=\"420\" height=\"315\" src=\"//www.youtube.com/embed/", "\" frameborder=\"0\" allowfullscreen></iframe>"),
                    // TAGs descartados
                    new CodeKicker.BBCode.BBTag("color", string.Empty, string.Empty)
                });

         // Prepara mensajes
         xhtml = xhtml.Replace("\r", "[CR]").
                               Replace("http://youtu.be/", "http://www.youtube.com/v/").
                               Replace("http://www.youtube.com/watch?v=", "http://www.youtube.com/v/");

         // Traduce códigos BBCode
         xhtml = parser.ToHtml(xhtml);

         // Emoticons
         xhtml = xhtml.Replace(":-)", "<img src=\"images/emo/eico_01.png\" alt=\"Emoticon\" />").
                       Replace(":)", "<img src=\"images/emo/eico_01.png\" alt=\"Emoticon\" />").
                       Replace(":-D", "<img src=\"images/emo/eico_02.png\" alt=\"Emoticon\" />").
                       Replace(":D", "<img src=\"images/emo/eico_02.png\" alt=\"Emoticon\" />").
                       Replace(";-)", "<img src=\"images/emo/eico_03.png\" alt=\"Emoticon\" />").
                       Replace(":-O", "<img src=\"images/emo/eico_04.png\" alt=\"Emoticon\" />").
                       Replace(":-P", "<img src=\"images/emo/eico_05.png\" alt=\"Emoticon\" />").
                       Replace(":-(", "<img src=\"images/emo/eico_06.png\" alt=\"Emoticon\" />");
         
         return xhtml;
      }

      #endregion

      #region Private Members

      private void InitializeBBParser()
      {
         BBAttribute[] urlParams = new BBAttribute[2];

         // Configura el parser para BBCodes
         IList<BBTag> bbTags = new List<BBTag>();
         bbTags.Add(new BBTag("b", "<strong>", "</strong>"));
         bbTags.Add(new BBTag("i", "<em>", "</em>"));
         bbTags.Add(new BBTag("u", "<u>", "</u>"));
         bbTags.Add(new BBTag("img", "<img src=\"${content}\" class=\"msg-img\" border=\"0\" alt=\"Imagen externa\" />", string.Empty, false, true));
         // bbTags.Add(new BBTag("url", "<a href=\"${href}\" target=\"_blank\" class=\"link-ext\">", "</a>", new BBAttribute("href", string.Empty), new BBAttribute("href", "href")));
         bbTags.Add(new BBTag("video", "<object type=\"application/x-shockwave-flash\" style=\"width:605px;height:405px\" data=\"${content}\" ><param name=\"movie\" value=\"${content}\" /></object>", ""));
         bbTags.Add(new BBTag("cr", "<br/>", string.Empty));

         urlParams[0] = new BBAttribute("href", string.Empty, GetUrlTagHrefAttributeValue);
         urlParams[1] = new BBAttribute("href", "href", GetUrlTagHrefAttributeValue);

         bbTags.Add(new BBTag("url", "<a href=\"${href}\" target=\"_blank\" class=\"link-ext\">", "</a>", urlParams));

         // bbParser = new BBCodeParser(bbTags);

         /*
         CODI VELL
         IList<BBTag> bbTags = new List<BBTag>();
         bbTags.Add(new BBTag("b", "<strong>", "</strong>"));
         bbTags.Add(new BBTag("i", "<em>", "</em>"));
         bbTags.Add(new BBTag("u", "<u>", "</u>"));
         bbTags.Add(new BBTag("img", "<img src=\"${content}\" class=\"msg-img\" border=\"0\" alt=\"Imagen externa\" />", string.Empty, false, true));
         bbTags.Add(new BBTag("url", "<a href=\"${href}\" target=\"_blank\" class=\"link-ext\">", "</a>",
                              new BBAttribute("href", string.Empty, GetUrlTagHrefAttributeValue),
                              new BBAttribute("href", "href", GetUrlTagHrefAttributeValue)));
         bbTags.Add(new BBTag("video", "<object type=\"application/x-shockwave-flash\" style=\"width:605px;height:405px\" data=\"${content}\" ><param name=\"movie\" value=\"${content}\" /></object>", string.Empty));
         bbTags.Add(new BBTag("cr", "<br/>", string.Empty));

         bbParser = new CodeKicker.BBCode.BBCodeParser(bbTags);
         */
      }

      static string GetUrlTagHrefAttributeValue(IAttributeRenderingContext attributeRenderingContext)
      {
         if (!string.IsNullOrEmpty(attributeRenderingContext.AttributeValue))
            return attributeRenderingContext.AttributeValue; //explicit href attribute on url-Tag

         string tagContent = attributeRenderingContext.GetAttributeValueByID(BBTag.ContentPlaceholderName);

         return tagContent;
      }

      #endregion

   }

}