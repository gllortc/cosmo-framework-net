using Cosmo.Cms.Content;
using Cosmo.Cms.Photos;
using Cosmo.Cms.Utils;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.WebApp.UserServices;
using System.Collections.Generic;

namespace Cosmo.WebApp.Photos
{
   [AuthenticationRequired]
   public class PhotosByUser : PageView
   {
      public override void LoadPage()
      {
         int userId;
         PhotoDAO pics = null;

         // Agrega la meta-información de la página
         Title = DocumentDAO.SERVICE_NAME;
         ActiveMenuId = "photos";

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_SHOPPING_CART;
         MainContent.Add(header);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene el ID del usuario para el que se desea obtener el listado
         userId = Parameters.GetInteger(Cosmo.Workspace.PARAM_USER_ID, 0);
         if (userId <= 0)
         {
            userId = Workspace.CurrentUser.User.ID;
         }

         // Obtiene el usuario
         User user = Workspace.AuthenticationService.GetUser(userId);
         if (user == null)
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Uppps! Hemos detectado una llamada incorrecta...";
            callout.Text = "La solicitud recibida contiene datos erróneos y no es posible generar la página.";
            MainContent.Add(callout);
            return;
         }

         Title = "Fotos subidas por " + user.GetDisplayName();

         // Insert a modal to show user data
         UserDataModal userData = new UserDataModal();
         Modals.Add(userData);

         //--------------------------------------------------------------
         // Cabecera
         //--------------------------------------------------------------

         MainContent.Clear();
         header = new PageHeaderControl(this);
         header.Title = "Fotos";
         header.Icon = IconControl.ICON_CAMERA;

         MainContent.Add(header);

         //-------------------------------------------------------------------
         // Lista de imagenes
         //-------------------------------------------------------------------

         // Genera la lista de documentos de la carpeta
         pics = new PhotoDAO(Workspace);

         List<Photo> pictures = pics.GetUserPictures(userId);
         if (pictures.Count > 0)
         {
            PictureControl picture = null;
            PictureGalleryControl picGallery = new PictureGalleryControl(this);
            picGallery.Columns = pics.GalleryColumnsCount;
            foreach (Photo photo in pictures)
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
                  picture.Footer = new UserLinkControl(this, user, userData);
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

            MainContent.Add(picGallery);


            /*MediaList thlist = LayoutAdapter.Pictures.ConvertToMediaList(this, pictures);
            thlist.UseItemSeparator = true;

            Panel panel = new Panel(this);
            panel.Caption = "Fotos subidas por " + user.GetDisplayName();
            panel.CaptionIcon = Icon.ICON_USER;
            panel.Content.Add(thlist);

            MainContent.Add(panel);*/
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

         ButtonControl recentBtn = new ButtonControl(this);
         recentBtn.Icon = IconControl.ICON_CALENDAR;
         recentBtn.Text = "Fotografias recientes";
         recentBtn.IsBlock = true;
         recentBtn.Href = PhotoDAO.GetRecentPhotosURL();

         PanelControl toolsPanel = new PanelControl(this);
         toolsPanel.Content.Add(navBtn);
         toolsPanel.Content.Add(recentBtn);

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
