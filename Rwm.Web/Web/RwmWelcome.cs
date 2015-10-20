using Cosmo.Net;
using Cosmo.UI.Controls;
using System.Reflection;

namespace Rwm.Web
{
   /// <summary>
   /// Implements the home page for RWM Sample Webapp.
   /// </summary>
   public class RwmWelcome : Cosmo.UI.PageView
   {

      private const string CACHE_CONTENT_HIGHLIGHTED = "rwm.webapp.content.highlighted.medialist";

      #region PageView Implementation

      public override void LoadPage()
      {
         ActiveMenuId = "home";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar-welcome"));

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
         jumbotron.Description = "<strong>Estamos reformando completamente el portal. Volvemos muy pronto!</strong>";
         jumbotron.BackgroundImage = "images/home_bg_003.jpg";
         jumbotron.ForeColor = "#eeeeee";

         MainContent.Add(jumbotron);

         HtmlContentControl html = new HtmlContentControl(this);

         MediaItem mitem = null;
         MediaListControl mlist = new MediaListControl(this);
         mlist.Style = MediaListControl.MediaListStyle.Thumbnail;

         mitem = new MediaItem();
         mitem.Title = "Hola!";
         mitem.Description = @"El Junio del 2001 nació Railwaymania, fruto de una necesidad de cubrir algunos huecos
                               que tenia la red respecto a nuestra afición favorita: los trenes!<br/><br/>
                               Después de todos estos
                               años la posibilidad de mantener Railwaymania ha ido decreciendo en la misma forma que las
                               obligaciones profesionales y sobretodo familiares han ido creciendo, llegando al punto de 
                               tomar la decisión de parar maquinas en Junio de 2015.<br/><br/>
                               Junto a un grupo de aficionados (y amigos)
                               hemos decidido crear un nuevo portal, que será mantenido por varias personas, tanto en
                               lo que respecta al foro como en lo que respecta a los contenidos. Lo hacemos con ilusión
                               y sobretodo con la única pretensión de fomentar nuestra afición.<br /><br />
                               Un abrazo y hasta muy pronto!";
         mitem.Icon = IconControl.ICON_COMMENT;
         mitem.Image = "images/banner_section_001.png";
         mlist.Add(mitem);

         mitem = new MediaItem();
         mitem.Title = "Secciones";
         mitem.Description = @"Internet ha cambiado mucho desde 2001 y esto nos ha hecho reflexionar sobre las
                               secciones existentes hasta el momento del cierre.<br/><br/>
                               Por ello, algunas de las secciones han desaparecido por quedar totalmente
                               obsoletas. Hemos eliminado las secciones de <em>enlaces</em>, <em>comercios</em>, 
                               <em>libros</em> o <em>noticias (RSS)</em> entre otras puesto que existen actualmente
                               servicios que pueden dar la información mucho más actualizada y completa.<br/><br/>
                               En cambio, en el nuevo portal se mantienen las secciones de <em>contenidos</em> (ahora
                               <em>Artículos</em>), <em>foros</em>, <em>fotos</em> y <em>clasificados</em> que serán 
                               el eje central del portal.";
         mitem.Icon = IconControl.ICON_CAMERA;
         mitem.Image = "images/banner_section_002.png";
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

   }
}