using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Cosmo.Diagnostics
{

   /// <summary>
   /// Genera una clase de servicio para la gestión del registro de eventos.
   /// </summary>
   public class LoggerService
   {
      private Workspace _workspace = null;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de Logger
      /// </summary>
      /// <param name="workspace">Workspace actual</param>
      public LoggerService(Workspace workspace)
      {
         _workspace = workspace;
      }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega una entrada en el registro de eventos.
      /// </summary>
      /// <param name="workspace">Workspace actual.</param>
      /// <param name="logentry">Registro a agregar.</param>
      public static void Add(Workspace workspace, LogEntry logentry)
      {
         string sql = string.Empty;
         SqlParameter param = null;

         // Inicializaciones
         logentry.WorkspaceName = workspace.Name;

         try
         {
            sql = "INSERT INTO syslog (slcontext, slapp, slerrcode, slmessage, slworkspace, sltype, sluser) " +
                  "VALUES (@slcontext, @slapp, @slerrcode, @slmessage, @slworkspace, @sltype, @sluser)";

            workspace.DataSource.Connect();
            SqlCommand cmd = new SqlCommand(sql, workspace.DataSource.Connection);

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
            workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Registra una entrada en el archivo de LOG del Workspace.
      /// </summary>
      /// <param name="entry">Una entrada descrita por una instancia de LogEntry.</param>
      public void Add(LogEntry entry)
      {
         Add(_workspace, entry);
      }

      /// <summary>
      /// Registra una entrada en el archivo de LOG del Workspace.
      /// </summary>
      /// <param name="message">Descripción del evento.</param>
      /// <param name="appName">Nombre de la aplicación.</param>
      /// <param name="type">Tipo de evento.</param>
      /// <param name="module">Módulo dónde se produce el evento.</param>
      /// <param name="code">Código del evento.</param>
      /// <returns>Un valor booleano indicando el resultado de la operación.</returns>
      public void Add(string message, string appName, LogEntry.LogEntryType type, string module, int code)
      {
         Add(_workspace, new LogEntry(appName, module.Trim(), code, message.Trim(), LogEntry.LOGIN_SYSTEM, type));
      }

      /// <summary>
      /// Registra una entrada en el archivo de LOG del Workspace.
      /// </summary>
      /// <param name="message">Descripción del evento.</param>
      /// <param name="appName">Nombre de la aplicación.</param>
      /// <param name="type">Tipo de evento.</param>
      /// <param name="module">Módulo dónde se produce el evento.</param>
      /// <returns>Un valor booleano indicando el resultado de la operación.</returns>
      public void Add(string message, string appName, LogEntry.LogEntryType type, string module)
      {
         Add(message, appName, type, module, 0);
      }

      /// <summary>
      /// Registra una entrada en el archivo de LOG del Workspace.
      /// </summary>
      /// <param name="message">Descripción del evento.</param>
      /// <param name="type">Tipo de evento.</param>
      /// <param name="module">Módulo dónde se produce el evento.</param>
      /// <returns>Un valor booleano indicando el resultado de la operación.</returns>
      public void Add(string message, LogEntry.LogEntryType type, string module)
      {
         Add(message, Assembly.GetEntryAssembly().FullName, type, module, 0);
      }

      /// <summary>
      /// Genera una lista de los objetos contenidos en una carpeta.
      /// </summary>
      /// <param name="status">Identificador de la carpeta que contiene los objetos.</param>
      public List<LogEntry> List(int status)
      {
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         List<LogEntry> events = new List<LogEntry>();

         try
         {
            // Abre una conexión a la BBDD
            _workspace.DataSource.Connect();

            string sql = "SELECT slid,sldate,sluser,slapp,slcontext,slerrcode,slmessage,sltype,slworkspace " +
                         "FROM syslog " +
                         "WHERE sltype=@sltype " +
                         "ORDER BY sldate DESC";

            cmd = new SqlCommand(sql, _workspace.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@sltype", SqlDbType.Int));
            cmd.Parameters["@sltype"].Value = status;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               LogEntry eventlog = new LogEntry();
               eventlog.ID = (int)reader[0];
               eventlog.Date = (DateTime)reader[1];
               eventlog.UserLogin = !reader.IsDBNull(2) ? (string)reader[2] : "";
               eventlog.ApplicationName = !reader.IsDBNull(3) ? (string)reader[3] : "";
               eventlog.Context = !reader.IsDBNull(4) ? (string)reader[4] : "";
               eventlog.Code = !reader.IsDBNull(5) ? (int)reader[5] : 0;
               eventlog.Message = !reader.IsDBNull(6) ? (string)reader[6] : "";
               eventlog.Type = !reader.IsDBNull(7) ? (LogEntry.LogEntryType)reader[7] : LogEntry.LogEntryType.EV_INFORMATION;
               eventlog.WorkspaceName = !reader.IsDBNull(8) ? (string)reader[8] : "";

               events.Add(eventlog);
            }
            reader.Close();

            return events;
         }
         catch
         {
            throw;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Permite recuperar los datos de un evento.
      /// </summary>
      /// <param name="id">Identificador de la entrada del registro.</param>
      /// <returns>Una instáncia de <see cref="LogEntry"/>.</returns>
      public LogEntry Item(int id)
      {
         LogEntry eventlog = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _workspace.DataSource.Connect();

            string sql = "SELECT slid,sldate,sluser,slapp,slcontext,slerrcode,slmessage,sltype,slworkspace " +
                         "FROM syslog " +
                         "WHERE slid=@slid";

            cmd = new SqlCommand(sql, _workspace.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@slid", SqlDbType.Int));
            cmd.Parameters["@slid"].Value = id;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               eventlog = new LogEntry();
               eventlog.ID = (int)reader[0];
               eventlog.Date = (DateTime)reader[1];
               eventlog.UserLogin = !reader.IsDBNull(2) ? (string)reader[2] : string.Empty;
               eventlog.ApplicationName = !reader.IsDBNull(3) ? (string)reader[3] : "";
               eventlog.Context = !reader.IsDBNull(4) ? (string)reader[4] : "";
               eventlog.Code = !reader.IsDBNull(5) ? (int)reader[5] : 0;
               eventlog.Message = !reader.IsDBNull(6) ? (string)reader[6] : "";
               eventlog.Type = !reader.IsDBNull(7) ? (LogEntry.LogEntryType)reader[7] : LogEntry.LogEntryType.EV_INFORMATION;
               eventlog.WorkspaceName = !reader.IsDBNull(8) ? (string)reader[8] : "";
            }
            reader.Close();

            return eventlog;
         }
         catch
         {
            throw;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina un evento del workspace.
      /// </summary>
      /// <param name="id">Identificador de la entrada al registro</param>
      public void Delete(int id)
      {
         SqlCommand cmd = null;
         SqlParameter param = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _workspace.DataSource.Connect();

            // Elimina el registro
            string sql = "DELETE FROM syslog WHERE slid=@slid";

            cmd = new SqlCommand(sql, _workspace.DataSource.Connection);

            param = new SqlParameter("@slid", SqlDbType.Int);
            param.Value = id;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
         }
         catch
         {
            throw;
         }
         finally
         {
            cmd.Dispose();
            _workspace.DataSource.Disconnect();
         }
      }

      #endregion

   }
}
