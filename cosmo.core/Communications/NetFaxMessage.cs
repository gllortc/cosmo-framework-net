using Cosmo.Diagnostics;
using MailBee.Mime;
using System;
using System.IO;

namespace Cosmo.Communications
{

   /// <summary>
   /// Implementa un mensaje de FAX
   /// </summary>
   public class NetFaxMessage : IDisposable
   {
      private Workspace _ws;
      private MailMessage _message;

      /// <summary>
      /// Devuelve una instáncia de <see cref="Cosmo.Workspace.Net.NetFaxMessage"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Cosmo.Workspace.Workspace"/>.</param>
      public NetFaxMessage(Workspace workspace)
      {
         _ws = workspace;

         _message = new MailMessage();
         _message.Attachments.Capacity = 1;
      }

      #region Properties

      /// <summary>
      /// Permite acceder al workspace asociado al mensaje.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
         set { _ws = value; }
      }

      /// <summary>
      /// Colección de archivos adjuntos al mensaje.
      /// </summary>
      internal AttachmentCollection Attachments
      {
         get { return _message.Attachments; }
      }

      /// <summary>
      /// Nombre del remitente.
      /// </summary>
      public string FromName
      {
         get { return _message.From.DisplayName; }
         set { _message.From.DisplayName = value; }
      }

      /// <summary>
      /// Número de FAX de destino.
      /// </summary>
      public string ToNumber
      {
         get { return _message.MessageID; }
         set { _message.MessageID = value; }
      }

      /// <summary>
      /// Nombre del destinatario.
      /// </summary>
      public string ToName
      {
         get { return _message.Organization; }
         set { _message.Organization = value; }
      }

      /// <summary>
      /// Título del mensaje.
      /// </summary>
      public string Subject
      {
         get { return _message.Subject; }
         set { _message.Subject = value; }
      }

      /// <summary>
      /// Texto del mensaje.
      /// </summary>
      public string Body
      {
         get { return _message.BodyPlainText; }
         set { _message.BodyPlainText = value; }
      }

      /// <summary>
      /// Devuelve o establece la organización.
      /// </summary>
      public string Organization
      {
         get { return _message.Organization; }
         set { _message.Organization = value; }
      }

      /// <summary>
      /// Prioridad del mensaje en la cola de salida del workspace.
      /// </summary>
      public MailPriority Priority
      {
         get { return _message.Priority; }
         set { _message.Priority = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Adjunta el archivo a mandar por FAX.
      /// </summary>
      /// <remarks>
      /// Un FAX sólo admite un archivo por envio. Si se invoca este método más de una vez, sobreescribirá el documento anterior.
      /// </remarks>
      public void AttachDocument(string filename)
      {
         if (_message.Attachments.Count > 0) _message.Attachments.Clear();
         _message.Attachments.Add(filename);
      }

      /// <summary>
      /// Indica si un archivo puede o no ser mandado por FAX (según su tipo).
      /// </summary>
      /// <param name="filename">Nombre + path del archivo.</param>
      /// <returns>Un valor booleano indicando si se puede o no mandaro por fax.</returns>
      public bool IsFaxableFile(string filename)
      {
         try
         {
            // Obtiene el archivo. Si no existe, devuelve falso
            FileInfo file = new FileInfo(filename);
            if (!file.Exists) return false;

            // Obtiene las extensiones
            string faxext = _ws.Settings.GetString(WorkspaceSettingsKeys.FaxAllowedExtensions);
            return faxext.Contains(file.Extension.Replace(".", "") + ";");
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      /// Carga un mensaje guardado.
      /// </summary>
      /// <param name="filename">Nombre + path del archivo a cargar.</param>
      public void LoadMessage(string filename)
      {
         try
         {
            _message.LoadMessage(filename);
         }
         catch (Exception ex)
         {
            Logger.Add(_ws, new LogEntry("NetFaxMessage.LoadMessage()", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
      }

      /// <summary>
      /// Guarda un mensaje a un archivo.
      /// </summary>
      /// <param name="filename">Nombre + path del archivo de destino.</param>
      public void SaveMessage(string filename)
      {
         try
         {
            _message.SaveMessage(filename);
         }
         catch (Exception ex)
         {
            Logger.Add(_ws, new LogEntry("NetFaxMessage.SaveMessage(string)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
      }

      /// <summary>
      /// Libera todos los recursos usados.
      /// </summary>
      public void Dispose()
      {
         _message.Dispose();
      }

      #endregion

   }
}
