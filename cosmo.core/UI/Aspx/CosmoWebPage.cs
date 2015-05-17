using Cosmo.Net;
using Cosmo.Security.Auth;
using Cosmo.UI.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Cosmo.UI.Aspx
{
   /// <summary>
   /// Implementa una página XHTML para representar bajo el framework Cosmo.
   /// </summary>
   public class CosmoWebPage : System.Web.UI.Page
   {
      // Declaracción de variables internas
      private Workspace _ws;
      private LayoutContainerControl _layout;

      /// <summary>
      /// Devuelve una instancia de <see cref="CosmoWebPage"/>.
      /// </summary>
      public CosmoWebPage()
      {
         Initialize();
      }

      /// <summary>
      /// Evento que se invoca antes de iniciar el renderizado ASP.
      /// </summary>
      protected void Page_PreInit(object sender, EventArgs e)
      {
         InitializePage();

         // ¿Es necesario?
         // Workspace.Context = Context;
      }

      /// <summary>
      /// Captura el evento de pre-renderizado (todo el contenido ya está completo) para agregar los metadatos de la página.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      protected void Page_PreRender(object sender, EventArgs e)
      {
         AddMetaInformation();
      }

      /// <summary>
      /// Devuelve el workspace actual.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la cabecera.
      /// </summary>
      public Cosmo.Utils.ControlCollection HeaderContent
      {
         get { return _layout.Header; }
         set { _layout.Header = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene el pie.
      /// </summary>
      public Cosmo.Utils.ControlCollection FooterContent
      {
         get { return _layout.Footer; }
         set { _layout.Footer = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la columna izquierda.
      /// </summary>
      public Cosmo.Utils.ControlCollection LeftContent
      {
         get { return _layout.LeftContent; }
         set { _layout.LeftContent = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la zona de contenidos de la página.
      /// </summary>
      public Cosmo.Utils.ControlCollection MainContent
      {
         get { return _layout.MainContent; }
         set { _layout.MainContent = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la columna derecha.
      /// </summary>
      public Cosmo.Utils.ControlCollection RightContent
      {
         get { return _layout.RightContent; }
         set { _layout.RightContent = value; }
      }

      /// <summary>
      /// Devuelve o establece el identificador del elemento de menú activo.
      /// </summary>
      public string ActiveMenuId
      {
         get { return ((CosmoMasterPage)Master).ActiveMenuId; }
         set { ((CosmoMasterPage)Master).ActiveMenuId = value; }
      }

      /// <summary>
      /// Devuelve o establece el título de la página.
      /// </summary>
      public string PageTitle
      {
         get { return Page.Title; }
         set { Page.Title = value; }
      }

      /// <summary>
      /// Devuelve o establece la descripción del contenido de la página.
      /// </summary>
      public string PageDescription
      {
         get { return Page.MetaDescription; }
         set { Page.MetaDescription = value; }
      }

      /// <summary>
      /// Devuelve o establece las palabras clave que definen el contenido de la página.
      /// </summary>
      public string PageKeywords
      {
         get { return Page.MetaKeywords; }
         set { Page.MetaKeywords = value; }
      }

      /// <summary>
      /// Indica si existe un usuario autenticado.
      /// </summary>
      /// <returns><c>true</c> si existe una sesión autenticada o <c>false</c> en cualquier otro caso.</returns>
      public bool IsAuthenticated
      {
         get { return AuthenticationService.IsAuthenticated(Session); }
      }

      /// <summary>
      /// Comprueba que exista una sesión autenticada. 
      /// De no ser así, redirige hacia la página de <em>login</em>. Una vez completada la autenticación
      /// redirige nuevamente a la página actual.
      /// </summary>
      public void CheckAuthentication()
      {
         CheckAuthentication(HttpContext.Current.Request.Url.AbsoluteUri);
      }

      /// <summary>
      /// Comprueba que exista una sesión autenticada. 
      /// De no ser así, redirige hacia la página de <em>login</em>. Una vez completada la autenticación
      /// redirige a la URL especificada.
      /// </summary>
      /// <param name="destinationUrl">URL de destino una vez efectuada la autenticación.</param>
      public void CheckAuthentication(string destinationUrl)
      {
         if (!AuthenticationService.IsAuthenticated(Session))
         {
            Url url = new Url(Cosmo.Workspace.COSMO_URL_LOGIN);
            url.AddParameter(Cosmo.Workspace.PARAM_LOGIN_REDIRECT, destinationUrl);

            Response.Redirect(url.ToString(true));
            Response.End();
         }
      }

      /// <summary>
      /// Comprueba la autorización para la sesión autenticada actual.. 
      /// De no existir sesión autenticada, redirige hacia la página de <em>login</em>.
      /// </summary>
      public void CheckAuthorization(string profile)
      {
         CheckAuthentication();
         AuthenticationService.IsAuthenticated(Workspace.Context.Session);
      }

      /// <summary>
      /// Agrega la información de cabecera de la página
      /// </summary>
      /// <param name="title">Título de la página.</param>
      /// <param name="description">Descripción de la página.</param>
      public void AddMetaInformation(string title, string description)
      {
         this.AddMetaInformation(title, description, string.Empty);
      }

      /// <summary>
      /// Agrega la información de cabecera de la página
      /// </summary>
      /// <param name="title">Título de la página.</param>
      public void AddMetaInformation(string title)
      {
         this.AddMetaInformation(title, string.Empty, string.Empty);
      }

      /// <summary>
      /// Agrega la información de cabecera de la página
      /// </summary>
      public void AddMetaInformation()
      {
         this.AddMetaInformation(string.Empty, string.Empty, string.Empty);
      }

      /// <summary>
      /// Agrega dinamicamente una referencia a un archivo JavaScript
      /// </summary>
      /// <param name="fileUrl">URL del archivo JS a agregar</param>
      public void AddJSScriptReference(string fileUrl)
      {
         HtmlGenericControl js = new HtmlGenericControl("script");
         js.Attributes["type"] = "text/javascript";
         js.Attributes["src"] = fileUrl;
         this.Header.Controls.Add(js);
      }

      /// <summary>
      /// Agrega dinamicamente un script JavaScript
      /// </summary>
      /// <param name="script">Codigo JavaScript a agregar</param>
      public void AddJSScript(string script)
      {
         HtmlGenericControl js = new HtmlGenericControl("script");
         js.Attributes["type"] = "text/javascript";
         js.InnerHtml = script;
         this.Header.Controls.Add(js);
      }

      /// <summary>
      /// Agrega dinamicamente una referencia a una hoja de estilos
      /// </summary>
      /// <param name="URL">Dirección del archivo CSS</param>
      public void AddCSSReference(string URL)
      {
         HtmlLink link = new HtmlLink();
         link.Href = URL;
         link.Attributes.Add("type", "text/css");
         link.Attributes.Add("rel", "stylesheet");
         this.Header.Controls.Add(link);
      }

      /// <summary>
      /// Renderiza un control y devuelve el código XHTML necesario para su representación.
      /// </summary>
      /// <param name="control">Una instancia que implemente <see cref="Control"/> y que representa el control a renderizar.</param>
      /// <returns>Una cadena que contiene el código XHTML necesario para representar el control.</returns>
      public string RenderControl(Cosmo.UI.Controls.Control control)
      {
         return _ws.UIService.Render(control);
      }

      /// <summary>
      /// Renderiza el contenido de la página y devuelve el código XHTML necesario para su representación.
      /// </summary>
      public void Render(HtmlGenericControl containerControl)
      {
         containerControl.InnerHtml = _ws.UIService.Render(_layout);
      }

      #region Private Methods

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         // Inicializa variables
         _ws = null;
         _layout = new LayoutContainerControl(null);
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void InitializePage()
      {
         // Inicializa variables
         _ws = Workspace.GetWorkspace(Context);

         PageTitle = _ws.Name;
      }

      /// <summary>
      /// Agrega la información de cabecera de la página
      /// </summary>
      /// <param name="title">Título de la página.</param>
      /// <param name="description">Descripción de la página.</param>
      /// <param name="keywords">Palabras clave de la página.</param>
      private void AddMetaInformation(string title, string description, string keywords)
      {
         // HtmlTitle ptitle = null;
         HtmlLink link = null;

         // Título de la página
         // ptitle = new HtmlTitle();
         // ptitle.ID = "title";
         // ptitle.Text = string.IsNullOrEmpty(title) ? Workspace.PropertyName : Workspace.PropertyName + " - " + title;
         // this.Header.Controls.Add(ptitle);

         // Metainformación básica de la página
         this.Title = string.IsNullOrEmpty(this.Title) ? Workspace.Name : Workspace.Name + " - " + this.Title;
         this.MetaDescription = string.IsNullOrEmpty(this.MetaDescription) ? Workspace.PageDescription : this.MetaDescription;
         this.MetaKeywords = string.IsNullOrEmpty(this.MetaKeywords) ? Workspace.PageKeywords : this.MetaKeywords;

         // Metainformación completa
         AddMeta("Content-Type", "text/html; charset=iso-8859-1");
         AddMeta("cache-control", "no-cache");
         AddMeta("expires", "3600");
         AddMeta("revisit-after", "2 days");
         AddMeta("robots", "index,follow");
         AddMeta("publisher", Workspace.Name);
         AddMeta("copyright", Workspace.Name);
         AddMeta("author", Workspace.Name);
         AddMeta("generator", Workspace.ProductName + " v" + Workspace.Version);
         AddMeta("distribution", "global");
         // AddMeta("description", string.IsNullOrEmpty(description) ? Workspace.PageDescription : description);
         // AddMeta("keywords", string.IsNullOrEmpty(keywords) ? Workspace.PageKeywords : keywords);

         // Favicon
         link = new HtmlLink();
         link.Href = "images/favicon.ico";
         link.Attributes.Add("rel", "icon");
         link.Attributes.Add("type", "image/x-icon");
         this.Header.Controls.Add(link);

         foreach (string css in _ws.UIService.CssResources)
         {
            AddCss(css);
         }

         foreach (string js in _ws.UIService.JavascriptResources)
         {
            AddJavaScript(js);
         }

         /*
         // Inclusión de Bootstrap
         AddCss("include/bootstrap/css/bootstrap.min.css");
         AddJavaScript("include/bootstrap/js/jquery-2.1.0.min.js");
         AddJavaScript("include/bootstrap/js/bootstrap.min.js");

         // Plugins - Docs
         AddCss("include/bootstrap/plugins/docs/css/docs.min.css");

         // Plugins - BootstrapDialog
         AddCss("include/bootstrap/plugins/dialogs/css/bootstrap-dialog.min.css");
         AddJavaScript("include/bootstrap/plugins/dialogs/js/bootstrap-dialog.min.js");

         // Plugins - SlimScroll
         AddJavaScript("include/bootstrap/plugins/slimscroll/js/jquery.slimscroll.min.js");

         // Plugins - AutoSize (TextAera)
         AddJavaScript("include/bootstrap/plugins/autosize/js/jquery.autosize.min.js");

         // Plugins - InputLimiter (TextAera)
         AddJavaScript("include/bootstrap/plugins/imputlimiter/js/jquery.inputlimiter.1.3.1.min.js");

         // Plugins - TypeAhead (Autocompletar)
         AddJavaScript("include/bootstrap/plugins/typeahead/js/typeahead.bundle.min.js");
         AddCss("include/bootstrap/plugins/typeahead/css/typeahead.css");

         // Plantilla
         AddCss("templates/" + Workspace.TemplateID + "/bootstrap-theme.css");
         AddCss("templates/" + Workspace.TemplateID + "/custom-theme.css");
         */

         // Canales RSS   
         /*
         if (this.Settings.GetBoolean(CSWebsiteSettings.RSSChannelDocs))
         {
            CSWebsite.AddRss(container,
                             this.PropertyName + " - " + Cosmo.Cms.Documents.DocumentDAO.SERVICE_NAME,
                             Cosmo.Cms.Rss.CSRssHandler.URL_SERVICE + "?" + RssChannel.UrlParamChannel + "=" + (int)NativeObjects.Documents);
         }

         if (this.Settings.GetBoolean(CSWebsiteSettings.RSSChannelAds))
         {
            CSWebsite.AddRss(container,
                             this.PropertyName + " - " + Cosmo.Cms.Ads.CSAds.SERVICE_NAME,
                             Cosmo.Cms.Rss.CSRssHandler.URL_SERVICE + "?" + RssChannel.UrlParamChannel + "=" + (int)NativeObjects.Ads);
         }

         if (this.Settings.GetBoolean(CSWebsiteSettings.RSSChannelPictures))
         {
            CSWebsite.AddRss(container,
                             this.PropertyName + " - " + Cosmo.Cms.Pictures.CSPictures.SERVICE_NAME,
                             Cosmo.Cms.Rss.CSRssHandler.URL_SERVICE + "?" + RssChannel.UrlParamChannel + "=" + (int)NativeObjects.Pictures);
         }

         if (this.Settings.GetBoolean(CSWebsiteSettings.RSSChannelForum))
         {
            CSWebsite.AddRss(container,
                             this.PropertyName + " - " + Cosmo.Cms.Forums.CSForums.SERVICE_NAME,
                             Cosmo.Cms.Rss.CSRssHandler.URL_SERVICE + "?" + RssChannel.UrlParamChannel + "=" + (int)NativeObjects.Forums);
         }

         if (this.Settings.GetBoolean(CSWebsiteSettings.RSSChannelBooks))
         {
            CSWebsite.AddRss(container,
                             this.PropertyName + " - " + Cosmo.Cms.Books.CSBooks.SERVICE_NAME,
                             Cosmo.Cms.Rss.CSRssHandler.URL_SERVICE + "?" + RssChannel.UrlParamChannel + "=" + (int)NativeObjects.Books);
         }
         */
      }

      /// <summary>
      /// Agrega metainformación a la cabecera de una página.
      /// </summary>
      /// <param name="meta">Contenido del parámetro <c>http-equiv</c>.</param>
      /// <param name="content">Contenido del parámetro <c>content</c>.</param>
      private void AddMeta(string meta, string content)
      {
         HtmlMeta metaInfo = new HtmlMeta();
         metaInfo.HttpEquiv = meta;
         metaInfo.Content = content;

         this.Header.Controls.Add(metaInfo);
      }

      /// <summary>
      /// Agrega una script JavaScript.
      /// </summary>
      /// <param name="url">La URL del código JavaScript.</param>
      private void AddJavaScript(string url)
      {
         string script = string.Empty;

         url = url.Trim().ToLower();

         foreach (Cosmo.UI.Controls.Control control in this.Header.Controls)
         {
            if (control.GetType() == typeof(HtmlGenericControl))
            {
               if (((HtmlGenericControl)control).Attributes["src"] != null)
               {
                  script = ((HtmlGenericControl)control).Attributes["src"].Trim().ToLower();
                  if (url.Trim().ToLower().Equals(script))
                  {
                     return;
                  }
               }
            }
         }

         HtmlGenericControl js = new HtmlGenericControl("script");
         js.Attributes["type"] = "text/javascript";
         js.Attributes["src"] = url;
         this.Header.Controls.Add(js);
      }

      /// <summary>
      /// Agrega un fragmento de código JavaScript.
      /// </summary>
      /// <param name="code">Código JavaScript.</param>
      private void AddJavaScriptCode(string code)
      {
         // Agrega el código JavaScript
         HtmlGenericControl js = new HtmlGenericControl("script");
         js.Attributes["type"] = "text/javascript";
         js.InnerHtml = code;

         this.Header.Controls.Add(js);
      }

      /// <summary>
      /// Agrega una script JavaScript.
      /// </summary>
      /// <param name="url">La URL del código JavaScript.</param>
      private void RemoveJavaScript(string url)
      {
         string script = string.Empty;
         HtmlGenericControl ctrl = null;

         url = url.Trim().ToLower();

         foreach (Control control in this.Header.Controls)
         {
            if (control.GetType() == typeof(HtmlGenericControl))
            {
               if (((HtmlGenericControl)control).Attributes["src"] != null)
               {
                  script = ((HtmlGenericControl)control).Attributes["src"].Trim().ToLower();
                  if (url.Trim().ToLower().Equals(script))
                  {
                     ctrl = (HtmlGenericControl)control;
                     break;
                  }
               }
            }
         }

         if (ctrl != null)
         {
            this.Header.Controls.Remove(ctrl);
         }
      }

      /// <summary>
      /// Agrega una referencia a hoja de estilo CSS.
      /// </summary>
      /// <param name="url">La URL de la hoja de estilo.</param>
      private void AddCss(string url)
      {
         string script = string.Empty;

         url = url.Trim().ToLower();

         foreach (Control control in this.Header.Controls)
         {
            if (control.GetType() == typeof(HtmlLink))
            {
               script = ((HtmlLink)control).Href.Trim().ToLower();
               if (url.Trim().ToLower().Equals(script))
               { 
                  return; 
               }
            }
         }

         HtmlLink link = new HtmlLink();
         link.Href = url;
         link.Attributes.Add("rel", "stylesheet");
         link.Attributes.Add("type", "text/css");
         this.Header.Controls.Add(link);
      }

      /// <summary>
      /// Agrega una referencia a un feed RSS.
      /// </summary>
      /// <param name="title">Título visible del feed.</param>
      /// <param name="url">La URL del feed.</param>
      private void AddRssFeed(string title, string url)
      {
         HtmlLink link = new HtmlLink();
         link.Href = url;
         link.Attributes.Add("rel", "alternate");
         link.Attributes.Add("type", "application/rss+xml");
         link.Attributes.Add("title", title);

         this.Header.Controls.Add(link);
      }

      #endregion

   }
}
