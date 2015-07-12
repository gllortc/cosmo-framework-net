using Cosmo;
using Cosmo.Net;
using System.Web;

namespace Rwn.Web.Handlers
{

   /// <summary>
   /// Implements a handler that translate old ASPX calls to valid Cosmo URLs.
   /// </summary>
   /// <remarks>
   /// Every URL transform is sended to client (specially serach engines) with 301 error (moved permanently).
   /// </remarks>
   public class AspxAdapterHandler : IHttpHandler
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="AspxAdapterHandler"/>.
      /// </summary>
      public AspxAdapterHandler() { }

      #endregion

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
      /// Proccess client request with old ASP/ASPX URLs and redirect to the correct URL.
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
            // Content
            //-----------------------------------------------------

            case "cs_docs_folder.aspx":

               url = Cosmo.Cms.Web.ContentByFolder.GetURL(qsAsp.GetInteger("fid"));
               break;

            case "cs_docs_viewer_std.aspx":
            case "cs_docs_viewer_reg.aspx":

               url = Cosmo.Cms.Web.ContentView.GetURL(qsAsp.GetInteger("id"));
               break;

            //-----------------------------------------------------
            // Forums
            //-----------------------------------------------------

            case "cs_forum_ch.aspx":

               url = Cosmo.Cms.Web.ForumFolder.GetURL(qsAsp.GetInteger("ch"));
               break;

            case "cs_forum_th.aspx":

               url = Cosmo.Cms.Web.ForumThreadView.GetURL(qsAsp.GetInteger("th"),
                                                          qsAsp.GetInteger("ch"),
                                                          qsAsp.GetInteger("p"));
               break;

            //-----------------------------------------------------
            // Photos
            //-----------------------------------------------------

            case "cs_img.aspx":

               url = Cosmo.Cms.Web.PhotosBrowse.GetURL();
               break;

            case "cs_img_folder.aspx":

               url = Cosmo.Cms.Web.PhotosByFolder.GetURL(qsAsp.GetInteger("fid"));
               break;

            //-----------------------------------------------------
            // Classified Ads
            //-----------------------------------------------------

            case "cs_ads_folder.aspx":

               url = Cosmo.Cms.Web.AdsByFolder.GetURL(qsAsp.GetInteger("fid"));
               break;

            case "cs_ads_viewer_std.aspx":

               url = Cosmo.Cms.Web.AdsView.GetURL(qsAsp.GetInteger("id"));
               break;

            //-----------------------------------------------------
            // Books
            //-----------------------------------------------------

            // Disabled section in this version of Cosmo.

            //-----------------------------------------------------
            // Links
            //-----------------------------------------------------

            // Disabled section in this version of Cosmo.

            //-----------------------------------------------------
            // Contacts / Addresses
            //-----------------------------------------------------

            // Disabled section in this version of Cosmo.

            //-----------------------------------------------------
            // RSS News
            //-----------------------------------------------------

            // Disabled section in this version of Cosmo.

            //-----------------------------------------------------
            // Other ASP/ASPX calls
            //-----------------------------------------------------

            default:

               url = Url.Combine(url, Workspace.COSMO_URL_DEFAULT);
               break;
         }

         // Redirige a la URL correcta indicando al navegador (y buscadores) que esta página ha sido 
         // trasladada de forma permanente a otra URL
         context.Response.Status = "301 Moved Permanently";
         context.Response.AddHeader("content-length", "0");
         context.Response.AddHeader("Location", url);
      }

      #endregion

   }
}