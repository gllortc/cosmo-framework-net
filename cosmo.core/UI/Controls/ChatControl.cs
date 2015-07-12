using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un componente Chat para representar un hilo de mensajes de texto.
   /// No es un componente nativo de Bootstrap. Está realizado usando componentes nativos.
   /// </summary>
   public class ChatControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ChatControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public ChatControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el título visible del componente.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Gets or sets la altura (en píxels) de la zona de mensajes.
      /// </summary>
      public int Height { get; set; }

      /// <summary>
      /// Indica si se debe mostrar el formulario de redacción de mensajes.
      /// Este formulario sirve sólo para envios a través de Cosmo Communication Services.
      /// </summary>
      public bool FormShow { get; set; }

      /// <summary>
      /// Gets or sets el ID del usuario destinatario del mensaje.
      /// Este dato sirve sólo para envios a través de Cosmo Communication Services.
      /// </summary>
      public int FormToUserID { get; set; }

      /// <summary>
      /// Gets or sets el ID (DOM) del formulario de envio de mensajes.
      /// Este dato sirve sólo para envios a través de Cosmo Communication Services.
      /// </summary>
      public string FormDomID { get; set; }

      /// <summary>
      /// Gets or sets el ID (DOM) del botón de envío de mensajes.
      /// Este dato sirve sólo para envios a través de Cosmo Communication Services.
      /// </summary>
      public string FormSubmitButtonID { get; set; }

      /// <summary>
      /// Gets or sets la lista de mensajes que aparecen en el chat.
      /// </summary>
      public List<ChatMessage> Messages { get; set; }

      /// <summary>
      /// Indica si se debe calcular la altura de forma automática para evitar el scroll vertical.
      /// Si se establece a <c>true</c> no se tendrá en cuenta el valor de la propiedad <c>Height</c>.
      /// </summary>
      public bool AutoSize { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.AutoSize = false;
         this.FormShow = false;
         this.FormToUserID = 0;
         this.Height = 400;
         this.Caption = string.Empty;
         this.FormDomID = "frmPMsgSend";
         this.FormSubmitButtonID = "csSendPMsg";
         this.Messages = new List<ChatMessage>();
      }

      #endregion

      #region Disabled Code

      /*
      /// <summary>
      /// Convierte el componente en código XHTML.
      /// </summary>
      /// <returns>Una cadena que contiene el código XHTML necesario para representar el componente en un navegador.</returns>
      public string ToXhtml()
      {
         Panel panel = new Panel();
         StringBuilder script = new StringBuilder();
         StringBuilder xhtml = new StringBuilder();

         if (_autosize)
         {
            script.AppendLine("var chHeight = $(window).height();");
            script.AppendLine("chHeight = chHeight - 200;");
            // script.AppendLine("console.log('Posición: ' + $('#pmChatMsgs').position());");
         }
         else
         {
            script.AppendLine("var chHeight = " + _height + ";");
         }

         script.AppendLine("$('#pmChatMsgs').slimScroll({");
         script.AppendLine("  height: chHeight + 'px',");
         script.AppendLine("  railVisible: true,");
         script.AppendLine("  start: 'bottom'");
         script.AppendLine("});");

         panel.DomID = this.DomID;
         panel.Caption = this.Caption;

         // Representa los mensajes
         xhtml.Clear();
         foreach (ChatMessage message in _messages)
         {
            xhtml.AppendLine(message.ToXhtml());
         }
         xhtml.AppendLine(_messages.Count <= 0 ? "<h2><small>No hay mensajes entre vosotros</small></h2>" : string.Empty);
         panel.ContentXhtml = xhtml.ToString();
         panel.ContentDomId = "pmChatMsgs";

         // Representa el formulario de envio
         if (_showComposeForm)
         {
            xhtml.Clear();
            xhtml.AppendLine("    <modal id=\"" + _formDomId + "\" role=\"modal\">");
            xhtml.AppendLine("      <input type=\"hidden\" name=\"" + Workspace.PARAM_COMMAND + "\" value=\"" + PrivateMessageDAO.COMMAND_SEND + "\">");
            xhtml.AppendLine("      <input type=\"hidden\" name=\"" + Workspace.PARAM_USER_ID + "\" value=\"" + _formToUserId + "\">");
            xhtml.AppendLine("      <div class=\"modal-group\">");
            xhtml.AppendLine("        <textarea id=\"" + PrivateMessageDAO.PARAM_BODY + "\" name=\"" + PrivateMessageDAO.PARAM_BODY + "\" class=\"modal-control\" rows=\"1\" placeholder=\"Escribe el texto a enviar...\"></textarea>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("      <button id=\"" + _formSubmitButtonId + "\" type=\"button\" class=\"btn btn-sm btn-primary\" data-loading-text=\"Enviando...\">" + Glyphicon.GetIcon(Glyphicon.ICON_SEND) + "&nbsp;&nbsp;Enviar</button>");
            xhtml.AppendLine("    </modal>");
            panel.Footer = xhtml.ToString();

            script.AppendLine("cosmoCommServices.sendPMsg('" + _formDomId + "','" + _formSubmitButtonId + "'," + _formToUserId + ");");
            script.AppendLine("$('#" + PrivateMessageDAO.PARAM_BODY + "').autosize({append: \"\\n\"});");
            script.AppendLine("$('#" + PrivateMessageDAO.PARAM_BODY + "').inputlimiter({");
				script.AppendLine("	remText: '%n character%s remaining...',");
				script.AppendLine("	limitText: 'max allowed : %n.'");
				script.AppendLine("});");
         }

         // Conjunta todo el código XHTML y el JavaScript
         xhtml.Clear();
         xhtml.AppendLine(panel.ToXhtml());
         xhtml.AppendLine("  <script type=\"text/javascript\">");
         xhtml.AppendLine("  $( document ).ready(function() {");
         xhtml.AppendLine(script.ToString());
         xhtml.AppendLine("  });");
         xhtml.AppendLine("  </script>");

         return xhtml.ToString();
      }
      */

      #endregion

   }
}
