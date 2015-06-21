﻿using Cosmo.Diagnostics;
using Cosmo.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Mail;

namespace Cosmo.Communications.PrivateMessages
{

   /// <summary>
   /// Implementa una clase de servicio para la gestión de mensajes inter-usuarios
   /// </summary>
   public class PrivateMessageDAO
   {
      // Declaración de variables internas
      private Workspace _ws = null;

      /// <summary>Identificador del parámetro QueryString para el identificador de mensaje.</summary>
      public const string PARAM_MESSAGEID = "mid";
      /// <summary>Identificador del parámetro QueryString para el identificador de thread.</summary>
      public const string PARAM_OWNERID = "tid";
      /// <summary>Identificador del parámetro QueryString para el identificador del usuario remitente.</summary>
      public const string PARAM_FROM = "from";
      /// <summary>Identificador del parámetro QueryString para el identificador del usuario destinatario.</summary>
      public const string PARAM_TO = "to";
      /// <summary>Identificador del parámetro QueryString para el identificador del asunto.</summary>
      public const string PARAM_SUBJECT = "subj";
      /// <summary>Identificador del parámetro QueryString para el identificador del cuerpo del mensaje.</summary>
      public const string PARAM_BODY = "body";

      private const string TAG_LOGIN = "<%LOGIN%>";
      private const string TAG_SUBJECT = "<%SUBJECT%>";
      private const string TAG_BODY = "<%BODY%>";
      private const string TAG_URL_RESPONSE = "<%URL_RESPONSE%>";

      // Cuenta de ex-mail desde dónde se envian notificaciones a los usuarios del workspace.
      private const string UsersMailFromAddress = "users.mail.fromadd";
      // Nombre asociado a la cuenta de ex-mail desde dónde se envian notificaciones a los usuarios del workspace.
      private const string UsersMailFromName = "users.mail.fromname";
      // Asunto del mensaje de notificación de recepción de mensaje privado.
      private const string PrivateMessagesNotifySubject = "workspace.privatemsg.notify.subject";
      // Cuerpo del mensaje de notificación de recepción de mensaje privado.
      private const string PrivateMessagesNotifyBody = "workspace.privatemsg.notify.body";

      private const string SQL_SELECT = "ID, FROMUSRID, TOUSRID, FROMIP, SENDED, SUBJECT, STATUS, BODY, THREADID";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de MWNetUserMessages
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      public PrivateMessageDAO(Workspace workspace)
      {
         _ws = workspace;
      }

      #endregion

      #region Methods

      /// <summary>
      /// Envia un mensaje interno entre dos usuarios
      /// </summary>
      /// <param name="message">Una instancia que contiene todos los datos del mensaje</param>
      public void SendMessage(PrivateMessage message)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlParameter param = null;
         SqlTransaction trans = null;

         try
         {
            _ws.DataSource.Connect();
            trans = _ws.DataSource.Connection.BeginTransaction();

            sql = "INSERT INTO sysusersmsg (threadid,fromusrid,tousrid,fromip,sended,subject,body,status) " +
                  "VALUES (@threadid,@fromusrid,@tousrid,@fromip,getdate(),@subject,@body,@status)";

            // Inserta el mensaje para el destinatario
            message.OwnerId = message.ToUserID;
            
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);

            param = new SqlParameter("@threadid", System.Data.SqlDbType.Int);
            param.Value = message.OwnerId;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@fromusrid", System.Data.SqlDbType.Int);
            param.Value = message.FromUserID;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@tousrid", System.Data.SqlDbType.Int);
            param.Value = message.ToUserID;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@fromip", System.Data.SqlDbType.NVarChar, 20);
            param.Value = message.FromIP;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@subject", System.Data.SqlDbType.NVarChar, 512);
            param.Value = message.Subject.Trim();
            cmd.Parameters.Add(param);

            param = new SqlParameter("@body", System.Data.SqlDbType.NVarChar, 4000);
            param.Value = (message.Body == null ? string.Empty : message.Body.Trim());
            cmd.Parameters.Add(param);

            param = new SqlParameter("@status", System.Data.SqlDbType.Int);
            param.Value = (int)PrivateMessage.UserMessageStatus.Unreaded;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();

            // Inserta el mensaje para el autor (copia)
            message.OwnerId = message.FromUserID;

            cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);

            param = new SqlParameter("@threadid", System.Data.SqlDbType.Int);
            param.Value = message.OwnerId;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@fromusrid", System.Data.SqlDbType.Int);
            param.Value = message.FromUserID;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@tousrid", System.Data.SqlDbType.Int);
            param.Value = message.ToUserID;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@fromip", System.Data.SqlDbType.NVarChar, 20);
            param.Value = message.FromIP;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@subject", System.Data.SqlDbType.NVarChar, 512);
            param.Value = message.Subject.Trim();
            cmd.Parameters.Add(param);

            param = new SqlParameter("@body", System.Data.SqlDbType.NVarChar, 4000);
            param.Value = (message.Body == null ? string.Empty : message.Body.Trim());
            cmd.Parameters.Add(param);

            param = new SqlParameter("@status", System.Data.SqlDbType.Int);
            param.Value = (int)PrivateMessage.UserMessageStatus.Unreaded;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();

            // Obtiene el identificador del mensaje enviado (no la copia)
            sql = "SELECT Max(id) FROM sysusersmsg WHERE threadid=@threadid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);

            param = new SqlParameter("@threadid", System.Data.SqlDbType.Int);
            param.Value = message.ToUserID;
            cmd.Parameters.Add(param);

            message.ID = (int)cmd.ExecuteScalar();
            message.OwnerId = message.ID;

            trans.Commit();

            // Notifica al receptor por eMail
            SendNotify(message);
         }
         catch (Exception ex)
         {
            trans.Rollback();

            _ws.Logger.Add(new LogEntry(GetType().Name + ".SendMessage()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));

            throw new CommunicationsException(ex.Message, ex);
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Recupera un mensaje
      /// </summary>
      /// <param name="messageId">Identificador del mensaje en el servidor</param>
      /// <returns>Una instancia que contiene todos los datos del mensaje</returns>
      public PrivateMessage GetMessage(int messageId)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         SqlTransaction trans = null;
         PrivateMessage message = null;

         try
         {
            _ws.DataSource.Connect();
            trans = _ws.DataSource.Connection.BeginTransaction();

            // Obtiene los datos del mensaje
            sql = "SELECT " + SQL_SELECT + " " +
                  "FROM sysusersmsg " +
                  "WHERE id=@id";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
            cmd.Parameters.Add(new SqlParameter("@id", messageId));

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               message = ReadPrivateMessage(reader);
            }
            reader.Close();

            // Lanza una excepción si no existe el mensaje
            if (message == null)
            {
               throw new CommunicationsException("El mensaje solicitado no existe.");
            }

            // Marca el mensaje como leído
            if (message.Status != PrivateMessage.UserMessageStatus.Readed)
            {
               sql = "UPDATE sysusersmsg SET status=" + (int)PrivateMessage.UserMessageStatus.Readed + " WHERE id=@id";
               cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
               cmd.Parameters.Add(new SqlParameter("@id", messageId));
               cmd.ExecuteNonQuery();
            }

            trans.Commit();

            return message;
         }
         catch (Exception ex)
         {
            trans.Rollback();

            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetMessage()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            trans.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina un mensaje
      /// </summary>
      /// <param name="messageId">Identificador del mensaje en el servidor</param>
      public void DeleteMessage(int messageId)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            // Elimina el mensaje
            sql = "DELETE FROM sysusersmsg WHERE id=@id";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@id", messageId));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".DeleteMessage()", 
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
      /// Recupera mensajes de un usuario
      /// </summary>
      /// <param name="recipientId">Identificador del usuario destinatario (recipient).</param>
      /// <param name="onlyUnreaded">Obtener sólo los mensajes no leídos.</param>
      /// <returns>Una lista de mensajes recibidos</returns>
      public List<PrivateMessage> GetMessagesByRecipient(int recipientId, bool onlyUnreaded)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         PrivateMessage message = null;
         List<PrivateMessage> messages = new List<PrivateMessage>();

         try
         {
            _ws.DataSource.Connect();

            // Obtiene los datos del mensaje
            sql = "SELECT " + SQL_SELECT + " " +
                  "FROM sysusersmsg " +
                  "WHERE tousrid=@tousrid" + (onlyUnreaded ? " And status=0" : string.Empty) + " " +
                  "ORDER BY id Desc";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@tousrid", recipientId));

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               message = ReadPrivateMessage(reader);
               messages.Add(message);
            }
            reader.Close();

            return messages;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetMessagesByRecipient()", 
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
      /// Recupera los mensajes entre dos usuarios (thread).
      /// </summary>
      /// <param name="ownerUserId">Identificador del primer usuario.</param>
      /// <param name="remoteUserId">Identificador del segundo usuario.</param>
      /// <returns></returns>
      public PrivateMessageThread GetThread(int ownerUserId, int remoteUserId)
      {
         bool first = true;
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         PrivateMessage message = null;
         PrivateMessageThread thread = new PrivateMessageThread();

         sql = @"SELECT    sysusersmsg.*, 
                           fru.usrid As fromid, 
                           fru.usrlogin As fromlogin, 
                           fru.usrname As fromname, 
                           tou.usrid As toid, 
                           tou.usrlogin As tologin, 
                           tou.usrname As toname 
                 FROM      sysusersmsg Left Join users fru On (sysusersmsg.fromusrid = fru.usrid) 
                                       Left Join users tou On (sysusersmsg.tousrid = tou.usrid) 
                 WHERE     (sysusersmsg.fromusrid = @remote Or sysusersmsg.tousrid=@remote) And 
                           sysusersmsg.threadid = @owner 
                 ORDER BY  sysusersmsg.id Desc";

         try
         {
            _ws.DataSource.Connect();

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@owner", ownerUserId));
            cmd.Parameters.Add(new SqlParameter("@remote", remoteUserId));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               message = ReadPrivateMessage(reader);
               thread.Messages.Add(message);

               if (first)
               {
                  /*thread.FromId = ((int)reader["fromid"] == null ? -1 : (int)reader["fromid"]);
                  thread.ToId = ((int)reader["toid"] == null ? -1 : (int)reader["toid"]);
                  thread.FromName = ((string)reader["fromname"] == null ? string.Empty : (string)reader["fromname"]);
                  thread.ToName = ((string)reader["toname"] == null ? string.Empty : (string)reader["toname"]);
                  thread.FromLogin = ((string)reader["fromlogin"] == null ? string.Empty : (string)reader["fromlogin"]);
                  thread.ToLogin = ((string)reader["tologin"] == null ? string.Empty : (string)reader["tologin"]);*/

                  thread.LastMessagesDate = thread.Messages[0].Sended;
                  thread.HaveUnreadMessages = (thread.Messages[0].Status == PrivateMessage.UserMessageStatus.Unreaded);
               }
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetThread()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }

         return null;
      }

      /// <summary>
      /// Devuelve todos los threads de un usuario (al estilo Facebook).
      /// </summary>
      /// <param name="userId">Identificador del usuario para el que se desean obtener todos los threads.</param>
      /// <returns></returns>
      public List<PrivateMessageThread> GetThreads(int userId)
      {
         // int id;
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         PrivateMessageThread thread = null;
         List<PrivateMessageThread> threads = new List<PrivateMessageThread>();
         List<int> userIds = new List<int>();

         try
         {
            _ws.DataSource.Connect();

            // Obtiene la lista de usuarios (ID) con los que existe una conversa abierta
            sql = "WITH summary AS (SELECT p.id,  " +
                  "                        p.REMOTEUSRID, " +
                  "                        p.sended, " +
                  "                        p.body, " +
                  "                        p.status, " +
                  "                        ROW_NUMBER() OVER(PARTITION BY p.remoteusrid " +
                  "                                          ORDER BY p.sended DESC) AS rk " + 
                  "                 FROM (SELECT id, " +
                  "                              CASE WHEN FROMUSRID = @userId AND TOUSRID = @userId THEN 0 " +
                  "                                   WHEN FROMUSRID = @userId THEN TOUSRID " +
                  "                                   WHEN TOUSRID   = @userId THEN FROMUSRID " +
                  "                                   ELSE 0 " +
                  "                              END AS REMOTEUSRID, " +
                  "                              sended, " +
                  "                              body, " +
                  "                              status " +
                  "                       FROM sysusersmsg " +
                  "                       WHERE threadid=@userId And FROMUSRID<>TOUSRID) p) " +
                  "SELECT s.* " +
                  "FROM summary s " +
                  "WHERE s.rk = 1";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               thread = new PrivateMessageThread();
               thread.RemoteUserId = (int)reader["remoteusrid"];
               thread.LastMessagesDate = (DateTime)reader["sended"];
               thread.HaveUnreadMessages = ((int)reader["status"] == 0);

               threads.Add(thread);
            }
            reader.Close();

            // Obtiene usuarios remotos
            foreach (PrivateMessageThread th in threads)
            {
               th.RemoteUser = _ws.SecurityService.GetUser(th.RemoteUserId);
            }

            return threads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetThreads(int)", 
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
      /// Recupera un mensaje
      /// </summary>
      /// <param name="ownerId">Identificador del propietario del hilo de mensajes.</param>
      /// <param name="userId">Identificador del usuario destinatario.</param>
      /// <returns>Una lista de instancias de <see cref="PrivateMessage"/>.</returns>
      public List<PrivateMessage> GetThreadMessages(int ownerId, int userId)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         PrivateMessage message = null;
         List<PrivateMessage> messages = new List<PrivateMessage>();

         try
         {
            _ws.DataSource.Connect();

            // Obtiene los datos del mensaje
            sql = "SELECT " + SQL_SELECT + " " +
                  "FROM sysusersmsg " +
                  "WHERE threadid=@threadid And " +
                  "      (tousrid=@tousrid Or fromusrid=@tousrid) " +
                  "ORDER BY id Asc";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@threadid", ownerId));
            cmd.Parameters.Add(new SqlParameter("@tousrid", userId));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               message = ReadPrivateMessage(reader);
               messages.Add(message);
            }
            reader.Close();

            return messages;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetThreadMessages(int)", 
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
      /// Recupera mensajes de un usuario
      /// </summary>
      /// <param name="uid">Identificador del usuario (destinatario)</param>
      /// <returns>Una lista de mensajes recibidos</returns>
      public List<PrivateMessage> Received(int uid)
      {
         return this.GetMessagesByRecipient(uid, false);
      }

      /// <summary>
      /// Recupera mensajes de un usuario
      /// </summary>
      /// <param name="uid">Identificador del usuario (destinatario)</param>
      /// <returns>Una lista de mensajes recibidos</returns>
      public List<PrivateMessage> ReceivedThreads(int uid)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         List<PrivateMessage> messages = new List<PrivateMessage>();

         try
         {
            _ws.DataSource.Connect();

            // Obtiene los datos del mensaje
            sql = "SELECT sm.id,sm.fromusrid,sm.tousrid,sm.fromip,sm.sended,sm.subject,sm.body,sm.status,sm.threadid,(SELECT Count(*) FROM sysusersmsg smc WHERE smc.threadid=sm.id) " +
                  "FROM sysusersmsg sm " +
                  "WHERE sm.tousrid=@tousrid And sm.id=sm.threadid " +
                  "ORDER BY sm.id Desc";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@tousrid", uid));

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               PrivateMessage message = new PrivateMessage();
               message.ID = reader.GetInt32(0);
               message.FromUserID = reader.GetInt32(1);
               message.ToUserID = reader.GetInt32(2);
               message.FromIP = reader.GetString(3);
               message.Sended = reader.GetDateTime(4);
               message.Subject = reader.GetString(5);
               // message.Body = reader.GetString(6);
               message.Status = (PrivateMessage.UserMessageStatus)reader.GetInt32(7);
               message.OwnerId = reader.GetInt32(8);
               message.Responses = reader.GetInt32(9);
               messages.Add(message);
            }
            reader.Close();

            return messages;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".ReceivedThreads(int)", 
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
      /// Recupera mensajes enviados por un usuario
      /// </summary>
      /// <param name="uid">Identificador del usuario (remitente)</param>
      /// <returns>Una lista de todos los mensajes enviados</returns>
      public List<PrivateMessage> Sended(int uid)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         PrivateMessage message = null;
         List<PrivateMessage> messages = new List<PrivateMessage>();

         try
         {
            _ws.DataSource.Connect();

            // Obtiene los datos del mensaje
            sql = "SELECT " + SQL_SELECT + " " +
                  "FROM sysusersmsg " +
                  "WHERE fromusrid=@fromusrid " +
                  "ORDER BY id Desc";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fromusrid", uid));

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               message = ReadPrivateMessage(reader);
               messages.Add(message);
            }
            reader.Close();

            return messages;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Sended(int)", 
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
      /// Devuelve el número de mensajes de un usuario
      /// </summary>
      /// <param name="uid">Identificador único del usuario</param>
      /// <returns>El número de mensajes en la bandeja de entrada</returns>
      public int Count(int uid)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            // Obtiene los datos del mensaje
            sql = "SELECT Count(*) " +
                  "FROM sysusersmsg " +
                  "WHERE fromusrid=@fromusrid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fromusrid", uid));

            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Count(int)", 
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
      /// Devuelve el número de mensajes de un usuario.
      /// </summary>
      /// <param name="uid">Identificador único del usuario destinatario.</param>
      /// <param name="status">Filtro para averiguar el número de mensajes de un determinado estado</param>
      /// <returns>El número de mensajes en la bandeja de entrada</returns>
      public int CountByReceiver(int uid, PrivateMessage.UserMessageStatus status)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            // Obtiene los datos del mensaje
            sql = "SELECT Count(*) " +
                  "FROM sysusersmsg " +
                  "WHERE tousrid=@tousrid And status=@status";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@tousrid", uid));
            cmd.Parameters.Add(new SqlParameter("@status", (int)status));

            return (int)cmd.ExecuteScalar();
         }
         catch (SqlException ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".CountByReceiver()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".CountByReceiver()", 
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
      /// Devuelve el número de mensajes de un usuario
      /// </summary>
      /// <param name="uid">Identificador único del usuario</param>
      /// <param name="status">Filtro para averiguar el número de mensajes de un determinado estado</param>
      /// <returns>El número de mensajes en la bandeja de entrada</returns>
      public int Count(int uid, PrivateMessage.UserMessageStatus status)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            // Obtiene los datos del mensaje
            sql = "SELECT Count(*) " +
                  "FROM sysusersmsg " +
                  "WHERE fromusrid=@fromusrid And status=@status";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fromusrid", uid));
            cmd.Parameters.Add(new SqlParameter("@status", (int)status));

            return (int)cmd.ExecuteScalar();
         }
         catch (SqlException ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Count()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Count()", 
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
      /// Recupera un mensaje de la base de datos.
      /// </summary>
      /// <param name="reader">Una instancia de <see cref="SqlDataReader"/> posicionada en la fila que contiene el mensaje a recuperar.</param>
      /// <returns>Una instancia de <see cref="PrivateMessage"/> que contiene los datos del mensaje solicitado.</returns>
      private PrivateMessage ReadPrivateMessage(SqlDataReader reader)
      {
         PrivateMessage message = null;

         try
         {
            message = new PrivateMessage();
            message.ID = reader.GetInt32(0);
            message.FromUserID = reader.GetInt32(1);
            message.ToUserID = reader.GetInt32(2);
            message.FromIP = reader.GetString(3);
            message.Sended = reader.GetDateTime(4);
            message.Subject = reader.GetString(5);
            message.Status = (PrivateMessage.UserMessageStatus)reader.GetInt32(6);
            message.Body = reader.GetString(7);
            message.OwnerId = reader.GetInt32(8);

            if (string.IsNullOrWhiteSpace(message.Body))
            {
               message.Body = message.Subject;
            }
         }
         catch
         {
            // Descarta la excepción
         }

         return message;
      }

      /// <summary>
      /// Envia una notificación de mensaje privado al receptor de un mensaje privado.
      /// </summary>
      /// <param name="message">Una instancia de <see cref="PrivateMessage"/> que representa el mensaje privado enviado.</param>
      private void SendNotify(PrivateMessage message)
      {
         try
         {
            // Manda una notificación al destinatario
            User user = _ws.SecurityService.GetUser(message.ToUserID);
            if (user.CanReceivePrivateMessagesNotify)
            {
               // Obtiene el remitente
               User sender = _ws.SecurityService.GetUser(message.FromUserID);

               // Manda un mail para que el usuario confirme la cuenta
               MailMessage mail = new MailMessage();
               mail.From = new MailAddress(_ws.Settings.GetString(PrivateMessageDAO.UsersMailFromAddress),
                                           _ws.Settings.GetString(PrivateMessageDAO.UsersMailFromName, _ws.Settings.GetString(PrivateMessageDAO.UsersMailFromAddress)));
               mail.To.Add(new MailAddress(user.Mail, string.IsNullOrEmpty(user.Name) ? user.Login : user.Name));
               mail.Subject = _ws.Settings.GetString(PrivateMessageDAO.PrivateMessagesNotifySubject).
                                 Replace(TAG_LOGIN, sender.Login.ToUpper());
               mail.Body = _ws.Settings.GetString(PrivateMessageDAO.PrivateMessagesNotifyBody).
                              Replace(TAG_LOGIN, sender.Login.ToUpper()).
                              Replace(TAG_SUBJECT, message.Subject).
                              Replace(TAG_BODY, message.Body).
                              Replace(TAG_URL_RESPONSE, Cosmo.Net.Url.Combine(_ws.Url, Workspace.COSMO_URL_USER_MESSAGES));
               mail.IsBodyHtml = false;

               _ws.Communications.Send(mail);
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".SendNotify()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            
            // Descarta la excepción
            // throw ex;
         }
      }

      #endregion

   }
}
