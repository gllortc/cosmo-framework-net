using Cosmo.Cms.Content;
using Cosmo.Cms.Photos;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.WebApp.UserServices;
using System.Collections.Generic;

namespace Cosmo.WebApp.Photos
{
   public class PhotosRecent : PageViewContainer
   {
      public override void LoadPage()
      {
         User author = null;
         PhotoDAO phdao = null;

         // Agrega la meta-información de la página
         Title = "Fotos más recientes";
         ActiveMenuId = "photos";

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_CAMERA;
         MainContent.Add(header);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene la carpeta solicitada
         phdao = new PhotoDAO(Workspace);

         // Insert a modal to show user data
         UserDataModal userData = new UserDataModal();
         Modals.Add(userData);

         //--------------------------------------------------------------
         // Cabecera
         //--------------------------------------------------------------

         MainContent.Clear();
         header = new PageHeaderControl(this);
         header.Title = PhotoDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_CAMERA;

         MainContent.Add(header);

         //-------------------------------------------------------------------
         // Lista de fotografias recientes
         //-------------------------------------------------------------------

         // Genera la lista de documentos de la carpeta
         List<Photo> documents = phdao.GetLatestPictures(20);

         if (documents.Count > 0)
         {
            PanelControl panel = new PanelControl(this);
            panel.Caption = Title;
            panel.CaptionIcon = IconControl.ICON_CALENDAR;

            PictureControl picture = null;
            PictureGalleryControl picGallery = new PictureGalleryControl(this);
            picGallery.Columns = phdao.GalleryColumnsCount;
            foreach (Photo photo in documents)
            {
               picture = new PictureControl(this);
               picture.DomID = "photo-" + photo.ID;
               picture.ImageUrl = photo.ThumbnailFile;
               picture.Text = photo.Description;

               if (!string.IsNullOrWhiteSpace(photo.Author))
               {
                  picture.Footer = photo.Author;
               }
               else if (photo.UserID > 0)
               {
                  author = Workspace.AuthenticationService.GetUser(photo.UserID);
                  picture.Footer = new UserLinkControl(this, author, userData);
               }

               if (IsAuthenticated && (Workspace.CurrentUser.User.ID == photo.UserID ||
                   Workspace.CurrentUser.CheckAuthorization(PhotoDAO.ROLE_PHOTOS_EDITOR)))
               {
                  picture.SplitButton.Icon = IconControl.ICON_WRENCH;
                  picture.SplitButton.Text = "Herramientas";
                  picture.SplitButton.MenuOptions.Add(new ButtonControl(this, "btn-pic-" + photo.ID + "-edit", "Editar", IconControl.ICON_EDIT, "#", string.Empty));
                  picture.SplitButton.MenuOptions.Add(new ButtonControl(this, "btn-pic-" + photo.ID + "-del", "Eliminar", IconControl.ICON_DELETE, "#", string.Empty));
               }

               picGallery.Pictures.Add(picture);
            }
            panel.Content.Add(picGallery);

            MainContent.Add(panel);
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
         // Opciones y herramientas
         //-------------------------------------------------------------------

         ButtonControl navBtn = new ButtonControl(this);
         navBtn.Icon = IconControl.ICON_SITEMAP;
         navBtn.Text = "Navegar por carpetas";
         navBtn.IsBlock = true;
         navBtn.Href = PhotoDAO.GetBrowseFoldersURL();

         ButtonControl myBtn = new ButtonControl(this);
         myBtn.Icon = IconControl.ICON_USER;
         myBtn.Text = "Mis fotografias";
         myBtn.IsBlock = true;
         myBtn.Href = PhotoDAO.GetUserPhotosURL();

         PanelControl toolsPanel = new PanelControl(this);
         toolsPanel.Content.Add(navBtn);
         toolsPanel.Content.Add(myBtn);

         RightContent.Add(toolsPanel);
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
   }
}
