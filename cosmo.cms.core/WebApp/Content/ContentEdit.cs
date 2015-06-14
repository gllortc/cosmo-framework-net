using Cosmo.Cms.Content;
using Cosmo.Net;
using Cosmo.REST;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using Cosmo.WebApp.FileSystemServices;
using System.IO;
using System.Reflection;

namespace Cosmo.WebApp.Content
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
         // Nothing to do
      }

      public override void LoadPage()
      {
         string cmd;
         Document doc = null;
         DocumentFolder folder;

         // Agrega los recursos necesarios para representar la página actual
         Resources.Add(new ViewResource(ViewResource.ResourceType.JavaScript, "include/ContentEdit.js"));

         //-------------------------------
         // Obtención de parámetros
         //-------------------------------

         // Obtiene los parámetros de llamada
         int docId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);

         //-------------------------------
         // Obtención de datos
         //-------------------------------

         // Inicializaciones
         DocumentDAO docs = new DocumentDAO(Workspace);

         cmd = Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND);
         switch (cmd)
         {
            case Cosmo.Workspace.COMMAND_EDIT: 
               doc = docs.Item(docId);
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
         // Configuración de la vista
         //-------------------------------
         ModalFormUpload frmUpload = new ModalFormUpload(doc.ID);
         Modals.Add(frmUpload);

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = doc.Title + " | " + DocumentDAO.SERVICE_NAME;
         ActiveMenuId = folder.MenuId;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_BOOK;
         MainContent.Add(header);

         // Formulario
         TabbedContainerControl tabs = new TabbedContainerControl(this);

         TabItemControl frmData = new TabItemControl(this, "tabData", "Contenido");
         frmData.Content.Add(new FormFieldText(this, FIELD_TITLE, "Título", FormFieldText.FieldDataType.Text, doc.Title));
         frmData.Content.Add(new FormFieldEditor(this, FIELD_DESCRIPTION, "Descripción", FormFieldEditor.FieldEditorType.Simple, doc.Description));
         frmData.Content.Add(new FormFieldEditor(this, FIELD_CONTENT, "Contenido", FormFieldEditor.FieldEditorType.HTML, doc.Content));

         FormFieldImage thumb = new FormFieldImage(this, FIELD_THUMBNAIL, "Imagen miniatura", doc.Thumbnail);
         thumb.Description = "Si deja este campo en blanco no se guardará la imagen y se mantendrá la actual.";
         thumb.PreviewUrl = Workspace.FileSystemService.GetFileURL(doc.ID.ToString(), doc.Thumbnail);
         frmData.Content.Add(thumb);

         FormFieldList lstStatus = new FormFieldList(this, FIELD_STATUS, "Estado", FormFieldList.ListType.Single, (doc.Published ? "1" : "0"));
         lstStatus.Values.Add(new KeyValue("Despublicado (Borrador)", "0"));
         lstStatus.Values.Add(new KeyValue("Publicado", "1"));
         frmData.Content.Add(lstStatus);

         frmData.Content.Add(new FormFieldBoolean(this, FIELD_HIGHLIGHT, "Artículo destacado en portada", doc.Hightlight));

         tabs.TabItems.Add(frmData);

         HtmlContentControl filesContent = new HtmlContentControl(this);
         filesContent.AppendParagraph("La siguiente lista contiene los archivos adjuntos al contenido.");

         ButtonGroupControl btnFiles = new ButtonGroupControl(this);
         btnFiles.Buttons.Add(new ButtonControl(this, "cmdAddFiles", "Agregar archivos", frmUpload));

         TableControl tableFiles = new TableControl(this);
         tableFiles.Bordered = false;
         tableFiles.Condensed = false;
         tableFiles.Hover = true;
         tableFiles.Header = new TableRow("Archivo", "Archivo", "Tamaño (bytes)", "Acciones");
         int fileIdx = 0;
         string rowId;
         ButtonGroupControl btnBar;
         foreach (FileInfo file in Workspace.FileSystemService.GetObjectFiles(doc.ID.ToString()))
         {
            rowId = "file-idx-" + ++fileIdx;

            btnBar = new ButtonGroupControl(this);
            btnBar.Buttons.Add(new ButtonControl(this, "btnCopyUrl" + fileIdx, "Copiar enlace", "fa-chain", string.Empty, "CopyFileUrl('" + Workspace.FileSystemService.GetFileURL(doc.ID.ToString(), file.Name, true) + "');"));
            btnBar.Buttons.Add(new ButtonControl(this, "btnDownload" + fileIdx, "Descargar", "fa-download", FileSystemRestHandler.GetDownloadFileUrl(doc.ID.ToString(), file.Name).ToString(), string.Empty));
            btnBar.Buttons.Add(new ButtonControl(this, "btnDelete" + fileIdx, "Borrar", "fa-trash-o", string.Empty, "deleteFile('" + rowId + "', '" + doc.ID + "', '" + file.Name + "');"));

            tableFiles.Rows.Add(new TableRow(rowId, 
                                             "<a href=\"" + Workspace.FileSystemService.GetFileURL(doc.ID.ToString(), file.Name) + "\" target=\"_blank\">" + file.Name + "</a>", 
                                             file.Length.ToString(),
                                             Workspace.UIService.Render(btnBar)));
         }

         TabItemControl frmFiles = new TabItemControl(this, "tabFiles", "Archivos");
         //frmFiles.Content.Add(filesContent);
         //frmFiles.Content.Add(btnFiles);
         //frmFiles.Content.Add(tableFiles);
         tabs.TabItems.Add(frmFiles);

         SimpleScript loadFiles = new SimpleScript(this);
         loadFiles.ExecutionType = Script.ScriptExecutionMethod.OnDocumentReady;
         loadFiles.AppendSourceLine("cosmoUIServices.loadTemplate('" + ContentEditFileList.GetTemplateUrl(cmd, docId.ToString()) + "', 'tabFiles');");
         frmFiles.AddScript(loadFiles);

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

         FormControl form = new FormControl(this);
         form.IsMultipart = true;
         form.Icon = "fa-edit";
         form.Caption = "Editar artículo";
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

         if (receivedForm.GetFileFieldValue(FIELD_THUMBNAIL) != null) doc.Thumbnail = receivedForm.GetFileFieldValue(FIELD_THUMBNAIL).Name;
         if (receivedForm.GetFileFieldValue(FIELD_ATTACHMENT) != null) doc.Attachment = receivedForm.GetFileFieldValue(FIELD_ATTACHMENT).Name;

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

         /// TODO: Tratamiento de los archivos adjuntos

         // Redirige a la página del artículo
         Redirect(ContentView.GetURL(doc.ID));
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve la URL que permite agregar un contenido a una determinada carpeta.
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
      /// Devuelve la URL que permite editar un determinado contenido.
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
