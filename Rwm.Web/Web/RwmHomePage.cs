using Cosmo.Cms.Model.Content;
using Cosmo.Net;
using Cosmo.UI.Controls;
using System.Reflection;

namespace Rwm.Web
{
   /// <summary>
   /// Implements the home page for RWM Sample Webapp.
   /// </summary>
   public class RwmHomePage : Cosmo.UI.PageView
   {

      private const string CACHE_CONTENT_HIGHLIGHTED = "rwm.webapp.content.highlighted.medialist";

      #region PageView Implementation

      public override void LoadPage()
      {
         ActiveMenuId = "home";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         //------------------------------------------------
         // Barra lateral derecha
         //------------------------------------------------

         RightContent.Add(GetHighlightedContent());

         //------------------------------------------------
         // Contenido de la página
         //------------------------------------------------

         if (Workspace.Settings.GetBoolean(CookiesAdvisorControl.SETTINGS_ENABLED))
         {
            CookiesAdvisorControl cookies = new CookiesAdvisorControl(this, "cookies-advisor");
            cookies.InformationHref = RwmPrivacy.GetURL();
            MainContent.Add(cookies);
         }

         JumbotronControl jumbotron = new JumbotronControl(this);
         jumbotron.Title = "Railwaymania.com";
         jumbotron.Description = "El portal del ferrocarril europeo en español.";
         jumbotron.BackgroundImage = "images/home_bg_005.jpg";
         jumbotron.ForeColor = "#eeeeee";
         jumbotron.ButtonText = "Suscribete";
         jumbotron.ButtonHref = Cosmo.Web.UserJoin.GetURL();

         MainContent.Add(jumbotron);

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

      #region Static Members

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         return url.ToString();
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
               item.Image = Workspace.FileSystemService.GetFileURL(doc.ID.ToString(), doc.Thumbnail);
               item.ImageWidth = 70; // TODO: Hacer esta medida dinámica
               item.LinkHref = Cosmo.Cms.Web.ContentView.GetURL(doc.ID);

               list.Add(item);
            }

            Cache.Add(CACHE_CONTENT_HIGHLIGHTED, list, 60, System.Web.Caching.CacheItemPriority.High);

            return list;
         }
      }

      #endregion

   }
}