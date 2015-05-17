using System.Text;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Clase base para los componentes DOM.
   /// </summary>
   public abstract class DomContentComponentBase
   {
      const string TOKEN_IF = "#IF";
      const string TOKEN_ENDIF = "#ENDIF";

      bool _cacheEnabled;
      int _cacheExpiration;
      string _id;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomContentComponentBase"/>.
      /// </summary>
      public DomContentComponentBase() 
      {
         _id = string.Empty;
         _cacheEnabled = false;
         _cacheExpiration = 0;
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public abstract string ELEMENT_ROOT { get; }

      /// <summary>
      /// Devuelve o establece el identificador del bloque XHTML generado.
      /// </summary>
      public string ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Indica si el componente debe usar la cache.
      /// </summary>
      public bool CacheEnabled
      {
         get { return _cacheEnabled; }
         set { _cacheEnabled = value; }
      }

      /// <summary>
      /// Devuelve o establece el tiempo (en segundos) de validez de la cache del componente.
      /// </summary>
      public int CacheExpiration
      {
         get { return _cacheExpiration; }
         set { _cacheExpiration = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el elemento.</returns>
      abstract public string Render(DomTemplate template, DomPage.ContentContainer container);

      /// <summary>
      /// Reemplaza un TAG que puede ser condicional.
      /// </summary>
      /// <param name="section">Código XHTML de la sección.</param>
      /// <param name="tag">Tag a reemplazar.</param>
      /// <param name="value">Valor de reemplazo.</param>
      protected string ReplaceConditional(string section, string tag, string value)
      {
         int startTagPos = -1;
         int endTagPos = -1;
         string startTag = "[#IF " + tag.Trim().ToUpper() + "]";
         string endTag = "[#ENDIF " + tag.Trim().ToUpper() + "]";
         StringBuilder sb = new StringBuilder(section);

         // Reemplaza TAG por el valor
         sb.Replace(DomContentComponentBase.GetTag(tag), value);

         // Obtiene la posición de los TAGs condicionales
         startTagPos = sb.ToString().IndexOf(startTag, 0);
         endTagPos = sb.ToString().IndexOf(endTag, 0);

         // Elimina la sección condicional si el valor 
         if (value.Equals(string.Empty))
         {
            if (startTagPos >= 0)
            {
               if (endTagPos >= 0)
               {
                  sb.Remove(startTagPos, (endTagPos + endTag.Length) - startTagPos);
               }
               else
               {
                  throw new DomRenderException("Error in conditional tag " + tag.ToUpper() + ": no #ENDIF detected.");
               }
            }
         }
         else
         {
            sb.Replace(startTag, string.Empty);
            sb.Replace(endTag, string.Empty);
         }

         return sb.ToString();
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Convierte una cadena de texto en un TAG reemplazable en las secciones de la plantilla de presentación.
      /// </summary>
      public static string GetTag(string key)
      {
         return "[@" + key.Trim().ToUpper() + "]";
      }

      /// <summary>
      /// Reemplaza un TAG por su valor. Soporta TAGs condicionales.
      /// </summary>
      /// <param name="text">Fragmento de plantilla.</param>
      /// <param name="tag">Tag</param>
      /// <param name="value">Valor que debe adoptar el Tag.</param>
      /// <returns>Devuelve el fragmento de plantilla con el TAG reemplazado.</returns>
      public static string ReplaceTag(string text, string tag, string value)
      {
         string ifStart = "[" + TOKEN_IF + " " + tag.Trim().ToUpper() + "]";
         string ifEnd = string.Empty;

         if (text.Contains(ifStart))
         {
            ifEnd = "[" + TOKEN_ENDIF + " " + tag.Trim().ToUpper() + "]";
            if (text.Contains(ifEnd))
            {
               if (!value.Equals(string.Empty))
               {
                  // Condicional con valor: elimina TAGs de condición y reemplaza TAGs interiores.
                  return text.Replace(DomContentComponentBase.GetTag(tag), value).Replace(ifStart, string.Empty).Replace(ifEnd, string.Empty);
               }
               else
               {
                  // Condicional sin valor: elimina TAGs de condición y todo su contenido.
                  int start = text.IndexOf(ifStart);
                  int end = text.IndexOf(ifEnd);
                  return text.Remove(start, (end + ifEnd.Length) - start).Trim();
               }
            }
         }

         return text.Replace(DomContentComponentBase.GetTag(tag), value);
      }

      #endregion

   }

}
