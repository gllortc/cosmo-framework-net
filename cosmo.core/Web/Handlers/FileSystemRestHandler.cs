using Cosmo.FileSystem;
using Cosmo.Net;
using Cosmo.Net.REST;
using System;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;

namespace Cosmo.Web.Handlers
{
   /// <summary>
   /// Handler que implementa los servicios REST correspondientes al servicio FileSystem de Cosmo.
   /// </summary>
   public class FileSystemRestHandler : RestHandler
   {
      /// <summary>parámetro de llamada: Nombre de archivo.</summary>
      public const string PARAM_FILENAME = "_fn_";

      #region RestHandler Implementation

      /// <summary>
      /// Método invocado al recibir una petición.
      /// </summary>
      /// <param name="command">Comando a ejecutar pasado mediante el parámetro definido mediante <see cref="Cosmo.Workspace.PARAM_COMMAND"/>.</param>
      public override void ServiceRequest(string command)
      {
         switch (command)
         {
            case COMMAND_DOWNLOAD:
               DownloadFile(Parameters.GetString(Cosmo.Workspace.PARAM_FOLDER_ID),
                            Parameters.GetString(FileSystemRestHandler.PARAM_FILENAME));
               break;

            case COMMAND_DELETE:
               DeleteFile(Parameters.GetString(Cosmo.Workspace.PARAM_FOLDER_ID),
                          Parameters.GetString(FileSystemRestHandler.PARAM_FILENAME));
               break;

            default:
               break;
         }
      }

      #endregion

      #region Command: Download File

      /// <summary>Comando para descargar un fichero asociado a un objeto.</summary>
      public const string COMMAND_DOWNLOAD = "_download_";

      /// <summary>
      /// Genera una URL válida para descargar un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin path o ruta).</param>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static Url GetDownloadFileUrl(IFileSystemID objectId, string filename)
      {
         return new Url(FileSystemRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_DOWNLOAD)
            .AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, objectId.ToFolderName())
            .AddParameter(FileSystemRestHandler.PARAM_FILENAME, filename);
      }

      /// <summary>
      /// Genera una URL válida para descargar un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin path o ruta).</param>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static Url GetDownloadFileUrl(string relativePath, string filename)
      {
         return new Url(FileSystemRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_DOWNLOAD)
            .AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, relativePath)
            .AddParameter(FileSystemRestHandler.PARAM_FILENAME, filename);
      }

      /// <summary>
      /// Descarga archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo a descargar.</param>
      /// <remarks>
      /// Ejemplo de llamada:<br />
      /// <c>www.company.com/APIFileSystem?_cmd_=_download_&oid=0001&_fn_=filename.txt</c>
      /// </remarks>
      private void DownloadFile(string relativePath, string filename)
      {
         // Verifica la existencia del archivo
         FileInfo file = new FileInfo(Workspace.FileSystemService.GetFilePath(relativePath, filename));

         if (file.Exists)
         {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
         }
         else
         {
            // Envia un código de error 404
            Response.Clear();
            Response.StatusCode = 404;
            Response.End();
         }
      }

      #endregion

      #region Command: Delete File

      /// <summary>Comando para eliminar un fichero asociado a un objeto.</summary>
      public const string COMMAND_DELETE = "_del_";

      /// <summary>
      /// Genera una URL válida para eliminar un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin path o ruta).</param>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static Url GetDeleteFileUrl(IFileSystemID objectId, string filename)
      {
         return new Url(FileSystemRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_DELETE)
            .AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, objectId.ToFolderName())
            .AddParameter(PARAM_FILENAME, filename);
      }

      /// <summary>
      /// Elimina un archivo asociado a un objeto.
      /// 
      /// Ejemplo de llamada:
      /// www.company.com/APIFileSystem?_cmd_=_del_&oid=0001&_fn_=filename.txt
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo a eliminar.</param>
      private void DeleteFile(string folder, string filename)
      {
         // Inicializaciones
         JavaScriptSerializer json = new JavaScriptSerializer();

         // Verifica la existencia del archivo
         FileInfo file = new FileInfo(Workspace.FileSystemService.GetFilePath(folder, filename));

         try
         {
            // Elimina el archivo físicamente
            file.Delete();

            // Envia confirmación al cliente
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            Response.Write(json.Serialize(new
            {
               response = "ok",
               code = "1",
               message = "Se ha eliminado el archivo " + filename + " correctamente."
            }));
         }
         catch (Exception ex)
         {
            // Envia un código de error 
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            Response.Write(json.Serialize(new
            {
               response = "error",
               code = "-1",
               message = ex.Message
            }));
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve la URL a la que se debe atacar para realizar operaciones REST sobre el servicio.
      /// </summary>
      public static string ServiceUrl
      {
         get { return MethodBase.GetCurrentMethod().DeclaringType.Name; }
      }

      #endregion

   }
}
