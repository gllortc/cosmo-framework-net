using Cosmo.Diagnostics;
using Cosmo.Security.Cryptography;
using Cosmo.Services;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace Cosmo.Security.Auth.Impl
{
   /// <summary>
   /// Proveedor de autenticación nativo de Cosmo.
   /// </summary>
   /// <remarks>
   /// Este proveedor usa para la autenticación de usuario la base de datos del workspace (tabla USERS).
   /// </remarks>
   public class SqlSecurityModule : SecurityModule
   {
      // Internal data declarations
      private Workspace _ws = null;

      private const int USER_OPTIONS_NOTIFY_INSIDE = 1;
      private const int USER_OPTIONS_NOTIFY_OUTSIDE = 2;
      private const int USER_OPTIONS_NOTIFY_PRIVATEMSGS = 4;
      private const int USER_OPTIONS_DISABLE_PRIVATEMSGS = 8;

      private const string SQL_USER_TABLE = "users";
      private const string SQL_USER_FIELDS = "usrid,usrlogin,usrmail,usrmail2,usrname,usrcity,usrcountryid,usrphone,usrdesc,usroptions,usrstatus,usrlastlogon,usrlogoncount,usrowner,usrcreated,usrpwd,usrroles";
      private const string SQL_ROLE_FIELDS = "rolid,rolname,roldescription";

      #region Constructors

      /// <summary>
      /// Gets a new instance of Users.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public SqlSecurityModule(Workspace workspace, Plugin plugin)
         : base(workspace, plugin)
      {
         _ws = workspace;
      }

      #endregion

      #region IAuthenticationModule Implementation

      /// <summary>
      /// Devuelve una lista de todos los usuarios.
      /// </summary>
      /// <param name="status">Filtra por el estado de la cuenta.</param>
      /// <returns>Una lista de objetos <see cref="User"/>.</returns>
      public override List<User> GetUsersList(User.UserStatus status)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         List<User> users = new List<User>();

         try
         {
            // Abre una conexión a la BBDD
            _ws.DataSource.Connect();

            sql = "SELECT    " + SQL_USER_FIELDS + " " +
                  "FROM      " + SQL_USER_TABLE + " " +
                  ((int)status < 3 ? "WHERE usrstatus=@usrstatus " : string.Empty) +
                  "ORDER BY  usrlogin";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            if (status >= 0) cmd.Parameters.Add(new SqlParameter("@usrstatus", (int)status));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  User user = ReadUserFields(reader);
                  users.Add(user);
               }
            }

            return users;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetUsersList", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve una lista de todos los usuarios.
      /// </summary>
      /// <returns>Una lista de objetos <see cref="User"/>.</returns>
      public override List<User> GetUsersList()
      {
         return GetUsersList(User.UserStatus.AllStates);
      }

      /// <summary>
      /// Obtiene los datos de una cuenta del workspace.
      /// </summary>
      /// <param name="login">Login de la cuenta de usuario.</param>
      /// <returns>Una instáncia <see cref="User"/>.</returns>
      public override User GetUser(string login)
      {
         string sql = string.Empty;
         User user = null;
         SqlCommand cmd = null;

         if (string.IsNullOrWhiteSpace(login))
         {
            return null;
         }

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            sql = "SELECT " + SQL_USER_FIELDS + " " +
                  "FROM " + SQL_USER_TABLE + " " +
                  "WHERE Upper(usrlogin) = @usrlogin";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", login.Trim().ToUpper()));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               if (reader.Read())
               {
                  user = ReadUserFields(reader);
               }
            }

            // If user not found, return null
            if (user == null)
            {
               return null;
            }

            // Get relations with other users Obtiene las relaciones con otros usuarios
            sql = @"SELECT userid, touserid, status, created, updated 
                    FROM   sysusersrel 
                    WHERE  userid = @userid Or 
                           touserid = @userid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@userid", user.ID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
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
            }

            return user;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetUser(string)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene los datos de una cuenta del workspace.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <returns>Una instáncia <see cref="User"/>.</returns>
      public override User GetUser(int uid)
      {
         return GetUser(ConvertUidToLogin(uid));
      }

      /// <summary>
      /// Crea una nueva cuenta de usuario del workspace
      /// </summary>
      /// <param name="user">Instáncia de <see cref="User"/> que contiene los datos de la nueva cuenta</param>
      /// <param name="confirm">Indica que al agregar el usuario se le mandará un correo electrónico para la confirmación de su cuenta de correo y mientras no se confirme, la cuenta estará en estado pendiente de verificación.</param>
      public override void Create(User user, bool confirm)
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
         if (user.DisablePrivateMessages) options |= USER_OPTIONS_DISABLE_PRIVATEMSGS;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            // Inicia una transacción
            transaction = _ws.DataSource.Connection.BeginTransaction();

            // Averigua si ya existe un usuario que usa el LOGIN proporcionado
            sql = "SELECT Count(*) AS nregs " +
                  "FROM " + SQL_USER_TABLE + " " +
                  "WHERE Lower(usrlogin)=@usrlogin";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", user.Login.ToLower()));
            if ((int)cmd.ExecuteScalar() > 0)
            {   
               throw new Exception("El nombre de usuario " + user.Login.ToUpper() + " está siendo usado por otra cuenta de usuario. Por favor, elija otro.");
            }

            // Averigua si ya existe un usuario que usa la cuenta de correo proporcionada
            sql = "SELECT Count(*) AS nregs " +
                  "FROM " + SQL_USER_TABLE + " " +
                  "WHERE Lower(usrmail)=@usrmail";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@usrmail", user.Mail));
            if ((int)cmd.ExecuteScalar() > 0)
            {   
               throw new Exception("La dirección de correo " + user.Mail + " está siendo usado por otra cuenta de usuario.");
            }

            // Añade el registro a la tabla USERS
            sql = "INSERT INTO " + SQL_USER_TABLE + " (usrlogin,usrmail,usrpwd,usrname,usrcity,usrcountryid,usrphone,usrmail2,usrdesc,usroptions,usrstatus,usrlogoncount,usrlastlogon,usrcreated,pin,usrowner) " +
                  "VALUES (@usrlogin,@usrmail,@usrpwd,@usrname,@usrcity,@usrcountryid,@usrphone,@usrmail2,@usrdesc,@usroptions,@usrstatus,0,getdate(),getdate(),'',@usrowner)";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
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
            cmd.Parameters.Add(new SqlParameter("@usrowner", (string.IsNullOrEmpty(user.Owner) ? SecurityService.ACCOUNT_SUPER : user.Owner)));
            cmd.ExecuteNonQuery();

            // Averigua el ID del usuario
            sql = "SELECT usrid AS id " +
                  "FROM " + SQL_USER_TABLE + " " +
                  "WHERE Lower(usrlogin)=@usrlogin";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", user.Login.ToLower()));
            user.ID = (int)cmd.ExecuteScalar();

            // Confirma la trasacción
            transaction.Commit();
         }
         catch (Exception ex)
         {
            if (transaction != null) transaction.Rollback();

            _ws.Logger.Add(new LogEntry(GetType().Name + ".Create(User,bool)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            transaction.Dispose();
            _ws.DataSource.Disconnect();
         }

         // Si es preciso, manda el correo de confirmación al finalizar la inserción y fuera de la transacción
         if (_ws.SecurityService.IsVerificationMailRequired)
         {
            _ws.Communications.Send(GetVerificationMail(user.ID));
         }
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="user">Instáncia de <see cref="User"/> que contiene los datos actualizados de la cuenta.</param>
      /// <remarks>
      /// Para actualizar la contraseña debe usar el método <c>SetPassword()</c> ya que éste método no 
      /// actualiza la contraseña.
      /// </remarks>
      public override void Update(User user)
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
         if (user.DisablePrivateMessages) options |= USER_OPTIONS_DISABLE_PRIVATEMSGS;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            // Inicia una transacción
            // _workspace.Connection.BeginTransaction();

            // Averigua si ya existe un usuario que usa la cuenta de correo proporcionada
            sql = "SELECT Count(*) AS nregs " +
                  "FROM " + SQL_USER_TABLE + " " +
                  "WHERE Lower(usrmail)=@usrmail And Lower(usrlogin)<>@usrlogin";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrmail", user.Mail));
            cmd.Parameters.Add(new SqlParameter("@usrlogin", user.Login.ToLower()));
            if ((int)cmd.ExecuteScalar() > 0)
            {   
               throw new Exception("La dirección de correo " + user.Mail + " está siendo usado por otra cuenta de usuario.");
            }

            // Actualiza el registro en la tabla USERS
            sql = "UPDATE " + SQL_USER_TABLE + " " +
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
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
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
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Update(User)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta a eliminar</param>
      public override void Delete(int uid)
      {
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            // Elimina el registro
            cmd = new SqlCommand("DELETE FROM " + SQL_USER_TABLE + " " +
                                 "WHERE usrid=@usrid", _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            cmd.ExecuteNonQuery();

            _ws.Logger.Add(new LogEntry(GetType().Name + ".Delete", 
                                        "La cuenta " + uid + " ha sido eliminada", 
                                        LogEntry.LogEntryType.EV_SECURITY));
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Delete(int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Cancela una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta a cancelar</param>
      public override void Cancel(int uid)
      {
         SqlParameter param = null;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            string sql = "UPDATE " + SQL_USER_TABLE + " " +
                         "SET usrmail   = ('--' + mail + '--'), " +
                             "usrpwd    = '', " +
                             "usrname   = usrlogin, " +
                             "usrcity   = '', " +
                             "usrphone  = '', " +
                             "usrmail2  = '', " +
                             "usrdesc   = '', " +
                             "usrstatus = @usrstatus " +
                         "WHERE usrid = @usrid";

            // Elimina el registro
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            
            param = new SqlParameter("@usrstatus", System.Data.SqlDbType.Int);
            param.Value = User.UserStatus.Disabled;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@usrid", System.Data.SqlDbType.Int);
            param.Value = uid;
            cmd.Parameters.Add(param);
            
            cmd.ExecuteNonQuery();

            _ws.Logger.Add(new LogEntry(GetType().Name + ".Cancel", 
                                        "La cuenta " + uid + " ha sido cancelada.", 
                                        LogEntry.LogEntryType.EV_SECURITY));
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Cancel(int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Verifica una cuenta de usuario (pasa del estado 'No verificada' a 'Activa'). Este método permite implementar
      /// un sistema de generación de cuentas por parte de los usuarios (registro de usuarios).
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="mail">Cuenta de correo del usuario</param>
      public override User Verify(int uid, string mail)
      {
         string sql;
         SqlCommand cmd = null;

         mail = mail.Trim().ToLower();

         try
         {
            // Abre una conexión a la BBDD
            _ws.DataSource.Connect();

            // Busca la existencia del usuario
            sql = "SELECT Count(*) AS nregs " +
                  "FROM " + SQL_USER_TABLE + " " +
                  "WHERE usrid=@usrid AND Lower(usrmail)=@usrmail";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            cmd.Parameters.Add(new SqlParameter("@usrmail", mail));
            if ((int)cmd.ExecuteScalar() <= 0)
            {
               throw new UserNotFoundException("La dirección de correo electrónico <strong>" + mail + "</strong> proporcionada no corresponde a ningúna suscripción.");
            }

            // Comprueba que no sea una cuenta bloqueada
            sql = "SELECT usrstatus " + 
                  "FROM " + SQL_USER_TABLE + " " +
                  "WHERE usrid=@usrid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            if ((int)cmd.ExecuteScalar() <= (int)User.UserStatus.Disabled)
            {
               throw new UserDisabledException("La dirección de correo electrónico <strong>" + mail + "</strong> ha sido bloqueada por el administrador de sistemas y la suscripción cancelada.");
            }

            // Verifica la cuenta
            sql = "UPDATE " + SQL_USER_TABLE + " " +
                  "SET usrstatus=" + (int)User.UserStatus.Enabled + ", " +
                      "usrlastlogon=GetDate() " +
                  "WHERE usrid=@usrid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            cmd.ExecuteNonQuery();

            return GetUser(uid);
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Verify(int,string)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            
            throw new Exception("Error verificando nueva cuenta: " + ex.Message, ex);
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Verifica una cuenta de usuario (pasa del estado 'No verificada' a 'Activa'). Este método permite implementar
      /// un sistema de generación de cuentas por parte de los usuarios (registro de usuarios).
      /// </summary>
      /// <param name="QueryString">Colección de parámetros del enlace de verificación mandado por correo electrónico al usuario (Request.QueryString).</param>
      public override User Verify(NameValueCollection QueryString)
      {
         string data = UriCryptography.Decrypt(QueryString["data"].Replace(" ", "+"), _ws.SecurityService.EncriptionKey);
         NameValueCollection parameters = GetQuerystringValues(data);

         return Verify(int.Parse(parameters["id"]), parameters["obj"]);
      }

      /// <summary>
      /// Envia los datos de un usuario a su cuenta de correo para el acceso al Workspace
      /// </summary>
      /// <param name="address">Dirección de correo a la que se mandarán los datos. Debe coincidir con la dirección de una cuenta de usuario.</param>
      /// <returns>La instáncia de <see cref="User"/> con los datos del usuario que realiza la petición.</returns>
      public override User SendData(string address)
      {
         int id = 0;
         string sql = string.Empty;
         SqlCommand cmd = null;

         // Formatea parámetros
         address = address.Trim().ToLower();

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            // Averigua el ID del usuario a partir del mail
            try
            {
               sql = "SELECT usrid " +
                     "FROM " + SQL_USER_TABLE + " " +
                     "WHERE Lower(usrmail)=@usrmail";
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@usrmail", address));
               id = (int)cmd.ExecuteScalar();
            }
            catch
            {
               // Si no está registrada, lanza una excepción
               throw new UserNotFoundException("La cuenta de correo proporcionada no corresponde a ninguna cuenta de usuario creada en este servidor.");
            }

            // Obtiene la cuenta de usuario
            User user = GetUser(id);
            if (user.Status == User.UserStatus.Disabled || user.Status == User.UserStatus.SecurityBloqued)
            {
               // Si no está registrada, lanza una excepción
               throw new UserNotFoundException("La cuenta de correo proporcionada no corresponde a ninguna cuenta de usuario creada en este servidor.");
            }

            // Envia el correo al usuario
            _ws.Communications.Send(GetUserDataMail(user));

            return user;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".SendData(string)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Permite verificar las credenciales de acceso a una cuenta de usuario (login).
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <param name="password">Contraseña del usuario.</param>
      /// <returns>
      /// Una instancia de <see cref="User"/> que representa el usuario autenticado.
      /// </returns>
      public override User Autenticate(string login, string password)
      {
         string sql;
         SqlCommand cmd = null;

         login = login.Trim().ToUpper();

         try
         {
            // Abre una conexión a la BBDD
            _ws.DataSource.Connect();

            // Busca la existencia del usuario
            sql = "SELECT Count(*) AS nregs " +
                  "FROM " + SQL_USER_TABLE + " " +
                  "WHERE Upper(usrlogin)=@usrlogin AND usrpwd=@usrpwd";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", login));
            cmd.Parameters.Add(new SqlParameter("@usrpwd", password));
            if ((int)cmd.ExecuteScalar() <= 0)
            {
               throw new AuthenticationException("Login o contraseña erróneos.");
            }

            // Actualiza la fecha del último acceso
            sql = "UPDATE " + SQL_USER_TABLE + " " +
                  "SET usrlastlogon=GetDate()," +
                      "usrlogoncount=usrlogoncount+1 " +
                  "WHERE Upper(usrlogin)=@usrlogin";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", login));
            cmd.ExecuteNonQuery();

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
            if (ex.GetType() != typeof(UserDisabledException) &&
                ex.GetType() != typeof(UserNotVerifiedException) &&
                ex.GetType() != typeof(AuthenticationException))
            {
               _ws.Logger.Add(new LogEntry(GetType().Name + ".Autenticate(string,string)", 
                                           ex.Message, 
                                           LogEntry.LogEntryType.EV_ERROR));
            }

            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Actualiza los datos de una cuenta de usuario del workspace.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta de usuario.</param>
      /// <param name="newPassword">Nueva contraseña del usuario.</param>
      public override void SetPassword(int uid, string newPassword)
      {
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            // Añade el registro a la tabla USERS
            cmd = new SqlCommand("UPDATE " + SQL_USER_TABLE + " " +
                                 "SET usrpwd=@usrpwd " +
                                 "WHERE usrid=@usrid", _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrpwd", newPassword));
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".SetPassword(int,string)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
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
      /// Los parámetros newPassword y newPasswordVerification deben coincidir o de lo contrario se lanzará la excepción <see cref="SecurityException"/>.
      /// </remarks>
      public override void SetPassword(int uid, string oldPassword, string newPassword, string newPasswordVerification)
      {
         SqlCommand cmd = null;

         // Verifica que la nueva contraseña y su verificación sean iguales
         if (!newPassword.Equals(newPasswordVerification))
            throw new SecurityException("La nueva contraseña no coincide con la verificación. La contraseña no se ha modificado.");

         if (!CheckPassword(uid, oldPassword))
            throw new SecurityException("La contraseña actual es incorrecta. La contraseña no se ha modificado.");

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            /*
            // Obtiene la contraseña actual y verifica su validez
            cmd = new SqlCommand("SELECT usrpwd FROM users WHERE usrid=@usrid", _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            string currentpwd = (string)cmd.ExecuteScalar();

            if (oldPassword != currentpwd)
               throw new CosmoSecurityException("La contraseña actual es incorrecta. La contraseña no se ha modificado.");
            */

            // Añade el registro a la tabla USERS
            cmd = new SqlCommand("UPDATE " + SQL_USER_TABLE + " " +
                                 "SET usrpwd=@usrpwd WHERE usrid=@usrid", _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrpwd", newPassword));
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".SetPassword(int,string,string,string)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Verifica una contraseña de usuario.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="password">Contraseña a verificar.</param>
      /// <returns>Devuelve <c>true</c> si la contraseña coincide o <c>false</c> en cualquier otro caso.</returns>
      public override bool CheckPassword(int uid, string password)
      {
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            // Obtiene la contraseña actual y verifica su validez
            cmd = new SqlCommand("SELECT usrpwd " +
                                 "FROM " + SQL_USER_TABLE + " " +
                                 "WHERE usrid=@usrid", _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));
            string currentpwd = (string)cmd.ExecuteScalar();

            return password.Equals(currentpwd);
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".CheckPassword(int,string)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Selecciona un grupo de usuarios.
      /// </summary>
      /// <param name="selector">Selector que contiene los critérios de búsqueda.</param>
      /// <returns>Una lista de instancias de <see cref="User"/>.</returns>
      public override List<User> FindByCriteria(UserSearchCriteria selector)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         List<User> find = new List<User>();

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT " + SQL_USER_FIELDS + " " +
                  "FROM " + SQL_USER_TABLE + " " +
                  this.SelectorSQL(selector) +
                  "ORDER BY usrlogin Asc";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  find.Add(ReadUserFields(reader));
               }
            }

            return find;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Search(UserSearchCriteria)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Busca usuarios del workspace.
      /// </summary>
      /// <param name="login">Login o fracción a buscar</param>
      /// <param name="city">Ciudad o fracción a buscar</param>
      /// <param name="countryId">Identificador del pais a buscar</param>
      /// <returns>Una lista de instancias de <see cref="User"/>.</returns>
      public override List<User> Find(string login, string city, int countryId)
      {
         return Find(login, city, countryId, true);
      }

      /// <summary>
      /// Busca usuarios del workspace.
      /// </summary>
      /// <param name="login">Login o fracción a buscar</param>
      /// <param name="city">Ciudad o fracción a buscar</param>
      /// <param name="countryId">Identificador del pais a buscar</param>
      /// <param name="onlyEnabledUsers"></param>
      /// <returns>Una lista de instancias de <see cref="User"/>.</returns>
      public override List<User> Find(string login, string city, int countryId, bool onlyEnabledUsers)
      {
         List<User> users = new List<User>();
         SqlCommand cmd = null;

         if (login.Equals(string.Empty) && city.Equals(string.Empty) && countryId == 0)
         {
            throw new Exception("No ha proporcionado ningún criterio de búsqueda.");
         }

         if (login.Equals(string.Empty) && city.Equals(string.Empty))
         {
            throw new Exception("Debe proporcionar un login (o fracción) o una ciudad (o fracción) para iniciar la búsqueda.");
         }

         string condition = string.Empty;
         condition += (onlyEnabledUsers ? "usrstatus = 2" : string.Empty);
         condition += (string.IsNullOrEmpty(login) ? string.Empty : (!string.IsNullOrEmpty(condition) ? " And " : string.Empty) + "(Lower(usrlogin) Like Lower('%" + login.Trim() + "%') Or Lower(usrname) Like Lower('%" + login.Trim() + "%'))");
         condition += (string.IsNullOrEmpty(city) ? string.Empty : (!string.IsNullOrEmpty(condition) ? " And " : string.Empty) + "Lower(usrcity) Like Lower('%" + city.Trim() + "%')");
         condition += (countryId <= 0 ? string.Empty : (!string.IsNullOrEmpty(condition) ? " And " : string.Empty) + "usrcountryid=" + countryId);

         // if (!string.IsNullOrEmpty(condition)) condition = " And " + condition;

         try
         {
            // Abre una conexión a la BBDD
            _ws.DataSource.Connect();

            cmd = new SqlCommand("SELECT TOP 50 " + SQL_USER_FIELDS + " " +
                                 "FROM          " + SQL_USER_TABLE + " " + 
                                 "WHERE         " + condition + " " +
                                 "ORDER BY      usrlogin Asc", _ws.DataSource.Connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  users.Add(ReadUserFields(reader));
               }
            }

            return users;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Search(string,string,int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve una lista de paises.
      /// </summary>
      public override List<Country> ListCountry()
      {
         CountryDAO cdao = new CountryDAO(_ws);
         return cdao.GetCountryList();
      }

      /// <summary>
      /// Gets the user location (city and country name) with a standard format.
      /// </summary>
      /// <param name="uid">User unique identifier.</param>
      /// <returns>A string with formatted city and country information.</returns>
      public override string GetUserLocation(int uid)
      {
         string sql = string.Empty;
         string location = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT u.USRCITY, c.COUNTRYNAME 
                    FROM   " + SQL_USER_TABLE + @" u 
                           Inner Join COUNTRY c On (c.COUNTRYID = u.USRCOUNTRYID) 
                    WHERE  u.USRID = @UserId";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("UserId", uid));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               if (reader.Read())
               {
                  if (reader.IsDBNull(0) && reader.IsDBNull(1))
                  {
                     location = "Ubicación desconocida";
                  }
                  else if (reader.IsDBNull(0) || string.IsNullOrWhiteSpace(reader.GetString(0)))
                  {
                     if (string.IsNullOrWhiteSpace(reader.GetString(1)))
                     {
                        location = "Ubicación desconocida";
                     }
                     else
                     {
                        location = reader.GetString(1);
                     }
                  }
                  else if (reader.IsDBNull(1) || string.IsNullOrWhiteSpace(reader.GetString(1)))
                  {
                     if (string.IsNullOrWhiteSpace(reader.GetString(0)))
                     {
                        location = "Ubicación desconocida";
                     }
                     else
                     {
                        location = reader.GetString(0);
                     }
                  }
                  else
                  {
                     location = reader.GetString(0) + " (" + reader.GetString(1) + ")";
                  }
               }
            }

            return location;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetUserLocation(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene el login asociado a un UID.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <returns>Una cadena que representa el login del usuario.</returns>
      public string ConvertUidToLogin(int uid)
      {
         SqlCommand cmd = null;

         try
         {
            // Obtiene el LOGIN del usuario
            _ws.DataSource.Connect();

            string sql = "SELECT usrlogin " +
                         "FROM " + SQL_USER_TABLE + " " +
                         "WHERE usrid=@usrid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrid", uid));

            return (string)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".ConvertUidToLogin(int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Gets a new instance of MWUser con los datos recuperados de un objeto SqlDataReader que apunte a un registro de la tabla USERS
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
            user.Owner = !reader.IsDBNull(13) ? reader.GetString(13) : SecurityService.ACCOUNT_SUPER;
            user.Created = reader.GetDateTime(14);
            user.Password = !reader.IsDBNull(15) ? reader.GetString(15) : string.Empty;

            string roles = !reader.IsDBNull(16) ? reader.GetString(16) : string.Empty;
            foreach (string role in roles.Split(','))
            {
               user.Roles.Add(new Role(0, role));
            }

            // TODO: Encriptar la contraseña de los usuarios del workspace
            // Desencripta la contraseña
            // user.Password = Cryptography.Decrypt(user.Password, user.Login.ToLower(), true);

            return user;
         }
         catch (Exception ex)
         {
            throw ex;
         }
      }

      /// <summary>
      /// Lee los datos de un rol de usuario de la base de datos.
      /// </summary>
      /// <param name="reader">Objeto SqlDataReader que apunte a un registro válido</param>
      /// <returns>Una instancia de <see cref="Role"/> que representa el rol leido.</returns>
      /// <remarks>
      /// Para usar este método la senténcia SQL debe contener los campos indicados en la constante USER_FIELDS
      /// </remarks>
      private Role ReadRole(SqlDataReader reader)
      {
         try
         {
            Role role = new Role();
            role.ID = reader.GetInt32(0);
            role.Name = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;
            role.Description = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty;

            return role;
         }
         catch (Exception ex)
         {
            throw ex;
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
         if (!string.IsNullOrEmpty(selector.City.Trim()))
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
         {   
            url = "data@data.com?" + url;
         }

         string[] parts = url.Split("?".ToCharArray());

         System.Collections.Specialized.NameValueCollection qs = new System.Collections.Specialized.NameValueCollection();
         // qs. ._document = parts[0];

         if (parts.Length == 1)
         {   
            return qs;
         }

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

      #region Disabled Code

      /*
      /// <summary>
      /// Obtiene una lista de los roles de un usuario.
      /// </summary>
      /// <param name="uid">Identificador único del usuario.</param>
      /// <returns>Una lista de instancias de <see cref="Role"/> que representan los roles asignados al usuario.</returns>
      public override List<Role> GetUserRoles(int uid)
      {
         return GetUserRoles(ConvertUidToLogin(uid));
      }

      /// <summary>
      /// Obtiene una lista de los roles de un usuario.
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <returns>Una lista de instancias de <see cref="Role"/> que representan los roles asignados al usuario.</returns>
      public override List<Role> GetUserRoles(string login)
      {
         string sql = string.Empty;
         Role role = null;
         List<Role> roles = new List<Role>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            // Rellena el control
            sql = "SELECT " + SQL_ROLE_FIELDS + " " +
                  "FROM " + SQL_USER_TABLE + " Inner Join sys_users_roles On (users.usrid=sys_users_roles.usrid) " +
                  "           Inner Join sys_roles On (sys_users_roles.roleid=sys_roles.rolid) " +
                  "WHERE users.usrlogin=@usrlogin";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@usrlogin", login));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               role = ReadRole(reader);
               roles.Add(role);
            }
            reader.Close();

            return roles;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().PropertyName + ".GetUserRoles(string)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve una lista de usuarios que tienen un rol específico.
      /// </summary>
      /// <param name="roleId">Identificador del rol.</param>
      /// <returns>
      /// Una lista que contiene las inatancias de <see cref="User"/> que pretenecen al rol solicitado.
      /// </returns>
      public override List<User> GetRoleUsers(int roleId)
      {
         List<User> users = new List<User>();
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            // Rellena el control
            sql = "SELECT " + SQL_USER_FIELDS + " " +
                  "FROM " + SQL_USER_TABLE + " Inner Join sys_users_roles On (users.usrid=sys_users_roles.usrid) " +
                  "WHERE sys_users_roles.roleid=@roleid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
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
            _ws.Logger.Add(new LogEntry(GetType().PropertyName + ".GetRoleUsers(int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }
      */

      #endregion

   }
}
