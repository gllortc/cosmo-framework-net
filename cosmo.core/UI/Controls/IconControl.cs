namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un icono basado en fuentes (soporta Font Awsome + Glyphicons).
   /// Más información: http://getbootstrap.com/components/#glyphicons
   /// </summary>
   public class IconControl : Control
   {

      #region Icon Codes

      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_ARROW_UP = "glyphicon-arrow-up";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_ASTERISK = "fa-asterisk";
      /// <summary>Código de icono: Campana </summary>
      public const string ICON_BELL = "fa-bell";
      /// <summary>Código de icono: Libro </summary>
      public const string ICON_BOOK = "fa-book";
      /// <summary>Código de icono: Favorito</summary>
      public const string ICON_BOOKMARK = "fa-bookmark";
      /// <summary>Código de icono: Calendario</summary>
      public const string ICON_CALENDAR = "fa-calendar";
      /// <summary>Código de icono: Calendario vacío</summary>
      public const string ICON_CALENDAR_EMPTY = "fa-calendar-o";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_CAMERA = "fa-camera";
      /// <summary>Código de icono: Diagrama de barras</summary>
      public const string ICON_CHART = "fa-bar-chart-o";
      /// <summary>Código de icono: Accón realizada correctamente</summary>
      public const string ICON_CHECK = "fa-check";
      /// <summary>Código de icono: Flecha hacia abajo</summary>
      public const string ICON_CHEVRON_DOWN = "fa-chevron-down";
      /// <summary>Código de icono: Flecha hacia arriba</summary>
      public const string ICON_CHEVRON_UP = "fa-chevron-up";
      /// <summary>Código de icono: Comentario</summary>
      public const string ICON_COMMENT = "fa-comment";
      /// <summary>Código de icono: Comentarios</summary>
      public const string ICON_COMMENTS = "fa-comments";
      /// <summary>Código de icono: Nube</summary>
      public const string ICON_CLOUD = "fa-cloud";
      /// <summary>Código de icono: Eliminar</summary>
      public const string ICON_DELETE = "fa-times";
      /// <summary>Código de icono: Descargar / download</summary>
      public const string ICON_DOWNLOAD = "fa-download";
      /// <summary>Código de icono: Editar / escribir</summary>
      public const string ICON_EDIT = "fa-pencil";
      /// <summary>Código de icono: Sobre de correo</summary>
      public const string ICON_ENVELOPE = "fa-envelope";
      /// <summary>Código de icono: Sobre de correo 2</summary>
      public const string ICON_ENVELOPE_2 = "fa-envelope-o";
      /// <summary>Icon code: Eye</summary>
      public const string ICON_EYE = "fa-eye";
      /// <summary>Icon code: Eye slashed</summary>
      public const string ICON_EYE_SLASH = "fa-eye-slash";
      /// <summary>Código de icono: Cuentagotas</summary>
      public const string ICON_EYEDROPER = "fa-eyedropper";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_FOLDER_OPEN = "fa-folder-open";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_FOLDER_CLOSE = "fa-folder";
      /// <summary>Código de icono: Regalo</summary>
      public const string ICON_GIFT = "fa-gift";
      /// <summary>Código de icono: Globo terráqueo</summary>
      public const string ICON_GLOBE = "fa-globe";
      /// <summary>Código de icono: Casa / Inicio</summary>
      public const string ICON_HOME = "fa-home";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_INBOX = "glyphicon-inbox";
      /// <summary>Código de icono: Normas</summary>
      public const string ICON_LEGAL = "fa-gavel";
      /// <summary>Código de icono: Candado cerrado</summary>
      public const string ICON_LOCK = "fa-lock";
      /// <summary>Código de icono: Marcador de mapa</summary>
      public const string ICON_MAP_MARKER = "fa-map-marker";
      /// <summary>Código de icono: Menos</summary>
      public const string ICON_MINUS = "fa-minus";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_MOBILE = "fa-mobile";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_PHONE = "fa-phone";
      /// <summary>Código de icono: Añadir / Crear nuevo elemento</summary>
      public const string ICON_PLUS = "fa-plus";
      /// <summary>Icon code: Prohibited</summary>
      public const string ICON_PROHIBITED = "fa-minus-circle";
      /// <summary>Icon code: Prohibited with circle</summary>
      public const string ICON_PROHIBITED_CIRCLE = "fa-times-circle";
      /// <summary>Icon code: Question</summary>
      public const string ICON_QUESTION = "fa-question-circle";
      /// <summary>Código de icono: Responder </summary>
      public const string ICON_REPLY = "fa-mail-reply";
      /// <summary>Código de icono: Refrescar / Recargar </summary>
      public const string ICON_REFRESH = "fa-refresh";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_REMOVE = "glyphicon-remove";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_RESIZE_HORIZONTAL = "glyphicon-resize-horizontal";
      /// <summary>Código de icono: Buscar / Lupa</summary>
      public const string ICON_SEARCH = "fa-search";
      /// <summary>Código de icono: Flecha hacia arriba </summary>
      public const string ICON_SEND = "glyphicon-send";
      /// <summary>Código de icono: Carrito de la compra</summary>
      public const string ICON_SHOPPING_CART = "fa-shopping-cart";
      /// <summary>Código de icono: Organigrama / Mapa de sitio</summary>
      public const string ICON_SITEMAP = "fa-sitemap";
      /// <summary>Código de icono: Circulo de espera</summary>
      public const string ICON_SPINNER = "fa-spinner";
      /// <summary>Código de icono: Etiqueta</summary>
      public const string ICON_TAG = "fa-tag";
      /// <summary>Código de icono: Etiquetas</summary>
      public const string ICON_TAGS = "fa-tags";
      /// <summary>Código de icono: Reloj</summary>
      public const string ICON_TIME = "fa-clock-o";
      /// <summary>Código de icono: Upload / Subir archivos </summary>
      public const string ICON_UPLOAD = "fa-upload";
      /// <summary>Código de icono: Candado abierto</summary>
      public const string ICON_UNLOCK = "fa-unlock";
      /// <summary>Código de icono: usuario</summary>
      public const string ICON_USER = "glyphicon-user";
      /// <summary>Código de icono: Señal de exclamación</summary>
      public const string ICON_WARNING = "fa-warning";
      /// <summary>Código de icono: Llave inglesa / Herramientas</summary>
      public const string ICON_WRENCH = "fa-wrench";

      #endregion

      /// <summary>Acción del icono: Rodar</summary>
      public const string ACTION_SPINNING = "fa-spin";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="IconControl"/>.
      /// </summary>
      /// <param name="code">Código de icono a representar.</param>
      /// <param name="container">El contenedor dónde se representará el control.</param>
      public IconControl(ViewContainer container, string code)
         : base(container)
      {
         Initialize();

         Code = code;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="IconControl"/>.
      /// </summary>
      /// <param name="container">El contenedor dónde se representará el control.</param>
      /// <param name="code">Código de icono a representar.</param>
      /// <param name="banned">Indica si l icono debe mostrarse baneado (con una señal de prohibición justo encima).</param>
      public IconControl(ViewContainer container, string code, bool banned)
         : base(container)
      {
         Initialize();

         Code = code;
         Banned = banned;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="IconControl"/>.
      /// </summary>
      /// <param name="code">Código de icono a representar.</param>
      /// <param name="bgColor">Color de fondo.</param>
      /// <param name="container">El contenedor dónde se representará el control.</param>
      public IconControl(ViewContainer container, string code, ComponentBackgroundColor bgColor)
         : base(container)
      {
         Initialize();

         Code = code;
         BackgroundColor = bgColor;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el código del icono.
      /// </summary>
      public string Code { get; set; }

      /// <summary>
      /// Devuelve o establece el código de movimiento del icono.
      /// </summary>
      public string Action { get; set; }

      /// <summary>
      /// Devuelve o establece el color de fondo del icono.
      /// </summary>
      public ComponentBackgroundColor BackgroundColor { get; set; }

      /// <summary>
      /// Indica si l icono debe mostrarse baneado (con una señal de prohibición justo encima).
      /// </summary>
      public bool Banned { get; set; }

      #endregion

      #region Static Members

      /// <summary>
      /// Genera el código XHTML para un icono de forma directa.
      /// </summary>
      /// <param name="viewport"></param>
      /// <param name="code"></param>
      /// <returns></returns>
      public static string GetIcon(ViewContainer viewport, string code)
      {
         return new IconControl(viewport, code).ToXhtml();
      }

      /// <summary>
      /// Genera el código XHTML para un icono de forma directa.
      /// </summary>
      /// <param name="viewport"></param>
      /// <param name="code"></param>
      /// <param name="banned"></param>
      /// <returns></returns>
      public static string GetIcon(ViewContainer viewport, string code, bool banned)
      {
         return new IconControl(viewport, code, banned).ToXhtml();
      }

      /// <summary>
      /// Genera el código XHTML para un icono de forma directa.
      /// </summary>
      /// <param name="viewport"></param>
      /// <param name="code"></param>
      /// <param name="bgColor"></param>
      /// <returns></returns>
      public static string GetIcon(ViewContainer viewport, string code, ComponentBackgroundColor bgColor)
      {
         return new IconControl(viewport, code, bgColor).ToXhtml();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         Code = string.Empty;
         Action = string.Empty;
         BackgroundColor = ComponentBackgroundColor.None;
         Banned = false;
      }

      #endregion

   }
}
