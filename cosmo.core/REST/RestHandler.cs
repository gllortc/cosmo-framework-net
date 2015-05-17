using Cosmo.Net;
using Cosmo.Security;
using Cosmo.Security.Auth;
using Cosmo.UI.Scripting;
using System;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace Cosmo.REST
{
   /// <summary>
   /// Implementa una clase base para la creación de servicios API REST de Cosmo.
   /// </summary>
   public abstract class RestHandler : IHttpHandler, IRequiresSessionState
   {
      // Declaración de variables internas
      private Url _url;
      private Workspace _ws;
      private HttpContext _context;

      #region Enumerations

      /// <summary>
      /// Tipos de respuesta a llamadas a métodos REST (que no devuelvan valores).
      /// </summary>
      public enum RestCallResponse
      {
         /// <summary>Indica que la llamada a un método REST ha tenido éxito.</summary>
         Successful,
         /// <summary>Indica que la llamada a un método REST ha fallado.</summary>
         Fail
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el workspace actual.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
      }

      /// <summary>
      /// Devuelve la instancia que contiene el contexto de la petición.
      /// </summary>
      public HttpRequest Request
      {
         get { return _context.Request; }
      }

      /// <summary>
      /// Devuelve la instancia que contiene la sesión de usuario en el servidor.
      /// </summary>
      public HttpSessionState Session
      {
         get { return _context.Session; }
      }

      /// <summary>
      /// Devuelve la instancia que contiene el contexto de la respuesta al cliente.
      /// </summary>
      public HttpResponse Response
      {
         get { return _context.Response; }
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
         get { return true; }
      }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Método invocado al recibir una petición.
      /// </summary>
      /// <param name="command">Comando a ejecutar pasado mediante el parámetro definido mediante <see cref="Cosmo.Workspace.PARAM_COMMAND"/>.</param>
      public abstract void ServiceRequest(string command);

      #endregion

      #region Methods

      /// <summary>
      /// Invoke the requested REST method.
      /// </summary>
      /// <param name="context">Call context.</param>
      public void InvokeRestMethod(HttpContext context)
      {
         int paramsSet = 0;
         int paramsCount =0;
         object[] urlParams;
         object[] callParams;
         ParameterInfo[] methodParams;

         // Get all methods
         foreach (MethodInfo method in this.GetType().GetMethods())
         {
            // Check if method is a rest method
            foreach (object attr in method.GetCustomAttributes(typeof(RestMethod), false))
            {
               if (attr.GetType() == typeof(RestMethod))
               {
                  // Check if the call is for the current class method
                  if (((RestMethod)attr).CommandName == Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND))
                  {
                     CheckSecurityConstrains(method);

                     urlParams = method.GetCustomAttributes(typeof(RestMethodParameter), false);
                     callParams = new object[urlParams.Length];

                     // Read method attributes
                     paramsSet = 0;
                     foreach (object urlParam in urlParams)
                     {
                        methodParams = method.GetParameters();
                        paramsCount = methodParams.Length;
                        foreach (ParameterInfo methodParam in methodParams)
                        {
                           if (methodParam.Name == ((RestMethodParameter)urlParam).MethodParameterName)
                           {
                              if (methodParam.ParameterType == typeof(String))
                              {
                                 callParams[methodParam.Position] = Parameters.GetString(((RestMethodParameter)urlParam).UrlParameterName);
                                 paramsSet++;

                                 break;
                              }
                              else if (methodParam.ParameterType == typeof(Int16) ||
                                       methodParam.ParameterType == typeof(Int32) ||
                                       methodParam.ParameterType == typeof(Int64))
                              {
                                 callParams[methodParam.Position] = Parameters.GetInteger(((RestMethodParameter)urlParam).UrlParameterName);
                                 paramsSet++;

                                 break;
                              }
                              else if (methodParam.ParameterType == typeof(Boolean))
                              {
                                 callParams[methodParam.Position] = Parameters.GetBoolean(((RestMethodParameter)urlParam).UrlParameterName);
                                 paramsSet++;

                                 break;
                              }
                           }
                        }
                     }

                     if (paramsCount == paramsSet)
                     {
                        method.Invoke(this, callParams);
                     }
                     else
                     {
                        SendResponse(new AjaxResponse(0, "REST method call failed: bad parameters."));
                     }

                     return;
                  }
               }
            }
         }
      }

      /// <summary>
      /// Evento que se lanza al recibir una petición.
      /// </summary>
      /// <param name="context">Contexto del servidor en el momento de la llamada.</param>
      public void ProcessRequest(HttpContext context)
      {
         try
         {
            _context = context;

            // Inicializa el workspace
            _ws = Workspace.GetWorkspace(context);

            // Inicializaciones
            _url = new Url(context.Request);

            InvokeRestMethod(context);

            // Invoca el método del servicio
            ServiceRequest(_url.GetString(Cosmo.Workspace.PARAM_COMMAND));
         }
         catch (Exception ex)
         {
            Exception err = ex;
         }
      }

      /// <summary>
      /// Envia una respuesta al cliente.
      /// </summary>
      /// <param name="response">Una instancia de <see cref="AjaxResponse"/> que describe si la 
      /// acción ha tenido éxito o no.</param>
      public void SendResponse(AjaxResponse response)
      {
         Response.ContentType = "application/json";
         Response.Charset = "UTF-8";

         Response.Write(response.ToJSON());
      }

      /*
      /// <summary>
      /// Permite mandar al cliente un token (JSON) en respuesta a la llamada a un método.
      /// </summary>
      /// <param name="response">Tipo de respuesta a enviar.</param>
      public void SendJSONResponse(RestCallResponse response)
      {
         SendJSONResponse(response, "ERROR");
      }

      /// <summary>
      /// Permite mandar al cliente un token (JSON) en respuesta a la llamada a un método.
      /// </summary>
      /// <param name="response">Tipo de respuesta a enviar.</param>
      /// <param name="errorMessage">mensaje de error a mostrar al usuario.</param>
      public void SendJSONResponse(RestCallResponse response, string errorMessage)
      {
         Response.ContentType = "application/json";

         switch (response)
         {
            case RestCallResponse.Successful:
               Response.Write(new AjaxResponse().ToJSON());
               break;
            case RestCallResponse.Fail:
               Response.Write(new AjaxResponse(0, errorMessage).ToJSON());
               break;
         }
      }
      */
      #endregion

      #region Private Members

      /// <summary>
      /// Check for security attributes
      /// </summary>
      private void CheckSecurityConstrains(MethodInfo method)
      {
         foreach (object attr in method.GetCustomAttributes(typeof(RestMethod), false))
         {
            if (attr.GetType() == typeof(AuthenticationRequired))
            {
               if (!Workspace.CurrentUser.IsAuthenticated)
               {
                  throw new SecurityException("Authentication required");
               }
            }
            else if (attr.GetType() == typeof(AuthorizationRequired))
            {
               AuthorizationRequired authorization = (AuthorizationRequired)attr;
               if (!Workspace.CurrentUser.CheckAuthorization(authorization.RequiredRole))
               {
                  throw new SecurityException("Not authorized");
               }
            }
         }
      }

      #endregion

   }
}
