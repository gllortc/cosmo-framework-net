using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implements a chat message for the <see cref="ChatControl"/>.
   /// </summary>
   public class ChatMessage
   {

      #region Enumerations

      /// <summary>
      /// Enumera las direcciones posibles del elemento.
      /// </summary>
      public enum ChatMessageDirections
      {
         /// <summary>Orientación derecha</summary>
         Right,
         /// <summary>Orientación izquierda</summary>
         Left
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ChatMessage"/>.
      /// </summary>
      public ChatMessage()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del mesnaje.
      /// </summary>
      public string DomID { get; set; }

      /// <summary>
      /// Gets or sets el título del mensaje.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Gets or sets el contenido del mensaje.
      /// Esta propiedad sólo admite texto (no reconoce HTML).
      /// </summary>
      public string Content { get; set; }

      /// <summary>
      /// Gets or sets el autor del mensaje.
      /// </summary>
      public string Author { get; set; }

      /// <summary>
      /// Gets or sets la fecha/hora de publicación del mensaje.
      /// </summary>
      public string Time { get; set; }

      /// <summary>
      /// Establece la dirección que en que muestra el mensaje (permite definir, por ejemplo, derecha para el usuario e izquierda para el interlocutor).
      /// </summary>
      public ChatMessageDirections Direction { get; set; }

      /// <summary>
      /// Gets or sets la lista de botones que contiene la barra de herramientas del mensaje.
      /// </summary>
      public List<ChatMessageToolbarButtonControl> ToolbarButtons { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.DomID = string.Empty;
         this.Caption = string.Empty;
         this.Content = string.Empty;
         this.Author = string.Empty;
         this.Time = string.Empty;
         this.ToolbarButtons = new List<ChatMessageToolbarButtonControl>();
         this.Direction = ChatMessageDirections.Left;
      }

      #endregion

   }
}
