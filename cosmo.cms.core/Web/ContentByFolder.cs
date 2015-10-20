using Cosmo.Cms.Model.Content;
using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Cms.Web
{
   public class ContentByFolder : PageView
   {

      #region PageView Implementation

      public override void LoadPage()
      {
         DocumentFolder folder = null;
         DocumentDAO docs = null;

         // Agrega la meta-información de la página
         Title = DocumentDAO.SERVICE_NAME;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = Cosmo.UI.Controls.IconControl.ICON_BOOK;
         MainContent.Add(header);

         // Obtiene la carpeta a mostrar
         int folderid = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);

         // Obtiene la carpeta solicitada
         docs = new DocumentDAO(Workspace);
         folder = docs.GetFolder(folderid, true);

         if (folder == null)
         {
            ShowError("Categoria no encontrada",
                      "La categoria de artículos solicitada no existe o bien no se encuentra disponible.");
            return;
         }

         Title = folder.Name + " | " + DocumentDAO.SERVICE_NAME;
         ActiveMenuId = folder.MenuId;

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         //--------------------------------------------------------------
         // Cabecera
         //--------------------------------------------------------------

         List<DocumentFolder> folders = docs.GetFolderRoute(folder.ID);

         MainContent.Clear();
         header = new PageHeaderControl(this);
         header.Title = folder.Name;
         header.Icon = "glyphicon-folder-open";
         header.Breadcrumb = DocumentUI.ConvertToBreadcrumb(this, folders);
         MainContent.Add(header);

         //-------------------------------------------------------------------
         // Lista de carpetas / Columna lateral derecha
         //-------------------------------------------------------------------

         // Publisher's toolbar
         if (Workspace.CurrentUser.CheckAuthorization(DocumentDAO.ROLE_CONTENT_EDITOR))
         {
            ButtonControl btnTool;

            PanelControl adminPanel = new PanelControl(this);
            adminPanel.Text = "Administrar";

            btnTool = new ButtonControl(this);
            btnTool.Icon = IconControl.ICON_EDIT;
            btnTool.Text = "Editar";
            btnTool.Color = ComponentColorScheme.Success;
            btnTool.IsBlock = true;
            btnTool.Href = ContentFolderEdit.GetEditURL(folder.ID);
            adminPanel.Content.Add(btnTool);

            btnTool = new ButtonControl(this);
            btnTool.Icon = IconControl.ICON_DELETE;
            btnTool.Text = "Eliminar";
            btnTool.Color = ComponentColorScheme.Success;
            btnTool.IsBlock = true;
            btnTool.Href = ContentFolderEdit.GetEditURL(folder.ID);
            adminPanel.Content.Add(btnTool);

            btnTool = new ButtonControl(this);
            btnTool.Icon = IconControl.ICON_PLUS;
            btnTool.Text = "Nuevo contenido";
            btnTool.Color = ComponentColorScheme.Success;
            btnTool.IsBlock = true;
            btnTool.Href = ContentEdit.GetURL(folder.ID);
            adminPanel.Content.Add(btnTool);

            btnTool = new ButtonControl(this);
            btnTool.Icon = IconControl.ICON_PLUS;
            btnTool.Text = "Nueva subcarpeta";
            btnTool.Color = ComponentColorScheme.Success;
            btnTool.IsBlock = true;
            btnTool.Href = ContentFolderEdit.GetAddURL(folder.ID);
            adminPanel.Content.Add(btnTool);

            RightContent.Add(adminPanel);
         }

         PanelControl panelFolders = new PanelControl(this);
         panelFolders.Text = "Navegación por carpetas";
         panelFolders.Content.Add(DocumentUI.ConvertFoldersToListGroup(this, folder, true));

         RightContent.Add(panelFolders);

         //-------------------------------------------------------------------
         // Lista de documentos
         //-------------------------------------------------------------------

         // Content
         if (!string.IsNullOrWhiteSpace(folder.Description))
         {
            HtmlContentControl html = new HtmlContentControl(this, folder.Description);
            PanelControl docPanel = new PanelControl(this);
            docPanel.Content.Add(html);
            MainContent.Add(docPanel);
         }

         PanelControl panelDocs = new PanelControl(this);
         panelDocs.Text = folder.Name;

         // Genera la lista de documentos de la carpeta
         List<Document> documents = docs.GetDocuments(folderid);
         if (documents.Count > 0)
         {
            MediaListControl thlist = DocumentUI.ConvertToMediaList(this, documents);
            thlist.UseItemSeparator = true;

            panelDocs.Content.Add(thlist);
         }
         else
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Categoría vacía";
            callout.Text = "Esta categoría no contiene documentos. Navegue por las subcategorías situadas a la derecha de esta página para acceder a los documentos.";
            callout.Type = ComponentColorScheme.Information;

            panelDocs.Content.Add(callout);
         }

         MainContent.Add(panelDocs);
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Gets the URL to show the folder contents.
      /// </summary>
      /// <returns>A string representing the relative URL requested.</returns>
      public static string GetURL(int folderId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, folderId);

         return url.ToString();
      }

      #endregion

   }
}
