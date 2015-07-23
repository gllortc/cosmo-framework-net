using Cosmo.Communications.PrivateMessages;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Web
{
   /// <summary>
   /// Implements a manager class for private messages.
   /// </summary>
   [AuthenticationRequired]
   public class PrivateMessages : PageView
   {

      #region PageView Implementation

      public override void InitPage()
      {
         int userId = Parameters.GetInteger(Cosmo.Workspace.PARAM_USER_ID);
         PrivateMessageDAO msgDao = new PrivateMessageDAO(Workspace);

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = "Mensajes Privados";
         ActiveMenuId = string.Empty;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = "Mensajes Privados";
         header.Icon = IconControl.ICON_COMMENTS;
         MainContent.Add(header);

         PrivateMessagesPartialView pmPartial = new PrivateMessagesPartialView();
         PartialViewContainerControl thControl = new PartialViewContainerControl(this, 
                                                                                 pmPartial, 
                                                                                 UI.Scripting.Script.ScriptExecutionMethod.OnDocumentReady,
                                                                                 userId);
         MainContent.Add(thControl);

         //if (userId > 0)
         //{
         //   ChatMessage message;
         //   ChatControl chat = new ChatControl(this);

         //   PrivateMessageThread th = msgDao.GetThread(Workspace.CurrentUser.User.ID, userId);
         //   foreach (PrivateMessage msg in th.Messages)
         //   {
         //      message = new ChatMessage();
         //      message.DomID = "pmsg" + msg.ID;
         //      message.Author = Workspace.SecurityService.GetUser(msg.FromUserID).GetDisplayName();
         //      message.Time = msg.Sended.ToString(Formatter.FORMAT_DATETIME);
         //      message.Content = msg.Body;

         //      chat.Messages.Add(message);
         //   }

         //   MainContent.Add(chat);
         //}
         //else
         //{
         //   CalloutControl callout = new CalloutControl(this);
         //   callout.Type = ComponentColorScheme.Information;
         //   callout.Title = "Seleccione una conversación";
         //   callout.Text = "Seleccione la conversación (en la parte derecha) para ver los mensajes con una determinada persona.";

         //   MainContent.Add(callout);
         //}

         //-------------------------------
         // Configuración de la lista de conversaciones
         //-------------------------------

         List<PrivateMessageThread> threads = msgDao.GetThreads(Workspace.CurrentUser.User.ID);

         ListItem litem = null;
         ListGroupControl list = new ListGroupControl(this);
         list.Style = ListGroupControl.ListGroupStyle.Simple;
         foreach (PrivateMessageThread thread in threads)
         {
            // if (thread.Messages.Count > 0 && thread.RemoteUser != null)
            if (thread.RemoteUser != null)
            {
               litem = new ListItem();
               litem.Text = thread.RemoteUser.GetDisplayName();
               litem.Icon = IconControl.ICON_USER;
               litem.Href = "javascript:" + pmPartial.GetInvokeFunctionWithParameters(thread.RemoteUserId);
               litem.IsActive = (userId == thread.RemoteUserId);

               list.ListItems.Add(litem);
            }
         }

         PanelControl thPanel = new PanelControl(this);
         thPanel.Text = "Conversaciones";
         thPanel.Content.Add(list);

         RightContent.Add(thPanel);
      }

      public override void FormDataReceived(UI.Controls.FormControl receivedForm)
      {
         // throw new NotImplementedException();
      }

      public override void FormDataLoad(string formDomID)
      {
         // throw new NotImplementedException();
      }

      public override void LoadPage()
      {
         // throw new NotImplementedException();
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);

         return url.ToString();
      }

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <param name="userId">User identifier.</param>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL(int userId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Workspace.PARAM_USER_ID, userId);

         return url.ToString();
      }

      #endregion

   }
}
