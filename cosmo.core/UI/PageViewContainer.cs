using Cosmo.UI.Controls;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cosmo.UI
{
   /// <summary>
   /// Implementa una página de Cosmo que se genera si usar la capa System.Web.UI.
   /// </summary>
   public abstract class PageViewContainer : ViewContainer
   {
      /// <summary>Nombre del enlace interno de inicio de página</summary>
      public const string LINK_TOP_PAGE = "page-top";

      // Declaracción de variables internas
      private LayoutContainerControl _layout;
      private List<ModalViewContainer> _modals;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="PageViewContainer"/>.
      /// </summary>
      protected PageViewContainer()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve la estructura de página (contenedor de controles).
      /// </summary>
      public LayoutContainerControl Layout
      {
         get { return _layout; }
      }

      /// <summary>
      /// Devuelve o establece el título de la página.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Devuelve o establece la descripción del contenido de la página.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Devuelve o establece las palabras clave que definene el contenido de la página.
      /// </summary>
      public string Keywords { get; set; }

      /// <summary>
      /// Indica si se debe mostrar el fondo del <em>layout</em> descolorido (o en un color alternativo dependiendo de la plantilla y/o el renderizador).
      /// Esto permite, por ejemplo, mostrar páginas con un solo control como <em>Login</em>, <em>Registro</em>, etc.
      /// </summary>
      public bool FadeBackground
      {
         get { return _layout.FadeBackground; }
         set { _layout.FadeBackground = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la cabecera.
      /// </summary>
      public ControlCollection HeaderContent
      {
         get { return _layout.Header; }
         set { _layout.Header = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene el pie.
      /// </summary>
      public ControlCollection FooterContent
      {
         get { return _layout.Footer; }
         set { _layout.Footer = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la columna izquierda.
      /// </summary>
      public ControlCollection LeftContent
      {
         get { return _layout.LeftContent; }
         set { _layout.LeftContent = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la zona de contenidos de la página.
      /// </summary>
      public ControlCollection MainContent
      {
         get { return _layout.MainContent; }
         set { _layout.MainContent = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la columna derecha.
      /// </summary>
      public ControlCollection RightContent
      {
         get { return _layout.RightContent; }
         set { _layout.RightContent = value; }
      }

      /// <summary>
      /// Devuelve o establece el identificador del elemento de menú que debe mostrarse activo.
      /// </summary>
      public string ActiveMenuId { get; set; }

      /// <summary>
      /// Lista de ventanas modales que se van a invocar desde la vista actual.
      /// </summary>
      public List<ModalViewContainer> Modals
      {
         get
         {
            if (_modals == null) _modals = new List<ModalViewContainer>();
            return _modals;
         }
         set { _modals = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Inicia el ciclo de vida de la vista.
      /// </summary>
      internal override void StartViewLifecycle()
      {
         string receivedFormID = string.Empty;
         var watch = Stopwatch.StartNew();

         try
         {
            // Initialze page load
            InitPage();

            // Process form data
            foreach (FormControl form in _layout.GetControlsByType(typeof(FormControl)))
            {
               if (IsFormReceived && form.DomID.Equals(FormReceivedDomID))
               {
                  form.ProcessForm(Parameters);
                  receivedFormID = form.DomID;

                  // If data is valid, raise FormDataReceived() event
                  if (form.IsValid == FormControl.ValidationStatus.ValidData)
                  {
                     FormDataReceived(form);
                  }
               }

               // Lanza el evento FormDataLoad()
               if (form.IsValid != FormControl.ValidationStatus.InvalidData)
               {
                  FormDataLoad(form.DomID);
               }
            }

            // Finish page load
            LoadPage();
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }

         // Renderiza la página
         Response.ContentType = "text/html";
         Response.Write(Workspace.UIService.RenderPage(this, receivedFormID));

         watch.Stop();
         Response.Write("<!-- Page created in " + watch.ElapsedMilliseconds + "mS -->");
      }

      /// <summary>
      /// Muestra un mensaje de error.
      /// </summary>
      /// <param name="title">Título del error.</param>
      /// <param name="description">Descripción del error.</param>
      public void ShowError(string title, string description)
      {
         StringBuilder xhtml = new StringBuilder();

         // Limpia el contenido de la zona principal dónde se va a mostrar el mensaje
         _layout.MainContent.Clear();
         _layout.RightContent.Clear();

         // Genera el mensaje de error
         CalloutControl callout = new CalloutControl(this);
         callout.Type = ComponentColorScheme.Error;
         callout.Title = title;
         callout.Text = description;

         // Agrega el mensaje al contenido
         _layout.MainContent.Add(callout);
      }

      /// <summary>
      /// Muestra un mensaje de error.
      /// </summary>
      /// <param name="exception">Excepción a mostrar.</param>
      public void ShowError(Exception exception)
      {
         StringBuilder xhtml = new StringBuilder();

         // Limpia el contenido de la zona principal dónde se va a mostrar el mensaje
         _layout.MainContent.Clear();
         _layout.RightContent.Clear();

         // Agrega el mensaje al contenido
         _layout.MainContent.Add(new ErrorControl(this, exception));
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _layout = new LayoutContainerControl(this);
         _modals = null;

         this.Title = string.Empty;
         this.Description = string.Empty;
         this.Keywords = string.Empty;
         this.ActiveMenuId = string.Empty;
      }

      #endregion

      #region Disabled Code

      /*
      /// <summary>
      /// Devuelve todos los scripts que se usan en la página.
      /// </summary>
      public override ControlCollection Scripts
      {
         get 
         {
            ControlCollection scripts = new ControlCollection();
            scripts.Add(GetPageScripts(Layout.Header));
            scripts.Add(GetPageScripts(Layout.Footer));
            scripts.Add(GetPageScripts(Layout.LeftContent));
            scripts.Add(GetPageScripts(Layout.MainContent));
            scripts.Add(GetPageScripts(Layout.RightContent));
            scripts.Add(GetPageScripts(ModalForms));

            return scripts;
         }
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
         if (!AuthenticationService.IsAuthenticated(_context.Session))
         {
            Url url = new Url(Cosmo.Workspace.COSMO_URL_LOGIN);
            url.AddParameter(Cosmo.Workspace.PARAM_LOGIN_REDIRECT, destinationUrl);

            _context.Result.Redirect(url.ToString(true));
            _context.Result.End();
         }
      }

      /// <summary>
      /// Comprueba la autorización para la sesión autenticada actual.
      /// De no existir sesión autenticada, devolverá <em>false</em>.
      /// </summary>
      public bool CheckAuthorization2(params string[] roles)
      {
         // Comprueba si 
         foreach (string role in roles)
         {
            if (CheckAuthorization(role))
            {
               return true;
            }
         }

         return false;
      }
      */

      #endregion

   }
}
