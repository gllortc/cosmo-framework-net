using Cosmo.Cms.Photos;
using Cosmo.UI;
using Cosmo.UI.Controls;

namespace Cosmo.WebApp.Photos
{
   [ViewParameter(ParameterName = Workspace.PARAM_OBJECT_ID,
                  PropertyName = "PhotoID")]
   public class PhotosEditModal : Cosmo.UI.ModalView
   {
      // Modal element unique identifier
      private const string DOM_ID = "photo-edit-modal";

      // Internal data declarations
      private FormControl form = null;
      private Photo photo = null;
      private PhotoDAO photoDao = null;

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="PhotosEditModal"/>.
      /// </summary>
      public PhotosEditModal()
         : base()
      {
         Initialize();

         this.DomID = PhotosEditModal.DOM_ID;
      }

      /// <summary>
      /// Gets an instance of <see cref="PhotosEditModal"/>.
      /// </summary>
      /// <param name="photoId">Photo unique identifier.</param>
      public PhotosEditModal(int photoId) 
         : base()
      {
         Initialize();

         this.DomID = PhotosEditModal.DOM_ID;
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
         Title = PhotoDAO.SERVICE_NAME + " - Editar fotografia";
         Icon = IconControl.ICON_CAMERA;

         // Gets the call parameters
         int picId = Parameters.GetInteger(Workspace.PARAM_OBJECT_ID);
         if (picId <= 0)
         {
            ShowError("Fotografia no encontrada",
                      "La fotografia especificada no existe o no se encuentra disponible en estos momentos.");
            return;
         }

         // Gets the picture to edit
         photoDao = new PhotoDAO(Workspace);
         photo = photoDao.GetPicture(picId);
         if (photo == null)
         {
            ShowError("Fotografia no encontrada",
                      "La fotografia especificada no existe o no se encuentra disponible en estos momentos.");
            return;
         }

         // Genera el formulario para objetos del tipo User
         form = new FormControl(this);
         form.IsMultipart = true;
         form.DomID = "frmUploadPhoto";
         form.Caption = "Editar fotografia";
         form.Icon = IconControl.ICON_UPLOAD;

         form.AddFormSetting(Cosmo.Workspace.PARAM_FOLDER_ID, photo.FolderId);
         form.AddFormSetting(Cosmo.Workspace.PARAM_USER_ID, photo.UserID);

         FormFieldEditor body = new FormFieldEditor(this, "body", "Texto descriptivo", FormFieldEditor.FieldEditorType.Simple);
         body.Description = "Solo texto, sin saltos de línea (no tienen efecto) ni enlaces (<em>links</em>).";
         body.Required = true;
         form.Content.Add(body);

         FormFieldText site = new FormFieldText(this, "site", "Lugar", FormFieldText.FieldDataType.Text);
         site.Description = "Use el formato: <em>Ciudad (Provincia)</em> o en su defecto <em>Ciudad (Pais)</em>.";
         site.Required = true;
         form.Content.Add(site);

         FormFieldDate date = new FormFieldDate(this, "date", "Fecha de captura", FormFieldDate.FieldDateType.Date);
         date.Required = true;
         date.Value = photo.Created;
         form.Content.Add(date);

         FormFieldText author = new FormFieldText(this, "author", "Autor de la fotografia", FormFieldText.FieldDataType.Text);
         author.Value = Workspace.CurrentUser.User.GetDisplayName();
         author.Description = "Rellena este campo sólo si tu no eres el autor.";
         author.Value = photo.Author;
         form.Content.Add(author);

         form.FormButtons.Add(new ButtonControl(this, "cmdSend", "Actualizar", IconControl.ICON_SEND, ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdCancel", "Cancelar", IconControl.ICON_REPLY, ButtonControl.ButtonTypes.CloseModalForm));

         Content.Add(form);
      }

      public override void FormDataReceived(UI.Controls.FormControl receivedForm)
      {
         // throw new NotImplementedException();
      }

      public override void FormDataLoad(string formDomID)
      {
         // throw new NotImplementedException();
      }

      public override void LoadPage()
      {
         // throw new NotImplementedException();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance.
      /// </summary>
      private void Initialize()
      {
         this.PhotoID = 0;
      }

      #endregion

   }
}
