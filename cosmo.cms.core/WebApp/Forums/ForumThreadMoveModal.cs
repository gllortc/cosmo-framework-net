using Cosmo.Cms.Forums;
using Cosmo.Cms.REST;
using Cosmo.UI;
using Cosmo.UI.Controls;

namespace Cosmo.WebApp.Forums
{
   /// <summary>
   /// Formulario modal que permite mover un determinado thread a un canal distinto al que se encuentra actualmente.
   /// </summary>
   [ViewParameter(ParameterName = ForumsDAO.PARAM_THREAD_ID,
                  PropertyName = "ThreadID")]
   [ViewParameter(ParameterName = ForumsDAO.PARAM_CHANNEL_ID,
                  PropertyName = "ChannelID")]
   public class ForumThreadMoveModal : ModalViewContainer // SimpleRestCallModalForm
   {

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del thread que se desea eliminar.
      /// </summary>
      public int ThreadID { get; set; }

      /// <summary>
      /// Devuelve o establece el identificador del thread que se desea eliminar.
      /// </summary>
      public int ChannelID { get; set; } 

      #endregion

      #region ModalViewContainer Implementation

      public override void InitPage()
      {
         Title = "Mover thread a otro canal";
         Closeable = true;
         Icon = IconControl.ICON_FOLDER_OPEN;

         FormControl form = new FormControl(this, "frmMoveThread");
         form.UsePanel = false;
         form.AddFormSetting(Cosmo.Workspace.PARAM_COMMAND, CmsApi.COMMAND_FORUM_THREAD_MOVE);
         form.AddFormSetting(ForumsDAO.PARAM_THREAD_ID, this.ThreadID);
         form.FormButtons.Add(new ButtonControl(this, "cmdAccept", "Mover", ButtonControl.ButtonTypes.SubmitJS));
         form.FormButtons.Add(new ButtonControl(this, "cmdClose", "Cancelar", ButtonControl.ButtonTypes.CloseModalForm));

         ForumsDAO fdao = new ForumsDAO(Workspace);
         FormFieldList lstChannels = new FormFieldList(this, ForumsDAO.PARAM_CHANNEL_ID, "Canal de destino", FormFieldList.ListType.Single);
         lstChannels.Required = true;
         lstChannels.Values = ForumsDAO.ConvertToKeyValueList(fdao.GetForums());
         form.Content.Add(lstChannels);

         Content.Add(form);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // Obtiene los parámetros de la llamada.
         int threadId = receivedForm.GetIntFieldValue(ForumsDAO.PARAM_THREAD_ID);
         int channelId = Parameters.GetInteger(ForumsDAO.PARAM_CHANNEL_ID);

         try
         {
            // Ejecuta el método para mover el hilo
            ForumsDAO fdao = new ForumsDAO(Workspace);
            fdao.MoveThread(threadId, channelId);

            Redirect(ForumsDAO.GetChannelUrl(channelId));
         }
         catch
         {
            // SendResponse(new AjaxResponse(5001, "Se ha producido un error interno y no ha sido posible mover el hilo solicitado."));
         }
      }

      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      public override void LoadPage()
      {
         // Nothing to do
      }

      #endregion

   }
}
