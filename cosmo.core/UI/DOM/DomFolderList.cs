using System.Collections.Generic;
using System.Web;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa un listado de carpetas (estructura de navegación).
   /// </summary>
   public class DomFolderList : DomContentComponentBase
   {
      private bool _showTitle;
      private string _title;
      private string _description;
      private List<DomFolderListItem> _items;

      #region Constants

      /// <summary>Identificador del título del elemento.</summary>
      internal const string SECTION_TITLE = "folder-list-title";
      /// <summary>Identificador de la cabecera de la lista.</summary>
      internal const string SECTION_HEAD = "folder-list-header";
      /// <summary>Identificador del elemento de la lista.</summary>
      internal const string SECTION_LISTITEM = "folder-list-item";
      /// <summary>Identificador del pié de la lista.</summary>
      internal const string SECTION_FOOT = "folder-list-footer";

      /// <summary>Identificador del bloque XHTML generado.</summary>
      public const string TAG_HTML_ID = "oid";
      /// <summary>Identificador de la plantilla.</summary>
      public const string TAG_TEMPLATE_ID = "tid";
      /// <summary>Número de elementos en la lista.</summary>
      public const string TAG_LIST_ITEMS = "items";
      /// <summary>Título del listado.</summary>
      public const string TAG_LIST_TITLE = "title";
      /// <summary>Descripción del listado.</summary>
      public const string TAG_LIST_DESCRIPTION = "description";
      /// <summary>Clase del primer elemento del listado.</summary>
      public const string TAG_LISTITEM_FIRSTITEMCSS = "class";
      /// <summary>Título del elemento de la lista.</summary>
      public const string TAG_LISTITEM_CAPTION = "caption";
      /// <summary>URL del elemento de la lista.</summary>
      public const string TAG_LISTITEM_HREF = "href";
      /// <summary>Descripción del elemento de la lista.</summary>
      public const string TAG_LISTITEM_DESCRIPTION = "description";
      /// <summary>Número del elemento dentro de la lista.</summary>
      public const string TAG_LISTITEM_ITEM = "item";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderList"/>.
      /// </summary>
      public DomFolderList() : base() 
      {
         // Inicialización
         Clear();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderList"/>.
      /// </summary>
      public DomFolderList(string title) : base()
      {
         // Inicialización
         Clear();

         _title = title;
         _showTitle = true;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderList"/>.
      /// </summary>
      public DomFolderList(string title, string description) : base()
      {
         // Inicialización
         Clear();

         _title = title;
         _description = description;
         _showTitle = true;
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "folder-list"; }
      }

      /// <summary>
      /// Devuelve o establece el título del listado.
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Devuelve o establece la descripción del listado.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Indica si se debe mostrar el título del listado.
      /// </summary>
      public bool ShowTitle
      {
         get { return _showTitle; }
         set { _showTitle = value; }
      }

      /// <summary>
      /// Colección de elementos <see cref="DomFolderListItem"/> del listado.
      /// </summary>
      public List<DomFolderListItem> Items
      {
         get { return _items; }
         set { _items = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Inicializa la lista.
      /// </summary>
      public void Clear()
      {
         this.ID = "flist1";
         _showTitle = false;
         _title = string.Empty;
         _description = string.Empty;
         _items = new List<DomFolderListItem>();
      }

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el componente.</returns>
      public override string Render(DomTemplate template, DomPage.ContentContainer container)
      {
         string xhtml = string.Empty;
         string part = string.Empty;

         // Obtiene la plantilla del componente
         DomTemplateComponent component = template.GetContentComponent(this.ELEMENT_ROOT, container);
         if (component == null) return string.Empty;

         // Zona de título del listado
         if (_showTitle || string.IsNullOrEmpty(this.Title))
         {
            part = component.GetFragment(DomFolderList.SECTION_TITLE);
            part = DomContentComponentBase.ReplaceTag(part, DomFolderList.TAG_LIST_TITLE, HttpUtility.HtmlEncode(this.Title));
            part = DomContentComponentBase.ReplaceTag(part, DomFolderList.TAG_LIST_DESCRIPTION, HttpUtility.HtmlEncode(this.Description));
            xhtml = string.Format("{0}{1}", xhtml, part);
         }

         // Cabecera del listado
         xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomFolderList.SECTION_HEAD));

         // Lista de elementos
         int count = 1;
         foreach (DomFolderListItem item in _items)
         {
            part = component.GetFragment(DomFolderList.SECTION_LISTITEM);
            part = DomContentComponentBase.ReplaceTag(part, DomFolderList.TAG_LISTITEM_CAPTION, HttpUtility.HtmlEncode(item.Caption));
            part = DomContentComponentBase.ReplaceTag(part, DomFolderList.TAG_LISTITEM_HREF, item.Href);
            part = DomContentComponentBase.ReplaceTag(part, DomFolderList.TAG_LISTITEM_DESCRIPTION, item.Description);
            part = DomContentComponentBase.ReplaceTag(part, DomFolderList.TAG_LISTITEM_ITEM, count.ToString());
            part = DomContentComponentBase.ReplaceTag(part, DomFolderList.TAG_LISTITEM_FIRSTITEMCSS, (count == 1 ? " class=\"" + template.FirstItemCssClass + "\"" : string.Empty));
            xhtml = string.Format("{0}{1}", xhtml, part);

            count++;
         }

         // Pié del listado
         xhtml = string.Format("{0}{1}\n", xhtml, component.GetFragment(DomFolderList.SECTION_FOOT));

         // Reemplaza los TAGs comunes
         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomFolderList.TAG_TEMPLATE_ID, template.ID.ToString());
         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomFolderList.TAG_LIST_ITEMS, _items.Count.ToString());
         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomFolderList.TAG_HTML_ID, this.ID);

         return xhtml;
      }

      #endregion

   }
}
