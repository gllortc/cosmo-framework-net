using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Cosmo.Diagnostics.Impl
{
   /// <summary>
   /// Debugger class that allow applications to write log files.
   /// </summary>
   /// <remarks>
   /// The current log file is located on:
   /// [DLL or APP path]\LOGS\[YEAR]\[MONTH]\EMDEP_[Date in format yyyyMMdd].log
   /// </remarks>
   public class FileLogger : ILogger
   {
      /// <summary>DateTime format to show timestamp on LOG entries</summary>
      private const string FORMAT_DATETIME_LONG = "dd/MM/yyyy hh:mm:ss.fff";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FileLogger"/>.
      /// </summary>
      /// <param name="workspace">Workspace actual</param>
      public FileLogger(Workspace workspace, Plugin plugin)
      {
         // this.Workspace = workspace;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the current active log file (name and path).
      /// </summary>
      public string CurrentLogFilename
      {
         get
         {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            
            string logsPath = Path.GetDirectoryName(path);
            logsPath = Path.Combine(Path.GetDirectoryName(path), "logs");
            logsPath = Path.Combine(logsPath, DateTime.Now.ToString("yyyy"));
            logsPath = Path.Combine(logsPath, DateTime.Now.ToString("MM"));

            return Path.Combine(logsPath, "EMDEP_" + DateTime.Now.ToString("yyyyMMdd") + ".log"); 
         }
      }

      #endregion

      #region ILogger Implementation

      public LogEntry GetByID(int entryId)
      {
         throw new NotSupportedException("FileLogger don't support get individual entry by its ID.");
      }

      public List<LogEntry> GetAll()
      {
         throw new NotSupportedException("FileLogger don't support list all entries.");
      }

      public List<LogEntry> GetByType(Cosmo.Diagnostics.LogEntry.LogEntryType type)
      {
         throw new NotSupportedException("FileLogger don't support list entries by type.");
      }

      public void Info(LogEntry entry)
      {
         try
         {
            CheckForLogFile();

            // Generate the log text
            StringBuilder txt = new StringBuilder();
            txt.AppendLine(DateTime.Now.ToString(FileLogger.FORMAT_DATETIME_LONG) + " - INFO : " + entry.Context);
            txt.AppendLine("   " + entry.Message);

            // Append text to log file
            using (StreamWriter sw = File.AppendText(this.CurrentLogFilename))
            {
               sw.WriteLine(txt.ToString());
            }
         }
         catch
         {
            // This class can't thrown exceptions
            // Any exception is ignored
         }
      }

      public void Warning(LogEntry entry)
      {
         try
         {
            CheckForLogFile();

            // Generate the log text
            StringBuilder txt = new StringBuilder();
            txt.Append(DateTime.Now.ToString(FileLogger.FORMAT_DATETIME_LONG) + " - WARN");
            if (!string.IsNullOrEmpty(entry.Context))
            {
               txt.Append(" at " + entry.Context);
            }
            txt.AppendLine(" : ");
            txt.AppendLine(entry.Message);

            // Append text to log file
            using (StreamWriter sw = File.AppendText(this.CurrentLogFilename))
            {
               sw.WriteLine(txt.ToString());
            }
         }
         catch
         {
            // This class can't thrown exceptions
            // Any exception is ignored
         }
      }

      public void Security(LogEntry entry)
      {
         try
         {
            CheckForLogFile();

            // Generate the log text
            StringBuilder txt = new StringBuilder();
            txt.Append(DateTime.Now.ToString(FileLogger.FORMAT_DATETIME_LONG) + " - SECURITY");
            if (!string.IsNullOrEmpty(entry.Context))
            {
               txt.Append(" at " + entry.Context);
            }
            txt.AppendLine(" : ");
            txt.AppendLine(entry.Message);

            // Append text to log file
            using (StreamWriter sw = File.AppendText(this.CurrentLogFilename))
            {
               sw.WriteLine(txt.ToString());
            }
         }
         catch
         {
            // This class can't thrown exceptions
            // Any exception is ignored
         }
      }

      public void Error(LogEntry entry)
      {
         try
         {
            CheckForLogFile();

            // Generate the log text
            StringBuilder txt = new StringBuilder();
            txt.Append(DateTime.Now.ToString(FileLogger.FORMAT_DATETIME_LONG) + " - ERROR");
            if (!string.IsNullOrEmpty(entry.Context))
            {
               txt.Append(" at " + entry.Context);
            }
            txt.AppendLine(" : ");
            txt.AppendLine(entry.Message);

            // Append text to log file
            using (StreamWriter sw = File.AppendText(this.CurrentLogFilename))
            {
               sw.WriteLine(txt.ToString());
            }
         }
         catch
         {
            // This class can't thrown exceptions
            // Any exception is ignored
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Ensure that the log file is created and located in the correct folder.
      /// </summary>
      /// <remarks>
      /// The current log file is located on:
      /// [DLL or APP path]\LOGS\[YEAR]\[MONTH]\EMDEP_[Date in format yyyyMMdd].log
      /// </remarks>
      private void CheckForLogFile()
      {
         // Ensure folder LOGS is created
         DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(this.CurrentLogFilename));
         if (!dir.Exists)
         {
            dir.Create();
         }

         // Ensure that the LOG file is created
         FileInfo file = new FileInfo(this.CurrentLogFilename);
         if (!file.Exists)
         {
            using (StreamWriter sw = file.CreateText())
            {
               sw.WriteLine(DateTime.Now.ToString(FileLogger.FORMAT_DATETIME_LONG) + " - INFO : LOG file created by ");
            }
         }

         dir = null;
         file = null;
      }

      #endregion

   }
}
