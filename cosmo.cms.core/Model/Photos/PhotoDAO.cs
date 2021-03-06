using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Cosmo.Cms.Model.Photos
{
   /// <summary>
   /// Implementa la capa de datos del servicio de galerias de fotografias.
   /// </summary>
   public class PhotoDAO
   {
      // Internal data declarations
      private Workspace _ws;

      /// <summary>Nombre del servicio</summary>
      public const string SERVICE_NAME = "Fotos";

      /// <summary>Nombre de la carpeta del servicio dentro de la ra�z del sistema de archivos de aplicaciones del workspace</summary>
      public const string SERVICE_FOLDER = "imgdb";

      /// <summary>Role for managing and publishing photos.</summary>
      public const string ROLE_PHOTOS_EDITOR = "content.editor";

      // Vistas del servicio
      private const string URL_HOME = "PhotosBrowse";
      private const string URL_ADD = "PhotosUpload";
      private const string URL_FOLDER = "PhotosByFolder";
      private const string URL_BYUSER = "PhotosByUser";
      private const string URL_RECENT = "PhotosRecent";

      // Variables de configuraci�n
      private const string SETUP_SETTING_THUMBMAXWIDTH = "cs.photos.gallery.thumbwith";
      private const string SETUP_SETTING_GALLERYCOLS = "cs.photos.gallery.columns";
      private const string SETUP_SETTING_USERSCANUPLOAD = "cs.photos.upload.enabled";
      private const string SETUP_SETTING_UPLOADMAXLENGTH = "cs.photos.upload.maxlength";
      private const string SETUP_SETTING_ALLOWEDFILEEXT = "cs.photos.upload.allowedext";

      // Fragmentos SQL reaprovechables
      private const string SQL_SELECT_OBJECT = "imgid,imfolder,imgtemplate,imgfile,imgwidth,imgheight,imgthumb,imgthwidth,imgthheigth,imgdesc,imgauthory,imguserid,imgdate,imgshows";
      private const string SQL_SELECT_FOLDER = "ifid,ifparentid,ifname,ifhtml,iflink,iforder,ifenabled,ifowner,iffilepattern";
      private const string SQL_INSERT_FOLDER = "ifparentid,ifimage,ifimgwidth,ifimgheight,ifname,ifhtml,iflink,iforder,ifenabled,ifowner,iffilepattern";
      private const string SQL_TABLE_FOLDERS = "imagefolders";
      private const string SQL_TABLE_IMAGES = "images";
      private const string SQL_TABLE_DOCIMAGES = "CMS_DOCSIMAGES";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PhotoDAO"/>.
      /// </summary>
      public PhotoDAO(Workspace ws)
      {
         Initialize();

         _ws = ws;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el n�mero de columnas de las galerias de fotografias.
      /// </summary>
      public int GalleryColumnsCount
      {
         get { return _ws.Settings.GetInt(PhotoDAO.SETUP_SETTING_GALLERYCOLS, 3); }
      }

      /// <summary>
      /// Devuelve el ancho (en p�xeles) que tendr�n las im�genes miniatura generadas a partir de una imagen original subida por un usuario.
      /// </summary>
      public int GalleryThumbnailWidth
      {
         get { return _ws.Settings.GetInt(PhotoDAO.SETUP_SETTING_THUMBMAXWIDTH, 100); }
      }

      /// <summary>
      /// Devuelve el tama�o m�ximo (en Kb) de los archivos correspondientes a im�genes.
      /// </summary>
      public int FileMaxLength
      {
         get { return _ws.Settings.GetInt(PhotoDAO.SETUP_SETTING_UPLOADMAXLENGTH, 2048); }
      }

      /// <summary>
      /// Devuelve una lista de las extensiones permitidas (en formato ".jpg" y en min�sculas)
      /// </summary>
      public List<string> FileAllowedExtensions
      {
         get
         {
            List<string> extensions = new List<string>();

            foreach (string ext in _ws.Settings.GetString(PhotoDAO.SETUP_SETTING_ALLOWEDFILEEXT).Split(','))
            {
               string fext = ext.Trim().ToLower();
               if (!string.IsNullOrWhiteSpace(fext))
               {
                  if (!fext.StartsWith(".")) fext = "." + fext;
                  extensions.Add(fext);
               }
            }

            return extensions;
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Adds a new folder into the photos service.
      /// </summary>
      /// <param name="folder">Folder to create.</param>
      public void AddFolder(PhotoFolder folder)
      {
         string sql = string.Empty;

         try
         {
            _ws.DataSource.Connect();

            sql = @"INSERT INTO " + SQL_TABLE_FOLDERS + @" (" + SQL_INSERT_FOLDER + @") 
                    VALUES (@ifparentid, '', 0, 0, @ifname, @ifhtml, @iflink, @iforder, @ifenabled, @ifowner, @iffilepattern)";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@ifparentid", folder.ParentID));
               cmd.Parameters.Add(new SqlParameter("@ifname", folder.Name));
               cmd.Parameters.Add(new SqlParameter("@ifhtml", folder.Description));
               cmd.Parameters.Add(new SqlParameter("@iflink", folder.IsContainer));
               cmd.Parameters.Add(new SqlParameter("@iforder", folder.Order));
               cmd.Parameters.Add(new SqlParameter("@ifenabled", (folder.Status == CmsPublishStatus.PublishStatus.Published)));
               cmd.Parameters.Add(new SqlParameter("@ifowner", _ws.CurrentUser.IsAuthenticated ? _ws.CurrentUser.User.Login : SecurityService.ACCOUNT_SUPER));
               cmd.Parameters.Add(new SqlParameter("@iffilepattern", folder.FilePattern));
               cmd.ExecuteNonQuery();
            }

            // Get the unique identifier assigned to the object
            sql = @"SELECT Max(ifid) 
                    FROM   " + SQL_TABLE_FOLDERS;

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               folder.ID = (int)cmd.ExecuteScalar();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "AddFolder", ex);
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Update a folder for the photos service.
      /// </summary>
      /// <param name="folder">Folder to update.</param>
      public void UpdateFolder(PhotoFolder folder)
      {
         string sql = string.Empty;

         try
         {
            _ws.DataSource.Connect();

            sql = @"UPDATE " + SQL_TABLE_FOLDERS + @" 
                    SET    ""ifparentid"" = @ifparentid, 
                           ""ifname"" = @ifname, 
                           ""ifhtml"" = @ifhtml, 
                           ""iflink"" = @iflink,
                           ""iforder"" = @iforder, 
                           ""ifenabled"" = @ifenabled, 
                           ""iffilepattern"" = @iffilepattern)";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@ifparentid", folder.ParentID));
               cmd.Parameters.Add(new SqlParameter("@ifname", folder.Name));
               cmd.Parameters.Add(new SqlParameter("@ifhtml", folder.Description));
               cmd.Parameters.Add(new SqlParameter("@iflink", folder.IsContainer));
               cmd.Parameters.Add(new SqlParameter("@iforder", folder.Order));
               cmd.Parameters.Add(new SqlParameter("@ifenabled", (folder.Status == CmsPublishStatus.PublishStatus.Published)));
               cmd.Parameters.Add(new SqlParameter("@iffilepattern", folder.FilePattern));
               cmd.ExecuteNonQuery();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "UpdateFolder", ex);
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Gets a folder.
      /// </summary>
      /// <param name="folderId">Folder unique identifier.</param>
      /// <param name="getSubfolders">A value indicating if the method must get all subfolders.</param>
      /// <returns>The requested instance of <see cref="PhotoFolder"/> or <c>null</c> if the folder doesn't exist.</returns>
      public PhotoFolder GetFolder(int folderId, bool getSubfolders)
      {
         string sql = string.Empty;
         PhotoFolder folder = null;

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT " + SQL_SELECT_FOLDER + @",
                           (SELECT  Count(*) 
                            FROM    " + SQL_TABLE_IMAGES + @" 
                            WHERE   imfolder = @ifid) As items 
                    FROM   " + SQL_TABLE_FOLDERS + @" 
                    WHERE  ifid = @ifid";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@ifid", folderId));
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  if (reader.Read())
                  {
                     folder = ReadFolder(reader, true);
                  }
               }
            }

            // Get subfolders
            if (getSubfolders && (folder != null))
            {
               folder.Subfolders = this.GetFolders(folder.ID);
            }

            return folder;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetFolder", ex);
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Gets all folder contained in a specified folder.
      /// </summary>
      /// <param name="parentId">Parent folder unique identifier (DB).</param>
      /// <returns>The requested folders list.</returns>
      /// <remarks>If <c>0</c> is provided as a parameter value, all root folders will be returned.</remarks>
      public List<PhotoFolder> GetFolders(int parentId)
      {
         string sql = string.Empty;
         List<PhotoFolder> folders = new List<PhotoFolder>();

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT    " + SQL_SELECT_FOLDER + @" 
                    FROM      " + SQL_TABLE_FOLDERS + @" 
                    WHERE     ifenabled = 1 And 
                              ifparentid = @ifparentid 
                    ORDER BY  iforder  Asc, 
                              ifname   Asc";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@ifparentid", parentId));
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     folders.Add(ReadFolder(reader, false));
                  }
               }
            }

            // Get the number of object contained in each folder
            foreach (PhotoFolder pfolder in folders)
            {
               folders[folders.IndexOf(pfolder)].Objects = this.GetFolderItems(pfolder.ID);
            }

            return folders;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetFolders", ex);
            return null;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Het all root folders.
      /// </summary>
      /// <returns>The requested list of folders.</returns>
      public List<PhotoFolder> GetFolders()
      {
         return this.GetFolders(0);
      }

      /// <summary>
      /// Obtiene las carpetas que contiene una carpeta concreta. Si se indica 0, obtiene las carpetasd de nivel superior.
      /// </summary>
      /// <param name="parentId">Id de la carpeta de nivel superior.</param>
      /// <returns>Una lista de inst�ncias de la clase CSPicturesFolder.</returns>
      public List<PhotoFolder> GetFoldersTree(int parentId)
      {
         string sql = string.Empty;
         List<PhotoFolder> folders = new List<PhotoFolder>();

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT     " + SQL_SELECT_FOLDER + " " +
                  "FROM       " + SQL_TABLE_FOLDERS + " " +
                  "WHERE      ifenabled = 1 And " +
                  "           ifparentid = @ifparentid " +
                  "ORDER BY   iforder  Asc, " +
                  "           ifname   Asc";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@ifparentid", parentId));
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     folders.Add(ReadFolder(reader, false));
                  }
               }
            }

            // Get all subfolders
            foreach (PhotoFolder pfolder in folders)
            {
               pfolder.Subfolders = GetFoldersTree(pfolder.ID);
            }

            return folders;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetFolders", ex);
            return null;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Get an image by its unique identifier.
      /// </summary>
      /// <param name="pictureId">Picture unique identifier (DB).</param>
      /// <returns>The requested instance or <c>null</c> if picture doesn't exist.</returns>
      [Obsolete("Use the normalized method GetByID() instead of this.")]
      public Photo GetPicture(int pictureId)
      {
         return GetByID(pictureId);
      }

      /// <summary>
      /// Get an image by its unique identifier.
      /// </summary>
      /// <param name="pictureId">Picture unique identifier (DB).</param>
      /// <returns>The requested instance or <c>null</c> if picture doesn't exist.</returns>
      public Photo GetByID(int pictureId)
      {
         string sql = string.Empty;
         Photo picture = null;

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT " + SQL_SELECT_OBJECT + @" 
                    FROM   " + SQL_TABLE_IMAGES + @" 
                    WHERE  imgid = @imgid";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@imgid", pictureId));
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     picture = ReadPicture(reader);
                  }
               }
            }

            return picture;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetPicture", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Get all pictures contained in a folder.
      /// </summary>
      /// <param name="folderId">Folder unique identifier (DB).
      /// <returns>The requested list of pictures.</returns>
      [Obsolete("Use the normalized method GetByFolderID() instead of this.")]
      public List<Photo> GetPictures(int folderId)
      {
         return GetByFolderID(folderId);
      }

      /// <summary>
      /// Get all pictures contained in a folder.
      /// </summary>
      /// <param name="folderId">Folder unique identifier (DB).
      /// <returns>The requested list of pictures.</returns>
      public List<Photo> GetByFolderID(int folderId)
      {
         string sql = string.Empty;
         List<Photo> pictures = new List<Photo>();

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT    " + SQL_SELECT_OBJECT + @" 
                    FROM      " + SQL_TABLE_IMAGES + @" 
                    WHERE     imfolder = @imfolder 
                    ORDER BY  imgdate Desc";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@imfolder", folderId));
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     pictures.Add(ReadPicture(reader));
                  }
               }
            }

            return pictures;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetByFolderID", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Get the most recent added photos.
      /// </summary>
      /// <param name="number">The number of objects to read.</param>
      /// <returns>The requested list of pictures.</returns>
      public List<Photo> GetLatest(int number)
      {
         string sql = string.Empty;
         List<Photo> pictures = new List<Photo>();

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT    TOP " + number + " " + SQL_SELECT_OBJECT + @" 
                    FROM      " + SQL_TABLE_IMAGES + @" 
                    ORDER BY  imgdate DESC";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     pictures.Add(ReadPicture(reader));
                  }
               }
            }

            return pictures;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetLatest", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Get the most recent added photos.
      /// </summary>
      /// <param name="fromDate">Date.</param>
      /// <returns>The requested list of pictures.</returns>
      public List<Photo> GetLatest(DateTime fromDate)
      {
         string sql = string.Empty;
         List<Photo> pictures = new List<Photo>();

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT    " + SQL_SELECT_OBJECT + @" 
                    FROM      " + SQL_TABLE_IMAGES + @" 
                    WHERE     imgdate >= @creationDate 
                    ORDER BY  imgdate Desc";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@creationDate", fromDate));
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     pictures.Add(ReadPicture(reader));
                  }
               }
            }

            return pictures;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetLatest", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Recupera las im�genes creadas por un determinado usuario
      /// </summary>
      /// <param name="uid">Identificador del usuario autor de las imagenes</param>
      /// <returns>Una lista de inst�ncias CSPicture</returns>
      public List<Photo> GetByUserID(int uid)
      {
         string sql = string.Empty;
         List<Photo> pictures = new List<Photo>();

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT    " + SQL_SELECT_OBJECT + @" 
                    FROM      " + SQL_TABLE_IMAGES + @" 
                    WHERE     imguserid = @imguserid 
                    ORDER BY  imgdate DESC";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@imguserid", uid));
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     pictures.Add(ReadPicture(reader));
                  }
               }
            }

            return pictures;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetUserPictures", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Agrega una nueva imagen a una carpeta
      /// </summary>
      /// <param name="picture">Imagen a agregar</param>
      public void Add(Photo picture, bool copyFiles)
      {
         string sql = string.Empty;

         try
         {
            if (copyFiles)
            {
               // Averigua si existen los archivos originales
               FileInfo picfile = new FileInfo(picture.PictureFile);
               if (!picfile.Exists)
               {
                  throw new Exception("No se encuentra el archivo correspondiente a la imagen.");
               }
               FileInfo thfile = new FileInfo(picture.ThumbnailFile);
               if (!thfile.Exists)
               {
                  throw new Exception("No se encuentra el archivo correspondiente a la imagen miniatura.");
               }

               // Averigua si existen los archivos en el destino
               FileInfo picfiledest = new FileInfo(_ws.FileSystemService.GetFilePath(new PhotosFSID(), picfile.Name));
               if (!picfile.Exists)
               {
                  throw new Exception("Ya existe una imagen que usa el mismo nombre de archivo (" + picfile.Exists + ").");
               }
               FileInfo thfiledest = new FileInfo(_ws.FileSystemService.GetFilePath(new PhotosFSID(), thfile.Name));
               if (!thfiledest.Exists)
               {
                  throw new Exception("Ya existe una imagen miniatura que usa el mismo nombre de archivo (" + thfiledest.Exists + ").");
               }

               // Copia los archivos
               picfile.CopyTo(_ws.FileSystemService.GetFilePath(new PhotosFSID(), picfile.Name));
               thfile.CopyTo(_ws.FileSystemService.GetFilePath(new PhotosFSID(), thfile.Name));

               picture.PictureFile = picfile.Name;
               picture.ThumbnailFile = thfile.Name;
            }

            _ws.DataSource.Connect();

            // Inserta el registro en la BBDD
            sql = "INSERT INTO " + SQL_TABLE_IMAGES + " (imfolder,imgtemplate,imgfile,imgwidth,imgheight,imgthumb,imgthwidth,imgthheigth,imgdesc,imgauthory,imgdate,imgshows,imguserid) " +
                  "VALUES (@imfolder,@imgtemplate,@imgfile,@imgwidth,@imgheight,@imgthumb,@imgthwidth,@imgthheigth,@imgdesc,@imgauthory,GetDate(),0,@imguserid)";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@imfolder", picture.FolderId));
               cmd.Parameters.Add(new SqlParameter("@imgtemplate", picture.Template));
               cmd.Parameters.Add(new SqlParameter("@imgfile", picture.PictureFile));
               cmd.Parameters.Add(new SqlParameter("@imgwidth", picture.PictureWidth));
               cmd.Parameters.Add(new SqlParameter("@imgheight", picture.PictureHeight));
               cmd.Parameters.Add(new SqlParameter("@imgthumb", picture.ThumbnailFile));
               cmd.Parameters.Add(new SqlParameter("@imgthwidth", picture.ThumbnailWidth));
               cmd.Parameters.Add(new SqlParameter("@imgthheigth", picture.ThumbnailHeight));
               cmd.Parameters.Add(new SqlParameter("@imgdesc", picture.Description));
               cmd.Parameters.Add(new SqlParameter("@imgauthory", picture.Author));
               cmd.Parameters.Add(new SqlParameter("@imguserid", picture.UserID));
               cmd.ExecuteNonQuery();
            }

            // Obtiene el nuevo Id
            sql = @"SELECT Top 1 Max(imgid) 
                    FROM   " + SQL_TABLE_IMAGES;

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               picture.ID = (int)cmd.ExecuteScalar();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "Add", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Delete a photo and all its associated files.
      /// </summary>
      /// <param name="pictureId">Photo unique identifier.</param>
      public void Delete(int pictureId)
      {
         Delete(pictureId, true);
      }

      /// <summary>
      /// Delete a photo and all its associated files.
      /// </summary>
      /// <param name="pictureId">Photo unique identifier.</param>
      /// <param name="deleteFiles">Indicate if the files must be deleted.</param>
      public void Delete(int pictureId, bool deleteFiles)
      {
         string sql = string.Empty;

         // Obtiene la imagen
         Photo picture = GetByID(pictureId);

         try
         {
            // Connect with database
            _ws.DataSource.Connect();

            // Start a transaction block
            using (SqlTransaction trans = _ws.DataSource.Connection.BeginTransaction())
            {
               // Elimina el registro de la base de datos
               sql = @"DELETE 
                       FROM   images 
                       WHERE  imgid = @imgid";

               using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans))
               {
                  cmd.Parameters.Add(new SqlParameter("@imgid", picture.ID));
                  cmd.ExecuteNonQuery();
               }

               // Elimina las relaciones con documentos
               sql = @"DELETE 
                       FROM   " + SQL_TABLE_DOCIMAGES + @" 
                       WHERE  idimgid = @idimgid";

               using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans))
               {
                  cmd.Parameters.Add(new SqlParameter("@idimgid", picture.ID));
                  cmd.ExecuteNonQuery();
               }

               // Elimina los archivos relacionados
               if (deleteFiles)
               {
                  _ws.FileSystemService.DeleteFile(new PhotosFSID(), picture.PictureFile, false);
                  _ws.FileSystemService.DeleteFile(new PhotosFSID(), picture.ThumbnailFile, false);
               }

               trans.Commit();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "Delete", ex);
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene el n�mero de elementos de una carpeta.
      /// </summary>
      /// <param name="folderId">Identificador de la carpeta contenedora.</param>
      /// <returns>El n�mero de im�genes que contiene la carpeta.</returns>
      public int GetFolderItems(int folderId)
      {
         string sql = string.Empty;

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT Count(*) 
                    FROM   " + SQL_TABLE_IMAGES + @" 
                    WHERE  imfolder = @imfolder";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@imfolder", folderId));
               return (int)cmd.ExecuteScalar();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetFolderItems", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene el n�mero de im�genes total.
      /// </summary>
      /// <returns>El n�mero de im�genes.</returns>
      public int GetFolderItems()
      {
         string sql = string.Empty;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT  Count(*) " + 
                  "FROM    " + SQL_TABLE_IMAGES;

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               return (int)cmd.ExecuteScalar();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetFolderItems", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene el n�mero de im�genes creadas a partir de una fecha determinada.
      /// </summary>
      /// <param name="fromDate">Fecha a partir de la cual se cuentan las im�genes.</param>
      /// <returns>El n�mero de im�genes creadas a partir de la fecha.</returns>
      public int GetFolderItems(DateTime fromDate)
      {
         string sql = string.Empty;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT  Count(*) " +
                  "FROM    " + SQL_TABLE_IMAGES + " " +
                  "WHERE   imgdate >= @creationDate";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@creationDate", fromDate));
               return (int)cmd.ExecuteScalar();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetFolderItems", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Configura una instancia de CSLayoutNavbar con la ruta de acceso a la carpeta
      /// </summary>
      /// <param name="navbar">Instancia de CSLayoutNavbar</param>
      /// <param name="folderId">Identificador de la carpeta</param>
      /// <param name="showFoldersAtLateral">Indica si se deben mostrar las carpetas en el lateral</param>
      public List<PhotoFolder> GetFolderRoute(int folderId)
      {
         string sql;
         int actfolder = folderId;
         int parentid = 0;
         PhotoFolder folder = null;
         List<PhotoFolder> items = new List<PhotoFolder>();

         try
         {
            _ws.DataSource.Connect();

            // Agrega las carpetas, de inferior a superior
            while (actfolder > 0)
            {
               sql = @"SELECT " + SQL_SELECT_FOLDER + @" 
                       FROM   " + SQL_TABLE_FOLDERS + @" 
                       WHERE  ifid = @ifid";

               using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
               {
                  cmd.Parameters.Add(new SqlParameter("@ifid", actfolder));
                  using (SqlDataReader reader = cmd.ExecuteReader())
                  {
                     if (reader.Read())
                     {
                        folder = ReadFolder(reader, false);

                        if (actfolder == folderId)
                        {
                           items.Add(folder);
                           parentid = folder.ParentID;
                        }
                        else
                        {
                           // items.Add(new CSNavbarLinkItem(reader.GetString(1), DocumentDAO.URL_CONTENT_FOLDER_VIEW + "?" + DocumentDAO.PARAM_FOLDERID + "=" + reader.GetInt32(0) + (showFoldersAtLateral ? "&lat=1" : ""), NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));
                           items.Add(folder);
                        }

                        actfolder = folder.ParentID;
                     }
                  }
               }
            }

            // Agrega el inicio
            // items.Add(new CSNavbarLinkItem("Inicio", Cms.URL_HOME, NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));

            // Ordena los elementos a�adidos del �ltimo al primero
            items.Reverse();

            // navbar.Clear();
            // for (int i = items.Count - 1; i >= 0; i--)
            // {
            //   navbar.Items.Add(items[i]);
            // }

            // Agrega el comando Volver (navega a la carpeta de nivel superior)
            /*if (parentid > 0)
               navbar.Items.Add(new CSNavbarLinkItem("Volver", DocumentDAO.URL_CONTENT_FOLDER_VIEW + "?" + DocumentDAO.PARAM_FOLDERID + "=" + parentid + (showFoldersAtLateral ? "&lat=1" : ""), NavbarItemPosition.Right, NavbarLinkDestination.InSameWindow));
            else
               navbar.Items.Add(new CSNavbarLinkItem("Volver", DocumentDAO.URL_HOME, NavbarItemPosition.Right, NavbarLinkDestination.InSameWindow));*/

            return items;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetFolderRoute", ex);
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve un nombre v�lido para una imagen miniatura
      /// </summary>
      /// <param name="filename">Nombre de la imagen original</param>
      /// <returns>El nombre a usar para la imagen miniatura</returns>
      public string GenerateThumbnailFilename(string filename)
      {
         FileInfo imgfile = new FileInfo(filename);
         if (!imgfile.Exists)
         {
            throw new Exception("El archivo " + imgfile.Name + " no se encuentra o no est� disponible.");
         }

         return imgfile.FullName.ToLower().Replace(imgfile.Extension.ToLower(), string.Empty) + "_th" + imgfile.Extension.ToLower();
      }

      /// <summary>
      /// Genera una imagen miniatura a partir de la imagen original
      /// </summary>
      /// <param name="filename">Nombre y path del archivo de imagen original</param>
      /// <returns>El nombre (sin path) de la imagen miniatura</returns>
      public string CreateThumnail(string filename)
      {
         string thname = string.Empty;

         try
         {
            // Se asegura de la existencia y accesibilidad del archivo
            FileInfo imgfile = new FileInfo(filename);
            if (!imgfile.Exists)
            {
               throw new Exception("El archivo " + imgfile.Name + " no se encuentra o no est� disponible.");
            }

            // Obtiene la imagen
            Image image = Image.FromFile(imgfile.FullName);

            // Genera la imagen miniatura
            Image thumb = image.GetThumbnailImage(this.GalleryThumbnailWidth,
                                                  (this.GalleryThumbnailWidth * image.Height / image.Width),
                                                  new Image.GetThumbnailImageAbort(ThumbnailCallback),
                                                  IntPtr.Zero);

            // Guarda la imagen miniatura
            thname = GenerateThumbnailFilename(imgfile.FullName);
            thumb.Save(thname);

            // Obtiene s�lo la parte del nombre
            imgfile = new FileInfo(thname);
            return imgfile.Name;
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Obtiene el nombre (sin ruta) para una nueva imagen a partir del patr�n de la carpeta
      /// </summary>
      /// <param name="folderid">Identificador de la carpeta para la que se desea generar el nuevo nombre de archivo</param>
      /// <param name="originalFileName">Nombre original del archivo</param>
      /// <returns>Un nombre de archivo v�lido para una determinada carpeta</returns>
      public string GetFileName(int folderid, string originalFileName)
      {
         string sql;
         string name = "image_%ROWS%";
         string extension = "jpg";

         // Recupera la extensi�n del nombre del archivo
         originalFileName = originalFileName.Trim().ToLower();
         extension = originalFileName.Substring(originalFileName.LastIndexOf("."));

         try
         {
            _ws.DataSource.Connect();

            sql = @"SELECT (SELECT Count(*) FROM " + SQL_TABLE_IMAGES + @" WHERE imfolder=@ifid)+1 AS row, 
                           (SELECT Count(*) FROM " + SQL_TABLE_IMAGES + @")+1 AS rows, 
                           iffilepattern 
                    FROM   " + SQL_TABLE_FOLDERS + @" 
                    WHERE  ifid = @ifid";

            using (SqlCommand cmd = new SqlCommand(sql, _ws.DataSource.Connection))
            {
               cmd.Parameters.Add(new SqlParameter("@ifid", folderid));
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  if (reader.Read())
                  {
                     // Obtiene el patr�n del nombre
                     name = reader.GetString(2).Trim();
                     if (string.IsNullOrWhiteSpace(name)) return originalFileName;

                     // Reemplaza los TAGs del patr�n
                     string tag = string.Empty;
                     int row = 0;

                     if (name.Contains("%ROW%"))
                     {
                        tag = "%ROW%";
                        row = reader.GetInt32(0);
                     }
                     else if (name.Contains("%ROWS%"))
                     {
                        tag = "%ROWS%";
                        row = reader.GetInt32(1);
                     }
                     else
                     {
                        return originalFileName;
                     }

                     FileInfo file = new FileInfo(_ws.FileSystemService.GetFilePath(new PhotosFSID(), name.Replace(tag, row.ToString()) + extension));
                     while (file.Exists)
                     {
                        row++;
                        file = new FileInfo(_ws.FileSystemService.GetFilePath(new PhotosFSID(), name.Replace(tag, row.ToString()) + extension));
                     }

                     return file.Name;
                  }
                  else
                  {
                     throw new Exception("No se ha podido acceder a la carpeta solicitada.");
                  }
               }
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetFileName", ex); 
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve la fecha desde la que se consideran nuevas im�genes
      /// </summary>
      public static DateTime NewPicturesDate
      {
         get { return System.DateTime.Now.AddDays(-30); }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _ws = null;
      }

      /// <summary>
      /// Recupera la fila actual del SqlDataReader como una imagen.
      /// </summary>
      private Photo ReadPicture(SqlDataReader reader)
      {
         Photo picture = new Photo();

         picture.ID = reader.GetInt32(0);
         picture.FolderId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
         picture.Template = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
         picture.PictureFile = _ws.FileSystemService.GetFileURL(new PhotosFSID(), (reader.IsDBNull(3) ? "cs_img_unknown.jpg" : reader.GetString(3)));
         picture.PictureWidth = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
         picture.PictureHeight = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
         picture.ThumbnailFile = _ws.FileSystemService.GetFileURL(new PhotosFSID(), (reader.IsDBNull(6) ? "cs_img_unknown.jpg" : reader.GetString(6)));
         picture.ThumbnailWidth = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
         picture.ThumbnailHeight = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
         picture.Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
         picture.Author = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
         picture.UserID = reader.GetInt32(11);
         picture.Created = reader.GetDateTime(12);
         picture.Shows = reader.IsDBNull(13) ? 0 : reader.GetInt32(13);

         return picture;
      }

      /// <summary>
      /// Recupera la fila actual del SqlDataReader como una carpeta.
      /// </summary>
      private PhotoFolder ReadFolder(SqlDataReader reader, bool getFolderObjects)
      {
         PhotoFolder folder = new PhotoFolder();
         folder.ID = reader.GetInt32(0);
         folder.ParentID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
         folder.Name = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
         folder.Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
         folder.IsContainer = reader.GetBoolean(4);
         folder.Order = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
         folder.Enabled = reader.GetBoolean(6);
         folder.Owner = reader.IsDBNull(7) ? SecurityService.ACCOUNT_SUPER : reader.GetString(7);
         folder.FilePattern = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
         if (getFolderObjects) folder.Objects = reader.GetInt32(9);

         folder.IsContainer = folder.IsContainer & _ws.Settings.GetBoolean(PhotoDAO.SETUP_SETTING_USERSCANUPLOAD, false);
         folder.IsContainer = folder.IsContainer & !folder.FilePattern.Equals(string.Empty);

         return folder;
      }

      /// <summary>
      /// Required, but not used
      /// </summary>
      /// <returns>true</returns>
      private bool ThumbnailCallback()
      {
         return true;
      }

      /// <summary>
      /// Averigua si un determinado archivo puede ser admitido o no como imagen.
      /// </summary>
      /// <param name="filename">Nombre del archivo (sin path).</param>
      /// <returns><c>True</c> si es admitido o <c>False</c> en cualquier otro caso.</returns>
      private bool IsAllowedFile(string filename)
      {
         string filter = string.Empty;
         List<string> extensions = new List<string>();

         filename = filename.ToLower().Trim();

         // Obtiene una lista de la extensiones permitidas
         foreach (string ext in FileAllowedExtensions)
         {
            string fext = ext.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(fext))
            {
               if (!fext.StartsWith(".")) fext = "." + fext;
               extensions.Add(fext);
            }
         }

         // Genera la expresi�n regular de filtrado
         filter = @"^.+\.(";
         foreach (string ext in FileAllowedExtensions)
         {
            filter += "(" + ext.Replace(".", string.Empty) + ")|";
         }
         filter = filter.Substring(0, filter.Length - 1);    // Elimina la �ltima barra vertical
         filter += ")$";

         // Evalua la expresi�n regular
         Match match = Regex.Match(filename, RegExFilter);

         return !match.Groups[1].Value.Trim().Equals(string.Empty);
      }

      /// <summary>
      /// Devuelve la cadena que permite filtrar mediante Regular Expressions los archivos recibidos.
      /// </summary>
      private string RegExFilter
      {
         get
         {
            string filter = "";

            filter = @"^.+\.(";
            foreach (string ext in FileAllowedExtensions)
            {
               filter += "(" + ext.Replace(".", "") + ")|";
            }
            filter = filter.Substring(0, filter.Length - 1);    // Elimina la �ltima barra vertical
            filter += ")$";

            return filter;
         }
      }

      #endregion

      #region Disabled Code

      /*
      
      /// <summary>
      /// Genera la ruta de navegaci�n para acceder a la carpeta especificada
      /// </summary>
      /// <param name="folderId">Identificador de la carpeta.</param>
      /// <param name="makeLastFolderLink">Indica si el �ltimo elemento de la ruta debe ser clicable o no</param>
      /// <returns>Un string con el c�digo XHTML.</returns>
      public CSNavbar FolderNavigationRoute(int folderId, Cosmo.Cms.Layout.NavbarSeparators separator, bool controlElementLegth, bool makeLastFolderLink)
      {
         int returnId = 0;
         List<CSNavbarLinkItem> items = new List<CSNavbarLinkItem>();

         CSNavbar navbar = new CSNavbar();
         navbar.Separator = separator;
         navbar.ControlTitleLength = controlElementLegth;

         try
         {
            // CSPicturesFolder folder = this.ReadFolder(folderId);
            // string path = " &raquo; " + HttpUtility.HtmlDecode(folder.PropertyName);
            int actfolder = folderId;

            // Agrega la ruta de carpetas
            while (actfolder > 0)
            {
               PictureFolder folder = this.GetFolder(actfolder, false);

               if (folder.ID == folderId)
               {
                  if (makeLastFolderLink)
                     items.Add(new CSNavbarLinkItem(folder.PropertyName, PictureDAO.URL_FOLDER + "?" + Cosmo.Workspace.PARAM_FOLDER_ID + "=" + folder.ID, NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));
                  else
                     items.Add(new CSNavbarLinkItem(folder.PropertyName, NavbarItemPosition.Left));
                  returnId = folder.ParentID;
               }
               else
                  items.Add(new CSNavbarLinkItem(folder.PropertyName, PictureDAO.URL_FOLDER + "?" + Cosmo.Workspace.PARAM_FOLDER_ID + "=" + folder.ID, NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));

               actfolder = folder.ParentID;
            }

            // Agrega el acceso al inicio del servicio
            items.Add(new CSNavbarLinkItem(PictureDAO.SERVICE_NAME, PictureDAO.URL_HOME, NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));

            // Agrega el acceso a la p�gina de inicio
            items.Add(new CSNavbarLinkItem("Inicio", Workspace.COSMO_URL_DEFAULT, NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));

            // Ordena los elementos a�adidos del �ltimo al primero
            navbar.Clear();
            for (int i = items.Count - 1; i >= 0; i--)
            {
               navbar.Items.Add(items[i]);
            }

            /*
            // Agrega el comando Volver (navega a la carpeta contenedora)
            if (returnId > 0)
               navbar.Items.Add(new CSNavbarLinkItem("Volver", CSPictures.URL_CONTENT_FOLDER_VIEW + "?" + CSPictures.PARAM_FOLDERID + "=" + returnId, NavbarItemPosition.Right, NavbarLinkDestination.InSameWindow));
            else
               navbar.Items.Add(new CSNavbarLinkItem("Volver", CSPictures.URL_HOME, NavbarItemPosition.Right, NavbarLinkDestination.InSameWindow));
            * /

            return navbar;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().PropertyName + ".FolderNavigationRoute()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
      }

      
      /// <summary>
      /// Genera la ruta de navegaci�n para acceder a la carpeta especificada
      /// </summary>
      /// <param name="folderId">Identificador de la carpeta.</param>
      /// <returns>Un string con el c�digo XHTML.</returns>
      public string GetFolderRoute(int folderId)
      {
         int returnId = 0;
         string res = string.Empty;

         try
         {
            // CSPicturesFolder folder = this.ReadFolder(folderId);
            // string path = " &raquo; " + HttpUtility.HtmlDecode(folder.PropertyName);
            int actfolder = folderId;

            // Agrega la ruta de carpetas
            while (actfolder > 0)
            {
               PictureFolder folder = this.GetFolder(actfolder);

               if (folder.ID == folderId) returnId = folder.ParentID;
               res = folder.PropertyName + " &raquo; " + res;
               actfolder = folder.ParentID;
            }

            // Agrega el acceso al inicio del servicio
            res = PictureDAO.SERVICE_NAME + " &raquo; " + res;

            // Elimina el �ltimo separador
            res = res.Trim();
            res = res.Substring(0, res.Length - ("&raquo;".Length));

            return res;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().PropertyName + ".FolderNavigationRoute()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
      } 
      
      /// <summary>
      /// Genera un canal RSS 2.0 que contiene las �ltimas im�genes agregadas a la secci�n
      /// </summary>
      /// <returns>Devuelve una cadena en formato RSS</returns>
      public string ToRss()
      {
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         RssChannel rss = null;

         try
         {
            _ws.DataSource.Connect();

            // Implementa las propiedades del canal
            rss = new RssChannel();
            rss.Title = _ws.PropertyName + " - " + PictureDAO.SERVICE_NAME;
            rss.Copyright = "Copyright &copy; " + _ws.PropertyName;
            rss.Language = "es-es";
            rss.ManagingEditor = _ws.Mail;
            rss.LastBuildDate = DateTime.Now;
            rss.PubDate = DateTime.Now;
            rss.Link = _ws.Url;
            rss.Image = new RssChannelImage();
            rss.Image.Url = new Uri(Cosmo.Net.Url.Combine(_ws.Url, "images", "cs_rss_image.jpg"));
            rss.Image.Link = _ws.Url;

            // Rellena las entradas de noticias al canal
            cmd = new SqlCommand("cs_RSS_GetImages", _ws.DataSource.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               RssItem item = new RssItem();
               item.Link = Cosmo.Net.Url.Combine(_ws.Url, reader.GetString(4)) + "?" + Cosmo.Workspace.PARAM_OBJECT_ID + "=" + reader.GetInt32(0);
               item.Title = reader.GetString(1);
               item.Description = "<img src=\"" + _ws.FileSystemService.GetFileURL(PictureDAO.SERVICE_FOLDER, reader.GetString(8)) + "\" alt=\"Imagen miniatura\" /><br />" + reader.GetString(2);
               item.PubDate = reader.GetDateTime(3);
               item.Category = reader.GetString(6);
               rss.Items.Add(item);
            }
            reader.Close();

            return rss.ToXml();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().PropertyName + ".ToRSS()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Genera un canal RSS 2.0 que contiene las �ltimas im�genes agregadas a la secci�n usando la cach� de servidor web
      /// </summary>
      /// <param name="cache">La inst�ncia Cache del servidor web</param>
      /// <param name="forceUpdate">Indica si debe refrescarse el contenido sin usar la cach�.</param>
      /// <returns>Devuelve una cadena en formato RSS</returns>
      public string ToRss(System.Web.Caching.Cache cache, bool forceUpdate)
      {
         // Calcula el n�mero de segundos de validez
         int secs = 3600;
         if (!int.TryParse(_ws.Settings.GetString(Cms.RSSCacheTimeout, "3600"), out secs)) secs = 3600;

         // Guarda el feed en cach�
         if (cache[Cms.CACHE_RSS_PICTURES] == null || forceUpdate)
            cache.Insert(Cms.CACHE_RSS_PICTURES, ToRss(), null, DateTime.Now.AddSeconds(secs), TimeSpan.Zero);

         // Devuelve el contenido de la cach�
         return cache[Cms.CACHE_RSS_PICTURES].ToString();
      }
       
       */

      #endregion

   }
}