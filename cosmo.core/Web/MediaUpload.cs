using Cosmo.FileSystem;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;

namespace Cosmo.Web
{
   /// <summary>
   /// Implementa un formulario modal para subir archivos al servidor.
   /// </summary>
   [AuthenticationRequired]
   [ViewParameter(ParameterName = Workspace.PARAM_FOLDER_ID,
                  PropertyName = "FolderPath")]
   public class MediaUpload : ModalView
   {
      // Modal element unique identifier
      private const string DOM_ID = "fs-upload-modal";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="MediaUpload"/>.
      /// </summary>
      public MediaUpload()
         : base(MediaUpload.DOM_ID)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="MediaUpload"/>.
      /// </summary>
      /// <param name="objectId">Identificador del objeto al que va asociado el contenido subido.</param>
      public MediaUpload(IFileSystemID objectId)
         : base(MediaUpload.DOM_ID)
      {
         Initialize();

         this.FolderPath = objectId.ToFolderName();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the relative folder where the files will be uploaded.
      /// </summary>
      public string FolderPath { get; set; }
      
      #endregion

      #region ModalView Implementation

      public override void InitPage()
      {
         string path = string.Empty;
         FormFieldFile fileField;
         
         Title = "Adjuntar archivos al contenido";
         Closeable = true;
         Icon = IconControl.ICON_FOLDER_OPEN;

         // this.ObjectID = Parameters.GetInteger(Workspace.PARAM_OBJECT_ID, this.ObjectID);
         // this.IsContainerObject = Parameters.GetBoolean(UploadFilesModal.PARAM_ISCONTAINER, this.IsContainerObject);

         FormControl form = new FormControl(this, "frmUploadFiles");
         form.SendDataMethod = FormControl.FormSendDataMethod.JSSubmit;
         form.IsMultipart = true;
         form.Action = GetType().Name;
         form.Method = "post";
         form.AddFormSetting(Cosmo.Workspace.PARAM_FOLDER_ID, this.FolderPath);

         // Get the current object ID
         path = Workspace.FileSystemService.GetObjectFolder(FolderPath);

         fileField = new FormFieldFile(this, "file1", "Archivo 1");
         fileField.DowloadPath = path;
         form.Content.Add(fileField);

         fileField = new FormFieldFile(this, "file2", "Archivo 2");
         fileField.DowloadPath = path;
         form.Content.Add(fileField);

         fileField = new FormFieldFile(this, "file3", "Archivo 3");
         fileField.DowloadPath = path;
         form.Content.Add(fileField);

         fileField = new FormFieldFile(this, "file4", "Archivo 4");
         fileField.DowloadPath = path;
         form.Content.Add(fileField);

         form.FormButtons.Add(new ButtonControl(this, "cmdAccept", "Enviar", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdClose", "Cancelar", ButtonControl.ButtonTypes.CloseModalForm));

         Content.Add(form);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         if (receivedForm.UploadedFiles > 0)
         {
            Content.Clear();

            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Operación completada con éxito";
            callout.Icon = IconControl.ICON_CHECK;
            callout.Text = "Se han subido correctament " + receivedForm.UploadedFiles + " archivo(s) al servidor.";
            callout.Type = ComponentColorScheme.Success;

            Content.Add(callout);
         }
         else
         {
            Content.Clear();

            CalloutControl callout = new CalloutControl(this);
            callout.Title = "La operación ha fallado";
            callout.Icon = IconControl.ICON_CHECK;
            callout.Text = "No se ha podido subir ningún archivo al servidor debido a errores.";
            callout.Type = ComponentColorScheme.Error;

            Content.Add(callout);
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.FolderPath = string.Empty;
      }

      #endregion

      #region Disabled Code

      /*#region IModalContainer Implementation

      public override void PreRenderForm()
      {
         Title = "Adjuntar archivos al contenido";

         Form.IsMultipart = true;
         Form.Method = "post";
         Form.DomID = DOM_ID;
         Form.AddFormSetting(Cosmo.Workspace.PARAM_COMMAND, Cosmo.REST.FileSystemRestHandler.COMMAND_UPLOAD);
         Form.AddFormSetting(Cosmo.Workspace.PARAM_OBJECT_ID, ObjectID);
         Form.Content.Add(new FormFieldFile(Container, "file1", "Archivo 1"));
         Form.Content.Add(new FormFieldFile(Container, "file2", "Archivo 2"));
         Form.Content.Add(new FormFieldFile(Container, "file3", "Archivo 3"));
         Form.Content.Add(new FormFieldFile(Container, "file4", "Archivo 4"));
         Form.FormButtons.Add(new ButtonControl(Container, "cmdSend", "Enviar", "#", "cmdUploadFiles();"));

         // Scripting

         SimpleScript js = new SimpleScript(null);

         js.Source.AppendLine("function cmdUploadFiles() {");
         js.Source.AppendLine("  var " + DOM_ID + "Data = new FormData();");
         js.Source.AppendLine("  " + DOM_ID + "Data.append('" + Cosmo.Workspace.PARAM_COMMAND + "', '" + Cosmo.REST.FileSystemRestHandler.COMMAND_UPLOAD + "');");
         js.Source.AppendLine("  " + DOM_ID + "Data.append('" + Cosmo.Workspace.PARAM_OBJECT_ID + "', '" + ObjectID + "');");
         js.Source.AppendLine("  " + DOM_ID + "Data.append('file1', $('#file1')[0].files[0]);");
         js.Source.AppendLine("  " + DOM_ID + "Data.append('file2', $('#file2')[0].files[0]);");
         js.Source.AppendLine("  " + DOM_ID + "Data.append('file3', $('#file3')[0].files[0]);");
         js.Source.AppendLine("  " + DOM_ID + "Data.append('file4', $('#file4')[0].files[0]);");
         js.Source.AppendLine("  $.ajax({");
         js.Source.AppendLine("    url: '" + FileSystemRestHandler.ServiceUrl + "',");
         js.Source.AppendLine("    type: 'POST',");
         js.Source.AppendLine("    data: " + DOM_ID + "Data,");
         js.Source.AppendLine("    async: false,");
         js.Source.AppendLine("    cache: false,");
         js.Source.AppendLine("    contentType: false,");
         js.Source.AppendLine("    processData: false,");
         js.Source.AppendLine("    success: function(data) {");
         js.Source.AppendLine("      alert(data.message);");
         js.Source.AppendLine("    }");
         js.Source.AppendLine("  });");
         js.Source.AppendLine("  return false;");
         js.Source.AppendLine("}");

         AddScript(js);
      }

      #endregion*/

      #endregion

   }
}
