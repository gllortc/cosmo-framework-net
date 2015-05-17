using Cosmo.Net;
using Cosmo.Utils.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Handler para el evento <see cref="DomPage.OnFormSubmited"/>.
   /// </summary>
   public delegate void OnFormSubmitedEventHandler(object source, DomFormEventArgs e);

   /// <summary>
   /// Implementa un documento DOM.
   /// </summary>
   public class DomPage : System.Web.UI.Page
   {

      #region Enumerations

      /// <summary>
      /// Tipos de página soportados.
      /// </summary>
      public enum PageType : int
      {
         /// <summary>Página de inicio</summary>
         HomePage = 1,
         /// <summary>Página de dos columnas (columna de opciones a la izquierda).</summary>
         TwoColumnsLeft = 2,
         /// <summary>Página de dos columnas (columna de opciones a la derecha).</summary>
         TwoColumnsRight = 3,
         /// <summary>Página de tres columnas.</summary>
         ThreeColumns = 4
      }

      /// <summary>
      /// Posiciones del contenido.
      /// </summary>
      public enum ContentContainer
      {
         /// <summary>Contenido de la columna derecha.</summary>
         RightColumn,
         /// <summary>Contenido de la columna central.</summary>
         CenterColumn,
         /// <summary>Contenido de la columna izquierda.</summary>
         LeftColumn
      }

      #endregion

      private string _urlhome;
      private DomPage.PageType _type;
      private DomMenu _menu;
      private DomNavigationBar _navBar;
      private DomBannerArea _banners;
      private DomTemplate _template;
      private ArrayList _contentCenter;
      private ArrayList _contentLeft;
      private ArrayList _contentRight;
      private Url _qs;
      private DomPageHeader _header;

      #region Constants

      /// <summary>Tag de uso interno.</summary>
      internal const string TAG_BODY = "__body__";

      /// <summary>Clave para la estructura de la página de inicio.</summary>
      internal const string SECTION_HOME = "home-page";
      /// <summary>Sección que contiene la estructura para páginas de dos columnas con opciones a la izquierda.</summary>
      internal const string SECTION_2COLUMNSLEFT = "two-cols-left";
      /// <summary>Sección que contiene la estructura para páginas de dos columnas con opciones a la derecha.</summary>
      internal const string SECTION_2COLUMNSRIGHT = "two-cols-right";
      /// <summary>Sección que contiene la estructura para páginas de tres columnas.</summary>
      internal const string SECTION_3COLUMNS = "three-cols";

      /// <summary>Tag: ID de plantilla</summary>
      public const string TAG_TEMPLATE_ID = "tid";
      /// <summary>Tag: Cabecera de la página</summary>
      public const string TAG_HEAD = "head";
      /// <summary>Tag: Menú principal</summary>
      public const string TAG_MENU = "menu";
      /// <summary>Tag: Barra de navegación</summary>
      public const string TAG_NAVBAR = "navbar";
      /// <summary>Tag: Zona de banners (central, superior)</summary>
      public const string TAG_BANNERS_CENTERTOP = "banners-center-top";
      /// <summary>Tag: Zona de banners (central, medio)</summary>
      public const string TAG_BANNERS_CENTERMIDDLE = "banners-center-middle";
      /// <summary>Tag: Zona de banners (central, inferior)</summary>
      public const string TAG_BANNERS_CENTERBOTTOM = "banners-center-bottom";
      /// <summary>Tag: Zona de banners (izquierda)</summary>
      public const string TAG_BANNERS_LEFT = "banners-left";
      /// <summary>Tag: Zona de banners (derecha)</summary>
      public const string TAG_BANNERS_RIGHT = "banners-right";
      /// <summary>Tag: Contenido central de la página</summary>
      public const string TAG_CONTENT_CENTER = "content";
      /// <summary>Tag: Contenido izquierdo de la página</summary>
      public const string TAG_CONTENT_LEFT = "content-left";
      /// <summary>Tag: Contenido derecho de la página</summary>
      public const string TAG_CONTENT_RIGHT = "content-right";
      /// <summary>Tag: Código de SEO</summary>
      public const string TAG_STATS = "stats";

      private const string RENDERER_INTERFACE_NAME = "MWRendererInterface";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomPage"/>.
      /// </summary>
      public DomPage() : base()
      {
         // Inicialización
         Clear();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomPage"/>.
      /// </summary>
      /// <param name="type">Tipo de página que se desea generar.</param>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el workspace para el que se desea generar la página.</param>
      /// <remarks>
      /// Este constructor determina la plantilla a usar mediante las reglas definidas.
      /// </remarks>
      public DomPage(Workspace workspace, DomPage.PageType type) : base()
      {
         // Inicialización
         Clear();

         // Si no hay plantilla, se genera un error
         this.LayoutType = type;

         // Determina la plantilla a aplicar
         DomTemplateRules rules = new DomTemplateRules(Path.Combine(workspace.FileSystemService.ApplicationPath, "templates"), true);
         this.Template = rules.GetTemplate(this.Request.UserAgent);
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomPage"/>.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla a aplicar.</param>
      /// <param name="type">Tipo de página que se desea generar.</param>
      public DomPage(DomTemplate template, DomPage.PageType type) : base()
      {
         // Inicialización
         Clear();

         // Si no hay plantilla, se genera un error
         this.LayoutType = type;
         this.Template = template;
      }

      #region Properties

      /// <summary>
      /// Contiene toda la información de la cabecera de la página.
      /// </summary>
      public new DomPageHeader Header
      {
         get { return _header; }
         set { _header = value; }
      }

      /// <summary>
      /// Devuelve la lista de parámetros proporcionados a la página durante su llamada.
      /// </summary>
      public Url Parameters
      {
         get { return _qs; }
      }

      /// <summary>
      /// Devuelve o establece la estructura de página a generar.
      /// </summary>
      public DomPage.PageType LayoutType
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// URL para ir al inicio de la aplicación.
      /// </summary>
      public string HomeURL
      {
         get { return _urlhome; }
         set { _urlhome = value; }
      }

      /// <summary>
      /// Partes del documento (fragmentos DOM).
      /// </summary>
      public ArrayList Parts
      {
         get { return _contentCenter; }
         set { _contentCenter = value; }
      }

      /// <summary>
      /// Devuelve o establece el menú principal de la página.
      /// </summary>
      public DomMenu Menu
      {
         get { return _menu; }
         set { _menu = value; }
      }

      /// <summary>
      /// Devuelve o establece la barra de navegación (y opciones) de la página.
      /// </summary>
      public DomNavigationBar NavigationBar
      {
         get { return _navBar; }
         set { _navBar = value; }
      }

      /// <summary>
      /// Devuelve o establece los banners de la página.
      /// </summary>
      public DomBannerArea Banners
      {
         get { return _banners; }
         set { _banners = value; }
      }

      /// <summary>
      /// Indica si la llamada proviene del envío de un formulario generado mediante Cosmo DOM.
      /// </summary>
      public bool IsFormPostback
      {
         get 
         {
            if (this.Request == null)
            {
               return false;
            }

            Url qs = new Url(Request.Params);
            return (qs.GetString(DomForm.PARAM_FORM_ACTION).Equals(DomForm.ACTION_SEND));
         }
      }

      /// <summary>
      /// Devuelve o establece la plantilla de presentación que usa la página.
      /// </summary>
      public DomTemplate Template
      {
         get { return _template; }
         set 
         { 
            _template = value;
            this.Header.Scripts.AddRange(_template.Scripts);
         }
      }

      /// <summary>
      /// Devuelve o establece la estructura de la página a generar.
      /// </summary>
      public DomPage.PageType LayouytType
      {
         get { return _type; }
         set { _type = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Inicializa la instancia de la página.
      /// </summary>
      public void Clear()
      {
         _header = new DomPageHeader(this);

         _qs = new Url(this.Request.Params);
         _type = DomPage.PageType.TwoColumnsRight;
         _urlhome = "#";
         _template = null;
         _menu = new DomMenu();
         _navBar = new DomNavigationBar();
         _banners = new DomBannerArea();

         _contentLeft = new ArrayList();
         _contentCenter = new ArrayList();
         _contentRight = new ArrayList();

         if (this.OnFormSubmited != null) 
            this.InitComplete += new EventHandler(Page_InitComplete);
      }

      /// <summary>
      /// Carga la plantilla adecuada a partir de las reglas.
      /// </summary>
      public void LoadTemplate(Workspace workspace)
      {
         DomTemplateRules rules = new DomTemplateRules(Path.Combine(workspace.FileSystemService.ApplicationPath, "templates"), true);
         this.Template = rules.GetTemplate(this.Request.UserAgent);
      }

      /// <summary>
      /// Carga el menú según las opciones definidas en el workspace.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Cosmo.Workspace.Workspace"/>.</param>
      public void LoadMenu(Workspace workspace)
      {
         this.Menu.LoadMenu(workspace);
      }

      /// <summary>
      /// Permite verificar la validez de un control captcha usado en la página.
      /// </summary>
      /// <param name="formControlId">Identificador del control que rellena el usuario.</param>
      /// <returns><c>true</c> si coincide el código con la entrada del usuario o <c>false</c> en cualquier otro caso.</returns>
      public bool CheckCaptcha(string formControlId)
      {
         return Captcha.ValidateCaptcha(this.Session, this.Parameters.GetString(formControlId));
      }

      /// <summary>
      /// Añade un fragmento de documento a una página.
      /// </summary>
      /// <param name="Part">Objeto que contiene el fragmento (debe ser obligatorioamente un fragmento de DOM).</param>
      /// <returns>True</returns>
      public void AddContent(object Part)
      {
         AddContent(Part, ContentContainer.CenterColumn, false);
      }

      /// <summary>
      /// Añade un fragmento de documento a una página.
      /// </summary>
      /// <param name="Part">Objeto que contiene el fragmento (debe ser obligatorioamente un fragmento de DOM).</param>
      /// <param name="saveInSession">Indica si se debe guardar el componente en la sesión para su uso posterior.</param>
      /// <returns>True</returns>
      public void AddContent(object Part, bool saveInSession)
      {
         AddContent(Part, ContentContainer.CenterColumn, saveInSession);
      }

      /// <summary>
      /// Añade un fragmento de documento a una página.
      /// </summary>
      /// <param name="Part">Objeto que contiene el fragmento (debe ser obligatorioamente un fragmento de DOM).</param>
      /// <param name="container">Indica en que columna se debe situar el contenido.</param>
      /// <param name="saveInSession">Indica si se debe guardar el componente en la sesión para su uso posterior.</param>
      /// <returns>True</returns>
      public void AddContent(object Part, ContentContainer container, bool saveInSession)
      {
         // Determina si el objeto es compatible con componentes de DOM
         if (!(Part is DomContentComponentBase))
            Part = new DomXhtml(string.Format("<!-- Cosmo Framework DOM ERROR: Unrecognized content component: {0} -->\n", Part.GetType().ToString()));

         // Almacena el fragmento de cocumento por si se debe usar en otro rendererFile
         switch (container)
         {
            case ContentContainer.LeftColumn: _contentLeft.Add(Part); break;
            case ContentContainer.RightColumn: _contentRight.Add(Part); break;
            default: _contentCenter.Add(Part); break;
         }

         // Agrega scripts y enlaces usados por el componente
         this.Header.Scripts.AddRange(GetComponentScripts(Part));

         // Almacena el fragmento en la sesión para ser usado posteriormente
         if (saveInSession)
            this.Session.Add(DomForm.GetFormCacheKey(((DomContentComponentBase)Part).ID), Part);
      }

      /// <summary>
      /// Renderiza la página y la devuelve al cliente.
      /// </summary>
      public void Render()
      {
         // Comprueba que la instancia de Page se haya especificado.
         // if (_page == null)
         //   throw new DomRenderException("No se puede renderizar la página. La instancia a System.Web.UI.Page es null.");

         this.Response.Write(ToXhtml());
      }

      /// <summary>
      /// Genera el código XHTML correspondiente a la página.
      /// </summary>
      /// <returns>Una cadena de texto que contiene el código XHTML correspondiente a la página renderizada.</returns>
      public string ToXhtml()
      {
         StringBuilder content = null;

         // Si debe usar un renderizador externo lo invoca
         // if (!string.IsNullOrEmpty(template.Renderer))
         //   return RenderExternal(template.Renderer, template.ID);

         try
         {
            //===========================================
            // Renderizado de la estructura del contenido
            //===========================================

            // Obtiene la estructura de la página
            StringBuilder body = new StringBuilder(_template.GetPage(_type));

            // Menú principal
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_MENU), _menu.Render(_template));

            // Barra de navegación
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_NAVBAR), _navBar.Render(_template));

            // Zonas de banners
            _banners.CurrentPosition = DomBanner.BannerPositions.CenterTop;
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_BANNERS_CENTERTOP), _banners.Render(_template));
            _banners.CurrentPosition = DomBanner.BannerPositions.CenterMiddle;
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_BANNERS_CENTERMIDDLE), _banners.Render(_template));
            _banners.CurrentPosition = DomBanner.BannerPositions.CenterBottom;
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_BANNERS_CENTERBOTTOM), _banners.Render(_template));
            _banners.CurrentPosition = DomBanner.BannerPositions.Left;
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_BANNERS_LEFT), _banners.Render(_template));
            _banners.CurrentPosition = DomBanner.BannerPositions.Right;
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_BANNERS_RIGHT), _banners.Render(_template));

            // Renderiza los controles estadísticos
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_STATS), string.Empty);

            //===========================================
            // Renderizado del contenido
            //===========================================

            // Contenido central
            content = new StringBuilder();
            foreach (DomContentComponentBase part in _contentCenter)
            {
               content.AppendLine(part.Render(_template, DomPage.ContentContainer.CenterColumn));
            }
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_CONTENT_CENTER), content.ToString());

            // Contenido columna izquierda
            content = new StringBuilder();
            foreach (DomContentComponentBase part in _contentLeft)
            {
               content.AppendLine(part.Render(_template, DomPage.ContentContainer.LeftColumn));
            }
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_CONTENT_LEFT), content.ToString());

            // Contenido columna derecha
            content = new StringBuilder();
            foreach (DomContentComponentBase part in _contentRight)
            {
               content.AppendLine(part.Render(_template, DomPage.ContentContainer.RightColumn));
            }
            body.Replace(DomContentComponentBase.GetTag(DomPage.TAG_CONTENT_RIGHT), content.ToString());

            //===========================================
            // Genera la estructura de la página
            //===========================================

            StringBuilder page = new StringBuilder(this.Header.Render());
            page.Replace(DomContentComponentBase.GetTag(DomPage.TAG_BODY), body.ToString());

            // Devuelve el código XHTML generado
            return page.ToString();
         }
         catch (Exception ex)
         {
            throw new DomRenderException(ex.Message, ex);
         }
      }

      /*
      /// <summary>
      /// Genera el código XHTML correspondiente a la página.
      /// </summary>
      /// <param name="dllFile">Archivo correspondiente a la DLL que contiene el renderizador externo.</param>
      /// <param name="templateId">Identificador de la plantilla asociada.</param>
      /// <returns>El código XHTML a devolver al cliente.</returns>
      public string RenderExternal(string dllFile, int templateId)
      {
         try
         {
            // Genera la instáncia
            string filename = Path.Combine(Path.Combine(_ws.ScriptPath, "bin"), dllFile);
            FileInfo file = new FileInfo(filename);
            Assembly assembly = Assembly.LoadFile(file.FullName);
            object renderer = assembly.CreateInstance("RendererBase.Renderer", true);

            // Se asegura que el Interface MWRendererInterface está implementado
            if (renderer.GetType().GetInterface(RENDERER_INTERFACE_NAME, true) == null)
               throw new Exception("El módulo DLL no es compatible con el modelo de renderizado del Framework de Cosmo.");

            // Inicializa las propiedades de la clase
            object[] parameters = new Object[1];
            parameters[0] = _ws;
            renderer.GetType().InvokeMember("Workspace", BindingFlags.SetProperty, null, renderer, parameters);

            parameters = new Object[1];
            parameters[0] = templateId.ToString();
            renderer.GetType().InvokeMember("TemplateId", BindingFlags.SetProperty, null, renderer, parameters);

            // Obtiene el método a ejecutar
            Type[] types = new Type[1];
            types[0] = this.GetType();
            MethodInfo method = renderer.GetType().GetMethod("RenderXhtml", types);
            if (method == null) throw new Exception("El módulo no responde.");

            // Ejecuta el método
            parameters = new Object[1];
            parameters[0] = this;

            return (string)method.Invoke(renderer, parameters);
         }
         catch (Exception ex)
         {
            throw ex;
         }
      }
      */

      #endregion

      #region Events

      /// <summary>
      /// Handler del evento <see cref="DomPage.OnFormSubmited"/>.
      /// </summary>
      public event OnFormSubmitedEventHandler OnFormSubmited;

      #endregion

      #region Event Handlers

      void Page_InitComplete(object sender, EventArgs e)
      {
         // Comprueba si se ha recibido un formulario
         DomForm form = DomForm.GetSubmitedForm(this);

         // Lanza eventos 
         if (this.IsFormPostback && form != null)
            if (OnFormSubmited != null) 
               OnFormSubmited(this, new DomFormEventArgs(form));
      }

      #endregion

      #region Private Methods

      /// <summary>
      /// Obtiene los scripts usados por un componente.
      /// </summary>
      private List<DomPageScript> GetComponentScripts(object component)
      {
         string id = ((DomContentComponentBase)component).ELEMENT_ROOT;

         DomTemplateComponent tcomponent = this.Template.GetContentComponent(id, ContentContainer.CenterColumn);
         if (tcomponent != null)
            return tcomponent.Scripts;
         else
            return new List<DomPageScript>();
      }

      #endregion

   }

}
