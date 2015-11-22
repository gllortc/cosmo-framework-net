using Cosmo.Cms.Model.Content;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using Cosmo.Web;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Cms.Web
{
   [AuthorizationRequired(DocumentDAO.ROLE_CONTENT_EDITOR)]
   public class ContentEdit : PageView
   {
      // Declaración de nombres de parámetros
      private const string FIELD_TITLE = "tit";
      private const string FIELD_DESCRIPTION = "des";
      private const string FIELD_CONTENT = "con";
      private const string FIELD_THUMBNAIL = "thb";
      private const string FIELD_ATTACHMENT = "att";
      private const string FIELD_STATUS = "sta";
      private const string FIELD_HIGHLIGHT = "hgl";

      #region PageView Implementation

      public override void InitPage()
      {
         string cmd;
         Document doc = null;
         DocumentFolder folder;
         AjaxUpdateListScript loaderScript;
         Dictionary<string, object> jsViewParams;

         // Agrega los recursos necesarios para representar la página actual
         Resources.Add(new ViewResource(ViewResource.ResourceType.JavaScript, "include/ContentEdit.js"));

         //-------------------------------
         // Obtención de parámetros
         //-------------------------------

         // Obtiene los parámetros de llamada
         int docId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);
         DocumentFSID fsId = new DocumentFSID(docId);

         //-------------------------------
         // Obtención de datos
         //-------------------------------

         // Inicializaciones
         DocumentDAO docs = new DocumentDAO(Workspace);

         cmd = Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND);
         switch (cmd)
         {
            case Cosmo.Workspace.COMMAND_EDIT:
               doc = docs.GetByID(docId);
               break;

            case Cosmo.Workspace.COMMAND_ADD:
               doc = new Document();
               doc.FolderId = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);
               break;

            default:
               ShowError("Llamada incorrecta!",
                         "Hemos detectado una llamada incorrecta al editor de artículos. No es posible abrir el editor en estas condiciones.");
               break;
         }

         // Obtiene el documento y la carpeta
         folder = docs.GetFolder(doc.FolderId, false);

         //-------------------------------
         // Habilita formularios modales
         //-------------------------------
         Cosmo.Web.MediaUpload frmUpload = new Cosmo.Web.MediaUpload(new DocumentFSID(doc.ID));
         Modals.Add(frmUpload);

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = doc.Title + " | " + DocumentDAO.SERVICE_NAME;
         ActiveMenuId = folder.MenuId;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_BOOK;
         MainContent.Add(header);

         TabbedContainerControl tabs = new TabbedContainerControl(this);

         // Content form tab

         TabItemControl frmData = new TabItemControl(this, "tabData", "Contenido");
         frmData.Content.Add(new FormFieldText(this, FIELD_TITLE, "Título", FormFieldText.FieldDataType.Text, doc.Title));
         frmData.Content.Add(new FormFieldEditor(this, FIELD_DESCRIPTION, "Descripción", FormFieldEditor.FieldEditorType.Simple, doc.Description));
         frmData.Content.Add(new FormFieldEditor(this, FIELD_CONTENT, "Contenido", FormFieldEditor.FieldEditorType.HTML, doc.Content));

         //FormFieldImage thumb = new FormFieldImage(this, FIELD_THUMBNAIL, "Imagen miniatura", doc.Thumbnail);
         //thumb.Description = "Si deja este campo en blanco no se guardará la imagen y se mantendrá la actual.";
         //thumb.PreviewUrl = Workspace.FileSystemService.GetFileURL(new DocumentFSID(doc.ID), doc.Thumbnail);
         //frmData.Content.Add(thumb);

         loaderScript = new AjaxUpdateListScript(this);
         loaderScript.InvokeOnLoad = true;
         loaderScript.ExecutionType = Script.ScriptExecutionMethod.OnFunctionCall;
         loaderScript.FunctionName = "loadThumbList";
         loaderScript.ListControlName = FIELD_THUMBNAIL;
         loaderScript.Url = Cosmo.Web.Handlers.FileSystemRestHandler.GetFolderFileListUrl(new DocumentFSID(doc.ID));
         loaderScript.ListItems.Add(new KeyValue("Sin miniatura", string.Empty));
         loaderScript.DefaultValue = doc.Thumbnail;
         Scripts.Add(loaderScript);

         FormFieldList lstThumbnail = new FormFieldList(this, FIELD_THUMBNAIL, "Imagen miniatura", FormFieldList.ListType.Single, doc.Thumbnail);
         lstThumbnail.LoadValuesFromAjax(loaderScript);
         frmData.Content.Add(lstThumbnail);

         loaderScript = new AjaxUpdateListScript(this);
         loaderScript.InvokeOnLoad = true;
         loaderScript.ExecutionType = Script.ScriptExecutionMethod.OnFunctionCall;
         loaderScript.FunctionName = "loadAttachList";
         loaderScript.ListControlName = FIELD_ATTACHMENT;
         loaderScript.Url = Cosmo.Web.Handlers.FileSystemRestHandler.GetFolderFileListUrl(new DocumentFSID(doc.ID));
         loaderScript.ListItems.Add(new KeyValue("Sin archivo adjunto", string.Empty));
         loaderScript.DefaultValue = doc.Attachment;
         Scripts.Add(loaderScript);

         FormFieldList lstAttachment = new FormFieldList(this, FIELD_ATTACHMENT, "Archivo adjunto", FormFieldList.ListType.Single, doc.Attachment);
         lstAttachment.LoadValuesFromAjax(loaderScript);
         frmData.Content.Add(lstAttachment);

         FormFieldList lstStatus = new FormFieldList(this, FIELD_STATUS, "Estado", FormFieldList.ListType.Single, (doc.Published ? "1" : "0"));
         lstStatus.Values.Add(new KeyValue("Despublicado (Borrador)", "0"));
         lstStatus.Values.Add(new KeyValue("Publicado", "1"));
         frmData.Content.Add(lstStatus);

         frmData.Content.Add(new FormFieldBoolean(this, FIELD_HIGHLIGHT, "Artículo destacado en portada", doc.Hightlight));

         tabs.TabItems.Add(frmData);

         // Files (media and content attachments) tab

         TabItemControl tabFiles = new TabItemControl(this, "tabFiles", "Archivos adjuntos");

         HtmlContentControl filesContent = new HtmlContentControl(this);
         filesContent.AppendParagraph("La siguiente lista contiene los archivos adjuntos al contenido.");
         tabFiles.Content.Add(filesContent);

         jsViewParams = new Dictionary<string, object>();
         jsViewParams.Add(Cosmo.Workspace.PARAM_FOLDER_ID, fsId.ToFolderName());

         ButtonGroupControl btnFiles = new ButtonGroupControl(this);
         btnFiles.Size = ButtonControl.ButtonSizes.Small;
         btnFiles.Buttons.Add(new ButtonControl(this, "cmdAddFiles", "Agregar archivos", string.Empty, frmUpload.GetInvokeCall(jsViewParams)));
         btnFiles.Buttons.Add(new ButtonControl(this, "cmdRefresh", "Actualizar", IconControl.ICON_REFRESH, "#", "cosmoUIServices.loadTemplate();"));
         tabFiles.Content.Add(btnFiles);

         MediaFileList fileList = new MediaFileList(new DocumentFSID(doc.ID));
         PartialViewContainerControl fileListView = new PartialViewContainerControl(this, fileList);
         tabFiles.Content.Add(fileListView);
         Scripts.Add(fileList.GetInvokeScript(Script.ScriptExecutionMethod.OnDocumentReady, cmd, doc.ID));

         btnFiles.Buttons[1].Href = string.Empty;
         btnFiles.Buttons[1].JavaScriptAction = fileList.GetInvokeCall(jsViewParams);

         tabs.TabItems.Add(tabFiles);

         // Content properties tab

         HtmlContentControl infoContent = new HtmlContentControl(this);
         infoContent.AppendParagraph("Propiedades del contenido en el momento actual (antes de guardar y actualizar las propiedades).");

         TableControl tableInfo = new TableControl(this);
         tableInfo.Bordered = false;
         tableInfo.Condensed = true;
         tableInfo.Header = new TableRow(string.Empty, "Propiedad", "Valor");
         tableInfo.Rows.Add(new TableRow(string.Empty, "Fecha de creación", doc.Created.ToString("dd/MM/yyyy hh:mm")));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Última actualización", doc.Updated.ToString("dd/MM/yyyy hh:mm")));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Propietario", doc.Owner));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Estado actual", doc.Published ? "Publicado" : "Despublicado (Borrador)"));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Destacado", doc.Hightlight ? "Sí" : "No"));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Nodo actual", "<a href=\"" + ContentByFolder.GetURL(folder.ID) + "\">" + folder.Name + "</a> (#" + folder.ID + ")"));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Visitas", doc.Shows));

         TabItemControl frmInfo = new TabItemControl(this, "tabInfo", "Información");
         frmInfo.Content.Add(infoContent);
         frmInfo.Content.Add(tableInfo);
         tabs.TabItems.Add(frmInfo);

         // Form generation

         FormControl form = new FormControl(this, "frmCEdit");
         form.IsMultipart = true;
         form.Icon = "fa-edit";
         form.Text = "Editar artículo";
         form.Action = GetType().Name;
         form.AddFormSetting(Cosmo.Workspace.PARAM_COMMAND, Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND));
         form.AddFormSetting(Cosmo.Workspace.PARAM_OBJECT_ID, doc.ID);
         form.AddFormSetting(Cosmo.Workspace.PARAM_FOLDER_ID, doc.FolderId);
         form.Content.Add(tabs);
         form.FormButtons.Add(new ButtonControl(this, "btnSave", "Guardar", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "btnCancel", "Cancelar", ContentView.GetURL(doc.ID), string.Empty));

         form.FormButtons[0].Color = ComponentColorScheme.Success;
         form.FormButtons[0].Icon = "fa-check";

         MainContent.Add(form);
      }

      /// <summary>
      /// Trata los datos recibidos de un formulario.
      /// </summary>
      public override void FormDataReceived(FormControl receivedForm)
      {
         Document doc = new Document();

         // Obtiene los datos del formulario
         doc.ID = receivedForm.GetIntFieldValue(Cosmo.Workspace.PARAM_OBJECT_ID);
         doc.FolderId = receivedForm.GetIntFieldValue(Cosmo.Workspace.PARAM_FOLDER_ID);
         doc.Title = receivedForm.GetStringFieldValue(FIELD_TITLE);
         doc.Description = receivedForm.GetStringFieldValue(FIELD_DESCRIPTION);
         doc.Content = receivedForm.GetStringFieldValue(FIELD_CONTENT);
         doc.Published = receivedForm.GetBoolFieldValue(FIELD_STATUS);
         doc.Hightlight = receivedForm.GetBoolFieldValue(FIELD_HIGHLIGHT);
         doc.Thumbnail = receivedForm.GetStringFieldValue(FIELD_THUMBNAIL);
         doc.Attachment = receivedForm.GetStringFieldValue(FIELD_ATTACHMENT);

         if (receivedForm.GetFileFieldValue(FIELD_THUMBNAIL) != null) doc.Thumbnail = receivedForm.GetFileFieldValue(FIELD_THUMBNAIL).Name;
         // if (receivedForm.GetFileFieldValue(FIELD_ATTACHMENT) != null) doc.Attachment = receivedForm.GetFileFieldValue(FIELD_ATTACHMENT).Name;

         // Realiza las operaciones de persistencia
         DocumentDAO docs = new DocumentDAO(Workspace);
         switch (Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND))
         {
            case Cosmo.Workspace.COMMAND_EDIT:
               docs.Update(doc);
               break;

            case Cosmo.Workspace.COMMAND_ADD:
               docs.Add(doc);
               break;

            default: 
               break;
         }

         // Remove content highlights to force to refresh
         Cache.Remove(Home.CACHE_CONTENT_HIGHLIGHTED);

         // Redirige a la página del artículo
         Redirect(ContentView.GetURL(doc.ID));
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Gets the URL for editing a new content.
      /// </summary>
      /// <param name="folderId">Identificador del contenido.</param>
      public static string GetURL(int folderId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, folderId);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_ADD);

         return url.ToString();
      }

      /// <summary>
      /// Gets the URL for editing an existing content.
      /// </summary>
      /// <param name="folderId">Identificador del contenido.</param>
      public static string GetURL(int folderId, int documentId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, folderId);
         url.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, documentId);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_EDIT);

         return url.ToString();
      }

      #endregion

   }
}
