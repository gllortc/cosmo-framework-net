using Cosmo.Net;
using System;
using System.Web;

namespace Cosmo.Cms.Utils.Banners
{
   /// <summary>
   /// Summary description for CSBannersHandler
   /// </summary>
   public class BannerHandler : IHttpHandler
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of CSBannersHandler.
      /// </summary>
      public BannerHandler() { }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si se puede reusar.
      /// </summary>
      /// <remarks>
      /// Implementación de IHttpHandler.
      /// </remarks>
      public bool IsReusable
      {
         get { return true; }
      }

      #endregion

      #region Methods

      public void ProcessRequest(System.Web.HttpContext context)
      {
         Workspace ws = new Workspace(context);
         BannerDAO banners = new BannerDAO(ws);

         // Obtiene los parámetros
         int objectid = Url.GetInteger(context.Request.Params, Cosmo.Workspace.PARAM_OBJECT_ID, 0);

         // Comprueba parámetros requeridos
         if (objectid <= 0) context.Response.Redirect(Workspace.COSMO_URL_DEFAULT, true);

         try
         {
            string url = banners.DoClick(objectid);

            // Reescribe la cabecera de la petición para redirigir al cliente a la URL
            context.Response.Status = "301 Moved Permanently";
            context.Response.AddHeader("content-length", "0");
            context.Response.AddHeader("Location", url);
         }
         catch (Exception ex)
         {
            // Cosmo.Workspace.SendError(context, ex, "No se ha podido redireccionar a la URL de destino.");
         }
      }

      #endregion

   }
}