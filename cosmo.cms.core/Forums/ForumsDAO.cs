using Cosmo.Data.Connection;
using Cosmo.Diagnostics;
using Cosmo.Security;
using Cosmo.Security.Auth;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;

namespace Cosmo.Cms.Forums
{
   /// <summary>
   /// Implementa una clase para gestionar los canales de los foros.
   /// </summary>
   public class ForumsDAO
   {
      // Declaración de variables internas
      private Workspace _ws;

      // Declaración de variables estáticas
      public static int MaxThreadsPerPage = 40;

      /// <summary>Nombre del servicio</summary>
      public const string SERVICE_NAME = "Foros";

      /// <summary>Rol de usuario: MODERADOR</summary>
      public const string ROLE_FORUM_MODERATOR = "forums.moderator";

      // Parámetros para uso en páginas ASPX
      public const string PARAM_CHANNEL_ID = "ch";
      public const string PARAM_THREAD_ID = "th";
      public const string PARAM_MESSAGE_ID = "msg";
      public const string PARAM_PAGE_NUM = "p";
      public const string PARAM_ORDER = "o";
      public const string PARAM_PARENT_MESSAGE_ID = "pm";

      // Variables de configuración
      private const string SETUP_SETTING_LIMITNEWTHREADS = "cs.forum.maxthreadsperday";
      private const string SETUP_SETTING_MAXINACTIVITYMONTHS = "cs.forum.maxinactivitymonths";

      // Fragmentos SQL reaprovechables
      private const string SQL_SELECT_FORUM = "forumid,forumname,forumdesc,forumdate,forumenabled,forumowner,(SELECT Count(*) FROM forum WHERE msgforumid=forums.forumid) as items";
      private const string SQL_SELECT_THREAD = "fldauto,msgforumid,fldtitle,flddate,msglastreply,msgnummsgs,fldname,msguserid,msgclosed";
      private const string SQL_SELECT_MESSAGE = "msgclosed,fldauto,msguserid,msgforumid,fldreply,fldname,fldtitle,fldbody,fldip,flddate,msgbbcodes";
      private const string SQL_TABLE_MESSAGES = "forum";
      private const string SQL_TABLE_FORUMS = "forums";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ForumsDAO"/>.
      /// </summary>
      /// <param name="ws">Una instancia de <see cref="Workspace"/>.</param>
      public ForumsDAO(Workspace ws)
      {
         Initialize();

         _ws = ws;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el número de meses que puede pasar inactivo un thread antes de cerrarse automáticamnente.
      /// </summary>
      public int MaxInactivityMonths
      {
         get { return _ws.Settings.GetInt(SETUP_SETTING_MAXINACTIVITYMONTHS, 5); }
      }

      /// <summary>
      /// Devuelve el número máximo de threads que puede abrir un usuario cada dia.
      /// </summary>
      public int MaxThreadsPerDay
      {
         get { return _ws.Settings.GetInt(SETUP_SETTING_LIMITNEWTHREADS, 5); }
      }

      #endregion

      #region Methods

      //=================================================================
      // FOROS
      //=================================================================

      /// <summary>
      /// Devuelve un canal del foro.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <returns>Una instáncia de CSForum.</returns>
      public ForumChannel GetForum(int channelid)
      {
         string sql = string.Empty;
         ForumChannel forum = null;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene el canal
            sql = "SELECT " + SQL_SELECT_FORUM + " " +
                  "FROM   " + SQL_TABLE_FORUMS + " " +
                  "WHERE  forumid = @forumid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@forumid", channelid));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               if (reader.Read())
               {
                  forum = ReadForum(reader);
               }
            }

            return forum;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".GetForum(int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve un array de objetos CSForum correspondiente a los canales del foro.
      /// </summary>
      /// <param name="onlyPublished">Indica si sólo recupera los canales publicados.</param>
      /// <returns>Un array de objetos CSForum.</returns>
      public List<ForumChannel> GetForums(bool published)
      {
         /*string sql = "SELECT forumid,forumname,forumdesc,forumdate,forumenabled,forumowner," +
                             "(SELECT Count(*) FROM cms_forum WHERE msgforumid=fdao.forumid) as items " +
                      "FROM cms_forums " +
                      "WHERE forumenabled=@forumenabled";*/
         // CSForum forum = null;

         List<ForumChannel> forums = new List<ForumChannel>();
         SqlCommand cmd = null;
         SqlParameter param = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene los foros
            // string sql = "SELECT " + SQL_SELECT_FORUM + " FROM cms_forums WHERE forumenabled=@forumenabled";
            cmd = new SqlCommand("cs_Forum_GetChannels", _ws.DataSource.Connection);
            cmd.CommandType = CommandType.StoredProcedure;

            param = new SqlParameter("@forumstatus", SqlDbType.Bit);
            param.Value = published;
            cmd.Parameters.Add(param);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  forums.Add(ReadForum(reader));
               }
            }

            return forums;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetForums(bool)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve un array de objetos CSForum correspondiente a los canales del foro.
      /// </summary>
      /// <returns>Un array de objetos CSForum.</returns>
      public List<ForumChannel> GetForums()
      {
         string sql = string.Empty;
         List<ForumChannel> forums = new List<ForumChannel>();
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene los foros
            sql = "SELECT " + SQL_SELECT_FORUM + " " +
                  "FROM   " + SQL_TABLE_FORUMS + " ";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  forums.Add(ReadForum(reader));
               }
            }

            return forums;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetForums()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene el número de mensajes de un canal.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <returns>El número de mensajes del canal.</returns>
      public int GetForumMessagesCount(int channelid)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            sql = "SELECT Count(*) " +
                  "FROM   " + SQL_TABLE_MESSAGES + " " +
                  "WHERE  msgforumid = @msgforumid";
            
            // Obtiene el número de canales
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@msgforumid", channelid));

            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetForumMessagesCount(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      //=================================================================
      // CANALES
      //=================================================================

      /// <summary>
      /// Obtiene las cabeceras de los hilos de un canal.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <param name="container">Número de página.</param>
      /// <param name="rowsperpage">Número de threads por página.</param>
      /// <returns>Un array de objetos CSForumThread[].</returns>
      /// <remarks>
      /// La estrategia SQL para paginación se basa en el siguiente artículo: 
      /// <a href="http://weblogs.sqlteam.com/jeffs/archive/2003/12/22/672.aspx">weblogs.sqlteam.com/jeffs/archive/2003/12/22/672.aspx</a>
      /// </remarks>
      public List<ForumThread> GetChannelThreads(int channelid, int page, int rowsperpage)
      {
         SqlCommand cmd = null;
         SqlParameter param = null;
         List<ForumThread> threads = new List<ForumThread>();

         try
         {
            _ws.DataSource.Connect();

            cmd = new SqlCommand("cs_Forum_GetThreadsByPage", _ws.DataSource.Connection);
            cmd.CommandType = CommandType.StoredProcedure;

            param = new SqlParameter("@channelId", SqlDbType.Int);
            param.Value = channelid;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@page", SqlDbType.Int);
            param.Value = page;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@itemsPerPage", SqlDbType.Int);
            param.Value = rowsperpage;
            cmd.Parameters.Add(param);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  threads.Add(ReadThread(reader, false));
               }
            }

            return threads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetChannelThreads(int, int, int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene las cabeceras de los hilos de un canal. Recupera el número de Threads indicado por MaxThreadsPerPage.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <param name="container">Número de página.</param>
      /// <returns>Un array de objetos CSForumThread[].</returns>
      public List<ForumThread> GetChannelThreads(int channelid, int page)
      {
         return this.GetChannelThreads(channelid, page, ForumsDAO.MaxThreadsPerPage);
      }

      /// <summary>
      /// Obtiene el número de hilos de un canal.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <returns>El número de threads del canal.</returns>
      public int GetChannelThreadsCount(int channelid)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            sql = "SELECT Count(*) As nregs " +
                  "FROM   " + SQL_TABLE_MESSAGES + " " +
                  "WHERE  fldreply = 0 And " +
                  "       msgforumid = @msgforumid";

            // Obtiene el número de canales
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@msgforumid", channelid));

            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetChannelThreadsCount(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene las propiedades de un thread.
      /// </summary>
      /// <param name="threadid">Identificador del Thread.</param>
      /// <returns>Una instáncia de CSForumThread.</returns>
      /// <remarks>
      /// Este método cierra automáticamente los hilos de 4 meses de antiguedad y que no tengan actividad 
      /// en los últimos 7 días.
      /// </remarks>
      public ForumThread GetThread(int threadid)
      {
         return GetThread(threadid, false, ThreadMessagesOrder.Ascending);
      }

      /// <summary>
      /// Obtiene las propiedades de un thread.
      /// </summary>
      /// <param name="threadid">Identificador del Thread.</param>
      /// <param name="getMessages">Indica si se debe recuperar la lista de mensajes.</param>
      /// <param name="order">Indica que orden deberan tener los mensajes.</param>
      /// <returns>Una instáncia de CSForumThread.</returns>
      /// <remarks>
      /// Este método cierra automáticamente los hilos de 4 meses de antiguedad y que no tengan actividad 
      /// en los últimos 7 días.
      /// </remarks>
      public ForumThread GetThread(int threadid, bool getMessages, ThreadMessagesOrder order)
      {
         bool head = true;
         string sql = string.Empty;
         ForumThread thread = null;
         SqlCommand cmd = null;
         SqlParameter param = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            if (getMessages)
            {
               cmd = new SqlCommand("cs_Forum_GetThread", _ws.DataSource.Connection);
               cmd.CommandType = CommandType.StoredProcedure;

               param = new SqlParameter("@threadId", SqlDbType.Int);
               param.Value = threadid;
               cmd.Parameters.Add(param);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     if (head)
                     {
                        thread = new ForumThread();
                        thread.ID = reader.GetInt32(0);
                        thread.AuthorID = reader.GetInt32(1);
                        thread.ForumID = reader.GetInt32(2);
                        thread.MessageCount = reader.GetInt32(3);
                        thread.AuthorName = reader.IsDBNull(5) ? "<unknown>" : reader.GetString(5);
                        thread.Title = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        thread.Created = reader.GetDateTime(9);
                        thread.LastReply = reader.GetDateTime(10);
                        thread.Closed = reader.GetBoolean(13);

                        // Verifica si el hilo está cerrado
                        if (thread.Created < DateTime.Now.AddMonths(-4))
                        {
                           if (thread.LastReply < DateTime.Now.AddDays(-7))
                           {
                              thread.Closed = true;
                           }
                        }
                        head = false;
                     }

                     ForumMessage message = new ForumMessage();
                     message.ThreadClosed = reader.GetBoolean(13);
                     message.ID = reader.GetInt32(0);
                     message.UserID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                     message.ForumID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                     message.ParentMessageID = threadid;
                     message.Name = reader.IsDBNull(5) ? "<unknown>" : reader.GetString(5);
                     message.Body = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                     message.IP = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                     message.Date = reader.GetDateTime(9);
                     message.BBCodes = reader.GetBoolean(14);

                     thread.Messages.Add(message);
                  }
               }

               // Si se especifica un órden descendiente, se reordena la lista de mensajes
               if (order == ThreadMessagesOrder.Descending)
               {
                  thread.Messages.Sort(delegate(ForumMessage p1, ForumMessage p2) { return p2.ID.CompareTo(p1.ID); });
               }
            }
            else
            {
               // Obtiene las propiedades del hilo
               sql = "SELECT " + SQL_SELECT_THREAD + " " +
                     "FROM   " + SQL_TABLE_MESSAGES + " " +
                     "WHERE  fldauto = @fldauto";
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@fldauto", threadid));

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  if (reader.Read())
                  {
                     thread = ReadThread(reader, false);
                  }
               }
            }

            return thread;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetThread(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene el estado de un thread.
      /// </summary>
      /// <param name="thid">Identificador del thread.</param>
      /// <param name="transaction">Transacción en que se encuentra la llamada.</param>
      /// <returns><c>true</c> si el thread está abierto o <c>false</c> en cualquier otro caso.</returns>
      public bool GetThreadStatus(int thid, SqlTransaction transaction)
      {
         bool closed = false;
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            if (_ws.DataSource.Connection.State != ConnectionState.Open) _ws.DataSource.Connect();

            // Comprueba si el hilo está cerrado
            sql = "SELECT  DateDiff(MM, MAX(flddate), getdate()), (SELECT msgclosed FROM " + SQL_TABLE_MESSAGES + " WHERE fldauto=@fldauto) " +
                  "FROM    " + SQL_TABLE_MESSAGES + " " +
                  "WHERE   fldauto = @fldauto Or " +
                  "        fldreply = @fldauto";
            // SELECT msgclosed FROM forum WHERE fldauto=@fldauto";

            if (transaction != null)
               cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            else
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fldauto", thid));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               if (reader.Read())
               {
                  closed = reader.GetBoolean(1);
                  if (!closed) closed = (reader.GetInt32(0) >= 6);
               }
            }

            return !closed;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetThreadStatus(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene el estado de un thread.
      /// </summary>
      /// <param name="thid">Identificador del thread.</param>
      /// <returns><c>true</c> si el thread está abierto o <c>false</c> en cualquier otro caso.</returns>
      public bool GetThreadStatus(int thid)
      {
         return GetThreadStatus(thid, null);
      }

      /// <summary>
      /// Obtiene las cabeceras de los hilos de un canal.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="container">Número de página.</param>
      /// <param name="rowsperpage">Número de threads por página.</param>
      /// <returns>Un array de objetos CSForumThread[].</returns>
      public List<ForumThread> GetThreadsByUser(int uid, int page, int rowsperpage)
      {
         string sql = string.Empty;
         List<ForumThread> threads = new List<ForumThread>();
         SqlDataAdapter adapter = null;
         DataSet dataSet = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene los mensjaes de nivel superior de la página a mostrar
            sql = "SELECT    forum.fldauto, forums.forumid, forum.fldtitle, forum.flddate, forum.msglastreply, forum.msgnummsgs, forum.fldname, forum.msguserid, forum.msgclosed, forums.forumname " +
                  "FROM      " + SQL_TABLE_MESSAGES + " " +
                  "          INNER JOIN " + SQL_TABLE_FORUMS + " ON (forum.msgforumid=forums.forumid) " +
                  "WHERE     fldauto IN (SELECT DISTINCT msgthread FROM forum WHERE msguserid=" + uid + ") " +
                  "ORDER BY  forum.msglastreply DESC";
            adapter = new SqlDataAdapter(sql, _ws.DataSource.Connection);
            dataSet = new DataSet();
            adapter.Fill(dataSet, (page - 1) * rowsperpage, rowsperpage, "forum");

            using (DataTableReader reader = dataSet.CreateDataReader())
            {
               // Carga la página
               while (reader.Read())
               {
                  threads.Add(ReadThread(reader));
               }
            }

            return threads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetUserThreads(int, int, int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(adapter);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene las cabeceras de los hilos de un canal. Recupera el número de Threads indicado por MaxThreadsPerPage.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="container">Número de página.</param>
      /// <returns>Un array de objetos CSForumThread[].</returns>
      public List<ForumThread> GetThreadsByUser(int uid, int page)
      {
         return this.GetThreadsByUser(uid, page, ForumsDAO.MaxThreadsPerPage);
      }

      /// <summary>
      /// Obtiene el número de hilos de un canal.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <returns>El número de threads del canal.</returns>
      public int CountThreadsByUser(int uid)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene el número de canales
            sql = "SELECT    Count(*) " +
                  "FROM      " + SQL_TABLE_MESSAGES + " " +
                  "WHERE     fldauto In (SELECT DISTINCT msgthread FROM forum WHERE msguserid=@msguserid)";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@msguserid", uid));

            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetUserThreadsCount(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Mueve un hilo a un determinado canal.
      /// </summary>
      /// <param name="threadId">Identificador del thread a mover.</param>
      /// <param name="destinationChannelId">Identificador del canal de destino.</param>
      public void MoveThread(int threadId, int destinationChannelId)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Asegura que el mensaje seleccionado es el enunciado de un hilo
            sql = "SELECT  fldreply " +
                  "FROM    " + SQL_TABLE_MESSAGES + " " +
                  "WHERE   fldauto=@fldauto";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fldauto", threadId));
            if ((int)cmd.ExecuteScalar() > 0)
               throw new Exception("El identificador proporcionado no corresponde a un enunciado de hilo.");

            // Obtiene el número de canales
            sql = "UPDATE  " + SQL_TABLE_MESSAGES + " " +
                  "SET     msgforumid = @msgforumid " +
                  "WHERE   fldauto = @fldauto Or " +
                  "        fldreply = @fldauto";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@msgforumid", destinationChannelId));
            cmd.Parameters.Add(new SqlParameter("@fldauto", threadId));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".MoveThread(int, int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Cierra/Abre un hilo un determinado hilo (invierte el estado).
      /// </summary>
      /// <param name="threadId">Identificador del thread a abrir/cerrar.</param>
      public void ToggleThreadStatus(int threadId)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Asegura que el mensaje seleccionado es el enunciado de un hilo
            sql = "SELECT  fldreply " +
                  "FROM    " + SQL_TABLE_MESSAGES + " " +
                  "WHERE   fldauto=@fldauto";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fldauto", threadId));
            if ((int)cmd.ExecuteScalar() > 0)
               throw new Exception("El identificador proporcionado no corresponde a un enunciado de hilo.");

            // Invierte el valor del indicador de CERRADO
            sql = "UPDATE  " + SQL_TABLE_MESSAGES + " " +
                  "SET     msgclosed = ~msgclosed " +
                  "WHERE   fldauto = @fldauto";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fldauto", threadId));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".ToggleThreadStatus(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }


      //=================================================================
      // MENSAJES
      //=================================================================

      /// <summary>
      /// Recupera las propiedades de un mensaje
      /// </summary>
      /// <param name="messageid">Identificador único del mensaje</param>
      /// <returns>Una instancia de CSForumMessage</returns>
      public ForumMessage GetMessage(int messageid)
      {
         string sql = string.Empty;
         ForumMessage message = null;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene el canal
            sql = "SELECT " + SQL_SELECT_MESSAGE + " " +
                  "FROM   " + SQL_TABLE_MESSAGES + " " +
                  "WHERE  fldauto = @fldauto";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fldauto", messageid));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               if (reader.Read())
               {
                  message = ReadMessage(reader);
               }
               else
               {
                  throw new Exception("El mensaje #" + messageid + " no existe o no está disponible en estos momentos.");
               }
            }

            return message;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetMessage(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Añade un mensaje a un canal o thread.
      /// </summary>
      /// <param name="message">Una instáncia de CSForumMessage.</param>
      public ForumMessage AddMessage(ForumMessage message)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         // Comprobaciones de seguridad
         if (message.ParentMessageID == 0)
         {
            // Comprueba si el usuario puede crear un nuevo thread
            if (!CanPostNewThread(message.UserID))
               throw new TooManyUserObjectsException("No ha sido posible crear el nuevo hilo debido a que ha superado el número de nuevos hilos abiertos por día. Deberá esperar 24h para volver a postear.");
         }
         else
         {
            // Chequea si se puede postear en el hilo
            if (!GetThreadStatus(message.ParentMessageID))
               throw new Exception("El hilo está cerrado y no admite más mensajes.");

            // Chequea si la IP ha sido usada por otro usuario en un periodo de tiempo demasiado corto (subplantación)
            if (!CheckIP(message))
            {
               _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                           "ForumsDAO.AddMessage(CSForumMessage)",
                                           "Subplantación: IP=" + message.IP + ", Thread: " + message.ParentMessageID,
                                           LogEntry.LogEntryType.EV_ERROR));

               throw new SecurityException("Desde una misma IP (" + message.IP + ") se están mandando mensajes con distinto autor. No se permite mandar este mensaje en este momento.");
            }
         }

         // Evita datos nulos
         if (string.IsNullOrEmpty(message.IP)) message.IP = string.Empty;

         try
         {
            // Abre una conexión con la BBDD err inicia una transacción
            _ws.DataSource.Connect();

            using (SqlTransaction dbtrans = _ws.DataSource.Connection.BeginTransaction())
            {
               // Genera la senténcia T/SQL a ejecutar
               if (message.ParentMessageID == 0)
               {
                  cmd = new SqlCommand("cs_Forum_NewThread", _ws.DataSource.Connection, dbtrans);
                  cmd.CommandType = CommandType.StoredProcedure;
                  cmd.Parameters.Add(new SqlParameter("@sTitle", message.Title));
                  cmd.Parameters.Add(new SqlParameter("@sBody", message.Body));
                  cmd.Parameters.Add(new SqlParameter("@sUser", message.Name));
                  cmd.Parameters.Add(new SqlParameter("@sMail", string.Empty));
                  cmd.Parameters.Add(new SqlParameter("@sIPAdd", message.IP));
                  cmd.Parameters.Add(new SqlParameter("@iChannel", message.ForumID));
                  cmd.Parameters.Add(new SqlParameter("@iUserID", message.UserID));
                  cmd.Parameters.Add(new SqlParameter("@bbcodes", message.BBCodes));
               }
               else
               {
                  cmd = new SqlCommand("cs_Forum_ThreadAddPost", _ws.DataSource.Connection, dbtrans);
                  cmd.CommandType = CommandType.StoredProcedure;
                  cmd.Parameters.Add(new SqlParameter("@iThread", message.ParentMessageID));
                  cmd.Parameters.Add(new SqlParameter("@sBody", message.Body));
                  cmd.Parameters.Add(new SqlParameter("@sUser", message.Name));
                  cmd.Parameters.Add(new SqlParameter("@sMail", string.Empty));
                  cmd.Parameters.Add(new SqlParameter("@sIPAdd", message.IP));
                  cmd.Parameters.Add(new SqlParameter("@iChannel", message.ForumID));
                  cmd.Parameters.Add(new SqlParameter("@iUserID", message.UserID));
                  cmd.Parameters.Add(new SqlParameter("@bbcodes", message.BBCodes));
               }

               cmd.ExecuteNonQuery();

               // Averigua el Id del nuevo mensaje
               sql = "SELECT     TOP 1 fldauto AS id " +
                     "FROM       " + SQL_TABLE_MESSAGES + " " +
                     "ORDER BY   fldauto DESC";
               cmd = new SqlCommand(sql, _ws.DataSource.Connection, dbtrans);
               message.ID = (int)cmd.ExecuteScalar();

               // Cierra la transacción
               dbtrans.Commit();
            }

            return message;
         }
         catch (TooManyUserObjectsException)
         {
            throw new TooManyUserObjectsException();
         }
         catch (SecurityException)
         {
            throw new SecurityException();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".AddMessage(ForumMessage)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Actualiza el contenido de un mensaje.
      /// </summary>
      /// <param name="message">Una instáncia de CSForumMessage.</param>
      /// <remarks>Sólo se permite actualizar el cuerpo del mensaje y el título si se informa ( si está vacío, no se actualiza).</remarks>
      public void UpdateMessage(ForumMessage message)
      {
         string sql = string.Empty;
         SqlParameter param = null;
         SqlCommand cmd = null;

         // Determina si el mensaje contiene bbCodes
         Regex rex = new Regex(@"\[.*\]");
         message.BBCodes = rex.Match(message.Body).Success;

         try
         {
            // Abre una conexión con la BBDD err inicia una transacción
            _ws.DataSource.Connect();

            sql = "UPDATE  " + SQL_TABLE_MESSAGES + " " +
                  "SET     fldbody = @fldbody, " + 
                  "        " + (string.IsNullOrEmpty(message.Title) ? string.Empty : "fldtitle=@fldtitle, ") + 
                  "        msgbbcodes = @msgbbcodes " +
                  "WHERE   fldauto = @fldauto";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);

            param = new SqlParameter("@fldbody", SqlDbType.NText);
            param.Value = message.Body;
            cmd.Parameters.Add(param);

            if (!string.IsNullOrEmpty(message.Title))
            {
               param = new SqlParameter("@fldtitle", SqlDbType.NVarChar, 512);
               param.Value = message.Title;
               cmd.Parameters.Add(param);
            }

            param = new SqlParameter("@msgbbcodes", SqlDbType.Bit);
            param.Value = message.BBCodes;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@fldauto", SqlDbType.Int);
            param.Value = message.ID;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
         }
         catch (SqlException ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".UpdateMessage(ForumMessage)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina un mensaje y (opcionalmente) todas sus respuestas (que equivale a eliminar un thread).
      /// </summary>
      /// <param name="messageId">Identificador del mensaje</param>
      /// <param name="deleteRelatedMessages">Indica si se deben eliminar también los mensajes relacionados</param>
      public void DeleteMessage(int messageId, bool deleteRelatedMessages)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión con la BBDD err inicia una transacción
            _ws.DataSource.Connect();

            // Averigua si se trata del enunciado de un hilo
            if (!deleteRelatedMessages)
            {
               sql = "SELECT  Count(*) AS Resplies " +
                     "FROM    " + SQL_TABLE_MESSAGES + " " +
                     "WHERE   flreply = @flreply";

               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@flreply", messageId));
               if ((int)cmd.ExecuteScalar() > 0)
               {
                  throw new Exception("El mensaje que desea eliminar contiene mensajes asociados y no se puede eliminar.");
               }
            }

            // Elimina el mensaje
            sql = "DELETE " +
                  "FROM    " + SQL_TABLE_MESSAGES + " " +
                  "WHERE   fldauto = @fldauto";

            if (deleteRelatedMessages)
            {
               sql += " Or fldreply = @fldauto";
            }

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fldauto", messageId));
            cmd.ExecuteNonQuery();
         }
         catch (SqlException ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".DeleteMessage(int, bool)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }


      /// <summary>
      /// Obtiene todos los mensajes de un thread. En la posición 0 se encuentra la cabecera del Thread.
      /// </summary>
      /// <param name="threadid">Identificador del mensaje de nivel superior.</param>
      /// <param name="order">Tipo de ordenación de los mensajes del thread</param>
      /// <returns>Un array de objetos CSForumMessage.</returns>
      public List<ForumMessage> GetThreadMessages(int threadid, ThreadMessagesOrder order)
      {
         string sql = string.Empty;
         List<ForumMessage> messages = new List<ForumMessage>();
         SqlCommand cmd = null;
         SqlParameter param = null;

         try
         {
            // Abre una conexión con la BBDD
            _ws.DataSource.Connect();

            // Obtiene los mensajes
            sql = "SELECT    " + SQL_SELECT_MESSAGE + " " +
                  "FROM      " + SQL_TABLE_MESSAGES + " " +
                  "WHERE     fldauto = @fldauto Or " +
                  "          fldreply = @fldreply " +
                  "ORDER BY  fldauto " + (order == ThreadMessagesOrder.Ascending ? "Asc" : "Desc");

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            param = new SqlParameter("@fldauto", SqlDbType.Int);
            param.Value = threadid;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@fldreply", SqlDbType.Int);
            param.Value = threadid;
            cmd.Parameters.Add(param);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  messages.Add(ReadMessage(reader));
               }
            }

            return messages;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetThreadMessages(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Testea si el usuario puede abrir un nuevo hilo.
      /// </summary>
      /// <param name="uid">Identificador único del usuario (UID).</param>
      /// <returns></returns>
      public bool CanPostNewThread(int uid, SqlTransaction transaction)
      {
         // Si está desactivada la limitación, permite postear nuevos hilos
         if (MaxThreadsPerDay == 0) return true;

         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión con la BBDD
            _ws.DataSource.Connect();

            // Obtiene el número de hilos abiertos en un dia
            sql = "SELECT Count(*) " +
                  "FROM   " + SQL_TABLE_MESSAGES + " " +
                  "WHERE  msguserid = @msguserid And " +
                  "       DateDiff(day, getdate(), flddate) = 0 And " +
                  "       fldreply = 0";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@msguserid", uid));
            int rows = (int)cmd.ExecuteScalar();

            return (rows < MaxThreadsPerDay);
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".CanPostNewThread(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Testea si el usuario puede abrir un nuevo hilo.
      /// </summary>
      /// <param name="uid">Identificador único del usuario (UID).</param>
      /// <returns></returns>
      public bool CanPostNewThread(int uid)
      {
         return CanPostNewThread(uid, null);
      }

      /// <summary>
      /// Chequea si el mensaje puede ser enviado sin comprometer la seguridad.
      /// </summary>
      /// <param name="message">Mensaje a testear.</param>
      /// <param name="transaction">Transacción en la que se encuentra el proceso.</param>
      /// <remarks>
      /// Este algorítmo testea los siguientes parámetros de seguridad:
      /// - Que el hilo no esté cerrado.
      /// - Identidad múltiple: Que la IP no haya sido usada anteriormente en el mismo hilo con usuario distinto dentro de las 
      ///                       dos últimas horas.
      /// </remarks>
      public bool CheckIP(ForumMessage message, SqlTransaction transaction)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         if (string.IsNullOrEmpty(message.IP))
         {
            return true;
         }

         // Permite a los superusuarios mandar siempre mensajes
         if (message.Name.ToLower().Equals("webmaster") || message.Name.ToLower().Equals("sa"))
         {
            return true;
         }

         try
         {
            _ws.DataSource.Connect();

            // Comprueba que una persona no envie mensajes en un mismo hilo con nombres distintos
            sql = "SELECT  fldauto, msguserid, msgforumid, fldreply, fldname, fldtitle, fldip, flddate, msglastreply, msgthread, msgclosed " +
                  "FROM    " + SQL_TABLE_MESSAGES + " " +
                  "WHERE   msgthread = @msgthread And " +
                  "        fldip = @fldip And " +
                  "        fldname <> @fldname And " +
                  "        Datediff(mi, flddate, getdate()) <= 60";

            if (transaction != null)
               cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            else
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            
            cmd.Parameters.Add(new SqlParameter("@msgthread", message.ParentMessageID));
            cmd.Parameters.Add(new SqlParameter("@fldip", message.IP));
            cmd.Parameters.Add(new SqlParameter("@fldname", message.Name));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  // Check: Identidad múltiple
                  if (Cosmo.Utils.Calendar.DateDiff(Cosmo.Utils.Calendar.DateInterval.Hour, reader.GetDateTime(7), DateTime.Now) <= 2)
                  {
                     string msg = "Identidad múltiple: El usuario " + message.Name.ToUpper() + " intenta escribir en el hilo " + message.ParentMessageID + " con identidad distinta a " + reader.GetString(4).ToUpper() + " usando la IP " + message.IP;

                     _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                                 "ForumsDAO.CheckIP()",
                                                 msg,
                                                 LogEntry.LogEntryType.EV_SECURITY));
                     return false;
                  }
               }
            }

            return true;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        "ForumsDAO.CheckIP()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Chequea si el mensaje puede ser enviado sin comprometer la seguridad.
      /// </summary>
      /// <param name="message">Mensaje a testear.</param>
      /// <remarks>
      /// Este algorítmo testea los siguientes parámetros de seguridad:
      /// - Que el hilo no esté cerrado.
      /// - Identidad múltiple: Que la IP no haya sido usada anteriormente en el mismo hilo con usuario distinto dentro de las 
      ///                       dos últimas horas.
      /// </remarks>
      public bool CheckIP(ForumMessage message)
      {
         return CheckIP(message, null);
      }

      /// <summary>
      /// Obtiene el modo de ordenación de los mensajes
      /// </summary>
      /// <param name="request">Instancia de HttpRequest del servidor</param>
      /// <param name="response">Instancia de HttpResponse del servidor</param>
      /// <returns>El tipo de ordenación a aplicar</returns>
      public ThreadMessagesOrder GetMessageOrder(HttpRequest request, HttpResponse response)
      {
         int torder = (int)ThreadMessagesOrder.Ascending;

         try
         {
            if (!int.TryParse(request[ForumsDAO.PARAM_ORDER], out torder))
            {
               if (request.Cookies["cs.forum"] != null)    // No ha podido recuperar el parámetro ex intenta recuperar la cookie
               {
                  if (request.Cookies["cs.forum"]["msgs.order"] != null)
                     if (!int.TryParse(request.Cookies["cs.forum"]["msgs.order"], out torder))
                        torder = (int)ThreadMessagesOrder.Ascending;
               }
               else
               {
                  torder = (int)ThreadMessagesOrder.Ascending;
               }
            }

            // Memoriza el tipo de ordenación obtenido de los parámetros
            HttpCookie cookie = new HttpCookie("cs.forum");
            cookie["msgs.order"] = torder.ToString();
            cookie.Expires = DateTime.Now.AddYears(1);
            response.Cookies.Add(cookie);

            return (ThreadMessagesOrder)torder;
         }
         catch
         {
            return ThreadMessagesOrder.Ascending;
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Convierte una lista de canales de foro en una lista de valores <see cref="KeyValue"/> que puede ser usado en controles.
      /// </summary>
      /// <param name="list">Lista de canales.</param>
      /// <returns>La lista solicitada.</returns>
      public static List<KeyValue> ConvertToKeyValueList(List<ForumChannel> list)
      {
         List<KeyValue> kval = new List<KeyValue>();

         foreach (ForumChannel channel in list)
         {
            kval.Add(new KeyValue(channel.Name, channel.ID.ToString()));
         }

         return kval;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _ws = null;
      }

      private ForumChannel ReadForum(SqlDataReader reader)
      {
         ForumChannel forum = null;

         forum = new ForumChannel();
         forum.ID = reader.GetInt32(0);
         forum.Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
         forum.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
         forum.Date = reader.GetDateTime(3);
         forum.Published = reader.GetBoolean(4);
         forum.Owner = reader.IsDBNull(5) ? AuthenticationService.ACCOUNT_SUPER : reader.GetString(5);
         forum.MessageCount = reader.GetInt32(6);

         return forum;
      }

      private ForumThread ReadThread(DataTableReader reader)
      {
         ForumThread thread = null;

         thread = new ForumThread();
         thread.ID = reader.GetInt32(0);
         thread.ForumID = reader.GetInt32(1);
         thread.Title = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
         thread.Created = reader.GetDateTime(3);
         thread.LastReply = reader.GetDateTime(4);
         thread.MessageCount = reader.GetInt32(5);
         thread.AuthorName = reader.IsDBNull(6) ? "<unknown>" : reader.GetString(6);
         thread.AuthorID = reader.GetInt32(7);
         thread.Closed = reader.GetBoolean(8);

         // Cierra automáticamente los hilos de 4 meses de antiguedad y que no tengan actividad en los últimos 7 días
         if (thread.LastReply <= DateTime.Now.AddMonths(0 - MaxInactivityMonths))
            thread.Closed = true;

         return thread;
      }

      private ForumThread ReadThread(DataTableReader reader, bool getChannelName)
      {
         ForumThread thread = null;

         thread = new ForumThread();
         thread.ID = reader.GetInt32(0);
         thread.ForumID = reader.GetInt32(1);
         thread.Title = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
         thread.Created = reader.GetDateTime(3);
         thread.LastReply = reader.GetDateTime(4);
         thread.MessageCount = reader.GetInt32(5);
         thread.AuthorName = reader.IsDBNull(6) ? "<unknown>" : reader.GetString(6);
         thread.AuthorID = reader.GetInt32(7);
         thread.Closed = reader.GetBoolean(8);
         if (getChannelName) thread.ChannelName = reader.IsDBNull(9) ? "<unknown>" : reader.GetString(9);

         // Cierra automáticamente los hilos de 4 meses de antiguedad y que no tengan actividad en los últimos 7 días
         if (thread.LastReply <= DateTime.Now.AddMonths(0 - MaxInactivityMonths))
            thread.Closed = true;

         return thread;
      }

      private ForumThread ReadThread(SqlDataReader reader, bool getChannelName)
      {
         ForumThread thread = null;

         thread = new ForumThread();
         thread.ID = reader.GetInt32(0);
         thread.ForumID = reader.GetInt32(1);
         thread.Title = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
         thread.Created = reader.GetDateTime(3);
         thread.LastReply = reader.GetDateTime(4);
         thread.MessageCount = reader.GetInt32(5);
         thread.AuthorName = reader.IsDBNull(6) ? "<unknown>" : reader.GetString(6);
         thread.AuthorID = reader.GetInt32(7);
         thread.Closed = reader.GetBoolean(8);
         if (getChannelName) thread.ChannelName = reader.IsDBNull(9) ? "<unknown>" : reader.GetString(9);

         // Cierra automáticamente los hilos con 4 meses sin actividad
         if (thread.LastReply <= DateTime.Now.AddMonths(0 - MaxInactivityMonths))
            thread.Closed = true;

         return thread;
      }

      private ForumMessage ReadMessage(SqlDataReader reader)
      {
         ForumMessage message = new ForumMessage();
         message.ThreadClosed = reader.GetBoolean(0);
         message.ID = reader.GetInt32(1);
         message.UserID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
         message.ForumID = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
         message.ParentMessageID = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
         message.Name = reader.IsDBNull(5) ? "<unknown>" : reader.GetString(5);
         message.Title = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
         message.Body = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
         message.IP = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
         message.Date = reader.GetDateTime(9);
         message.BBCodes = reader.GetBoolean(10);

         return message;
      }

      #endregion

      #region Disabled Code

      /*
      public const string CONF_RSS_CACHETIMEOUT = "cs.rss.rsscachetime";
      public const string CONF_RSS_MAXITEMS = "cs.rss.rssmaxitems";
      public const string CONF_RSS_PICTURE = "cs.rss.image";
      public const string CONF_RSS_CHANNELFORUM = "cs.rss.channel.forum";

      public const string CACHE_RSS_DOCUMENTS = "cs.rss.channel.doc";
      public const string CACHE_RSS_PICTURES = "cs.rss.channel.images";
      public const string CACHE_RSS_BOOKS = "cs.rss.channel.books";
      public const string CACHE_RSS_ADS = "cs.rss.channel.ads";
      
      public const string CACHE_RSS_FORUMS = "cs.rss.channel.forum";
      */

      /*
      /// <summary>
      /// Genera un canal RSS 2.0 que contiene los últimos mensajes publicados en el foro usando la caché de servidor web
      /// </summary>
      /// <param name="cache">La instáncia Cache del servidor web</param>
      /// <param name="forceUpdate">Indica si se debe forzar al refresco del contenido del canal si usar la caché.</param>
      /// <returns>Devuelve una cadena en formato RSS</returns>
      public string ToRss(System.Web.Caching.Cache cache, bool forceUpdate)
      {
         // Calcula el número de segundos de validez
         int secs = 3600;
         if (!int.TryParse(_ws.Settings.GetString(CONF_RSS_CACHETIMEOUT, "3600"), out secs)) secs = 3600;

         // Guarda el feed en caché
         if (cache[CACHE_RSS_FORUMS] == null || forceUpdate)
            cache.Insert(CACHE_RSS_FORUMS, ToRss(), null, DateTime.Now.AddSeconds(secs), TimeSpan.Zero);

         // Devuelve el contenido de la caché
         return cache[CACHE_RSS_FORUMS].ToString();
      }
        
      /// <summary>
      /// Genera un canal RSS 2.0 que contiene los últimos mensajes publicados en el foro
      /// </summary>
      /// <returns>Devuelve una cadena en formato RSS</returns>
      public string ToRss()
      {
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         RssChannel rss = null;
         RssItem item = null;

         try
         {
            _ws.DataSource.Connect();

            // Implementa las propiedades del canal
            rss = new RssChannel();
            rss.Title = _ws.PropertyName + " - " + ForumsDAO.SERVICE_NAME;
            rss.Copyright = "Copyright &copy; " + _ws.PropertyName;
            rss.Language = "es-es";
            rss.ManagingEditor = _ws.Mail;
            rss.LastBuildDate = DateTime.Now;
            rss.PubDate = DateTime.Now;
            rss.Link = _ws.Url;
            rss.Image = new RssChannelImage();
            rss.Image.Url = new Uri(Cosmo.Net.Url.Combine(_ws.Url, "images", "cs_rss_image.jpg"));
            rss.Image.Link = _ws.Url;

            // Rellena las entradas de noticias al canal
            cmd = new SqlCommand("cs_RSS_GetForum", _ws.DataSource.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               Url qs = new Url(ForumsDAO.URL_THREAD);
               qs.AddParameter(ForumsDAO.PARAM_CHANNEL_ID, reader.GetInt32(5));
               qs.AddParameter(ForumsDAO.PARAM_THREAD_ID, reader.GetInt32(7));
               qs.AnchorName = "msg" + reader.GetInt32(0);

               item = new RssItem();
               item.Link = Cosmo.Net.Url.Combine(_ws.Url, qs.ToString(true));
               item.Title = reader.GetString(1);
               item.Description = reader.GetString(2);
               item.PubDate = reader.GetDateTime(3);
               item.Guid = item.Link;
               item.Category = reader.GetString(6);
               
               rss.Items.Add(item);
            }
            reader.Close();

            return rss.ToXml();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().PropertyName + ".ToRSS()",
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