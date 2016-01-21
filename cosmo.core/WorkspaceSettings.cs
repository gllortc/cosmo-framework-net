using Cosmo.Utils;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Xml;

namespace Cosmo
{
   /// <summary>
   /// Cosmo settings manager.
   /// </summary>
   public class WorkspaceSettings
   {
      // Declaración del espacio de nombres XML
      private const string XML_NODE_ROOT = "cosmo-settings";
      private const string XML_NODE_APP = "application";
      private const string XML_NODE_DATA_SERVICE = "data-services";
      private const string XML_NODE_DATA_CONNECTION = "connection";
      private const string XML_NODE_DATA_LISTS = "data-list";
      private const string XML_NODE_SETTINGS = "settings";
      private const string XML_NODE_PARAMETER = "param";
      private const string XML_NODE_COMM_AGENT = "comm-agent";
      private const string XML_NODE_FS_AGENT = "filesystem-module";
      private const string XML_NODE_FS_SERVICE = "filesystem-services";
      private const string XML_NODE_UI_AGENT = "render-agent";
      private const string XML_NODE_UI_MENU = "menu";
      private const string XML_NODE_AUTH_SERVICE = "security-services";
      private const string XML_NODE_AUTH_MODULE = "authentication-module";
      private const string XML_NODE_LOG_SERVICE = "logging-services";

      private const string XML_ATTR_VERSION = "ver";
      private const string XML_ATTR_PLUGIN_ID = "id";
      private const string XML_ATTR_PLUGIN_DRIVER = "driver";
      private const string XML_ATTR_PLUGIN_DEFAULT = "default";
      private const string XML_ATTR_PARAM_KEY = "key";
      private const string XML_ATTR_PARAM_VALUE = "value";
      private const string XML_ATTR_APP_NAME = "name";
      private const string XML_ATTR_APP_URL = "url";
      private const string XML_ATTR_APP_MAIL = "mail";
      private const string XML_ATTR_APP_CHARSET = "charset";
      private const string XML_ATTR_APP_LANG = "language";
      private const string XML_ATTR_APP_PATH = "path";
      private const string XML_ATTR_APP_DESCRIPTION = "description";
      private const string XML_ATTR_APP_KEYWORDS = "keywords";
      private const string XML_ATTR_AUTH_MODULE = "authentication-module";
      private const string XML_ATTR_LOG_MODULE = "log-module";
      private const string XML_ATTR_DATA_DEFAULTDS = "default-connection";
      private const string XML_ATTR_PLUGIN_SELECTED = "selected";

      // Versión de la definición del archivo de configuración soportada
      private const string PROPERTIES_VERSION = "1.0";

      // Internal data declarations
      private string _appName;
      private string _appUrl;
      private string _appMail;
      private string _appCharset;
      private string _appLanguage;
      private string _appStatus;
      private string _appStatusUrlClosed;
      private string _appDescription;
      private string _appKeywords;
      private string _filename;
      private NameValueCollection _appSettings;

      /// <summary>Nombre del archivo de configuración de Cosmo (sin path).</summary>
      public const string PROPERTIES_FILENAME = "cosmo.config.xml";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="WorkspaceSettings"/>.
      /// </summary>
      /// <param name="context">Contexto de la llamada HTTP.</param>
      public WorkspaceSettings(HttpContext context)
      {
         Initialize(context);
         LoadFile();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve la ruta y nombre de archivo usado para cargar la configuración del workspace.
      /// </summary>
      public string ConfigFilename
      {
         get { return _filename; }
      }

      /// <summary>
      /// Devuelve el nombre de la aplicación.
      /// </summary>
      public string ApplicationName
      {
         get { return _appName; }
      }

      /// <summary>
      /// Devuelve la URL de la aplicación.
      /// </summary>
      public string ApplicationUrl
      {
         get { return _appUrl; }
      }

      /// <summary>
      /// Devuelve es estado en que se encuentra la aplicación.
      /// </summary>
      public WorkspaceStatus Status
      {
         get { return (_appStatus.Equals("1") || _appStatus.Equals("online") ? WorkspaceStatus.Running : WorkspaceStatus.Stopped); }
      }

      /// <summary>
      /// Devuelve el mail de contacto con el responsable de la aplicación.
      /// </summary>
      public string ApplicationMail
      {
         get { return _appMail; }
      }

      /// <summary>
      /// Devuelve el conjunto de caracteres usado para representar las páginas.
      /// </summary>
      public string ApplicationCharset
      {
         get { return _appCharset; }
      }

      /// <summary>
      /// Devuelve la descripción por defecto que describe el contenido de las páginas.
      /// </summary>
      public string ApplicationDescription
      {
         get { return _appDescription; }
      }

      /// <summary>
      /// Devuelve la lista de palabras clave que describen el contenido por defecto de las páginas.
      /// </summary>
      public string ApplicationKeywords
      {
         get { return _appKeywords; }
      }

      /// <summary>
      /// Devuelve el código de idioma.
      /// </summary>
      public string ApplicationLanguage
      {
         get { return _appLanguage; }
      }

      /// <summary>
      /// Devuelve la URL a la que se debe redirigir al cliente si el workspace se encuentra <em>offline</em>.
      /// </summary>
      public string ApplicationUrlClosed
      {
         get { return _appStatusUrlClosed; }
      }

      /// <summary>
      /// Devuelve las propiedades de configuración de la aplicación.
      /// </summary>
      public NameValueCollection Settings
      {
         get { return _appSettings; }
      }

      /// <summary>
      /// Devuelve la lista de módulos de autenticación del workspace.
      /// </summary>
      public PluginCollection AuthenticationModules { get; private set; }

      /// <summary>
      /// Devuelve la lista de módulos de datos del workspace.
      /// </summary>
      public PluginCollection DataModules { get; private set; }

      /// <summary>
      /// Devuelve la lista de módulos de datos del workspace.
      /// </summary>
      public PluginCollection DataLists { get; private set; }

      /// <summary>
      /// Devuelve la lista de módulos de comunicación del workspace.
      /// </summary>
      public PluginCollection CommunicationModules { get; private set; }

      /// <summary>
      /// Gets the list of all render modules defined in the workspace.
      /// </summary>
      public PluginCollection RenderModules { get; private set; }

      /// <summary>
      /// Gets the list of all menu providers defined in the workspace.
      /// </summary>
      public PluginCollection MenuModules { get; private set; }

      /// <summary>
      /// Gets the list of all file system controllers defined in the workspace.
      /// </summary>
      public PluginCollection FileSystemControllers { get; private set; }

      /// <summary>
      /// Gets the list of LOG modules.
      /// </summary>
      public PluginCollection LogModules { get; private set; }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve el valor de una clave de configuración de tipo <see cref="String"/>.
      /// </summary>
      /// <param name="key">Clave</param>
      /// <param name="defaultValue">Valor a devolver si la clave no consta en la configuración</param>
      /// <returns>El valor guardado en la configuración o si no puede hallarlo, el valor por defecto</returns>
      public string GetString(string key, string defaultValue)
      {
         try
         {
            // Actualiza o agrega el valor
            if (_appSettings[key] == null)
            {
               return defaultValue;
            }
            else
            {
               return _appSettings[key];
            }
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Devuelve el valor de una clave de configuración de tipo string
      /// </summary>
      /// <param name="key">Clave</param>
      /// <returns>El valor guardado en la configuración o si no puede hallarlo, el valor por defecto</returns>
      public string GetString(string key)
      {
         return GetString(key, string.Empty);
      }

      /// <summary>
      /// Devuelve el valor de una clave de configuración booleana
      /// </summary>
      /// <param name="key">Clave</param>
      /// <param name="defaultValue">Valor a devolver si la clave no consta en la configuración</param>
      /// <returns>El valor guardado en la configuración o si no puede hallarlo, el valor por defecto</returns>
      public bool GetBoolean(string key, bool defaultValue)
      {
         try
         {
            // Actualiza o agrega el valor
            if (_appSettings[key] == null)
            {
               return defaultValue;
            }
            else
            {
               string value = _appSettings[key].Trim();
               if (value.Equals("1") || value.ToLower().Equals("true")) return true;
               return false;
            }
         }
         catch
         {
            return defaultValue;
         }
      }

      /// <summary>
      /// Devuelve el valor de una clave de configuración booleana
      /// </summary>
      /// <param name="key">Clave</param>
      /// <returns>El valor guardado en la configuración o si no puede hallarlo, el valor por defecto</returns>
      public bool GetBoolean(string key)
      {
         return this.GetBoolean(key, false);
      }

      /// <summary>
      /// Devuelve el valor de una clave de configuración numérico entero
      /// </summary>
      /// <param name="key">Clave</param>
      /// <param name="defaultValue">Valor a devolver si la clave no consta en la configuración</param>
      /// <returns>El valor guardado en la configuración o si no puede hallarlo, el valor por defecto</returns>
      public int GetInt(string key, int defaultValue)
      {
         try
         {
            // Actualiza o agrega el valor
            if (_appSettings[key] == null)
            {
               return defaultValue;
            }
            else
            {
               int ival = defaultValue;
               string value = _appSettings[key].Trim();

               if (!int.TryParse(value, out ival))
                  return defaultValue;
               else
                  return ival;
            }
         }
         catch
         {
            return defaultValue;
         }
      }

      /// <summary>
      /// Devuelve el valor de una clave de configuración numérico entero
      /// </summary>
      /// <param name="key">Clave</param>
      /// <returns>El valor guardado en la configuración o si no puede hallarlo, el valor por defecto</returns>
      public int GetInt(string key)
      {
         return this.GetInt(key, 0);
      }

      /// <summary>
      /// Establece el valor de una clave de configuración
      /// </summary>
      /// <param name="key">Clave</param>
      /// <param name="value">Valor</param>
      public void SetSetting(string key, string value)
      {
         try
         {
            // Actualiza o agrega el valor
            if (_appSettings[key] != null)
            {
               _appSettings[key] = value;
            }
            else
            {
               _appSettings.Add(key, value);
            }
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Establece el valor de una clave de configuración
      /// </summary>
      /// <param name="key">Clave</param>
      /// <param name="value">Valor</param>
      public void SetSetting(string key, object value)
      {
         try
         {
            SetSetting(key, value.ToString());
         }
         catch
         {
            throw;
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize(HttpContext context)
      {
         _appName = string.Empty;
         _appUrl = string.Empty;
         _appMail = string.Empty;
         _appCharset = string.Empty;
         _appLanguage = string.Empty;
         _appKeywords = string.Empty;
         _appDescription = string.Empty;
         _appStatus = string.Empty;
         _appStatusUrlClosed = string.Empty;
         _appSettings = new NameValueCollection();
         this.AuthenticationModules = new PluginCollection();
         this.DataModules = new PluginCollection();
         this.DataLists = new PluginCollection();
         this.CommunicationModules = new PluginCollection();
         this.RenderModules = new PluginCollection();
         this.MenuModules = new PluginCollection();
         this.FileSystemControllers = new PluginCollection();
         this.LogModules = new PluginCollection();

         _filename = Path.Combine(context.Server.MapPath(string.Empty), WorkspaceSettings.PROPERTIES_FILENAME);
      }

      /// <summary>
      /// Carga el archivo de confioguración del workspace.
      /// </summary>
      private void LoadFile()
      {
         XmlNodeList settingsNode;

         try
         {
            // Comprueba que exista el archivo
            FileInfo file = new FileInfo(_filename);
            if (!file.Exists)
            {
               throw new FileNotFoundException("Archivo de configuración no encontrado [" + _filename + "]");
            }

            // Carga el archivo XML
            XmlTextReader reader = new XmlTextReader(_filename);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_filename);
            reader.Close();

            // Comprueba que el paquete corresponda a una definición correcta
            XmlNode xnod = xmlDoc.DocumentElement;
            if (!xnod.Name.ToLower().Equals(XML_NODE_ROOT))
            {
               throw new Exception("El archivo XML proporcionado no corresponde a un archivo de configuración de Cosmo.");
            }
            if (!xnod.Attributes[XML_ATTR_VERSION].Value.ToLower().Equals(PROPERTIES_VERSION))
            {
               throw new Exception("Esta versión de Cosmo (" + Cosmo.Properties.ProductVersion + ") sólo soporta archivos de configuración según la especificación " + PROPERTIES_VERSION + ".");
            }

            // Carga las propiedades principales del workspace
            XmlNodeList appNodes = xmlDoc.GetElementsByTagName(XML_NODE_APP);
            if (appNodes.Count > 0)
            {
               foreach (XmlNode appParam in appNodes[0].ChildNodes)
               {
                  if (appParam.Attributes != null && appParam.Attributes.Count > 0)
                  {
                     switch (appParam.Attributes[XML_ATTR_PARAM_KEY].Value)
                     {
                        case "app.name": _appName = appParam.Attributes[XML_ATTR_PARAM_VALUE].Value; break;
                        case "app.url": _appUrl = appParam.Attributes[XML_ATTR_PARAM_VALUE].Value; break;
                        case "app.mail": _appMail = appParam.Attributes[XML_ATTR_PARAM_VALUE].Value; break;
                        case "app.status": _appStatus = appParam.Attributes[XML_ATTR_PARAM_VALUE].Value; break;
                        case "app.status.closedurl": _appStatusUrlClosed = appParam.Attributes[XML_ATTR_PARAM_VALUE].Value; break;
                        case "html.charset": _appCharset = appParam.Attributes[XML_ATTR_PARAM_VALUE].Value; break;
                        case "html.language": _appLanguage = appParam.Attributes[XML_ATTR_PARAM_VALUE].Value; break;
                        case "html.description": _appDescription = appParam.Attributes[XML_ATTR_PARAM_VALUE].Value; break;
                        case "html.keywords": _appKeywords = appParam.Attributes[XML_ATTR_PARAM_VALUE].Value; break;
                     }
                  }
               }
            }
            else
            {
               throw new WorkspaceSettingsException("El archivo de configuración no contiene ningúna sección <application> para definir las propiedades del workspace.");
            }

            // Carga las propiedades de las aplicaciones
            _appSettings = new NameValueCollection();
            settingsNode = xmlDoc.GetElementsByTagName(XML_NODE_SETTINGS);
            if (settingsNode.Count > 0)
            {
               XmlNodeList settings = ((XmlElement)settingsNode.Item(0)).GetElementsByTagName(XML_NODE_PARAMETER);
               foreach (XmlNode setting in settings)
               {
                  _appSettings.Add(setting.Attributes[XML_ATTR_PARAM_KEY].Value, 
                                   setting.Attributes[XML_ATTR_PARAM_VALUE].Value);
               }
            }

            // Carga la configuración de autenticación
            this.AuthenticationModules.LoadPluginCollection(xmlDoc, XML_NODE_AUTH_SERVICE, XML_ATTR_AUTH_MODULE);

            // Carga los proveedores de datos
            this.DataModules.LoadPluginCollection(xmlDoc, XML_NODE_DATA_SERVICE, XML_NODE_DATA_CONNECTION);
            this.DataLists.LoadPluginCollection(xmlDoc, XML_NODE_DATA_LISTS);

            // Carga los proveedores de comunicaciones
            this.CommunicationModules.LoadPluginCollection(xmlDoc, XML_NODE_COMM_AGENT);

            // Carga los módulos de renderización
            this.RenderModules.LoadPluginCollection(xmlDoc, XML_NODE_UI_AGENT);

            // Carga los módulos proveedores de menús
            this.MenuModules.LoadPluginCollection(xmlDoc, XML_NODE_UI_MENU);

            // Carga los módulos de gestión de archivos (FileSystem)
            this.FileSystemControllers.LoadPluginCollection(xmlDoc, XML_NODE_FS_SERVICE, XML_NODE_FS_AGENT);

            // Load LOG plugins
            this.LogModules.LoadPluginCollection(xmlDoc, XML_NODE_LOG_SERVICE, XML_ATTR_LOG_MODULE);
         }
         catch (Exception ex)
         {
            throw new WorkspaceSettingsException(ex.Message, ex);
         }
      }

      #endregion

   }
}
