using Cosmo.Cms.Content;
using Cosmo.Net;
using Cosmo.REST;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.WebApp.FileSystemServices;
using System.IO;

namespace Cosmo.WebApp.Content
{
   /// <summary>
   /// Representa la lista de archivos adjuntos de un documento.
   /// </summary>
   [AuthorizationRequired(DocumentDAO.ROLE_CONTENT_EDITOR)]
   public class ContentEditFileList : PartialViewContainer
   {

      public override void InitPage()
      {
         Document doc = null;
         DocumentFolder folder;

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

         switch (Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND))
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
         // Habilita formularios modales
         //-------------------------------
         ModalFormUpload frmUpload = new ModalFormUpload(doc.ID);
         Modals.Add(frmUpload);

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         HtmlContentControl filesContent = new HtmlContentControl(this);
         filesContent.AppendParagraph("La siguiente lista contiene los archivos adjuntos al contenido.");
         Content.Add(filesContent);

         ButtonGroupControl btnFiles = new ButtonGroupControl(this);
         btnFiles.Size = ButtonControl.ButtonSizes.Small;
         btnFiles.Buttons.Add(new ButtonControl(this, "cmdAddFiles", "Agregar archivos", frmUpload));
         btnFiles.Buttons.Add(new ButtonControl(this, "cmdRefresh", "Actualizar", IconControl.ICON_REFRESH, "#", "cosmoUIServices.loadTemplate();"));
         Content.Add(btnFiles);

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
            btnBar.Size = ButtonControl.ButtonSizes.Small;
            btnBar.Buttons.Add(new ButtonControl(this, "btnCopyUrl" + fileIdx, "Copiar enlace", "fa-chain", string.Empty, "CopyFileUrl('" + Workspace.FileSystemService.GetFileURL(doc.ID.ToString(), file.Name, true) + "');"));
            btnBar.Buttons.Add(new ButtonControl(this, "btnDownload" + fileIdx, "Descargar", "fa-download", FileSystemRestHandler.GetDownloadFileUrl(doc.ID.ToString(), file.Name).ToString(), string.Empty));
            btnBar.Buttons.Add(new ButtonControl(this, "btnDelete" + fileIdx, "Borrar", "fa-trash-o", string.Empty, "deleteFile('" + rowId + "', '" + doc.ID + "', '" + file.Name + "');"));

            tableFiles.Rows.Add(new TableRow(rowId,
                                             "<a href=\"" + Workspace.FileSystemService.GetFileURL(doc.ID.ToString(), file.Name) + "\" target=\"_blank\">" + file.Name + "</a>",
                                             file.Length.ToString(),
                                             Workspace.UIService.Render(btnBar)));
         }
         Content.Add(tableFiles);
      }

      public override void LoadPage()
      {
         // throw new System.NotImplementedException();
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // throw new System.NotImplementedException();
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      /// <summary>
      /// Devuelve la URL de llamada de la plantilla.
      /// </summary>
      /// <param name="command">Comando de la llamada.</param>
      /// <param name="objectId">Identificador único del objeto.</param>
      /// <returns>Una cadena que contiene la URL de llamada de la plantilla.</returns>
      public static string GetTemplateUrl(string command, string objectId)
      {
         Url url = new Url("ContentEditFileList");
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, command);
         url.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, objectId);

         return url.ToString(true);
      }

   }
}
