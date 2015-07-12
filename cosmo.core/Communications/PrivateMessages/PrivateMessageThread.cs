using Cosmo.Security;
using System;
using System.Collections.Generic;

namespace Cosmo.Communications.PrivateMessages
{
   /// <summary>
   /// Representa un hilo de mensajes privados entre dos usuarios.
   /// </summary>
   public class PrivateMessageThread
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PrivateMessageThread"/>.
      /// </summary>
      public PrivateMessageThread()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador de usuario remoto.
      /// </summary>
      internal int RemoteUserId { get; set; }

      /// <summary>
      /// Gets or sets el usuario remoto con el que se establece la conversación.
      /// </summary>
      public User RemoteUser { get; set; }

      /// <summary>
      /// Indica si el hilo contiene elementos no leidos.
      /// </summary>
      public bool HaveUnreadMessages { get; set; }

      /// <summary>
      /// Gets or sets la fecha del último mensaje enviado al hilo.
      /// </summary>
      public DateTime LastMessagesDate { get; set; }

      /// <summary>
      /// Gets or sets la lista de mensajes privados que componen el hilo.
      /// </summary>
      public List<PrivateMessage> Messages { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega un mensaje al thread siempre que no sea del propio autor.
      /// </summary>
      /// <param name="message">Una instancia de <see cref="PrivateMessage"/> que representa el mensaje privado a agregar al thread.</param>
      public void AddMostRecent(PrivateMessage message)
      {
         if (message.FromUserID != message.ToUserID)
         {
            Messages.Add(message);
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.RemoteUserId = 0;
         this.RemoteUser = null;
         this.HaveUnreadMessages = false;
         this.LastMessagesDate = new DateTime(0);
         this.Messages = new List<PrivateMessage>();
      }

      #endregion

   }
}
