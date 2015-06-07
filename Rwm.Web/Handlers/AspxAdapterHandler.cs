using Cosmo;
using Cosmo.Net;
using System.Web;

namespace Rwn.WebApp.Handlers
{

   /// <summary>
   /// Implementa un handler que permite redirigir las peticiones a sites ContentServer de versiones 
   /// anteriores (con tecnología ASPX) a las páginas correctas.
   /// </summary>
   public class AspxAdapterHandler : IHttpHandler
   {
      /// <summary>
      /// Return an instance of <see cref="AspxAdapterHandler"/>.
      /// </summary>
      public AspxAdapterHandler() { }

      #region Properties

      /// <summary>
      /// Indica si este objeto se puede reusar.
      /// </summary>
      public bool IsReusable
      {
         get { return false; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Procesa la petición de una página ASP en el workspace actual.
      /// </summary>
      public void ProcessRequest(System.Web.HttpContext context)
      {
         Workspace ws = new Workspace(context); 
         string url = ws.Url;

         // Obtiene los parámetros de la llamada para usarlos en la formación de la nueva URL
         Url qsAsp = new Url(context.Request);

         // Transforma la URL correspondiente al script ASP a la llamada correcta al script ASPX
         switch (qsAsp.Filename)
         {

            //-----------------------------------------------------
            // Documentos
            //-----------------------------------------------------

            case "cs_docs_folder.aspx":

               url = Cosmo.Cms.Content.DocumentDAO.GetDocumentFolderURL(qsAsp.GetInteger("fid"));
               break;

            case "cs_docs_viewer_std.aspx":
            case "cs_docs_viewer_reg.aspx":

               url = Cosmo.Cms.Content.DocumentDAO.GetDocumentViewURL(qsAsp.GetInteger("id"));
               break;

            //-----------------------------------------------------
            // Foro de discusión
            //-----------------------------------------------------

            case "cs_forum_ch.aspx":

               url = Cosmo.Cms.Forums.ForumsDAO.GetChannelUrl(qsAsp.GetInteger("ch"));
               break;

            case "cs_forum_th.aspx":

               url = Cosmo.Cms.Forums.ForumsDAO.GetThreadUrl(qsAsp.GetInteger("th"),
                                                             qsAsp.GetInteger("ch"),
                                                             qsAsp.GetInteger("p"));
               break;

            //-----------------------------------------------------
            // Galerías de imágenes
            //-----------------------------------------------------

            case "cs_img.aspx":

               url = Cosmo.WebApp.Photos.PhotosBrowse.GetPhotosBrowseUrl();
               break;

            case "cs_img_folder.aspx":

               url = Cosmo.WebApp.Photos.PhotosByFolder.GetPhotosByFolderUrl(qsAsp.GetInteger("fid"));
               break;

            //-----------------------------------------------------
            // Tablón de anuncios
            //-----------------------------------------------------

            case "cs_ads_folder.aspx":

               url = Cosmo.Cms.Classified.ClassifiedAdsDAO.GetClassifiedAdsFolderURL(qsAsp.GetInteger("fid"));
               break;

            case "cs_ads_viewer_std.aspx":

               url = Cosmo.Cms.Classified.ClassifiedAdsDAO.GetClassifiedAdsViewURL(qsAsp.GetInteger("id"));
               break;

            //-----------------------------------------------------
            // Biblioteca
            //-----------------------------------------------------

            // Sección deshabilitada en esta versión

            //-----------------------------------------------------
            // Enlaces
            //-----------------------------------------------------

            // Sección deshabilitada en esta versión

            //-----------------------------------------------------
            // Contactos
            //-----------------------------------------------------

            // Sección deshabilitada en esta versión

            //-----------------------------------------------------
            // Noticias RSS
            //-----------------------------------------------------

            // Sección deshabilitada en esta versión

            //-----------------------------------------------------
            // Cualquier otra llamada a scripts ASP
            //-----------------------------------------------------

            default:

               url = Url.Combine(url, Workspace.COSMO_URL_DEFAULT);
               break;
         }

         // Redirige a la URL correcta indicando al navegador (y buscadores) que esta página ha sido 
         // trasladada de forma permanente a otra URL
         context.Response.Status = "301 Moved Permanently";
         context.Response.AddHeader("Location", url);
      }

      #endregion

   }
}