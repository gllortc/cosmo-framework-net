using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Cosmo.Utils.Html
{

   #region Enumerations

   enum HtmlTokens
   {
      /// <summary>Lee texto (sin Tag analizado)</summary>
      Text,
      /// <summary>Lee el Tag</summary>
      Tag,
      /// <summary>Lee un parámetro</summary>
      Attribute,
      /// <summary>Lee el valor de parámetro</summary>
      Value
   }

   #endregion

   /// <summary>
   /// Implementa una clase que representa un documento HTML separado por TAGs y texto. 
   /// Esta clase permite sanitizar fragmentos HTML capturados externamente.
   /// </summary>
   public class HtmlDocument
   {
      string _inputHtml;
      List<string> _disallowedTags;
      List<string> _allowedAttrs;
      List<object> _html = new List<object>();

      /// <summary>
      /// Devuelve una instancia de Html.
      /// </summary>
      public HtmlDocument()
      {
         _disallowedTags = new List<string>();
         _allowedAttrs = new List<string>();
         _inputHtml = "";
      }

      /// <summary>
      /// Devuelve una instancia de Html.
      /// </summary>
      public HtmlDocument(string inputHtml)
      {
         _disallowedTags = new List<string>();
         _allowedAttrs = new List<string>();

         Parse(inputHtml);
      }

      #region Settings

      /// <summary>
      /// Devuelve o establece la cadena HTML original (sin parsear).
      /// </summary>
      public string InputHtml
      {
         get { return _inputHtml; }
         set { _inputHtml = value.Trim(); }
      }

      /// <summary>
      /// Contiene la lista de TAGs prohibidos.
      /// </summary>
      public List<string> DisallowedTags
      {
         get { return _disallowedTags; }
         set { _disallowedTags = value; }
      }

      /// <summary>
      /// Contiene la lista de atributos prohibidos.
      /// </summary>
      public List<string> DisallowedAttributes
      {
         get { return _allowedAttrs; }
         set { _allowedAttrs = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Parsea el contenido Html original.
      /// </summary>
      /// <param name="inputHtml">Una cadena de texto que contiene el Html original.</param>
      public void Parse(string inputHtml)
      {
         _inputHtml = inputHtml;

         // Inicializaciones
         string html = _inputHtml;
         string text = "";
         HtmlTokens token = HtmlTokens.Text;

         _html = new List<object>();

         // Elimina los TAGs no deseados, usando Regular Expresions
         string rexp = @"<[/]?(";
         foreach (string dtag in _disallowedTags) rexp += dtag + "|";
         rexp = rexp + @"[ovwxp]:\w+)[^>]*?>";
         html = Regex.Replace(html, rexp, "", RegexOptions.IgnoreCase);

         // Parsea el código HTML
         for (int i = 0; i < html.Length; i++)
         {
            // Obtiene el carácter actual
            char let = html[i];

            if (token == HtmlTokens.Text && let == '<')
            {
               if (!text.Trim().Equals(""))
                  _html.Add(new HtmlText(text));

               text = "";
               token = HtmlTokens.Tag;

               continue;
            }

            if (token == HtmlTokens.Tag && let == '>')
            {
               if (!text.Trim().Equals(""))
                  _html.Add(new HtmlTag(this, text));

               text = "";
               token = HtmlTokens.Text;

               continue;
            }

            text = text + let;
         }

         // Contempla el caso de tener un fragmento de texto no añadido
         if (!text.Trim().Equals(""))
            _html.Add(new HtmlText(text));
      }

      /// <summary>
      /// Indica si un determinado TAG está permitido.
      /// </summary>
      /// <param name="tag">Nombre del TAG.</param>
      public bool IsAllowedTag(string tag)
      {
         foreach (string atag in _disallowedTags)
         {
            if (tag.Trim().ToLower().Equals(atag.Trim().ToLower())) return false;
         }
         return true;
      }

      /// <summary>
      /// Indica si un determinado parámetro está permitido.
      /// </summary>
      /// <param name="attribute">Nombre del parámetro.</param>
      public bool IsAllowedAttribute(string attribute)
      {
         foreach (string attr in _allowedAttrs)
         {
            if (attr.Trim().ToLower().Equals(attribute.Trim().ToLower())) return false;
         }
         return true;
      }

      /// <summary>
      /// Convierte el Html interpretado en una cadena lista para enviar al cliente.
      /// </summary>
      public string ToHtml()
      {
         string html = "";

         foreach (object fragment in _html)
         {
            if (fragment.GetType() == typeof(HtmlText))
            {
               html = html + ((HtmlText)fragment).ToXhtml();
            }
            else if (fragment.GetType() == typeof(HtmlTag))
            {
               html = html + ((HtmlTag)fragment).ToXhtml();
            }
         }

         return html.Trim();
      }

      #endregion

   }
}
