using Cosmo.Cms.Forums;
using Cosmo.Cms.REST;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Modals;
using Cosmo.UI.Scripting.Behaviors;

namespace Cosmo.Cms.WebApp.Forums.Modals
{
   public class ForumThreadRemoveModal : SimpleRestCallModalForm
   {
      private const string FORM_ID = "frmForumThreadDelete";

      #region Constructors

      public ForumThreadRemoveModal(ViewContainer parentViewport, int threadId, int channelId)
         : base(parentViewport, FORM_ID)
      {
         Initialize();

         DomID = FORM_ID;
         ThreadID = threadId;
         Title = "Eliminar thread";
         TitleIcon = IconControl.ICON_DELETE;
         RestURL = CmsApi.ServiceUrl;
         Closeable = true;
         Form.UsePanel = false;
         Form.DomID = "frmDeleteThread";
         Form.AddFormSetting(Cosmo.Workspace.PARAM_COMMAND, CmsApi.COMMAND_FORUM_THREAD_REMOVE);
         Form.AddFormSetting(ForumsDAO.PARAM_THREAD_ID, threadId);

         CalloutControl callout = new CalloutControl(Container);
         callout.Type = ComponentColorScheme.Warning;
         callout.Title = "ATENCIÓN";
         callout.Icon = IconControl.ICON_WARNING;
         callout.Text = "Tenga en cuenta que esta acción no puede deshacerse. Cuando se trata de threads creados por usuarios es más adecuado moverlo a un canal oculto (acción que permite recuperar el thread original).";
         Form.Content.Add(callout);

         // Agrega un mensaje de confirmación
         HtmlContentControl confirm = new HtmlContentControl(Container);
         confirm.AppendParagraph("¿Está completamente seguro/a que desea eliminar el thread actual?");
         Form.Content.Add(confirm);

         // Define el comportamiento que debe adoptar si la ejecución funciona bien
         OnSuccess.Add(new NavigateBehavior(Container, ForumsDAO.GetChannelUrl(channelId)));
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
