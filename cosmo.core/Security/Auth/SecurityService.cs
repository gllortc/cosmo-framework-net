using Cosmo.Net;
using Cosmo.Security.Cryptography;
using Cosmo.Services;
using Cosmo.Utils;
using Cosmo.WebApp.UserServices;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace Cosmo.Security.Auth
{
   /// <summary>
   /// Implementa una clase para la autenticación y gestión de las cuentas de usuario.
   /// </summary>
   /// <remarks>
   /// Esta clase hace uso de los proveedores de autenticación.
   /// </remarks>
   public class SecurityService 
   {
      // Cuenta de ex-mail desde dónde se envian notificaciones a los usuarios del workspace.
      private const string UsersMailFromAddress = "users.mail.fromadd";
      // Nombre asociado a la cuenta de ex-mail desde dónde se envian notificaciones a los usuarios del workspace.
      private const string UsersMailFromName = "users.mail.fromname";
      // Asunto del correo de verificación que reciben los usuarios al crear una nueva cuenta.
      private const string UsersJoinSubject = "users.join.subject";
      // Cuerpo del correo de verificación que reciben los usuarios al crear una nueva cuenta.
      private const string UsersJoinBody = "users.join.body";
      // Formato del correo de verificación que reciben los usuarios al crear una nueva cuenta.
      private const string UsersJoinHTMLFormat = "users.join.html";
      // Asunto del correo para recuperar las credenciales de un usuario del workspace.
      private const string UsersRecoverSubject = "users.rpwd.subject";
      // Cuerpo del correo para recuperar las credenciales de un usuario del workspace.
      private const string UsersRecoverBody = "users.rpwd.body";
      // Formato del correo para recuperar las credenciales de un usuario del workspace.
      private const string UsersRecoverHTMLFormat = "users.rpwd.html";

      // Declaración de variables internas
      private Workspace _ws = null;
      private Dictionary<string, SecurityModule> _modules;

      /// <summary>Proveedor de autenticación por defecto (Cosmo Workspace Authentication provider).</summary>
      public const String PROVIDER_DEFAULT = "Cosmo.Security.Auth.Impl.SqlServerAuthenticationImpl";

      /// <summary>Login del usuario super</summary>
      public const String ACCOUNT_SUPER = "SA";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="SecurityService"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      public SecurityService(Workspace workspace)
      {
         Initialize();

         _ws = workspace;

         LoadAuthenticationModules();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets a boolean indicating id the security is enabled for the current workspace.
      /// </summary>
      public bool SecurityRequired
      {
         get { return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].SecurityRequired; }
      }

      /// <summary>
      /// Indica si al crear una cuenta se verifica la cuenta de correo mediante el envio de un correo 
      /// de verificación.
      /// </summary>
      public bool IsVerificationMailRequired
      {
         get { return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].IsVerificationMailRequired; }
      }

      /// <summary>
      ///  Devuelve la clave de encriptación usada para encriptar todo lo referente a seguridad.
      /// </summary>
      public string EncriptionKey
      {
         get { return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].EncriptionKey; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Check if a IP address can access to workspace.
      /// </summary>
      /// <param name="ipAddress">IP address to check.</param>
      /// <returns><c>true</c> if IP can access to workspace or <c>false</c> in all other cases.</returns>
      public bool IsValidIPAddress(string ipAddress)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].IsValidIPAddress(ipAddress);
      }

      /// <summary>
      /// Autentica al usuario.
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <param name="password">Contraseña.</param>
      /// <returns>Si ha tenido exito, devuelve </returns>
      public User Autenticate(string login, string password)
      {
         try
         {
            // Invoca el método
            User user = _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].Autenticate(login, password);

            // Invoca el método
            // User user = (User)_type.InvokeMember("Autenticate", BindingFlags.InvokeMethod, null, _provider, parameters);

            // Actualiza la sesión abierta del workspace
            _ws.CurrentUser.GenerateSession(user);

            return user;
         }
         catch (TargetInvocationException e)
         {
            if (e.InnerException.GetType() == typeof(UserDisabledException)) throw new UserDisabledException();
            if (e.InnerException.GetType() == typeof(UserNotVerifiedException)) throw new UserNotVerifiedException();
            if (e.InnerException.GetType() == typeof(AuthenticationException)) throw new AuthenticationException();

            throw e;
         }
      }

      /// <summary>
      /// Obtiene una lista de usuarios.
      /// </summary>
      /// <returns>Una lista de usuarios.</returns>
      public List<User> GetUsersList()
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].GetUsersList();
      }

      /// <summary>
      /// Obtiene una lista de usuarios.
      /// </summary>
      /// <returns>Una lista de usuarios.</returns>
      public List<User> GetUsersList(User.UserStatus status)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].GetUsersList(status);
      }

      /// <summary>
      /// Obtiene las propiedades de un usuario.
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <returns>Una instancia de User con los datos del usuario.</returns>
      public User GetUser(string login)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].GetUser(login);
      }

      /// <summary>
      /// Obtiene las propiedades de un usuario.
      /// </summary>
      /// <param name="uid">Identificador único del usuario.</param>
      /// <returns>Una instancia de User con los datos del usuario.</returns>
      public User GetUser(int uid)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].GetUser(uid);
      }

      /// <summary>
      /// Gets the user location (city and country name) with a standard format.
      /// </summary>
      /// <param name="uid">User unique identifier.</param>
      /// <returns>A string with formatted city and country information.</returns>
      public String GetUserLocation(int uid)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].GetUserLocation(uid);
      }

      /// <summary>
      /// Crea una nueva cuenta de usuario.
      /// </summary>
      /// <param name="user">Una instancia de User con los datos de la cuenta a crear.</param>
      public void Create(User user)
      {
         _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].Create(user);
      }

      /// <summary>
      /// Crea una nueva cuenta de usuario.
      /// </summary>
      /// <param name="user">Una instancia de User con los datos de la cuenta a crear.</param>
      /// <param name="confirm">Indica si se desea confirmar la cuenta de correo electrónico vía correo electrónico.</param>
      public void Create(User user, bool confirm)
      {
         _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].Create(user, confirm);
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario.
      /// </summary>
      /// <param name="user">Una instancia de User con los datos actualizados de la cuenta.</param>
      /// <remarks>
      /// Para actualizar la contraseña debe usar el método SetPassword() ya que éste método no actualiza la contraseña.
      /// </remarks>
      public void Update(User user)
      {
         _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].Update(user);
      }

      /// <summary>
      /// Elimina una cuenta de usuario.
      /// </summary>
      /// <param name="id">Identificador de la cuenta a eliminar.</param>
      public void Delete(int id)
      {
         _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].Delete(id);
      }

      /// <summary>
      /// Cancela una cuenta de usuario.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta a cancelar.</param>
      public void Cancel(int uid)
      {
         _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].Cancel(uid);
      }

      /// <summary>
      /// Verifica una cuenta de usuario (pendiente de verificación por correo electrónico).
      /// </summary>
      /// <param name="uid">Identificador de la cuenta de usuario.</param>
      /// <param name="mail">Correo electrónico asociado a la cuenta.</param>
      /// <returns>Una instancia de User con los datos del usuario verificado.</returns>
      public User Verify(int uid, string mail)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].Verify(uid, mail);
      }

      /// <summary>
      /// Verifica una cuenta de usuario (pendiente de verificación por correo electrónico).
      /// </summary>
      /// <param name="QueryString">Una instancia de NameValueCollection que puede ser Server.Params.</param>
      /// <returns>Una instancia de User con los datos del usuario verificado.</returns>
      public User Verify(NameValueCollection QueryString)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].Verify(QueryString);
      }

      /// <summary>
      /// Envia los datos de un usuario a su cuenta de correo para el acceso al Workspace
      /// </summary>
      /// <param name="address">Dirección de correo a la que se mandarán los datos. Debe coincidir con la dirección de una cuenta de usuario.</param>
      public User SendData(string address)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].SendData(address);
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta de usuario.</param>
      /// <param name="newPassword">Nueva contraseña del usuario.</param>
      public void SetPassword(int uid, string newPassword)
      {
         _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].SetPassword(uid, newPassword);
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta de usuario.</param>
      /// <param name="oldPassword">Contraseña actual.</param>
      /// <param name="newPassword">Nueva contraseña del usuario.</param>
      /// <param name="newPasswordVerification">Verificación de la nueva contraseña.</param>
      /// <remarks>
      /// Los parámetros newPassword y newPasswordVerification deben coincidir o de lo contrario se lanzará la excepción <see cref="SecurityException"/>.
      /// </remarks>
      public void SetPassword(int uid, string oldPassword, string newPassword, string newPasswordVerification)
      {
         _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].SetPassword(uid, oldPassword, newPassword, newPasswordVerification);
      }

      /// <summary>
      /// Obtiene una lista de usuarios que cumplen los criterios de selección.
      /// </summary>
      /// <param name="selector">Criterio de selección de usuarios.</param>
      /// <returns>Una lista de usuarios que cumnplen con los criterios establecidos.</returns>
      public List<User> FindByCriteria(UserSearchCriteria selector)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].FindByCriteria(selector);
      }

      /// <summary>
      /// Busca usuarios del workspace.
      /// </summary>
      /// <param name="login">Login o fracción a buscar</param>
      /// <param name="city">Ciudad o fracción a buscar</param>
      /// <param name="countryId">Identificador del pais a buscar</param>
      /// <returns></returns>
      public List<User> Find(string login, string city, int countryId)
      {
         return Find(login, city, countryId, true);
      }

      /// <summary>
      /// Busca usuarios del workspace.
      /// </summary>
      /// <param name="login">Login o fracción a buscar</param>
      /// <param name="city">Ciudad o fracción a buscar</param>
      /// <param name="countryId">Identificador del pais a buscar</param>
      /// <returns></returns>
      public List<User> Find(string login, string city, int countryId, bool onlyEnabledUsers)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].Find(login, city, countryId, onlyEnabledUsers);
      }

      /// <summary>
      /// Devuelve una lista de paises.
      /// </summary>
      public List<Country> ListCountry()
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].ListCountry();
      }

      /// <summary>
      /// Generates the login view Url.
      /// </summary>
      /// <returns>A string containing the requested url.</returns>
      public string GetLoginUrl()
      {
         return GetLoginUrl(string.Empty);
      }

      /// <summary>
      /// Generates the login view Url.
      /// </summary>
      /// <param name="redirectToUrl">If the authentication process is successful, the Url to navigate.</param>
      /// <returns>A string containing the requested url.</returns>
      public string GetLoginUrl(string redirectToUrl)
      {
         return _modules[_ws.Settings.AuthenticationModules.DefaultPluginId].GetLoginUrl(redirectToUrl);
      }

      /// <summary>TAG para insertar el login del usuario en el texto del mensaje.</summary>
      public const string TAG_USER_LOGIN = "<%LOGIN%>";
      /// <summary>TAG para insertar el correo electrónico del usuario en el texto del mensaje.</summary>
      public const string TAG_USER_MAIL = "<%MAIL%>";
      /// <summary>TAG para insertar el nombre real del usuario en el texto del mensaje.</summary>
      public const string TAG_USER_NAME = "<%NAME%>";
      /// <summary>TAG para insertar la contraseña del usuario en el texto del mensaje.</summary>
      public const string TAG_USER_PASSWORD = "<%PASSWORD%>";
      /// <summary>TAG para insertar el link que da acceso a la verificación de la cuenta de correo.</summary>
      public const string TAG_USER_VERIFYLINK = "<%VERIFY_LINK%>";
      /// <summary>TAG para insertar el mail de contacto del responsable del workspace en el texto del mensaje.</summary>
      public const string TAG_WORKSPACE_MAIL = "<%CONTACTMAIL%>";

      /// <summary>
      /// Genera el correo de verificación de cuenta de correo.
      /// </summary>
      /// <param name="user">Una instancia de <see cref="User"/>.</param>
      /// <returns>Una instancia de <see cref="MailMessage"/> que contiene el correo de verificación de cuentas de eMail.</returns>
      public MailMessage GetVerificationMail(User user)
      {
         MailMessage msg = new MailMessage();

         // Genera la URL de verificación
         string qs = UriCryptography.Encrypt("obj=" + user.Mail + "&id=" + user.ID, this.EncriptionKey);
         // string url = Cosmo.Net.Url.Combine(_ws.Url, Workspace.COSMO_URL_JOIN_VERIFICATION) + "?mode=verify&data=" + qs;
         string url = Cosmo.Net.Url.Combine(_ws.Url, UserJoinVerification.GetURL(qs));

         // Inicializa el correo electrónico
         msg.From = new MailAddress(_ws.Settings.GetString(SecurityService.UsersMailFromAddress),
                                    _ws.Settings.GetString(SecurityService.UsersMailFromName, _ws.Settings.GetString(SecurityService.UsersMailFromAddress)));
         msg.To.Add(new MailAddress(user.Mail,
                                    string.IsNullOrWhiteSpace(user.Name) ? user.Login : user.Name));

         // Formatea el cuerpo del mensaje
         string body = _ws.Settings.GetString(SecurityService.UsersJoinBody);
         body = body.Replace(TAG_USER_LOGIN, user.Login);
         body = body.Replace(TAG_USER_MAIL, user.Mail);
         body = body.Replace(TAG_USER_NAME, user.Name);
         body = body.Replace(TAG_USER_PASSWORD, user.Password);
         body = body.Replace(TAG_WORKSPACE_MAIL, _ws.Mail);

         if (_ws.Settings.GetBoolean(SecurityService.UsersJoinHTMLFormat))
         {
            body = body.Replace(TAG_USER_VERIFYLINK, "<a href=\"" + url.Replace("&", "&amp;") + "\" target=\"_blank\">" + url.Replace("&", "&amp;") + "</a>");
            msg.Body = body;
            msg.IsBodyHtml = true;
         }
         else
         {
            body = body.Replace(TAG_USER_VERIFYLINK, url);
            msg.Body = body;
            msg.IsBodyHtml = false;
         }

         // Formatea el asunto del mensaje
         string subject = _ws.Settings.GetString(SecurityService.UsersJoinSubject);
         subject = subject.Replace(TAG_USER_LOGIN, user.Login);
         subject = subject.Replace(TAG_USER_MAIL, user.Mail);
         subject = subject.Replace(TAG_USER_NAME, user.Name);
         subject = subject.Replace(TAG_USER_PASSWORD, user.Password);
         subject = subject.Replace(TAG_WORKSPACE_MAIL, _ws.Mail);

         msg.Subject = subject;

         return msg;
      }

      /// <summary>
      /// Genera el correo de verificación de cuenta de correo.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      public MailMessage GetVerificationMail(int uid)
      {
         return GetVerificationMail(this.GetUser(uid));
      }

      /// <summary>
      /// Genera el correo de envío de datos de connexión.
      /// </summary>
      /// <param name="user">Una instancia de <see cref="User"/>.</param>
      /// <returns>Una instancia de <see cref="MailMessage"/> que contiene los datos de conexión de un usuario.</returns>
      public MailMessage GetUserDataMail(User user)
      {
         MailMessage msg = new MailMessage();

         // Inicializa el correo electrónico
         msg.From = new MailAddress(_ws.Settings.GetString(SecurityService.UsersMailFromAddress),
                                    _ws.Settings.GetString(SecurityService.UsersMailFromName, _ws.Settings.GetString(SecurityService.UsersMailFromAddress)));
         msg.To.Add(new MailAddress(user.Mail,
                                    string.IsNullOrWhiteSpace(user.Name) ? user.Login : user.Name));

         // Formatea el cuerpo del mensaje
         string body = _ws.Settings.GetString(SecurityService.UsersRecoverBody);
         body = body.Replace(TAG_USER_LOGIN, user.Login);
         body = body.Replace(TAG_USER_MAIL, user.Mail);
         body = body.Replace(TAG_USER_NAME, user.Name);
         body = body.Replace(TAG_USER_PASSWORD, user.Password);
         body = body.Replace(TAG_WORKSPACE_MAIL, _ws.Mail);

         if (_ws.Settings.GetBoolean(SecurityService.UsersRecoverHTMLFormat))
         {
            msg.Body = body;
            msg.IsBodyHtml = true;
         }
         else
         {
            msg.Body = body;
            msg.IsBodyHtml = false;
         }

         // Formatea el asunto del mensaje
         string subject = _ws.Settings.GetString(SecurityService.UsersRecoverSubject);
         subject = subject.Replace(TAG_USER_LOGIN, user.Login);
         subject = subject.Replace(TAG_USER_MAIL, user.Mail);
         subject = subject.Replace(TAG_USER_NAME, user.Name);
         subject = subject.Replace(TAG_USER_PASSWORD, user.Password);
         subject = subject.Replace(TAG_WORKSPACE_MAIL, _ws.Mail);

         msg.Subject = subject;

         return msg;
      }

      /// <summary>
      /// Genera el correo de envío de datos de connexión.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      public MailMessage GetUserDataMail(int uid)
      {
         return GetUserDataMail(this.GetUser(uid));
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Determina si existe una sesión de usuario iniciada.
      /// </summary>
      /// <param name="session">La instancia Session accesible desde cualquier página ASPX</param>
      /// <returns>Un valor booleano indicando si existe o no sesión de usuario.</returns>
      public static bool IsAuthenticated(System.Web.SessionState.HttpSessionState session)
      {
         try
         {
            // Detecta si existe un objeto en la sessión
            if (session == null || session[Workspace.SESSION_WORKSPACE] == null)
            {
               return false;
            }

            if (!((Workspace)session[Workspace.SESSION_WORKSPACE]).CurrentUser.IsAuthenticated)
            {
               return false;
            }

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
      public static bool IsAuthenticated(System.Web.SessionState.HttpSessionState session, string login)
      {
         try
         {
            // Detecta si existe un objeto en la sessión
            if (session == null || session[Workspace.SESSION_WORKSPACE] == null)
            {
               return false;
            }

            if (!((Workspace)session[Workspace.SESSION_WORKSPACE]).CurrentUser.IsAuthenticated)
            {
               return false;
            }

            // Detecta si el usuario almacenado es válido
            if (!((Workspace)session[Workspace.SESSION_WORKSPACE]).CurrentUser.User.Login.Trim().ToLower().Equals(login.Trim().ToLower()))
            {
               return false;
            }

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
      /// <param name="destinationUrl">La URL actual dónde se redirigirá al usuario una vez identificado</param>
      /// <returns>Un valor booleano indicando si existe o no sesión de usuario</returns>
      public static bool IsAuthenticated(HttpContext context, string loginScript, string destinationUrl)
      {
         Url url = new Url(loginScript);
         url.AddParameter(Workspace.PARAM_LOGIN_REDIRECT, destinationUrl);

         try
         {
            // Detecta si existe un objeto en la sessión
            if (context.Session[Workspace.SESSION_WORKSPACE] == null)
            {
               context.Response.Redirect(url.ToString(true), true);
               context.Response.End();
            }

            // Detecta si ya existe una sesión de usuario
            if (!((Workspace)context.Session[Workspace.SESSION_WORKSPACE]).CurrentUser.IsAuthenticated)
            {
               context.Response.Redirect(url.ToString(true), true);
               context.Response.End();
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
      public static bool IsAuthenticated(HttpContext context, string loginScript)
      {
         return IsAuthenticated(context,
                                loginScript,
                                context.Request.RawUrl.Replace(context.Request.ApplicationPath, string.Empty).Replace("/", ""));

         // TODO: Revisar si no és Request.Url.OriginalString el tercer paràmetre
      }

      /// <summary>
      /// Permite verificar si un Login es válido o no. EL login debe tener como mínimo 5 carácteres y 35 como máximo.
      /// </summary>
      /// <param name="login">Login a testear.</param>
      /// <returns>Un valor booleano que indica si es válido o no.</returns>
      /// <remarks>
      /// Regular Expression obtenida de:
      /// http://immike.net/blog/2007/04/06/5-regular-expressions-every-web-programmer-should-know/
      /// (Usar "[a-zA-Z0-9]" para verificar sólo si admite letras y números)
      /// </remarks>
      public static bool IsValidLogin(string login)
      {
         return Regex.IsMatch(login, @"^(?=[a-zA-Z])[-\w.]{4,23}([a-zA-Z\d]|(?<![-.])_)$");
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _ws = null;
         _modules = new Dictionary<string, SecurityModule>();
      }

      /// <summary>
      /// Carga el módulo que se usará para renderizar los controles del workspace.
      /// </summary>
      private void LoadAuthenticationModules()
      {
         string applyRule = string.Empty;
         Type type = null;
         SecurityModule _module;

         foreach (Plugin plugin in _ws.Settings.AuthenticationModules.Plugins)
         {
            Object[] args = new Object[2];
            args[0] = _ws;
            args[1] = plugin;

            type = Type.GetType(plugin.Class, true, true);
            _module = (SecurityModule)Activator.CreateInstance(type, args);

            if (_module != null)
            {
               _modules.Add(_module.ID, _module);
            }
         }
      }

      #endregion

   }
}
