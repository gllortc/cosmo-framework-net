using System.Collections.Generic;

namespace Cosmo.Diagnostics
{
   public interface ILogger
   {

      /// <summary>
      /// Gets a log entry by its unique identifier.
      /// </summary>
      /// <param name="id">Log entry unique identifier.</param>
      /// <returns>An instance of <see cref="LogEntry"/> or <c>null</c> if the identifier doesn't exist.</returns>
      LogEntry GetByID(int entryId);

      /// <summary>
      /// Gets a list of all entries.
      /// </summary>
      List<LogEntry> GetAll();

      List<LogEntry> GetByType(Cosmo.Diagnostics.LogEntry.LogEntryType type);

      void Info(LogEntry entry);

      void Warning(LogEntry entry);

      void Security(LogEntry entry);

      void Error(LogEntry entry);

   }
}
