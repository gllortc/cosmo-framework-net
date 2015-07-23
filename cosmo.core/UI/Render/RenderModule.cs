using Cosmo.Net;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Cosmo.UI.Render
{
   /// <summary>
   /// Clase que deben implementar todos los módulos de renderizado de componentes de Cosmo.
   /// Los modulos de renderizado deben incluir como dependencias los siguientes frameworks/plugins:
   /// <ul>
   /// <li>JQuery 2.0.0+</li>
   /// <li>Bootbox 4.2.0+</li>
   /// <li>Bootstrap 3.0.0+</li>
   /// </ul>
   /// </summary>
   public abstract class RenderModule
   {
      /// <summary>Declara el nombre de la carpeta que contiene los recursos para plantillas.</summary>
      public const string FOLDER_TEMPLATES = "templates";

      /// <summary>TAG XML que alberga un recurso</summary>
      public const string XML_RESOURCE_TAG = "resource";
      /// <summary>Atributo XML para especificar el tipo de recurso</summary>
      public const string XML_RESOURCE_ATTR_TYPE = "type";
      /// <summary>Atributo XML para especificar el recurso (archivo o url)</summary>
      public const string XML_RESOURCE_ATTR_VALUE = "value";

      // Internal data declarations
      private Workspace _ws;
      private string _xIcon;
      private string _skin;
      private Plugin _plugin;
      private List<string> _css;
      private List<string> _js;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="RenderModule"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      protected RenderModule(Workspace workspace, Plugin plugin)
      {
         Initialize();

         _plugin = plugin;
         _ws = workspace;

         LoadResources(_plugin.XmlPluginNode);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el nombre (ID) del módulo de renderizado.
      /// </summary>
      public string ID
      {
         get { return _plugin.ID; }
      }

      /// <summary>
      /// Devuelve el workspace actual.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
      }

      /// <summary>
      /// Devuelve una lista de recursos JavaScript necesarios para representar el contenido renderizado.
      /// </summary>
      public List<string> JavascriptResources
      {
         get { return _js; }
      }

      /// <summary>
      /// Devuelve una lista de recursos CSS necesarios para representar el contenido renderizado.
      /// </summary>
      public List<string> CssResources
      {
         get { return _css; }
      }

      /// <summary>
      /// Devuelve la referencia al icono de tipo <c>X-Icon</c> que debe decorar la pestaña/página actual.
      /// </summary>
      public string XIcon
      {
         get { return _xIcon; }
      }

      /// <summary>
      /// Devuelve el nombre (identificador) del skin a aplicar.
      /// </summary>
      public string Skin
      {
         get { return _plugin.GetString("skin"); }
      }

      /// <summary>
      /// Gets the relative path to active template.
      /// </summary>
      public string TemplatePath
      {
         get { return Url.Combine(RenderModule.FOLDER_TEMPLATES, Url.Combine(_plugin.GetString("folder"))); }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Renderiza un conjunto de controles.
      /// </summary>
      /// <param name="controls">Colección de controles a renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public string Render(ControlCollection controls)
      {
         return Render(controls, string.Empty);
      }

      /// <summary>
      /// Renderiza un conjunto de controles.
      /// </summary>
      /// <param name="controls">Colección de controles a renderizar.</param>
      /// <param name="postback">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public string Render(ControlCollection controls, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();

         foreach (Control ctrl in controls)
         {
            xhtml.AppendLine(Render(ctrl, receivedFormID));
         }

         return xhtml.ToString();
      }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Convierte un color de control en una clase CSS.
      /// </summary>
      /// <param name="bgColor">Color para el que se desea obtener la clase CSS.</param>
      /// <returns>Una cadena que representa la clase CSS a aplicar al elemento.</returns>
      public abstract string GetCssClassFromControlColor(ComponentColorScheme bgColor);

      /// <summary>
      /// Convierte un color de fondo para controles en una clase CSS.
      /// </summary>
      /// <param name="bgColor">Color para el que se desea obtener la clase CSS.</param>
      /// <returns>Una cadena que representa la clase CSS a aplicar al elemento.</returns>
      public abstract string GetCssClassFromBackgroundColor(ComponentBackgroundColor bgColor);

      /// <summary>
      /// Renderiza una vista de página.
      /// </summary>
      /// <param name="parentView">Una instancia de <see cref="PageView"/> que representa la instancia renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public abstract string RenderPage(PageView parentView);

      /// <summary>
      /// Renderiza una vista de página.
      /// </summary>
      /// <param name="parentView">Una instancia de <see cref="PageView"/> que representa la instancia renderizar.</param>
      /// <param name="receivedFormID">Contiene el identificador (DOM) del formulario recibido en la llamada.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public abstract string RenderPage(PageView parentView, string receivedFormID);

      /// <summary>
      /// Renderiza una vista parcial.
      /// </summary>
      /// <param name="parentView">Una instancia de <see cref="PartialView"/> que representa la instancia renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public abstract string RenderPage(PartialView parentView);

      /// <summary>
      /// Renderiza una vista parcial.
      /// </summary>
      /// <param name="parentView">Una instancia de <see cref="PartialView"/> que representa la instancia renderizar.</param>
      /// <param name="receivedFormID">Contiene el identificador (DOM) del formulario recibido en la llamada.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public abstract string RenderPage(PartialView parentView, string receivedFormID);

      /// <summary>
      /// Renderiza una vista modal.
      /// </summary>
      /// <param name="parentView">Una instancia de <see cref="ModalView"/> que representa la instancia renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public abstract string RenderPage(ModalView parentView);

      /// <summary>
      /// Renderiza una vista modal.
      /// </summary>
      /// <param name="parentView">Una instancia de <see cref="ModalView"/> que representa la instancia renderizar.</param>
      /// <param name="receivedFormID">Contiene el identificador (DOM) del formulario recibido en la llamada.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public abstract string RenderPage(ModalView parentView, string receivedFormID);

      /// <summary>
      /// Renderiza un control.
      /// </summary>
      /// <param name="control">Instancia del control a renderizar.</param>
      /// <param name="receivedFormID">Contiene el identificador (DOM) del formulario recibido en la llamada.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public abstract string Render(Control control, string receivedFormID);

      /// <summary>
      /// Renderiza un control.
      /// </summary>
      /// <param name="control">Instancia del control a renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public abstract string Render(Control control);

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _ws = null;
         _xIcon = string.Empty;
         _skin = string.Empty;
         _css = new List<string>();
         _js = new List<string>();
      }

      /// <summary>
      /// Carga los recursos necesarios para representar los controles de página.
      /// </summary>
      /// <param name="settingsNode">Nodo XML que contiene la configuración del plugin.</param>
      private void LoadResources(XmlNode settingsNode)
      {
         string resource;

         XmlNodeList resourcesList = ((XmlElement)settingsNode).GetElementsByTagName(RenderModule.XML_RESOURCE_TAG);
         foreach (XmlNode settingNode in resourcesList)
         {
            resource = Url.Combine(RenderModule.FOLDER_TEMPLATES, Url.Combine(_plugin.GetString("folder"), 
                                                                               settingNode.Attributes[RenderModule.XML_RESOURCE_ATTR_VALUE].Value));

            switch (settingNode.Attributes[RenderModule.XML_RESOURCE_ATTR_TYPE].Value)
            {
               case "text/css": _css.Add(resource); break;
               case "text/javascript": _js.Add(resource); break;
               case "image/x-icon": _xIcon = resource; break;
            }
         }
      }

      #endregion

   }
}
