using Cosmo.Net;
using Cosmo.Security;
using Cosmo.Security.Auth;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace Cosmo.UI
{
   /// <summary>
   /// Implements a container that represents a web page.
   /// </summary>
   public abstract class View : IHttpHandler, IRequiresSessionState
   {
      // Internal data declarations
      private List<ViewResource> _resources;
      private List<Script> _scripts;
      private Url _url;
      private HttpContext _context;
      private DeviceDetector.DeviceType _device;
      private ControlCache _cache;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="View"/>.
      /// </summary>
      protected View(string domId)
      {
         Initialize();

         this.DomID = domId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the current workspace.
      /// </summary>
      public Workspace Workspace { get; set; }

      /// <summary>
      /// Gets or sets the DOM unique identifier.
      /// </summary>
      /// <remarks>
      /// This property have a protected <c>setter</c> because every modal view must have a 
      /// constant DOM unique identifier. You can set this property only in a implementations
      /// of the abstract class <see cref="ModalView"/>.
      /// </remarks>
      public string DomID { get; protected set; }

      /// <summary>
      /// Return an instance of <see cref="ControlCache"/> that allow to store and retrieve controls in cache.
      /// </summary>
      public ControlCache Cache
      {
         get { return _cache; }
      }

      /// <summary>
      /// Gets a new instance of <see cref="HttpRequest"/> que representa el contexto de llamada a la página.
      /// </summary>
      public HttpRequest Request
      {
         get { return _context.Request; }
      }

      /// <summary>
      /// Gets a new instance of <see cref="HttpResponse"/> que representa la respuesta del servidor.
      /// </summary>
      public HttpResponse Response
      {
         get { return _context.Response; }
      }

      /// <summary>
      /// Devuelve la colección de parámetros de llamada (o de un formulario) que ha recibido la página.
      /// </summary>
      public Url Parameters
      {
         get { return _url; }
      }

      /// <summary>
      /// Indica si existe un usuario autenticado.
      /// </summary>
      /// <returns><c>true</c> si existe una sesión autenticada o <c>false</c> en cualquier otro caso.</returns>
      public bool IsAuthenticated
      {
         get { return SecurityService.IsAuthenticated(_context.Session); }
      }

      /// <summary>
      /// Gets <c>true</c> if request is a form sending or <c>false</c> in all other cases.
      /// </summary>
      public bool IsFormReceived
      {
         get { return Parameters.GetString(Cosmo.Workspace.PARAM_ACTION).Equals(FormControl.FORM_ACTION_SEND); }
      }

      /// <summary>
      /// Gets the DOM identifier of the form taht is being received.
      /// </summary>
      public string FormReceivedDomID
      {
         get 
         {
            if (!IsFormReceived)
            {
               return string.Empty;
            }
            else
            {
               return Parameters.GetString(FormControl.FORM_ID);
            }
         }
      }

      /// <summary>
      /// Gets the device type used to reques the view.
      /// </summary>
      public DeviceDetector.DeviceType DeviceType
      {
         get { return _device; }
      }

      /// <summary>
      /// Gets or sets the list of scripts used to construct the view.
      /// </summary>
      public List<Script> Scripts
      {
         get
         {
            if (_scripts == null) _scripts = new List<Script>();
            return _scripts;
         }
         set { _scripts = value; }
      }

      /// <summary>
      /// Gets or sets the list of web resources used to create the view.
      /// </summary>
      public List<ViewResource> Resources
      {
         get
         {
            if (_resources == null) _resources = new List<ViewResource>();
            return _resources;
         }
         set { _resources = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Redirige al cliente hacia otra URL.
      /// </summary>
      /// <param name="destinationUrl">La URL de destino.</param>
      public void Redirect(string destinationUrl)
      {
         _context.Response.Redirect(destinationUrl, true);
      }

      /// <summary>
      /// Add a control to cache.
      /// </summary>
      /// <param name="control">Control to add to cache.</param>
      public void AddControlToCache(Control control)
      {
         throw new NotImplementedException();
      }

      #endregion

      #region IHttpHandler Implementation

      /// <summary>
      /// Indica al servidor web si la instancia es rehusable.
      /// </summary>
      public bool IsReusable
      {
         get { return false; }
      }

      /// <summary>
      /// Evento que se lanza al recibir una petición.
      /// Equivale al ciclo de vida de la página.
      /// </summary>
      /// <param name="context">Contexto del servidor en el momento de la llamada.</param>
      public void ProcessRequest(HttpContext context)
      {
         // Almacena el contexto para sus diversos usos durante el ciclo de vida de la vista
         _context = context;
         _cache = new ControlCache(Workspace, _context.Cache);

         // Inicializa el workspace
         Workspace = Workspace.GetWorkspace(context);

         // Obtiene los parámetros de llamada
         _url = new Url(context.Request);

         // Comprobaciones de acceso a la página
         CheckSecurityConstrains();

         // Recupera los parámetros mapeados
         ReadParameters();

         // Obtiene el dispositivo de acceso
         _device = DeviceDetector.DetectDeviceType(_context.Request);

         // Inicia el ciclo de vida de la página
         StartViewLifecycle(this.ControlContainer);
      }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Gets the control container used to store the view controls.
      /// </summary>
      public abstract IControlContainer ControlContainer { get; }

      /// <summary>
      /// Show a error message.
      /// </summary>
      /// <param name="title">Error title.</param>
      /// <param name="description">Error description.</param>
      public abstract void ShowError(string title, string description);

      /// <summary>
      /// Show a error message.
      /// </summary>
      /// <param name="exception">Exception to be shown.</param>
      public abstract void ShowError(Exception exception);

      #endregion

      #region Virtual Members

      /// <summary>
      /// Método invocado al iniciar la carga de la página, antes de procesar los datos recibidos.
      /// </summary>
      public virtual void InitPage()
      {
         // Nothing to do here
      }

      /// <summary>
      /// Método invocado al recibir datos de un formulario.
      /// </summary>
      /// <param name="receivedForm">Una instancia de <see cref="FormControl"/> que representa el formulario recibido. El formulario está actualizado con los datos recibidos.</param>
      public virtual void FormDataReceived(FormControl receivedForm)
      {
         // Nothing to do here
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public virtual void FormDataLoad(string formDomID)
      {
         // Nothing to do here
      }

      /// <summary>
      /// Método invocado durante la carga de la página.
      /// </summary>
      public virtual void LoadPage()
      {
         // Nothing to do here
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Gets the name of view.
      /// </summary>
      /// <remarks>
      /// This name is useful to use in URL creation.
      /// </remarks>
      public static string ViewName
      {
         get { return MethodBase.GetCurrentMethod().DeclaringType.Name; }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _resources = null;
         _scripts = null;
         _url = null;
         _device = DeviceDetector.DeviceType.Desktop;
         _cache = null;

         this.Workspace = null;
         this.Resources = new List<ViewResource>();
         this.Scripts = new List<Script>();
      }

      /// <summary>
      /// Execute the view lifecycle.
      /// </summary>
      private void StartViewLifecycle(IControlContainer controlContainer)
      {
         string receivedFormID = string.Empty;
         var watch = Stopwatch.StartNew();

         try
         {
            // Raises the InitPage() event
            InitPage();

            // Process form data for each form in view
            foreach (FormControl form in controlContainer.GetControlsByType(typeof(FormControl)))
            {
               if (IsFormReceived && form.DomID.Equals(FormReceivedDomID))
               {
                  form.ProcessForm(Parameters);
                  receivedFormID = form.DomID;

                  // If form data is valid, raises the FormDataReceived() event
                  if (form.IsValid == FormControl.ValidationStatus.ValidData)
                  {
                     FormDataReceived(form);
                  }
               }

               // If form data is valid, raises the FormDataLoad() event
               if (form.IsValid != FormControl.ValidationStatus.InvalidData)
               {
                  FormDataLoad(form.DomID);
               }
            }

            // Raises the LoadPage() event
            LoadPage();
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }

         // Renderizes the view
         Response.ContentType = "text/html";
         Response.Write(Workspace.UIService.RenderView(this, receivedFormID));

         // Compute total time and ads a comment in HTML code
         watch.Stop();
         Response.Write("<!-- Created in " + watch.ElapsedMilliseconds + "mS -->");
      }

      /// <summary>
      /// Lee los parámetros de llamada defenidos mediante <see cref="ViewParameter"/> i los coloca
      /// ens las correspondientes propiedades.
      /// </summary>
      private void ReadParameters()
      {
         foreach (ViewParameter param in this.GetType().GetCustomAttributes(typeof(ViewParameter), false))
         {
            // Obtiene la propiedad y sus características
            PropertyInfo property = this.GetType().GetProperty(param.PropertyName);
            if (property != null)
            {
               // Recupera el valor segun el tipo de datos y lo asigna a cada propiedad
               if (property.PropertyType == typeof(int) ||
                   property.PropertyType == typeof(Int16) ||
                   property.PropertyType == typeof(Int32) ||
                   property.PropertyType == typeof(Int64) ||
                   property.PropertyType == typeof(long))
               {
                  property.SetValue(this, Parameters.GetInteger(param.ParameterName), null);
               }
               else if (property.PropertyType == typeof(bool) ||
                        property.PropertyType == typeof(Boolean))
               {
                  property.SetValue(this, Parameters.GetBoolean(param.ParameterName), null);
               }
               else if (property.PropertyType == typeof(DateTime))
               {
                  // TODO: Make dates transferible as a parameter
               }
               else
               {
                  property.SetValue(this, Parameters.GetString(param.ParameterName), null);
               }
            }
         }
      }

      /// <summary>
      /// Check the security rules for the current user to access to current view.
      /// </summary>
      private void CheckSecurityConstrains()
      {
         System.Reflection.MemberInfo info = this.GetType();
         object[] attributes = info.GetCustomAttributes(true);

         foreach (object attr in attributes)
         {
            if (attr.GetType() == typeof(AuthenticationRequired))
            {
               if (!IsAuthenticated)
               {
                  Redirect(Workspace.SecurityService.GetLoginUrl(Request.RawUrl));
               }
            }
            else if (attr.GetType() == typeof(AuthorizationRequired))
            {
               if (!IsAuthenticated)
               {
                  Redirect(Workspace.SecurityService.GetLoginUrl(Request.RawUrl));
               }

               if (!Workspace.CurrentUser.CheckAuthorization(((AuthorizationRequired)attr).RequiredRole))
               {
                  throw new AuthenticationException("ACCESO DENEGADO: Su cuenta de usuario no tiene suficientes permisos para acceder a esta página o recurso.");
               }
            }
         }
      }

      #endregion

      #region Disabled Code

      /*
      /// <summary>
      /// Obtiene todos los componentes <see cref="Script"/> en una colección de controles UI.
      /// </summary>
      /// <param name="controls">Colección de controles para los que se desea obtener los scripts.</param>
      /// <returns>Todos los scripts usados en una sola colección.</returns>
      public ControlCollection GetPageScripts(ControlCollection controls)
      {
         ControlCollection scripts = new ControlCollection();

         foreach (IControl control in controls)
         {
            scripts.Add(control.Scripts);

            if (typeof(IControlSingleContainer).IsAssignableFrom(control.GetType()))
            {
               scripts.Add(GetPageScripts(((IControlSingleContainer)control).Content));
            }
         }

         return scripts;
      }
      */

      #endregion

   }
}
