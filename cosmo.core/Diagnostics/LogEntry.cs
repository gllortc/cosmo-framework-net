using System;

namespace Cosmo.Diagnostics
{

   /// <summary>
   /// Implementa una entrada del registro de eventos
   /// </summary>
   public class LogEntry
   {

      #region Enumerations

      /// <summary>
      /// Tipos de registro
      /// </summary>
      public enum LogEntryType : int
      {
         /// <summary>Registro de tipo informativo</summary>
         EV_INFORMATION = 1,
         /// <summary>Registro de tipo advertencia</summary>
         EV_WARNING = 2,
         /// <summary>Registro de tipo error de aplicación</summary>
         EV_ERROR = 3,
         /// <summary>Registro de advertencia de seguridad</summary>
         EV_SECURITY = 4
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of LogEntry
      /// </summary>
      public LogEntry()
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of LogEntry
      /// </summary>
      /// <param name="context">Contexto</param>
      /// <param name="message">Mensaje</param>
      /// <param name="type">Tipo de registro</param>
      public LogEntry(string context, string message, LogEntryType type)
      {
         Initialize();

         this.Context = context;
         this.Message = message;
         this.Type = type;
      }

      /// <summary>
      /// Gets a new instance of LogEntry
      /// </summary>
      /// <param name="application">Nombre de la aplicación</param>
      /// <param name="context">Contexto</param>
      /// <param name="message">Mensaje</param>
      /// <param name="type">Tipo de registro</param>
      public LogEntry(string application, string context, string message, LogEntryType type)
      {
         Initialize();

         this.ApplicationName = application;
         this.Context = context;
         this.Message = message;
         this.Type = type;
      }

      /// <summary>
      /// Gets a new instance of LogEntry
      /// </summary>
      /// <param name="application">Nombre de la aplicación</param>
      /// <param name="context">Contexto</param>
      /// <param name="message">Mensaje</param>
      /// <param name="login">Login del usuario</param>
      /// <param name="type">Tipo de registro</param>
      public LogEntry(string application, string context, string message, string login, LogEntryType type)
      {
         Initialize();

         this.ApplicationName = application;
         this.Context = context;
         this.Message = message;
         this.Type = type;
         this.UserLogin = login;
         
      }

      /// <summary>
      /// Gets a new instance of LogEntry
      /// </summary>
      /// <param name="application">Nombre de la aplicación</param>
      /// <param name="context">Contexto</param>
      /// <param name="code">Código del error</param>
      /// <param name="message">Mensaje</param>
      /// <param name="type">Tipo de registro</param>
      public LogEntry(string application, string context, int code, string message, LogEntryType type)
      {
         Initialize();

         this.ApplicationName = application;
         this.Context = context;
         this.Message = message;
         this.Type = type;
         this.Code = code;
      }

      /// <summary>
      /// Gets a new instance of LogEntry
      /// </summary>
      /// <param name="application">Nombre de la aplicación</param>
      /// <param name="context">Contexto</param>
      /// <param name="code">Código del error</param>
      /// <param name="message">Mensaje</param>
      /// <param name="login">Login del usuario</param>
      /// <param name="type">Tipo de registro</param>
      public LogEntry(string application, string context, int code, string message, string login, LogEntryType type)
      {
         Initialize();

         this.ApplicationName = application;
         this.Context = context;
         this.Message = message;
         this.Type = type;
         this.UserLogin = login;
         this.Code = code;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Identificador único del registro.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Decha y hora de la entrada al registro de eventos.
      /// </summary>
      public DateTime Date { get; set; }

      /// <summary>
      /// Login del usuario identificado
      /// </summary>
      public string UserLogin { get; set; }

      /// <summary>
      /// Nombre de la aplicación o libreria que generó la entrada al registro
      /// </summary>
      /// <remarks>
      /// Usualmente se usa para este campo:
      ///    Assembly.GetEntryAssembly().FullName
      ///    Assembly.GetExecutingAssembly().FullName
      /// </remarks>
      public string ApplicationName { get; set; }

      /// <summary>
      /// Contexto dónde se provocó el registro
      /// </summary>
      /// <remarks>
      /// Para los Assemblies se puede usar la notación Clase.Método() y para las aplicaciones
      /// web el nombre del script ASPX (por ejemplo).
      /// </remarks>
      public string Context { get; set; }

      /// <summary>
      /// Nombre del workspace dónde se estaba ejecutando el código.
      /// </summary>
      public string WorkspaceName { get; set; }

      /// <summary>
      /// Gets or sets el código del error o evento informativo.
      /// </summary>
      public int Code { get; set; }

      /// <summary>
      /// Gets or sets el mensaje descriptivo.
      /// </summary>
      public string Message { get; set; }

      /// <summary>
      /// Gets or sets el tipo de registro.
      /// </summary>
      public LogEntryType Type { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.ID = 0;
         this.Date = System.DateTime.Now;
         this.UserLogin = Security.Auth.SecurityService.ACCOUNT_SYSTEM;
         this.ApplicationName = Cosmo.Properties.ProductName;
         this.Context = string.Empty;
         this.WorkspaceName = string.Empty;
         this.Code = 0;
         this.Message = string.Empty;
         this.Type = LogEntryType.EV_INFORMATION;
      }

      #endregion

   }
}
