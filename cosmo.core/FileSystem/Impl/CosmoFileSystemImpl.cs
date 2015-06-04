using Cosmo.Net;
using Cosmo.Utils;
using Cosmo.Utils.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cosmo.FileSystem.Impl
{
   /// <summary>
   /// Implementa una clase para la gestión del sistema de archivos del workspace.
   /// </summary>
   public class CosmoFileSystemImpl : IFileSystemService
   {
      // Declaración de variables internas
      private string _wsPath;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="Cosmo.Workspace.FileSystemService"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/>.</param>
      /// <param name="plugin">Contiene toda la información de configuración del módulo.</param>
      public CosmoFileSystemImpl(Workspace workspace, Plugin plugin)
         : base(workspace)
      {
         _wsPath = plugin.Settings.Get("path");
         this.RootFolderName = plugin.Settings.Get("root"); // workspace.Settings.GetString(WorkspaceSettingsKeys.WorkspaceFSPath, "docs");
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el nombre de la carpeta que contiene el sistema de archivos del workspace.
      /// </summary>
      /// <remarks>
      /// Usualmente suele ser <c>docs</c>.
      /// El sistema de archivos permite a los servicios gestionar archivos (documentos, imágenes, etc) que contengan datos de los mismos.
      /// </remarks>
      public string RootFolderName { get; set; }

      /// <summary>
      /// Devuelve la URL al sistema de archivos de datos del workspace.
      /// </summary>
      public string RootUrl
      {
         get { return Cosmo.Net.Url.Combine(Workspace.Url, this.RootFolderName); }
      }

      /// <summary>
      /// Devuelve la ruta raíz de la aplicación.
      /// </summary>
      public override string ApplicationPath
      {
         get { return RootPath; }
      }

      /// <summary>
      /// Devuelve la ruta al sistema de archivos de datos del workspace (usualmente [WebServerPath]/docs).
      /// </summary>
      public string RootPath
      {
         get { return System.IO.Path.Combine(_wsPath, this.RootFolderName); }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Averigua si existe un determinado archivo.
      /// </summary>
      /// <param name="filename">Nombre del archivo sin ruta.</param>
      /// <param name="folders">Carpetas de acceso.</param>
      /// <returns><c>True</c> si existe el archivo o <c>False</c> en cualquier otro caso.</returns>
      public bool FileExist(string filename, params string[] folders)
      {
         FileInfo file = this.FileInformation(filename, folders);
         return file.Exists;
      }

      /// <summary>
      /// Obtiene las propiedades de un archivo.
      /// </summary>
      /// <param name="filename">Nombre del archivo sin ruta.</param>
      /// <param name="folders">Carpetas de acceso.</param>
      /// <returns>Una instancia de <see cref="FileInfo"/> que contiene las priopiedades del archivo.</returns>
      public FileInfo FileInformation(string filename, params string[] folders)
      {
         string path = RootPath;

         if (folders.Length == 1)
         {
            path = folders[0];
         }
         else
         {
            string temp = folders[0].Trim();
            for (int i = 1; i < folders.Length; i++)
            {
               temp = Path.Combine(temp, folders[i]);
            }

            path = temp;
         }

         return new FileInfo(PathInfo.Combine(RootPath, path, filename));
      }

      /// <summary>
      /// Crea una carpeta en el sistema de archivos del workspace.
      /// </summary>
      /// <param name="filename">Archivo que se desea copiar en el sistema de archivos.</param>
      /// <remarks>
      /// Si la carpeta existe no hace nada.
      /// </remarks>
      public void FileCopy(string filename)
      {
         FileInfo file = new FileInfo(filename);
         file.CopyTo(RootPath);
      }

      /// <summary>
      /// Crea una carpeta en el sistema de archivos del workspace.
      /// </summary>
      /// <param name="filename">Archivo que se desea copiar al sistema de archivos.</param>
      /// <param name="destinationFolderName">Nombre de la carpeta dónde se desea incorporar el archivo.</param>
      /// <remarks>
      /// Si la carpeta existe no hace nada.
      /// </remarks>
      public void FileCopy(string filename, string destinationFolderName)
      {
         DirectoryInfo directory = new DirectoryInfo(Path.Combine(RootPath, destinationFolderName));
         if (!directory.Exists) directory.Create();

         FileInfo file = new FileInfo(filename);
         file.CopyTo(Path.Combine(directory.FullName, file.Name));
      }

      /// <summary>
      /// Crea una carpeta en el sistema de archivos del workspace.
      /// </summary>
      /// <param name="name">Nombre de la carpeta a crear.</param>
      /// <remarks>
      /// Si la carpeta existe no hace nada.
      /// </remarks>
      public void DirectoryCreate(string name)
      {
         DirectoryInfo directory = new DirectoryInfo(Path.Combine(RootPath, name));
         if (!directory.Exists) directory.Create();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DirectoryInfo"/> que representa la carpeta temporal.
      /// </summary>
      public DirectoryInfo GetTemporaryFolder()
      {
         DirectoryInfo tempdir = new DirectoryInfo(Path.Combine(RootPath, "temp"));
         if (!tempdir.Exists) tempdir.Create();

         return tempdir;
      }

      #endregion

      #region IFileSystemService Implementation

      /// <summary>
      /// Obtiene la ruta física a la carpeta de un determinado servicio.
      /// </summary>
      /// <param name="serviceFolderName">Identificador del objeto.</param>
      /// <returns>La ruta física en el servidor web a la carpeta solicitada.</returns>
      public override string GetServicePath(string serviceFolderName)
      {
         return Path.Combine(RootPath, serviceFolderName);
      }

      /// <summary>
      /// Obtiene la ruta física a un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin ruta).</param>
      /// <returns>La ruta al archivo solicitado.</returns>
      public override string GetFilePath(string objectId, string filename)
      {
         return Path.Combine(Path.Combine(RootPath, objectId), filename);
      }

      /// <summary>
      /// Obtiene la URL a un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin ruta).</param>
      /// <returns>La URL al archivo solicitado.</returns>
      public override string GetFileURL(string objectId, string filename)
      {
         return GetFileURL(objectId, filename, false);
      }

      /// <summary>
      /// Obtiene la URL a un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin ruta).</param>
      /// <param name="relativeUrl">Indica si se desea obtener una URL relativa.</param>
      /// <returns>La URL al archivo solicitado.</returns>
      public override string GetFileURL(string objectId, string filename, bool relativeUrl)
      {
         if (relativeUrl)
         {
            return Url.Combine(Url.Combine(RootFolderName, objectId), filename);
         }
         else
         {
            return Url.Combine(Url.Combine(RootUrl, objectId), filename);
         }
      }

      /// <summary>
      /// Obtiene la ruta física a la carpeta contenedora de un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <returns>La ruta al directorio privado de un objeto.</returns>
      public override string GetObjectFolder(string objectId)
      {
         return Path.Combine(RootPath, objectId);
      }

      /// <summary>
      /// Obtiene una lista con los archivos asociados a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <returns>Una lista de instancias de <see cref="FileInfo"/> que representan los archivos asociados al objeto.</returns>
      public override List<FileInfo> GetObjectFiles(string objectId)
      {
         List<FileInfo> files = new List<FileInfo>();

         // Obtiene la carpeta física
         DirectoryInfo dir = new DirectoryInfo(GetObjectFolder(objectId));

         // Agrega los archivos si existe el objeto (o su carpeta contenedora)
         if (dir.Exists)
         {
            files.AddRange(dir.GetFiles());
         }

         return files;
      }

      /// <summary>
      /// Delete a file associated to an object.
      /// </summary>
      /// <param name="objectId">Object unique identifier.</param>
      /// <param name="filename">File name without path.</param>
      /// <param name="throwError">Indicates if the method must thrown an error if the file don't exist or if it can be deleted.</param>
      public override void DeleteFile(string objectId, string filename, bool throwError)
      {
         try
         {
            string path = Path.Combine(Path.Combine(RootPath, objectId), filename);
            File.Delete(path);
         }
         catch (Exception ex)
         {
            if (throwError)
            {
               throw ex;
            }
         }
      }

      #endregion

   }
}
