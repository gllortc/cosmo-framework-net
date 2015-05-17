using System;

namespace Cosmo.UI.DOM.Widgets
{

   /// <summary>
   /// Implementa una caja de búsqueda basada en Google CSE (Custom Search Engine).
   /// </summary>
   public class DomGCSEWidget : DomContentComponentBase
   {
      string _cseKey;
      string _resultScript;
      string _text;
      string _desc;
      string _qlabel;
      string _siteUrl;
      System.Web.Caching.Cache _cache;

      #region Constants

      /// <summary>Título del grid.</summary>
      internal const string SECTION_BOX = "gcsesb-box";

      /// <summary>Identificador del bloque XHTML generado.</summary>
      public const string TAG_HTML_ID = "oid";
      /// <summary>Tag: ID de plantilla.</summary>
      public const string TAG_TEMPLATE_ID = "tid";
      /// <summary>Tag: Título a mostrar del menú.</summary>
      public const string TAG_BOX_TITLE = "title";
      /// <summary>Tag: Descripción del menú.</summary>
      public const string TAG_BOX_DESCRIPTION = "desc";
      /// <summary>Tag: Texto de la etiqueta del campo de búsqueda.</summary>
      public const string TAG_BOX_QUERYLABEL = "qlabel";
      /// <summary>Tag: Clave de Google CSE.</summary>
      public const string TAG_GCSE_KEY = "csekey";
      /// <summary>Tag: URL a la que se devolverán los resultados.</summary>
      public const string TAG_GCSE_RESULTSSCRIPT = "results";
      /// <summary>Tag: URL a la que se devolverán los resultados.</summary>
      public const string TAG_GCSE_SITEURL = "siteurl";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomGCSEWidget"/>.
      /// </summary>
      /// <param name="cache">Una instancia de <see cref="System.Web.Caching.Cache"/> que permite cachear el widget si este es cacheable.</param>
      public DomGCSEWidget(System.Web.Caching.Cache cache)
      {
         Clear();

         _cache = cache;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomGCSEWidget"/>.
      /// </summary>
      /// <param name="cache">Una instancia de <see cref="System.Web.Caching.Cache"/> que permite cachear el widget si este es cacheable.</param>
      /// <param name="id">Identificador único del widget dentro del HTML DOM.</param>
      public DomGCSEWidget(System.Web.Caching.Cache cache, string id)
      {
         Clear();

         _cache = cache;
         this.ID = id;
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "google-cse-search-box"; }
      }

      /// <summary>
      /// Clave de Google CSE (identificador del motor de búsqueda).
      /// </summary>
      /// <remarks>
      /// Proporcionado por Google.
      /// </remarks>
      public string GoogleCSEKey
      {
         get { return _cseKey; }
         set { _cseKey = value; }
      }

      /// <summary>
      /// Clave de Google CSE (identificador del motor de búsqueda).
      /// </summary>
      /// <remarks>
      /// Proporcionado por Google.
      /// </remarks>
      public string ResultScriptUrl
      {
         get { return _resultScript; }
         set { _resultScript = value; }
      }

      /// <summary>
      /// URL del sitio que usa CSE.
      /// </summary>
      public string SiteUrl
      {
         get { return _siteUrl; }
         set { _siteUrl = value; }
      }

      /// <summary>
      /// Título de la caja de búsqueda.
      /// </summary>
      /// <remarks>
      /// Campo opcional.
      /// </remarks>
      public string Text
      {
         get { return _text; }
         set { _text = value; }
      }

      /// <summary>
      /// Descripción de la búsqueda.
      /// </summary>
      /// <remarks>
      /// Campo opcional.
      /// </remarks>
      public string Description
      {
         get { return _desc; }
         set { _desc = value; }
      }

      /// <summary>
      /// Texto que se mostrará en la etiqueta correspondiente al campo de texto.
      /// </summary>
      /// <remarks>
      /// Campo opcional.
      /// </remarks>
      public string QueryFieldLabel
      {
         get { return _qlabel; }
         set { _qlabel = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Limpia la tabla y la deja a punto para generar una nueva tabla.
      /// </summary>
      /// <remarks>
      /// Por defecto, esta propiedad adopta el valor <c>cse-search-box</c>.
      /// </remarks>
      public void Clear()
      {
         this.ID = "cse-search-box";
         _cseKey = string.Empty;
         _resultScript = string.Empty;
         _text = string.Empty;
         _desc = string.Empty;
         _qlabel = string.Empty;
         _siteUrl = string.Empty;
      }

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el widget.</returns>
      public override string Render(DomTemplate template, DomPage.ContentContainer container)
      {
         string xhtml;

         try
         {
            // Si el elemento está en caché lo devuelve, sin comprobar si es cacheable o no
            if (_cache[template.GetCacheKey(ELEMENT_ROOT)] != null)
               return (string)_cache[template.GetCacheKey(ELEMENT_ROOT)];

            // Obtiene la plantilla del widget
            DomTemplateComponent widget = template.GetContentComponent(this.ELEMENT_ROOT, container);
            if (widget == null) return string.Empty;

            // Representa la caja
            xhtml = widget.GetFragment(DomGCSEWidget.SECTION_BOX);
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomGCSEWidget.TAG_HTML_ID, this.ID);
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomGCSEWidget.TAG_BOX_TITLE, _text);
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomGCSEWidget.TAG_BOX_DESCRIPTION, _desc);
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomGCSEWidget.TAG_BOX_QUERYLABEL, _qlabel);
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomGCSEWidget.TAG_GCSE_KEY, _cseKey);
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomGCSEWidget.TAG_GCSE_RESULTSSCRIPT, _resultScript);
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomGCSEWidget.TAG_GCSE_SITEURL, _siteUrl);

            // Cachea el elemento
            if (_cache != null && this.CacheEnabled)
            {
               if (_cache[template.GetCacheKey(ELEMENT_ROOT)] == null)
               {
                  _cache.Insert(template.GetCacheKey(ELEMENT_ROOT), xhtml, null, DateTime.Now.AddSeconds(this.CacheExpiration), TimeSpan.Zero);
               }
            }

            // Devuelve el código html
            return xhtml.ToString();
         }
         catch
         {
            throw;
         }
      }

      #endregion

   }

}
