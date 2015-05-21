﻿using Cosmo.UI;
using Cosmo.UI.Controls;
using System;

namespace Cosmo.WebApp.FileSystemServices
{
   /// <summary>
   /// Implementa un formulario modal para subir archivos al servidor.
   /// </summary>
   public class ModalFormUpload : ModalViewContainer
   {
      // Modal element unique identifier
      private const string DOM_ID = "fs-upload-modal";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ModalFormUpload"/>.
      /// </summary>
      public ModalFormUpload()
         : base()
      {
         Initialize();

         this.DomID = ModalFormUpload.DOM_ID;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ModalFormUpload"/>.
      /// </summary>
      /// <param name="objectId">Identificador del objeto al que va asociado el contenido subido.</param>
      public ModalFormUpload(int objectId)
         : base()
      {
         Initialize();

         this.DomID = ModalFormUpload.DOM_ID;
         this.ObjectID = objectId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del objeto al que va asociado el contenido subido.
      /// </summary>
      public int ObjectID { get; set; }
      
      #endregion

      #region ModalViewContainer Implementation

      public override void InitPage()
      {
         Title = "Adjuntar archivos al contenido";
         Closeable = true;
         Icon = IconControl.ICON_FOLDER_OPEN;

         FormControl form = new FormControl(this, "frmUploadFiles");
         form.IsMultipart = true;
         form.Method = "post";
         form.AddFormSetting(Cosmo.Workspace.PARAM_OBJECT_ID, ObjectID);
         form.Content.Add(new FormFieldFile(this, "file1", "Archivo 1"));
         form.Content.Add(new FormFieldFile(this, "file2", "Archivo 2"));
         form.Content.Add(new FormFieldFile(this, "file3", "Archivo 3"));
         form.Content.Add(new FormFieldFile(this, "file4", "Archivo 4"));
         form.FormButtons.Add(new ButtonControl(this, "cmdAccept", "Enviar", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdClose", "Cancelar", ButtonControl.ButtonTypes.CloseModalForm));

         Content.Add(form);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // Obtiene los parámetros de la llamada.
         int objId = receivedForm.GetIntFieldValue(Cosmo.Workspace.PARAM_OBJECT_ID);

         // Inicializaciones
         int savedFiles = 0;
         string file;

         string[] keys = Request.Files.AllKeys;

         foreach (string fileKey in keys)
         {
            try
            {
               file = Workspace.FileSystemService.GetFilePath(objId.ToString(), Request.Files[fileKey].FileName);
               Request.Files[fileKey].SaveAs(file);

               savedFiles++;
            }
            catch (Exception ex)
            {
               file = ex.Message;
            }
         }

         if (savedFiles > 0)
         {
            Content.Clear();

            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Operación completada con éxito";
            callout.Icon = IconControl.ICON_CHECK;
            callout.Text = "Se han subido correctament " + savedFiles + " archivo(s) al servidor.";
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
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.ObjectID = 0;
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
