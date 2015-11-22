using Cosmo.Communications;
using Cosmo.Data;
using Cosmo.Data.Connection;
using Cosmo.Diagnostics;
using Cosmo.FileSystem;
using Cosmo.Security;
using Cosmo.Security.Auth;
using Cosmo.UI;
using System;
using System.Reflection;
using System.Web;

[assembly: CLSCompliant(true)]
namespace Cosmo
{

   #region Enumerations

   /// <summary>
   /// Enumera los posibles estados de un workspace
   /// </summary>
   public enum WorkspaceStatus : int
   {
      /// <summary>Activo</summary>
      Running = 1,
      /// <summary>Detenido</summary>
      Stopped = 0
   }

   #endregion

   /// <summary>
   /// Implementa un workspace, un espacio de trabajo para aplicaciones Cosmo.
   /// </summary>
   public class Workspace
   {
      // Internal data declarations
      private DataService _dataSrv;
      private HttpContext _context;
      private WorkspaceSettings _properties;
      private UserSession _user;
      private SecurityService _authSrv;
      private CommunicationsService _commSrv;
      private UIService _uiSrv;
      private FileSystemService _fsSrv;
      private LoggerService _logSrv;

      #region Constants

      // PREDEFINED ROLES

      /// <summary>Users who have administrative role: access all services and server control</summary>
      public const string ROLE_ADMINISTRATOR = "admin";

      // Páginas públicas

      /// <summary>Página de inicio</summary>
      public const string COSMO_URL_DEFAULT = "Home";          // "default.aspx";
      /// <summary>Página de mensajes privados personal</summary>
      public const string COSMO_URL_USER_MESSAGES = "usrmsg.aspx";
      /// <summary>Página de contacto</summary>
      public const string COSMO_URL_CONTACT = "cs_sys_contact.aspx";

      // REST Services (Handlers)

      /// <summary>Handler para operaciones de usuario i autenticación/autorización</summary>
      public const string REST_SERVICE_USERS = "users.do";
      /// <summary>Handler para acceso al servidor REST de Communication Services</summary>
      public const string REST_SERVICE_COMM = "CommService";
      /// <summary>Handler para operaciones del servidor web</summary>
      public const string REST_SERVER_HANDLER = "server.do";

      // parámetros URL 

      /// <summary>Parámetro que contiene la acción de autenticación a efectuar.</summary>
      public const string PARAM_LOGIN_ACTION = "action";
      /// <summary>Parámetro que contiene la redirección a una URL después del login.</summary>
      public const string PARAM_LOGIN_REDIRECT = "redir";
      /// <summary>Parámetro que permite indicar la acción ejecutada (constantes ACTION_XXXX).</summary>
      public const string PARAM_ACTION = "_act_";
      /// <summary>Parámetro que permite indicar el comando a ejecutar (constantes COMMAND_XXXX).</summary>
      public const string PARAM_COMMAND = "_cmd_";
      /// <summary>Parámetro que permite indicar un usuario mediante ID.</summary>
      public const string PARAM_USER_ID = "usrid";
      /// <summary>Parámetro que permite indicar un usuario mediante nombre/login.</summary>
      public const string PARAM_USER_NAME = "usr";
      /// <summary>Parámetro que permite indicar el identificador de un objeto.</summary>
      public const string PARAM_OBJECT_ID = "oid";
      /// <summary>Parámetro que permite indicar el identificador de una carpeta/categoria.</summary>
      public const string PARAM_FOLDER_ID = "fid";
      /// <summary>Parámetro que permite indicar el menu activo.</summary>
      public const string PARAM_MENU_ACTIVE = "mnu";

      // Comandos

      /// <summary>Comando para crear un nuevo objeto.</summary>
      public const string COMMAND_ADD = "_add_";
      /// <summary>Comando para actualizar un objeto.</summary>
      public const string COMMAND_EDIT = "_edit_";
      /// <summary>Comando para eliminar un objeto.</summary>
      public const string COMMAND_DELETE = "_del_";

      // Criptografia

      // Clave de encriptación de los datos de la sesión de usuario.
      private const string KeySessionEncode = "Kuy16bYg2f4k1jYG";

      // Claves de almacenamiento de datos en SESSION

      /// <summary>Clave de acceso al contexto del workspace almacenado en la sesión del servidor web.</summary>
      public const string SESSION_WORKSPACE = "cosmo.session.ws";
      /// <summary>Clave de acceso al último error generado en la sesión.</summary>
      public const string SESSION_LASTERROR = "cosmo.session.lasterror";

      // Claves de almacenamiento de datos en CACHE

      /// <summary>Clave de acceso al contexto del workspace almacenado en la sesión del servidor web.</summary>
      public const string CACHE_WORKSPACE = "cosmo.cache.ws";
      /// <summary>Clave de acceso al último error generado en la sesión.</summary>
      public const string CACHE_LASTERROR = "cosmo.cache.lasterror";

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="Workspace"/>. 
      /// </summary>
      /// <param name="context">Instancia de <see cref="HttpContext"/> que contiene el contexto actual de servidor web.</param>
      public Workspace(HttpContext context)
      {
         Initialize(context);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el nombre del workspace.
      /// </summary>
      public string Name
      {
         get { return _properties.ApplicationName; }
      }

      /// <summary>
      /// Gets the main workspace URL.
      /// </summary>
      public string Url
      {
         get { return _properties.ApplicationUrl; }
      }

      /// <summary>
      /// Gets the main mail account for workspace.
      /// </summary>
      public string Mail
      {
         get { return _properties.ApplicationMail; }
      }

      /// <summary>
      /// Devuelve el conjunto de carácteres usado para representar las páginas XHTML del workspace.
      /// </summary>
      public string HtmlCharset
      {
         get { return _properties.ApplicationCharset; }
      }

      /// <summary>
      /// Devuelve el código de idioma correspondiente al contenido del workspace.
      /// </summary>
      public string HtmlLanguage
      {
         get { return _properties.ApplicationLanguage; }
      }

      /// <summary>
      /// Devuelve la URL dónde se redireccionará al cliente si el workspace está inactivo
      /// o si la IP de acceso está en la lista de bloqueadas.
      /// </summary>
      public string UrlClosed
      {
         get { return _properties.ApplicationUrlClosed; }
      }

      /// <summary>
      /// Devuelve la descripción por defecto a insertar en las páginas.
      /// </summary>
      public string PageDescription
      {
         get { return _properties.ApplicationDescription; }
      }

      /// <summary>
      /// Devuelve las palabras clave por defecto a insertar en las páginas.
      /// </summary>
      public string PageKeywords
      {
         get { return _properties.ApplicationKeywords; }
      }

      /// <summary>
      /// Gets the IP from the client.
      /// </summary>
      public string RequestIP
      {
         get { return _context.Request.ServerVariables["REMOTE_ADDR"]; }
      }

      /// <summary>
      /// Devuelve una instancia al servicio de datos del workspace.
      /// </summary>
      public DataService DataService
      {
         get
         {
            if (_dataSrv == null) _dataSrv = new DataService(this);
            return _dataSrv;
         }
         set { _dataSrv = value; }
      }

      /// <summary>
      /// Gets or sets la conexión a la base de datos.
      /// </summary>
      public IDataModule DataSource
      {
         get { return DataService.GetDataSource(); }
      }

      /// <summary>
      /// Gets or sets la instancia de <see cref="HttpContext"/> del servidor.
      /// </summary>
      public HttpContext Context
      {
         get { return _context; }
         set
         {
            _context = value;
            // Initialize(value);
         }
      }

      /// <summary>
      /// Gets or sets el estado del workspace.
      /// </summary>
      public WorkspaceStatus Status
      {
         get { return _properties.Status; }
      }

      /// <summary>
      /// Gets a new instance of <see cref="FileSystemService"/> que permite acceder al sistema de archivos del workspace.
      /// </summary>
      public FileSystemService FileSystemService
      {
         get
         {
            if (_fsSrv == null) _fsSrv = new FileSystemService(this, _properties.FileSystemControllers);
            return _fsSrv;
         }
      }

      /// <summary>
      /// Gets or sets la instancia de <see cref="WorkspaceSettings"/> que contiene la configuración del workspace.
      /// </summary>
      public WorkspaceSettings Settings
      {
         get { return _properties; }
         set { _properties = value; }
      }

      /// <summary>
      /// Gets the logging service instance for the current workspace.
      /// </summary>
      public LoggerService Logger
      {
         get
         {
            if (_logSrv == null) _logSrv = new LoggerService(this);
            return _logSrv;
         }
      }

      /// <summary>
      /// Gets the security service instance for the current workspace.
      /// </summary>
      public SecurityService SecurityService
      {
         get
         {
            if (_authSrv == null) _authSrv = new SecurityService(this);
            return _authSrv;
         }
      }

      /// <summary>
      /// Gets the UI service instance for the current workspace.
      /// </summary>
      public UIService UIService
      {
         get
         {
            if (_uiSrv == null) _uiSrv = new UIService(this, Context);
            return _uiSrv;
         }
      }

      /// <summary>
      /// Devuelve el servicio de comunicaciones del workspace.
      /// </summary>
      public CommunicationsService Communications
      {
         get
         {
            if (_commSrv == null) _commSrv = new CommunicationsService(this);
            return _commSrv;
         }
      }

      /// <summary>
      /// Permite obtener la cuenta de usuario autenticado actualmente.
      /// </summary>
      public UserSession CurrentUser
      {
         get
         {
            if (_user == null) _user = new UserSession(this);
            return _user;
         }
      }

      /// <summary>
      /// Devuelve la versión de la libreria que define el objeto WSWorkspace
      /// </summary>
      public static string Version
      {
         get
         {
            return Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                   Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                   Assembly.GetExecutingAssembly().GetName().Version.Revision + "." +
                   Assembly.GetExecutingAssembly().GetName().Version.Build; ;
         }
      }

      /// <summary>
      /// Gets the Cosmo Core product name.
      /// </summary>
      public static string ProductName
      {
         get 
         {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).ProductName;
         }
      }

      /// <summary>
      /// Gets the current relative URL and query string.
      /// </summary>
      public string CurrentUrl
      {
         get { return Context.Request.Url.PathAndQuery; }
      }

      /// <summary>
      /// Gets the current absolute URL and query string.
      /// </summary>
      public string CurrentAbsoluteUrl
      {
         get { return Cosmo.Net.Url.Combine(this.Url, Context.Request.Url.PathAndQuery); }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Obtiene la instancia de <see cref="Workspace"/> usada por el usuario durante la sesión.
      /// </summary>
      /// <param name="context">Una instancia de <see cref="System.Web.HttpContext"/> que describe el contexto de la llamada al servidor.</param>
      public static Workspace GetWorkspace(HttpContext context)
      {
         Workspace wws = null;

         if (context == null || context.Session == null || context.Session[Workspace.SESSION_WORKSPACE] == null)
         {
            wws = new Workspace(context);
            if (context.Session != null)
            {
               context.Session[Workspace.SESSION_WORKSPACE] = wws;
            }
         }
         else
         {
            wws = (Workspace)context.Session[Workspace.SESSION_WORKSPACE];
            wws.Context = context;
         }

         if (wws.Status == WorkspaceStatus.Stopped)
         {
            context.Response.Redirect(wws.UrlClosed, true);
         }

         return wws;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize(HttpContext context)
      {
         _context = context;

         _dataSrv = null;;
         _properties = null;
         _user = null;
         _authSrv = null;
         _commSrv = null;
         _uiSrv = null;
         _fsSrv = null;
         _logSrv = null;

         if (_context != null && _context.Request != null)
         {
            _properties = new WorkspaceSettings(_context);
         }

         // Preload log services
         _logSrv = new LoggerService(this);
      }

      #endregion

   }
}
