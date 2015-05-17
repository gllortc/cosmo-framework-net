using Cosmo.Services;
using Cosmo.Utils;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Cosmo.Security.Auth
{

   /// <summary>
   /// Interface para los proveedores de seguridad de los usuarios del workspace.
   /// </summary>
   public abstract class IAuthenticationModule
   {
      // Declaración de variables internas
      private Plugin _plugin;
      private Workspace _ws;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="IAuthenticationModule"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      protected IAuthenticationModule(Workspace workspace, Plugin plugin)
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

      #endregion

      #region Abstract Members

      /// <summary>
      /// Autentica al usuario.
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <param name="password">Contraseña.</param>
      /// <returns>Si ha tenido exito, devuelve </returns>
      public abstract User Autenticate(string login, string password);

      /// <summary>
      /// Obtiene una lista de usuarios.
      /// </summary>
      /// <returns>Una lista de usuarios.</returns>
      public abstract List<User> GetUsersList();

      /// <summary>
      /// Obtiene una lista de usuarios de un determinado estado.
      /// </summary>
      /// <param name="status">Estado para el que se desea filtrar.</param>
      /// <returns>Una lista de usuarios.</returns>
      public abstract List<User> GetUsersList(User.UserStatus status);

      /// <summary>
      /// Obtiene las propiedades de un usuario.
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <returns>Una instancia de User con los datos del usuario.</returns>
      public abstract User GetUser(string login);

      /// <summary>
      /// Obtiene las propiedades de un usuario.
      /// </summary>
      /// <param name="uid">Identificador único del usuario.</param>
      /// <returns>Una instancia de User con los datos del usuario.</returns>
      public abstract User GetUser(int uid);

      /// <summary>
      /// Crea una nueva cuenta de usuario.
      /// </summary>
      /// <param name="user">Una instancia de User con los datos de la cuenta a crear.</param>
      public abstract void Create(User user);

      /// <summary>
      /// Crea una nueva cuenta de usuario.
      /// </summary>
      /// <param name="user">Una instancia de User con los datos de la cuenta a crear.</param>
      /// <param name="confirm">Indica si se desea confirmar la cuenta de correo electrónico vía correo electrónico.</param>
      public abstract void Create(User user, bool confirm);

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario.
      /// </summary>
      /// <param name="user">Una instancia de User con los datos actualizados de la cuenta.</param>
      /// <remarks>
      /// Para actualizar la contraseña debe usar el método SetPassword() ya que éste método no actualiza la contraseña.
      /// </remarks>
      public abstract void Update(User user);

      /// <summary>
      /// Elimina una cuenta de usuario.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta a eliminar.</param>
      public abstract void Delete(int uid);

      /// <summary>
      /// Cancela una cuenta de usuario.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta a cancelar.</param>
      public abstract void Cancel(int uid);

      /// <summary>
      /// Verifica una cuenta de usuario (pendiente de verificación por correo electrónico).
      /// </summary>
      /// <param name="uid">Identificador de la cuenta de usuario.</param>
      /// <param name="mail">Correo electrónico asociado a la cuenta.</param>
      /// <returns>Una instancia de User con los datos del usuario verificado.</returns>
      public abstract User Verify(int uid, string mail);

      /// <summary>
      /// Verifica una cuenta de usuario (pendiente de verificación por correo electrónico).
      /// </summary>
      /// <param name="QueryString">Una instancia de NameValueCollection que puede ser Server.Params.</param>
      /// <returns>Una instancia de User con los datos del usuario verificado.</returns>
      public abstract User Verify(NameValueCollection QueryString);

      /// <summary>
      /// Envia los datos de un usuario a su cuenta de correo para el acceso al Workspace
      /// </summary>
      /// <param name="address">Dirección de correo a la que se mandarán los datos. Debe coincidir con la dirección de una cuenta de usuario.</param>
      public abstract User SendData(string address);

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta de usuario.</param>
      /// <param name="newPassword">Nueva contraseña del usuario.</param>
      public abstract void SetPassword(int uid, string newPassword);

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
