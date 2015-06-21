using Cosmo.Net;
using Cosmo.Security;
using Cosmo.Security.Auth;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Cosmo.UI
{
   /// <summary>
   /// Implementa un fragmento de página que se puede refrescar mediante AJAX.
   /// Indicado para listados y otros elementos dinámicos.
   /// </summary>
   [Obsolete]
   public abstract class CosmoTemplate : View, IHttpHandler, IRequiresSessionState
   {
      private string _domId;
      private Url _url;
      private ControlCollection _content;
      private HttpContext _context;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="CosmoTemplate"/>.
      /// </summary>
      protected CosmoTemplate()
      {
         Initialize();
      }

      #endregion

      /// <summary>
      /// Devuelve o establece el identificador del elemento en la estructura DOM de la página web.
      /// </summary>
      public string DomID
      {
         get { return _domId; }
         set { _domId = value; }
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="HttpRequest"/> que representa el contexto de llamada a la página.
      /// </summary>
      public HttpRequest Request
      {
         get { return _context.Request; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la cabecera.
      /// </summary>
      public ControlCollection Content
      {
         get { return _content; }
         set { _content = value; }
      }

      /*
      /// <summary>
      /// Devuelve todos los scripts que se usan en la vista.
      /// </summary>
      public override ControlCollection Scripts
      {
         get { return GetPageScripts(_content); }
      }
       * */

      /// <summary>
      /// Indica si existe un usuario autenticado.
      /// </summary>
      /// <returns><c>true</c> si existe una sesión autenticada o <c>false</c> en cualquier otro caso.</returns>
      public bool IsAuthenticated
      {
         get { return SecurityService.IsAuthenticated(_context.Session); }
      }

      /// <summary>
      /// Devuelve la colección de parámetros de llamada (o de un formulario) que ha recibido la página.
      /// </summary>
      public Url Parameters
      {
         get { return _url; }
      }

      /// <summary>
      /// Indica al servidor web si la instancia es rehusable.
      /// </summary>
      public bool IsReusable
      {
         get { return false; }
      }

      /// <summary>
      /// Método invocado durante la carga de la página.
      /// </summary>
      public abstract void LoadTemplate();

      /// <summary>
      /// Evento que se lanza al recibir una petición.
      /// Equivale al ciclo de vida de la página.
      /// </summary>
      /// <param name="context">Contexto del servidor en el momento de la llamada.</param>
      public void ProcessRequest(HttpContext context)
      {
         try
         {
            _context = context;

            // Inicializa el workspace
            Workspace = Workspace.GetWorkspace(context);

            // Inicializaciones
            _url = new Url(context.Request);

            // Comprobaciones de acceso a la página
            CheckSecurityConstrains();

            // Carga la página
            LoadTemplate();
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }

         // Renderiza la página
         context.Response.ContentType = "text/html";
         context.Response.Write(Workspace.UIService.Render(_content));
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _domId = string.Empty;
         _content = new ControlCollection();
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
                  throw new AuthenticationException("ACCESO DENEGADO: Se precisa sesión de usuario para visualizar el contenido solicitado.");
               }
            }
            else if (attr.GetType() == typeof(AuthorizationRequired))
            {
               if (!IsAuthenticated)
               {
                  throw new AuthenticationException("ACCESO DENEGADO: Se precisa sesión de usuario para visualizar el contenido solicitado.");
               }

               if (!Workspace.CurrentUser.CheckAuthorization(((AuthorizationRequired)attr).RequiredRole))
               {
                  throw new AuthenticationException("ACCESO DENEGADO: Su cuenta de usuario no tiene suficientes permisos para acceder a esta página o recurso.");
               }
            }
         }
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
         _content.Clear();

         // Genera el mensaje de error
         CalloutControl callout = new CalloutControl(this);
         callout.Type = ComponentColorScheme.Error;
         callout.Title = title;
         callout.Text = description;

         // Agrega el mensaje al contenido
         _content.Add(callout);
      }

      /// <summary>
      /// Muestra un mensaje de error.
      /// </summary>
      /// <param name="exception">Excepción a mostrar.</param>
      public void ShowError(Exception exception)
      {
         StringBuilder xhtml = new StringBuilder();

         // Limpia el contenido de la zona principal dónde se va a mostrar el mensaje
         _content.Clear();

         // Agrega el mensaje al contenido
         _content.Add(new ErrorControl(this, exception));
      }
   }
}
