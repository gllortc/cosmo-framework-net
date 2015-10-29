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
         //jumbotron.ButtonText = "Suscribete";
         //jumbotron.ButtonHref = Cosmo.Web.UserJoin.GetURL();

         MainContent.Add(jumbotron);

         PanelControl panel;
         HtmlContentControl content;

         panel = new PanelControl(this);
         panel.Text = "Hola!";
         content = new HtmlContentControl(this);
         content.AppendParagraph(@"El Junio del 2001 nació Railwaymania, fruto de una necesidad de cubrir algunos huecos
                                   que tenia la red respecto a nuestra afición favorita: los trenes!");
         content.AppendParagraph(@"Después de todos estos años la posibilidad de mantener Railwaymania ha ido decreciendo 
                                   en la misma forma que las obligaciones profesionales y sobretodo familiares han ido 
                                   creciendo, llegando al punto de tomar la decisión de parar maquinas en Junio de 2015.");
         content.AppendParagraph(@"Junto a un grupo de aficionados (y amigos) hemos decidido crear un nuevo portal, que será 
                                   mantenido por varias personas, tanto en lo que respecta al foro como en lo que respecta a 
                                   los contenidos. Lo hacemos con ilusión y sobretodo con la única pretensión de fomentar 
                                   nuestra afición.");
         content.AppendParagraph(HtmlContentControl.BoldText(@"Un abrazo y hasta muy pronto!"));
         panel.Content.Add(content);
         MainContent.Add(panel);

         panel = new PanelControl(this);
         panel.Text = "Secciones";
         content = new HtmlContentControl(this);
         content.AppendParagraph(@"Internet ha cambiado mucho desde 2001 y esto nos ha hecho reflexionar sobre las
                                   secciones existentes hasta el momento del cierre.<br/><br/>
                                   Por ello, algunas de las secciones han desaparecido por quedar totalmente
                                   obsoletas. Hemos eliminado las secciones de <em>enlaces</em>, <em>comercios</em>, 
                                   <em>libros</em> o <em>noticias (RSS)</em> entre otras puesto que existen actualmente
                                   servicios que pueden dar la información mucho más actualizada y completa.<br/><br/>
                                   En cambio, en el nuevo portal se mantienen las secciones de <em>contenidos</em> (ahora
                                   <em>Artículos</em>), <em>foros</em>, <em>fotos</em> y <em>clasificados</em> que serán 
                                   el eje central del portal.");
         panel.Content.Add(content);
         MainContent.Add(panel);

         //MediaItem mitem = null;
         //MediaListControl mlist = new MediaListControl(this);
         //mlist.Style = MediaListControl.MediaListStyle.Thumbnail;

         //mitem = new MediaItem();
         //mitem.Title = "Foros";
         //mitem.Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.";
         //mitem.Icon = IconControl.ICON_COMMENT;
         //mitem.Image = "images/banner_section_001.png";
         //mlist.Add(mitem);

         //mitem = new MediaItem();
         //mitem.Title = "Fotos";
         //mitem.Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.";
         //mitem.Icon = IconControl.ICON_CAMERA;
         //mitem.Image = "images/banner_section_002.png";
         //mlist.Add(mitem);

         //mitem = new MediaItem();
         //mitem.Title = "Compra/Venta";
         //mitem.Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.";
         //mitem.Icon = IconControl.ICON_GIFT;
         //mitem.Image = "images/banner_section_003.png";
         //mlist.Add(mitem);

         //MainContent.Add(mlist);
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

         if (Cache.Exist(Cosmo.Cms.Web.Home.CACHE_CONTENT_HIGHLIGHTED))
         {
            return (MediaListControl)Cache.Get(Cosmo.Cms.Web.Home.CACHE_CONTENT_HIGHLIGHTED);
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
               item.LinkHref = Cosmo.Cms.Web.ContentView.GetURL(doc.ID);

               list.Add(item);
            }

            Cache.Add(Cosmo.Cms.Web.Home.CACHE_CONTENT_HIGHLIGHTED, list, 60, System.Web.Caching.CacheItemPriority.High);

            return list;
         }
      }

      #endregion

   }
}