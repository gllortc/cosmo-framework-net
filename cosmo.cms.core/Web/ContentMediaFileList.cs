using Cosmo.Cms.Model.Content;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.IO;
using System.Reflection;

namespace Cosmo.Cms.Web
{

   /// <summary>
   /// Representa la lista de archivos adjuntos de un documento.
   /// </summary>
   [AuthorizationRequired(DocumentDAO.ROLE_CONTENT_EDITOR)]
   [ViewParameter(ParameterName = Cosmo.Workspace.PARAM_OBJECT_ID,
                  PropertyName = "ContentID")]
   [ViewParameter(ParameterName = Cosmo.Workspace.PARAM_COMMAND,
                  PropertyName = "Command")]
   public class ContentMediaFileList : PartialView
   {
      // Modal element unique identifier
      private const string DOM_ID = "content-edit-file-list";

      // Parameter declarations
      private const string PARAM_ISCONTAINER = "ic";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="ContentMediaFileList"/>.
      /// </summary>
      public ContentMediaFileList()
         : base(ContentMediaFileList.DOM_ID)
      {
         this.ContentID = 0;
      }

      /// <summary>
      /// Gets an instance of <see cref="ContentMediaFileList"/>.
      /// </summary>
      /// <param name="cmd">Command type.</param>
      /// <param name="contentId">Thread identifier.</param>
      public ContentMediaFileList(int contentId, string cmd) 
         : base(ContentMediaFileList.DOM_ID)
      {
         this.Command = cmd;
         this.ContentID = contentId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the content ID corresponding to file list.
      /// </summary>
      public int ContentID { get; set; }

      /// <summary>
      /// Gets or sets the command type (edit or new document).
      /// </summary>
      public string Command { get; set; }

      #endregion

      #region PartialView Implementation

      public override void InitPage()
      {
         //-------------------------------
         // Obtención de parámetros
         //-------------------------------

         // Obtiene los parámetros de llamada
         int objId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);
         bool isContainer = Parameters.GetBoolean(ContentMediaFileList.PARAM_ISCONTAINER);
         string fsObjectID = Workspace.FileSystemService.GenerateValidObjectID(objId, isContainer);

         //-------------------------------
         // Obtención de datos
         //-------------------------------

         if (objId <= 0)
         {
            ShowError("Llamada incorrecta!",
                      "Hemos detectado una llamada incorrecta al editor de artículos. No es posible abrir el editor en estas condiciones.");
         }

         TableControl tableFiles = new TableControl(this);
         tableFiles.Bordered = false;
         tableFiles.Condensed = false;
         tableFiles.Hover = true;
         tableFiles.Header = new TableRow("Archivo", "Archivo", "Tamaño (bytes)", "Acciones");
         int fileIdx = 0;
         string rowId;
         ButtonGroupControl btnBar;

         foreach (FileInfo file in Workspace.FileSystemService.GetObjectFiles(fsObjectID))
         {
            rowId = "file-idx-" + ++fileIdx;

            btnBar = new ButtonGroupControl(this);
            btnBar.Size = ButtonControl.ButtonSizes.Small;
            btnBar.Buttons.Add(new ButtonControl(this, "btnCopyUrl" + fileIdx, "Copiar enlace", "fa-chain", string.Empty, "CopyFileUrl('" + Workspace.FileSystemService.GetFileURL(objId.ToString(), file.Name, true) + "');"));
            btnBar.Buttons.Add(new ButtonControl(this, "btnDownload" + fileIdx, "Descargar", "fa-download", Cosmo.Web.Handlers.FileSystemRestHandler.GetDownloadFileUrl(objId.ToString(), file.Name).ToString(), string.Empty));
            btnBar.Buttons.Add(new ButtonControl(this, "btnDelete" + fileIdx, "Borrar", "fa-trash-o", string.Empty, "deleteFile('" + rowId + "', '" + objId + "', '" + file.Name + "');"));

            tableFiles.Rows.Add(new TableRow(rowId,
                                             "<a href=\"" + Workspace.FileSystemService.GetFileURL(objId.ToString(), file.Name) + "\" target=\"_blank\">" + file.Name + "</a>",
                                             file.Length.ToString(),
                                             Workspace.UIService.Render(btnBar)));
         }
         Content.Add(tableFiles);
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve la URL de llamada de la plantilla.
      /// </summary>
      /// <param name="command">Comando de la llamada.</param>
      /// <param name="objectId">Identificador único del objeto.</param>
      /// <returns>Una cadena que contiene la URL de llamada de la plantilla.</returns>
      public static string GetUrl(string command, string objectId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, command);
         url.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, objectId);

         return url.ToString(true);
      }

      #endregion

   }
}
