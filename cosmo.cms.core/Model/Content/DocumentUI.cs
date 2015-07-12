using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;

namespace Cosmo.Cms.Model.Content
{
   /// <summary>
   /// Implements a helper class that allows to generate UO controls from content.
   /// </summary>
   public class DocumentUI
   {
      /// <summary>
      /// Convierte una carpeta en un elemento de lista.
      /// </summary>
      /// <param name="folder">Una instancia de <see cref="DocumentFolder"/> que representa la carpeta a convertir.</param>
      /// <returns>Una instancia de <see cref="ListItem"/> que representa la carpeta convertida.</returns>
      public static ListItem ConvertFolderToListItem(View parentViewport, DocumentFolder folder)
      {
         ListItem listItem = new ListItem();
         listItem.Text = folder.Name;
         listItem.Description = string.Empty; // folder.Description;
         listItem.BadgeText = folder.Objects.ToString();
         listItem.Icon = IconControl.ICON_FOLDER_OPEN;
         listItem.Href = Cosmo.Cms.Web.ContentByFolder.GetURL(folder.ID);

         return listItem;
      }

      /// <summary>
      /// Convierte la lista de subcarpetas contenidas en una carpeta.
      /// </summary>
      /// <param name="folder">La carpeta que contiene las subcarpetas.</param>
      /// <param name="showUpOption">Indica si se debe mostrar la opción de subir de nivel en la lista.</param>
      /// <returns>El componente listo para ser representado al cliente.</returns>
      public static ListGroupControl ConvertFoldersToListGroup(View parentViewport, DocumentFolder folder, bool showUpOption)
      {
         ListItem listItem = null;
         ListGroupControl folderList = new ListGroupControl(parentViewport);

         if (folder.ParentID > 0)
         {
            listItem = new ListItem();
            listItem.Text = "..";
            listItem.Icon = IconControl.ICON_ARROW_UP;
            listItem.Href = Cosmo.Cms.Web.ContentByFolder.GetURL(folder.ParentID);

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
      /// Convierte una lista de documentos en una lista de componentes <see cref="MediaItem"/>.
      /// </summary>
      /// <param name="documentList">La lista de instancias <see cref="Document"/> a convertir.</param>
      /// <returns>Una de medios de Bootstrap.</returns>
      public static MediaListControl ConvertToMediaList(View parentViewport, List<Document> documentList)
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
            item.LinkHref = Cosmo.Cms.Web.ContentView.GetURL(document.ID);

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
         return new BreadcrumbItem(folder.Name, Cosmo.Cms.Web.ContentByFolder.GetURL(folder.ID));
      }

      /// <summary>
      /// Convierte una lista de carpetas de documentos que representan una ruta en una instancia de <see cref="BreadcrumbControl"/>.
      /// </summary>
      /// <param name="folders">Lista de carpetas </param>
      /// <returns></returns>
      public static BreadcrumbControl ConvertToBreadcrumb(View parentViewport, List<DocumentFolder> folders)
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
}
