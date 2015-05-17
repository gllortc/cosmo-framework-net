using System;
using MailBee.Mime;
using MailBee.SmtpMail;

namespace Cosmo.Communications
{

   /// <summary>
   /// Implementa un mensaje de correo electrónico
   /// </summary>
   public class NetMailMessage : MailMessage
   {
      private Workspace _workspace = null;

      /// <summary>
      /// Devuelve una instancia de NetMailMessage
      /// </summary>
      /// <param name="workspace">Workspace actual</param>
      public NetMailMessage(Workspace workspace)
      {
         _workspace = workspace;

         this.From.Email = _workspace.Settings.GetString(WorkspaceSettingsKeys.UsersMailFromAddress);
         this.From.DisplayName = _workspace.Settings.GetString(WorkspaceSettingsKeys.UsersMailFromName, this.From.Email);
      }

      /// <summary>
      /// Devuelve una instancia de MailMessage.
      /// </summary>
      public NetMailMessage(Workspace workspace, System.Net.Mail.MailMessage systemMailMessage)
      {
         _workspace = workspace;

         this.From.Email = _workspace.Settings.GetString(WorkspaceSettingsKeys.UsersMailFromAddress);
         this.From.DisplayName = _workspace.Settings.GetString(WorkspaceSettingsKeys.UsersMailFromName, this.From.Email);

         FromSystemMailMessage(systemMailMessage);
      }

      #region Properties

      /// <summary>
      /// Permite acceder al workspace al que pertenece el mensaje.
      /// </summary>
      public Workspace Workspace
      {
         get { return _workspace; }
         set { _workspace = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Convierte un mensaje de correo System.Net.Mail.MailMessage.
      /// </summary>
      public void FromSystemMailMessage(System.Net.Mail.MailMessage message)
      {
         // From
         this.From.DisplayName = message.From.DisplayName;
         this.From.Email = message.From.Address;

         // To
         foreach (System.Net.Mail.MailAddress address in message.To)
            this.To.Add(address.Address, address.DisplayName);

         // Cc
         foreach (System.Net.Mail.MailAddress address in message.CC)
            this.Cc.Add(address.Address, address.DisplayName);

         // Bcc
         foreach (System.Net.Mail.MailAddress address in message.Bcc)
            this.Bcc.Add(address.Address, address.DisplayName);

         // Reply To
         this.ReplyTo.Add(message.ReplyToList[0].Address, message.ReplyToList[0].DisplayName);

         // Priority
         switch (message.Priority)
         {
            case System.Net.Mail.MailPriority.High: this.Priority = MailPriority.Highest; break;
            case System.Net.Mail.MailPriority.Normal: this.Priority = MailPriority.Normal; break;
            case System.Net.Mail.MailPriority.Low: this.Priority = MailPriority.Low; break;
         }

         // Subject
         this.Subject = message.Subject;

         // Body
         if (message.IsBodyHtml)
            this.BodyHtmlText = message.Body;
         else
            this.BodyPlainText = message.Body;

         // Attachments
         foreach (System.Net.Mail.Attachment attachment in message.Attachments)
         {
            // Convierte el adjunto a un array de bytes
            byte[] data = new byte[attachment.ContentStream.Length];
            attachment.ContentStream.Read(data, 0, System.Convert.ToInt32(attachment.ContentStream.Length));

            this.Attachments.Add(data, attachment.Name, attachment.ContentId, attachment.ContentType.Name, null, NewAttachmentOptions.None, MailTransferEncoding.Base64);
         }
      }

      #endregion

   }

}