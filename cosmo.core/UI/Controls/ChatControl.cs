using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un componente Chat para representar un hilo de mensajes de texto.
   /// No es un componente nativo de Bootstrap. Está realizado usando componentes nativos.
   /// </summary>
   public class ChatControl : FormControl
   {
      // Internal data declarations
      private int toUsrID;

      /// <summary>Field name for message text.</summary>
      public const string FIELD_MESSAGE_DOMID = "txtMsg";
      /// <summary>Field name for message text.</summary>
      public const string FIELD_TOUSER_DOMID = Workspace.PARAM_USER_ID;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ChatControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public ChatControl(View parentView)
         : base(parentView)
      {
         Initialize();
         CreateMessageForm();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ChatControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="toUserID">Remote user unique ID.</param>
      public ChatControl(View parentView, int toUserID)
         : base(parentView)
      {
         Initialize();

         this.toUsrID = toUserID;

         CreateMessageForm();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets la altura (en píxels) de la zona de mensajes.
      /// </summary>
      public int Height { get; set; }

      /// <summary>
      /// Indica si se debe mostrar el formulario de redacción de mensajes.
      /// </summary>
      public bool FormShow { get; set; }

      /// <summary>
      /// Gets or sets the remote user unique ID.
      /// </summary>
      public int ToUserID 
      { 
         get { return toUsrID; }
         set
         {
            toUsrID = value;
            this.SetFieldValue(ChatControl.FIELD_TOUSER_DOMID, toUsrID);
         } 
      }

      /// <summary>
      /// Gets or sets el ID (DOM) del botón de envío de mensajes.
      /// </summary>
      public string FormSubmitButtonID { get; set; }

      /// <summary>
      /// Gets or sets the message list.
      /// </summary>
      public List<ChatMessage> Messages { get; set; }

      /// <summary>
      /// Indica si se debe calcular la altura de forma automática para evitar el scroll vertical.
      /// Si se establece a <c>true</c> no se tendrá en cuenta el valor de la propiedad <c>Height</c>.
      /// </summary>
      public bool AutoSize { get; set; }

      /// <summary>
      /// Gets or sets the buttons placed at the top of chat control.
      /// </summary>
      public List<ButtonControl> Buttons { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.toUsrID = 0;

         this.AutoSize = false;
         this.FormShow = true;
         this.Height = 400;
         this.Text = string.Empty;
         this.FormSubmitButtonID = "csSendPMsg";
         this.Messages = new List<ChatMessage>();
         this.Buttons = new List<ButtonControl>();
      }

      /// <summary>
      /// Include a form to manage the message sending.
      /// </summary>
      private void CreateMessageForm()
      {
         this.AddFormSetting(ChatControl.FIELD_TOUSER_DOMID, this.ToUserID);
         this.Content.Add(new FormFieldText(this.ParentView, ChatControl.FIELD_MESSAGE_DOMID));
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
