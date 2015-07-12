using Cosmo.Cms.Web;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;

namespace Cosmo.Cms.Model.Photos
{
   public class PhotoUI
   {
      /// <summary>
      /// Convierte una carpeta de documentos en un elemento de ruta.
      /// </summary>
      /// <param name="folder"></param>
      /// <returns></returns>
      public static BreadcrumbItem ConvertToBreadcrumbItem(PhotoFolder folder)
      {
         return new BreadcrumbItem(folder.Name, PhotosByFolder.GetURL(folder.ID));
      }

      /// <summary>
      /// Convierte una lista de carpetas de documentos que representan una ruta en una instancia de <see cref="BreadcrumbControl"/>.
      /// </summary>
      /// <param name="folders">Lista de carpetas </param>
      /// <returns></returns>
      public static BreadcrumbControl ConvertToBreadcrumb(View parentViewport, List<PhotoFolder> folders)
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
}
