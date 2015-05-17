using Cosmo.Communications;
using Cosmo.Diagnostics;
using Cosmo.Platform;
using Cosmo.Services;
using Cosmo.Utils.Cryptography;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace Cosmo.Security.Auth.Impl
{

   /// <summary>
   /// Proveedor de autenticación contra el grupo de usuarios del servidor Cosmo.
   /// </summary>
   /// <remarks>
   /// Este proveedor usa para la autenticación de usuario la base de datos del servidor.
   /// </remarks>
   [Obsolete]
   public class CosmoServerAuthenticationProvider : IAutheticationModule
   {
      private Workspace _ws = null;
      private Server _conn = null;

      private const int USER_OPTIONS_NOTIFY_INSIDE = 1;
      private const int USER_OPTIONS_NOTIFY_OUTSIDE = 2;
      private const int USER_OPTIONS_NOTIFY_PRIVATEMSGS = 4;
      private const int USER_OPTIONS_DISABLE_PRIVATEMSGS = 8;

      private const string USER_FIELDS = "usrid,usrlogin,usrmail,'' As usrmail2,usrname,'' As usrcity,0 As usrcountryid,'' As usrphone,usrdesc,15 As usroptions,2 As usrstatus,getdate() As usrlastlogon,0 As usrlogoncount,'SA',getdate() As usrcreated,usrpwd";

      /// <summary>
      /// Devuelve una instancia de Users
      /// </summary>
      public CosmoServerAuthenticationProvider() { }

      /// <summary>
      /// Devuelve una instancia de Users
      /// </summary>
      /// <param name="workspace">Workspace</param>
      public CosmoServerAuthenticationProvider(Workspace workspace)
      {
         _ws = workspace;

         _conn = new Server(false);
         _conn.ConnectionString = _ws.RegistryDSN;
      }

      #region Properties

      /// <summary>
      /// Establece el workspace para el que se está autenticando.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
         set 
         { 
            _ws = value;

            _conn = new Server(false);
            _conn.ConnectionString = _ws.RegistryDSN;
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve una lista de todos los usuarios
      /// </summary>
      /// <param name="status">Filtra por el estado de la cuenta</param>
      /// <returns>Una lista de objetos <see cref="User"/>.</returns>
      public List<User> GetUsersList(User.UserStatus status)
      {
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         List<User> users = new List<User>();

         try
         {
            // Abre una conexión a la BBDD
            _conn.Connect();

            string sql = "SELECT " + USER_FIELDS + " FROM users " +
                         (status != User.UserStatus.AllStates ? "WHERE usrstatus=@usrstatus " : string.Empty) +
                         "ORDER BY usrlogin";
            cmd = new SqlCommand(sql, _conn.Connection);
            if (status >= 0) cmd.Parameters.Add(new SqlParameter("@usrstatus", (int)status));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               User user = ReadUserFields(reader);
               users.Add(user);
            }
            reader.Close();

            return users;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.GetUsersList", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            if (!reader.IsClosed) reader.Close();

            reader.Dispose();
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve una lista de todos los usuarios
      /// </summary>
      /// <returns>Una lista de objetos WSUser</returns>
      public List<User> GetUsersList()
      {
         // return GetUsersList(User.UserStatus.Unknown);
         return GetUsersList(User.UserStatus.AllStates);
      }

      /// <summary>
      /// Obtiene los datos de una cuenta del workspace.
      /// </summary>
      /// <param name="login">Login de la cuenta de usuario.</param>
      /// <returns>Una instáncia MWUser.</returns>
      public User GetUser(string login)
      {
         string sql = string.Empty;
         User user = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _conn.Connect();

            sql = "SELECT " + USER_FIELDS + " FROM users " +
                  "WHERE Upper(usrlogin)=@usrlogin";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", login.Trim().ToUpper()));

            reader = cmd.ExecuteReader();
            if (reader.Read()) user = ReadUserFields(reader);
            reader.Close();

            // Obtiene las relaciones con otros usuarios
            /*sql = "SELECT uid,touserid,status,created,updated " +
                  "FROM sysusersrel " +
                  "WHERE uid=@uid Or touserid=@uid";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@uid", user.ID));

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               UserRelation relation = new UserRelation();
               relation.FromUserID = reader.GetInt32(0);
               relation.ToUserID = reader.GetInt32(2);
               relation.Status = (UserRelation.UserRelationStatus)reader.GetInt32(3);
               relation.Created = (reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4));
               relation.Updated = (reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5));
               user.Relations.Add(relation);
            }
            reader.Close();*/

            return user;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.GetUser", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene los datos de una cuenta del workspace.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <returns>Una instáncia MWUser.</returns>
      public User GetUser(int uid)
      {
         SqlCommand cmd = null;

         try
         {
            // Obtiene el LOGIN del usuario
            _conn.Connect();

            string sql = "SELECT usrlogin FROM users WHERE usrid=@usrid";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));

            string login = (string)cmd.ExecuteScalar();

            // Debe cerrar aquí la conexión para que el método Item() encuentre esta cerrada
            cmd.Dispose();
            _conn.Disconnect();

            if (login == null)
               return null;
            else
               return GetUser(login);
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.GetUser", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
      }

      /// <summary>
      /// Crea una nueva cuenta de usuario del workspace
      /// </summary>
      /// <param name="user">Instáncia de WSUser que contiene los datos de la nueva cuenta</param>
      /// <param name="confirm">Indica que al agregar el usuario se le mandará un correo electrónico para la confirmación de su cuenta de correo y mientras no se confirme, la cuenta estará en estado pendiente de verificación.</param>
      public void Create(User user, bool confirm)
      {
         string sql;
         SqlCommand cmd = null;
         SqlTransaction transaction = null;

         user.Login = user.Login.Trim();
         user.Mail = user.Mail.Trim().ToLower();
         user.Name = user.Name.Trim();
         user.City = user.City.Trim();
         user.Phone = user.Phone.Trim();
         user.MailAlternative = user.MailAlternative.Trim().ToLower();
         user.Description = user.Description.Trim();

         // Genera las opciones
         int options = 0;
         if (user.CanReceiveInternalMessages) options |= USER_OPTIONS_NOTIFY_INSIDE;
         if (user.CanReceiveExternalMessages) options |= USER_OPTIONS_NOTIFY_OUTSIDE;
         if (user.CanReceivePrivateMessagesNotify) options |= USER_OPTIONS_NOTIFY_PRIVATEMSGS;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _conn.Connect();

            // Inicia una transacción
            transaction = _conn.Connection.BeginTransaction();

            // Averigua si ya existe un usuario que usa el LOGIN proporcionado
            sql = "SELECT Count(*) AS nregs FROM users WHERE Lower(usrlogin)=@usrlogin";
            cmd = new SqlCommand(sql, _conn.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", user.Login.ToLower()));
            if ((int)cmd.ExecuteScalar() > 0)
               throw new Exception("El nombre de usuario " + user.Login.ToUpper() + " está siendo usado por otra cuenta de usuario. Por favor, elija otro.");

            // Averigua si ya existe un usuario que usa la cuenta de correo proporcionada
            sql = "SELECT Count(*) AS nregs FROM users WHERE Lower(usrmail)=@usrmail";
            cmd = new SqlCommand(sql, _conn.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@usrmail", user.Mail));
            if ((int)cmd.ExecuteScalar() > 0)
               throw new Exception("La dirección de correo " + user.Mail + " está siendo usado por otra cuenta de usuario.");

            // Añade el registro a la tabla USERS
            sql = "INSERT INTO users (usrlogin,usrmail,usrpwd,usrname,usrcity,usrcountryid,usrphone,usrmail2,usrdesc,usroptions,usrstatus,usrlogoncount,usrlastlogon,usrcreated,pin,usrowner) " +
                  "VALUES (@usrlogin,@usrmail,@usrpwd,@usrname,@usrcity,@usrcountryid,@usrphone,@usrmail2,@usrdesc,@usroptions,@usrstatus,0,getdate(),getdate(),'',@usrowner)";
            cmd = new SqlCommand(sql, _conn.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", user.Login));
            cmd.Parameters.Add(new SqlParameter("@usrmail", user.Mail));
            cmd.Parameters.Add(new SqlParameter("@usrpwd", user.Password));
            cmd.Parameters.Add(new SqlParameter("@usrname", user.Name));
            cmd.Parameters.Add(new SqlParameter("@usrcity", user.City));
            cmd.Parameters.Add(new SqlParameter("@usrcountryid", user.CountryID));
            cmd.Parameters.Add(new SqlParameter("@usrphone", user.Phone));
            cmd.Parameters.Add(new SqlParameter("@usrmail2", user.MailAlternative));
            cmd.Parameters.Add(new SqlParameter("@usrdesc", user.Description));
            cmd.Parameters.Add(new SqlParameter("@usroptions", options));
            cmd.Parameters.Add(new SqlParameter("@usrstatus", (confirm ? (int)User.UserStatus.NotVerified : (int)user.Status)));
            cmd.Parameters.Add(new SqlParameter("@usrowner", (string.IsNullOrEmpty(user.Owner) ? AuthenticationFactory.ACCOUNT_SUPER : user.Owner)));
            cmd.ExecuteNonQuery();

            // Averigua el ID del usuario
            sql = "SELECT usrid AS id FROM users WHERE Lower(usrlogin)=@usrlogin";
            cmd = new SqlCommand(sql, _conn.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", user.Login.ToLower()));
            user.ID = (int)cmd.ExecuteScalar();

            // if (confirm)
            if (_ws.Settings.GetBoolean(WorkspaceSettingsKeys.SecurityMailVerification))
            {
               _ws.MailServer.Send(_ws.Authentication.GetVerificationMail(user.ID));
            }

            // Confirma la trasacción
            transaction.Commit();
         }
         catch (Exception ex)
         {
            if (transaction != null) transaction.Rollback();

            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.Create", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            transaction.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Crea una nueva cuenta de usuario del workspace
      /// </summary>
      /// <param name="user">Instáncia de WSUser que contiene los datos de la nueva cuenta</param>
      public void Create(User user)
      {
         Create(user, false);
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="user">Instáncia de MWUser que contiene los datos actualizados de la cuenta.</param>
      /// <remarks>
      /// Para actualizar la contraseña debe usar el método SetPassword() ya que éste método no
      /// actualiza la contraseña.
      /// </remarks>
      public void Update(User user)
      {
         string sql;
         SqlCommand cmd = null;

         user.Login = user.Login.Trim();
         user.Mail = user.Mail.Trim().ToLower();
         user.Name = user.Name.Trim();
         user.City = user.City.Trim();
         user.Phone = user.Phone.Trim();
         user.MailAlternative = user.MailAlternative.Trim().ToLower();
         user.Description = user.Description.Trim();

         // Genera las opciones
         int options = 0;
         if (user.CanReceiveInternalMessages) options |= USER_OPTIONS_NOTIFY_INSIDE;
         if (user.CanReceiveExternalMessages) options |= USER_OPTIONS_NOTIFY_OUTSIDE;
         if (user.CanReceivePrivateMessagesNotify) options |= USER_OPTIONS_NOTIFY_PRIVATEMSGS;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _conn.Connect();

            // Inicia una transacción
            // _workspace.Connection.BeginTransaction();

            // Averigua si ya existe un usuario que usa la cuenta de correo proporcionada
            sql = "SELECT Count(*) AS nregs " +
                  "FROM users " +
                  "WHERE Lower(usrmail)=@usrmail And Lower(usrlogin)<>@usrlogin";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrmail", user.Mail));
            cmd.Parameters.Add(new SqlParameter("@usrlogin", user.Login.ToLower()));
            if ((int)cmd.ExecuteScalar() > 0)
               throw new Exception("La dirección de correo " + user.Mail + " está siendo usado por otra cuenta de usuario.");

            // Añade el registro a la tabla USERS
            sql = "UPDATE users " +
                  "SET usrmail=@usrmail," +
                      "usrname=@usrname," +
                      "usrcity=@usrcity," +
                      "usrcountryid=@usrcountryid," +
                      "usrphone=@usrphone," +
                      "usrmail2=@usrmail2," +
                      "usrdesc=@usrdesc," +
                      "usroptions=@usroptions," +
                      "usrstatus=@usrstatus " +
                  "WHERE usrid=@usrid";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrmail", user.Mail));
            cmd.Parameters.Add(new SqlParameter("@usrname", user.Name));
            cmd.Parameters.Add(new SqlParameter("@usrcity", user.City));
            cmd.Parameters.Add(new SqlParameter("@usrcountryid", user.CountryID));
            cmd.Parameters.Add(new SqlParameter("@usrphone", user.Phone));
            cmd.Parameters.Add(new SqlParameter("@usrmail2", user.MailAlternative));
            cmd.Parameters.Add(new SqlParameter("@usrdesc", user.Description));
            cmd.Parameters.Add(new SqlParameter("@usroptions", options));
            cmd.Parameters.Add(new SqlParameter("@usrstatus", (int)user.Status));
            cmd.Parameters.Add(new SqlParameter("@usrid", user.ID));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.Update", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Elimina una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="id">Identificador de la cuenta a eliminar</param>
      public void Delete(int id)
      {
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _conn.Connect();

            // Elimina el registro
            cmd = new SqlCommand("DELETE FROM users WHERE usrid=@usrid", _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", id));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.Delete", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Cancela una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta a cancelar</param>
      public void Cancel(int uid)
      {
         SqlParameter param = null;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _conn.Connect();

            string sql = "UPDATE users " +
                         "SET usrdisabled = 1 " +
                         "WHERE usrid = @usrid";

            // Elimina el registro
            cmd = new SqlCommand(sql, _conn.Connection);

            param = new SqlParameter("@usrid", System.Data.SqlDbType.Int);
            param.Value = uid;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();

            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.Cancel", "La cuenta " + uid + " ha sido cancelada.", LogEntry.LogEntryType.EV_SECURITY));
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("Cosmo.Workspace.Authentication.CosmoServerAuthenticationProvider.Cancel", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Verifica una cuenta de usuario (pasa del estado 'No verificada' a 'Activa'). Este método permite implementar
      /// un sistema de generación de cuentas por parte de los usuarios (registro de usuarios).
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="mail">Cuenta de correo del usuario</param>
      public User Verify(int uid, string mail)
      {
         string sql;
         SqlCommand cmd = null;

         mail = mail.Trim().ToLower();

         try
         {
            // Abre una conexión a la BBDD
            _conn.Connect();

            // Busca la existencia del usuario
            sql = "SELECT Count(*) AS nregs FROM users WHERE usrid=@usrid AND Lower(usrmail)=@usrmail";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            cmd.Parameters.Add(new SqlParameter("@usrmail", mail));
            if ((int)cmd.ExecuteScalar() <= 0)
               throw new Exception("La dirección de correo electrónico " + mail + " proporcionada no corresponde a ningúna suscripción.");

            // Comprueba que no sea una cuenta bloqueada
            sql = "SELECT usrstatus FROM users WHERE usrid=@usrid";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            if ((int)cmd.ExecuteScalar() <= (int)User.UserStatus.Disabled)
               throw new Exception("La suscripción asociada a la dirección de correo electrónico " + mail + " ha sido bloqueada por el administrador de sistemas y la suscripción cancelada.");

            // Verifica la cuenta
            sql = "UPDATE users " +
                  "SET usrstatus=" + (int)User.UserStatus.Enabled + ", " +
                      "usrlastlogon=GetDate() " +
                  "WHERE usrid=@usrid";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            cmd.ExecuteNonQuery();

            return GetUser(uid);
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.Verify", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            
            throw new Exception("Error verificando nueva cuenta: " + ex.Message, ex);
         }
         finally
         {
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Verifica una cuenta de usuario (pasa del estado 'No verificada' a 'Activa'). Este método permite implementar
      /// un sistema de generación de cuentas por parte de los usuarios (registro de usuarios).
      /// </summary>
      /// <param name="QueryString">Colección de parámetros del enlace de verificación mandado por correo electrónico al usuario (Request.QueryString).</param>
      public User Verify(NameValueCollection QueryString)
      {
         string data = UriCryptography.Decrypt(QueryString["data"].Replace(" ", "+"), _ws.Settings.GetString(WorkspaceSettingsKeys.SecurityEncryptionKey));
         NameValueCollection parameters = GetQuerystringValues(data);

         return Verify(int.Parse(parameters["id"]), parameters["obj"]);
      }

      /// <summary>
      /// Envia los datos de un usuario a su cuenta de correo para el acceso al Workspace
      /// </summary>
      /// <param name="address">Dirección de correo a la que se mandarán los datos. Debe coincidir con la dirección de una cuenta de usuario.</param>
      /// <returns>La instáncia de <see cref="User"/> con los datos del usuario que realiza la petición.</returns>
      public User SendData(string address)
      {
         int id = 0;
         string sql = string.Empty;
         SqlCommand cmd = null;

         // Formatea parámetros
         address = address.Trim().ToLower();

         try
         {
            // Abre una conexión a la BBDD del workspace
            _conn.Connect();

            // Averigua el ID del usuario a partir del mail
            try
            {
               sql = "SELECT usrid FROM users WHERE Lower(usrmail)=@usrmail";
               cmd = new SqlCommand(sql, _conn.Connection);
               cmd.Parameters.Add(new SqlParameter("@usrmail", address));
               id = (int)cmd.ExecuteScalar();
            }
            catch
            {
               // Si no está registrada, lanza una excepción
               throw new InvalidMailException("La cuenta de correo proporcionada no corresponde a ninguna cuenta de usuario creada en este servidor.");
            }

            // Obtiene la cuenta de usuario
            User user = GetUser(id);

            _ws.MailServer.Send(_ws.Authentication.GetUserDataMail(user.ID));

            return user;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("Cosmo.Workspace.Authentication.CosmoServerAuthenticationProvider.SendData", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Permite verificar las credenciales de acceso a una cuenta de usuario (login).
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <param name="password">Contraseña del usuario.</param>
      public User Autenticate(string login, string password)
      {
         string sql;
         SqlCommand cmd = null;

         login = login.Trim().ToUpper();

         try
         {
            // Abre una conexión a la BBDD
            _conn.Connect();

            // Busca la existencia del usuario
            sql = "SELECT Count(*) AS nregs FROM users WHERE Upper(usrlogin)=@usrlogin AND usrpwd=@usrpwd";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", login));
            cmd.Parameters.Add(new SqlParameter("@usrpwd", password));
            if ((int)cmd.ExecuteScalar() <= 0)
               throw new Exception("Login o contraseña erróneos.");

            // Actualiza la fecha del último acceso
            /* sql = "UPDATE users " +
                  "SET usrlastlogon=GetDate()," +
                      "usrlogoncount=usrlogoncount+1 " +
                  "WHERE Upper(usrlogin)=@usrlogin";
            cmd = new SqlCommand(sql, _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", login));
            cmd.ExecuteNonQuery(); */

            // Obtiene la cuenta de usuario
            User user = GetUser(login);

            // Comprueba si se trata de una cuenta deshabilitada o sin verificar
            switch (user.Status)
            {
               case User.UserStatus.Disabled:
                  throw new UserDisabledException();

               case User.UserStatus.NotVerified:
                  throw new UserNotVerifiedException();

               default:
                  return user;
            }

         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.Autenticate", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta de usuario.</param>
      /// <param name="newPassword">Nueva contraseña del usuario.</param>
      public void SetPassword(int uid, string newPassword)
      {
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _conn.Connect();

            // Añade el registro a la tabla USERS
            cmd = new SqlCommand("UPDATE users SET usrpwd=@usrpwd WHERE usrid=@usrid", _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrpwd", newPassword));
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.SetPassword", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta de usuario.</param>
      /// <param name="oldPassword">Contraseña actual.</param>
      /// <param name="newPassword">Nueva contraseña del usuario.</param>
      /// <param name="newPasswordVerification">Verificación de la nueva contraseña.</param>
      /// <remarks>
      /// Los parámetros newPassword y newPasswordVerification deben coincidir o de lo contrario se lanzará la excepción <see cref="Cosmo.Workspace.CosmoSecurityException"/>.
      /// </remarks>
      public void SetPassword(int uid, string oldPassword, string newPassword, string newPasswordVerification)
      {
         SqlCommand cmd = null;

         // Verifica que la nueva contraseña y su verificación sean iguales
         if (!newPassword.Equals(newPasswordVerification))
            throw new SecurityException("La nueva contraseña no coincide con la verificación. La contraseña no se ha modificado.");

         try
         {
            // Abre una conexión a la BBDD del workspace
            _conn.Connect();

            // Obtiene la contraseña actual y verifica su validez
            cmd = new SqlCommand("SELECT usrpwd FROM users WHERE WHERE usrid=@usrid", _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            string currentpwd = (string)cmd.ExecuteScalar();

            if (oldPassword != currentpwd)
               throw new SecurityException("La contraseña actual es incorrecta. La contraseña no se ha modificado.");

            // Añade el registro a la tabla USERS
            cmd = new SqlCommand("UPDATE users SET usrpwd=@usrpwd WHERE usrid=@usrid", _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrpwd", newPassword));
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.SetPassword", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Verifica una contraseña de usuario.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="password">Contraseña a verificar.</param>
      /// <returns>Devuelve <c>true</c> si la contraseña coincide o <c>false</c> en cualquier otro caso.</returns>
      public bool CheckPassword(int uid, string password)
      {
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _conn.Connect();

            // Obtiene la contraseña actual y verifica su validez
            cmd = new SqlCommand("SELECT usrpwd FROM users WHERE usrid=@usrid", _conn.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            string currentpwd = (string)cmd.ExecuteScalar();

            return password.Equals(currentpwd);
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.CheckPassword", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene una lista de los roles de un usuario.
      /// </summary>
      /// <param name="uid">Identificador único del usuario.</param>
      /// <returns>La lista de instancias de <see cref="Role"/> solicitada.</returns>
      public List<Role> GetUserRoles(int uid)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Obtiene una lista de los roles de un usuario.
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <returns>La lista de instancias de <see cref="Role"/> solicitada.</returns>
      public List<Role> GetUserRoles(string login)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Devuelve una lista de usuarios que tienen un rol específico.
      /// </summary>
      /// <param name="roleId">Identificador del rol.</param>
      /// <returns>
      /// Una inatsnacia de <see cref="System.Collections.Generic.List&lt;T&gt;"/> que contiene las inatancias de <see cref="Cosmo.Workspace.User"/> que 
      /// pretenecen al rol solicitado.
      /// </returns>
      public List<User> GetRoleUsers(int roleId)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Selecciona un grupo de usuarios
      /// </summary>
      /// <param name="selector">Selector que contiene los critérios de búsqueda</param>
      /// <returns>La lista de usuarios seleccioandos</returns>
      public List<User> Search(UserSearchCriteria selector)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         List<User> find = new List<User>();

         try
         {
            _conn.Connect();

            sql = "SELECT " + USER_FIELDS + " FROM users " +
                  this.SelectorSQL(selector) +
                  "ORDER BY usrlogin Asc";
            cmd = new SqlCommand(sql, _conn.Connection);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
               find.Add(ReadUserFields(reader));
            }
            reader.Close();

            return find;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.Search", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            if (!reader.IsClosed) reader.Close();

            reader.Dispose();
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Busca usuarios del workspace.
      /// </summary>
      /// <param name="login">Login o fracción a buscar</param>
      /// <param name="city">Ciudad o fracción a buscar</param>
      /// <param name="countryId">Identificador del pais a buscar</param>
      /// <returns></returns>
      public List<User> Search(string login, string city, int countryId)
      {
         List<User> users = new List<User>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         if (login.Equals(string.Empty) && city.Equals(string.Empty) && countryId == 0)
            throw new Exception("No ha proporcionado ningún criterio de búsqueda.");

         if (login.Equals(string.Empty) && city.Equals(string.Empty))
            throw new Exception("Debe proporcionar un login (o fracción) o una ciudad (o fracción) para iniciar la búsqueda.");

         string condition = string.Empty;
         condition += (string.IsNullOrEmpty(login) ? string.Empty : (!string.IsNullOrEmpty(condition) ? " And " : string.Empty) + "(Lower(usrlogin) Like Lower('%" + login.Trim() + "%') Or Lower(usrname) Like Lower('%" + login.Trim() + "%'))");
         condition += (string.IsNullOrEmpty(city) ? string.Empty : (!string.IsNullOrEmpty(condition) ? " And " : string.Empty) + "Lower(usrcity) Like Lower('%" + city.Trim() + "%')");
         condition += (countryId <= 0 ? string.Empty : (!string.IsNullOrEmpty(condition) ? " And " : string.Empty) + "usrcountryid=" + countryId);
         if (!string.IsNullOrEmpty(condition)) condition = "WHERE " + condition;

         string sql = "SELECT TOP 50 " + USER_FIELDS + " " +
                      "FROM users " + condition + " ORDER BY usrlogin Asc";

         try
         {
            // Abre una conexión a la BBDD
            _conn.Connect();

            cmd = new SqlCommand(sql, _conn.Connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               users.Add(ReadUserFields(reader));
            }

            return users;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CosmoServerAuthenticationProvider.Search", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            if (!reader.IsClosed) reader.Close();

            reader.Dispose();
            cmd.Dispose();
            _conn.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve una lista de paises
      /// </summary>
      public List<Country> ListCountry()
      {
         CountryDAO cdao = new CountryDAO(_ws);
         return cdao.GetCountryList();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Devuelve una instancia de MWUser con los datos recuperados de un objeto SqlDataReader que apunte a un registro de la tabla USERS
      /// </summary>
      /// <param name="reader">Objeto SqlDataReader que apunte a un registro válido</param>
      /// <returns>Una instancia de WSUser</returns>
      /// <remarks>
      /// Para usar este método la senténcia SQL debe contener los campos indicados en la constante USER_FIELDS
      /// </remarks>
      private User ReadUserFields(SqlDataReader reader)
      {
         try
         {
            User user = new User();
            user.ID = reader.GetInt32(0);
            user.Login = reader.GetString(1);
            user.Mail = reader.GetString(2);
            user.MailAlternative = !reader.IsDBNull(3) ? reader.GetString(3) : string.Empty;
            user.Name = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty;
            user.City = !reader.IsDBNull(5) ? reader.GetString(5) : string.Empty;
            user.CountryID = reader.GetInt32(6);
            user.Phone = !reader.IsDBNull(7) ? reader.GetString(7) : string.Empty;
            user.Description = !reader.IsDBNull(8) ? reader.GetString(8) : string.Empty;
            user.CanReceiveInternalMessages = Convert.ToBoolean(reader.GetInt32(9) & USER_OPTIONS_NOTIFY_INSIDE);
            user.CanReceiveExternalMessages = Convert.ToBoolean(reader.GetInt32(9) & USER_OPTIONS_NOTIFY_OUTSIDE);
            user.CanReceivePrivateMessagesNotify = Convert.ToBoolean(reader.GetInt32(9) & USER_OPTIONS_NOTIFY_PRIVATEMSGS);
            user.DisablePrivateMessages = Convert.ToBoolean(reader.GetInt32(9) & USER_OPTIONS_DISABLE_PRIVATEMSGS);
            user.Status = (User.UserStatus)reader.GetInt32(10);
            user.LastLogon = reader.GetDateTime(11);
            user.LogonCount = reader.GetInt32(12);
            user.Owner = !reader.IsDBNull(13) ? reader.GetString(13) : AuthenticationFactory.ACCOUNT_SUPER;
            user.Created = reader.GetDateTime(14);
            user.Password = !reader.IsDBNull(15) ? reader.GetString(15) : string.Empty;

            // TODO: Encriptar la contraseña de los usuarios del workspace
            // Desencripta la contraseña
            // user.Password = Cryptography.Decrypt(user.Password, user.Login.ToLower(), true);

            return user;
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Genera la clausula WHERE correspondiente a un filtro para seleccionar usuarios
      /// </summary>
      /// <param name="selector"></param>
      /// <returns></returns>
      private string SelectorSQL(UserSearchCriteria selector)
      {
         string sql = string.Empty;
         string query = string.Empty;

         // Compone la sentencia SQL
         if (selector.CountryId > 0) query += "usrcountryid=" + selector.CountryId;
         if (!selector.City.Trim().Equals(string.Empty))
         {
            if (!query.Equals(string.Empty)) query += " And ";
            query += "Lower(usrcity)='" + selector.City.Trim().ToLower() + "'";
         }
         if (selector.FromLastLogon > DateTime.MinValue)
         {
            if (!query.Equals(string.Empty)) query += " And ";
            query += "usrlastlogon>'" + selector.FromLastLogon.ToString("yyyyMMdd") + "'";
         }
         if (selector.ToLastLogon > DateTime.MinValue)
         {
            if (!query.Equals(string.Empty)) query += " And ";
            query += "usrlastlogon<'" + selector.ToLastLogon.ToString("yyyyMMdd") + "'";
         }
         if (selector.InternalMessages)
         {
            if (!query.Equals(string.Empty)) query += " And ";
            query += "(usroptions & " + USER_OPTIONS_NOTIFY_INSIDE + "=" + USER_OPTIONS_NOTIFY_INSIDE + ")";
         }
         if (selector.ExternalMessages)
         {
            if (!query.Equals(string.Empty)) query += " And ";
            query += "(usroptions & " + USER_OPTIONS_NOTIFY_OUTSIDE + "=" + USER_OPTIONS_NOTIFY_OUTSIDE + ")";
         }
         if (!query.Equals(string.Empty)) sql += "WHERE " + query.Trim() + " ";

         return sql;
      }

      private System.Collections.Specialized.NameValueCollection GetQuerystringValues(string url)
      {
         // Contempla la posibilidad de pasar únicamente el QueryString sin la Uri
         if (!url.Contains("?"))
            url = "data@data.com?" + url;

         string[] parts = url.Split("?".ToCharArray());

         System.Collections.Specialized.NameValueCollection qs = new System.Collections.Specialized.NameValueCollection();
         // qs. ._document = parts[0];

         if (parts.Length == 1)
            return qs;

         string[] keys = parts[1].Split("&".ToCharArray());
         foreach (string key in keys)
         {
            string[] part = key.Split("=".ToCharArray());
            if (part.Length == 1) qs.Add(part[0], string.Empty);
            qs.Add(part[0], part[1]);
         }

         return qs;
      }

      #endregion

   }
}
