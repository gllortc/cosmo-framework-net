﻿using Cosmo.Cms.Model.Content;
using Cosmo.Cms.Model.Photos;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Cms.Web
{
   public class PhotosByFolder : PageView
   {

      #region PageView Implementation

      public override void LoadPage()
      {
         User author = null;
         PhotoFolder folder = null;
         PhotoDAO phdao = null;
         List<Photo> pictures = null;

         // Agrega la meta-información de la página
         Title = DocumentDAO.SERVICE_NAME;
         ActiveMenuId = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID) != FOLDER_RECENT ? "photo-browse" : "photo-recent";

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_CAMERA;
         MainContent.Add(header);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         //--------------------------------------------------------------
         // Database operations
         //--------------------------------------------------------------

         // Gets the folder identifier
         int folderid = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);

         // Grenerate new persistence manager for Photo service
         phdao = new PhotoDAO(Workspace);

         // Gets the pictures to show in view
         if (folderid == FOLDER_BYUSER)
         {
            if (!IsAuthenticated)
            {
               Redirect(Workspace.SecurityService.GetLoginUrl(Workspace.CurrentUrl));
            }

            pictures = phdao.GetByUserID(Workspace.CurrentUser.User.ID);

            Title = "Mis fotografias";

            header.Title = PhotoDAO.SERVICE_NAME;
            header.SubTitle = Title;
            header.Icon = IconControl.ICON_CAMERA;
         }
         else if (folderid == FOLDER_RECENT)
         {
            pictures = phdao.GetLatest(20);

            Title = "Fotografias recientes";

            header.Title = PhotoDAO.SERVICE_NAME;
            header.SubTitle = Title;
            header.Icon = IconControl.ICON_CAMERA;

            ActiveMenuId = "photo-recent";
         }
         else
         {
            pictures = phdao.GetPictures(folderid);

            // Obtiene la carpeta solicitada
            folder = phdao.GetFolder(folderid, false);
            if (folder == null)
            {
               ShowError("Categoria no encontrada",
                         "La categoria de fotografias solicitada no existe o bien no se encuentra disponible.");
               return;
            }

            Title = folder.Name;

            List<PhotoFolder> folders = phdao.GetFolderRoute(folder.ID);

            header.Title = PhotoDAO.SERVICE_NAME;
            header.SubTitle = folder.Name;
            header.Icon = IconControl.ICON_CAMERA;
            header.Breadcrumb = PhotoUI.ConvertToBreadcrumb(this, folders);
         }

         // Insert a modal to show user data
         Cosmo.Web.UserDataModal userData = new Cosmo.Web.UserDataModal();
         Modals.Add(userData);

         PhotosRemoveModal photoDeleteModal = new PhotosRemoveModal();
         Modals.Add(photoDeleteModal);

         // PhotosEditModal photoEditModal = new PhotosEditModal();
         // Modals.Add(photoEditModal);

         //-------------------------------------------------------------------
         // Lista de imagenes
         //-------------------------------------------------------------------

         if (pictures.Count > 0)
         {
            PictureControl picture = null;
            PictureGalleryControl picGallery = new PictureGalleryControl(this);
            picGallery.Columns = phdao.GalleryColumnsCount;
            foreach (Photo photo in pictures)
            {
               picture = new PictureControl(this);
               picture.DomID = "photo-" + photo.ID;
               picture.ImageUrl = photo.ThumbnailFile;
               picture.Text = photo.Description;
               picture.ImageHref = photo.PictureFile;

               if (!string.IsNullOrWhiteSpace(photo.Author))
               {
                  picture.Footer = photo.Author;
               }
               else if (photo.UserID > 0)
               {
                  author = Workspace.SecurityService.GetUser(photo.UserID);
                  picture.Footer = new UserLinkControl(this, author, userData);
               }

               if (IsAuthenticated && (Workspace.CurrentUser.User.ID == photo.UserID ||
                   Workspace.CurrentUser.CheckAuthorization(PhotoDAO.ROLE_PHOTOS_EDITOR)))
               {
                  photoDeleteModal.PhotoID = photo.ID;
                  // photoEditModal.PhotoID = photo.ID;

                  picture.SplitButton.Icon = IconControl.ICON_WRENCH;
                  picture.SplitButton.Text = "Herramientas";
                  // picture.SplitButton.MenuOptions.Add(new ButtonControl(this, "btn-pic-" + photo.ID + "-edit", "Editar", photoEditModal));
                  picture.SplitButton.MenuOptions.Add(new ButtonControl(this, "btn-pic-" + photo.ID + "-del", "Eliminar", photoDeleteModal));
               }

               picGallery.Pictures.Add(picture);
            }

            MainContent.Add(picGallery);
         }
         else
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Categoría vacía";
            callout.Text = "Esta categoría no contiene fotografias. Navegue por las subcategorías situadas a la derecha de esta página para acceder a los documentos.";
            callout.Type = ComponentColorScheme.Information;

            MainContent.Add(callout);
         }

         //-------------------------------------------------------------------
         // Navegación del servicio
         //-------------------------------------------------------------------

         ButtonControl navBtn = new ButtonControl(this);
         navBtn.Icon = IconControl.ICON_SITEMAP;
         navBtn.Text = "Navegar por carpetas";
         navBtn.IsBlock = true;
         navBtn.Href = PhotosBrowse.GetURL();

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
         toolsPanel.Text = "Menú Fotos";
         toolsPanel.Content.Add(navBtn);
         toolsPanel.Content.Add(recentBtn);
         toolsPanel.Content.Add(myBtn);

         RightContent.Add(toolsPanel);

         //-------------------------------------------------------------------
         // Agregar imágenes
         //-------------------------------------------------------------------

         if (folder != null && folder.IsContainer)
         {
            HtmlContentControl content = new HtmlContentControl(this);
            content.AppendParagraph("Si lo desea, puede agregar imágenes en esta carpeta.");

            ButtonControl addBtn = new ButtonControl(this);
            addBtn.Icon = IconControl.ICON_UPLOAD;
            addBtn.Text = "Enviar fotografia";
            addBtn.Color = ComponentColorScheme.Primary;
            addBtn.IsBlock = true;
            addBtn.Href = PhotosUpload.GetPhotosUploadURL(folderid);

            PanelControl panel = new PanelControl(this);
            panel.Text = "Enviar imágenes";
            panel.Content.Add(content);
            panel.Content.Add(addBtn);

            RightContent.Add(panel);
         }
      }

      #endregion

      #region Static Members

      private const int FOLDER_BYUSER = -100;
      private const int FOLDER_RECENT = -101;

      /// <summary>
      /// Gets the URL to show a photos folder contents.
      /// </summary>
      /// <param name="folderId">Folder unique identifier.</param>
      /// <returns>A string representing the relative URL requested.</returns>
      public static string GetURL(int folderId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, folderId.ToString());

         return url.ToString();
      }

      /// <summary>
      /// Gets the URL to show a current authenticated user photos.
      /// </summary>
      /// <param name="folderId">Folder unique identifier.</param>
      /// <returns>A string representing the relative URL requested.</returns>
      public static string GetPhotosByUserUrl()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, FOLDER_BYUSER.ToString());

         return url.ToString();
      }

      /// <summary>
      /// Gets the URL to show the most recent photos.
      /// </summary>
      /// <param name="folderId">Folder unique identifier.</param>
      /// <returns>A string representing the relative URL requested.</returns>
      public static string GetPhotosRecentUrl()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, FOLDER_RECENT.ToString());

         return url.ToString();
      }

      #endregion

   }
}
