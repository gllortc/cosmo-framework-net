using Cosmo.REST;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Modals;
using Cosmo.UI.Scripting;

namespace Cosmo.WebApp.FileSystemServices
{
   /// <summary>
   /// Implementa un formulario modal para subir archivos al servidor.
   /// </summary>
   public class ModalFormUpload : IModalForm
   {
      private const string FORM_ID = "frmUploadForm";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ModalFormUpload"/>.
      /// </summary>
      /// <param name="container">Contenedor principal dónde se encuentra el control.</param>
      /// <param name="domID">Identificador único (DOM) del formulario.</param>
      /// <param name="objectId">Identificador del objeto al que va asociado el contenido subido.</param>
      public ModalFormUpload(ViewContainer parentViewport, string domID, string objectId)
         : base(parentViewport, domID)
      {
         Initialize();

         this.DomID = domID;
         this.ObjectID = objectId;
      }

      #endregion

      #region Properties;

      /// <summary>
      /// Devuelve o establece el identificador del objeto al que va asociado el contenido subido.
      /// </summary>
      public string ObjectID { get; set; }
      
      #endregion

      #region IModalContainer Implementation

      public override void PreRenderForm()
      {
         Title = "Adjuntar archivos al contenido";

         Form.IsMultipart = true;
         Form.Method = "post";
         Form.DomID = FORM_ID;
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
         js.Source.AppendLine("  var " + FORM_ID + "Data = new FormData();");
         js.Source.AppendLine("  " + FORM_ID + "Data.append('" + Cosmo.Workspace.PARAM_COMMAND + "', '" + Cosmo.REST.FileSystemRestHandler.COMMAND_UPLOAD + "');");
         js.Source.AppendLine("  " + FORM_ID + "Data.append('" + Cosmo.Workspace.PARAM_OBJECT_ID + "', '" + ObjectID + "');");
         js.Source.AppendLine("  " + FORM_ID + "Data.append('file1', $('#file1')[0].files[0]);");
         js.Source.AppendLine("  " + FORM_ID + "Data.append('file2', $('#file2')[0].files[0]);");
         js.Source.AppendLine("  " + FORM_ID + "Data.append('file3', $('#file3')[0].files[0]);");
         js.Source.AppendLine("  " + FORM_ID + "Data.append('file4', $('#file4')[0].files[0]);");
         js.Source.AppendLine("  $.ajax({");
         js.Source.AppendLine("    url: '" + FileSystemRestHandler.ServiceUrl + "',");
         js.Source.AppendLine("    type: 'POST',");
         js.Source.AppendLine("    data: " + FORM_ID + "Data,");
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

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         ObjectID = string.Empty;
      }

      #endregion

   }
}
