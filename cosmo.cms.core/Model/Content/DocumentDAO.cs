using Cosmo.Cms.Model.Photos;
using Cosmo.Data.Connection;
using Cosmo.Diagnostics;
using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace Cosmo.Cms.Model.Content
{
   /// <summary>
   /// Permite gestionar documentos y carpetas.
   /// </summary>
   public class DocumentDAO
   {
      // Internal data declarations
      private Workspace _ws;

      /// <summary>Nombre del servicio</summary>
      public const string SERVICE_NAME = "Artículos";

      /// <summary>Rol correspondiente a publicador de contenido</summary>
      public const string ROLE_CONTENT_EDITOR = "content.editor";

      private const string SMARTTAG_OBJECT_ID = "%DOCID%";
      private const string SMARTTAG_WORKSPACE_NAME = "%WS-NAME%";

      // Fragmentos SQL reaprovechables
      private const string SQL_TABLE_OBJECTS = "CMS_DOCS";
      private const string SQL_TABLE_FOLDERS = "CMS_DOCFOLDERS";
      private const string SQL_TABLE_RELOBJOBJ = "CMS_DOCRELATIONS";
      private const string SQL_TABLE_RELOBJPIC = "CMS_DOCSIMAGES";
      private const string SQL_DOC_SELECT = "docid,docfolder,doctitle,docdesc,dochtml,docpic,docviewer,dochighlight,docenabled,docdate,docupdated,docshows,docfile";
      private const string SQL_FOLDER_SELECT = "folderid,folderparentid,foldername,folderdesc,folderorder,foldercreated,folderenabled,foldershowtitle,foldermenu";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="DocumentDAO"/>.
      /// </summary>
      public DocumentDAO(Workspace ws)
      {
         Initialize();

         _ws = ws;
      }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene las propiedades de un documento.
      /// </summary>
      /// <param name="docID">Idetificador del documento.</param>
      /// <returns>Una instáncia de la clase <see cref="Document"/>.</returns>
      public Document Item(int docID)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         Document reldoc = null;
         Document doc = null;

         try
         {
            // Ensure that the private folder for a object is created.
            EnsureObjectFolderExists(docID, false);

            _ws.DataSource.Connect();

            // Obtiene el documento
            sql = @"SELECT  " + SQL_DOC_SELECT + @" 
                    FROM    " + SQL_TABLE_OBJECTS + @" 
                    WHERE   docid = @docid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@docid", docID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               if (reader.Read())
               {
                  doc = ReadDocument(reader);

                  // Reemplaza los smartTAGS del contenido HTML
                  doc.Content = doc.Content.Replace(DocumentDAO.SMARTTAG_OBJECT_ID, doc.ID.ToString());
                  doc.Content = doc.Content.Replace(DocumentDAO.SMARTTAG_WORKSPACE_NAME, _ws.Name);
               }
            }

            // Obtiene los documentos relacionados
            sql = @"SELECT " + SQL_DOC_SELECT + @" 
                    FROM   " + SQL_TABLE_RELOBJOBJ + @" 
                           INNER JOIN " + SQL_TABLE_OBJECTS + @" ON (" + SQL_TABLE_RELOBJOBJ + @".docdestid=" + SQL_TABLE_OBJECTS + @".docid) 
                    WHERE  docsourceid = @docsourceid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@docsourceid", doc.ID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  reldoc = ReadDocument(reader);
                  if (reldoc != null)
                  {
                     doc.RelatedDocuments.Add(reldoc);
                  }
               }
            }

            // Obtiene las imágenes relacionadas
            sql = @"SELECT    imgid, imfolder, imgtemplate, imgfile, imgwidth, imgheight, imgthumb, imgthwidth, imgthheigth, imgdesc, imgauthory, imgdate, imgshows 
                    FROM      " + SQL_TABLE_RELOBJPIC + @" 
                              INNER JOIN images ON (" + SQL_TABLE_RELOBJPIC + @".idimgid = images.imgid) 
                    WHERE     iddocid = @iddocid 
                    ORDER BY  idorder Asc, idimgid Desc";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@iddocid", doc.ID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  Photo picture = new Photo();
                  picture.ID = reader.GetInt32(0);
                  picture.FolderId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                  picture.Template = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                  picture.PictureFile = _ws.FileSystemService.GetFileURL(new PhotosFSID(), (reader.IsDBNull(3) ? string.Empty : reader.GetString(3)));
                  picture.PictureWidth = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                  picture.PictureHeight = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                  picture.ThumbnailFile = _ws.FileSystemService.GetFileURL(new PhotosFSID(), (reader.IsDBNull(6) ? string.Empty : reader.GetString(6)));
                  picture.ThumbnailWidth = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                  picture.ThumbnailHeight = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                  picture.Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                  picture.Author = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                  picture.Created = reader.GetDateTime(11);
                  picture.Shows = reader.IsDBNull(12) ? 0 : reader.GetInt32(12);

                  doc.RelatedPictures.Add(picture);
               }
            }

            return doc;
         }
         catch (Exception err)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".Item()", 
                                        err.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw err;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Agrega una nueva imagen a una carpeta
      /// </summary>
      /// <param name="picture">Imagen a agregar</param>
      /// <remarks>
      /// Este método espera en las propiedades CSPicture.
      /// </remarks>
      public void Add(Document document)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         FileInfo picfile = null;
         FileInfo attfile = null;

         // Averigua si existen los archivos adjuntos al documento
         if (!String.IsNullOrEmpty(document.Thumbnail))
         {
            picfile = new FileInfo(document.Thumbnail);
            if (!picfile.Exists)
            {
               throw new Exception("No se encuentra el archivo correspondiente a la imagen miniatura.");
            }
         }
         if (!String.IsNullOrEmpty(document.Attachment))
         {
            attfile = new FileInfo(document.Attachment);
            if (!attfile.Exists)
            {
               throw new Exception("No se encuentra el archivo correspondiente al contenido adjunto.");
            }
         }

         try
         {
            _ws.DataSource.Connect();

            using (SqlTransaction trans = _ws.DataSource.Connection.BeginTransaction())
            {
               // Inserta el registro en la BBDD
               sql = "INSERT INTO " + SQL_TABLE_OBJECTS + "kk " +
                        "(docsection,docfolder,doctitle,docdesc,dochtml,docpic,docviewer,dochighlight,docenabled,docdate,docupdated,docshows,doctype,docfile,docowner) " +
                     "VALUES " +
                        "(0,@docfolder,@doctitle,@docdesc,@dochtml,@docpic,@docviewer,@dochighlight,@docenabled,getdate(),getdate(),0,1,@docfile,@docowner)";

               cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
               cmd.Parameters.Add(new SqlParameter("@docfolder", document.FolderId));
               cmd.Parameters.Add(new SqlParameter("@doctitle", document.Title));
               cmd.Parameters.Add(new SqlParameter("@docdesc", document.Description));
               cmd.Parameters.Add(new SqlParameter("@dochtml", document.Content));
               cmd.Parameters.Add(new SqlParameter("@docpic", (String.IsNullOrEmpty(document.Thumbnail) ? string.Empty : picfile.Name)));
               cmd.Parameters.Add(new SqlParameter("@docviewer", document.Template));
               cmd.Parameters.Add(new SqlParameter("@dochighlight", document.Hightlight));
               cmd.Parameters.Add(new SqlParameter("@docenabled", document.Published));
               cmd.Parameters.Add(new SqlParameter("@docfile", (String.IsNullOrEmpty(document.Attachment) ? string.Empty : attfile.Name)));
               cmd.Parameters.Add(new SqlParameter("@docowner", _ws.CurrentUser.IsAuthenticated ? _ws.CurrentUser.User.Login : SecurityService.ACCOUNT_SUPER));
               cmd.ExecuteNonQuery();

               // Obtiene el nuevo ID
               sql = "SELECT Top 1 Max(docid) " +
                     "FROM " + SQL_TABLE_OBJECTS;
               cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
               document.ID = (int)cmd.ExecuteScalar();

               // Ensure that the private folder for a object is created.
               EnsureObjectFolderExists(document.ID, false);

               // Copia los archivos (thumbnail y adjunto) a la carpeta del documento
               if (picfile != null) picfile.CopyTo(_ws.FileSystemService.GetFilePath(new DocumentFSID(document.ID), picfile.Name));
               if (attfile != null) attfile.CopyTo(_ws.FileSystemService.GetFilePath(new DocumentFSID(document.ID), attfile.Name));

               trans.Commit();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Add()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Actualiza la información de un documento.
      /// </summary>
      /// <param name="document">Documento a actualizar</param>
      public void Update(Document document)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         FileInfo attfile = null;
         FileInfo picfile = null;

         try
         {
            // Ensure that the private folder for a object is created.
            EnsureObjectFolderExists(document.ID, false);

            // Copy or update the thumbnail
            if (!String.IsNullOrEmpty(document.Thumbnail))
            {
               picfile = new FileInfo(document.Thumbnail);
               if (!picfile.Exists)
               {
                  throw new Exception("No se encuentra o no está accesible el archivo correspondiente a la imagen miniatura.");
               }

               // Si existe un archivo con el mismo nombre lo sobreescribe
               picfile.CopyTo(_ws.FileSystemService.GetFilePath(new DocumentFSID(document.ID), picfile.Name));
            }

            // Copy or update the attached file
            if (!String.IsNullOrEmpty(document.Attachment))
            {
               attfile = new FileInfo(document.Attachment);
               if (!attfile.Exists)
               {
                  throw new Exception("No se encuentra o no está accesible el archivo adjunto.");
               }

               // Si existe un archivo con el mismo nombre lo sobreescribe
               attfile.CopyTo(_ws.FileSystemService.GetFilePath(new DocumentFSID(document.ID), attfile.Name));
            }

            _ws.DataSource.Connect();

            // Update the database record
            sql = @"UPDATE " + SQL_TABLE_OBJECTS + @" 
                    SET    docfolder = @docfolder, 
                           doctitle = @doctitle, 
                           docdesc = @docdesc, 
                           dochtml = @dochtml, 
                           docviewer = @docviewer, 
                           docenabled = @docenabled, 
                           dochighlight = @dochighlight, 
                           docupdated = getdate() 
                    WHERE  docid = @docid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@docfolder", document.FolderId));
            cmd.Parameters.Add(new SqlParameter("@doctitle", document.Title));
            cmd.Parameters.Add(new SqlParameter("@docdesc", document.Description));
            cmd.Parameters.Add(new SqlParameter("@dochtml", document.Content));
            cmd.Parameters.Add(new SqlParameter("@docviewer", document.Template));
            cmd.Parameters.Add(new SqlParameter("@docenabled", document.Status == Common.CmsPublishStatus.PublishStatus.Published ? true : false));
            cmd.Parameters.Add(new SqlParameter("@dochighlight", document.Hightlight));
            cmd.Parameters.Add(new SqlParameter("@docid", document.ID));
            cmd.ExecuteNonQuery();

            // Add content thumbnail if provided
            if (!String.IsNullOrEmpty(document.Thumbnail))
            {
               sql = @"UPDATE " + SQL_TABLE_OBJECTS + @" 
                       SET    docpic = @docpic 
                       WHERE  docid = @docid";

               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@docpic", picfile.Name));
               cmd.Parameters.Add(new SqlParameter("@docid", document.ID));
               cmd.ExecuteNonQuery();
            }

            // Add content attachment if provided
            if (!String.IsNullOrEmpty(document.Attachment))
            {
               sql = @"UPDATE " + SQL_TABLE_OBJECTS + @" 
                       SET    docfile = @docfile 
                       WHERE  docid = @docid";

               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@docfile", attfile.Name));
               cmd.Parameters.Add(new SqlParameter("@docid", document.ID));
               cmd.ExecuteNonQuery();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Update()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene los documentos destacados en una carpeta.
      /// </summary>
      /// <param name="folderid">Identificador de la carpeta</param>
      /// <returns>Un array de objetos <see cref="Document"/>.</returns>
      public List<Document> GetHighlighted(int folderid)
      {
         string sql = string.Empty;
         string xhtml = string.Empty;
         string link = string.Empty;
         Document document = null;
         List<Document> list = new List<Document>();
         SqlCommand cmd = null;

         _ws.DataSource.Connect();

         try
         {
            sql = "SELECT " + 
                     SQL_DOC_SELECT + " " +
                  "FROM " +
                     "CMS_DOCS INNER Join CMS_DOCFOLDERS On (CMS_DOCS.DOCFOLDER=CMS_DOCFOLDERS.FOLDERID) " +
                  "WHERE " +
                     "CMS_DOCS.DOCHIGHLIGHT=1 And " +
                     "CMS_DOCS.DOCENABLED=1 And " +
                     "(CMS_DOCFOLDERS.FOLDERPARENTID=@folderId Or CMS_DOCFOLDERS.FOLDERID=@folderId) " +
                  "ORDER BY " +
                     "CMS_DOCS.DOCSECTION, " +
                     "CMS_DOCS.DOCDATE DESC";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@folderId", folderid));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  document = ReadDocument(reader);
                  if (document != null) list.Add(document);
               }
            }

            return list;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".GetHighlighted()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Adds a new content folder.
      /// </summary>
      /// <param name="folder">The instance of the new folder.</param>
      public void AddFolder(DocumentFolder folder)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            using (SqlTransaction trans = _ws.DataSource.Connection.BeginTransaction())
            {
               // Insert the data into database
               sql = @"INSERT INTO " + SQL_TABLE_FOLDERS + @" (folderparentid, foldername, folderdesc, foldershowtitle, folderorder, foldercreated, folderenabled, updated, foldermenu) 
                       VALUES (@folderparentid, @foldername, @folderdesc, @foldershowtitle, @folderorder, getdate(), @folderenabled, getdate(), @foldermenu)";

               cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
               cmd.Parameters.Add(new SqlParameter("@folderparentid", folder.ParentID));
               cmd.Parameters.Add(new SqlParameter("@foldername", folder.Name));
               cmd.Parameters.Add(new SqlParameter("@folderdesc", folder.Description));
               cmd.Parameters.Add(new SqlParameter("@foldershowtitle", folder.ShowTitle));
               cmd.Parameters.Add(new SqlParameter("@folderorder", folder.Order));
               cmd.Parameters.Add(new SqlParameter("@folderenabled", (folder.Status == Common.CmsPublishStatus.PublishStatus.Published)));
               cmd.Parameters.Add(new SqlParameter("@foldermenu", folder.MenuId));
               cmd.ExecuteNonQuery();

               // Get the new unique identifier
               sql = @"SELECT Top 1 Max(folderid) 
                       FROM " + SQL_TABLE_FOLDERS;
               cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
               folder.ID = (int)cmd.ExecuteScalar();

               // Ensure that the private folder for a object is created.
               EnsureObjectFolderExists(folder.ID, true);

               trans.Commit();
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".AddFolder()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Update the data of a content folder.
      /// </summary>
      /// <param name="folder">Content folder instance with the data updated.</param>
      public void UpdateFolder(DocumentFolder folder)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            // Ensure that the private folder for a object is created.
            EnsureObjectFolderExists(folder.ID, true);

            _ws.DataSource.Connect();

            using (SqlTransaction trans = _ws.DataSource.Connection.BeginTransaction())
            {
               // Inserta el registro en la BBDD
               sql = @"UPDATE " + SQL_TABLE_FOLDERS + @" 
                       SET    folderparentid = @folderparentid, 
                              foldername = @foldername, 
                              folderdesc = @folderdesc, 
                              foldershowtitle = @foldershowtitle, 
                              folderorder = @folderorder, 
                              folderenabled = @folderenabled, 
                              updated = getdate(), 
                              foldermenu = @foldermenu 
                       WHERE  folderid = @folderid";

               cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
               cmd.Parameters.Add(new SqlParameter("@folderparentid", folder.ParentID));
               cmd.Parameters.Add(new SqlParameter("@foldername", folder.Name));
               cmd.Parameters.Add(new SqlParameter("@folderdesc", folder.Description));
               cmd.Parameters.Add(new SqlParameter("@foldershowtitle", folder.ShowTitle));
               cmd.Parameters.Add(new SqlParameter("@folderorder", folder.Order));
               cmd.Parameters.Add(new SqlParameter("@folderenabled", (folder.Status == Common.CmsPublishStatus.PublishStatus.Published)));
               cmd.Parameters.Add(new SqlParameter("@foldermenu", folder.MenuId));
               cmd.Parameters.Add(new SqlParameter("@folderid", folder.ID));
               cmd.ExecuteNonQuery();

               trans.Commit();
            }

         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".UpdateFolder()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Get all content folders contained in a folder.
      /// </summary>
      /// <param name="parentId">Parent folder unique identifier (DB).</param>
      /// <returns>A list of <see cref="DocumentFolder"/> instances corresponding to a subfolders of the specified folder.</returns>
      public List<DocumentFolder> GetSubfolders(int parentId)
      {
         string sql = string.Empty;
         DocumentFolder folder = null;
         List<DocumentFolder> folders = new List<DocumentFolder>();
         SqlCommand cmd = null;

         _ws.DataSource.Connect();

         try
         {
            sql = @"SELECT    " + SQL_FOLDER_SELECT + @", 
                              (SELECT  Count(*) 
                               FROM    " + SQL_TABLE_OBJECTS + @" 
                               WHERE   " + SQL_TABLE_OBJECTS + @".docfolder=" + SQL_TABLE_FOLDERS + @".folderid) As items,
                              foldershowtitle 
                    FROM      " + SQL_TABLE_FOLDERS + @" 
                    WHERE     folderparentid = @folderparentid AND 
                              folderenabled = 1 
                    ORDER BY  folderorder Asc, 
                              foldername Asc";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@folderparentid", parentId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  folder = ReadFolder(reader, true);
                  folders.Add(folder);
               }
            }

            return folders;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".GetSubfolders(int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Get all content folders of top level.
      /// </summary>
      /// <returns>A list of <see cref="DocumentFolder"/> instances corresponding to a folder of top level.</returns>
      public List<DocumentFolder> GetSubfolders()
      {
         return GetSubfolders(0);
      }

      /// <summary>
      /// Obtiene las propiedades de una carpeta de documentos.
      /// </summary>
      /// <param name="folderId">Identificador de la carpeta.</param>
      /// <param name="getFolderContents">Indics si se deben recuperar los elementos de la carpeta (documentos y subcarpetas).</param>
      /// <returns>Una instáncia de la clase <see cref="DocumentFolder"/>.</returns>
      public DocumentFolder GetFolder(int folderId, bool getFolderContents)
      {
         string sql = string.Empty;
         DocumentFolder folder = null;
         SqlCommand cmd = null;

         // Evita realizar consultas de objetos no existentes
         if (folderId <= 0)
         {
            return null;
         }



         try
         {
            // Ensure that the private folder for a object is created.
            EnsureObjectFolderExists(folderId, true);

            _ws.DataSource.Connect();

            sql = @"SELECT    " + SQL_FOLDER_SELECT + @", 
                              (SELECT  Count(*) 
                               FROM    " + SQL_TABLE_OBJECTS + @" 
                               WHERE   " + SQL_TABLE_OBJECTS + @".docfolder = " + SQL_TABLE_FOLDERS + @".folderid) As items 
                    FROM      " + SQL_TABLE_FOLDERS + @" 
                    WHERE     folderid = @folderid 
                    ORDER BY  folderorder Asc, 
                              foldername Asc";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@folderid", folderId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               if (reader.Read())
               {
                  folder = ReadFolder(reader, true);
               }
            }

            if (folder == null)
            {
               return null;
            }

            if (!getFolderContents)
            {
               // Obtiene el número de documentos que contiene
               folder.Objects = this.GetFolderItems(folderId);
            }
            else
            {
               folder.Documents = GetDocuments(folderId);
               folder.Objects = folder.Documents.Count;
               folder.Subfolders = GetSubfolders(folderId);
            }

            return folder;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".GetFolder(int,bool)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex; 
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene los documentos de una carpeta.
      /// </summary>
      /// <param name="folderId">Identificador de la carpeta.</param>
      /// <returns>Un array de objetos <see cref="Document"/>.</returns>
      public List<Document> GetDocuments(int folderId)
      {
         string sql = string.Empty;
         Document doc = null;
         List<Document> docs = new List<Document>();
         SqlCommand cmd = null;

         _ws.DataSource.Connect();

         try
         {
            sql = "SELECT " + 
                     SQL_DOC_SELECT + " " +
                  "FROM " + 
                     SQL_TABLE_OBJECTS + " " +
                  "WHERE " +
                     "docfolder=@docfolder AND docenabled=1 " +
                  "ORDER BY " +
                     "docupdated DESC";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@docfolder", folderId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  doc = ReadDocument(reader);
                  if (doc != null)
                  {
                     // Reemplaza los smartTAGS del contenido HTML
                     doc.Content = doc.Content.Replace(DocumentDAO.SMARTTAG_OBJECT_ID, doc.ID.ToString());
                     doc.Content = doc.Content.Replace(DocumentDAO.SMARTTAG_WORKSPACE_NAME, _ws.Name);

                     docs.Add(doc);
                  }
               }
            }

            return docs;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".GetDocuments(int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Configura una instancia de CSLayoutNavbar con la ruta de acceso a la carpeta
      /// </summary>
      /// <param name="navbar">Instancia de CSLayoutNavbar</param>
      /// <param name="folderId">Identificador de la carpeta</param>
      /// <param name="showFoldersAtLateral">Indica si se deben mostrar las carpetas en el lateral</param>
      public List<DocumentFolder> GetFolderRoute(int folderId)
      {
         int actfolder = folderId;
         int parentid = 0;
         SqlCommand cmd = null;
         DocumentFolder folder = null;
         List<DocumentFolder> items = new List<DocumentFolder>();

         _ws.DataSource.Connect();

         try
         {
            // Agrega las carpetas, de inferior a superior
            while (actfolder > 0)
            {
               string sql = "SELECT " + SQL_FOLDER_SELECT + " " +
                            "FROM " + SQL_TABLE_FOLDERS + " " +
                            "WHERE folderid=@folderid";

               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@folderid", actfolder));

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

            // Agrega el inicio
            // items.Add(new CSNavbarLinkItem("Inicio", Cms.URL_HOME, NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));

            // Ordena los elementos añadidos del último al primero
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
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".GetFolderRoute(int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            _ws.DataSource.Disconnect();
         }
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
      /// Obtiene el número de documentos que contiene una carpeta.
      /// </summary>
      /// <param name="folderId">Identificador d el acarpeta.</param>
      /// <returns>El número de objetos.</returns>
      private int GetFolderItems(int folderId)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         _ws.DataSource.Connect();

         try
         {
            sql = "SELECT " +
                     "Count(*) " +
                  "FROM " +
                     SQL_TABLE_OBJECTS + " " +
                  "WHERE " +
                     "docenabled=1 AND docfolder=@docfolder";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@docfolder", folderId));

            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetFolderItems(int)",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Lee un documento de una fila en la base de datos.
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      private Document ReadDocument(SqlDataReader reader)
      {
         if (reader == null)
         {
            return null;
         }
         else if (reader.IsClosed)
         {
            return null;
         }
         else if (reader.FieldCount < 13)
         {
            return null;
         }

         // SELECT 
         // docid,docfolder,doctitle,docdesc,dochtml,docpic,docviewer,dochighlight,docenabled,docdate,docupdated,docshows,docfile

         Document doc = new Document();
         doc.ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
         doc.FolderId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
         doc.Title = reader.IsDBNull(2) ? string.Empty : reader.GetString(2).Trim();
         doc.Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3).Trim();
         doc.Content = reader.IsDBNull(4) ? string.Empty : reader.GetString(4).Trim();
         doc.Thumbnail = reader.IsDBNull(5) ? string.Empty : reader.GetString(5).Trim();
         doc.Template = reader.IsDBNull(6) ? string.Empty : reader.GetString(6).Trim();
         doc.Hightlight = reader.GetBoolean(7);
         doc.Published = reader.GetBoolean(8);
         doc.Status = doc.Published ? Common.CmsPublishStatus.PublishStatus.Published : Common.CmsPublishStatus.PublishStatus.Unpublished;
         doc.Created = reader.GetDateTime(9);
         doc.Updated = reader.GetDateTime(10);
         doc.Shows = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
         doc.Attachment = reader.IsDBNull(12) ? string.Empty : reader.GetString(12).Trim().ToLower();

         return doc;
      }

      /// <summary>
      /// Lee una carpeta de una fila en la base de datos.
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      private DocumentFolder ReadFolder(SqlDataReader reader, bool getFolderNumItems)
      {
         if (reader == null)
         {
            return null;
         }
         else if (reader.IsClosed)
         {
            return null;
         }
         else if (reader.FieldCount < 8)
         {
            return null;
         }

         // SELECT 
         // folderid,folderparentid,foldername,folderdesc,folderorder,foldercreated,folderenabled,foldershowtitle[,numItems]

         DocumentFolder folder = new DocumentFolder();
         folder.ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
         folder.ParentID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
         folder.Name = reader.IsDBNull(2) ? string.Empty : reader.GetString(2).Trim();
         folder.Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3).Trim();
         folder.Order = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
         folder.Created = reader.GetDateTime(5);
         folder.Enabled = reader.GetBoolean(6);
         folder.ShowTitle = reader.GetBoolean(7);
         folder.MenuId = reader.IsDBNull(8) ? string.Empty : reader.GetString(8).Trim();

         if (getFolderNumItems)
         {
            folder.Objects = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
         }

         return folder;
      }

      /// <summary>
      /// Ensure that the private folder for a object is created.
      /// </summary>
      private void EnsureObjectFolderExists(int objectId, bool isContainer)
      {
         // Create the private file system folder for store related media
         DirectoryInfo dirInfo = new DirectoryInfo(_ws.FileSystemService.GetObjectFolder(new DocumentFSID(objectId, isContainer)));
         if (!dirInfo.Exists)
         {
            dirInfo.Create();
         }
      }

      #endregion

      #region Disabled Code

      /*
      /// <summary>
      /// Obtiene los documentos destacados en una carpeta.
      /// </summary>
      /// <returns>Un array de objetos <see cref="Document"/>.</returns>
      [Obsolete]
      public string GetHighlightedDoclist()
      {
         string xhtml = string.Empty;
         string link = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         _ws.DataSource.Connect();

         try
         {
            cmd = new SqlCommand("cs_Docs_GetHighlighted", _ws.DataSource.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               link = (string)reader["docviewer"] + "?" + Cosmo.Workspace.PARAM_OBJECT_ID + "=" + (int)reader["docid"];

               xhtml += "<div class=\"media\">\n";
               xhtml += "   <a class=\"pull-left\" href=\"" + link + "\">\n";
               xhtml += "      <img class=\"media-object img-thumbnail\" alt=\"64x64\" src=\"" + Cosmo.Net.Url.Combine(_ws.FileSystemService.RootUrl, ((int)reader["docid"]).ToString(), (string)reader["docpic"]) + "\" style=\"width: 64px; height: 64px;\">\n";
               xhtml += "   </a>\n";
               xhtml += "   <div class=\"media-body\">\n";
               xhtml += "      <h4 class=\"media-heading\">" + (string)reader["doctitle"] + "</h4>\n";
               xhtml += "      <p>" + HttpUtility.HtmlDecode((string)reader["docdesc"]) + "</p>\n";
               xhtml += "      <p><a class=\"btn btn-default btn-xs\" href=\"" + link + "\" role=\"button\">Leer »</a></p>\n";
               xhtml += "   </div>\n";
               xhtml += "</div>\n";
               xhtml += "<hr />\n";
            }
            reader.Close();

            return xhtml;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().PropertyName + ".GetHighlightedDoclist()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            if (reader != null) reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene los documentos destacados en una carpeta usando la caché del servidor.
      /// </summary>
      /// <param name="cache">Una referencia al objeto <see cref="Cache"/> del servidor web</param>
      /// <returns>Un array de objetos <see cref="Document"/>.</returns>
      [Obsolete]
      public string GetHighlightedDoclist(System.Web.Caching.Cache cache)
      {
         // Comprueba si se debe usar la caché
         if (!_ws.Settings.GetBoolean(Cms.HomeUseCache))
         {
            return GetHighlightedDoclist();
         }

         try
         {
            if (cache[Cms.CACHE_DOCS_HIGHLIGHT] == null)
            {
               cache.Insert(Cms.CACHE_DOCS_HIGHLIGHT,
                            GetHighlightedDoclist(),
                            null,
                            DateTime.Now.AddSeconds(_ws.Settings.GetInt(Cms.HomeCacheTimeout, 500)),
                            TimeSpan.Zero);
            }

            return cache[Cms.CACHE_DOCS_HIGHLIGHT].ToString();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().PropertyName + ".GetHighlightedDoclist()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
      }

      /// <summary>
      /// Obtiene los documentos destacados en una carpeta.
      /// </summary>
      /// <param name="folderid">Identificador de la carpeta</param>
      /// <returns>Un array de objetos <see cref="Document"/>.</returns>
      [Obsolete]
      public string GetHighlightedDoclist(int folderid)
      {
         string xhtml = string.Empty;
         string link = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         _ws.DataSource.Connect();

         try
         {
            cmd = new SqlCommand("cs_Docs_GetFolderHighlighted", _ws.DataSource.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@folderId", folderid));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               link = (string)reader["docviewer"] + "?" + Cosmo.Workspace.PARAM_OBJECT_ID + "=" + (int)reader["docid"];

               xhtml += "<div class=\"media\">\n";
               xhtml += "   <a class=\"pull-left\" href=\"" + link + "\">\n";
               xhtml += "      <img class=\"media-object img-thumbnail\" alt=\"64x64\" src=\"" + Cosmo.Net.Url.Combine(_ws.FileSystemService.RootUrl, ((int)reader["docid"]).ToString(), (string)reader["docpic"]) + "\" style=\"width: 64px; height: 64px;\">\n";
               xhtml += "   </a>\n";
               xhtml += "   <div class=\"media-body\">\n";
               xhtml += "      <h4 class=\"media-heading\">" + (string)reader["doctitle"] + "</h4>\n";
               xhtml += "      <p>" + HttpUtility.HtmlDecode((string)reader["docdesc"]) + "</p>\n";
               xhtml += "      <p><a class=\"btn btn-default btn-xs\" href=\"" + link + "\" role=\"button\">Leer »</a></p>\n";
               xhtml += "   </div>\n";
               xhtml += "</div>\n";
               xhtml += "<hr />\n";

               // xhtml += "<div class=\"hlbox\">\n";
               // xhtml += "<span class=\"title\">" + (string)reader["doctitle"] + "</span><br />\n";
               // xhtml += "<a href=\"" + link + "\"><img class=\"thumb\" src=\"" + Cosmo.Net.Url.Combine(_ws.FileSystemUrl(), ((int)reader["docid"]).ToString(), (string)reader["docpic"]) + "\" alt=\"" + (string)reader["doctitle"] + "\" /></a><br />";
               // xhtml += HttpUtility.HtmlDecode((string)reader["docdesc"]) + "&nbsp;<a href=\"" + link + "\">Leer</a>";
               // xhtml += "</div>\n";
            }

            return xhtml;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().PropertyName + ".GetHighlightedDoclist()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            if (!reader.IsClosed) reader.Close();

            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Genera una fuente RSS 2.0 que contiene los últimos documentos publicados
      /// </summary>
      /// <returns>Una cadena que representa el canal RSS solicitado</returns>
      public string ToRss()
      {
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         RssChannel rss = null;

         _ws.DataSource.Connect();

         try
         {
            // Implementa las propiedades del canal
            rss = new RssChannel();
            rss.Title = _ws.PropertyName + " - " + DocumentDAO.SERVICE_NAME;
            rss.Copyright = "Copyright &copy; " + _ws.PropertyName;
            rss.Language = "es-es";
            rss.ManagingEditor = _ws.Mail;
            rss.LastBuildDate = DateTime.Now;
            rss.PubDate = DateTime.Now;
            rss.Link = _ws.Url;
            rss.Image = new RssChannelImage();
            rss.Image.Title = _ws.PropertyName;
            rss.Image.Url = new Uri(Cosmo.Net.Url.Combine(_ws.Url, "images", "cs_rss_image.jpg"));
            rss.Image.Link = _ws.Url;

            // Rellena las entradas de noticias al canal
            cmd = new SqlCommand("cs_RSS_GetDocuments", _ws.DataSource.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               RssItem item = new RssItem();
               item.Link = _ws.Url + reader.GetString(4) + "?id=" + reader.GetInt32(0);
               item.Title = reader.GetString(1);
               item.Description = (String.IsNullOrEmpty(reader.GetString(8)) ? string.Empty : "<img src=\"" + _ws.FileSystemService.GetFileURL(reader.GetInt32(0).ToString(), reader.GetString(8)) + "\" alt=\"Imagen miniatura\" /><br />") + reader.GetString(2);
               item.PubDate = reader.GetDateTime(3);
               item.Guid = item.Link;
               item.Category = reader.GetString(6);
               rss.Items.Add(item);
            }
            reader.Close();

            return rss.ToXml();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().PropertyName + ".ToXML()", 
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
      /// Genera una fuente RSS 2.0 que contiene los últimos documentos publicados usando la caché de servidor web
      /// </summary>
      /// <param name="cache">La instáncia Cache del servidor web</param>
      /// <param name="forceUpdate">Indica si se debe forzar al refresco del contenido del canal si usar la caché.</param>
      /// <returns>Una cadena que representa el canal RSS solicitado</returns>
      public string ToRss(System.Web.Caching.Cache cache, bool forceUpdate)
      {
         // Calcula el número de segundos de validez
         int secs = 3600;
         if (!int.TryParse(_ws.Settings.GetString(Cms.RSSCacheTimeout, "3600"), out secs)) secs = 3600;

         // Guarda el feed en caché
         if (cache[Cms.CACHE_RSS_DOCUMENTS] == null || forceUpdate)
            cache.Insert(Cms.CACHE_RSS_DOCUMENTS, ToRss(), null, DateTime.Now.AddSeconds(secs), TimeSpan.Zero);

         // Devuelve el contenido de la caché
         return cache[Cms.CACHE_RSS_DOCUMENTS].ToString();
      }
      
      /// <summary>
      /// Configura una instancia de CSLayoutNavbar con la ruta de acceso a la carpeta
      /// </summary>
      /// <param name="navbar">Instancia de CSLayoutNavbar</param>
      /// <param name="folderId">Identificador de la carpeta</param>
      /// <param name="showFoldersAtLateral">Indica si se deben mostrar las carpetas en el lateral</param>
      public void FolderNavigationRoute(CSNavbar navbar, int folderId, bool showFoldersAtLateral)
      {
         int actfolder = folderId;
         int parentid = 0;
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         List<CSNavbarLinkItem> items = new List<CSNavbarLinkItem>();

         _ws.DataSource.Connect();

         try
         {
            // Agrega las carpetas, de inferior a superior
            while (actfolder > 0)
            {
               sql = "SELECT " +
                        "folderid, foldername, folderparentid " +
                     "FROM " + 
                        SQL_TABLE_FOLDERS + " " +
                     "WHERE " +
                        "folderid=@folderid";

               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@folderid", actfolder));
               reader = cmd.ExecuteReader();
               if (reader.Read())
               {
                  if (actfolder == folderId)
                  {
                     items.Add(new CSNavbarLinkItem(reader.GetString(1), NavbarItemPosition.Left));
                     parentid = reader.GetInt32(2);
                  }
                  else
                     items.Add(new CSNavbarLinkItem(reader.GetString(1), DocumentDAO.URL_CONTENT_FOLDER_VIEW + "?" + Cosmo.Workspace.PARAM_FOLDER_ID + "=" + reader.GetInt32(0) + (showFoldersAtLateral ? "&lat=1" : ""), NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));
                  actfolder = reader.GetInt32(2);
               }
               reader.Close();
            }

            // Agrega el inicio
            items.Add(new CSNavbarLinkItem("Inicio", Workspace.COSMO_URL_LOGIN, NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));

            // Ordena los elementos añadidos del último al primero
            navbar.Clear();
            for (int i = items.Count - 1; i >= 0; i--)
            {
               navbar.Items.Add(items[i]);
            }

            // Agrega el comando Volver (navega a la carpeta de nivel superior)
            /*if (parentid > 0)
               navbar.Items.Add(new CSNavbarLinkItem("Volver", DocumentDAO.URL_CONTENT_FOLDER_VIEW + "?" + DocumentDAO.PARAM_FOLDERID + "=" + parentid + (showFoldersAtLateral ? "&lat=1" : ""), NavbarItemPosition.Right, NavbarLinkDestination.InSameWindow));
            else
               navbar.Items.Add(new CSNavbarLinkItem("Volver", DocumentDAO.URL_HOME, NavbarItemPosition.Right, NavbarLinkDestination.InSameWindow));
            * /
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().PropertyName + ".FolderNavigationRoute(CSLayoutNavbar, int, bool)", 
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
      /// Configura una instancia de CSLayoutNavbar con la ruta de acceso a la carpeta
      /// </summary>
      /// <param name="navbar">Instancia de CSLayoutNavbar</param>
      /// <param name="folderId">Identificador de la carpeta</param>
      public void FolderNavigationRoute(CSNavbar navbar, int folderId)
      {
         this.FolderNavigationRoute(navbar, folderId, false);
      }

      /// <summary>
      /// Configura una instancia de CSLayoutNavbar con la ruta de acceso a un documento
      /// </summary>
      /// <param name="navbar">Instancia de CSLayoutNavbar</param>
      /// <param name="folderId">Identificador del documento</param>
      /// <param name="showAtLateral">Indica si la carpeta que contiene el documento muestra las carpetas en la parte lateral</param>
      public void DocumentNavigationRoute(CSNavbar navbar, int documentId, bool showFoldersAtLateral)
      {
         int actfolder = 0;
         int parentid = 0;
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         List<CSNavbarLinkItem> items = new List<CSNavbarLinkItem>();

         _ws.DataSource.Connect();

         try
         {
            // Agrega el nombre del documento
            sql = "SELECT doctitle,docfolder " +
                  "FROM " + SQL_TABLE_OBJECTS + " " +
                  "WHERE docid=@docid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@docid", documentId));
            reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
               throw new Exception("El documento #" + documentId + " no se encuentra o no está disponible.");
            }

            items.Add(new CSNavbarLinkItem(reader.GetString(0), NavbarItemPosition.Left));
            parentid = reader.GetInt32(1);
            reader.Close();

            // Agrega las carpetas, de inferior a superior
            actfolder = parentid;
            while (actfolder > 0)
            {
               sql = "SELECT folderid, foldername, folderparentid " +
                     "FROM " + SQL_TABLE_FOLDERS + " " +
                     "WHERE folderid=@folderid";
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@folderid", actfolder));

               reader = cmd.ExecuteReader();
               if (reader.Read())
               {
                  items.Add(new CSNavbarLinkItem(reader.GetString(1), DocumentDAO.URL_CONTENT_FOLDER_VIEW + "?" + Cosmo.Workspace.PARAM_FOLDER_ID + "=" + reader.GetInt32(0) + (showFoldersAtLateral ? "&lat=1" : ""), NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));
                  actfolder = reader.GetInt32(2);
               }
               reader.Close();
            }

            // Agrega el inicio
            items.Add(new CSNavbarLinkItem("Inicio", Workspace.COSMO_URL_DEFAULT, NavbarItemPosition.Left, NavbarLinkDestination.InSameWindow));

            // Ordena los elementos añadidos del último al primero
            navbar.Clear();
            for (int i = items.Count - 1; i >= 0; i--)
            {
               navbar.Items.Add(items[i]);
            }

            // Agrega el comando Volver (navega a la carpeta contenedora)
            /*if (parentid > 0)
               navbar.Items.Add(new CSNavbarLinkItem("Volver", DocumentDAO.URL_CONTENT_FOLDER_VIEW + "?" + DocumentDAO.PARAM_FOLDERID + "=" + parentid + (showFoldersAtLateral ? "&lat=1" : ""), NavbarItemPosition.Right, NavbarLinkDestination.InSameWindow));
            else
               navbar.Items.Add(new CSNavbarLinkItem("Volver", DocumentDAO.URL_HOME, NavbarItemPosition.Right, NavbarLinkDestination.InSameWindow));
            * /
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().PropertyName + ".DocumentNavigationRoute(CSLayoutNavbar, int, bool)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            if (!reader.IsClosed) reader.Close();

            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Configura una instancia de CSLayoutNavbar con la ruta de acceso a un documento
      /// </summary>
      /// <param name="navbar">Instancia de CSLayoutNavbar</param>
      /// <param name="folderId">Identificador del documento</param>
      public void DocumentNavigationRoute(CSNavbar navbar, int documentId)
      {
         this.DocumentNavigationRoute(navbar, documentId, false);
      }
      */

      #endregion

   }
}