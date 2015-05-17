using Cosmo.Cms.Content;
using Cosmo.Cms.Photos;
using Cosmo.Cms.Utils;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Collections.Generic;

namespace Cosmo.WebApp.Photos
{
   public class PhotosByFolder : PageViewContainer
   {
      public override void LoadPage()
      {
         PhotoFolder folder = null;
         PhotoDAO phdao = null;

         // Agrega la meta-información de la página
         Title = DocumentDAO.SERVICE_NAME;
         ActiveMenuId = "photos";

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_CAMERA;
         MainContent.Add(header);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene la carpeta a mostrar
         int folderid = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);

         // Obtiene la carpeta solicitada
         phdao = new PhotoDAO(Workspace);
         folder = phdao.GetFolder(folderid, false);

         if (folder == null)
         {
            ShowError("Categoria no encontrada",
                      "La categoria de fotografias solicitada no existe o bien no se encuentra disponible.");
            return;
         }

         Title = folder.Name;

         //--------------------------------------------------------------
         // Cabecera
         //--------------------------------------------------------------

         List<PhotoFolder> folders = phdao.GetFolderRoute(folder.ID);

         MainContent.Clear();
         header = new PageHeaderControl(this);
         header.Title = PhotoDAO.SERVICE_NAME;
         header.SubTitle = folder.Name;
         header.Icon = IconControl.ICON_CAMERA;
         header.Breadcrumb = LayoutAdapter.Pictures.ConvertToBreadcrumb(this, folders);

         MainContent.Add(header);

         //-------------------------------------------------------------------
         // Lista de imagenes
         //-------------------------------------------------------------------

         // Genera la lista de documentos de la carpeta
         List<Photo> pictures = phdao.GetPictures(folderid);

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
               picture.Footer = photo.Author;

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
         navBtn.Caption = "Navegar por carpetas";
         navBtn.IsBlock = true;
         navBtn.Href = PhotoDAO.GetBrowseFoldersURL();

         ButtonControl recentBtn = new ButtonControl(this);
         recentBtn.Icon = IconControl.ICON_CALENDAR;
         recentBtn.Caption = "Fotografias recientes";
         recentBtn.IsBlock = true;
         recentBtn.Href = PhotoDAO.GetRecentPhotosURL();

         ButtonControl myBtn = new ButtonControl(this);
         myBtn.Icon = IconControl.ICON_USER;
         myBtn.Caption = "Mis fotografias";
         myBtn.IsBlock = true;
         myBtn.Href = PhotoDAO.GetUserPhotosURL();

         PanelControl toolsPanel = new PanelControl(this);
         toolsPanel.Caption = "Menú Fotos";
         toolsPanel.Content.Add(navBtn);
         toolsPanel.Content.Add(recentBtn);
         toolsPanel.Content.Add(myBtn);

         RightContent.Add(toolsPanel);

         //-------------------------------------------------------------------
         // Agregar imágenes
         //-------------------------------------------------------------------

         if (folder.CanUpload)
         {
            HtmlContentControl content = new HtmlContentControl(this);
            content.AppendParagraph("Si lo desea, puede agregar imágenes en esta carpeta.");

            ButtonControl addBtn = new ButtonControl(this);
            addBtn.Icon = IconControl.ICON_UPLOAD;
            addBtn.Caption = "Enviar fotografia";
            addBtn.Color = ComponentColorScheme.Primary;
            addBtn.IsBlock = true;
            addBtn.Href = PhotoDAO.GetUploadURL(folderid);

            PanelControl panel = new PanelControl(this);
            panel.Caption = "Enviar imágenes";
            panel.Content.Add(content);
            panel.Content.Add(addBtn);

            RightContent.Add(panel);
         }
      }

      public override void InitPage()
      {
         // Nothing to do
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         throw new NotImplementedException();
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
