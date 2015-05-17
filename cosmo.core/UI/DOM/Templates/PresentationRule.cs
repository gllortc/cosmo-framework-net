namespace Cosmo.UI.DOM.Templates
{

   /// <summary>
   /// Implementa una regla de presentación Web
   /// </summary>
   public class PresentationRule
   {

      #region Enumerations

      /// <summary>
      /// Tipos de dispositivo
      /// </summary>
      public enum DeviceTypes : int
      {
         /// <summary>No clasificado o desconocido</summary>
         Unknown = 0,
         /// <summary>PC, portátil</summary>
         PC = 1,
         /// <summary>Pad's (iPad, etc).</summary>
         Pad = 2,
         /// <summary>Smartphones (iPhone, iTouch, Android, BlackBerry, etc).</summary>
         SmartPhone = 4,
         /// <summary>Teléfonos móbiles (con navegadores WAP, cHTML o de prestaciones limitadas).</summary>
         MobilePhone = 8
      }

      #endregion

      private int _id;
      private string _name;
      private string _agent;
      private DeviceTypes _type;
      private int _templateId;
      private int _priority;
      private string _redirectToURL;
      private bool _defaultRule;
      private bool _canDownloadFiles;
      private bool _canUploadFiles;
      private bool _canExecuteJavaScript;
      private bool _canAcceptCookies;

      /// <summary>
      /// Devuelve una instancia de PresentationRule
      /// </summary>
      public PresentationRule()
      {
         _id = 0;
         _name = "";
         _agent = "";
         _type = DeviceTypes.Unknown;
         _templateId = 0;
         _priority = 0;
         _redirectToURL = "";
         _defaultRule = false;
         _canDownloadFiles = false;
         _canUploadFiles = false;
         _canExecuteJavaScript = false;
         _canAcceptCookies = false;
      }

      #region Properties

      /// <summary>
      /// Identificador único de la regla de presentación
      /// </summary>
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Nombre de la regla de presentación
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Fragmento del BrowserAgent que describe de forma única los dispositivos englobados por esta regla
      /// </summary>
      public string Agent
      {
         get { return _agent; }
         set { _agent = value; }
      }

      /// <summary>
      /// Tipo de dispositivo que describe la regla de presentación
      /// </summary>
      public DeviceTypes Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Identificador de la plantilla de presentación que se aplicará cuando se active la regla de presentación
      /// </summary>
      public int TemplateID
      {
         get { return _templateId; }
         set { _templateId = value; }
      }

      /// <summary>
      /// Prioridad de aplicación de la regla
      /// </summary>
      public int Priority
      {
         get { return _priority; }
         set { _priority = value; }
      }

      /// <summary>
      /// Contiene la URL a la que se redirige al cliente si se activa esta regla de presentación
      /// </summary>
      public string RedirectToURL
      {
         get { return _redirectToURL; }
         set { _redirectToURL = value; }
      }

      /// <summary>
      /// Indica si se trata de la regla de visualización por defecto
      /// </summary>
      public bool Default
      {
         get { return _defaultRule; }
         set { _defaultRule = value; }
      }

      /// <summary>
      /// Indica si permite la descarga de archivos (Download)
      /// </summary>
      public bool CanDownloadFiles
      {
         get { return _canDownloadFiles; }
         set { _canDownloadFiles = value; }
      }

      /// <summary>
      /// Indica si permite cargar archivos (Upload)
      /// </summary>
      public bool CanUploadFiles
      {
         get { return _canUploadFiles; }
         set { _canUploadFiles = value; }
      }

      /// <summary>
      /// Indica si puede ejecutar secuencias JavaScript en el navegador
      /// </summary>
      public bool CanExecuteJavaScript
      {
         get { return _canExecuteJavaScript; }
         set { _canExecuteJavaScript = value; }
      }

      /// <summary>
      /// Indica si el navegador acepta Cookies
      /// </summary>
      public bool CanAcceptCookies
      {
         get { return _canAcceptCookies; }
         set { _canAcceptCookies = value; }
      }

      #endregion

   }
}

