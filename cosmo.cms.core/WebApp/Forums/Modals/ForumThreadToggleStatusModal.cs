using Cosmo.Cms.Forums;
using Cosmo.Cms.REST;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Modals;

namespace Cosmo.Cms.WebApp.Forums.Modals
{
   public class ForumThreadToggleStatusModal : SimpleRestCallModalForm
   {
      private const string FORM_ID = "frmForumThreadToggle";

      #region Constructors

      public ForumThreadToggleStatusModal(ViewContainer parentViewport, int threadId)
         : base(parentViewport, FORM_ID)
      {
         Initialize();

         DomID = FORM_ID;
         ThreadID = threadId;
         Title = "Cerrar/reactivar thread";
         TitleIcon = IconControl.ICON_DELETE;
         RestURL = CmsApi.ServiceUrl;
         Closeable = true;
         Form.UsePanel = false;
         Form.DomID = "frmToggleThread";
         Form.AddFormSetting(Cosmo.Workspace.PARAM_COMMAND, CmsApi.COMMAND_FORUM_THREAD_REMOVE);
         Form.AddFormSetting(ForumsDAO.PARAM_THREAD_ID, threadId);

         // Agrega un mensaje de confirmación
         HtmlContentControl confirm = new HtmlContentControl(Container);
         confirm.AppendParagraph("¿Está seguro/a que desea cerrar o reactivar el thread actual?");
         confirm.AppendParagraph("Si el hilo actual está cerrado debido a la inactividad por un determinado tiempo, esta acción no lo reabrirá.");
         Form.Content.Add(confirm);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del thread que se desea eliminar.
      /// </summary>
      public int ThreadID { get; set; } 

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         ThreadID = 0;
      }

      #endregion
   }
}
