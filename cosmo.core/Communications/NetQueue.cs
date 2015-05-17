using Cosmo.Diagnostics;
using MailBee.Mime;
using System;
using System.Data.SqlClient;
using System.IO;

namespace Cosmo.Communications
{

   #region Enumerations

   /// <summary>
   /// Tipos de trabajo que acepta la cola de un workspace
   /// </summary>
   public enum MWNetQueueType : int
   {
      /// <summary>Mensaje de correo</summary>
      Mail = 0,
      /// <summary>Mensaje de FAX</summary>
      FAX = 1,
      /// <summary>Impresión en impresora local del servidor</summary>
      Print = 2
   }

   #endregion

   /// <summary>
   /// Implementa una clase de servicio para la gestión de la cola de trabajos de un workspace
   /// </summary>
   public class NetQueue
   {
      private Workspace _ws;

      /// <summary>Número máximo de intentos de envío antes de producir error.</summary>
      public const int QUEUE_MAX_RETRY = 5;

      /// <summary>
      /// Devuelve una instancia de NetQueue
      /// </summary>
      /// <param name="workspace">Workspace actual</param>
      public NetQueue(Workspace workspace)
      {
         _ws = workspace;
      }

      #region Properties

      /// <summary>
      /// La ruta de la carpeta que contiene los archivos correspondientes a los trabajos.
      /// </summary>
      public DirectoryInfo QueueFolder
      {
         get { return new DirectoryInfo(_ws.Settings.GetString(WorkspaceSettingsKeys.NetQueuePath)); }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega un nuevo trabajo a la cola de salida
      /// </summary>
      /// <param name="outerObject">Objeto a encolar. Debe ser del tipo NetMailMessage, NetFaxMessage o NetPrintMessage.</param>
      /// <param name="owner">Propietario del trabajo</param>
      /// <returns>Una instáncia de la clase NetQueueJob con las propiedades del nuevo trabajo.</returns>
      public NetQueueJob AddJob(object outerObject, string owner)
      {
         Random rnd = new Random();
         SqlCommand cmd = null;

         // Genera la órden de trabajo
         NetQueueJob job = new NetQueueJob();
         job.Owner = owner;
         job.Priority = (MailPriority)outerObject;
         job.Retry = 0;
         job.TimeStamp = DateTime.Now;
         job.LastRetry = job.TimeStamp;
         job.ID = job.TimeStamp.ToString("yyyyMMddhhmmss") + "_" + rnd.Next(0, int.Parse(job.TimeStamp.ToString("hhmmss"))).ToString("000000");

         // Agrega un mensaje de correo electrónico
         if (job.GetType() == typeof(NetMailMessage))
         {
            job.Type = MWNetQueueType.Mail;
            ((NetMailMessage)outerObject).SaveMessage(job.ID);
         }
         
         // Inserta la órden en la cola
         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            // Agrega el trabajo a la BBDD
            string sql = "INSERT INTO SYSQUEUE (sqid,sqcreated,sqretry,sqlastretry,sqpriority,sqtype,sqowner) " +
                         "VALUES (@sqid,@sqcreated,@sqretry,@sqlastretry,@sqpriority,@sqtype,@sqowner)";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@sqid", job.ID));
            cmd.Parameters.Add(new SqlParameter("@sqcreated", job.TimeStamp));
            cmd.Parameters.Add(new SqlParameter("@sqretry", job.Retry));
            cmd.Parameters.Add(new SqlParameter("@sqlastretry", job.LastRetry));
            cmd.Parameters.Add(new SqlParameter("@sqpriority", (int)job.Priority));
            cmd.Parameters.Add(new SqlParameter("@sqtype", (int)job.Type));
            cmd.Parameters.Add(new SqlParameter("@sqowner", job.Owner));
            cmd.ExecuteNonQuery();

            return job;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("MWNetQueue.AddJob()", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Agrega un nuevo trabajo a la cola de salida
      /// </summary>
      /// <param name="outerObject">Objeto a encolar. Debe ser del tipo NetMailMessage, NetFaxMessage o NetPrintMessage.</param>
      /// <param name="Priority">Prioridad</param>
      /// <returns>Una instáncia de la clase NetQueueJob con las propiedades del nuevo trabajo.</returns>
      public NetQueueJob AddJob(object outerObject, MailPriority Priority)
      {
         return AddJob(outerObject, "SYS");
      }

      /// <summary>
      /// Agrega un nuevo trabajo a la cola de salida
      /// </summary>
      /// <param name="outerObject">Objeto a encolar. Debe ser del tipo NetMailMessage, NetFaxMessage o NetPrintMessage.</param>
      /// <returns>Una instáncia de la clase NetQueueJob con las propiedades del nuevo trabajo.</returns>
      public NetQueueJob AddJob(object outerObject)
      {
         return AddJob(outerObject, "SYS");
      }

      /// <summary>
      /// Recupera el siguiente trabajo de salida encolado y actualiza el número de intentos.
      /// </summary>
      /// <returns>Una instáncia de la clase NetQueueJob con las propiedades del trabajo.</returns>
      public NetQueueJob GetNextJob()
      {
         string sql = "";
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         NetQueueJob job = null;

         // Abre una conexión a la BBDD del workspace
         _ws.DataSource.Connect();

         // Inicia la transacción
         SqlTransaction transaction = _ws.DataSource.Connection.BeginTransaction();

         try
         {
            // Obtiene el trabajo asociado
            sql = "SELECT TOP 1 * " +
                  "FROM sysqueue " +
                  "WHERE sqretry<=@maxretry " +
                  "ORDER BY sqpriority DESC, sqcreated DESC";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@maxretry", NetQueue.QUEUE_MAX_RETRY));
            reader = cmd.ExecuteReader();

            // Contempla el caso de que no haya ninguna tarea
            if (!reader.Read())
            {
               reader.Dispose();
               cmd.Dispose();
               transaction.Rollback();
               _ws.DataSource.Disconnect();
               return job;
            }

            job = new NetQueueJob();
            job.ID = (string)reader["sqid"];
            job.TimeStamp = (DateTime)reader["sqcreated"];
            job.Retry = (int)reader["sqretry"];
            job.LastRetry = (DateTime)reader["sqlastretry"];
            job.Priority = (MailPriority)reader["sqpriority"];
            job.Type = (MWNetQueueType)reader["sqtype"];
            job.Owner = (string)reader["sqowner"];
            reader.Close();

            // Actualiza el número de intentos
            sql = "UPDATE sysqueue SET sqretry=sqretry+1,sqlastretry=GetDate() WHERE sqid=@sqid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@sqid", job.ID));
            cmd.ExecuteNonQuery();

            // Confirma la transacción
            transaction.Commit();
         }
         catch (Exception ex)
         {
            // Confirma la transacción
            transaction.Rollback();

            // Informa del error
            _ws.Logger.Add(new LogEntry("MWNetQueue.GetNextJob()", ex.Message, LogEntry.LogEntryType.EV_ERROR));
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }

         return job;
      }

      /// <summary>
      /// Cancela un trabajo de salida. Este comando elimina el archivo asociado al trabajo
      /// </summary>
      /// <param name="jobId">Identificador del trabajo.</param>
      public void CancelJob(string jobId)
      {
         SqlCommand cmd = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            cmd = new SqlCommand("DELETE FROM SYSQUEUE WHERE id=@sqid", _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@sqid", jobId));
            cmd.ExecuteNonQuery();

            // Elimina el archivo
            string filename = Path.Combine(_ws.Settings.GetString(WorkspaceSettingsKeys.NetQueuePath), jobId + NetQueueJob.QUEUE_FILE_EXTENSION);
            FileInfo file = new FileInfo(filename);
            file.Delete();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("MWNetQueue.Cancel()", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene el nombre y la ruta del archivo asociado a un trabajo a partir de su identificador.
      /// </summary>
      /// <param name="jobId">Identificador del trabajo.</param>
      /// <returns>El nombre del archivo con la ruta.</returns>
      public string GetJobFilename(string jobId)
      {
         return Path.Combine(this.QueueFolder.FullName, jobId + NetQueueJob.QUEUE_FILE_EXTENSION);
      }

      #endregion

   }

}
