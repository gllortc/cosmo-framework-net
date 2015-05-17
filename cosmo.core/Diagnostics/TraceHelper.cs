using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Cosmo.Diagnostics
{
   /// <summary>
   /// Trace helper class.
   /// </summary>
   public static class TraceHelper
   {
      private static Dictionary<TraceEventType, TraceLevel> _traceLevels = null;

      #region Methods

      /// <summary>
      /// Converts a <see cref="TraceEventType"/> to a <see cref="TraceLevel"/>.
      /// </summary>
      /// <param name="eventType"><see cref="TraceEventType"/> to convert.</param>
      /// <returns><see cref="TraceLevel"/> that represents a <see cref="TraceEventType"/>.</returns>
      public static TraceLevel ConvertTraceEventTypeToTraceLevel(TraceEventType eventType)
      {
         // Check if dictionary is initialized
         if (_traceLevels == null)
         {
            // No, initialize
            _traceLevels = new Dictionary<TraceEventType, TraceLevel>();
            _traceLevels.Add(TraceEventType.Critical, TraceLevel.Error);
            _traceLevels.Add(TraceEventType.Error, TraceLevel.Error);
            _traceLevels.Add(TraceEventType.Warning, TraceLevel.Warning);
            _traceLevels.Add(TraceEventType.Information, TraceLevel.Info);
            _traceLevels.Add(TraceEventType.Verbose, TraceLevel.Verbose);
         }

         // Return right value
         return _traceLevels.ContainsKey(eventType) ? _traceLevels[eventType] : TraceLevel.Off;
      }

      /// <summary>
      /// Traces an error message with details.
      /// </summary>
      /// <param name="message">Message of the trace.</param>
      /// <param name="details">Additional details which will be listed later in the trace.</param>
      public static void TraceErrorWithDetails(string message, string details)
      {
         TraceWithDetails(TraceLevel.Error, message, details);
      }

      /// <summary>
      /// Traces a warning message with details.
      /// </summary>
      /// <param name="message">Message of the trace.</param>
      /// <param name="details">Additional details which will be listed later in the trace.</param>
      public static void TraceWarningWithDetails(string message, string details)
      {
         TraceWithDetails(TraceLevel.Warning, message, details);
      }

      /// <summary>
      /// Traces an information message with details.
      /// </summary>
      /// <param name="message">Message of the trace.</param>
      /// <param name="details">Additional details which will be listed later in the trace.</param>
      public static void TraceInformationWithDetails(string message, string details)
      {
         TraceWithDetails(TraceLevel.Info, message, details);
      }

      /// <summary>
      /// Traces a verbose message with details.
      /// </summary>
      /// <param name="message">Message of the trace.</param>
      /// <param name="details">Additional details which will be listed later in the trace.</param>
      public static void TraceVerboseWithDetails(string message, string details)
      {
         TraceWithDetails(TraceLevel.Verbose, message, details);
      }

      /// <summary>
      /// Traces a message with details.
      /// </summary>
      /// <param name="level"><see cref="TraceLevel"/> to write.</param>
      /// <param name="message">Message of the trace.</param>
      /// <param name="details">Additional details which will be listed later in the trace.</param>
      public static void TraceWithDetails(TraceLevel level, string message, string details)
      {
         // Set up message
         string traceMessage = string.Format(CultureInfo.CurrentUICulture, Properties.Resources.TraceWithDetails,
             message, details);

         // Trace
         switch (level)
         {
            case TraceLevel.Error:
               Trace.TraceError(traceMessage);
               break;

            case TraceLevel.Warning:
               Trace.TraceWarning(traceMessage);
               break;

            case TraceLevel.Info:
               Trace.TraceInformation(traceMessage);
               break;

            case TraceLevel.Verbose:
               Trace.Write(traceMessage);
               break;
         }
      }

      #endregion

   }
}
