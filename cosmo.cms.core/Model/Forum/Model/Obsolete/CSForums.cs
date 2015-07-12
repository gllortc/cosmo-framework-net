using Cosmo.Diagnostics;
using Cosmo.Net;
using Cosmo.Net.Rss;
using Cosmo.Security;
using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;

namespace Cosmo.Cms.Forums.Model.Obsolete
{

   /// <summary>
   /// Implementa una clase para gestionar los canales de los foros.
   /// </summary>
   public class CSForums
   {
      // Declaración de variables internas
      private Workspace _ws = null;

      public static int MaxThreadsPerPage = 40;

      // Parámetros para uso en páginas ASPX
      public const string PARAM_CHANNEL_ID = "ch";
      public const string PARAM_THREAD_ID = "th";
      public const string PARAM_MESSAGE_ID = "msg";
      public const string PARAM_PAGE_NUM = "p";
      public const string PARAM_ORDER = "o";

      public const string SERVICE_NAME = "Foros de discusión";

      public const string URL_HOME = "cs_forum.aspx";
      public const string URL_CHANNEL = "cs_forum_ch.aspx";
      public const string URL_THREAD = "cs_forum_th.aspx";
      public const string URL_MESSAGE = "cs_forum_msg.aspx";
      public const string URL_RULES = "cs_forum_rules.aspx";
      public const string URL_USERTHREADS = "cs_forum_usr.aspx";

      private const string SQL_SELECT_FORUM = "forumid,forumname,forumdesc,forumdate,forumenabled,forumowner,(SELECT Count(*) FROM forum WHERE msgforumid=forums.forumid) as items";
      private const string SQL_SELECT_THREAD = "fldauto,msgforumid,fldtitle,flddate,msglastreply,msgnummsgs,fldname,msguserid,msgclosed";
      private const string SQL_SELECT_MESSAGE = "msgclosed,fldauto,msguserid,msgforumid,fldreply,fldname,fldtitle,fldbody,fldip,flddate,msgbbcodes";

      /// <summary>
      /// Constructor de la clase
      /// </summary>
      /// <param name="website">Una instancia del site actual</param>
      public CSForums(Workspace website)
      {
         _ws = website;
      }

      #region Methods

      //=================================================================
      // FOROS
      //=================================================================

      /// <summary>
      /// Devuelve un canal del foro.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <returns>Una instáncia de CSForum.</returns>
      public CSForum GetForum(int channelid)
      {
         CSForum forum = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene el canal
            string sql = "SELECT " + SQL_SELECT_FORUM + " FROM forums WHERE forumid=@forumid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@forumid", channelid));

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               /*forum = new CSForum();
               forum.Id = reader.GetInt32(0);
               forum.Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
               forum.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
               forum.Created = reader.GetDateTime(3);
               forum.Published = reader.GetBoolean(4);
               forum.Owner = reader.IsDBNull(5) ? WebWorkspace.DEFAULT_USER : reader.GetString(5);
               forum.MessageCount = reader.GetInt32(6);*/

               forum = ReadForum(reader);
            }
            else
               throw new Exception("El canal " + channelid + " no existe o no está disponible en estos momentos.");

            reader.Close();

            return forum;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".GetForum()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            // _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve un array de objetos CSForum correspondiente a los canales del foro.
      /// </summary>
      /// <param name="onlyPublished">Indica si sólo recupera los canales publicados.</param>
      /// <returns>Un array de objetos CSForum.</returns>
      public List<CSForum> GetForums(bool published)
      {
         /*string sql = "SELECT forumid,forumname,forumdesc,forumdate,forumenabled,forumowner," +
                             "(SELECT Count(*) FROM forum WHERE msgforumid=forums.forumid) as items " +
                      "FROM forums " +
                      "WHERE forumenabled=@forumenabled";*/
         // CSForum forum = null;
         List<CSForum> forums = new List<CSForum>();
         SqlCommand cmd = null;
         SqlParameter param = null;
         SqlDataReader reader = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene los foros
            // string sql = "SELECT " + SQL_SELECT_FORUM + " FROM forums WHERE forumenabled=@forumenabled";
            cmd = new SqlCommand("cs_Forum_GetChannels", _ws.DataSource.Connection);
            cmd.CommandType = CommandType.StoredProcedure;

            param = new SqlParameter("@forumstatus", SqlDbType.Bit);
            param.Value = published;
            cmd.Parameters.Add(param);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               /*forum = new CSForum();
               forum.Id = reader.GetInt32(0);
               forum.Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
               forum.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
               forum.Created = reader.GetDateTime(3);
               forum.Published = reader.GetBoolean(4);
               forum.Owner = reader.IsDBNull(5) ? WebWorkspace.DEFAULT_USER : reader.GetString(5);
               forum.MessageCount = reader.GetInt32(6);

               forums.Add(forum);*/
               
               forums.Add(ReadForum(reader));
            }
            reader.Close();

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
            reader.Dispose();
            cmd.Dispose();
            // _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve un array de objetos CSForum correspondiente a los canales del foro.
      /// </summary>
      /// <returns>Un array de objetos CSForum.</returns>
      public List<CSForum> GetForums()
      {
         /*string sql = "SELECT forumid,forumname,forumdesc,forumdate,forumenabled,forumowner," +
                             "(SELECT Count(*) FROM forum WHERE msgforumid=forums.forumid) as items " +
                      "FROM forums";
         CSForum forum = null;*/
         List<CSForum> forums = new List<CSForum>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene los foros
            string sql = "SELECT " + SQL_SELECT_FORUM + " FROM forums";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               /*forum = new CSForum();
               forum.Id = reader.GetInt32(0);
               forum.Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
               forum.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
               forum.Created = reader.GetDateTime(3);
               forum.Published = reader.GetBoolean(4);
               forum.Owner = reader.IsDBNull(5) ? WebWorkspace.DEFAULT_USER : reader.GetString(5);
               forum.MessageCount = reader.GetInt32(6);

               forums.Add(forum);*/

               forums.Add(ReadForum(reader));
            }
            reader.Close();

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
            reader.Dispose();
            cmd.Dispose();
            // _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene el número de mensajes de un canal.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <returns>El número de mensajes del canal.</returns>
      public int GetForumMessagesCount(int channelid)
      {
         string sql = "SELECT Count(*) " +
                      "FROM forum " +
                      "WHERE msgforumid=@msgforumid";
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

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
            cmd.Dispose();
            // _ws.DataSource.Disconnect();
         }
      }

      //=================================================================
      // CANALES
      //=================================================================

      /// <summary>
      /// Obtiene las cabeceras de los hilos de un canal.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <param name="page">Número de página.</param>
      /// <param name="rowsperpage">Número de threads por página.</param>
      /// <returns>Un array de objetos CSForumThread[].</returns>
      /// <remarks>
      /// La estrategia SQL para paginación se basa en el siguiente artículo: 
      /// <a href="http://weblogs.sqlteam.com/jeffs/archive/2003/12/22/672.aspx">weblogs.sqlteam.com/jeffs/archive/2003/12/22/672.aspx</a>
      /// </remarks>
      public List<CSForumThread> GetChannelThreads(int channelid, int page, int rowsperpage)
      {
         /*List<CSForumThread> threads = new List<CSForumThread>();
         SqlDataAdapter adapter = null;
         DataSet dataSet = null;
         DataTableReader reader = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene los mensjaes de nivel superior de la página a mostrar
            string sql = "SELECT " + SQL_SELECT_THREAD + " FROM forum WHERE fldreply=0 And msgforumid=" + channelid + " ORDER BY msglastreply Desc";
            adapter = new SqlDataAdapter(sql, _ws.DataSource.Connection);
            dataSet = new DataSet();
            adapter.Fill(dataSet, (page - 1) * rowsperpage, rowsperpage, "forum");
            reader = dataSet.CreateDataReader();

            // Carga la página
            while (reader.Read())
            {
               threads.Add(ReadThread(reader));
            }
            reader.Close();

            return threads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, "CSForums.GetChannelThreads(int, int, int)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            reader.Dispose();
            adapter.Dispose();
            _ws.DataSource.Disconnect();
         }*/

         SqlCommand cmd = null;
         SqlParameter param = null;
         SqlDataReader reader = null;
         List<CSForumThread> threads = new List<CSForumThread>();

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

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               threads.Add(ReadThread(reader, false));
            }
            reader.Close();

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
            reader.Dispose();
         }
      }

      /// <summary>
      /// Obtiene las cabeceras de los hilos de un canal. Recupera el número de Threads indicado por MaxThreadsPerPage.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <param name="page">Número de página.</param>
      /// <returns>Un array de objetos CSForumThread[].</returns>
      public List<CSForumThread> GetChannelThreads(int channelid, int page)
      {
         return this.GetChannelThreads(channelid, page, CSForums.MaxThreadsPerPage);
      }

      /// <summary>
      /// Obtiene el número de hilos de un canal.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <returns>El número de threads del canal.</returns>
      public int GetChannelThreadsCount(int channelid)
      {
         string sql = "SELECT Count(*) As nregs " +
                      "FROM forum " +
                      "WHERE fldreply=0 And msgforumid=@msgforumid";
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

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
            cmd.Dispose();
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
      public CSForumThread GetThread(int threadid)
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
      public CSForumThread GetThread(int threadid, bool getMessages, ThreadMessagesOrder order)
      {
         bool head = true;
         CSForumThread thread = null;
         SqlCommand cmd = null;
         SqlParameter param = null;
         SqlDataReader reader = null;

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

               reader = cmd.ExecuteReader();
               while (reader.Read())
               {
                  if (head)
                  {
                     thread = new CSForumThread();
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
                        if (thread.LastReply < DateTime.Now.AddDays(-7))
                           thread.Closed = true;

                     head = false;
                  }

                  CSForumMessage message = new CSForumMessage();
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
               reader.Close();

               // Si se especifica un órden descendiente, se reordena la lista de mensajes
               if (order == ThreadMessagesOrder.Descending)
               {
                  thread.Messages.Sort(delegate(CSForumMessage p1, CSForumMessage p2) { return p2.ID.CompareTo(p1.ID); });
               }
            }
            else
            {
               // Obtiene las propiedades del hilo
               string sql = "SELECT " + SQL_SELECT_THREAD + " FROM forum WHERE fldauto=@fldauto";
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@fldauto", threadid));

               reader = cmd.ExecuteReader();
               if (reader.Read())
               {
                  thread = ReadThread(reader, false);
               }
               reader.Close();
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
            reader.Dispose();
            cmd.Dispose();
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
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            if (_ws.DataSource.Connection.State != ConnectionState.Open) _ws.DataSource.Connect();

            // Comprueba si el hilo está cerrado
            sql = "SELECT msgclosed FROM forum WHERE fldauto=@fldauto";

            if (transaction != null)
               cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            else
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fldauto", thid));

            return !(bool)cmd.ExecuteScalar();
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
            cmd.Dispose();
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
      /// <param name="page">Número de página.</param>
      /// <param name="rowsperpage">Número de threads por página.</param>
      /// <returns>Un array de objetos CSForumThread[].</returns>
      public List<CSForumThread> GetUserThreads(int uid, int page, int rowsperpage)
      {
         List<CSForumThread> threads = new List<CSForumThread>();
         SqlDataAdapter adapter = null;
         DataSet dataSet = null;
         DataTableReader reader = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene los mensjaes de nivel superior de la página a mostrar
            string sql = "SELECT forum.fldauto, forums.forumid, forum.fldtitle, forum.flddate, forum.msglastreply, forum.msgnummsgs, forum.fldname, forum.msguserid, forum.msgclosed, forums.forumname " +
                         "FROM forum INNER JOIN forums ON (forum.msgforumid=forums.forumid) " +
                         "WHERE fldauto IN (SELECT DISTINCT msgthread FROM forum WHERE msguserid=" + uid + ") " +
                         "ORDER BY forum.msglastreply DESC";
            adapter = new SqlDataAdapter(sql, _ws.DataSource.Connection);
            dataSet = new DataSet();
            adapter.Fill(dataSet, (page - 1) * rowsperpage, rowsperpage, "forum");
            reader = dataSet.CreateDataReader();

            // Carga la página
            while (reader.Read())
            {
               /*thread = new CSForumThread();
               thread.Id = reader.GetInt32(0);
               thread.ForumID = channelid;
               thread.Title = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
               thread.Created = reader.GetDateTime(2);
               thread.LastReply = reader.GetDateTime(3);
               thread.MessageCount = reader.GetInt32(4);
               thread.AuthorName = reader.IsDBNull(5) ? "<unknown>" : reader.GetString(5);
               thread.AuthorID = reader.GetInt32(6);
               thread.Closed = reader.GetBoolean(7);

               // Cierra automáticamente los hilos de 4 meses de antiguedad y que no tengan actividad en los últimos 7 días
               if (thread.Created < DateTime.Now.AddMonths(-4)) 
                  if (thread.LastReply < DateTime.Now.AddDays(-7))
                     thread.Closed = true;

               threads.Add(thread);*/

               threads.Add(ReadThread(reader));
            }
            reader.Close();

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
            reader.Dispose();
            adapter.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene las cabeceras de los hilos de un canal. Recupera el número de Threads indicado por MaxThreadsPerPage.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <param name="page">Número de página.</param>
      /// <returns>Un array de objetos CSForumThread[].</returns>
      public List<CSForumThread> GetUserThreads(int uid, int page)
      {
         return this.GetUserThreads(uid, page, CSForums.MaxThreadsPerPage);
      }

      /// <summary>
      /// Obtiene el número de hilos de un canal.
      /// </summary>
      /// <param name="channelid">Identificador del canal.</param>
      /// <returns>El número de threads del canal.</returns>
      public int GetUserThreadsCount(int uid)
      {
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene el número de canales
            string sql = "SELECT Count(*) FROM forum WHERE fldauto In (SELECT DISTINCT msgthread FROM forum WHERE msguserid=@msguserid)";
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
            cmd.Dispose();
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
      public CSForumMessage GetMessage(int messageid)
      {
         /*string sql = "SELECT msgclosed,fldauto,msguserid,msgforumid,fldreply,fldname,fldtitle,fldbody,fldip,flddate,msgbbcodes " +
                      "FROM forum " +
                      "WHERE fldauto=@fldauto";*/
         CSForumMessage message = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            // Abre una conexión a la BBDD del Workspace
            _ws.DataSource.Connect();

            // Obtiene el canal
            string sql = "SELECT " + SQL_SELECT_MESSAGE + " FROM forum WHERE fldauto=@fldauto";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@fldauto", messageid));

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               /*message = new CSForumMessage();
               message.ThreadClosed = reader.GetBoolean(0);
               message.Id = reader.GetInt32(1);
               message.UserID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
               message.ForumID = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
               message.ParentMessageID = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
               message.Name = reader.IsDBNull(5) ? "<unknown>" : reader.GetString(5);
               message.Title = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
               message.Body = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
               message.IP = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
               message.Created = reader.GetDateTime(9);
               message.BBCodes = reader.GetBoolean(10);*/

               message = ReadMessage(reader);
            }
            else
               throw new Exception("El mensaje #" + messageid + " no existe o no está disponible en estos momentos.");

            reader.Close();

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
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Añade un mensaje a un canal o thread.
      /// </summary>
      /// <param name="message">Una instáncia de CSForumMessage.</param>
      public CSForumMessage AddMessage(CSForumMessage message)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         // Abre una conexión con la BBDD err inicia una transacción
         _ws.DataSource.Connect();
         SqlTransaction dbtrans = _ws.DataSource.Connection.BeginTransaction();

         // Determina si el mensaje contiene bbCodes
         Regex rex = new Regex(@"\[.*\]");
         message.BBCodes = rex.Match(message.Body).Success;

         try
         {
            // Genera la senténcia T/SQL a ejecutar
            if (message.ParentMessageID == 0)
            {
               // Comprueba si el usuario puede crear un nuevo thread
               if (!CanPostNewThread(message.UserID, dbtrans))
                  throw new CSSendFormException("No ha sido posible crear el nuevo hilo debido a que aa superado el número de nuevos hilos abiertos por día. Deberá esperar 24h para volver a postear.");

               cmd = new SqlCommand("cs_Forum_NewThread", _ws.DataSource.Connection, dbtrans);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add(new SqlParameter("@sTitle", message.Title));
               cmd.Parameters.Add(new SqlParameter("@sBody", message.Body));
               cmd.Parameters.Add(new SqlParameter("@sUser", message.Name));
               cmd.Parameters.Add(new SqlParameter("@sMail", ""));
               cmd.Parameters.Add(new SqlParameter("@sIPAdd", message.IP));
               cmd.Parameters.Add(new SqlParameter("@iChannel", message.ForumID));
               cmd.Parameters.Add(new SqlParameter("@iUserID", message.UserID));
               cmd.Parameters.Add(new SqlParameter("@bbcodes", message.BBCodes));
            }
            else
            {
               // Chequea si se puede postear en el hilo
               if (!GetThreadStatus(message.ParentMessageID, dbtrans))
                  throw new CSSendFormException("El hilo está cerrado y no admite más mensajes.");

               // Chequea si la IP ha sido usada por otro usuario en un periodo de tiempo demasiado corto (subplantación)
               if (!CheckIP(message, dbtrans))
                  throw new SecurityException("El hilo está cerrado y no admite más mensajes.");

               cmd = new SqlCommand("cs_Forum_ThreadAddPost", _ws.DataSource.Connection, dbtrans);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add(new SqlParameter("@iThread", message.ParentMessageID));
               cmd.Parameters.Add(new SqlParameter("@sBody", message.Body));
               cmd.Parameters.Add(new SqlParameter("@sUser", message.Name));
               cmd.Parameters.Add(new SqlParameter("@sMail", ""));
               cmd.Parameters.Add(new SqlParameter("@sIPAdd", message.IP));
               cmd.Parameters.Add(new SqlParameter("@iChannel", message.ForumID));
               cmd.Parameters.Add(new SqlParameter("@iUserID", message.UserID));
               cmd.Parameters.Add(new SqlParameter("@bbcodes", message.BBCodes));
            }

            cmd.ExecuteNonQuery();

            // Averigua el Id del nuevo mensaje
            sql = "SELECT TOP 1 fldauto AS id " +
                  "FROM forum " +
                  "ORDER BY fldauto DESC";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, dbtrans);
            message.ID = (int)cmd.ExecuteScalar();

            // Cierra la transacción
            dbtrans.Commit();

            return message;
         }
         catch (CSSendFormException ex)
         {
            dbtrans.Rollback();
            throw ex;
         }
         catch (SecurityException ex)
         {
            dbtrans.Rollback();
            throw ex;
         }
         catch (Exception ex)
         {
            dbtrans.Rollback();

            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".AddMessage(CSForumMessage)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            if (cmd != null) cmd.Dispose();
            if (dbtrans != null) dbtrans.Dispose();

            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Actualiza el contenido de un mensaje.
      /// </summary>
      /// <param name="message">Una instáncia de CSForumMessage.</param>
      /// <remarks>Sólo se permite actualizar el cuerpo del mensaje y el título si se informa ( si está vacío, no se actualiza).</remarks>
      public void UpdateMessage(CSForumMessage message)
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

            sql = "UPDATE forum " +
                  "SET fldbody=@fldbody" + (string.IsNullOrEmpty(message.Title) ? "" : ", fldtitle=@fldtitle") + ", msgbbcodes=@msgbbcodes " +
                  "WHERE fldauto=@fldauto";

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
                                        GetType().Name + ".UpdateMessage(CSForumMessage)", 
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
      /// Obtiene todos los mensajes de un thread. En la posición 0 se encuentra la cabecera del Thread.
      /// </summary>
      /// <param name="threadid">Identificador del mensaje de nivel superior.</param>
      /// <param name="order">Tipo de ordenación de los mensajes del thread</param>
      /// <returns>Un array de objetos CSForumMessage.</returns>
      public List<CSForumMessage> GetThreadMessages(int threadid, ThreadMessagesOrder order)
      {
         // bool closed = false;
         // int idx = 0;

         // CSForumMessage message = null;
         List<CSForumMessage> messages = new List<CSForumMessage>();
         SqlCommand cmd = null;
         SqlParameter param = null;
         SqlDataReader reader = null;

         try
         {
            // Abre una conexión con la BBDD
            _ws.DataSource.Connect();

            // Obtiene los mensajes
            /*string sql = "SELECT msgclosed,fldauto,msguserid,msgforumid,fldreply,fldname,fldtitle,fldbody,fldip,flddate,msgbbcodes " +
                         "FROM forum " +
                         "WHERE fldauto=@fldauto Or fldreply=@fldreply " +
                         "ORDER BY fldauto " + (order == ThreadMessagesOrder.Ascending ? "Asc" : "Desc");*/
            string sql = "SELECT " + SQL_SELECT_MESSAGE + " FROM forum WHERE fldauto=@fldauto Or fldreply=@fldreply " +
                         "ORDER BY fldauto " + (order == ThreadMessagesOrder.Ascending ? "Asc" : "Desc");
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);

            param = new SqlParameter("@fldauto", SqlDbType.Int);
            param.Value = threadid;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@fldreply", SqlDbType.Int);
            param.Value = threadid;
            cmd.Parameters.Add(param);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               /*if (idx == 0) closed = reader.GetBoolean(0);

               message = new CSForumMessage();
               message.Id = reader.GetInt32(1);
               message.UserID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
               message.ForumID = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
               message.ParentMessageID = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
               message.Name = reader.IsDBNull(5) ? "<unknown>" : reader.GetString(5);
               message.Title = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
               message.Body = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
               message.IP = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
               message.Created = reader.GetDateTime(9);
               message.ThreadClosed = closed;
               message.BBCodes = reader.GetBoolean(10);

               messages.Add(message);*/

               messages.Add(ReadMessage(reader));
            }
            reader.Close();

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
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
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
            rss.Title = _ws.Name + " - " + CSForums.SERVICE_NAME;
            rss.Copyright = "Copyright &copy; " + _ws.Name;
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
               Url qs = new Url(CSForums.URL_THREAD, "msg" + reader.GetInt32(0));
               qs.AddParameter(CSForums.PARAM_CHANNEL_ID, reader.GetInt32(5));
               qs.AddParameter(CSForums.PARAM_THREAD_ID, reader.GetInt32(7));

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
                                        GetType().Name + ".ToRSS()", 
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
      /// Genera un canal RSS 2.0 que contiene los últimos mensajes publicados en el foro usando la caché de servidor web
      /// </summary>
      /// <param name="cache">La instáncia Cache del servidor web</param>
      /// <param name="forceUpdate">Indica si se debe forzar al refresco del contenido del canal si usar la caché.</param>
      /// <returns>Devuelve una cadena en formato RSS</returns>
      public string ToRss(System.Web.Caching.Cache cache, bool forceUpdate)
      {
         // Calcula el número de segundos de validez
         int secs = _ws.Properties.GetInt(Cms.RSSCacheTimeout, 3600);

         // Guarda el feed en caché
         if (cache[Cms.CACHE_RSS_FORUMS] == null || forceUpdate)
         {
            cache.Insert(Cms.CACHE_RSS_FORUMS, ToRss(), null, DateTime.Now.AddSeconds(secs), TimeSpan.Zero);
         }

         // Devuelve el contenido de la caché
         return cache[Cms.CACHE_RSS_FORUMS].ToString();
      }

      /// <summary>
      /// Testea si el usuario puede abrir un nuevo hilo.
      /// </summary>
      /// <param name="uid">Identificador único del usuario (UID).</param>
      /// <returns></returns>
      public bool CanPostNewThread(int uid, SqlTransaction transaction)
      {
         // Si está desactivada la limitación, permite postear nuevos hilos
         if (_ws.Properties.GetInt(Cms.ForumLimitNewThreadsNumber) == 0) return true;

         SqlCommand cmd = null;

         try
         {
            // Abre una conexión con la BBDD
            _ws.DataSource.Connect();

            // Obtiene el número de hilos abiertos en un dia
            string sql = "SELECT Count(*) " +
                         "FROM forum " +
                         "WHERE msguserid=@msguserid And DateDiff(day,getdate(),flddate)=0 And fldreply=0";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@msguserid", uid));
            int rows = (int)cmd.ExecuteScalar();

            return (rows < _ws.Properties.GetInt(Cms.ForumLimitNewThreadsNumber));
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
            cmd.Dispose();
            // _ws.DataSource.Disconnect();
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
      public bool CheckIP(CSForumMessage message, SqlTransaction transaction)
      {
         string sql = "";
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            // Comprueba que una persona no envie mensajes en un mismo hilo con nombres distintos
            sql = "SELECT fldauto, msguserid, msgforumid, fldreply, fldname, fldtitle, fldip, flddate, msglastreply, msgthread, msgclosed " +
                  "FROM forum " +
                  "WHERE msgthread=@msgthread And fldip=@fldip And fldname<>@fldname";

            if (transaction != null)
            {
               cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            }
            else
            {
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            }

            cmd.Parameters.Add(new SqlParameter("@msgthread", message.ParentMessageID));
            cmd.Parameters.Add(new SqlParameter("@fldip", message.IP));
            cmd.Parameters.Add(new SqlParameter("@fldname", message.Name));

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               // Check: Identidad múltiple
               if (Cosmo.Utils.Calendar.DateDiff(Cosmo.Utils.Calendar.DateInterval.Hour, reader.GetDateTime(7), DateTime.Now) <= 2)
               {
                  string msg = "Identidad múltiple: El usuario " + message.Name.ToUpper() + " intenta escribir en el hilo " + message.ParentMessageID + " con identidad distinta a " + reader.GetString(4).ToUpper() + " usando la IP " + message.IP;
                  _ws.Logger.Add(new LogEntry(Cms.ProductName, "CSForums.CheckIP()", msg, LogEntry.LogEntryType.EV_SECURITY));
                  reader.Close();
                  return false;
               }
            }
            reader.Close();

            return true;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".CheckIP()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
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
      public void CheckIP(CSForumMessage message)
      {
         CheckIP(message, null);
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
            if (!int.TryParse(request[CSForums.PARAM_ORDER], out torder))
            {
               if (request.Cookies["cs.forum"] != null)    // No ha podido recuperar el parámetro ex intenta recuperar la cookie
               {
                  if (request.Cookies["cs.forum"]["msgs.order"] != null)
                  {
                     if (!int.TryParse(request.Cookies["cs.forum"]["msgs.order"], out torder))
                     {
                        torder = (int)ThreadMessagesOrder.Ascending;
                     }
                  }
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

      #region Private Members

      private CSForum ReadForum(SqlDataReader reader)
      {
         CSForum forum = null;

         forum = new CSForum();
         forum.ID = reader.GetInt32(0);
         forum.Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
         forum.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
         forum.Date = reader.GetDateTime(3);
         forum.Published = reader.GetBoolean(4);
         forum.Owner = reader.IsDBNull(5) ? AuthenticationService.ACCOUNT_SUPER : reader.GetString(5);
         forum.MessageCount = reader.GetInt32(6);

         return forum;
      }

      private CSForumThread ReadThread(DataTableReader reader)
      {
         CSForumThread thread = null;

         thread = new CSForumThread();
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
         if (thread.Created < DateTime.Now.AddMonths(-4))
            if (thread.LastReply < DateTime.Now.AddDays(-7))
               thread.Closed = true;

         return thread;
      }

      private CSForumThread ReadThread(DataTableReader reader, bool getChannelName)
      {
         CSForumThread thread = null;

         thread = new CSForumThread();
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
         if (thread.Created < DateTime.Now.AddMonths(-4))
            if (thread.LastReply < DateTime.Now.AddDays(-7))
               thread.Closed = true;

         return thread;
      }

      private CSForumThread ReadThread(SqlDataReader reader, bool getChannelName)
      {
         CSForumThread thread = null;

         thread = new CSForumThread();
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
         if (thread.Created < DateTime.Now.AddMonths(-4))
            if (thread.LastReply < DateTime.Now.AddDays(-7))
               thread.Closed = true;

         return thread;
      }

      private CSForumMessage ReadMessage(SqlDataReader reader)
      {
         CSForumMessage message = new CSForumMessage();
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

   }
}