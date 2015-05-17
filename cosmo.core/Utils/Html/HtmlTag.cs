using System.Collections.Generic;

namespace Cosmo.Utils.Html
{

   /// <summary>
   /// Representa un TAG Html, es decir, cualquier elemento encapsulado entre &lt; y &gt;.
   /// </summary>
   class HtmlTag
   {
      bool _closingTag;
      string _name;
      List<HtmlAttribute> _attributes;
      HtmlDocument _html = null;

      /// <summary>
      /// Devuelve una instancia de HtmlTag.
      /// </summary>
      public HtmlTag(HtmlDocument html, string tagContents)
      {
         _html = html;
         _closingTag = false;
         _name = "";
         _attributes = new List<HtmlAttribute>();

         Parse(tagContents);
      }

      /// <summary>
      /// Indica si se trata de un TAG de cierre.
      /// </summary>
      public bool IsClosigTag
      {
         get { return _closingTag; }
         set { _closingTag = value; }
      }

      #region Settings

      /// <summary>
      /// Nombre del TAG.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value.Trim().ToLower(); }
      }

      /// <summary>
      /// Contiene la lista de atributos del TAG.
      /// </summary>
      public List<HtmlAttribute> Attributes
      {
         get { return _attributes; }
         set { _attributes = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Permite obtener el valor de un parámetro capturado.
      /// </summary>
      /// <param name="key">Clave del parámetro que se desea obtener.</param>
      public string GetParameter(string key)
      {
         foreach (HtmlAttribute att in _attributes)
         {
            if (att.Name.Trim().ToLower().Equals(key.Trim().ToLower()))
               return att.Value;
         }
         return "";
      }

      /// <summary>
      /// Convierte el TAG en una cadena de texto lista para enviar a un cliente.
      /// </summary>
      public string ToXhtml()
      {
         // Evita agregar TAGs prohibidos
         if (!_html.IsAllowedTag(_name)) return "";

         // Reemplaza TAGs obsoletos
         if (_name.Equals("b")) _name = "strong";
         if (_name.Equals("i")) _name = "em";

         // Abre el TAG
         string html = "<" + (_closingTag ? "/" : "") + _name;

         // Comprueba si es una imagen de 1x1
         if (_name.Equals("img") && GetParameter("width").Equals("1") && GetParameter("height").Equals("1"))
            return "";

         // Agrega los atributos
         foreach (HtmlAttribute att in _attributes)
         {
            if (_html.IsAllowedAttribute(att.Name))
               html = html + " " + att.Name + "=\"" + System.Web.HttpUtility.HtmlEncode(att.Value) + "\"";
         }

         // Asegura que los TAGS sin cierre queden cerrados correctamente
         switch (_name)
         {
            case "area":
            case "base":
            case "basefont":
            case "br":
            case "col":
            case "frame":
            case "hr":
            case "img":
            case "input": 
            case "link":
            case "meta":
            case "param": html = html + "/"; break;
         }

         // Cierra el TAG
         html = html + ">";

         return html;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Parsea el contenido del TAG.
      /// </summary>
      private void Parse(string tag)
      {
         string text = "";
         HtmlTokens token = HtmlTokens.Tag;
         // HtmlAttribute currentAttr = null;

         for (int i = 0; i < tag.Length; i++)
         {
            // Obtiene el carácter actual
            char let = tag[i];

            if (token == HtmlTokens.Tag && let == '/')
            {
               _closingTag = true;
               text = "";

               continue;
            }

            if (token == HtmlTokens.Tag && let == ' ')
            {
               _name = text.Trim().ToLower();
               text = "";
               token = HtmlTokens.Attribute;

               continue;
            }

            if (token == HtmlTokens.Attribute && let == '=')
            {
               string value = GetValue(tag, ref i);

               _attributes.Add(new HtmlAttribute(text.Trim().ToLower(), value));
               text = "";
               token = HtmlTokens.Attribute;

               continue;
            }

            /*if (token == HtmlTokens.Value && (let == '"' || let == '\''))
            {
               currentAttr.Value = text.Trim();
               _attributes.Add(currentAttr);
               currentAttr = null;

               text = "";
               token = HtmlTokens.Attribute;

               continue;
            }*/

            text = text + let;
         }

         // Controla si existe algún TAG no agregado
         if (token == HtmlTokens.Tag && !text.Trim().Equals("") && _name.Equals(""))
            _name = text;
      }

      /// <summary>
      /// Obtiene el siguiente valor. Este debe estar entre comillar simples o dobles.
      /// </summary>
      private string GetValue(string input, ref int startIndex)
      {
         bool inside = false;
         int i = 0;
         string value = "";

         for (i = startIndex; i < input.Length; i++)
         {
            char let = input[i];

            if (!inside && (let == '"' || let == '\''))
            {
               inside = true;
               continue;
            }

            if (inside && (let == '"' || let == '\''))
            {
               startIndex = i;
               inside = false;

               return value;
            }

            if (inside) value = value + let;
         }

         return "";
      }

      private int NextChar(string text, int startIndex, bool bracesAsSpace)
      {
         int i = 0;

         for (i = startIndex; i < text.Length; i++)
         {
            char let = text[i];
            if (let != ' ' && !(bracesAsSpace && (let == '"' || let == '\''))) return i - 1;
         }
         return i;
      }

      #endregion

   }
}
