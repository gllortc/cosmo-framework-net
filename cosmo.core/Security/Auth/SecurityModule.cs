using Cosmo.Net;
using Cosmo.Security.Cryptography;
using Cosmo.Services;
using Cosmo.Utils;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;

namespace Cosmo.Security.Auth
{

   /// <summary>
   /// Abstrcta class that must be implemented by security modules.
   /// </summary>
   public abstract class SecurityModule
   {
      // Internal data declarations
      private Plugin _plugin;
      private Workspace _ws;

      #region Constants

      // Sesión de usuario requerida.
      private const string SETTING_SECURITY_ENABLED = "security.enabled";
      // Sesión de usuario requerida.
      private const string SETTING_SECURITY_BLOQUEDIP = "security.bloquedip";
      // Clave de encriptación por defecto a usar para encriptar cadenas de texto.
      private const string SETTING_SECURITY_ENCRYPTIONKEY = "security.encryptionkey";
      // Verification mail enabled
      private const string SETTING_SECURITY_VERIFMSG_ENABLED = "security.verifymail.required";
      // Verification mail HTML format
      private const string SETTING_SECURITY_VERIFMSG_HTML = "security.verifymail.html";
      // Verification mail subject
      private const string SETTING_SECURITY_VERIFMSG_SUBJECT = "security.verifymail.subject";
      // Verification mail body
      private const string SETTING_SECURITY_VERIFMSG_BODY = "security.verifymail.body";
      // Personal data mail HTML format
      private const string SETTING_SECURITY_PDATAMSG_HTML = "security.pdatamail.html";
      // Personal data mail subject
      private const string SETTING_SECURITY_PDATAMSG_SUBJECT = "security.pdatamail.subject";
      // Personal data mail body
      private const string SETTING_SECURITY_PDATAMSG_BODY = "security.pdatamail.body";

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

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="SecurityModule"/>.
      /// </summary>
      /// <param name="workspace">An instance of current Cosmo workspace</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      protected SecurityModule(Workspace workspace, Plugin plugin)
      {
         _ws = workspace;
         _plugin = plugin;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el workspace para el que se está autenticando.
      /// </summary>
      public Workspace Workspace 
      { 
         get { return _ws; } 
      }

      /// <summary>
      /// Devuelve el nombre (ID) del módulo de autenticación configurado.
      /// </summary>
      public string ID
      {
         get { return _plugin.ID; }
      }

      /// <summary>
      /// Gets a boolean indicating id the security is enabled for the current workspace.
      /// </summary>
      public bool SecurityRequired 
      {
         get { return _plugin.GetBoolean(SETTING_SECURITY_ENABLED); } 
      }

      /// <summary>
      ///  Devuelve la clave de encriptación usada para encriptar todo lo referente a seguridad.
      /// </summary>
      public string EncriptionKey
      {
         get { return _plugin.GetString(SETTING_SECURITY_ENCRYPTIONKEY); }
      }

      /// <summary>
      /// Gets a boolean value indicating if the signup process must verify the mail address sending a mail
      /// with a verification link.
      /// </summary>
      public bool IsVerificationMailRequired
      {
         get { return _plugin.GetBoolean(SETTING_SECURITY_VERIFMSG_ENABLED); }
      }

      /// <summary>
      /// Gets the verification mail subject.
      /// </summary>
      public string VerificationMailSubject
      {
         get { return _plugin.GetString(SETTING_SECURITY_VERIFMSG_SUBJECT); }
      }

      /// <summary>
      /// Gets the verification mail body.
      /// </summary>
      public string VerificationMailBody
      {
         get { return _plugin.GetString(SETTING_SECURITY_VERIFMSG_BODY); }
      }

      /// <summary>
      /// Gets the verification mail body.
      /// </summary>
      public bool IsVerificationMailHtmlFormat
      {
         get { return _plugin.GetBoolean(SETTING_SECURITY_VERIFMSG_HTML); }
      }

      /// <summary>
      /// Gets the verification mail subject.
      /// </summary>
      public string PersonalDataMailSubject
      {
         get { return _plugin.GetString(SETTING_SECURITY_PDATAMSG_SUBJECT); }
      }

      /// <summary>
      /// Gets the verification mail body.
      /// </summary>
      public string PersonalDataMailBody
      {
         get { return _plugin.GetString(SETTING_SECURITY_PDATAMSG_BODY); }
      }

      /// <summary>
      /// Gets the verification mail body.
      /// </summary>
      public bool IsPersonalDataMailHtmlFormat
      {
         get { return _plugin.GetBoolean(SETTING_SECURITY_PDATAMSG_HTML); }
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
         return !_plugin.GetString(SETTING_SECURITY_BLOQUEDIP).Contains(ipAddress);
      }

      /// <summary>
      /// Generates the login view Url.
      /// </summary>
      /// <param name="redirectToUrl">If the authentication process is successful, the Url to navigate.</param>
      /// <returns>A string containing the requested url.</returns>
      public string GetLoginUrl(string redirectToUrl)
      {
         Url url = new Url(_plugin.GetString("security.LoginView", Cosmo.Web.UserAuth.GetURL()));
         if (!string.IsNullOrWhiteSpace(redirectToUrl))
         {
            url.AddParameter(Cosmo.Workspace.PARAM_LOGIN_REDIRECT, redirectToUrl);
         }

         return url.ToString();
      }

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
         string url = Cosmo.Net.Url.Combine(_ws.Url, Cosmo.Web.UserJoinVerification.GetURL(qs));

         // Generate mail message
         msg.To.Add(new MailAddress(user.Mail, string.IsNullOrWhiteSpace(user.Name) ? user.Login : user.Name));

         // Message body generation
         string body = this.VerificationMailBody;
         body = body.Replace(TAG_USER_LOGIN, user.Login);
         body = body.Replace(TAG_USER_MAIL, user.Mail);
         body = body.Replace(TAG_USER_NAME, user.Name);
         body = body.Replace(TAG_USER_PASSWORD, user.Password);
         body = body.Replace(TAG_WORKSPACE_MAIL, _ws.Mail);

         if (this.IsVerificationMailHtmlFormat)
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
         string subject = this.VerificationMailSubject;
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
         msg.To.Add(new MailAddress(user.Mail, string.IsNullOrWhiteSpace(user.Name) ? user.Login : user.Name));

         // Formatea el cuerpo del mensaje
         string body = this.PersonalDataMailBody;
         body = body.Replace(TAG_USER_LOGIN, user.Login);
         body = body.Replace(TAG_USER_MAIL, user.Mail);
         body = body.Replace(TAG_USER_NAME, user.Name);
         body = body.Replace(TAG_USER_PASSWORD, user.Password);
         body = body.Replace(TAG_WORKSPACE_MAIL, _ws.Mail);

         if (this.IsPersonalDataMailHtmlFormat)
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
         string subject = this.PersonalDataMailSubject;
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

      #region Abstract Members

      /// <summary>
      /// User authentication.
      /// </summary>
      /// <param name="login">User login.</param>
      /// <param name="password">User password.</param>
      /// <returns>Returns the <see cref="User"/> instance with authenticated user data.</returns>
      /// <remarks>
      /// In case of authentication failure, this method must raise an exception. See security exceptions.
      /// </remarks>
      public abstract User Autenticate(string login, string password);

      /// <summary>
      /// Gets a list of all users in workspace.
      /// </summary>
      /// <returns>A list of <see cref="User"/> instances.</returns>
      public abstract List<User> GetUsersList();

      /// <summary>
      /// Gets a list of all users in workspace filtered by his status.
      /// </summary>
      /// <param name="status">Status filter.</param>
      /// <returns>A list of <see cref="User"/> instances.</returns>
      public abstract List<User> GetUsersList(User.UserStatus status);

      /// <summary>
      /// Get a user account.
      /// </summary>
      /// <param name="login">User login.</param>
      /// <returns>An instance of <see cref="User"/> corresponding to the requested user or <c>null</c> if login don't exist.</returns>
      public abstract User GetUser(string login);

      /// <summary>
      /// Get a user account.
      /// </summary>
      /// <param name="uid">User unique identifier (DB).</param>
      /// <returns>An instance of <see cref="User"/> corresponding to the requested user or <c>null</c> if identifier doesn't exist.</returns>
      public abstract User GetUser(int uid);

      /// <summary>
      /// Gets the user location (city and country name) with a standard format.
      /// </summary>
      /// <param name="uid">User unique identifier.</param>
      /// <returns>A string with formatted city and country information.</returns>
      public abstract string GetUserLocation(int uid);

      /// <summary>
      /// Creates a new user account.
      /// </summary>
      /// <param name="user">An instance containing the new user data.</param>
      /// <param name="confirm">If it's <c>true</c> this process must be validated by clicking a link in an email sent to user.</param>
      public abstract void Create(User user, bool confirm);

      /// <summary>
      /// Update user account data.
      /// </summary>
      /// <param name="user">An instance with updated user data.</param>
      /// <remarks>
      /// This method don't update the password. The method <c>SetPassword()</c> perform this change.
      /// </remarks>
      public abstract void Update(User user);

      /// <summary>
      /// Delete a user account.
      /// </summary>
      /// <param name="uid">User account unique identifier (DB).</param>
      /// <remarks>
      /// This method remove the record in database.
      /// </remarks>
      public abstract void Delete(int uid);

      /// <summary>
      /// Cancel a user account.
      /// </summary>
      /// <param name="uid">User account unique identifier (DB).</param>
      /// <remarks>
      /// This method don't remove the database related record, only update data to erase all personal data. It can be
      /// useful to mantain the login reserved.
      /// </remarks>
      public abstract void Cancel(int uid);

      /// <summary>
      /// Verify a user account (with the verification pending status).
      /// </summary>
      /// <param name="uid">User account unique identifier (DB).</param>
      /// <param name="mail">Mail account corresponding to the user account.</param>
      /// <returns>An instance of <see cref="User"/> corresponding to the verified user.</returns>
      public abstract User Verify(int uid, string mail);

      /// <summary>
      /// Verify a user account (with the verification pending status).
      /// </summary>
      /// <param name="QueryString">A collection with URL parameters obtained in request.</param>
      /// <returns>An instance of <see cref="User"/> corresponding to the verified user.</returns>
      public abstract User Verify(NameValueCollection QueryString);

      /// <summary>
      /// Send a mail to user with connection data.
      /// </summary>
      /// <param name="address">Mail address corresponding to the user account.</param>
      public abstract User SendData(string address);

      /// <summary>
      /// Change the passord for an user account.
      /// </summary>
      /// <param name="uid">User account unique identifier (DB).</param>
      /// <param name="newPassword">A string with new password.</param>
      public abstract void SetPassword(int uid, string newPassword);

      /// <summary>
      /// Change the passord for an user account.
      /// </summary>
      /// <param name="uid">User account unique identifier (DB).</param>
      /// <param name="oldPassword">Current password.</param>
      /// <param name="newPassword">New password.</param>
      /// <param name="newPasswordVerification">New password re-typed.</param>
      /// <remarks>
      /// If parameters <c>newPassword</c> and <c>newPasswordVerification</c> must be identicals. If not, 
      /// a <see cref="SecurityException"/> is raised.
      /// </remarks>
      public abstract void SetPassword(int uid, string oldPassword, string newPassword, string newPasswordVerification);

      /// <summary>
      /// Verifica una contraseña de usuario.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="password">Contraseña a verificar.</param>
      /// <returns>Devuelve <c>true</c> si la contraseña coincide o <c>false</c> en cualquier otro caso.</returns>
      public abstract bool CheckPassword(int uid, string password);

      /// <summary>
      /// Selecciona un grupo de usuarios
      /// </summary>
      /// <param name="selector">Selector que contiene los critérios de búsqueda</param>
      /// <returns>La lista de usuarios seleccioandos</returns>
      public abstract List<User> FindByCriteria(UserSearchCriteria selector);

      /// <summary>
      /// Busca usuarios del workspace.
      /// </summary>
      /// <param name="login">Login o fracción a buscar</param>
      /// <param name="city">Ciudad o fracción a buscar</param>
      /// <param name="countryId">Identificador del pais a buscar</param>
      /// <returns></returns>
      public abstract List<User> Find(string login, string city, int countryId);

      /// <summary>
      /// Busca usuarios del workspace.
      /// </summary>
      /// <param name="login">Login o fracción a buscar</param>
      /// <param name="city">Ciudad o fracción a buscar</param>
      /// <param name="countryId">Identificador del pais a buscar</param>
      /// <param name="onlyEnabledUsers">Indicates if the methos must show only the enabled users.</param>
      /// <returns></returns>
      public abstract List<User> Find(string login, string city, int countryId, bool onlyEnabledUsers);

      /// <summary>
      /// Devuelve una lista de paises
      /// </summary>
      public abstract List<Country> ListCountry();

      #endregion

      #region Disabled Code

      /// <summary>
      /// Obtiene una lista de los roles de un usuario.
      /// </summary>
      /// <param name="uid">Identificador único del usuario.</param>
      /// <returns>Una lista de instancias de <see cref="Role"/> que representan los roles del usuario.</returns>
      // public abstract List<Role> GetUserRoles(int uid);

      /// <summary>
      /// Obtiene una lista de los roles de un usuario.
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <returns>Una lista de instancias de <see cref="Role"/> que representan los roles del usuario.</returns>
      // public abstract List<Role> GetUserRoles(string login);

      /// <summary>
      /// Obtiene una lista de usuarios para un determinado rol.
      /// </summary>
      /// <param name="roleId">Identificador del rol.</param>
      /// <returns>Una instancia de <see cref="System.Collections.Generic.List&lt;T&gt;"/> rellenada con la lista de usuarios.</returns>
      // public abstract List<User> GetRoleUsers(int roleId);

      #endregion

   }

}
