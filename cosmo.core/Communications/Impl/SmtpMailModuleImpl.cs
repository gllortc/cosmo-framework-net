using Cosmo.Diagnostics;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Cosmo.Communications.Impl
{
   /// <summary>
   /// Implementa un módulo de servicio de correo SMTP usando el cliente nativo de .NET.
   /// </summary>
   public class SmtpMailModuleImpl : ICommunicationsModule
   {
      private const string SETTING_SMTP_HOST = "smtp.hostname";
      private const string SETTING_SMTP_PORT = "smtp.port";
      private const string SETTING_SMTP_LOGIN = "smtp.login";
      private const string SETTING_SMTP_PASSWORD = "smtp.password";
      private const string SETTING_SMTP_USESSL = "smtp.useSSL";
      private const string SETTING_SMTP_MSGCLASS = "smtp.message.class";
      private const string SETTING_DEFAULT_FROM_ADDRESS = "smtp.default.from.address";
      private const string SETTING_DEFAULT_FROM_NAME = "smtp.default.from.name";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="SmtpMailModuleImpl"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public SmtpMailModuleImpl(Workspace workspace, Plugin plugin) 
         : base(workspace, plugin)
      {
         Workspace = workspace;

         Initialize(plugin);
      }

      #endregion

      #region ICommunicationsModule Implementation

      /// <summary>
      /// Devuelve el nombre de la clase (nombre calificado) admitida como contenedor de mensajes.
      /// </summary>
      public override string MessageQualifiedName
      {
         get { return MessageClassName; }
      }

      /// <summary>
      /// Envia un mensaje usando el servidor de correo del workspace.
      /// </summary>
      /// <param name="message">Mensaje a enviar.</param>
      public override void Send(Object message)
      {
         if (message.GetType() != typeof(MailMessage))
         {
            throw new CommunicationsException("El módulo de comunicaciones " + GetType().AssemblyQualifiedName + " no puede enviar mensajes de tipo " + message.GetType().AssemblyQualifiedName);
         }

         // Establece el remitente si no se ha especificado
         if (((MailMessage) message).From == null)
         {
            ((MailMessage)message).From = new MailAddress(Workspace.Settings.GetString(SETTING_DEFAULT_FROM_ADDRESS),
                                                          Workspace.Settings.GetString(SETTING_DEFAULT_FROM_NAME));
         }

         try
         {
            using (SmtpClient smtp = new SmtpClient(ServerAddress, ServerPort))
            {
               smtp.Credentials = new NetworkCredential(AccountLogin, AccountPassword);
               smtp.EnableSsl = UseSSL;
               smtp.Send((MailMessage) message);
            }
         }
         catch (Exception ex)
         {
            Workspace.Logger.Add(new LogEntry(GetType().FullName + ".Send()", ex.Message, LogEntry.LogEntryType.EV_ERROR));

            throw new CommunicationsException(ex.Message, ex);
         }
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el workspace actual.
      /// </summary>
      private Workspace Workspace { get; set; }

      /// <summary>
      /// Indica si el servidor usa SSL.
      /// </summary>
      private bool UseSSL { get; set; }

      /// <summary>
      /// Devuelve o establece la dirección del servidor SMTP.
      /// </summary>
      private string ServerAddress { get; set; }

      /// <summary>
      /// Devuelve o establece el puerto del servidor SMTP.
      /// </summary>
      private int ServerPort { get; set; }

      /// <summary>
      /// Devuelve o establece el login del usuario de acceso al servidor SMTP.
      /// </summary>
      private string AccountLogin { get; set; }

      /// <summary>
      /// Devuelve o establece la contraseña del usuario de acceso al servidor SMTP.
      /// </summary>
      private string AccountPassword { get; set; }

      /// <summary>
      /// Devuelve o establece la clase (con namespace) de los mensajes que gestiona este módulo.
      /// </summary>
      private string MessageClassName { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Envia los mensajes contenidos en un array.
      /// </summary>
      /// <param name="messages">Array de mensajes.</param>
      public void Send(Object[] messages)
      {
         foreach (MailMessage msg in messages)
         {
            this.Send(msg);
         }
      }

      /// <summary>
      /// Envia los mensajes contenidos en un array.
      /// </summary>
      /// <param name="messages">Lista de mensajes.</param>
      public void Send(List<Object> messages)
      {
         this.Send(messages.ToArray());
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa el servidor de correo electrónico.
      /// </summary>
      private void Initialize(Plugin plugin)
      {
         ServerAddress = plugin.GetString(SETTING_SMTP_HOST);
         ServerPort = plugin.GetInteger(SETTING_SMTP_PORT);
         AccountLogin = plugin.GetString(SETTING_SMTP_LOGIN);
         AccountPassword = plugin.GetString(SETTING_SMTP_PASSWORD);
         UseSSL = plugin.GetBoolean(SETTING_SMTP_USESSL);
         MessageClassName = plugin.GetString(SETTING_SMTP_MSGCLASS);
      }

      #endregion

   }
}
