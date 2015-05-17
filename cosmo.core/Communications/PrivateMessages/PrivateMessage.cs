using System;

namespace Cosmo.Communications.PrivateMessages
{

   /// <summary>
   /// Implementa un mensaje de usuario a usuario
   /// </summary>
   public class PrivateMessage
   {

      #region Enumerations

      /// <summary>
      /// Estados posibles de un mensaje
      /// </summary>
      public enum UserMessageStatus
      {
         /// <summary>No leído</summary>
         Unreaded = 0,
         /// <summary>Leído</summary>
         Readed = 1
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="PrivateMessage"/>.
      /// </summary>
      public PrivateMessage()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Identificador único del mensaje en el servidor
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Devuelve o establece el propietario del mensaje.
      /// </summary>
      public int OwnerId { get; set; }

      /// <summary>
      /// Identificador del autor del mensaje
      /// </summary>
      public int FromUserID { get; set; }

      /// <summary>
      /// Identificador del receptor del mensaje
      /// </summary>
      public int ToUserID { get; set; }

      /// <summary>
      /// Título del mensaje (asunto)
      /// </summary>
      public string Subject { get; set; }

      /// <summary>
      /// Cuerpo del mensaje
      /// </summary>
      public string Body { get; set; }

      /// <summary>
      /// IP del equipo que envió el mensaje
      /// </summary>
      public string FromIP { get; set; }

      /// <summary>
      /// Fecha en la que realizó en envio del mensaje
      /// </summary>
      public DateTime Sended { get; set; }

      /// <summary>
      /// Fecha en la que realizó en envio del mensaje
      /// </summary>
      public UserMessageStatus Status { get; set; }

      /// <summary>
      /// Devuelve el número de respuestas al mensaje (incluyendo el mensaje inicial).
      /// </summary>
      public int Responses { get; internal set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.ID = 0;
         this.OwnerId = 0;
         this.FromUserID = 0;
         this.ToUserID = 0;
         this.Subject = string.Empty;
         this.Body = string.Empty;
         this.FromIP = "0.0.0.0";
         this.Sended = DateTime.MinValue;
         this.Status = UserMessageStatus.Unreaded;
         this.Responses = 0;
      }

      #endregion

   }
}
