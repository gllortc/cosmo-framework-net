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
               litem.Href = "javascript:" + pmPartial.GetInvokeCall(thread.RemoteUserId);
               litem.IsActive = (userId == thread.RemoteUserId);

               list.ListItems.Add(litem);
            }
         }

         PanelControl thPanel = new PanelControl(this);
         thPanel.Text = "Conversaciones";
         thPanel.Content.Add(list);

         RightContent.Add(thPanel);
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
