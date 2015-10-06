﻿using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cosmo.FileSystem
{
   /// <summary>
   /// Implementa una clase para la gestión del sistema de archivos del workspace.
   /// </summary>
   public class FileSystemService
   {
      // Internal data declarations
      private Workspace _ws = null;
      private IFileSystemService _controller = null;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="Cosmo.Workspace.FileSystemService"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/>.</param>
      public FileSystemService(Workspace workspace)
      {
         // Inicializaciones
         _ws = workspace;
         // _folderName = _ws.Settings.GetString(WorkspaceSettingsKeys.WorkspaceFSPath, "docs");

         // Carga el controlador seleccionado
         if (!string.IsNullOrWhiteSpace(_ws.Settings.FileSystemControllers.DefaultPluginId))
         {
            LoadController();
         }
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el nombre (ID) del módulo activo de gestion de archivos.
      /// </summary>
      public string ActiveControllerId
      {
         get { return _ws.Settings.FileSystemControllers.DefaultPluginId; }
      }

      /// <summary>
      /// Devuelve la ruta raíz de la aplicación.
      /// </summary>
      public string ApplicationPath
      {
         get { return _controller.ApplicationPath; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene la ruta física a la carpeta de un determinado servicio.
      /// </summary>
      /// <param name="serviceFolderName">Identificador del objeto.</param>
      /// <returns>La ruta física en el servidor web a la carpeta solicitada.</returns>
      public string GetServicePath(string serviceFolderName)
      {
         return _controller.GetServicePath(serviceFolderName);
      }

      /// <summary>
      /// Gets the path (without filename) to a object folder.
      /// </summary>
      /// <param name="objectId">Object unique identifier.</param>
      /// <param name="filename">Nombre del archivo (sin ruta).</param>
      /// <returns>A string containing the requested path.</returns>
      public string GetFilePath(string objectId)
      {
         return _controller.GetFilePath(objectId);
      }

      /// <summary>
      /// Gets the path (with filename) to a object related file.
      /// </summary>
      /// <param name="objectId">Object unique identifier.</param>
      /// <param name="filename">Filename without path.</param>
      /// <returns>A string containing the requested filename and path.</returns>
      public string GetFilePath(string objectId, string filename)
      {
         return _controller.GetFilePath(objectId, filename);
      }

      /// <summary>
      /// Obtiene la URL a un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin ruta).</param>
      /// <returns>La URL al archivo solicitado.</returns>
      public string GetFileURL(string objectId, string filename)
      {
         return _controller.GetFileURL(objectId, filename);
      }

      /// <summary>
      /// Obtiene la URL a un archivo asociado a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo (sin ruta).</param>
      /// <param name="relativeUrl">Indica si se desea obtener una URL relativa.</param>
      /// <returns>La URL al archivo solicitado.</returns>
      public string GetFileURL(string objectId, string filename, bool relativeUrl)
      {
         return _controller.GetFileURL(objectId, filename, relativeUrl);
      }

      /// <summary>
      /// Obtiene la ruta física a la carpeta contenedora de un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <returns>La ruta al directorio privado de un objeto.</returns>
      public string GetObjectFolder(string objectId)
      {
         return _controller.GetObjectFolder(objectId);
      }

      /// <summary>
      /// Obtiene una lista con los archivos asociados a un objeto.
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <returns>Una lista de instancias de <see cref="FileInfo"/> que representan los archivos asociados al objeto.</returns>
      public List<FileInfo> GetObjectFiles(string objectId)
      {
         return _controller.GetObjectFiles(objectId);
      }

      /// <summary>
      /// Delete a file associated to an object.
      /// </summary>
      /// <param name="objectId">Object unique identifier.</param>
      /// <param name="filename">File name without path.</param>
      /// <param name="throwError">Indicates if the method must thrown an error if the file don't exist or if it can be deleted.</param>
      public void DeleteFile(string objectId, string filename, bool throwError)
      {
         _controller.DeleteFile(objectId, filename, throwError);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Carga el módulo controlador del sistema de archivos configurado.
      /// </summary>
      private void LoadController()
      {
         Type type = null;
         Plugin plugin = _ws.Settings.FileSystemControllers.GetPlugin(_ws.Settings.FileSystemControllers.DefaultPluginId);

         if (plugin != null)
         {
            Object[] args = new Object[2];
            args[0] = _ws;
            args[1] = plugin;

            type = Type.GetType(plugin.Class, true, true);
            _controller = (IFileSystemService)Activator.CreateInstance(type, args);
         }
      }

      #endregion

   }
}
