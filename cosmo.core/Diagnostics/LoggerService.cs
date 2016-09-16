using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Cosmo.Diagnostics
{
   /// <summary>
   /// Implements the event log service for the workspace.
   /// </summary>
   public class LoggerService : WorkspaceService<ILogger>, ILogger
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of Logger
      /// </summary>
      /// <param name="workspace">Workspace actual</param>
      public LoggerService(Workspace workspace)
         : base(workspace, workspace.Settings.LogModules)
      {
         // Nothing to do here
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the current authenticated user's login.
      /// </summary>
      /// <remarks>If there are not any user authenticated, the <c>[SYS]</c> token is returned.</remarks>
      internal string CurrentUserLogin
      {
         get { return Workspace.CurrentUser.IsAuthenticated ? Workspace.CurrentUser.User.Login : SecurityService.ACCOUNT_SYSTEM; }
      }

      #endregion

      #region ILogger Implementation

      /// <summary>
      /// Gets a log entry by its unique identifier.
      /// </summary>
      /// <param name="entryId">Log entry unique identifier.</param>
      /// <returns>An instance of <see cref="LogEntry"/> or <c>null</c> if the identifier doesn't exist.</returns>
      public LogEntry GetByID(int entryId)
      {
         return this.DefaultModule.GetByID(entryId);
      }

      /// <summary>
      /// Gets a list of all entries.
      /// </summary>
      public List<LogEntry> GetAll()
      {
         return this.DefaultModule.GetAll();
      }

      public List<LogEntry> GetByType(Cosmo.Diagnostics.LogEntry.LogEntryType type)
      {
         return GetByType(type);
      }

      public void Info(LogEntry entry)
      {
         if (this.DefaultModule != null)
         {
            this.DefaultModule.Info(entry);
         }
      }

      public void Warning(LogEntry entry)
      {
         if (this.DefaultModule != null)
         {
            this.DefaultModule.Warning(entry);
         }
      }

      public void Security(LogEntry entry)
      {
         if (this.DefaultModule != null)
         {
            this.DefaultModule.Security(entry);
         }
      }

      public void Error(LogEntry entry)
      {
         if (this.DefaultModule != null)
         {
            this.DefaultModule.Error(entry);
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Log an info event using the default logger.
      /// </summary>
      /// <param name="message">A string containing the entry message.</param>
      public void Info(string message)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = message;

         this.Info(entry);
      }

      /// <summary>
      /// Log an info event using the default logger.
      /// </summary>
      /// <param name="obj">Context object.</param>
      /// <param name="methodName">The name of the caller method.</param>
      /// <param name="message">A string containing the entry message.</param>
      public void Info(object obj, string methodName, string message)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.Context = obj.GetType().Name + "." + methodName + "()";
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = message;

         this.Info(entry);
      }

      /// <summary>
      /// Log a warning event using the default logger.
      /// </summary>
      /// <param name="message">A string containing the entry message.</param>
      public void Warning(string message)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = message;

         this.Warning(entry);
      }

      /// <summary>
      /// Log a warning event using the default logger.
      /// </summary>
      /// <param name="context">A string containing the context.</param>
      /// <param name="message">A string containing the entry message.</param>
      public void Warning(string context, string message)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.Context = context;
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = message;

         this.Warning(entry);
      }

      /// <summary>
      /// Log a security event using the default logger.
      /// </summary>
      /// <param name="message">A string containing the entry message.</param>
      public void Security(string message)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = message;

         this.Warning(entry);
      }

      /// <summary>
      /// Log a security event using the default logger.
      /// </summary>
      /// <param name="message">A string containing the entry message.</param>
      public void Error(string message)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = message;

         this.Warning(entry);
      }

      /// <summary>
      /// Log a security event using the default logger.
      /// </summary>
      /// <param name="exception">The exception raised.</param>
      public void Error(Exception exception)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = exception.ToString();

         this.Error(entry);
      }

      /// <summary>
      /// Log a security event using the default logger.
      /// </summary>
      /// <param name="context">A string containing the context.</param>
      /// <param name="exception">The exception raised.</param>
      public void Error(string context, Exception exception)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.Context = context;
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = exception.ToString();

         this.Error(entry);
      }

      /// <summary>
      /// Log a security event using the default logger.
      /// </summary>
      /// <param name="obj">Context object.</param>
      /// <param name="methodName">The name of the caller method.</param>
      /// <param name="exception">The exception raised.</param>
      public void Error(object obj, string methodName, Exception exception)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.Context = obj.GetType().Name + "." + methodName + "()";
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = exception.ToString();

         this.Error(entry);
      }

      /// <summary>
      /// Log a security event using the default logger.
      /// </summary>
      /// <param name="obj">Context object.</param>
      /// <param name="methodName">The name of the caller method.</param>
      /// <param name="message">A string containing the entry message.</param>
      public void Error(object obj, string methodName, string message)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

         LogEntry entry = new LogEntry();
         entry.ApplicationName = fvi.ProductName;
         entry.Context = obj.GetType().Name + "." + methodName + "()";
         entry.WorkspaceName = this.Workspace.Name;
         entry.UserLogin = this.CurrentUserLogin;
         entry.Message = message;

         this.Error(entry);
      }

      #endregion

   }
}
