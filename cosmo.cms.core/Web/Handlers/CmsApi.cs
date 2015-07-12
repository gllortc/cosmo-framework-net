using Cosmo.Cms.Model.Ads;
using Cosmo.Cms.Model.Forum;
using Cosmo.Net;
using Cosmo.Net.REST;
using Cosmo.UI.Scripting;
using System.Reflection;

namespace Cosmo.Cms.Web.Handlers
{
   /// <summary>
   /// Implementa la API que permite ejecutar determinadas operaciones CMS via REST.
   /// </summary>
   public class CmsApi : RestHandler
   {
      /// <summary>Handler command: Move forum thread to another section.</summary>
      public const string COMMAND_FORUM_THREAD_MOVE = "ftm";
      /// <summary>Handler command: Delete forum thread.</summary>
      public const string COMMAND_FORUM_THREAD_REMOVE = "ftr";
      /// <summary>Handler command: Open/Close forum thread.</summary>
      public const string COMMAND_FORUM_THREAD_TOGGLESTATUS = "ftts";
      /// <summary>Handler command: Republish classified ad.</summary>
      public const string COMMAND_ADS_REPUBLISH = "car";
      /// <summary>Handler command: Delete classified ad.</summary>
      public const string COMMAND_ADS_DELETE = "cad";

      #region IRestHandler Implementation

      /// <summary>
      /// Evento que se invoca al recibir una petición.
      /// </summary>
      /// <param name="command">Comando a ejecutar pasado mediante el parámetro definido mediante <see cref="Cosmo.Workspace.PARAM_COMMAND"/>.</param>
      public override void ServiceRequest(string command)
      {
         switch (command)
         {
            case COMMAND_FORUM_THREAD_MOVE:
               ForumMoveThread();
               break;

            case COMMAND_FORUM_THREAD_REMOVE:
               ForumRemoveThread();
               break;

            case COMMAND_FORUM_THREAD_TOGGLESTATUS:
               break;

            default:
               SendResponse(new AjaxResponse(5000, "No se ha especificado ningún comando. La llamada ha sido ignorada."));
               break;
         }
      }

      #endregion

      #region Command: Forum Thread Move

      /// <summary>
      /// Permite obtener una URL que llama al método REST que mueve un thread.
      /// </summary>
      /// <param name="threadId">Identificador del thread a mover.</param>
      /// <param name="destinationChannelId">Identificador del canal de destino.</param>
      /// <returns>Una cadena que contiene la URL solicitada.</returns>
      public static string GetForumMoveThreadRESTUrl(int threadId, int destinationChannelId)
      {
         Url url = new Url(CmsApi.ServiceUrl);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, CmsApi.COMMAND_FORUM_THREAD_MOVE);
         url.AddParameter(ForumsDAO.PARAM_THREAD_ID, threadId);
         url.AddParameter(ForumsDAO.PARAM_CHANNEL_ID, destinationChannelId);

         return url.ToString(true);
      }

      /// <summary>
      /// Mover un thread a otro canal.
      /// <br /><br />
      /// Parámetros de la llamada:<br />
      /// <ul>
      /// <li><c><strong>Cosmo.Workspace.PARAM_COMMAND</strong></c>: <c>CmsApi.COMMAND_FORUM_THREAD_MOVE</c></li>
      /// <li><c><strong>ForumsDAO.PARAM_THREAD_ID</strong></c>: Identificador del thread a mover.</li>
      /// <li><c><strong>ForumsDAO.PARAM_CHANNEL_ID</strong></c>: Identificador del canal de destino.</li>
      /// </ul>
      /// </summary>
      private void ForumMoveThread()
      {
         // Obtiene los parámetros de la llamada.
         int threadId = Parameters.GetInteger(ForumsDAO.PARAM_THREAD_ID);
         int channelId = Parameters.GetInteger(ForumsDAO.PARAM_CHANNEL_ID);

         try
         {
            // Ejecuta el método para mover el hilo
            ForumsDAO fdao = new ForumsDAO(Workspace);
            fdao.MoveThread(threadId, channelId);

            SendResponse(new AjaxResponse());
         }
         catch
         {
            SendResponse(new AjaxResponse(5001, "Se ha producido un error interno y no ha sido posible mover el hilo solicitado."));
         }
      }

      #endregion

      #region Command: Forum Thread Remove

      /// <summary>
      /// Permite obtener una URL que llama al método REST que elimina un thread.
      /// </summary>
      /// <param name="threadId">Identificador del thread a eliminar.</param>
      /// <returns>Una cadena que contiene la URL solicitada.</returns>
      public static string GetForumRemoveThreadRESTUrl(int threadId)
      {
         Url url = new Url(CmsApi.ServiceUrl);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, CmsApi.COMMAND_FORUM_THREAD_REMOVE);
         url.AddParameter(ForumsDAO.PARAM_THREAD_ID, threadId);

         return url.ToString(true);
      }

      /// <summary>
      /// Eliminar un determinado thread.
      /// <br /><br />
      /// Parámetros de la llamada:<br />
      /// <ul>
      /// <li><c><strong>Cosmo.Workspace.PARAM_COMMAND</strong></c>: <c>CmsApi.COMMAND_FORUM_THREAD_REMOVE</c></li>
      /// <li><c><strong>ForumsDAO.PARAM_THREAD_ID</strong></c>: Identificador del thread a eliminar.</li>
      /// </ul>
      /// </summary>
      private void ForumRemoveThread()
      {
         // Obtiene los parámetros de la llamada.
         int threadId = Parameters.GetInteger(ForumsDAO.PARAM_THREAD_ID);

         try
         {
            // Ejecuta el método para mover el hilo
            ForumsDAO fdao = new ForumsDAO(Workspace);
            fdao.DeleteMessage(threadId, true);

            SendResponse(new AjaxResponse());
         }
         catch
         {
            SendResponse(new AjaxResponse(5002, "Se ha producido un error interno y no ha sido posible eliminar el hilo solicitado."));
         }
      }

      #endregion

      #region Command: Forum Thread Toggle Status

      /// <summary>
      /// Modifica el estado de un determinado thread.
      /// <br /><br />
      /// Parámetros de la llamada:<br />
      /// <ul>
      /// <li><c><strong>Cosmo.Workspace.PARAM_COMMAND</strong></c>: <c>CmsApi.COMMAND_FORUM_THREAD_TOGGLESTATUS</c></li>
      /// <li><c><strong>ForumsDAO.PARAM_THREAD_ID</strong></c>: Identificador del thread.</li>
      /// </ul>
      /// </summary>
      private void ForumToggleStatusThread()
      {
         // Obtiene los parámetros de la llamada.
         int threadId = Parameters.GetInteger(ForumsDAO.PARAM_THREAD_ID);

         try
         {
            // Ejecuta el método para mover el hilo
            ForumsDAO fdao = new ForumsDAO(Workspace);
            fdao.ToggleThreadStatus(threadId);

            SendResponse(new AjaxResponse());
         }
         catch
         {
            SendResponse(new AjaxResponse(5003, "Se ha producido un error interno y no ha sido posible modificar el estado del hilo solicitado."));
         }
      }

      #endregion

      #region Command: Classified Ads Republish Ad

      /// <summary>
      /// Gets the URL to call the REST method.
      /// </summary>
      /// <param name="adId">Ad unique identifier.</param>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetAdsRepublishUrl(int adId)
      {
         Url url = new Url(CmsApi.ServiceUrl);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, CmsApi.COMMAND_ADS_REPUBLISH);
         url.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, adId);

         return url.ToString();
      }

      /// <summary>
      /// Republish a classified ad.
      /// </summary>
      private void AdsRepublish()
      {
         // Obtiene los parámetros de la llamada.
         int adId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);

         try
         {
            // Ejecuta el método para mover el hilo
            AdsDAO adsDao = new AdsDAO(Workspace);
            adsDao.Publish(adId);

            SendResponse(new AjaxResponse());
         }
         catch
         {
            SendResponse(new AjaxResponse(6003, "Se ha producido un error interno y no ha sido posible republicar el anuncio solicitado."));
         }
      }

      #endregion

      #region Command: Classified Ads Delete Ad

      /// <summary>
      /// Gets the URL to call the REST method.
      /// </summary>
      /// <param name="adId">Ad unique identifier.</param>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetAdsDeleteUrl(int adId)
      {
         Url url = new Url(CmsApi.ServiceUrl);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, CmsApi.COMMAND_ADS_DELETE);
         url.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, adId);

         return url.ToString();
      }

      /// <summary>
      /// Delete a classified ad.
      /// </summary>
      private void AdsDelete()
      {
         // Obtiene los parámetros de la llamada.
         int adId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);

         try
         {
            // Ejecuta el método para mover el hilo
            AdsDAO adsDao = new AdsDAO(Workspace);
            adsDao.Delete(adId, true);

            SendResponse(new AjaxResponse());
         }
         catch
         {
            SendResponse(new AjaxResponse(6003, "Se ha producido un error interno y no ha sido posible eliminar el anuncio solicitado."));
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve la URL a la que se debe atacar para realizar operaciones REST sobre el servicio.
      /// </summary>
      public static string ServiceUrl
      {
         get { return MethodBase.GetCurrentMethod().DeclaringType.Name; }
      }

      #endregion

   }
}
