using System;
using System.Diagnostics;
using System.Globalization;

namespace Cosmo.Diagnostics
{
   /// <summary>
   /// Exception helper class.
   /// </summary>
   public static class ExceptionHelper
   {

      #region Methods

      /// <summary>
      /// Traces an error message with exception details.
      /// </summary>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="message">Message of the trace.</param>
      public static void TraceExceptionAsError(Exception ex, string message)
      {
         TraceException(TraceLevel.Error, ex, message, new object[] { });
      }

      /// <summary>
      /// Traces an error message with exception details.
      /// </summary>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the args array.</param>
      /// <param name="args">An object array containing zero or more objects to format.</param>
      public static void TraceExceptionAsError(Exception ex, string format, params object[] args)
      {
         TraceException(TraceLevel.Error, ex, format, args);
      }

      /// <summary>
      /// Traces a warning message with exception details.
      /// </summary>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="message">Message of the trace.</param>
      public static void TraceExceptionAsWarning(Exception ex, string message)
      {
         TraceException(TraceLevel.Warning, ex, message, new object[] { });
      }

      /// <summary>
      /// Traces a warning message with exception details.
      /// </summary>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the args array.</param>
      /// <param name="args">An object array containing zero or more objects to format.</param>
      public static void TraceExceptionAsWarning(Exception ex, string format, params object[] args)
      {
         TraceException(TraceLevel.Warning, ex, format, args);
      }

      /// <summary>
      /// Traces an information message with exception details.
      /// </summary>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="message">Message of the trace.</param>
      public static void TraceExceptionAsInformation(Exception ex, string message)
      {
         TraceException(TraceLevel.Info, ex, message, new object[] { });
      }

      /// <summary>
      /// Traces an information message with exception details.
      /// </summary>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the args array.</param>
      /// <param name="args">An object array containing zero or more objects to format.</param>
      public static void TraceExceptionAsInformation(Exception ex, string format, params object[] args)
      {
         TraceException(TraceLevel.Info, ex, format, args);
      }

      /// <summary>
      /// Traces a verbose message with exception details.
      /// </summary>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="message">Message of the trace.</param>
      public static void TraceExceptionAsVerbose(Exception ex, string message)
      {
         TraceException(TraceLevel.Verbose, ex, message, new object[] { });
      }

      /// <summary>
      /// Traces a verbose message with exception details.
      /// </summary>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the args array.</param>
      /// <param name="args">An object array containing zero or more objects to format.</param>
      public static void TraceExceptionAsVerbose(Exception ex, string format, params object[] args)
      {
         TraceException(TraceLevel.Verbose, ex, format, args);
      }

      /// <summary>
      /// Traces a message with exception details.
      /// </summary>
      /// <param name="level"><see cref="TraceLevel"/> to write.</param>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="message">Message of the trace.</param>
      public static void TraceException(TraceLevel level, Exception ex, string message)
      {
         TraceException(level, ex, message, new object[] { });
      }

      /// <summary>
      /// Traces a message with exception details.
      /// </summary>
      /// <param name="level"><see cref="TraceLevel"/> to write.</param>
      /// <param name="ex"><see cref="Exception"/> that contains the additional information.</param>
      /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the args array.</param>
      /// <param name="args">An object array containing zero or more objects to format.</param>
      public static void TraceException(TraceLevel level, Exception ex, string format, params object[] args)
      {
         // Set up final message
         string message = (args.Length == 0) ? format : string.Format(CultureInfo.CurrentUICulture, format, args);
         string deepestExceptionMessage = (ex.InnerException != null ? GetInnerException(ex).Message : "null");
         string finalMessage = string.Format("{0}  -------------> (Details: '{1}' | InnerException: {2})", message,
            ex.Message, deepestExceptionMessage);

         // Now trace
         TraceHelper.TraceWithDetails(level, finalMessage, ex.ToString());
      }

      /// <summary>
      /// Gets the inner exception.
      /// </summary>
      /// <param name="ex">The ex.</param>
      private static Exception GetInnerException(Exception ex)
      {
         if (ex.InnerException != null)
            return GetInnerException(ex.InnerException);
         else
            return ex;
      }

      #endregion

   }
}
