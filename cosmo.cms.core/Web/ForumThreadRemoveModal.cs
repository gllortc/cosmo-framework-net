using Cosmo.Cms.Model.Forum;
using Cosmo.UI;
using Cosmo.UI.Controls;

namespace Cosmo.Cms.Web
{
   [ViewParameter(ParameterName = ForumsDAO.PARAM_THREAD_ID,
                  PropertyName = "ThreadID")]
   public class ForumThreadRemoveModal : ModalView
   {
      // Modal element unique identifier
      private const string DOM_ID = "forum-thread-delete-modal";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="ForumThreadRemoveModal"/>.
      /// </summary>
      public ForumThreadRemoveModal()
         : base()
      {
         Initialize();

         this.DomID = ForumThreadRemoveModal.DOM_ID;
      }

      /// <summary>
      /// Gets an instance of <see cref="ForumThreadRemoveModal"/>.
      /// </summary>
      /// <param name="threadId">Thread identifier.</param>
      public ForumThreadRemoveModal(int threadId) 
         : base()
      {
         Initialize();

         this.DomID = ForumThreadRemoveModal.DOM_ID;
         this.ThreadID = threadId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del thread que se desea eliminar.
      /// </summary>
      public int ThreadID { get; set; } 

      #endregion

      #region ModalView Implementation

      public override void InitPage()
      {
         Title = "Eliminar thread";
         Icon = IconControl.ICON_DELETE;
         Closeable = true;

         FormControl form = new FormControl(this, "frmRemoveThread");
         form.UsePanel = false;
         form.SendDataMethod = FormControl.FormSendDataMethod.JSSubmit;
         form.AddFormSetting(ForumsDAO.PARAM_THREAD_ID, this.ThreadID);

         CalloutControl callout = new CalloutControl(this);
         callout.Type = ComponentColorScheme.Warning;
         callout.Title = "ATENCIÓN";
         callout.Icon = IconControl.ICON_WARNING;
         callout.Text = "Tenga en cuenta que esta acción no puede deshacerse. Cuando se trata de threads creados por usuarios es más adecuado moverlo a un canal oculto (acción que permite recuperar el thread original).";
         form.Content.Add(callout);

         // Agrega un mensaje de confirmación
         HtmlContentControl confirm = new HtmlContentControl(this);
         confirm.AppendParagraph("¿Está completamente seguro/a que desea eliminar el thread actual?");
         form.Content.Add(confirm);

         form.FormButtons.Add(new ButtonControl(this, "cmdAccept", "Eliminar", ButtonControl.ButtonTypes.Submit));
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
            fdao.DeleteMessage(threadId, true);

            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Operación completada con éxito";
            callout.Icon = IconControl.ICON_CHECK;
            callout.Text = "El hilo se ha eliminado con éxito.";
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

      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      public override void LoadPage()
      {
         // Nothing to do
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         ThreadID = 0;
      }

      #endregion

   }
}
