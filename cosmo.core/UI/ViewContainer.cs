using Cosmo.Net;
using Cosmo.Security;
using Cosmo.Security.Auth;
using Cosmo.UI.Controls;
using Cosmo.UI.Modals;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;

namespace Cosmo.UI
{
   /// <summary>
   /// Implements a container that represents a web page.
   /// </summary>
   public abstract class ViewContainer : IHttpHandler, IRequiresSessionState
   {
      // Internal data declaration
      private List<ViewResource> _resources;
      private List<Script> _scripts;
      private ControlCollection _modalForms;
      private Url _url;
      private HttpContext _context;
      private DeviceDetector.DeviceType _device;
      private ControlCache _cache;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ViewContainer"/>.
      /// </summary>
      protected ViewContainer()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el workspace actual.
      /// </summary>
      public Workspace Workspace { get; set; }

      /// <summary>
      /// Return an instance of <see cref="ControlCache"/> that allow to store and retrieve controls in cache.
      /// </summary>
      public ControlCache Cache
      {
         get { return _cache; }
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="HttpRequest"/> que representa el contexto de llamada a la página.
      /// </summary>
      public HttpRequest Request
      {
         get { return _context.Request; }
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="HttpResponse"/> que representa la respuesta del servidor.
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
         get { return AuthenticationService.IsAuthenticated(_context.Session); }
      }

      /// <summary>
      /// Devuelve <c>true</c> si la llamada corresponde al envio de un formulario o <c>false</c> 
      /// en cualquier otro caso.
      /// </summary>
      public bool IsFormReceived
      {
         get { return Parameters.GetString(Cosmo.Workspace.PARAM_ACTION).Equals(FormControl.FORM_ACTION_SEND); }
      }

      /// <summary>
      /// Returns the DOM identifier of the form taht is being received.
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
      /// Devuelve el tipo de dispositivo usado para acceder a la página.
      /// </summary>
      public DeviceDetector.DeviceType DeviceType
      {
         get { return _device; }
      }

      /// <summary>
      /// Lista de formularios modales que se van a invocar desde la vista actual.
      /// </summary>
      public ControlCollection ModalForms
      {
         get
         {
            if (_modalForms == null) _modalForms = new ControlCollection(typeof(IModalForm));
            return _modalForms;
         }
         set { _modalForms = value; }
      }

      /// <summary>
      /// Lista de los componentes <see cref="Script"/> que se deben usar para representar correctamente el contenido.
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
      /// Lista de los recursos <see cref="ViewResource"/> (archivos CSS, JS, fuentes, etc.) que se deben usar para representar correctamente el contenido.
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

      public void AddControlToCache(Control control)
      {

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
         _cache = new ControlCache(_context.Cache);

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
         StartViewLifecycle();
      }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Inicia el ciclo de vida de la vista.
      /// </summary>
      internal abstract void StartViewLifecycle();

      /// <summary>
      /// Método invocado al iniciar la carga de la página, antes de procesar los datos recibidos.
      /// </summary>
      public abstract void InitPage();

      /// <summary>
      /// Método invocado al recibir datos de un formulario.
      /// </summary>
      /// <param name="receivedForm">Una instancia de <see cref="FormControl"/> que representa el formulario recibido. El formulario está actualizado con los datos recibidos.</param>
      public abstract void FormDataReceived(FormControl receivedForm);

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public abstract void FormDataLoad(string formDomID);

      /// <summary>
      /// Método invocado durante la carga de la página.
      /// </summary>
      public abstract void LoadPage();

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _modalForms = null;
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
      /// Lee los parámetros de llamada defenidos mediante <see cref="ViewParameter"/> i los coloca
      /// ens las correspondientes propiedades.
      /// </summary>
      private void ReadParameters()
      {
         foreach (ViewParameter param in this.GetType().GetCustomAttributes(typeof(ViewParameter), false))
         {
            // Obtiene la propiedad y sus características
            PropertyInfo property = this.GetType().GetProperty(param.PropertyName);

            // Recupera el valor segun el tipo de datos y lo asigna a cada propiedad
            if (property.DeclaringType == typeof(int) ||
                property.DeclaringType == typeof(Int16) ||
                property.DeclaringType == typeof(Int32) ||
                property.DeclaringType == typeof(Int64) ||
                property.DeclaringType == typeof(long))
            {
               property.SetValue(this, Parameters.GetInteger(param.ParameterName), null);
            }
            else if (property.DeclaringType == typeof(bool) ||
                     property.DeclaringType == typeof(Boolean))
            {
               property.SetValue(this, Parameters.GetBoolean(param.ParameterName), null);
            }
            else
            {
               property.SetValue(this, Parameters.GetString(param.ParameterName), null);
            }
         }
      }

      /// <summary>
      /// Comprueba las reglas de seguridad para acceder a la página actual.
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
                  Url url = new Url(Cosmo.Workspace.COSMO_URL_LOGIN);
                  url.AddParameter(Cosmo.Workspace.PARAM_LOGIN_REDIRECT, Request.RawUrl);

                  Redirect(url.ToString(true));
               }
            }
            else if (attr.GetType() == typeof(AuthorizationRequired))
            {
               if (!IsAuthenticated)
               {
                  Url url = new Url(Cosmo.Workspace.COSMO_URL_LOGIN);
                  url.AddParameter(Cosmo.Workspace.PARAM_LOGIN_REDIRECT, Request.RawUrl);

                  Redirect(url.ToString(true));
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
