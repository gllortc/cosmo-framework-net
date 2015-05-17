using System;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa una cabecera de 
   /// </summary>
   public class DomHeader : DomContentComponentBase
   {
      bool _showUtils = false;
      string _title = string.Empty;
      string _description = string.Empty;
      string _author = string.Empty;

      #region Constants

      /// <summary>Título de la página.</summary>
      public const string SECTION_TITLE = "page-title";
      /// <summary>Utilidades de la página.</summary>
      public const string SECTION_UTILS = "page-utilities";

      /// <summary>Identificador del bloque XHTML generado.</summary>
      public const string TAG_HTML_ID = "oid";
      /// <summary>Tag: ID de plantilla.</summary>
      public const string TAG_TEMPLATE_ID = "tid";
      /// <summary>Tag: Título de la página/documento.</summary>
      public const string TAG_HEADER_TITLE = "title";
      /// <summary>Tag: Descripción de la página/documento.</summary>
      public const string TAG_HEADER_DESCRIPTION = "description";
      /// <summary>Tag: Autor del contenido.</summary>
      public const string TAG_HEADER_AUTHOR = "author";
      /// <summary>Tag: Utilidades del documento.</summary>
      public const string TAG_HEADER_UTILITIES = "utilities";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomHeader"/>.
      /// </summary>
      public DomHeader() : base() 
      {
         Clear();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomHeader"/>.
      /// </summary>
      public DomHeader(string title, string description) : base() 
      {
         Clear();

         _title = title;
         _description = description;
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "page-header"; }
      }

      /// <summary>
      /// Devuelve o establece el título del documento.
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Devuelve o establece el texto descriptivo del documento.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Devuelve o establece el autor del documento.
      /// </summary>
      public string Author
      {
         get { return _author; }
         set { _author = value; }
      }

      /// <summary>
      /// Indica si se deben mostrar las utilidades del documento.
      /// </summary>
      public bool ShowUtilities
      {
         get { return _showUtils; }
         set { _showUtils = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Reinicializa el componente y lo deja listo para crear uno nuevo.
      /// </summary>
      public void Clear()
      {
         this.ID = "head1";
         _showUtils = false;
         _title = string.Empty;
         _description = string.Empty;
         _author = string.Empty;
      }

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el elemento.</returns>
      public override string Render(DomTemplate template, DomPage.ContentContainer container)
      {
         string xhtml = string.Empty;
         string part = string.Empty;
         string utils = string.Empty;

         try
         {
            // Obtiene la plantilla del componente
            DomTemplateComponent component = template.GetContentComponent(this.ELEMENT_ROOT, container);
            if (component == null) return string.Empty;

            // Utilidades
            if (_showUtils)
            {
               utils = component.GetFragment(DomHeader.SECTION_UTILS);
               utils = DomContentComponentBase.ReplaceTag(utils, DomHeader.TAG_HEADER_AUTHOR, _author);
            }

            // Cabecera del menú
            part = component.GetFragment(DomHeader.SECTION_TITLE);
            part = DomContentComponentBase.ReplaceTag(part, DomHeader.TAG_HEADER_TITLE, _title);
            part = DomContentComponentBase.ReplaceTag(part, DomHeader.TAG_HEADER_DESCRIPTION, _description);
            part = DomContentComponentBase.ReplaceTag(part, DomHeader.TAG_HEADER_UTILITIES, (_showUtils ? utils : string.Empty));
            xhtml = string.Format("{0}{1}", xhtml, part);

            // Reemplaza los TAGs comunes en todas las secciones del elemento.
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomHeader.TAG_TEMPLATE_ID, template.ID.ToString());
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomHeader.TAG_HTML_ID, this.ID);

            return xhtml;
         }
         catch
         {
            throw;
         }
      }

      #endregion

   }
}
