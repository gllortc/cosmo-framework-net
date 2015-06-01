using Cosmo.Cms.Photos;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Collections.Generic;

namespace Cosmo.WebApp.Photos
{
   public class PhotosBrowse : PageViewContainer
   {
      public override void LoadPage()
      {
         List<PhotoFolder> tree = null;
         PhotoDAO docs = null;

         // Agrega la meta-información de la página
         Title = PhotoDAO.SERVICE_NAME;
         ActiveMenuId = "photos";

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = PhotoDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_CAMERA;
         MainContent.Add(header);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene la carpeta a mostrar
         // int folderid = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);

         // Obtiene el árbol d ecarpetas
         /*docs = new PhotoDAO(Workspace);
         tree = docs.GetFoldersTree(0);

         if (tree == null || tree.Count <= 0)
         {
            ShowError("Servicio no disponible",
                      "No se ha encontrado ninguna carpeta de fotografias.");
            return;
         }*/

         //-------------------------------------------------------------------
         // Árbol de carpetas
         //-------------------------------------------------------------------

         MainContent.Add(GetFoldersTreeView());

         //-------------------------------------------------------------------
         // Opciones y herramientas
         //-------------------------------------------------------------------

         ButtonControl recentBtn = new ButtonControl(this);
         recentBtn.Icon = IconControl.ICON_CALENDAR;
         recentBtn.Text = "Fotografias recientes";
         recentBtn.IsBlock = true;
         recentBtn.Href = PhotoDAO.GetRecentPhotosURL();

         ButtonControl myBtn = new ButtonControl(this);
         myBtn.Icon = IconControl.ICON_USER;
         myBtn.Text = "Mis fotografias";
         myBtn.IsBlock = true;
         myBtn.Href = PhotoDAO.GetUserPhotosURL();
         
         PanelControl toolsPanel = new PanelControl(this);
         toolsPanel.Content.Add(recentBtn);
         toolsPanel.Content.Add(myBtn);

         RightContent.Add(toolsPanel);

         PanelControl addPanel = new PanelControl(this);
         addPanel.Caption = "Enviar imágenes";
         addPanel.ContentXhtml = "<p>Si desea enviar sus imágenes y es susbscriptor puede hacerlo desde la carpeta dónde desea agregarlas. Acceda a la carpeta dónde las desea agregar y pulse el enlace Enviar imagen situado en la barra vertical derecha.</p>";

         RightContent.Add(addPanel);
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

      #region Private Members

      private const string CACHE_PHOTOS_FOLDERS_TREEVIEW = "cosmo.cms.photos.folders.treeview";

      private PanelControl GetFoldersTreeView()
      {
         if (Cache.Exist(CACHE_PHOTOS_FOLDERS_TREEVIEW))
         {
            return (PanelControl)Cache.Get(CACHE_PHOTOS_FOLDERS_TREEVIEW);
         }
         else
         {
            PhotoDAO docs = new PhotoDAO(Workspace);
            List<PhotoFolder> tree = docs.GetFoldersTree(0);

            TreeViewControl treeView = new TreeViewControl(this);
            treeView.Collapsed = true;
            foreach (PhotoFolder folder in tree)
            {
               treeView.ChildItems.Add(ConvertToChildItem(folder));
            }

            HtmlContentControl description = new HtmlContentControl(this, "Las fotografias se encuentran clasificadas por temática. Puede navegar por el árbol hasta encontrar las categorías de su interés.");

            PanelControl panel = new PanelControl(this);
            panel.CaptionIcon = IconControl.ICON_FOLDER_OPEN;
            panel.Caption = "Categorías";
            panel.Content.Add(description);
            panel.Content.Add(treeView);

            Cache.Add(CACHE_PHOTOS_FOLDERS_TREEVIEW, panel, 720, System.Web.Caching.CacheItemPriority.High);

            return panel;
         }
      }

      private TreeViewChildItemControl ConvertToChildItem(PhotoFolder folder)
      {
         TreeViewChildItemControl child = new TreeViewChildItemControl(this);

         child.DomID = "photo-folder-" + folder.ID;
         child.Caption = folder.Name;
         child.Description = folder.Description;
         child.Href = PhotoDAO.GetFolderURL(folder.ID);

         if (folder.Subfolders.Count > 0)
            child.Icon = "glyphicon-chevron-right";
         else
            child.Icon = IconControl.ICON_CAMERA;

         foreach (PhotoFolder childFolder in folder.Subfolders)
         {
            child.ChildItems.Add(ConvertToChildItem(childFolder));
         }

         return child;
      }

      #endregion
   }
}
