using Cosmo.Cms.Model.Content;
using Cosmo.Net;
using Cosmo.UI.Controls;
using System;
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
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

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
         jumbotron.Description = "Estamos reformando completamente el portal. Volvemos en septiembre!";
         jumbotron.BackgroundImage = "images/home_bg_003.jpg";
         jumbotron.ForeColor = "#eeeeee";

         MainContent.Add(jumbotron);

         MediaItem mitem = null;
         MediaListControl mlist = new MediaListControl(this);
         mlist.Style = MediaListControl.MediaListStyle.Thumbnail;

         mitem = new MediaItem();
         mitem.Title = "Foros";
         mitem.Description = "Volveremos con el foro. Ahora moderado por un equipo externo de aficionados que garantizará su buen funcionamiento.";
         mitem.Icon = IconControl.ICON_COMMENT;
         mitem.Image = "images/banner_section_001.png";
         mlist.Add(mitem);

         mitem = new MediaItem();
         mitem.Title = "Fotos";
         mitem.Description = "Con la sección de fotos completamente reformada. Ahora mucho más fácil subir fotos y gestionar las fotos de cada autor.";
         mitem.Icon = IconControl.ICON_CAMERA;
         mitem.Image = "images/banner_section_002.png";
         mlist.Add(mitem);

         mitem = new MediaItem();
         mitem.Title = "Compra/Venta";
         mitem.Description = "Un mercadillo más sencillo e intuitivo, con más facilidad para contactar con los autores de los anuncios. Y una gestión de anuncios más potente y simplificada.";
         mitem.Icon = IconControl.ICON_GIFT;
         mitem.Image = "images/banner_section_003.png";
         mlist.Add(mitem);

         MainContent.Add(mlist);
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