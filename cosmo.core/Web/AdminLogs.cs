using Cosmo.Diagnostics;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Web
{
   /// <summary>
   /// Implements a page that allows admins to manage user accounts.
   /// </summary>
   [AuthorizationRequired(Cosmo.Workspace.ROLE_ADMINISTRATOR)]
   public class AdminLogs : PageView
   {

      #region PageView Implementation

      public override void InitPage()
      {
         string icon = string.Empty;
         TableRow row;
         TableControl table;

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = "Registro de eventos";
         ActiveMenuId = "adminLogs";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = "Registro de eventos";
         header.Icon = IconControl.ICON_USER;
         MainContent.Add(header);

         table = new TableControl(this);
         table.Condensed = true;
         table.Striped = true;

         row = new TableRow(6);
         row.Cells[0] = new TableCell("Application");
         row.Cells[1] = new TableCell("Context");
         row.Cells[2] = new TableCell("User");
         row.Cells[3] = new TableCell("Date");
         table.Header = row;

         AdminLogDataModal dataModal = new AdminLogDataModal();
         Modals.Add(dataModal);

         Dictionary<string, object> modalArgs = new Dictionary<string,object>();

         foreach (LogEntry entry in Workspace.Logger.GetAll())
         {
            switch (entry.Type)
            {
               case LogEntry.LogEntryType.EV_INFORMATION:
                  icon = IconControl.GetIcon(this, IconControl.ICON_CHECK);
                  break;
               case LogEntry.LogEntryType.EV_WARNING:
                  icon = IconControl.GetIcon(this, IconControl.ICON_WARNING);
                  break;
               case LogEntry.LogEntryType.EV_ERROR:
                  icon = IconControl.GetIcon(this, IconControl.ICON_PROHIBITED_CIRCLE);
                  break;
               case LogEntry.LogEntryType.EV_SECURITY:
                  icon = IconControl.GetIcon(this, IconControl.ICON_LOCK);
                  break;
            }

            if (modalArgs.ContainsKey(Cosmo.Workspace.PARAM_OBJECT_ID))
            {
               modalArgs[Cosmo.Workspace.PARAM_OBJECT_ID] = entry.ID;
            }
            else
            {
               modalArgs.Add(Cosmo.Workspace.PARAM_OBJECT_ID, entry.ID);
            }

            row = new TableRow(6);
            row.Cells[0] = new TableCell(icon + " " + entry.ApplicationName, Script.ConvertInvokeCallToLink(dataModal.GetInvokeCall(modalArgs)));
            row.Cells[1] = new TableCell(entry.Context);
            row.Cells[2] = new TableCell(entry.UserLogin);
            row.Cells[3] = new TableCell(entry.Date.ToString(Calendar.FORMAT_DATETIME));

            table.Rows.Add(row);
         }

         PanelControl resultsPanel = new PanelControl(this);
         resultsPanel.Text = "Registered events";
         resultsPanel.Content.Add(table);

         MainContent.Add(resultsPanel);
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

      #endregion

   }
}
