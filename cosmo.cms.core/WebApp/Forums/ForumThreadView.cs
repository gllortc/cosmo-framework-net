using Cosmo.Cms.Forums;
using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils.Html;
using Cosmo.WebApp.UserServices;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.WebApp.Forums
{
   /// <summary>
   /// FORUM SERVICE.
   /// Muestra el contenido de un thread.
   /// </summary>
   public class ForumThreadView : PageView
   {
      // Internal data declarations
      ForumsDAO fdao = null;

      #region PageView Implementation

      public override void LoadPage()
      {
         bool isModerator = Workspace.CurrentUser.CheckAuthorization(ForumsDAO.ROLE_FORUM_MODERATOR);

         Title = ForumsDAO.SERVICE_NAME;
         ActiveMenuId = "mnuForum" + Parameters.GetString(ForumsDAO.PARAM_CHANNEL_ID);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         // Obtiene los parámetros
         int channelid = Parameters.GetInteger(ForumsDAO.PARAM_CHANNEL_ID);
         int threadid = Parameters.GetInteger(ForumsDAO.PARAM_THREAD_ID);
         int page = Parameters.GetInteger(ForumsDAO.PARAM_PAGE_NUM, 1);

         // if (channelid <= 0) Redirect(ForumsDAO.URL_HOME);
         if (threadid <= 0) Redirect(ForumFolder.GetURL(channelid));

         // Recupera la información del canal, thread y mensajes
         fdao = new ForumsDAO(Workspace);
         ForumChannel forum = fdao.GetForum(channelid);
         ForumThread thread = fdao.GetThread(threadid);

         // Obtiene las propiedades particulares de la carpeta actual
         if (thread == null)
         {
            ShowError("Hilo no accesible",
                      "Este hilo no existe, ha sido eliminado o no está disponible.");
            return;
         }

         // Agrega la meta-información de la página
         Title = ForumsDAO.SERVICE_NAME + " - " + thread.Title;

         // Insert a modal to show user data
         UserDataModal userData = new UserDataModal();
         Modals.Add(userData);

         //--------------------------------------------------------------
         // Genera la lista de mensajes
         //--------------------------------------------------------------

         bool first = true;
         TimelineItem item = null;
         TimelineControl timeline = new TimelineControl(this);
         List<ForumMessage> messages = new List<ForumMessage>();
         ForumThread.ThreadMessagesOrder order = ForumThread.ThreadMessagesOrder.Ascending;

         order = thread.GetMessageOrder(Workspace.Context.Request, Workspace.Context.Response);
         messages = fdao.GetThreadMessages(thread.ID, order);

         if (messages.Count <= 0)
         {
            ShowError("Mensajes no accesibles",
                      "No se han podido recuperar los mensajes de este hilo.");
            return;
         }

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = ForumsDAO.SERVICE_NAME;
         header.SubTitle = forum.Name;
         header.Icon = IconControl.ICON_COMMENTS;
         MainContent.Add(header);

         // Contenedor de los mensajes
         PanelControl panel = new PanelControl(this);
         panel.Caption = thread.Title;

         // Genera la barra de herramientas
         if (isModerator)
         {
            AddAdministrationTools(panel, thread.ID, forum.ID);
         }
         panel.ButtonBar.Buttons.Add(new ButtonControl(this, 
                                                "btnAddMsg", 
                                                "Enviar comentario", 
                                                IconControl.ICON_PLUS, 
                                                ForumMessageEdit.GetURL(threadid, channelid, page), 
                                                string.Empty));
         panel.ButtonBar.Buttons.Add(new ButtonControl(this, 
                                                "btnOrder", 
                                                "Orden " + (order == ForumThread.ThreadMessagesOrder.Ascending ? "descendente" : "ascendente"),
                                                order == ForumThread.ThreadMessagesOrder.Ascending ? IconControl.ICON_CHEVRON_DOWN : IconControl.ICON_CHEVRON_UP,
                                                ForumThreadView.GetURL(threadid, channelid, page, 1 - (int)order), 
                                                string.Empty));
         panel.ButtonBar.Buttons.Add(new ButtonControl(this, 
                                                "btnReturnTop", 
                                                "Volver al canal", 
                                                IconControl.ICON_REPLY,
                                                ForumFolder.GetURL(channelid, page, "msg" + threadid), 
                                                string.Empty));

         // Si el hilo está cerrado advierte al usuario
         if (thread.Closed)
         {
            panel.Content.Add(new AlertControl(this, "Este hilo está cerrado y no admite más comentarios.", ComponentColorScheme.Warning));
         }

         // Muestra los mensajes
         string month = string.Empty;
         BBCodeTextParser formatter = new BBCodeTextParser();
         foreach (ForumMessage message in messages)
         {
            if (!month.Equals(message.Date.ToString(Formatter.FORMAT_LONGDATE)) && !first)
            {
               month = message.Date.ToString(Formatter.FORMAT_LONGDATE);

               item = new TimelineItem();
               item.Type = TimelineItem.TimelineItemType.Label;
               item.Title = month;
               item.BackgroundColor = ComponentBackgroundColor.Red;
               timeline.AddItem(item);
            }

            item = new TimelineItem();
            item.ID = "msg" + message.ID;
            item.Type = TimelineItem.TimelineItemType.Entry;
            item.TitleControl = new UserLinkControl(this, message.UserID, message.Name, userData);
            item.Icon = message.ParentMessageID == 0 ? IconControl.ICON_ENVELOPE : IconControl.ICON_REPLY;
            item.Body = formatter.ParseText(message.Body);
            item.Time = message.Date.ToString(Formatter.FORMAT_DATETIME);

            if (Workspace.CurrentUser.IsAuthenticated)
            {
               // Agrega el botón de editar mensaje si el mensaje es del usuario actual o si 
               // el usuario actual es moderador del foro
               if (message.UserID == Workspace.CurrentUser.User.ID || isModerator)
               {
                  item.Buttons.Add(new ButtonControl(this, 
                                              "cmdEditMsg" + message.ID, 
                                              "Editar", 
                                              IconControl.ICON_EDIT,
                                              ForumMessageEdit.GetURL(message.ID, message.ParentMessageID, message.ForumID, page), 
                                              string.Empty));
               }
            }

            timeline.AddItem(item);

            first = false;
         }

         ButtonGroupControl bottomBar = new ButtonGroupControl(this);
         bottomBar.Buttons.Add(new ButtonControl(this, "cmdTop", "Subir", IconControl.ICON_ARROW_UP, "#" + LINK_TOP_PAGE, string.Empty));

         panel.Content.Add(timeline);
         panel.Content.Add(bottomBar);
         MainContent.Add(panel);

         // Habilita Linkify para convertir links de texto en hyperenlaces.
         // URL: http://soapbox.github.io/jQuery-linkify/
         // Necesario para compatibilizar BBCode actual con anteriores formatos de enlaces
         SimpleScript linkify = new SimpleScript(this);
         linkify.ExecutionType = Script.ScriptExecutionMethod.OnDocumentReady;
         linkify.AppendSourceLine("$('.timeline-body').linkify();");
         Scripts.Add(linkify);
      }

      public override void InitPage()
      {
         // Nothing to do
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // Nothing to do
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Permite obtener una URL relativa a un thread concreto de los foros.
      /// </summary>
      /// <param name="threadId">Identificador del thread.</param>
      public static string GetURL(int threadId, int channelId)
      {
         return GetURL(threadId, channelId, 1);
      }

      /// <summary>
      /// Permite obtener una URL relativa a un thread concreto de los foros.
      /// </summary>
      /// <param name="threadId">Identificador del thread.</param>
      /// <param name="pageNum">Número de página (para listado de threads paginados).</param>
      public static string GetURL(int threadId, int channelId, int pageNum, int order)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(ForumsDAO.PARAM_THREAD_ID, threadId);
         url.AddParameter(ForumsDAO.PARAM_CHANNEL_ID, channelId);
         url.AddParameter(ForumsDAO.PARAM_PAGE_NUM, pageNum);
         url.AddParameter(ForumsDAO.PARAM_ORDER, order);

         return url.ToString();
      }

      /// <summary>
      /// Permite obtener una URL relativa a un thread concreto de los foros.
      /// </summary>
      /// <param name="threadId">Identificador del thread.</param>
      /// <param name="pageNum">Número de página (para listado de threads paginados).</param>
      public static string GetURL(int threadId, int channelId, int pageNum)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(ForumsDAO.PARAM_THREAD_ID, threadId);
         url.AddParameter(ForumsDAO.PARAM_CHANNEL_ID, channelId);
         url.AddParameter(ForumsDAO.PARAM_PAGE_NUM, pageNum);

         return url.ToString();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Genera el menú de herramientas administrativas.
      /// </summary>
      private void AddAdministrationTools(PanelControl panel, int threadId, int channelId)
      {
         // Genera el formulario para mover el hilo actual a otro canal.
         Modals.Add(new ForumThreadToggleStatusModal(threadId));
         Modals.Add(new ForumThreadRemoveModal(threadId));
         Modals.Add(new ForumThreadMoveModal(threadId, channelId));

         SplitButtonControl adminDropdown = new SplitButtonControl(this);
         adminDropdown.Text = "Moderación";
         adminDropdown.Icon = IconControl.ICON_WRENCH;
         adminDropdown.Size = ButtonControl.ButtonSizes.Small;
         adminDropdown.MenuOptions.Add(new ButtonControl(this,
            "mnu-admin-close",
            "Cerrar/reactivar hilo",
            Modals[0]));
         adminDropdown.MenuOptions.Add(new ButtonControl(this,
            "mnu-admin-delete",
            "Eliminar hilo",
            Modals[1]));
         adminDropdown.MenuOptions.Add(new ButtonControl(this,
            "mnu-admin-move",
            "Mover hilo a otro canal",
            Modals[2]));

         panel.ButtonBar.Buttons.Add(adminDropdown);
      }

      #endregion

   }
}