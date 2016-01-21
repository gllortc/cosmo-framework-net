using Cosmo.Cms.Model.Content;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Cms.Web
{
   /// <summary>
   /// Implements a page that allows admins to manage user accounts.
   /// </summary>
   [AuthorizationRequired(DocumentDAO.ROLE_CONTENT_EDITOR)]
   public class ContentAdmin : PageView
   {

      #region PageView Implementation

      public override void InitPage()
      {
         string icon = string.Empty;
         DocumentFolder folder;
         TableRow row;
         TableControl table;

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = "Content administration";
         ActiveMenuId = "adminContent";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = "Content administration";
         header.Icon = IconControl.ICON_EDIT;
         MainContent.Add(header);

         table = new TableControl(this);
         table.Condensed = true;
         table.Striped = true;

         row = new TableRow(6);
         row.Cells[0] = new TableCell("Content");
         row.Cells[1] = new TableCell("Folder");
         row.Cells[2] = new TableCell("Owner");
         row.Cells[3] = new TableCell("Status");
         table.Header = row;

         DocumentDAO docs = new DocumentDAO(Workspace);

         Dictionary<string, object> modalArgs = new Dictionary<string,object>();

         foreach (Document document in docs.GetByPublishStatus(Model.CmsPublishStatus.PublishStatus.Unpublished))
         {
            icon = IconControl.GetIcon(this, IconControl.ICON_FILE);

            folder = docs.GetFolder(document.FolderId, false);

            row = new TableRow(6);
            row.Cells[0] = new TableCell(icon + " " + document.Title, ContentEdit.GetURL(document.FolderId, document.ID));
            row.Cells[1] = new TableCell(folder != null ? folder.Name : "<unavailable>");
            row.Cells[2] = new TableCell(document.Owner);
            row.Cells[3] = new TableCell(document.Status.ToString());
            table.Rows.Add(row);
         }

         PanelControl resultsPanel = new PanelControl(this);
         resultsPanel.Text = "Unpublished content";
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
