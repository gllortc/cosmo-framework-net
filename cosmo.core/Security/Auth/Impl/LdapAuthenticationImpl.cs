using Cosmo.Services;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Cosmo.Security.Auth.Impl
{
   /// <summary>
   /// Proveedor de autenticación nativo de Cosmo.
   /// </summary>
   /// <remarks>
   /// Este proveedor usa para la autenticación de usuario la base de datos del workspace (tabla USERS).
   /// </remarks>
   public class LdapAuthenticationImpl : IAuthenticationModule
   {
      // Declaración de variables internas
      private Workspace _ws = null;

      private const int USER_OPTIONS_NOTIFY_INSIDE = 1;
      private const int USER_OPTIONS_NOTIFY_OUTSIDE = 2;
      private const int USER_OPTIONS_NOTIFY_PRIVATEMSGS = 4;

      private const string USER_FIELDS = "usrid,usrlogin,usrmail,usrmail2,usrname,usrcity,usrcountryid,usrphone,usrdesc,usroptions,usrstatus,usrlastlogon,usrlogoncount,usrowner,usrcreated,usrpwd";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="LdapAuthenticationImpl"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que permite obtener los parámetros de configuración.</param>
      public LdapAuthenticationImpl(Workspace workspace, Plugin plugin) 
         : base(workspace, plugin) 
      { }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve una lista de todos los usuarios
      /// </summary>
      /// <param name="status">Filtra por el estado de la cuenta</param>
      /// <returns>Una lista de objetos WSUser</returns>
      public override List<User> GetUsersList(User.UserStatus status)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Devuelve una lista de todos los usuarios
      /// </summary>
      /// <returns>Una lista de objetos WSUser</returns>
      public override List<User> GetUsersList()
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Obtiene los datos de una cuenta del workspace.
      /// </summary>
      /// <param name="login">Login de la cuenta de usuario.</param>
      /// <returns>Una instáncia MWUser.</returns>
      public override User GetUser(string login)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Obtiene los datos de una cuenta del workspace.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <returns>Una instáncia MWUser.</returns>
      public override User GetUser(int uid)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Gets the user location (city and country name) with a standard format.
      /// </summary>
      /// <param name="uid">User unique identifier.</param>
      /// <returns>A string with formatted city and country information.</returns>
      public override string GetUserLocation(int uid)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Crea una nueva cuenta de usuario del workspace
      /// </summary>
      /// <param name="user">Instáncia de WSUser que contiene los datos de la nueva cuenta</param>
      /// <param name="confirm">Indica que al agregar el usuario se le mandará un correo electrónico para la confirmación de su cuenta de correo y mientras no se confirme, la cuenta estará en estado pendiente de verificación.</param>
      public override void Create(User user, bool confirm)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Crea una nueva cuenta de usuario del workspace
      /// </summary>
      /// <param name="user">Instáncia de WSUser que contiene los datos de la nueva cuenta</param>
      public override void Create(User user)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="user">Instáncia de MWUser que contiene los datos actualizados de la cuenta.</param>
      /// <remarks>
      /// Para actualizar la contraseña debe usar el método SetPassword() ya que éste método no
      /// actualiza la contraseña.
      /// </remarks>
      public override void Update(User user)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Elimina una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="id">Identificador de la cuenta a eliminar</param>
      public override void Delete(int id)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Cancela una cuenta de usuario.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta a cancelar.</param>
      public override void Cancel(int uid)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Verifica una cuenta de usuario (pasa del estado 'No verificada' a 'Activa'). Este método permite implementar
      /// un sistema de generación de cuentas por parte de los usuarios (registro de usuarios).
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="mail">Cuenta de correo del usuario</param>
      public override User Verify(int uid, string mail)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Verifica una cuenta de usuario (pasa del estado 'No verificada' a 'Activa'). Este método permite implementar
      /// un sistema de generación de cuentas por parte de los usuarios (registro de usuarios).
      /// </summary>
      /// <param name="QueryString">Colección de parámetros del enlace de verificación mandado por correo electrónico al usuario (Request.QueryString).</param>
      public override User Verify(NameValueCollection QueryString)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Envia los datos de un usuario a su cuenta de correo para el acceso al Workspace
      /// </summary>
      /// <param name="address">Dirección de correo a la que se mandarán los datos. Debe coincidir con la dirección de una cuenta de usuario.</param>
      /// <returns>La instáncia de MWUser con los datos del usuario que realiza l apetición.</returns>
      public override User SendData(string address)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Permite verificar las credenciales de acceso a una cuenta de usuario (login).
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <param name="password">Contraseña del usuario.</param>
      public override User Autenticate(string login, string password)
      {
         /*
         bool authenticated = false;
         string domain = _ws.Settings.GetString("workspace.ldap.domain");  // e.g. LDAP://domain.com

         try
         {
            DirectoryEntry entry = new DirectoryEntry(domain, login, password);
            object nativeObject = entry.NativeObject;
            authenticated = true;
         }
         catch (DirectoryServicesCOMException cex)
         {
            //not authenticated; reason why is in cex
         }
         catch (Exception ex)
         {
            //not authenticated due to some other exception [this is optional]
         }
        
         return authenticated;
         */

         throw new NotImplementedException();
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta de usuario.</param>
      /// <param name="newPassword">Nueva contraseña del usuario.</param>
      public override void SetPassword(int uid, string newPassword)
      {
         throw new NotImplementedException();
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
      public override void SetPassword(int uid, string oldPassword, string newPassword, string newPasswordVerification)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Verifica una contraseña de usuario.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="password">Contraseña a verificar.</param>
      /// <returns>Devuelve <c>true</c> si la contraseña coincide o <c>false</c> en cualquier otro caso.</returns>
      public override bool CheckPassword(int uid, string password)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Selecciona un grupo de usuarios
      /// </summary>
      /// <param name="selector">Selector que contiene los critérios de búsqueda</param>
      /// <returns>La lista de usuarios seleccioandos</returns>
      public override List<User> FindByCriteria(UserSearchCriteria selector)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Busca usuarios del workspace.
      /// </summary>
      /// <param name="login">Login o fracción a buscar</param>
      /// <param name="city">Ciudad o fracción a buscar</param>
      /// <param name="countryId">Identificador del pais a buscar</param>
      /// <returns></returns>
      public override List<User> Find(string login, string city, int countryId)
      {
         return Find(login, city, countryId, false);
      }

      /// <summary>
      /// Busca usuarios del workspace.
      /// </summary>
      /// <param name="login">Login o fracción a buscar</param>
      /// <param name="city">Ciudad o fracción a buscar</param>
      /// <param name="countryId">Identificador del pais a buscar</param>
      /// <returns></returns>
      public override List<User> Find(string login, string city, int countryId, bool onlyEnabledUsers)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Devuelve una lista de paises
      /// </summary>
      public override List<Country> ListCountry()
      {
         CountryDAO cdao = new CountryDAO(_ws);
         return cdao.GetCountryList();
      }

      #endregion

   }
}
