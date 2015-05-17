using System;
using System.IO;

namespace Cosmo.Diagnostics
{

   /// <summary>
   /// Implemnta una clase para generar archivos de LOG en modo texto en la estación local.
   /// </summary>
   public class EventLog
   {

      #region Static Members

      /// <summary>
      /// Devuelve un nombre válido de archivo LOG
      /// </summary>
      /// <param name="basepath">Ruta al directorio dónde se debe generar el archivo</param>
      /// <param name="BaseFileName">Nombre base del archivo</param>
      /// <param name="WithDate">Indica si se debe agregar la fecha al nombre de archivo</param>
      /// <returns>Una cadena que representa el nombre del archuivo</returns>
      public static string GenerateDefaultLogFileName(string basepath, string BaseFileName, bool WithDate)
      {
         if (WithDate)
            return Path.Combine(basepath, BaseFileName + "_" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".log");
         else
            return Path.Combine(basepath, BaseFileName + ".log");
      }

      /// <summary>
      /// Pass in the fully qualified name of the log file you want to write to
      /// and the message to write
      /// </summary>
      /// <param name="LogPath"></param>
      /// <param name="Message"></param>
      public static void WriteToLog(string LogPath, string Message)
      {
         System.IO.StreamWriter s = null;

         try
         {
            s = System.IO.File.AppendText(LogPath);
            s.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - " + Message);
            s.Close();
         }
         catch (Exception ex)
         {
            System.Diagnostics.Debug.WriteLine(ex.Message);
         }

         s = null;
      }

      /// <summary>
      /// Writes a message to the application event log
      /// </summary>
      /// <param name="Source">Source is the source of the message ususally you will want this to be the application name</param>
      /// <param name="Message">message to be written</param>
      /// <param name="EntryType">the entry type to use to categorize the message like for exmaple error or information</param>
      public static void WriteToEventLog(string Source, string Message, System.Diagnostics.EventLogEntryType EntryType)
      {
         try
         {
            if (!System.Diagnostics.EventLog.SourceExists(Source))
            {
               System.Diagnostics.EventLog.CreateEventSource(Source, "Application");
            }
            System.Diagnostics.EventLog.WriteEntry(Source, Message, EntryType);
         }
         catch { }
      }

      #endregion

   }
}
