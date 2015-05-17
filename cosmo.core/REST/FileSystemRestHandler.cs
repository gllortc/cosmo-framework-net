using Cosmo.Net;
using System;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;

namespace Cosmo.REST
{
   /// <summary>
   /// Handler que implementa los servicios REST correspondientes al servicio FileSystem de Cosmo.
   /// </summary>
   public class FileSystemRestHandler : RestHandler
   {
      /// <summary>URL de invocación de la API REST.</summary>
      // public const string URL_REST_SERVICE = "FileSystemApi";

      /// <summary>parámetro de llamada: Nombre de archivo.</summary>
      public const string PARAMETER_FILENAME = "_fn_";

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
               DownloadFile(Parameters.GetString(Cosmo.Workspace.PARAM_OBJECT_ID), 
                            Parameters.GetString(PARAMETER_FILENAME));
               break;

            case COMMAND_DELETE:
               DeleteFile(Parameters.GetString(Cosmo.Workspace.PARAM_OBJECT_ID), 
                          Parameters.GetString(PARAMETER_FILENAME));
               break;

            case COMMAND_UPLOAD:
               UploadFiles(Parameters.GetString(Cosmo.Workspace.PARAM_OBJECT_ID));
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
      public static Url GetDownloadFileUrl(string objectId, string filename)
      {
         return new Url(FileSystemRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_DOWNLOAD)
            .AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, objectId)
            .AddParameter(PARAMETER_FILENAME, filename);
      }

      /// <summary>
      /// Descarga archivo asociado a un objeto.
      /// 
      /// Ejemplo de llamada:
      /// www.company.com/APIFileSystem?_cmd_=_download_&oid=0001&_fn_=filename.txt
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo a descargar.</param>
      private void DownloadFile(string objectId, string filename)
      {
         // Verifica la existencia del archivo
         FileInfo file = new FileInfo(Workspace.FileSystemService.GetFilePath(objectId, filename));

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
      public static Url GetDeleteFileUrl(string objectId, string filename)
      {
         return new Url(FileSystemRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_DELETE)
            .AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, objectId)
            .AddParameter(PARAMETER_FILENAME, filename);
      }

      /// <summary>
      /// Elimina un archivo asociado a un objeto.
      /// 
      /// Ejemplo de llamada:
      /// www.company.com/APIFileSystem?_cmd_=_del_&oid=0001&_fn_=filename.txt
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo a eliminar.</param>
      private void DeleteFile(string objectId, string filename)
      {
         // Inicializaciones
         JavaScriptSerializer json = new JavaScriptSerializer();

         // Verifica la existencia del archivo
         FileInfo file = new FileInfo(Workspace.FileSystemService.GetFilePath(objectId, filename));

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

      #region Command: Upload Files

      /// <summary>Agrega un archivo a un objeto.</summary>
      public const string COMMAND_UPLOAD = "_upl_";

      /// <summary>
      /// Genera una URL válida para eliminar un archivo asociado a un objeto.
      /// </summary>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static Url GetUploadFilesUrl(string objectId)
      {
         return new Url(FileSystemRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_UPLOAD)
            .AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, objectId);
      }

      /// <summary>
      /// Recibe uno o más archivos y los guarda asociados a un determinado objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      private void UploadFiles(string objectId)
      {
         // Inicializaciones
         int savedFiles = 0;
         string file;
         JavaScriptSerializer json = new JavaScriptSerializer();

         string[] keys = Request.Files.AllKeys;

         foreach (string fileKey in keys)
         {
            try
            {
               file = Workspace.FileSystemService.GetFilePath(objectId, Request.Files[fileKey].FileName);
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
            // Envia confirmación al cliente
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            Response.Write(json.Serialize(new
            {
               response = "ok",
               code = "1",
               message = "Se han subido " + savedFiles + " archivo(s) al servidor."
            }));
         }
         else
         {
            // Envia un código de error 
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            Response.Write(json.Serialize(new
            {
               response = "error",
               code = "-1",
               message = "No se ha subido ningún archivo al servidor"
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
