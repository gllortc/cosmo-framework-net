using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un componente que permite mostrar contenido textual (tipografia) en una página.
   /// </summary>
   public class HtmlContentControl : Control
   {
      /// <summary>Blank space character.</summary>
      public const string HTML_SPACE = "&nbsp;";
      /// <summary>New line tag.</summary>
      public const string HTML_NEW_LINE = "<br/>";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="HtmlContentControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public HtmlContentControl(View parentView) 
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="HtmlContentControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="html">Una cadena que contiene el código HTML que será renderizado.</param>
      public HtmlContentControl(View parentView, string html)
         : base(parentView)
      {
         Initialize();

         this.Html.AppendLine(html);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el contenido.
      /// </summary>
      public StringBuilder Html { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Adds a new paragraph to the HTML content.
      /// </summary>
      /// <param name="level">Header level (1 to 6).</param>
      /// <param name="text">Texto contenido en el párrafo.</param>
      public HtmlContentControl AppendHeader(int level, string text)
      {
         this.Html.AppendLine("<h" + level + ">" + HttpUtility.HtmlDecode(text) + "</h" + level + ">");

         return this;
      }

      /// <summary>
      /// Adds a new paragraph to the HTML content.
      /// </summary>
      /// <param name="text">Texto contenido en el párrafo.</param>
      public HtmlContentControl AppendParagraph(string text)
      {
         this.Html.AppendLine("<p>" + HttpUtility.HtmlDecode(text) + "</p>");
         
         return this;
      }

      /// <summary>
      /// Agrega una línea separadora entre párrafos o elementos.
      /// </summary>
      public HtmlContentControl AppendLineSeparator()
      {
         this.Html.AppendLine("<hr />");

         return this;
      }

      /// <summary>
      /// Agrega una tabla de datos.
      /// </summary>
      public HtmlContentControl AppendDataTable(List<KeyValue> data)
      {
         if (data == null || data.Count <= 0)
         {
            return this;
         }

         this.Html.AppendLine("<dl class=\"dl-horizontal\">");
         foreach (KeyValue value in data)
         {
            this.Html.AppendLine("  <dt>" + value.Label + "</dt>");
            this.Html.AppendLine("  <dd>" + value.Value + "</dd>");
         }
         this.Html.AppendLine("</dl>");

         return this;
      }

      /// <summary>
      /// Agrega una lista (sin órden).
      /// </summary>
      public HtmlContentControl AppendUnorderedList(List<String> data)
      {
         if (data == null || data.Count <= 0)
         {
            return this;
         }

         this.Html.AppendLine("<ul>");
         foreach (String value in data)
         {
            this.Html.AppendLine("  <li>" + value + "</li>");
         }
         this.Html.AppendLine("</ul>");

         return this;
      }

      /// <summary>
      /// Adds an image as a paragraph to the HTML content.
      /// </summary>
      /// <param name="url">Image URL</param>
      /// <param name="text">Alternative text</param>
      public HtmlContentControl AppendImage(string url, string text)
      {
         this.Html.AppendLine(string.Format(@"<p><img src=""{0}"" alt=""{1}"" /></p>", url, text));

         return this;
      }

      /// <summary>
      /// Agrega el código HTML correspondiente a un enlace.
      /// </summary>
      /// <param name="url">URL</param>
      /// <param name="text">Tenxto del enlace</param>
      /// <param name="openInNewWindow">Indica si se debe abrir el enlace en una nueva ventana.</param>
      /// <returns>Una cadena que contiene el código HTML correspondiente a un enlace.</returns>
      public HtmlContentControl AppendLink(string url, string text, bool openInNewWindow)
      {
         this.Html.AppendLine(HtmlContentControl.Link(url, text, openInNewWindow));

         return this;
      }

      /// <summary>
      /// Adds a data list structure to the HTML content.
      /// </summary>
      /// <param name="values">List of values. <c>KeyValue.Label</c> is used as a title, <c>KeyValue.Label</c> is used as a text.</param>
      /// <returns>The current instance updated with the new data list.</returns>
      public HtmlContentControl AppendDataList(List<KeyValue> values)
      {
         this.Html.AppendLine("<dl class=\"dl-horizontal\">");
         foreach (KeyValue value in values)
         {
            if (!string.IsNullOrWhiteSpace(value.Label))
            {
               this.Html.AppendLine("<dt>" + value.Label + "</dt>");
            }
            this.Html.AppendLine("<dd>" + value.Value + "</dd>");
         }
         this.Html.AppendLine("</dl>");

         return this;
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve el código HTML correspondiente a un enlace.
      /// </summary>
      /// <param name="url">URL</param>
      /// <param name="text">Tenxto del enlace</param>
      /// <param name="openInNewWindow">Indica si se debe abrir el enlace en una nueva ventana.</param>
      /// <returns>Una cadena que contiene el código HTML correspondiente a un enlace.</returns>
      public static string Link(string url, string text, bool openInNewWindow)
      {
         return "<a href=\"" + HttpUtility.HtmlDecode(url) + "\" " + (openInNewWindow ? "target=\"_blank\"" : string.Empty) + ">" + text + "</a>";
      }

      /// <summary>
      /// Mark text as a bold text.
      /// </summary>
      /// <param name="number">Numeric value to format as a bold text</param>
      /// <returns>A string containing formatted number.</returns>
      public static string BoldText(int number)
      {
         return HtmlContentControl.BoldText(number.ToString());
      }

      /// <summary>
      /// Mark text as a bold text.
      /// </summary>
      /// <param name="text">Text to format as a bold text</param>
      /// <returns>A string containing formatted text.</returns>
      public static string BoldText(string text)
      {
         return "<strong>" + text + "</strong>";
      }

      /// <summary>
      /// Mark numeric value as a emphatized text.
      /// </summary>
      /// <param name="number">Numeric value to format as a emphatized text</param>
      /// <returns>A string containing formatted numeric value.</returns>
      public static string EmphatizedText(int number)
      {
         return HtmlContentControl.EmphatizedText(number.ToString());
      }

      /// <summary>
      /// Mark text as a emphatized text.
      /// </summary>
      /// <param name="text">Text to format as a emphatized text</param>
      /// <returns>A string containing formatted text.</returns>
      public static string EmphatizedText(string text)
      {
         return "<em>" + text + "</em>";
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Html = new StringBuilder();
      }

      #endregion

   }
}
