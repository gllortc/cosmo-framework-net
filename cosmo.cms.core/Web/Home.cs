using Cosmo.Cms.Model.Content;
using Cosmo.UI.Controls;

namespace Cosmo.Cms.Web
{
   /// <summary>
   /// Descripción breve de Home
   /// </summary>
   public class Home : Cosmo.UI.PageView
   {

      /// <summary>Cache key for highlighted content in home page.</summary>
      public const string CACHE_CONTENT_HIGHLIGHTED = "cosmo.cms.content.highlighted.medialist";

      #region PageView Implementation

      public override void LoadPage()
      {
         ActiveMenuId = "home";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         //------------------------------------------------
         // Barra lateral derecha
         //------------------------------------------------

         RightContent.Add(GetHighlightedContent());

         //------------------------------------------------
         // Contenido de la página
         //------------------------------------------------

         MediaItem mitem = null;
         MediaListControl mlist = new MediaListControl(this);
         mlist.Style = MediaListControl.MediaListStyle.Thumbnail;

         mitem = new MediaItem();
         mitem.Title = "Foros";
         mitem.Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.";
         mitem.Icon = IconControl.ICON_COMMENT;
         mitem.Image = "images/banner_section_001.png";
         mlist.Add(mitem);

         mitem = new MediaItem();
         mitem.Title = "Fotos";
         mitem.Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.";
         mitem.Icon = IconControl.ICON_CAMERA;
         mitem.Image = "images/banner_section_002.png";
         mlist.Add(mitem);

         mitem = new MediaItem();
         mitem.Title = "Compra/Venta";
         mitem.Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.";
         mitem.Icon = IconControl.ICON_GIFT;
         mitem.Image = "images/banner_section_003.png";
         mlist.Add(mitem);

         MainContent.Add(mlist);
      }

      #endregion

      #region Private Members

      private MediaListControl GetHighlightedContent()
      {
         MediaItem item = null;

         if (Cache.Exist(CACHE_CONTENT_HIGHLIGHTED))
         {
            return (MediaListControl)Cache.Get(CACHE_CONTENT_HIGHLIGHTED);
         }
         else
         {
            MediaListControl list = new MediaListControl(this);
            list.Style = MediaListControl.MediaListStyle.Media;
            list.UseItemSeparator = true;

            DocumentDAO docs = new DocumentDAO(Workspace);
            foreach (Document doc in docs.GetHighlighted(0))
            {
               item = new MediaItem();
               item.Title = doc.Title;
               item.Description = doc.Description;
               item.Image = Workspace.FileSystemService.GetFileURL(new DocumentFSID(doc.ID), doc.Thumbnail);
               item.ImageWidth = 70; // TODO: Hacer esta medida dinámica
               item.LinkHref = ContentView.GetURL(doc.ID);

               list.Add(item);
            }

            Cache.Add(CACHE_CONTENT_HIGHLIGHTED, list, 60, System.Web.Caching.CacheItemPriority.High);

            return list;
         }
      }

      #endregion

   }
}