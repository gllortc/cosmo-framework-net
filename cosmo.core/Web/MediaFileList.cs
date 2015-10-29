using Cosmo.FileSystem;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.IO;
using System.Reflection;

namespace Cosmo.Web
{

   /// <summary>
   /// Representa la lista de archivos adjuntos de un documento.
   /// </summary>
   [AuthenticationRequired]
   [ViewParameter(ParameterName = Workspace.PARAM_FOLDER_ID,
                  PropertyName = "FolderPath")]
   public class MediaFileList : PartialView
   {
      // Modal element unique identifier
      private const string DOM_ID = "content-edit-file-list";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="MediaFileList"/>.
      /// </summary>
      public MediaFileList()
         : base(MediaFileList.DOM_ID)
      {
         this.FolderPath = string.Empty;
      }

      /// <summary>
      /// Gets an instance of <see cref="MediaFileList"/>.
      /// </summary>
      /// <param name="objectId">File system object identifier.</param>
      public MediaFileList(IFileSystemID objectId) 
         : base(MediaFileList.DOM_ID)
      {
         this.FolderPath = objectId.ToFolderName();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the relative folder which is being listed in tle files list.
      /// </summary>
      public string FolderPath { get; set; }

      #endregion

      #region PartialView Implementation

      public override void InitPage()
      {
         // Check preconditions
         if (string.IsNullOrWhiteSpace(this.FolderPath))
         {
            ShowError("Llamada incorrecta!",
                      "Hemos detectado una llamada incorrecta al editor de artículos. No es posible abrir el editor en estas condiciones.");
         }

         // Generate file list
         TableControl tableFiles = new TableControl(this);
         tableFiles.Bordered = false;
         tableFiles.Condensed = false;
         tableFiles.Hover = true;
         tableFiles.Header = new TableRow("Archivo", "Archivo", "Tamaño (bytes)", "Acciones");
         int fileIdx = 0;
         string rowId;
         ButtonGroupControl btnBar;

         foreach (FileInfo file in Workspace.FileSystemService.GetObjectFiles(this.FolderPath))
         {
            rowId = "file-idx-" + ++fileIdx;

            btnBar = new ButtonGroupControl(this);
            btnBar.Size = ButtonControl.ButtonSizes.Small;
            btnBar.Buttons.Add(new ButtonControl(this, "btnCopyUrl" + fileIdx, "Copiar enlace", "fa-chain", string.Empty, "CopyFileUrl('" + Workspace.FileSystemService.GetFileURL(this.FolderPath, file.Name, true) + "');"));
            btnBar.Buttons.Add(new ButtonControl(this, "btnDownload" + fileIdx, "Descargar", "fa-download", Cosmo.Web.Handlers.FileSystemRestHandler.GetDownloadFileUrl(this.FolderPath, file.Name).ToString(), string.Empty));
            // btnBar.Buttons.Add(new ButtonControl(this, "btnDelete" + fileIdx, "Borrar", "fa-trash-o", string.Empty, "deleteFile('" + rowId + "', '" + objId + "', '" + file.Name + "');"));

            tableFiles.Rows.Add(new TableRow(rowId,
                                             "<a href=\"" + Workspace.FileSystemService.GetFileURL(this.FolderPath, file.Name, true) + "\" target=\"_blank\">" + file.Name + "</a>",
                                             file.Length.ToString(),
                                             Workspace.UIService.Render(btnBar)));
         }
         Content.Add(tableFiles);
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Gets the URL for calling the view.
      /// </summary>
      /// <param name="objectId">Object unique identifier (DB).</param>
      /// <param name="isContainer">Indicate if the object can contains other objects.</param>
      /// <returns>A string representing the requested URL.</returns>
      public static string GetUrl(IFileSystemID objectId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, objectId.ToFolderName());

         return url.ToString(true);
      }

      #endregion

   }
}
