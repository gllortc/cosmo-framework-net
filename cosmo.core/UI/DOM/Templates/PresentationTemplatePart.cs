namespace Cosmo.UI.DOM.Templates
{

   /// <summary>
   /// Implementa un fragmento de una plantilla de presentación
   /// </summary>
   public class PresentationTemplatePart
   {
      private DOMTemplateParts _id;
      private string _html;
      private bool _updated;

      /// <summary>Identificador de la plantilla de presentación.</summary>
      public const string TAG_TEMPLATE_ID = "<%TEMPLATE_ID%>";
      /// <summary>Título del objeto.</summary>
      public const string TAG_OBJECT_TITLE = "<%OBJ_TITLE%>";
      /// <summary>Nombre del objeto.</summary>
      public const string TAG_OBJECT_NAME = "<%OBJ_NAME%>";
      /// <summary>Descripción del objeto.</summary>
      public const string TAG_OBJECT_DESC = "<%OBJ_DESCRIPTION%>";
      /// <summary>Número de elementos en el objeto.</summary>
      public const string TAG_OBJECT_ITEMS = "<%OBJ_ITEMS%>";
      /// <summary>Título del menú.</summary>
      public const string TAG_MENU_CAPTION = "<%MENU_CAPTION%>";
      /// <summary>Elemento del menú.</summary>
      public const string TAG_MENU_ITEM = "<%MENU_ITEM%>";
      /// <summary>Número de elementos del menú.</summary>
      public const string TAG_MENU_ITEMS = "<%MENU_ITEMCOUNT%>";
      /// <summary>Contenido de la celda.</summary>
      public const string TAG_TABLE_CELL_VALUE = "<%DETAIL_VALUE%>";
      /// <summary>Título de la tabla.</summary>
      public const string TAG_TABLE_CAPTION = "<%TABLE_CAPTION%>";
      /// <summary>Sumario (descripción) de la tabla.</summary>
      public const string TAG_TABLE_SUMMARY = "<%TABLE_SUMMARY%>";
      /// <summary>Etiqueta del campo de formulario.</summary>
      public const string TAG_FORM_LABEL = "<%FORM_LABEL%>";
      /// <summary>Control del campo de formulario.</summary>
      public const string TAG_FORM_CONTROL = "<%FORM_CONTROL%>";
      /// <summary>Nombre (y ID) del campo de formulario.</summary>
      public const string TAG_FORM_CONTROLNAME = "<%FORM_CONTROLNAME%>";
      /// <summary>Descripción (ayuda) del campo de formulario.</summary>
      public const string TAG_FORM_CONTROLDESC = "<%FORM_CONTROLDESC%>";
      /// <summary>Zona de botones de comando del formulario.</summary>
      public const string TAG_FORM_TOOLBAR = "<%FORM_SEND%>";
      /// <summary>Etiqueta del campo de una página de propiedades.</summary>
      public const string TAG_RECORD_LABEL = "<%RECORD_LABEL%>";
      /// <summary>Valor del campo de una página de propiedades.</summary>
      public const string TAG_RECORD_VALUE = "<%RECORD_VALUE%>";
      /// <summary>Elemento de una página de propiedades.</summary>
      public const string TAG_RECORD_ITEM = "<%RECORD_ITEM%>";
      /// <summary>Título de una lista.</summary>
      public const string TAG_LIST_CAPTION = "<%LIST_CAPTION%>";
      /// <summary>Número de elementos de una lista.</summary>
      public const string TAG_LISTITEM_ITEMCOUNT = "<%LIST_ITEMCOUNT%>";
      /// <summary>Icono del elemento de la lista.</summary>
      public const string TAG_LISTITEM_ICON = "<%LISTITEM_ICON%>";
      /// <summary>Elemento de la lista.</summary>
      public const string TAG_LISTITEM_ITEM = "<%LISTITEM_ITEM%>";
      /// <summary>Espacio de identación del elemento de la lista.</summary>
      public const string TAG_LISTITEM_IDENTATION = "<%LISTITEM_IDENTATION%>";
      /// <summary>Título del mensaje.</summary>
      public const string TAG_MSGBOX_TITLE = "<%MSGBOX_TITLE%>";
      /// <summary>Contenido del mensaje.</summary>
      public const string TAG_MSGBOX_DESCRIPTION = "<%MSGBOX_DESCRIPTION%>";
      /// <summary>Barra de botones del mensaje.</summary>
      public const string TAG_MSGBOX_ACTION = "<%MSGBOX_ACTION%>";
      /// <summary>Icono del mensaje.</summary>
      public const string TAG_MSGBOX_ICON = "<%MSGBOX_ICON%>";
      /// <summary>Título de la página.</summary>
      public const string TAG_PAGE_TITLE = "<%PAGE_TITLE%>";
      /// <summary>Descripción breve de la página.</summary>
      public const string TAG_PAGE_DESCRIPTION = "<%PAGE_DESCRIPTION%>";
      /// <summary>Palabras clave de la página.</summary>
      public const string TAG_PAGE_KEYWORDS = "<%PAGE_KEYWORDS%>";
      /// <summary>Juego de carácteres de la página.</summary>
      public const string TAG_PAGE_CHARSET = "<%PAGE_CHARSET%>";
      /// <summary>Software con el que se generó la página.</summary>
      public const string TAG_PAGE_GENERATOR = "<%PAGE_GENERATOR%>";
      /// <summary>Código del idioma de la página.</summary>
      public const string TAG_PAGE_LANGUAGE = "<%PAGE_LANGUAGE%>";
      /// <summary>Orígen del contenido de la página.</summary>
      public const string TAG_PAGE_SOURCE = "<%PAGE_SOURCE%>";
      /// <summary>Título de la página.</summary>
      public const string TAG_PAGE_HEAD_TITLE = "<%PAGE_HEAD_TITLE%>";
      /// <summary>Descripción breve de la página.</summary>
      public const string TAG_PAGE_HEAD_DESCRIPTION = "<%PAGE_HEAD_DESCRIPTION%>";
      /// <summary>Parte derecha de la barra de navegación de la página.</summary>
      public const string TAG_PAGE_NAVBAR_RIGHT = "<%PAGE_NAVBAR_RIGHT%>";
      /// <summary>Parte izquierda de la barra de navegación de la página.</summary>
      public const string TAG_PAGE_NAVBAR_LEFT = "<%PAGE_NAVBAR_LEFT%>";

      #region Enumerations

      /// <summary>
      /// Enumera los tipos de partes de una plantilla DOM.
      /// </summary>
      public enum DOMTemplateParts : int
      {
         /// <summary>Cabecera de la página.</summary>
         PagesHeader = 1001,
         /// <summary>Pié de la página.</summary>
         PagesFooter = 1002,
         /// <summary>Título de la página.</summary>
         PagesTitle = 1004,
         /// <summary>Zona de código XHTML libre (no controlado).</summary>
         PagesUnmanagedHtml = 1005,
         /// <summary>Barra de navegación de la página.</summary>
         PagesNavbar = 1006,

         /// <summary>Cabecera de menú.</summary>
         MenuHeader = 2000,
         /// <summary>Cabecera de grupo de opciones de menú.</summary>
         MenuGroupHeader = 2002,
         /// <summary>Elemento de menú.</summary>
         MenuElement = 2003,
         /// <summary>Pié de grupo de opciones de menú.</summary>
         MenuGroupFooter = 2004,
         /// <summary>Pié de menú.</summary>
         MenuFooter = 2999,

         /// <summary>Cabecera de tabla.</summary>
         TableHeader = 3002,
         /// <summary>Cabecera de la fila de títulos de la tabla.</summary>
         TableTitleHeader = 3003,
         /// <summary>Contenido de las celdas de títulos de la tabla.</summary>
         TableTitleCell = 3004,
         /// <summary>Pié de la fila de títulos de la tabla.</summary>
         TableTitleFooter = 3005,
         /// <summary>Cabecera de las filas de datos de la tabla.</summary>
         TableDataHeader = 3006,
         /// <summary>Contenido de las celdas de datos de la tabla.</summary>
         TableDataCell = 3007,
         /// <summary>Pié de las celdas de datos de la tabla.</summary>
         TableDataFooter = 3008,
         /// <summary>Cabecera de las celdas de resultados de la tabla.</summary>
         TableReportHeader = 3009,
         /// <summary>Contenido de las celdas de resultados de la tabla.</summary>
         TableReportCell = 3010,
         /// <summary>Pié de las celdas de resultados de la tabla.</summary>
         TableReportFooter = 3011,
         /// <summary>Pié de la tabla.</summary>
         TableFooter = 3012,

         /// <summary>Cabecera del formulario.</summary>
         FormHeader = 4001,
         /// <summary>Cabecera de campo del formulario.</summary>
         FormFieldHeader = 4002,
         /// <summary>Contenido del campo del formulario (etiqueta + control + ayuda).</summary>
         FormFieldControl = 4003,
         /// <summary>Pié de campo del formulario.</summary>
         FormFieldFooter = 4004,
         /// <summary>Cabecera de campo de texto largo (textarea) del formulario.</summary>
         FormLongtextHeader = 4007,
         /// <summary>Contenido del campo de texto largo (textarea) del formulario.</summary>
         FormLongtextControl = 4008,
         /// <summary>Pié de campo de texto largo (textarea) del formulario.</summary>
         FormLongtextFooter = 4009,
         /// <summary>Zona de botones del formulario.</summary>
         FormButtons = 4005,
         /// <summary>Pié del formulario.</summary>
         FormFooter = 4006,

         /// <summary>Cabecera de la página de propiedades.</summary>
         PropertyPageHeader = 5000,
         /// <summary>Cabecera de campo de la página de propiedades.</summary>
         PropertyPageFieldHeader = 5002,
         /// <summary>Contenido del campo de la página de propiedades.</summary>
         PropertyPageFieldValue = 5003,
         /// <summary>Pié de campo de la página de propiedades.</summary>
         PropertyPageFieldFooter = 5004,
         /// <summary>Pié de la página de propiedades.</summary>
         PropertyPageFooter = 5999,

         /// <summary>Cabecera de la lista.</summary>
         ListHeader = 6001,
         /// <summary>Elemento de la lista.</summary>
         ListElement = 6002,
         /// <summary>Pié de la lista.</summary>
         ListFooter = 6003,

         /// <summary>Cuerpo del mensaje.</summary>
         MessageBody = 7001
      }

      #endregion

      /// <summary>
      /// Devuelve una instancia de PresentationTemplatePart
      /// </summary>
      /// <param name="partid">Identificador de la parte</param>
      /// <param name="html">Código XHTML que contiene</param>
      public PresentationTemplatePart(DOMTemplateParts partid, string html)
      {
         _id = partid;
         _html = html;
         _updated = false;
      }

      /// <summary>
      /// Devuelve una instancia de PresentationTemplatePart
      /// </summary>
      public PresentationTemplatePart()
      {
         _html = "";
         _updated = false;
      }

      #region Properties

      /// <summary>
      /// Identificador de la parte
      /// </summary>
      public DOMTemplateParts ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Contenido XHTML de la parte
      /// </summary>
      public string Html
      {
         get { return _html; }
         set
         {
            _html = value;
            _updated = true;
         }
      }

      /// <summary>
      /// Indica si ha sido actualizada y no guardada en la base de datos
      /// </summary>
      public bool Updated
      {
         get { return _updated; }
         set { _updated = value; }
      }

      #endregion

   }
}
