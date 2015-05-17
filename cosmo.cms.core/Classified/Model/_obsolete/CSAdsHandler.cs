using Cosmo.Cms.Layout;
using Cosmo.Net;
using Cosmo.Security.Auth;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace Cosmo.Cms.Classified.Model
{
   /// <summary>
   /// Implementa un handler para la gestión de las acciones sobre usuarios
   /// </summary>
   /// <remarks>
   /// Para usar cualquier método de este Handler debe existir sesión de usuario inicuiada.
   /// </remarks>
   public class CSAdsHandler : IHttpHandler, IReadOnlySessionState
   {
      /// <summary>
      /// Devuelve una instancia de CSUserHandler.
      /// </summary>
      public CSAdsHandler()
      {
         // Nothing to do
      }

      #region Properties

      public bool IsReusable
      {
         get { return true; }
      }

      #endregion

      #region Event Handler

      public void ProcessRequest(System.Web.HttpContext context)
      {
         // Evalua la acción a realizar
         switch (Url.GetString(context.Request.Params, Workspace.PARAM_COMMAND, string.Empty))
         {
            default:
               UserAds(context);
               break;
         }

      }

      #endregion

      #region Private Members

      /// <summary>
      /// Muestra la ficha informativa de un usuario.
      /// </summary>
      /// <param name="context">Contexto de la llamada.</param>
      private void UserAds(System.Web.HttpContext context)
      {
         string xhtml = string.Empty;

         try
         {
            if (AuthenticationService.IsAuthenticated(context.Session))
            {
               // Obtiene la lista de anuncios de un usuario
               CSAds ads = new CSAds(new Workspace(context));
               List<CSAd> userads = ads.Items(true, Url.GetInteger(context.Request.Params, Workspace.PARAM_USER_ID, 0));

               // Genera los datos en formato JSON
               /*JSONArray array = new JSONArray();

               foreach (CSAd ad in userads)
               {
                  // Obtiene la carpeta
                  CSAdsFolder folder = ads.GetFolder(ad.FolderId);

                  JSONObject row = new JSONObject();
                  row.Add(new JSONValue("id", ad.Id));                              // Identificador
                  row.Add(new JSONValue("title", ad.Title));                        // Título 
                  row.Add(new JSONValue("section", folder.Name));                   // Sección
                  row.Add(new JSONValue("date", ad.Created.ToString("dd/MM/yyyy")));   // Fecha

                  array.Add(row);
               }

               // Envia el resultado al cliente
               context.Response.Write(array.Writer());*/

               JavaScriptSerializer jss = new JavaScriptSerializer();
               context.Response.Write(jss.Serialize(userads));
            }
            else
            {
               xhtml += "<p>Debe iniciar una sesión de usuario para usar esta funcionalidad.</p>\n";
               xhtml += "<p><a href=\"" + Workspace.COSMO_URL_LOGIN + "?" + Workspace.PARAM_LOGIN_REDIRECT + "=" + HttpUtility.UrlEncode(context.Request.UrlReferrer.ToString()) + "\">Iniciar sesión</a></p>\n";
               xhtml += CSAjaxMessageBox.SetTitle("Ficha de suscriptor", CSAjaxMessageBox.MessageType.Error);
            }
         }
         catch
         {
            // ERROR
         }
      }

      #endregion

   }
}