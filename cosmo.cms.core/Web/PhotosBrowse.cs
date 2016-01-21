using Cosmo.Cms.Model.Photos;
using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Cms.Web
{
   public class PhotosBrowse : PageView
   {

      #region Properties

      /// <summary>
      /// Gets a value indicating id the current user can administrate the photo service.
      /// </summary>
      private bool IsPhotoAdmin
      {
         get { return Workspace.CurrentUser.CheckAuthorization(Cosmo.Workspace.ROLE_ADMINISTRATOR, PhotoDAO.ROLE_PHOTOS_EDITOR); }
      }

      #endregion

      #region PageView Implementation

      public override void LoadPage()
      {
         // Agrega la meta-información de la página
         Title = PhotoDAO.SERVICE_NAME;
         ActiveMenuId = "photo-browse";

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = PhotoDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_CAMERA;
         MainContent.Add(header);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

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
         recentBtn.Href = PhotosByFolder.GetPhotosRecentUrl();

         ButtonControl myBtn = new ButtonControl(this);
         myBtn.Icon = IconControl.ICON_USER;
         myBtn.Text = "Mis fotografias";
         myBtn.IsBlock = true;
         myBtn.Href = PhotosByFolder.GetPhotosByUserUrl();

         PanelControl toolsPanel = new PanelControl(this);
         toolsPanel.Content.Add(recentBtn);
         toolsPanel.Content.Add(myBtn);

         RightContent.Add(toolsPanel);

         PanelControl addPanel = new PanelControl(this);
         addPanel.Text = "Enviar imágenes";
         addPanel.ContentXhtml = "<p>Si desea enviar sus imágenes y es susbscriptor puede hacerlo desde la carpeta dónde desea agregarlas. Acceda a la carpeta dónde las desea agregar y pulse el enlace Enviar imagen situado en la barra vertical derecha.</p>";

         RightContent.Add(addPanel);
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Gets the URL to show a tree with photos folders.
      /// </summary>
      /// <returns>A string representing the relative URL requested.</returns>
      public static string GetURL()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         return url.ToString();
      }

      #endregion

      #region Private Members

      private const string CACHE_PHOTOS_FOLDERS_TREEVIEW = "cosmo.cms.photos.folders.treeview";

      private PanelControl GetFoldersTreeView()
      {
         if (Cache.Exist(CACHE_PHOTOS_FOLDERS_TREEVIEW) && !this.IsPhotoAdmin)
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

            if (this.IsPhotoAdmin)
            {
               TreeViewChildItemControl item = new TreeViewChildItemControl(this);
               item.Icon = IconControl.ICON_PLUS;
               item.Href = Cosmo.Cms.Web.PhotosFolderEdit.GetAddURL();
               item.Caption = "New folder...";

               treeView.ChildItems.Add(item);
            }

            HtmlContentControl description = new HtmlContentControl(this, "Las fotografias se encuentran clasificadas por temática. Puede navegar por el árbol hasta encontrar las categorías de su interés.");

            PanelControl panel = new PanelControl(this);
            panel.CaptionIcon = IconControl.ICON_FOLDER_OPEN;
            panel.Text = "Categorías";
            panel.Content.Add(description);
            panel.Content.Add(treeView);

            if (!this.IsPhotoAdmin)
            {
               Cache.Add(CACHE_PHOTOS_FOLDERS_TREEVIEW, panel, 720, System.Web.Caching.CacheItemPriority.High);
            }

            return panel;
         }
      }

      private TreeViewChildItemControl ConvertToChildItem(PhotoFolder folder)
      {
         TreeViewChildItemControl child = new TreeViewChildItemControl(this);

         child.DomID = "photo-folder-" + folder.ID;
         child.Caption = folder.Name;
         child.Description = folder.Description;
         child.Href = PhotosByFolder.GetURL(folder.ID);
         child.Icon = (folder.Subfolders.Count > 0 ? "glyphicon-chevron-right" : IconControl.ICON_CAMERA);

         foreach (PhotoFolder childFolder in folder.Subfolders)
         {
            child.ChildItems.Add(ConvertToChildItem(childFolder));
         }

         if (this.IsPhotoAdmin)
         {
            TreeViewChildItemControl item = new TreeViewChildItemControl(this);
            item.Icon = IconControl.ICON_PLUS;
            item.Href = Cosmo.Cms.Web.PhotosFolderEdit.GetAddURL(folder.ID);
            item.Caption = "New folder...";

            child.ChildItems.Add(item);
         }

         return child;
      }

      #endregion

   }
}
