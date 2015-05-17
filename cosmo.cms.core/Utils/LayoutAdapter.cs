using Cosmo.Cms.Classified;
using Cosmo.Cms.Content;
using Cosmo.Cms.Forums;
using Cosmo.Cms.Photos;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;

namespace Cosmo.Cms.Utils
{
   /// <summary>
   /// Permite transformar estructuras de gestión del CMS a componentes de Layout (Bootstrap).
   /// </summary>
   public class LayoutAdapter
   {
      /// <summary>
      /// Clase que gestiona las conversiones de documentos y carpetas de documentos.
      /// </summary>
      public class Documents
      {
         /// <summary>
         /// Convierte una carpeta en un elemento de lista.
         /// </summary>
         /// <param name="folder">Una instancia de <see cref="DocumentFolder"/> que representa la carpeta a convertir.</param>
         /// <returns>Una instancia de <see cref="ListItem"/> que representa la carpeta convertida.</returns>
         public static ListItem ConvertFolderToListItem(ViewContainer parentViewport, DocumentFolder folder)
         {
            ListItem listItem = new ListItem();
            listItem.Caption = folder.Name;
            listItem.Description = string.Empty; // folder.Description;
            listItem.BadgeText = folder.Objects.ToString();
            listItem.Icon = IconControl.ICON_FOLDER_OPEN;
            listItem.Href = DocumentDAO.GetDocumentFolderURL(folder.ID);

            return listItem;
         }

         /// <summary>
         /// Convierte una lista de carpetas en una lista.
         /// </summary>
         /// <param name="folders">Lista de instancias de <see cref="DocumentFolder"/> que representan las carpetas a listar.</param>
         /// <returns>El componente listo para ser representado al cliente.</returns>
         public static ListGroupControl ConvertFoldersToListGroup(ViewContainer parentViewport, List<DocumentFolder> folders)
         {
            ListGroupControl folderList = new ListGroupControl(parentViewport);

            foreach (DocumentFolder subfolder in folders)
            {
               folderList.Add(ConvertFolderToListItem(parentViewport, subfolder));
            }

            folderList.Style = ListGroupControl.ListGroupStyle.CustomContent;

            return folderList;
         }

         /// <summary>
         /// Convierte la lista de subcarpetas contenidas en una carpeta.
         /// </summary>
         /// <param name="folder">La carpeta que contiene las subcarpetas.</param>
         /// <param name="showUpOption">Indica si se debe mostrar la opción de subir de nivel en la lista.</param>
         /// <returns>El componente listo para ser representado al cliente.</returns>
         public static ListGroupControl ConvertFoldersToListGroup(ViewContainer parentViewport, DocumentFolder folder, bool showUpOption)
         {
            ListItem listItem = null;
            ListGroupControl folderList = new ListGroupControl(parentViewport);

            if (folder.ParentID > 0)
            {
               listItem = new ListItem();
               listItem.Caption = "..";
               listItem.Icon = IconControl.ICON_ARROW_UP;
               listItem.Href = DocumentDAO.GetDocumentFolderURL(folder.ParentID);

               folderList.Add(listItem);
            }

            foreach (DocumentFolder subfolder in folder.Subfolders)
            {
               folderList.Add(ConvertFolderToListItem(parentViewport, subfolder));
            }

            folderList.Style = ListGroupControl.ListGroupStyle.CustomContent;

            return folderList;
         }

         /// <summary>
         /// Convierte un documento en un componente <see cref="MediaItem"/>.
         /// </summary>
         /// <param name="document">La instancia de <see cref="Document"/> que se desea convertir.</param>
         /// <returns>Una instancia de <see cref="MediaItem"/> que representa el documento original.</returns>
         public static MediaItem ConvertToMediaItem(ViewContainer parentViewport, Document document)
         {
            MediaItem item = new MediaItem();
            item.Title = document.Title;
            item.Description = document.Description;
            item.Image = parentViewport.Workspace.FileSystemService.GetFileURL(document.ID.ToString(), document.Thumbnail);
            item.ImageWidth = 70; // TODO: Hacer esta medida dinámica
            item.LinkHref = DocumentDAO.GetDocumentViewURL(document.ID);
            return item;
         }

         /// <summary>
         /// Convierte una lista de documentos en una lista de componentes <see cref="MediaItem"/>.
         /// </summary>
         /// <param name="documentList">La lista de instancias <see cref="Document"/> a convertir.</param>
         /// <returns>Una de medios de Bootstrap.</returns>
         public static MediaListControl ConvertToMediaList(ViewContainer parentViewport, List<Document> documentList)
         {
            MediaItem item = null;
            MediaListControl list = new MediaListControl(parentViewport);
            list.Style = MediaListControl.MediaListStyle.Media;

            foreach (Document document in documentList)
            {
               item = new MediaItem();
               item.Title = document.Title;
               item.Description = document.Description;
               item.Image = parentViewport.Workspace.FileSystemService.GetFileURL(document.ID.ToString(), document.Thumbnail);
               item.ImageWidth = 70; // TODO: Hacer esta medida dinámica
               item.LinkHref = DocumentDAO.GetDocumentViewURL(document.ID);

               list.Add(item);
            }

            return list;
         }

         /// <summary>
         /// Convierte una carpeta de documentos en un elemento de ruta.
         /// </summary>
         /// <param name="folder"></param>
         /// <returns></returns>
         public static BreadcrumbItem ConvertToBreadcrumbItem(DocumentFolder folder)
         {
            return new BreadcrumbItem(folder.Name, DocumentDAO.GetDocumentFolderURL(folder.ID));
         }

         /// <summary>
         /// Convierte una lista de carpetas de documentos que representan una ruta en una instancia de <see cref="BreadcrumbControl"/>.
         /// </summary>
         /// <param name="folders">Lista de carpetas </param>
         /// <returns></returns>
         public static BreadcrumbControl ConvertToBreadcrumb(ViewContainer parentViewport, List<DocumentFolder> folders)
         {
            BreadcrumbControl breadcrumb = new BreadcrumbControl(parentViewport);

            breadcrumb.Items.Add(new BreadcrumbItem("Inicio", Workspace.COSMO_URL_DEFAULT, "fa-home"));

            foreach (DocumentFolder folder in folders)
            {
               breadcrumb.Items.Add(ConvertToBreadcrumbItem(folder));
            }

            breadcrumb.SetLastActive();

            return breadcrumb;
         }
      }

      /// <summary>
      /// Clase que gestiona las conversiones de documentos y carpetas de documentos.
      /// </summary>
      public class Pictures
      {
         /// <summary>
         /// Convierte un árbol de carpetas del servicio de fotografias a un componente TreeView.
         /// </summary>
         /// <param name="folders">La lista de las carpetas y subcarpetas del servicio.</param>
         /// <returns>Una instancia de <see cref="TreeViewControl"/> lista para ser representada.</returns>
         public static TreeViewControl ConvertToTreeView(ViewContainer parentViewport, List<PhotoFolder> folders)
         {
            TreeViewControl treeView = new TreeViewControl(parentViewport);

            foreach (PhotoFolder folder in folders)
            {
               treeView.ChildItems.Add(ConvertToChildItem(parentViewport, folder));
            }

            return treeView;
         }

         private static TreeViewChildItemControl ConvertToChildItem(ViewContainer parentViewport, PhotoFolder folder)
         {
            TreeViewChildItemControl child = new TreeViewChildItemControl(parentViewport);

            child.DomID = "photo-folder-" + folder.ID;
            child.Caption = folder.Name;
            child.Description = folder.Description;
            child.Href = PhotoDAO.GetFolderURL(folder.ID);

            if (folder.Subfolders.Count > 0)
               child.Icon = "glyphicon-chevron-right";
            else
               child.Icon = "fa-camera";

            foreach (PhotoFolder childFolder in folder.Subfolders)
            {
               child.ChildItems.Add(ConvertToChildItem(parentViewport, childFolder));
            }

            return child;
         }

         /// <summary>
         /// Convierte una carpeta en un elemento de lista.
         /// </summary>
         /// <param name="folder">Una instancia de <see cref="DocumentFolder"/> que representa la carpeta a convertir.</param>
         /// <returns>Una instancia de <see cref="ListItem"/> que representa la carpeta convertida.</returns>
         public static ListItem ConvertFolderToListItem(PhotoFolder folder)
         {
            ListItem listItem = new ListItem();
            listItem.Caption = folder.Name;
            listItem.Description = folder.Description;
            listItem.BadgeText = folder.Objects.ToString();
            listItem.Icon = IconControl.ICON_FOLDER_OPEN;
            listItem.Href = PhotoDAO.GetFolderURL(folder.ID);

            return listItem;
         }

         /// <summary>
         /// Convierte una lista de carpetas en una lista.
         /// </summary>
         /// <param name="folders">Lista de instancias de <see cref="DocumentFolder"/> que representan las carpetas a listar.</param>
         /// <returns>El componente listo para ser representado al cliente.</returns>
         public static ListGroupControl ConvertFoldersToListGroup(ViewContainer parentViewport, List<PhotoFolder> folders)
         {
            ListGroupControl folderList = new ListGroupControl(parentViewport);

            foreach (PhotoFolder subfolder in folders)
            {
               folderList.Add(ConvertFolderToListItem(subfolder));
            }

            folderList.Style = ListGroupControl.ListGroupStyle.CustomContent;

            return folderList;
         }

         /// <summary>
         /// Convierte la lista de subcarpetas contenidas en una carpeta.
         /// </summary>
         /// <param name="folder">La carpeta que contiene las subcarpetas.</param>
         /// <param name="showUpOption">Indica si se debe mostrar la opción de subir de nivel en la lista.</param>
         /// <returns>El componente listo para ser representado al cliente.</returns>
         public static ListGroupControl ConvertFoldersToListGroup(ViewContainer parentViewport, PhotoFolder folder, bool showUpOption)
         {
            ListItem listItem = null;
            ListGroupControl folderList = new ListGroupControl(parentViewport);

            if (folder.ParentID > 0)
            {
               listItem = new ListItem();
               listItem.Caption = "..";
               listItem.Icon = IconControl.ICON_ARROW_UP;
               listItem.Href = PhotoDAO.GetFolderURL(folder.ParentID);

               folderList.Add(listItem);
            }

            foreach (PhotoFolder subfolder in folder.Subfolders)
            {
               folderList.Add(ConvertFolderToListItem(subfolder));
            }

            folderList.Style = ListGroupControl.ListGroupStyle.CustomContent;

            return folderList;
         }

         /// <summary>
         /// Convierte un documento en un componente <see cref="MediaItem"/>.
         /// </summary>
         /// <param name="photo">La instancia de <see cref="PictureControl"/> que se desea convertir.</param>
         /// <returns>Una instancia de <see cref="MediaItem"/> que representa el documento original.</returns>
         public static MediaItem ConvertToMediaItem(Photo photo)
         {
            MediaItem item = new MediaItem();
            item.Title = photo.Author;
            item.Description = photo.Description;
            item.Image = photo.ThumbnailFile;
            item.ImageWidth = photo.ThumbnailWidth;
            item.ImageHeight = photo.ThumbnailHeight;
            // item.ImageWidth = 70; // TODO: Hacer esta medida dinámica
            return item;
         }

         /// <summary>
         /// Convierte una lista de documentos en una lista de componentes <see cref="MediaItem"/>.
         /// </summary>
         /// <param name="documentList">La lista de instancias <see cref="Document"/> a convertir.</param>
         /// <returns>Una de medios de Bootstrap.</returns>
         public static MediaListControl ConvertToMediaList(ViewContainer parentViewport, List<Photo> documentList)
         {
            MediaListControl list = new MediaListControl(parentViewport);
            list.Style = MediaListControl.MediaListStyle.Media;

            foreach (Photo document in documentList)
            {
               list.Add(LayoutAdapter.Pictures.ConvertToMediaItem(document));
            }

            return list;
         }

         /// <summary>
         /// Convierte una carpeta de documentos en un elemento de ruta.
         /// </summary>
         /// <param name="folder"></param>
         /// <returns></returns>
         public static BreadcrumbItem ConvertToBreadcrumbItem(PhotoFolder folder)
         {
            return new BreadcrumbItem(folder.Name, PhotoDAO.GetFolderURL(folder.ID));
         }

         /// <summary>
         /// Convierte una lista de carpetas de documentos que representan una ruta en una instancia de <see cref="BreadcrumbControl"/>.
         /// </summary>
         /// <param name="folders">Lista de carpetas </param>
         /// <returns></returns>
         public static BreadcrumbControl ConvertToBreadcrumb(ViewContainer parentViewport, List<PhotoFolder> folders)
         {
            BreadcrumbControl breadcrumb = new BreadcrumbControl(parentViewport);

            breadcrumb.Items.Add(new BreadcrumbItem(PhotoDAO.SERVICE_NAME, "PhotosBrowse", IconControl.ICON_CAMERA));

            foreach (PhotoFolder folder in folders)
            {
               breadcrumb.Items.Add(ConvertToBreadcrumbItem(folder));
            }

            breadcrumb.SetLastActive();

            return breadcrumb;
         }
      }

      /// <summary>
      /// Clase que gestiona las conversiones de anuncios clasificados.
      /// </summary>
      public class ClassifiedAds
      {
         /// <summary>
         /// Convierte una categoria de anuncios clasificados en un elemento de lista.
         /// </summary>
         /// <param name="folder">Una instancia de <see cref="ClassifiedAdsSection"/> que representa la carpeta a convertir.</param>
         /// <returns>Una instancia de <see cref="ListItem"/> que representa la carpeta convertida.</returns>
         public static ListItem ConvertSectionToListItem(ClassifiedAdsSection folder)
         {
            ListItem listItem = new ListItem();
            listItem.Caption = folder.Name;
            // listItem.Description = folder.Description;
            listItem.BadgeText = folder.Objects.ToString();
            listItem.Icon = IconControl.ICON_FOLDER_OPEN;
            listItem.Href = ClassifiedAdsDAO.GetClassifiedAdsFolderURL(folder.ID);

            return listItem;
         }

         /// <summary>
         /// Convierte una lista de secciones de anuncios classificados en un componente ListGroup.
         /// </summary>
         /// <param name="sections"></param>
         /// <returns></returns>
         public static ListGroupControl ConvertSectionsToListGroup(ViewContainer parentViewport, List<ClassifiedAdsSection> sections, int activeFolderId)
         {
            ListItem item;
            ListGroupControl folderList = new ListGroupControl(parentViewport);

            foreach (ClassifiedAdsSection folder in sections)
            {
               item = ConvertSectionToListItem(folder);
               item.IsActive = (activeFolderId >= 0 && folder.ID == activeFolderId);

               folderList.Add(item); 
            }

            folderList.Style = ListGroupControl.ListGroupStyle.CustomContent;

            return folderList;
         }


         /// <summary>
         /// Convierte una lista de secciones de anuncios classificados en un componente ListGroup.
         /// </summary>
         /// <param name="sections"></param>
         /// <returns></returns>
         public static ListGroupControl ConvertSectionsToListGroup(ViewContainer parentViewport, List<ClassifiedAdsSection> sections)
         {
            return ConvertSectionsToListGroup(parentViewport, sections, -1);
         }
      }

      /// <summary>
      /// Clase que gestiona los foros.
      /// </summary>
      public class Forums
      {
         /// <summary>
         /// Convierte una categoria de anuncios clasificados en un elemento de lista.
         /// </summary>
         /// <param name="folder">Una instancia de <see cref="ClassifiedAdsSection"/> que representa la carpeta a convertir.</param>
         /// <returns>Una instancia de <see cref="ListItem"/> que representa la carpeta convertida.</returns>
         public static ListItem ConvertSectionToListItem(ForumChannel folder)
         {
            ListItem listItem = new ListItem();
            listItem.Caption = folder.Name;
            // listItem.Description = folder.Description;
            listItem.BadgeText = folder.MessageCount.ToString();
            listItem.Icon = IconControl.ICON_FOLDER_OPEN;
            // listItem.Href = ForumsDAO.URL_CHANNEL + "?" + Workspace.PARAM_FOLDER_ID + "=" + folder.ID;

            return listItem;
         }

         /// <summary>
         /// Convierte una lista de secciones de anuncios classificados en un componente ListGroup.
         /// </summary>
         /// <param name="sections"></param>
         /// <returns></returns>
         public static ListGroupControl ConvertSectionsToListGroup(ViewContainer parentViewport, List<ForumChannel> sections, int activeFolderId)
         {
            ListItem item;
            ListGroupControl folderList = new ListGroupControl(parentViewport);

            foreach (ForumChannel folder in sections)
            {
               item = ConvertSectionToListItem(folder);
               item.IsActive = (activeFolderId >= 0 && folder.ID == activeFolderId);

               folderList.Add(item);
            }

            folderList.Style = ListGroupControl.ListGroupStyle.CustomContent;

            return folderList;
         }


         /// <summary>
         /// Convierte una lista de secciones de anuncios classificados en un componente ListGroup.
         /// </summary>
         /// <param name="sections"></param>
         /// <returns></returns>
         public static ListGroupControl ConvertSectionsToListGroup(ViewContainer parentViewport, List<ForumChannel> sections)
         {
            return ConvertSectionsToListGroup(parentViewport, sections, -1);
         }
      }

   }
}
