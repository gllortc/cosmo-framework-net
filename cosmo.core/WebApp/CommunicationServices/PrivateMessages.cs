using Cosmo.Communications.PrivateMessages;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils.Html;
using System;
using System.Collections.Generic;

namespace Cosmo.WebApp.CommunicationServices
{
   [AuthenticationRequired]
   public class PrivateMessages : PageViewContainer
   {
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

         if (userId > 0)
         {
            ChatMessage message;
            ChatControl chat = new ChatControl(this);

            PrivateMessageThread th = msgDao.GetThread(Workspace.CurrentUser.User.ID, userId);
            foreach (PrivateMessage msg in th.Messages)
            {
               message = new ChatMessage();
               message.DomID = "pmsg" + msg.ID;
               message.Author = Workspace.AuthenticationService.GetUser(msg.FromUserID).GetDisplayName();
               message.Time = msg.Sended.ToString(Formatter.FORMAT_DATETIME);
               message.Content = msg.Body;

               chat.Messages.Add(message);
            }

            MainContent.Add(chat);
         }
         else
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Type = ComponentColorScheme.Information;
            callout.Title = "Seleccione una conversación";
            callout.Text = "Seleccione la conversación (en la parte derecha) para ver los mensajes con una determinada persona.";

            MainContent.Add(callout);
         }

         //-------------------------------
         // Configuración de la lista de conversaciones
         //-------------------------------

         List<PrivateMessageThread> threads = msgDao.GetThreads(Workspace.CurrentUser.User.ID);

         Url url = null;
         ListItem litem = null;
         ListGroupControl list = new ListGroupControl(this);
         list.Style = ListGroupControl.ListGroupStyle.Simple;
         foreach (PrivateMessageThread thread in threads)
         {
            if (thread.Messages.Count > 0 && thread.RemoteUser != null)
            {
               url = new Url("PrivateMessages");
               url.AddParameter(Cosmo.Workspace.PARAM_USER_ID, thread.RemoteUserId);

               litem = new ListItem();
               litem.Caption = thread.RemoteUser.GetDisplayName();
               litem.Href = url.ToString();
               litem.IsActive = (Parameters.GetInteger(Cosmo.Workspace.PARAM_USER_ID) == thread.RemoteUserId);

               list.ListItems.Add(litem);
            }
         }

         RightContent.Add(list);
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
   }
}
