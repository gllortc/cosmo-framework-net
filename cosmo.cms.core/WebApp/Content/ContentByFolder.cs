﻿using Cosmo.Cms.Content;
using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.WebApp.Content
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

         PanelControl panelFolders = new PanelControl(this);
         panelFolders.Caption = "Navegación por carpetas";
         panelFolders.Content.Add(DocumentUI.ConvertFoldersToListGroup(this, folder, true));

         RightContent.Add(panelFolders);

         //-------------------------------------------------------------------
         // Lista de documentos
         //-------------------------------------------------------------------

         PanelControl panelDocs = new PanelControl(this);
         panelDocs.Caption = folder.Name;

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

         // Panel de herramientas administrativas

         if (Workspace.CurrentUser.CheckAuthorization(DocumentDAO.ROLE_CONTENT_EDITOR))
         {
            ButtonControl btnTool;

            PanelControl adminPanel = new PanelControl(this);
            adminPanel.Caption = "Administrar";

            btnTool = new ButtonControl(this);
            btnTool.Icon = IconControl.ICON_PLUS;
            btnTool.Text = "Nuevo artículo";
            btnTool.Color = ComponentColorScheme.Success;
            btnTool.IsBlock = true;
            btnTool.Href = ContentEdit.GetURL(folder.ID);

            adminPanel.Content.Add(btnTool);

            RightContent.Add(adminPanel);
         }
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
         // Nothing todo
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
