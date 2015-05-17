using Cosmo.Net;
using Cosmo.Net.Rss;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Cosmo.Cms.Utils.Rss
{

   /// <summary>
   /// Implementa un handler para servir contenido RSS.
   /// </summary>
   public class CSRssHandler : IHttpHandler, IReadOnlySessionState
   {
      /*public enum NativeObjects : int
      {
         Documents = 1,
         Pictures = 2,
         Books = 3,
         Forums = 4,
         Ads = 5
      }*/

      /// <summary>URL para realizar la llamada del servisdio.</summary>
      public const string URL_SERVICE = "rss.do";

      /// <summary>
      /// Devuelve una instancia de CSLinksHandler.
      /// </summary>
      public CSRssHandler() { }

      #region Settings

      public bool IsReusable
      {
         get { return true; }
      }

      #endregion

      #region Methods

      public void ProcessRequest(System.Web.HttpContext context)
      {
         // Inicializaciones
         Workspace ws = new Workspace(context);

         // Establece el tipo de contenido y su codificación
         context.Response.ContentType = "text/xml";
         context.Response.ContentEncoding = Encoding.UTF8;

         // Obtiene los parámetros adicionales
         bool update = Url.GetBoolean(context.Request.Params, RssChannel.UrlParamForceUpdate, false);

         // Envia el contenido al cliente
         /*switch (GetObjectType(context.Request.QueryString[RssChannel.UrlParamChannel]))
         {
            case CSWebsite.NativeObjects.Ads:
               Cosmo.Cms.Ads.CSAds ads = new Cosmo.Cms.Ads.CSAds(ws);
               context.Result.Write(ads.ToRSS(context.Cache, update));
               break;

            case CSWebsite.NativeObjects.Pictures:
               Cosmo.Cms.Pictures.CSPictures pictures = new Cosmo.Cms.Pictures.CSPictures(ws);
               context.Result.Write(pictures.ToRss(context.Cache, update));
               break;

            case CSWebsite.NativeObjects.Books:
               Cosmo.Cms.Books.CSBooks books = new Cosmo.Cms.Books.CSBooks(ws);
               context.Result.Write(books.ToRss(context.Cache, update));
               break;

            case CSWebsite.NativeObjects.Forums:
               Cosmo.Cms.Forums.CSForums forum = new Cosmo.Cms.Forums.CSForums(ws);
               context.Result.Write(forum.ToRss(context.Cache, update));
               break;

            default:
               Cosmo.Cms.Documents.DocumentDAO docs = new Cosmo.Cms.Documents.DocumentDAO(ws);
               context.Result.Write(docs.ToRss(context.Cache, update));
               break;
         }*/
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Obtiene el tipo de objeto
      /// </summary>
      /*private CSWebsite.NativeObjects GetObjectType(string parameter)
      {
         int type = 0;

         if (!int.TryParse(parameter, out type)) type = 0;
         switch (type)
         {
            case (int)CSWebsite.NativeObjects.Ads: return CSWebsite.NativeObjects.Ads;
            case (int)CSWebsite.NativeObjects.Books: return CSWebsite.NativeObjects.Books;
            case (int)CSWebsite.NativeObjects.Forums: return CSWebsite.NativeObjects.Forums;
            case (int)CSWebsite.NativeObjects.Pictures: return CSWebsite.NativeObjects.Pictures;
            default: return CSWebsite.NativeObjects.Documents;
         }
      }*/

      #endregion

   }
}