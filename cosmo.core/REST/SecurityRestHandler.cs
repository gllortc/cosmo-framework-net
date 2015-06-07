using Cosmo.Diagnostics;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.Security.Auth;
using Cosmo.Services;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace Cosmo.REST
{

   /// <summary>
   /// Implementa un handler para la gestión de las acciones sobre usuarios
   /// </summary>
   /// <remarks>
   /// Para usar cualquier método de este Handler debe existir sesión de usuario inicuiada.
   /// </remarks>
   public class SecurityRestHandler : RestHandler
   {
      /// <summary>parámetro para pasar consultas.</summary>
      private const string PARAM_QUERY = "q";
      /// <summary>Parámetro que contiene la clave de verificación de una cuenta de correo.</summary>
      private const string PARAM_VERIFY_KEY = "data";
      /// <summary>Parámetro que contiene el correo electrónico.</summary>
      private const string PARAM_PWD_REC_MAIL_KEY = "eml";

      #region RestHandler Implementation

      /// <summary>
      /// Evento que se invoca al recibir una petición.
      /// </summary>
      /// <param name="command">Comando a ejecutar pasado mediante el parámetro definido mediante <see cref="Cosmo.Workspace.PARAM_COMMAND"/>.</param>
      public override void ServiceRequest(string command)
      {

      }

      #endregion

      #region Command: User Data

      /// <summary>Comando para el handler de usuarios: Devuelve los datos públicos de un usuario.</summary>
      private const string COMMAND_USER_DATA = "ud";

      /// <summary>
      /// Generate a valid URL to call the <c>SecurityRestHandler.UserSearch()</c> method.
      /// </summary>
      /// <param name="userId">User identifier.</param>
      /// <returns>A string containing the URL to call the method.</returns>
      public static Url GetUserDataUrl(int userId)
      {
         return new Url(SecurityRestHandler.ServiceUrl).
            AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_USER_DATA).
            AddParameter(Cosmo.Workspace.PARAM_USER_ID, userId);
      }

      /// <summary>
      /// Muestra la ficha informativa de un usuario.
      /// </summary>
      [AuthenticationRequired]
      [RestMethod(CommandName = COMMAND_USER_DATA,
                  DataType = RestMethod.RestMethodReturnType.JsonDataResponse)]
      [RestMethodParameter(DataType = typeof(Int32),
                           MethodParameterName = "userId",
                           UrlParameterName = Cosmo.Workspace.PARAM_USER_ID)]
      public void UserData(int userId)
      {
         // Inicializaciones
         JavaScriptSerializer json = new JavaScriptSerializer();
         StringBuilder xhtml = new StringBuilder();

         try
         {
            // Genera el código XML
            User user = Workspace.AuthenticationService.GetUser(userId);
            
            // Obtiene el pais del usuario
            CountryDAO cdao = new CountryDAO(Workspace);

            // Envia confirmación al cliente
            var data = new
            {
               response = "ok",
               id = user.ID,
               login = user.Login,
               name = user.GetDisplayName(),
               description = user.Description,
               city = user.City,
               country = cdao.GetCountryName(user.CountryID)
            };

            SendResponse(new AjaxResponse(data));
         }
         catch
         {
            SendResponse(new AjaxResponse(0, "Ha sido imposible recuperar los datos de este usuario. Es posible que esta cuenta de usuario haya sido dado de baja."));
         }
      }

      #endregion

      #region Command: User Search

      /// <summary>Comando para el handler de usuarios: Comando para búsquedas.</summary>
      private const string COMMAND_USER_SEARCH = "us";

      /// <summary>
      /// Generate a valid URL to call the <c>SecurityRestHandler.UserSearch()</c> method.
      /// </summary>
      /// <param name="queryText">Text to search.</param>
      /// <returns>A string containing the URL to call the method.</returns>
      public static Url GetUserSearchUrl(string queryText)
      {
         return new Url(SecurityRestHandler.ServiceUrl).
            AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_USER_SEARCH).
            AddParameter(PARAM_QUERY, queryText);
      }

      /// <summary>
      /// Busca usuarios por <em>login</em>.
      /// </summary>
      [AuthenticationRequired]
      [RestMethod(CommandName = COMMAND_USER_SEARCH,
                  DataType = RestMethod.RestMethodReturnType.JsonDataResponse)]
      [RestMethodParameter(DataType = typeof(String),
                           MethodParameterName = "queryText",
                           UrlParameterName = PARAM_QUERY)]
      public void UserSearch(string queryText)
      {
         List<KeyValue> values = new List<KeyValue>();
         JavaScriptSerializer json = new JavaScriptSerializer();

         // Verifica los resultados
         if (string.IsNullOrWhiteSpace(queryText) || (queryText.Trim().Length < 4))
         {
            SendResponse(new AjaxResponse(0, "No se ha especificado ninguna búsqueda!"));
            return;
         }

         try
         {
            // Obtiene los resultados de la búsqueda
            List<User> users = Workspace.AuthenticationService.Find(queryText, string.Empty, 0);

            // Convierte los resultados en objetos serializables JSON
            foreach (User user in users)
            {
               values.Add(new KeyValue(user.Name + " (" + user.Login + ")", user.ID.ToString()));
            }

            // Serializa los resultados
            SendResponse(new AjaxResponse(values));
         }
         catch
         {
            SendResponse(new AjaxResponse(0, "ERROR recuperando usuarios!"));
         }
      }

      #endregion

      #region Command: User Authentication 

      /// <summary>Comando para el handler de usuarios: Comando para autenticar usuarios.</summary>
      public const string COMMAND_USER_AUTHENTICATION = "ua";

      /// <summary>
      /// Generate a valid URL to call the <c>SecurityRestHandler.UserAuthentication()</c> method.
      /// </summary>
      /// <param name="urlLoginRedirect"></param>
      /// <param name="login"></param>
      /// <param name="password"></param>
      /// <returns>A string containing the URL to call the method.</returns>
      public static Url GetUserAuthenticationUrl(string urlLoginRedirect, string login, string password)
      {
         return new Url(SecurityRestHandler.ServiceUrl).
            AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_USER_AUTHENTICATION).
            AddParameter(Workspace.PARAM_LOGIN_REDIRECT, urlLoginRedirect).
            AddParameter("txtLogin", login).
            AddParameter("txtPwd", password);
      }

      /// <summary>
      /// Authenticate a user with basic credentials.
      /// </summary>
      [RestMethod(CommandName = COMMAND_USER_AUTHENTICATION,
                  DataType = RestMethod.RestMethodReturnType.StandardRestResponse)]
      [RestMethodParameter(DataType = typeof(String),
                           MethodParameterName = "urlLoginRedirect",
                           UrlParameterName = Workspace.PARAM_LOGIN_REDIRECT)]
      [RestMethodParameter(DataType = typeof(String),
                           MethodParameterName = "login",
                           UrlParameterName = "txtLogin")]
      [RestMethodParameter(DataType = typeof(String),
                           MethodParameterName = "password",
                           UrlParameterName = "txtPwd")]
      public void UserAuthentication(string urlLoginRedirect, string login, string password)
      {
         AjaxResponse response = null;

         // Se asegura que el usuario tenga una sesión iniciada
         if (Workspace.CurrentUser.IsAuthenticated)
         {
            response = new AjaxResponse();
            response.ToURL = urlLoginRedirect;
            SendResponse(response);
         }
         else
         {
            try
            {
               Workspace.AuthenticationService.Autenticate(login, password);

               response = new AjaxResponse();
               response.ToURL = urlLoginRedirect;
               SendResponse(response);
            }
            catch (UserDisabledException)
            {
               response = new AjaxResponse(1001, "Disabled");
               response.ToURL = urlLoginRedirect;
               SendResponse(response);
            }
            catch (UserNotVerifiedException)
            {
               response = new AjaxResponse(1002, "Not verified");
               response.ToURL = urlLoginRedirect;
               SendResponse(response);
            }
            catch (AuthenticationException)
            {
               response = new AjaxResponse(1002, "Auth failed");
               response.ToURL = urlLoginRedirect;
               SendResponse(response);
            }
         }
      }

      #endregion

      #region Command: User Authentication Log off

      /// <summary>Comando: Comando para terminar con la sesión de usuario.</summary>
      private const string COMMAND_USER_LOGOFF = "logoff";

      /// <summary>
      /// Genera una URL válida para cerrar la sesión de usuario.
      /// </summary>
      /// <param name="urlLoginRedirect">La URL dónde se debe redirigir al usuario después de cerrar la sesión.</param>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static Url GetUserLogOffUrl(string urlLoginRedirect)
      {
         return new Url(SecurityRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, SecurityRestHandler.COMMAND_USER_LOGOFF)
            .AddParameter(Cosmo.Workspace.PARAM_LOGIN_REDIRECT, urlLoginRedirect);
      }

      /// <summary>
      /// Genera una URL válida para cerrar la sesión de usuario.
      /// </summary>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static Url GetUserLogOffUrl()
      {
         return new Url(SecurityRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, SecurityRestHandler.COMMAND_USER_LOGOFF);
      }

      /// <summary>
      /// Close de current autheticated session and redirects usert to a specified URL.
      /// </summary>
      [RestMethod(CommandName = COMMAND_USER_LOGOFF,
                  DataType = RestMethod.RestMethodReturnType.Redirect)]
      [RestMethodParameter(DataType = typeof(String),
                           MethodParameterName = "urlLoginRedirect",
                           UrlParameterName = Cosmo.Workspace.PARAM_LOGIN_REDIRECT)]
      public void UserLogOff(string urlLoginRedirect)
      {
         try
         {
            // Destruye la sesión actual (si la hubiera)
            Workspace.CurrentUser.Destroy();
         }
         catch  
         {
            // Discard exceptions
         }

         // Redirije a la dirección especificada (o a Home si no se especifica)
         if (string.IsNullOrWhiteSpace(urlLoginRedirect))
         {
            Response.Redirect(urlLoginRedirect);
         }
         else
         {
            Response.Redirect(Cosmo.Workspace.COSMO_URL_DEFAULT);
         }
      }

      #endregion

      #region Command: User Verification

      /// <summary>Comando para el handler de usuarios: Verificación de cuenta de correo electrónico.</summary>
      private const string COMMAND_USER_VERIFIY = "uver";

      /// <summary>
      /// Genera una URL válida para verificar una dirección de correo electrónico de un usuario.
      /// </summary>
      /// <param name="verificationKey">Clave de verificación (recibida en un correo electrónico).</param>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static Url GetUserMailVerificationUrl(string verificationKey)
      {
         return new Url(SecurityRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, SecurityRestHandler.COMMAND_USER_VERIFIY)
            .AddParameter(SecurityRestHandler.PARAM_VERIFY_KEY, verificationKey);
      }

      /// <summary>
      /// Verifica el correo de un usuario y activa su cuenta.
      /// </summary>
      private void UserMailVerification()
      {
         try
         {
            // User user = auth.Verify(Request.QueryString);
            User user = Workspace.AuthenticationService.Verify(Request.QueryString);

            Workspace.Logger.Add(new LogEntry("SecurityApi.UserMailVerification()",
                                              "Suscripción verificada de " + user.Login + " desde " + Request.ServerVariables["REMOTE_ADDR"],
                                              LogEntry.LogEntryType.EV_INFORMATION));

            SendResponse(new AjaxResponse());
         }
         catch
         {
            SendResponse(new AjaxResponse(1001, "Se ha producido un error durante el proceso de verificación y no se ha podido completar"));
         }
      }

      #endregion

      #region Command: User Password Recovery

      /// <summary>Comando para el handler de usuarios: Recuperación de los datos de acceso.</summary>
      private const string COMMAND_USER_PASSWORD_RECOVER = "pwrec";

      /// <summary>
      /// Genera una URL válida para recuperar los datos de conexión de un usuario.
      /// </summary>
      /// <param name="eMail">Cuenta de correo del usuario que desea recuperar los datos de conexión.</param>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static Url GetPasswordRecoveryUrl(string eMail)
      {
         return new Url(SecurityRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, SecurityRestHandler.COMMAND_USER_PASSWORD_RECOVER)
            .AddParameter(SecurityRestHandler.PARAM_PWD_REC_MAIL_KEY, eMail);
      }

      /// <summary>
      /// Envia los datos de conexión a un usuario.
      /// </summary>
      private void UserPasswordRecover()
      {
         CalloutControl callout = new CalloutControl(null);

         // Recupera los parámetros requeridos
         string mail = Parameters.GetString("eml");

         try
         {
            // Envia el correo para refrescar los datos del usuario
            Workspace.AuthenticationService.SendData(mail);

            callout.Type = ComponentColorScheme.Success;
            callout.Title = "Datos enviados";
            callout.Text = "En breves instantes recibirás un correo electrónico que contiene los " +
                           "datos para que puedas acceder nuevamente a tu cuenta de usuario de " +
                           Workspace.Name + ". Si tienes algún problema o duda, puedes " +
                           "<a href=\"" + Cosmo.Workspace.COSMO_URL_CONTACT + "\">contactar nosotros</a>.";
         }
         catch (SecurityException mex)
         {
            callout.Type = ComponentColorScheme.Warning;
            callout.Title = "Cuenta incorrecta!";
            callout.Text = mex.Message;
         }
         catch
         {
            callout.Type = ComponentColorScheme.Error;
            callout.Title = "Ooooops!";
            callout.Text = "Se produjo un error interno del servidor y no ha sido posible enviarte los datos. " +
                           "Verifica si la cuenta de e-mail está bien escrita y repite el proceso. " +
                           "Si tienes algún problema o duda, puedes " +
                           "<a href=\"" + Cosmo.Workspace.COSMO_URL_CONTACT + "\">contactar nosotros</a>.";
         }

         // Envia el resultado al cliente
         Response.Write(Workspace.UIService.Render(callout));
      }

      #endregion

      #region Command: User cancel

      /// <summary>User handler command: Cancel user account.</summary>
      private const string COMMAND_USER_CANCEL = "cancel";

      /// <summary>
      /// Generates a valid URL to call this REST method.
      /// </summary>
      /// <param name="userId">User identifier.</param>
      /// <returns>A string containg the requested URL.</returns>
      public static Url GetCancelAccountUrl(int userId)
      {
         return new Url(SecurityRestHandler.ServiceUrl).
            AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_USER_CANCEL).
            AddParameter(Cosmo.Workspace.PARAM_USER_ID, userId);
      }

      /// <summary>
      /// Cancel a user account.
      /// </summary>
      [AuthorizationRequired(Cosmo.Workspace.ROLE_ADMINISTRATOR)]
      [RestMethod( CommandName = COMMAND_USER_CANCEL,
                   DataType = RestMethod.RestMethodReturnType.StandardRestResponse )]
      [RestMethodParameter(DataType = typeof(Int32),
                           MethodParameterName = "userId",
                           UrlParameterName = Cosmo.Workspace.PARAM_USER_ID )]
      public void CancelAccount(int userId)
      {
         try
         {
            Workspace.AuthenticationService.Cancel(userId);

            SendResponse(new AjaxResponse());
         }
         catch
         {
            SendResponse(new AjaxResponse(1002, "Ha ocurrido un error y no ha sido posible cancelar la cuenta solicitada."));
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve la URL a la que se debe atacar para realizar operaciones REST sobre el servicio.
      /// </summary>
      public static string ServiceUrl
      {
         get { return MethodBase.GetCurrentMethod().DeclaringType.Name; }
      }

      #endregion

      #region Disabled Code

      /*
      
      /// <summary>
      /// Agrega un fragmento de código JavaScript.
      /// </summary>
      /// <param name="ownerView">Una instancia de <see cref="System.Web.UI.Page"/>.</param>
      /// <param name="code">Código JavaScript.</param>
      public static void AddJavaScriptCode(System.Web.UI.Page ownerView, string code)
      {
         // Agrega el código JavaScript
         HtmlGenericControl js = new HtmlGenericControl("script");
         js.Attributes["type"] = "text/javascript";
         js.InnerHtml = code;

         ownerView.Header.Controls.Add(js);
      }
      
      /// <summary>
      /// Agrega una script JavaScript.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="System.Web.UI.Page"/>.</param>
      /// <param name="url">La URL del código JavaScript.</param>
      public static void RemoveJavaScript(System.Web.UI.Page container, string url)
      {
         string script = "";
         HtmlGenericControl ctrl = null;

         url = url.Trim().ToLower();

         foreach (Control control in container.Header.Controls)
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
            container.Header.Controls.Remove(ctrl);
      }
      
      /// <summary>
      /// Agrega una referencia a hoja de estilo CSS.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="System.Web.UI.Page"/>.</param>
      /// <param name="url">La URL de la hoja de estilo.</param>
      public static void AddCss(System.Web.UI.Page container, string url)
      {
         string script = "";

         url = url.Trim().ToLower();

         foreach (Control control in container.Header.Controls)
         {
            if (control.GetType() == typeof(HtmlLink))
            {
               script = ((HtmlLink)control).Href.Trim().ToLower();
               if (url.Trim().ToLower().Equals(script))
                  return;
            }
         }

         HtmlLink link = new HtmlLink();
         link.Href = url;
         link.Attributes.Add("rel", "stylesheet");
         link.Attributes.Add("type", "text/css");
         container.Header.Controls.Add(link);
      }
      
      /// <summary>
      /// Agrega una referencia a un feed RSS.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="System.Web.UI.Page"/>.</param>
      /// <param name="title">Título visible del feed.</param>
      /// <param name="url">La URL del feed.</param>
      public static void AddRss(System.Web.UI.Page container, string title, string url)
      {
         HtmlLink link = new HtmlLink();
         link.Href = url;
         link.Attributes.Add("rel", "alternate");
         link.Attributes.Add("type", "application/rss+xml");
         link.Attributes.Add("title", title);

         container.Header.Controls.Add(link);
      }
      
      /// <summary>
      /// Agrega una script JavaScript.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="System.Web.UI.Page"/>.</param>
      /// <param name="url">La URL del código JavaScript.</param>
      public static void AddJavaScript(System.Web.UI.Page container, string url)
      {
         string script = "";

         url = url.Trim().ToLower();

         foreach (Control control in container.Header.Controls)
         {
            if (control.GetType() == typeof(HtmlGenericControl))
            {
               if (((HtmlGenericControl)control).Attributes["src"] != null)
               {
                  script = ((HtmlGenericControl)control).Attributes["src"].Trim().ToLower();
                  if (url.Trim().ToLower().Equals(script))
                     return;
               }
            }
         }

         HtmlGenericControl js = new HtmlGenericControl("script");
         js.Attributes["type"] = "text/javascript";
         js.Attributes["src"] = url;
         container.Header.Controls.Add(js);
      } 
       
      /// <summary>
      /// Agrega metainformación a la cabecera de una página.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="System.Web.UI.Page"/>.</param>
      /// <param name="meta">Contenido del parámetro <c>http-equiv</c>.</param>
      /// <param name="content">Contenido del parámetro <c>content</c>.</param>
      public static void AddMeta(System.Web.UI.Page container, string meta, string content)
      {
         HtmlMeta metaInfo = new HtmlMeta();
         metaInfo.HttpEquiv = meta;
         metaInfo.Content = content;

         container.Header.Controls.Add(metaInfo);
      }

      /// <summary>
      /// Obtiene la instancia de <see cref="Cosmo.Workspace.WebWorkspace"/> usada por el usuario durante la sesión.
      /// </summary>
      /// <param name="context">Una instancia de <see cref="System.Web.HttpContext"/> que describe el contexto de la llamada al servidor.</param>
      public static Workspace GetWorkspace(HttpContext context)
      {
         Workspace wws = null;

         if (context == null || context.Session == null || context.Session[Workspace.SESSION_WORKSPACE] == null)
         {
            wws = new Workspace(context);
            context.Session[Workspace.SESSION_WORKSPACE] = wws;
         }
         else
         {
            wws = (Workspace)context.Session[Workspace.SESSION_WORKSPACE];
         }

         if (wws.Status == WorkspaceStatus.Stopped)
         {
            context.Result.Redirect(wws.UrlClosed, true);
         }

         return wws;
      }
       */
      /*
      /// <summary>
      /// Determina si existe una sesión de usuario iniciada.
      /// </summary>
      /// <param name="session">La instancia Session accesible desde cualquier página ASPX</param>
      /// <returns>Un valor booleano indicando si existe o no sesión de usuario.</returns>
      public static bool DetectUserSession(System.Web.SessionState.HttpSessionState session)
      {
         try
         {
            // Detecta si existe un objeto en la sessión
            if (session[Workspace.SESSION_WORKSPACE] == null)
               return false;

            if (!((Workspace)session[Workspace.SESSION_WORKSPACE]).CurrentUser.IsAuthenticated)
               return false;

            // Detecta si el usuario almacenado es válido
            return (((Workspace)session[Workspace.SESSION_WORKSPACE]).CurrentUser.User.ID > 0);
         }
         catch (Exception)
         {
            return false;
         }
      }

      /// <summary>
      ///  Determina si existe una sesión de usuario iniciada y esta pertenece a un login concreto.
      /// </summary>
      /// <param name="session">La instancia Session accesible desde cualquier página ASPX</param>
      /// <param name="login">Login para el cual se desea testear si la sesión actual le pertenece</param>
      /// <returns>Un valor booleano indicando si existe o no sesión de usuario</returns>
      public static bool DetectUserSession(System.Web.SessionState.HttpSessionState session, string login)
      {
         try
         {
            // Detecta si existe un objeto en la sessión
            if (session == null)
               return false;

            if (session[Workspace.SESSION_WORKSPACE] == null)
               return false;

            if (!((Workspace)session[Workspace.SESSION_WORKSPACE]).CurrentUser.IsAuthenticated)
               return false;

            // Detecta si el usuario almacenado es válido
            if (!((Workspace)session[Workspace.SESSION_WORKSPACE]).CurrentUser.User.Login.Trim().ToLower().Equals(login.Trim().ToLower()))
               return false;

            return true;
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      ///  Determina si existe una sesión de usuario iniciada.
      /// </summary>
      /// <param name="context">Contexto de la aplicación (propiedad Contect de la página ASPX)</param>
      /// <param name="loginScript">Script usado para autenticar a los usuarios.</param>
      /// <param name="url">La URL actual dónde se redirigirá al usuario una vez identificado</param>
      /// <returns>Un valor booleano indicando si existe o no sesión de usuario</returns>
      public static bool DetectUserSession(HttpContext context, string loginScript, string url)
      {
         string loginUrl = loginScript + "?" + Workspace.PARAM_LOGIN_REDIRECT + "=" + HttpUtility.UrlEncode(url);

         try
         {
            // Detecta si existe un objeto en la sessión
            if (context.Session[Workspace.SESSION_WORKSPACE] == null)
            {
               context.Result.Redirect(loginUrl, true);
            }

            // Detecta si ya existe una sesión de usuario
            if (!((Workspace)context.Session[Workspace.SESSION_WORKSPACE]).CurrentUser.IsAuthenticated)
            {
               context.Result.Redirect(loginUrl, true);
            }

            return true;
         }
         catch (System.Threading.ThreadAbortException)
         {
            // Retorna true ja que al arribar aquí ha completat el mètode Redirect
            return true;
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      ///  Determina si existe una sesión de usuario iniciada.
      /// </summary>
      /// <param name="context">Contexto de la aplicación (propiedad Contect de la página ASPX)</param>
      /// <param name="loginScript">Script usado para autenticar a los usuarios.</param>
      /// <returns>Un valor booleano indicando si existe o no sesión de usuario</returns>
      public static bool DetectUserSession(HttpContext context, string loginScript)
      {
         return AuthenticationService.IsAuthenticated(context,
                                                       loginScript, 
                                                       context.Request.RawUrl.Replace(context.Request.ApplicationPath, string.Empty).Replace("/", string.Empty));

         // TODO: Revisar si no és Request.Url.OriginalString el tercer paràmetre
      }
      
      /// <summary>
      /// Obliga a refrescar los datos del usuario de la sesión.
      /// </summary>
      /// <param name="session">La instancia Session accesible desde cualquier página ASPX</param>
      /// <param name="user">Perfil del usuario con los datos actualizados.</param>
      /// <returns>La instancia de User con los datos actualizados</returns>
      [Obsolete()]
      public static User UserSessionRefresh(System.Web.SessionState.HttpSessionState session, User user)
      {
         try
         {
            // Detecta si existe un objeto en la sessión
            if (session[Workspace.SESSION_WORKSPACE] == null)
            {
               return null;
            }

            // Actualiza los datos de la sesión
            return ((Workspace) session[Workspace.SESSION_WORKSPACE]).AuthenticationService.Autenticate(user.Login, user.Password);
         }
         catch
         {
            return null;
         }
      }*/

      #endregion

   }
}