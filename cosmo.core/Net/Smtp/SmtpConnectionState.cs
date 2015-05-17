
namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Enumera los estados posibles de una conexión Smtp.
   /// </summary>
   public enum SmtpConnectionState
   {
      /// <summary>Disconnected</summary>
      Disconnected,
      /// <summary>Connected</summary>
      Connected,
      /// <summary>Authenticated</summary>
      Authenticated,
      /// <summary>MailFromCommandExecuting</summary>
      MailFromCommandExecuting,
      /// <summary>MailFromCommandExecuted</summary>
      MailFromCommandExecuted,
      /// <summary>RcptToCommandExecuting</summary>
      RcptToCommandExecuting,
      /// <summary>RcptToCommandExecuted</summary>
      RcptToCommandExecuted,
      /// <summary>DataCommandExecuting</summary>
      DataCommandExecuting,
      /// <summary>DataCommandExecuted</summary>
      DataCommandExecuted
   }

}
