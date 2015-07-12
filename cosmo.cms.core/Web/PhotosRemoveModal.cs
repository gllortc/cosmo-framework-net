using Cosmo.Cms.Model.Photos;
using Cosmo.UI;
using Cosmo.UI.Controls;

namespace Cosmo.Cms.Web
{
   [ViewParameter(ParameterName = Workspace.PARAM_OBJECT_ID,
                  PropertyName = "PhotoID")]
   public class PhotosRemoveModal : ModalView
   {
      // Modal element unique identifier
      private const string DOM_ID = "photo-delete-modal";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="PhotosRemoveModal"/>.
      /// </summary>
      public PhotosRemoveModal()
         : base()
      {
         Initialize();

         this.DomID = PhotosRemoveModal.DOM_ID;
      }

      /// <summary>
      /// Gets an instance of <see cref="PhotosRemoveModal"/>.
      /// </summary>
      /// <param name="photoId">Photo unique identifier.</param>
      public PhotosRemoveModal(int photoId) 
         : base()
      {
         Initialize();

         this.DomID = PhotosRemoveModal.DOM_ID;
         this.PhotoID = photoId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the photo unique identifier.
      /// </summary>
      public int PhotoID { get; set; } 

      #endregion

      #region ModalView Implementation

      public override void InitPage()
      {
         Title = "Eliminar fotografia";
         Icon = IconControl.ICON_DELETE;
         Closeable = true;

         FormControl form = new FormControl(this, "frmRemovePhoto");
         form.UsePanel = false;
         form.SendDataMethod = FormControl.FormSendDataMethod.JSSubmit;
         form.AddFormSetting(Cosmo.Workspace.PARAM_OBJECT_ID, this.PhotoID);

         CalloutControl callout = new CalloutControl(this);
         callout.Type = ComponentColorScheme.Warning;
         callout.Title = "ATENCIÓN";
         callout.Icon = IconControl.ICON_WARNING;
         callout.Text = "Tenga en cuenta que esta acción no puede deshacerse. Una vez eliminada la foto ya no hay forma de recuperarla (ni los datos ni los archivos). Puede usar una carpeta oculta para situar fotografias que no desea mostrar sin eliminarlas definitivamente.";
         form.Content.Add(callout);

         // Agrega un mensaje de confirmación
         HtmlContentControl confirm = new HtmlContentControl(this);
         confirm.AppendParagraph("¿Está completamente seguro/a que desea eliminar la fotografia?");
         form.Content.Add(confirm);

         form.FormButtons.Add(new ButtonControl(this, "cmdAccept", "Eliminar", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdClose", "Cancelar", ButtonControl.ButtonTypes.CloseModalForm));

         Content.Add(form);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // Obtiene los parámetros de la llamada.
         int photoId = receivedForm.GetIntFieldValue(Cosmo.Workspace.PARAM_OBJECT_ID);

         Content.Clear();

         try
         {
            // Ejecuta el método para mover el hilo
            PhotoDAO fdao = new PhotoDAO(Workspace);
            fdao.Delete(photoId, true);

            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Operación completada con éxito";
            callout.Icon = IconControl.ICON_CHECK;
            callout.Text = "La fotografia ha sido eliminada con éxito.";
            callout.Type = ComponentColorScheme.Success;

            Content.Add(callout);
         }
         catch
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Uuupppsss! Se produjo un error...";
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
         this.PhotoID = 0;
      }

      #endregion

   }
}
