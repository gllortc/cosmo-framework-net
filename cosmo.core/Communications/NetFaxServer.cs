using Cosmo.Net.Fax;
using Cosmo.Diagnostics;
// using FAXCOMLib;

namespace Cosmo.Communications
{
   /// <summary>
   /// Implementa el servidor de FAX para los workspaces.
   /// </summary>
   public class NetFaxServer
   {
      private Workspace _ws = null;
      private FaxServer _server = null;

      /// <summary>
      /// Devuelve una instancia de <see cref="Cosmo.Workspace.Net.NetFaxServer"/>.
      /// </summary>
      /// <param name="workspace">una instancia de <see cref="Cosmo.Workspace.Workspace"/>.</param>
      public NetFaxServer(Workspace workspace)
      {
         this.Workspace = workspace;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el workspace al que está asociado el servidor de Fax.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
         set 
         { 
            _ws = value;
            Initialize();
         }
      }

      /// <summary>
      /// Indica si el servicio de Fax está habilitado.
      /// </summary>
      /// <remarks>
      /// El servicio de FAX (Windows Fax) está sólo disponible en versiones de Windows Business o superiores.
      /// </remarks>
      public bool IsServerAvailable
      {
         get { return FaxServer.IsFaxServiceInstalled; }
      }

      /// <summary>
      /// Devuelve el nombre del servidor.
      /// </summary>
      /// <remarks>
      /// Una cadena vacía indica que el servidor es el propio equipo (localhost).
      /// </remarks>
      public string ServerName
      {
         get { return _server.ServerName; }
      }

      /// <summary>
      /// Devuelve el nombre de la impresora de Fax.
      /// </summary>
      public string PrinterName
      {
         get { return _server.PrinterName; }
      }

      #endregion

      #region Methods

      /*/// <summary>
      /// Envia el FAX directamente sin pasar por la cola de envios de Cosmo
      /// </summary>
      /// <param name="message">Mensaje de FAX a enviar.</param>
      /// <remarks>
      /// Importante: 
      /// No está implementada la posibilidad de enviar texto ubicado en la propiedad BODY. Esta función será implementada
      /// en futuras versiones.
      /// </remarks>
      public void Send(NetFaxMessage message)
      {
         bool errors = false;

         try
         {
            // Inicializa el servidor de FAX
            // FaxServer server = new FaxServer();
            // server.Connect(Environment.MachineName);

            // Genera el documento para enviar por FAX
            // Si hay más de un archivo adjunto, se mandan varios FAX
            foreach (Attachment attachment in message.Attachments)
            {
               try
               {
                  FaxDoc doc = (FaxDoc)server.CreateDocument(attachment.FilenameOriginal);
                  doc.RecipientName = message.Organization;
                  doc.FaxNumber = message.ToNumber;
                  doc.DisplayName = message.Subject;

                  int response = doc.Send();

                  // Informa al sistema del envío
                  _ws.Logger.Add(new LogEntry("NetFaxServer.Send()", "FAX enviado al " + message.ToNumber, LogEntry.LogEntryType.EV_INFORMATION));
               }
               catch (Exception ex)
               {
                  _ws.Logger.Add(new LogEntry("NetFaxServer.Send()", ex.Message, LogEntry.LogEntryType.EV_ERROR));
                  errors = true;
               }
            }

            // Cierra la conexión con el servidor
            // server.Disconnect();

            if (errors)
               throw new Exception("Se han producido errores durante el envio del FAX. Mire el archivo de errores para más información.");
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("NetFaxServer.Send()", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw new Exception("Error enviando FAX: " + ex.Message, ex);
         }
      }*/

      /// <summary>
      /// Envia el FAX directamente sin pasar por la cola de envios de Cosmo
      /// </summary>
      /// <param name="message">Una instancia de <see cref="Cosmo.Workspace.Net.NetFaxMessage"/> que contiene el FAX a enviar.</param>
      /// <remarks>
      /// Importante: 
      /// No está implementada la posibilidad de enviar texto ubicado en la propiedad BODY. Esta función será implementada
      /// en futuras versiones.
      /// </remarks>
      public void Send(NetFaxMessage message)
      {
         if (!this.IsServerAvailable)
            new FaxException("El servicio de FAX del workspace no está habilitado.");

         using (_server)
         {
            // Rellena la información básica del FAX
            FaxInformations faxInfo = new FaxInformations();
            faxInfo.SenderName = message.FromName;
            faxInfo.SenderCompany = message.Organization;
            faxInfo.RecipientName = message.ToName;
            faxInfo.RecipientNumber = message.ToNumber;

            // Rellena la cubierta de FAX
            if (_ws.Settings.GetBoolean(WorkspaceSettingsKeys.FaxCoverUse, false))
            {
               FaxCoverPage cover = new FaxCoverPage();
               cover.RecipientName = message.ToName;
               cover.RecipientNumber = message.ToNumber;
               cover.Subject = message.Subject;
               cover.SenderName = message.FromName;
               faxInfo.CoverPage = cover;
            }

            FaxJob faxJob = _server.Send(faxInfo, message.Attachments[0].Filename);
         }
      }

      /// <summary>
      /// Envia el correo usando la cola de salida del Workspace
      /// </summary>
      /// <param name="message">Mensaje de FAX a enviar.</param>
      public void SendQueued(NetFaxMessage message)
      {
         // Genera el trabajo
         NetQueue queue = new NetQueue(_ws);
         NetQueueJob job = queue.AddJob(this, message.Priority);

         // Registra el envío en el LOG del workspace
         _ws.Logger.Add(new LogEntry("NetFaxServer.SendQueued()", "FAX enviado al " + message.ToNumber, LogEntry.LogEntryType.EV_INFORMATION));
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa el servidor de FAX.
      /// </summary>
      private void Initialize()
      {
         if (_server != null) _server.Dispose();

         string server = _ws.Settings.GetString(WorkspaceSettingsKeys.FaxServerName);
         string printer = _ws.Settings.GetString(WorkspaceSettingsKeys.FaxPrinterName);

         _server = new FaxServer(server, printer);
      }

      #endregion

   }

}
