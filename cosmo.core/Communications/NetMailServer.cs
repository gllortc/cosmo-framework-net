using Cosmo.Diagnostics;
using MailBee.Mime;
using MailBee.SmtpMail;
using System;
using System.Collections.Generic;

namespace Cosmo.Communications
{

   /// <summary>
   /// Implementa un servidor de correo de workspaces.
   /// </summary>
   public class NetMailServer
   {
      private Workspace _ws;
      private SmtpServer _server;

      /// <summary>
      /// Devuelve una instancia de NetMailServer.
      /// </summary>
      /// <param name="workspace">Una instancia de Workspace que representa el workspace asociado al servidor de correo electrónico.</param>
      public NetMailServer(Workspace workspace)
      {
         _ws = workspace;

         Initialize();
      }

      /// <summary>
      /// Inicializa el servidor de correo electrónico.
      /// </summary>
      public void Initialize()
      {
         // Establece la licencia de uso de los objetos MailBee
         Smtp.LicenseKey = _ws.Settings.GetString(WorkspaceSettingsKeys.LicenceMailbeeObjects);

         // Configura el servidor mediante la configuración del workspace
         _server = new SmtpServer();
         _server.Name = _ws.Settings.GetString(WorkspaceSettingsKeys.SMTPHost);
         _server.Port = _ws.Settings.GetInt(WorkspaceSettingsKeys.SMTPPort, 25);
         _server.AccountName = _ws.Settings.GetString(WorkspaceSettingsKeys.SMTPLogin);
         _server.Password = _ws.Settings.GetString(WorkspaceSettingsKeys.SMTPPassword);
         _server.AuthMethods = (MailBee.AuthenticationMethods)_ws.Settings.GetInt(WorkspaceSettingsKeys.SMTPAuthentication);
         if (_ws.Settings.GetBoolean(WorkspaceSettingsKeys.SMTPUseSSL)) _server.SslMode = MailBee.Security.SslStartupMode.UseStartTls;
      }

      /// <summary>
      /// Envia un mensaje usando el servidor de correo del workspace.
      /// </summary>
      /// <param name="message">Mensaje a enviar.</param>
      public void Send(NetMailMessage message)
      {
         try
         {
            // Manda el correo electrónico
            Smtp smtp = new Smtp();
            smtp.SmtpServers.Add(_server);
            smtp.Connect();
            smtp.Message = (MailMessage)message;
            smtp.Send();
            smtp.Disconnect();
         }
         catch (MailBee.MailBeeException ex)
         {
            _ws.Logger.Add(new LogEntry("NetMailServer.Send(NetMailMessage)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw new Exception("Error mandando el correo: " + ex.Message, ex);
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("NetMailServer.Send(NetMailMessage)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw new Exception("Error mandando el correo: " + ex.Message, ex);
         }
      }

      /// <summary>
      /// Envia los mensajes contenidos en un array.
      /// </summary>
      /// <param name="messages">Array de mensajes.</param>
      public void Send(NetMailMessage[] messages)
      {
         foreach (NetMailMessage msg in messages)
         {
            this.Send(msg);
         }
      }

      /// <summary>
      /// Envia los mensajes contenidos en un array.
      /// </summary>
      /// <param name="messages">Lista de mensajes.</param>
      public void Send(List<NetMailMessage> messages)
      {
         this.Send(messages.ToArray());
      }

      /// <summary>
      /// Envia el correo usando la cola de salida (Workspace Outer Queue).
      /// </summary>
      /// <param name="message">Usuario del workspace que genera el envio.</param>
      /// <param name="user">Usuario que provocó el envio.</param>
      /*public void SendQueued(NetMailMessage message, User user)
      {
         _ws.NetQueue.AddJob(message, user.Login);
      }*/

      /// <summary>
      /// Envia los mensajes contenidos en un array.
      /// </summary>
      /// <param name="messages">Array de mensajes.</param>
      /// <param name="user">Usuario que provocó el envio.</param>
      /*public void SendQueued(NetMailMessage[] messages, User user)
      {
         foreach (NetMailMessage msg in messages)
         {
            this.SendQueued(msg, user);
         }
      }*/

      /// <summary>
      /// Envia los mensajes contenidos en un array.
      /// </summary>
      /// <param name="messages">Lista de mensajes.</param>
      /// <param name="user">Usuario que provocó el envio.</param>
      /*public void SendQueued(List<NetMailMessage> messages, User user)
      {
         this.SendQueued(messages.ToArray(), user);
      }*/
   }

}
