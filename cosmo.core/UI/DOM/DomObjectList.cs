using System.Collections.Generic;
using System.Web;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa una lista de objetos.
   /// </summary>
   public class DomObjectList : DomContentComponentBase
   {
      private bool _showTitle;
      private string _title;
      private string _description;
      private List<DomObjectListItem> _items;

      #region Constants

      /// <summary>Sección de cabecera del listado.</summary>
      internal const string SECTION_TITLE = "object-list-title";
      /// <summary>Sección de cabecera del listado.</summary>
      internal const string SECTION_HEAD = "object-list-header";
      /// <summary>Sección de elemento del listado.</summary>
      internal const string SECTION_ITEM = "object-list-item";
      /// <summary>Sección de pié del listado.</summary>
      internal const string SECTION_FOOT = "object-list-footer";

      /// <summary>Identificador del bloque XHTML generado.</summary>
      public const string TAG_HTML_ID = "objid";
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
      /// <summary>URL (absoluta o relativa) de la imagen miniatura.</summary>
      public const string TAG_LISTITEM_THUMB = "thumb";
      /// <summary>Descripción del elemento de la lista.</summary>
      public const string TAG_LISTITEM_DESCRIPTION = "description";
      /// <summary>Sección de autor/fecha o info adicional del elemento de la lista.</summary>
      public const string TAG_LISTITEM_AUTHOR = "author";
      /// <summary>Número del elemento dentro de la lista.</summary>
      public const string TAG_LISTITEM_ITEM = "item";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderList"/>.
      /// </summary>
      public DomObjectList() : base() 
      {
         // Inicialización
         Clear();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderList"/>.
      /// </summary>
      /// <param name="id">Identificador del bloque XHTML generado.</param>
      public DomObjectList(string id) : base()
      {
         // Inicialización
         Clear();

         this.ID = id;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderList"/>.
      /// </summary>
      /// <param name="id">Identificador del bloque XHTML generado.</param>
      /// <param name="title">Título del listado.</param>
      public DomObjectList(string id, string title) : base()
      {
         // Inicialización
         Clear();

         this.ID = id;
         _title = title;
         _showTitle = true;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderList"/>.
      /// </summary>
      /// <param name="id">Identificador del bloque XHTML generado.</param>
      /// <param name="title">Título del listado.</param>
      /// <param name="description">Descripción del contenido del listado.</param>
      public DomObjectList(string id, string title, string description) : base()
      {
         // Inicialización
         Clear();

         this.ID = id;
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
         get { return "object-list"; }
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
      /// Devuelve o establece la descripción del contenido del listado.
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
      /// Colección de elementos <see cref="DomObjectListItem"/> del listado.
      /// </summary>
      public List<DomObjectListItem> Items
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
         this.ID = "olist1";
         _showTitle = false;
         _title = string.Empty;
         _description = string.Empty;
         _items = new List<DomObjectListItem>();
      }

      /*
      /// <summary>
      /// Agrega un conjunto de objetos a la lista.
      /// </summary>
      /// <param name="objectList">Una instancia de <see cref="System.Collections.Generic.List&lt;T&gt;"/> que contiene los objetos a agregar.</param>
      /// <param name="urlBase">URL dónde residen las carpetas de cada objeto.</param>
      public void AddObjects(List<MetaObjectBase> objectList, string urlBase)
      {
         DomObjectListItem item;

         foreach (MetaObjectBase obj in objectList)
         {
            item = new DomObjectListItem();
            item.Caption = obj.Title;
            item.Description = obj.Description;
            item.Author = obj.Owner;
            item.Thumbnail = Cosmo.Net.Url.Combine(urlBase, obj.ID.ToString());
            item.Href = obj.GetUrl();

            this.Items.Add(item);
         }
      }
      */

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el elemento.</returns>
      /// <remarks>
      /// Este <em>render</em> acepta los siguientes condicionales:
      /// - THUMB
      /// - AUTHOR
      /// </remarks>
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
            part = component.GetFragment(DomObjectList.SECTION_TITLE);
            part = DomContentComponentBase.ReplaceTag(part, DomObjectList.TAG_LIST_TITLE, HttpUtility.HtmlEncode(this.Title));
            part = DomContentComponentBase.ReplaceTag(part, DomObjectList.TAG_LIST_DESCRIPTION, HttpUtility.HtmlEncode(this.Description));
            
            xhtml = string.Format("{0}{1}", xhtml, part);
         }

         // Cabecera del listado
         xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomObjectList.SECTION_HEAD));

         // Lista de elementos
         int count = 1;
         foreach (DomObjectListItem item in _items)
         {
            part = component.GetFragment(DomObjectList.SECTION_ITEM);
            part = DomContentComponentBase.ReplaceTag(part, DomObjectList.TAG_LISTITEM_CAPTION, HttpUtility.HtmlEncode(item.Caption));
            part = DomContentComponentBase.ReplaceTag(part, DomObjectList.TAG_LISTITEM_HREF, item.Href);
            part = DomContentComponentBase.ReplaceTag(part, DomObjectList.TAG_LISTITEM_DESCRIPTION, item.Description);
            part = DomContentComponentBase.ReplaceTag(part, DomObjectList.TAG_LISTITEM_ITEM, count.ToString());
            part = DomContentComponentBase.ReplaceTag(part, DomObjectList.TAG_LISTITEM_FIRSTITEMCSS, (count == 1 ? " class=\"" + template.FirstItemCssClass + "\"" : string.Empty));
            part = DomContentComponentBase.ReplaceTag(part, DomObjectList.TAG_LISTITEM_THUMB, item.Thumbnail);
            part = DomContentComponentBase.ReplaceTag(part, DomObjectList.TAG_LISTITEM_AUTHOR, item.Author);

            xhtml = string.Format("{0}{1}", xhtml, part);

            count++;
         }

         // Pié del listado
         xhtml = string.Format("{0}{1}\n", xhtml, component.GetFragment(DomObjectList.SECTION_FOOT));

         // Reemplaza los TAGs comunes
         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomObjectList.TAG_HTML_ID, this.ID);
         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomObjectList.TAG_TEMPLATE_ID, template.ID.ToString());
         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomObjectList.TAG_LIST_ITEMS, _items.Count.ToString());

         return xhtml;
      }

      #endregion

   }
}
