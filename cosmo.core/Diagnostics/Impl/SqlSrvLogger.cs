using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Cosmo.Diagnostics.Impl
{

   /// <summary>
   /// Genera una clase de servicio para la gestión del registro de eventos.
   /// </summary>
   public class SqlSrvLogger : ILogger
   {
      // SQL constants
      private const string SQL_TABLE = "syslog";
      private const string SQL_FIELDS_SELECT = "slid, sldate, sluser, slapp, slcontext, slerrcode, slmessage, sltype, slworkspace";
      private const string SQL_FIELDS_INSERT = "slcontext, slapp, slerrcode, slmessage, slworkspace, sltype, sluser";

      // Settings constants
      private const string SETTING_MAX_ENTRIES = "max-entries";

      #region Constructors

      /// <summary>
      /// Gets a new instance of Logger
      /// </summary>
      /// <param name="workspace">Workspace actual</param>
      public SqlSrvLogger(Workspace workspace, Plugin plugin)
      {
         this.Workspace = workspace;
         this.Plugin = plugin;
      }

      #endregion

      #region Properties

      public Workspace Workspace { get; private set; }

      public Plugin Plugin { get; private set; }

      #endregion

      #region ILogger Implementation

      public void Info(LogEntry entry)
      {
         entry.Type = LogEntry.LogEntryType.EV_INFORMATION;

         Add(entry);
      }

      public void Warning(LogEntry entry)
      {
         entry.Type = LogEntry.LogEntryType.EV_WARNING;

         Add(entry);
      }

      public void Security(LogEntry entry)
      {
         entry.Type = LogEntry.LogEntryType.EV_SECURITY;

         Add(entry);
      }

      public void Error(LogEntry entry)
      {
         entry.Type = LogEntry.LogEntryType.EV_ERROR;

         Add(entry);
      }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega una entrada en el registro de eventos.
      /// </summary>
      /// <param name="logentry">Registro a agregar.</param>
      public void Add(LogEntry logentry)
      {
         string sql = string.Empty;
         SqlParameter param = null;

         // Inicializaciones
         logentry.WorkspaceName = this.Workspace.Name;

         try
         {
            sql = @"INSERT INTO " + SQL_TABLE + " (" + SQL_FIELDS_INSERT + @") 
                    VALUES (@slcontext, @slapp, @slerrcode, @slmessage, @slworkspace, @sltype, @sluser)";

            this.Workspace.DataSource.Connect();
            SqlCommand cmd = new SqlCommand(sql, this.Workspace.DataSource.Connection);

            param = new SqlParameter("@slcontext", SqlDbType.NVarChar, 255);
            param.Value = logentry.Context.Trim();
            cmd.Parameters.Add(param);

            param = new SqlParameter("@slapp", SqlDbType.NVarChar, 255);
            param.Value = logentry.ApplicationName.Trim();
            cmd.Parameters.Add(param);

            param = new SqlParameter("@slerrcode", SqlDbType.Int);
            param.Value = logentry.Code;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@slmessage", SqlDbType.NVarChar, 2048);
            param.Value = logentry.Message.Trim();
            cmd.Parameters.Add(param);

            param = new SqlParameter("@slworkspace", SqlDbType.NVarChar, 255);
            param.Value = logentry.WorkspaceName.Trim();
            cmd.Parameters.Add(param);

            param = new SqlParameter("@sltype", SqlDbType.Int);
            param.Value = (int)logentry.Type;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@sluser", SqlDbType.NVarChar, 64);
            param.Value = logentry.UserLogin;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
         }
         catch
         {
            // No hace nada
         }
         finally
         {
            this.Workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Remove an event from the Cosmo logs.
      /// </summary>
      /// <param name="id">Identificador de la entrada al registro</param>
      public void Delete(int id)
      {
         string sql = string.Empty;
         SqlParameter param = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            this.Workspace.DataSource.Connect();

            // Elimina el registro
            sql = @"DELETE 
                    FROM   " + SQL_TABLE + @"  
                    WHERE  slid = @slid";

            using (SqlCommand cmd = new SqlCommand(sql, this.Workspace.DataSource.Connection))
            {
               param = new SqlParameter("@slid", SqlDbType.Int);
               param.Value = id;
               cmd.Parameters.Add(param);

               cmd.ExecuteNonQuery();
            }
         }
         catch
         {
            throw;
         }
         finally
         {
            this.Workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Gets a log entry by its unique identifier.
      /// </summary>
      /// <param name="id">Log entry unique identifier.</param>
      /// <returns>An instance of <see cref="LogEntry"/> or <c>null</c> if the identifier doesn't exist.</returns>
      public LogEntry GetByID(int id)
      {
         string sql = string.Empty;

         try
         {
            // Abre una conexión a la BBDD del workspace
            this.Workspace.DataSource.Connect();

            sql = @"SELECT " + SQL_FIELDS_SELECT + @" 
                    FROM   " + SQL_TABLE + @" 
                    WHERE  slid = @slid";

            using (SqlCommand cmd = new SqlCommand(sql, this.Workspace.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@slid", SqlDbType.Int));
               cmd.Parameters["@slid"].Value = id;

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  if (reader.Read())
                  {
                     return ReadEntity(reader);
                  }
               }
            }

            return null;
         }
         catch
         {
            throw;
         }
         finally
         {
            this.Workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Gets a list of all entries.
      /// </summary>
      public List<LogEntry> GetAll()
      {
         int maxEntries;
         string sql = string.Empty;
         LogEntry eventlog = null;
         List<LogEntry> events = new List<LogEntry>();

         maxEntries = Plugin.GetInteger(SqlSrvLogger.SETTING_MAX_ENTRIES);

         try
         {
            // Abre una conexión a la BBDD
            this.Workspace.DataSource.Connect();

            sql = @"SELECT    " + (maxEntries > 0 ? "TOP " + maxEntries + " " : string.Empty) + SQL_FIELDS_SELECT + @" 
                    FROM      " + SQL_TABLE + @" 
                    ORDER BY  sldate DESC";

            using (SqlCommand cmd = new SqlCommand(sql, this.Workspace.DataSource.Connection))
            {
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     eventlog = ReadEntity(reader);
                     if (eventlog != null)
                     {
                        events.Add(eventlog);
                     }
                  }
               }
            }

            return events;
         }
         catch
         {
            throw;
         }
         finally
         {
            this.Workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Genera una lista de los objetos contenidos en una carpeta.
      /// </summary>
      /// <param name="status">Identificador de la carpeta que contiene los objetos.</param>
      public List<LogEntry> GetByType(Cosmo.Diagnostics.LogEntry.LogEntryType status)
      {
         string sql = string.Empty;
         LogEntry eventlog = null;
         List<LogEntry> events = new List<LogEntry>();

         try
         {
            // Abre una conexión a la BBDD
            this.Workspace.DataSource.Connect();

            sql = @"SELECT    " + SQL_FIELDS_SELECT + @" 
                    FROM      " + SQL_TABLE + @" 
                    WHERE     sltype = @sltype 
                    ORDER BY  sldate DESC";

            using (SqlCommand cmd = new SqlCommand(sql, this.Workspace.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@sltype", SqlDbType.Int));
               cmd.Parameters["@sltype"].Value = (int)status;

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     eventlog = ReadEntity(reader);
                     if (eventlog != null)
                     {
                        events.Add(eventlog);
                     }
                  }
               }
            }

            return events;
         }
         catch
         {
            throw;
         }
         finally
         {
            this.Workspace.DataSource.Disconnect();
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Gets a log entry from reader.
      /// </summary>
      private LogEntry ReadEntity(SqlDataReader reader)
      {
         LogEntry logEntry = new LogEntry();
         logEntry.ID = (int)reader[0];
         logEntry.Date = (DateTime)reader[1];
         logEntry.UserLogin = !reader.IsDBNull(2) ? (string)reader[2] : string.Empty;
         logEntry.ApplicationName = !reader.IsDBNull(3) ? (string)reader[3] : string.Empty;
         logEntry.Context = !reader.IsDBNull(4) ? (string)reader[4] : string.Empty;
         logEntry.Code = !reader.IsDBNull(5) ? (int)reader[5] : 0;
         logEntry.Message = !reader.IsDBNull(6) ? (string)reader[6] : string.Empty;
         logEntry.Type = !reader.IsDBNull(7) ? (LogEntry.LogEntryType)reader[7] : LogEntry.LogEntryType.EV_INFORMATION;
         logEntry.WorkspaceName = !reader.IsDBNull(8) ? (string)reader[8] : string.Empty;

         return logEntry;
      }

      #endregion

   }
}
