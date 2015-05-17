using Cosmo.Cms.Forums;
using Cosmo.Cms.REST;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Modals;
using Cosmo.UI.Scripting.Behaviors;

namespace Cosmo.Cms.WebApp.Forums.Modals
{
   /// <summary>
   /// Formulario modal que permite mover un determinado thread a un canal distinto al que se encuentra actualmente.
   /// </summary>
   public class ForumThreadMoveModal : SimpleRestCallModalForm
   {
      private const string FORM_ID = "frmForumThreadMove";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ForumThreadMoveModal"/>.
      /// </summary>
      /// <param name="container"></param>
      /// <param name="threadId">Identificador del thread a mover.</param>
      /// <param name="channelId">Identificador del canal dónde se encuentra inicialmente el thread.</param>
      public ForumThreadMoveModal(ViewContainer parentViewport, int threadId, int channelId)
         : base(parentViewport, FORM_ID)
      {
         Initialize();

         DomID = FORM_ID;
         ThreadID = threadId;
         Title = "Mover thread a otro canal";
         TitleIcon = IconControl.ICON_FOLDER_OPEN;
         RestURL = CmsApi.ServiceUrl;
         Closeable = true;
         Form.UsePanel = false;
         Form.DomID = "frmMoveThread";
         Form.AddFormSetting(Cosmo.Workspace.PARAM_COMMAND, CmsApi.COMMAND_FORUM_THREAD_MOVE);
         Form.AddFormSetting(ForumsDAO.PARAM_THREAD_ID, threadId);

         // Obtiene la lista de canales de los foros
         ForumsDAO fdao = new ForumsDAO(Container.Workspace);
         FormFieldList lstChannels = new FormFieldList(Container, ForumsDAO.PARAM_CHANNEL_ID, "Canal de destino", FormFieldList.ListType.Single);
         lstChannels.Required = true;
         lstChannels.Values = ForumsDAO.ConvertToKeyValueList(fdao.GetForums());
         Form.Content.Add(lstChannels);

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
