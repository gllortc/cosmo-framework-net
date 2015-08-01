using Cosmo.Cms.Model.Forum;
using Cosmo.UI;
using Cosmo.UI.Controls;

namespace Cosmo.Cms.Web
{
   [ViewParameter(ParameterName = ForumsDAO.PARAM_THREAD_ID,
                  PropertyName = "ThreadID")]
   public class ForumThreadToggleStatusModal : ModalView
   {
      // Modal element unique identifier 
      private const string DOM_ID = "forum-thread-toggle-modal";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="ForumThreadToggleStatusModal"/>.
      /// </summary>
      public ForumThreadToggleStatusModal()
         : base(ForumThreadToggleStatusModal.DOM_ID)
      {
         Initialize();
      }

      /// <summary>
      /// Gets an instance of <see cref="ForumThreadToggleStatusModal"/>.
      /// </summary>
      /// <param name="threadId">Thread identifier.</param>
      public ForumThreadToggleStatusModal(int threadId)
         : base(ForumThreadToggleStatusModal.DOM_ID)
      {
         Initialize();

         this.ThreadID = threadId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del thread que se desea eliminar.
      /// </summary>
      public int ThreadID { get; set; } 

      #endregion

      #region ModalViewContainer Implementation

      public override void InitPage()
      {
         Title = "Cerrar/reactivar thread";
         Icon = IconControl.ICON_DELETE;
         Closeable = true;

         FormControl form = new FormControl(this, "frmToggleThread");
         form.UsePanel = false;
         form.SendDataMethod = FormControl.FormSendDataMethod.JSSubmit;
         form.AddFormSetting(ForumsDAO.PARAM_THREAD_ID, this.ThreadID);

         CalloutControl callout = new CalloutControl(this);
         callout.Type = ComponentColorScheme.Information;
         callout.Title = "Información sobre la acción";
         callout.Icon = IconControl.ICON_WARNING;
         callout.Text = "Si el hilo actual está cerrado debido a la inactividad por un determinado tiempo, esta acción no lo reabrirá.";
         form.Content.Add(callout);

         // Agrega un mensaje de confirmación
         HtmlContentControl confirm = new HtmlContentControl(this);
         confirm.AppendParagraph("¿Está seguro/a que desea cerrar o reactivar el thread actual?");
         form.Content.Add(confirm);

         form.FormButtons.Add(new ButtonControl(this, "cmdAccept", "Aceptar", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdClose", "Cancelar", ButtonControl.ButtonTypes.CloseModalForm));

         Content.Add(form);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // Obtiene los parámetros de la llamada.
         int threadId = receivedForm.GetIntFieldValue(ForumsDAO.PARAM_THREAD_ID);

         Content.Clear();

         try
         {
            // Ejecuta el método para mover el hilo
            ForumsDAO fdao = new ForumsDAO(Workspace);
            fdao.ToggleThreadStatus(threadId);

            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Operación completada con éxito";
            callout.Icon = IconControl.ICON_CHECK;
            callout.Text = "El hilo ha cambiado de estado.";
            callout.Type = ComponentColorScheme.Success;

            Content.Add(callout);
         }
         catch
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Title = "ERROR";
            callout.Icon = IconControl.ICON_WARNING;
            callout.Text = "Se ha producido un error al intentar completar la operación.";
            callout.Type = ComponentColorScheme.Error;

            Content.Add(callout);
         }

         ButtonControl cmdClose = new ButtonControl(this, "cmdClose", "Cerrar", ButtonControl.ButtonTypes.CloseModalForm);
         Content.Add(cmdClose);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.ThreadID = 0;
      }

      #endregion

   }
}
