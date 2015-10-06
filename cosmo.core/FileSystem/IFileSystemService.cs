using System.Collections.Generic;
using System.IO;

namespace Cosmo.FileSystem
{

   #region Enumerations

   /// <summary>
   /// Carpetas de un workspace
   /// </summary>
   public enum WorkspaceFolders : int
   {
      /// <summary>Directorio raíz de instalación en las estaciones cliente.</summary>
      ClientRoot = 0,

      /// <summary>Carpeta raíz del servidor web.</summary>
      ServerRoot = 100,
      /// <summary>Carpeta \App_Browsers (.NET).</summary>
      ServerAppBrowsers = 101,
      /// <summary>Carpeta \App_Code (.NET).</summary>
      ServerAppCode = 102,
      /// <summary>Carpeta \App_Data (.NET).</summary>
      ServerAppData = 103,
      /// <summary>Carpeta \App_LocalResources (.NET).</summary>
      ServerAppLocalResources = 104,
      /// <summary>Carpeta \App_GlobalResources (.NET).</summary>
      ServerAppGlobalResources = 105,
      /// <summary>Carpeta \App_Themes (.NET).</summary>
      ServerAppThemes = 106,
      /// <summary>Carpeta \App_WebReferences (.NET).</summary>
      ServerAppWebReferences = 107,
      /// <summary>Carpeta \bin (.NET).</summary>
      ServerBin = 108,
      /// <summary>Carpeta \images usada para poner las imágenes que no forman parte de ninguna plantilla.</summary>
      ServerImages = 109,
      /// <summary>Carpeta \includes.</summary>
      ServerInclude = 110,
      /// <summary>Carpeta raíz del sistema de archivos del workspace (usualmente \docs).</summary>
      ServerWorkspaceFileStorageRoot = 111,
      /// <summary>Carpeta para ubicar archivos temporales.</summary>
      ServerTemporaryStorage = 112,

      /// <summary>Carpeta para una plantilla de presentación \webserver\Templates\%TEMPLATE_ID%.</summary>
      ServerTemplatesPrivate = 200,
      /// <summary>Carpeta de recursos compartidos para plantillas de presentación.</summary>
      ServerTemplatesShared = 201,
      /// <summary>Carpeta raíz para las plantillas (en teoria, nunca deberá haber ningún archivo aquí).</summary>
      ServerTemplatesRoot = 201,

      /// <summary>
      /// Carpeta indfefinida
      /// </summary>
      Unknown = 9999
   }

   #endregion

   /// <summary>
   /// Declaración de la clase que deben implementar todos los módulos del servicio FileSystem.
   /// </summary>
   public abstract class IFileSystemService
   {
      // Internal data declarations
      private Workspace _ws = null;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="IFileSystemService"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/>.</param>
      protected IFileSystemService(Workspace workspace)
      {
         _ws = workspace;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve la instancia de <see cref="Workspace"/> usada en el módulo.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
      }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Devuelve la ruta raíz de la aplicación.
      /// </summary>
      public abstract string ApplicationPath { get; }

      /// <summary>
      /// Obtiene la ruta física a la carpeta de un determinado servicio.
      /// </summary>
      /// <param name="serviceFolderName">Identificador del objeto.</param>
      /// <returns>La ruta física en el servidor web a la carpeta solicitada.</returns>
      public abstract string GetServicePath(string serviceFolderName);

      /// <summary>
      /// Gets the path (without filename) to a object folder.
      /// </summary>
      /// <param name="objectId">Object unique identifier.</param>
      /// <param name="filename">Nombre del archivo (sin ruta).</param>
      /// <returns>A string containing the requested path.</returns>
      public abstract string GetFilePath(string objectId);

      /// <summary>
      /// Gets the path (with filename) to a object related file.
      /// </summary>
      /// <param name="objectId">Object unique identifier.</param>
      /// <param name="filename">Filename without path.</param>
      /// <returns>A string containing the requested filename and path.</returns>
      public abstract string GetFilePath(string objectId, string filename);

      /// <summary>
      /// Obtiene la URL a un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin ruta).</param>
      /// <returns>La URL al archivo solicitado.</returns>
      public abstract string GetFileURL(string objectId, string filename);

      /// <summary>
      /// Obtiene la URL a un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin ruta).</param>
      /// <param name="relativeUrl">Indica si se desea obtener una URL relativa.</param>
      /// <returns>La URL al archivo solicitado.</returns>
      public abstract string GetFileURL(string objectId, string filename, bool relativeUrl);

      /// <summary>
      /// Obtiene la ruta física a la carpeta contenedora de un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <returns>La ruta al directorio privado de un objeto.</returns>
      public abstract string GetObjectFolder(string objectId);

      /// <summary>
      /// Obtiene una lista con los archivos asociados a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <returns>Una lista de instancias de <see cref="FileInfo"/> que representan los archivos asociados al objeto.</returns>
      public abstract List<FileInfo> GetObjectFiles(string objectId);

      /// <summary>
      /// Delete a file associated to an object.
      /// </summary>
      /// <param name="objectId">Object unique identifier.</param>
      /// <param name="filename">File name without path.</param>
      /// <param name="throwError">Indicates if the method must thrown an error if the file don't exist or if it can be deleted.</param>
      public abstract void DeleteFile(string objectId, string filename, bool throwError);

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve el nombre de una carpeta Cosmo
      /// </summary>
      /// <param name="folder">Identificador de la carpeta</param>
      /// <returns>Una cadena que representa el nombre de la carpeta</returns>
      public static string GetFolderName(WorkspaceFolders folder)
      {
         switch (folder)
         {
            case WorkspaceFolders.ClientRoot: return "[workstation_root]";
            case WorkspaceFolders.ServerRoot: return "[webserver_root]";
            case WorkspaceFolders.ServerInclude: return "[webserver_root]\\Include";
            case WorkspaceFolders.ServerImages: return "[webserver_root]\\Images";
            case WorkspaceFolders.ServerBin: return "[webserver_root]\\Bin";
            case WorkspaceFolders.ServerAppBrowsers: return "[webserver_root]\\App_Browsers";
            case WorkspaceFolders.ServerAppData: return "[webserver_root]\\App_Data";
            case WorkspaceFolders.ServerAppCode: return "[webserver_root]\\App_Code";
            case WorkspaceFolders.ServerAppGlobalResources: return "[webserver_root]\\App_GlobalResources";
            case WorkspaceFolders.ServerAppLocalResources: return "[webserver_root]\\App_LocalResources";
            case WorkspaceFolders.ServerAppThemes: return "[webserver_root]\\App_Themes";
            case WorkspaceFolders.ServerAppWebReferences: return "[webserver_root]\\App_WebReferences";
            case WorkspaceFolders.ServerTemplatesShared: return "[webserver_root]\\Templates\\Shared";
            case WorkspaceFolders.ServerTemplatesPrivate: return "[webserver_root]\\Templates\\[template_id]";
         }
         return "-- ¡Desconocido! --";
      }

      #endregion
   }
}
