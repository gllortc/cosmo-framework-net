using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Representa un mnesaje de chat.
   /// </summary>
   public class ChatMessage
   {
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

      // Declaración de variables sinternas
      private string _id;
      private string _caption;
      private string _content;
      private string _author;
      private string _time;
      private List<ChatMessageToolbarButtonControl> _buttons;
      private ChatMessageDirections _direction;

      /// <summary>
      /// Devuelve una instancia de <see cref="PopoverControl"/>.
      /// </summary>
      public ChatMessage()
      {
         Initialize();
      }

      /*/// <summary>
      /// Devuelve o establece el identificador del elemento en el DOM del navegador.
      /// </summary>
      public string DomID
      {
         get { return _domId; }
         set { _domId = value; }
      }*/

      /// <summary>
      /// Devuelve o establece el identificador del mesnaje.
      /// </summary>
      public string DomID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Devuelve o establece el título del mensaje.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Devuelve o establece el contenido del mensaje.
      /// Esta propiedad sólo admite texto (no reconoce HTML).
      /// </summary>
      public string Content
      {
         get { return _content; }
         set { _content = value; }
      }

      /// <summary>
      /// Devuelve o establece el autor del mensaje.
      /// </summary>
      public string Author
      {
         get { return _author; }
         set { _author = value; }
      }

      /// <summary>
      /// Devuelve o establece la fecha/hora de publicación del mensaje.
      /// </summary>
      public string Time
      {
         get { return _time; }
         set { _time = value; }
      }

      /// <summary>
      /// Establece la dirección que en que muestra el mensaje (permite definir, por ejemplo, derecha para el usuario e izquierda para el interlocutor).
      /// </summary>
      public ChatMessageDirections Direction
      {
         get { return _direction; }
         set { _direction = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de botones que contiene la barra de herramientas del mensaje.
      /// </summary>
      public List<ChatMessageToolbarButtonControl> ToolbarButtons
      {
         get { return _buttons; }
         set { _buttons = value; }
      }
      /*
      /// <summary>
      /// Convierte el componente en código XHTML.
      /// </summary>
      /// <returns>Una cadena que contiene el código XHTML necesario para representar el componente en un navegador.</returns>
      public string ToXhtml()
      {
         StringBuilder xhtml = new StringBuilder();

         // xhtml.AppendLine("<div id=\"" + _domId + "\" class=\"popover " + ConvertPopoverDirectionToString(_direction) + " chat-message-" + ConvertPopoverDirectionToString(_direction) + "\">");
         xhtml.AppendLine("  <div class=\"arrow\"></div>");
         xhtml.AppendLine("  <div class=\"chat-msg-toolbar clearfix\">");
         xhtml.AppendLine("    <div class=\"chat-msg-title\">" + HttpUtility.HtmlDecode(_caption) + "</div>");
         if (_buttons.Count > 0)
         {
            xhtml.AppendLine("    <div class=\"btn-group pull-right\">");
            foreach (ChatMessageToolbarButton button in _buttons)
            {
               xhtml.AppendLine("      <a href=\"" + button.Href + "\" class=\"btn btn-default btn-xs\">" + (string.IsNullOrWhiteSpace(button.Icon) ? string.Empty : Icon.GetIcon(button.Icon)) + " " + HttpUtility.HtmlDecode(button.Caption) + "</a>");
            }
            xhtml.AppendLine("    </div>");
         }
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <div class=\"popover-content\">");
         xhtml.AppendLine(HttpUtility.HtmlDecode(_content).Replace("\n", "<br />\n"));
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }
      */
      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _id = string.Empty;
         _caption = string.Empty;
         _content = string.Empty;
         _author = string.Empty;
         _time = string.Empty;
         _buttons = new List<ChatMessageToolbarButtonControl>();
         _direction = ChatMessageDirections.Left;
      }
   }
}
