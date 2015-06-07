using Cosmo.Cms.Forums;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System;
using System.Collections.Generic;

namespace Cosmo.WebApp.Forums
{
   [AuthenticationRequired]
   public class ForumMessageEdit :PageView
   {
      private int messageId;
      private int parentMsgId;
      private int channelId;
      private int pageIdx;
      private ForumsDAO forumDao = null;
      private ForumThread forumThread = null;
      private String formMode = Workspace.COMMAND_ADD;
      private FormControl form = null;

      public override void InitPage()
      {
         // Declaraciones
         ForumChannel channel = null;

         Title = ForumsDAO.SERVICE_NAME;
         ActiveMenuId = "forum";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene los parámetros
         parentMsgId = Parameters.GetInteger(ForumsDAO.PARAM_THREAD_ID, 0);
         channelId = Parameters.GetInteger(ForumsDAO.PARAM_CHANNEL_ID, 0);
         pageIdx = Parameters.GetInteger(ForumsDAO.PARAM_PAGE_NUM, 1);
         messageId = Parameters.GetInteger(ForumsDAO.PARAM_MESSAGE_ID, 0);
         formMode = (messageId <= 0 ? Workspace.COMMAND_ADD : Workspace.COMMAND_EDIT);

         // Obtiene las propiedades particulares de la carpeta actual
         forumDao = new ForumsDAO(Workspace);
         channel = forumDao.GetForum(channelId);
         if (channel == null)
         {
            ShowError("Categoria no encontrada",
                      "El foro solicitado no existe o bien no se encuentra disponible en estos momentos.");
            return;
         }

         // Obtiene las propiedades del mensaje original
         if (parentMsgId > 0)
         {
            forumThread = forumDao.GetThread(parentMsgId);
         }

         PageHeaderControl header = new PageHeaderControl(this);
         header.Icon = IconControl.ICON_COMMENTS;
         header.Title = ForumsDAO.SERVICE_NAME;
         header.SubTitle = channel.Name;
         MainContent.Add(header);

         // Agrega la meta-información de la página
         Title = ForumsDAO.SERVICE_NAME + " - Editar mensaje";

         // Genera el formulario para objetos del tipo User
         form = new FormControl(this);
         form.DomID = "frmMsgEdit";
         form.Caption = forumThread == null ? "Nuevo tema" : forumThread.Title;
         form.Icon = forumThread == null ? "fa-comment-o" : "fa-comments-o";

         FormFieldHidden parentid = new FormFieldHidden(this, ForumsDAO.PARAM_THREAD_ID, parentMsgId.ToString());
         form.Content.Add(parentid);

         FormFieldHidden chid = new FormFieldHidden(this, ForumsDAO.PARAM_CHANNEL_ID, channelId.ToString());
         form.Content.Add(chid);

         FormFieldHidden msgId = new FormFieldHidden(this, ForumsDAO.PARAM_MESSAGE_ID, messageId.ToString());
         form.Content.Add(msgId);

         FormFieldHidden pageNum = new FormFieldHidden(this, ForumsDAO.PARAM_PAGE_NUM, pageIdx.ToString());
         form.Content.Add(pageNum);

         FormFieldHidden formAction = new FormFieldHidden(this, Workspace.PARAM_COMMAND);
         form.Content.Add(formAction);

         if (parentMsgId == 0)
         {
            FormFieldText title = new FormFieldText(this, "title", "Título", FormFieldText.FieldDataType.Text);
            title.Required = true;
            form.Content.Add(title);
         }

         FormFieldEditor body = new FormFieldEditor(this, "body", "Mensaje", FormFieldEditor.FieldEditorType.BBCode);
         body.Required = true;
         form.Content.Add(body);

         form.FormButtons.Add(new ButtonControl(this, "cmdSend", "Enviar", IconControl.ICON_SEND, ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdCancel", "Cancelar", IconControl.ICON_REPLY, "", "history.back(-1);"));

         MainContent.Add(form);

         // Extracto de las normas de publicación
         List<String> rulesText = new List<String>();
         rulesText.Add("Estos foros están moderados. " + Workspace.Name + " se reserva el derecho a eliminar cualquier mensaje o hilo sin previo aviso, especialmente si no se ciñen al tema de este portal.");
         rulesText.Add("No se permite el uso de expresiones lesivas, discriminatorias ni la difusión de productos o actividades comerciales de ningún tipo.");
         rulesText.Add(Workspace.Name + " no se hace responsable de las opiniones vertidas por los usuarios en este foro ni necesariamente comparte las mismas.");
         rulesText.Add("Participando en este foro, usetd acepta implícitamente las Normas de participación en los foros.");
         
         HtmlContentControl rules = new HtmlContentControl(this);
         rules.AppendUnorderedList(rulesText);

         PanelControl rulesPanel = new PanelControl(this);
         rulesPanel.Caption = "Normas para la publicación de mensajes";
         rulesPanel.CaptionIcon = IconControl.ICON_BELL;
         rulesPanel.Content.Add(rules);

         MainContent.Add(rulesPanel);
      }

      public override void LoadPage()
      {
         if (forumDao == null)
         {
            forumDao = new ForumsDAO(Workspace);
         }

         if (messageId > 0)
         {
            ForumMessage msg = forumDao.GetMessage(messageId);

            form.SetFieldValue(Workspace.PARAM_COMMAND, Workspace.COMMAND_EDIT);
            form.SetFieldValue("body", msg.Body);

            if (parentMsgId <= 0)
            {
               form.SetFieldValue("title", msg.Title);
            }
         }
         else
         {
            form.SetFieldValue(Workspace.PARAM_COMMAND, Workspace.COMMAND_ADD);
         }
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // Recoge los datos recibidos del formulario
         ForumMessage message = new ForumMessage();
         message.ID = receivedForm.GetIntFieldValue(ForumsDAO.PARAM_MESSAGE_ID);
         message.ForumID = receivedForm.GetIntFieldValue(ForumsDAO.PARAM_CHANNEL_ID);
         message.ParentMessageID = receivedForm.GetIntFieldValue(ForumsDAO.PARAM_THREAD_ID);
         message.Title = receivedForm.GetStringFieldValue("title");
         message.Body = receivedForm.GetStringFieldValue("body");
         message.IP = Request.Params["HTTP_X_FORWARDED_FOR"];
         message.UserID = Workspace.CurrentUser.User.ID;
         message.Name = Workspace.CurrentUser.User.Login;
         message.BBCodes = true;

         // Persiste los datos en la BBDD
         ForumsDAO forumsDao = new ForumsDAO(Workspace);
         if (Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND).Equals(Cosmo.Workspace.COMMAND_ADD))
         {
            forumsDao.AddMessage(message);
         }
         else if (Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND).Equals(Cosmo.Workspace.COMMAND_EDIT))
         {
            forumDao.UpdateMessage(message);
         }

         // Redirige al usuario a la página del thread creado o para el que se ha editado el mensaje
         Redirect(ForumsDAO.GetThreadUrl(message.ParentMessageID, message.ForumID, pageIdx));
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }
   }
}
