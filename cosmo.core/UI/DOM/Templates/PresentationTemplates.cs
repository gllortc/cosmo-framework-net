using Cosmo.Diagnostics;
using Cosmo.FileSystem;
using Cosmo.Security.Auth;
using Cosmo.Utils;
using Cosmo.Utils.IO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Xml;

namespace Cosmo.UI.DOM.Templates
{

   /// <summary>
   /// Implementa una clase de servicio para gestionar las plantillas de presentación
   /// </summary>
   public class PresentationTemplates
   {
      private Workspace _ws = null;

      /// <summary>
      /// Devuelve una instancia de PresentationTemplates
      /// </summary>
      /// <param name="workspace">Una instáncia de Workspace que representa el workspace actual</param>
      public PresentationTemplates(Workspace workspace)
      {
         _ws = workspace;
      }

      #region Methods

      /// <summary>
      /// Devuelve una lista de plantillas de presentación filtrada por tipo de plataforma que se encuentran instaladas en el workspace
      /// </summary>
      /// <param name="type">Tipo de plataforma</param>
      /// <returns></returns>
      public List<PresentationTemplate> List(PresentationRule.DeviceTypes type)
      {
         string sql = "";
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         List<PresentationTemplate> templates = new List<PresentationTemplate>();

         try
         {
            _ws.DataSource.Connect();

            // Rellena el control
            sql = "SELECT id,name,description,platformtype,usepageheadders,created,updated,owner,renderer FROM systemplates " +
                  "WHERE platformtype=" + (int)type + " " +
                  "ORDER BY name";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               PresentationTemplate template = new PresentationTemplate();
               template.ID = (int)reader[0];
               template.Name = (string)reader[1];
               template.Description = !reader.IsDBNull(2) ? (string)reader[2] : "";
               template.Type = (PresentationRule.DeviceTypes)((int)reader[3]);
               template.UsePageHeaders = (bool)reader[4];
               template.Created = (DateTime)reader[5];
               template.Updated = (DateTime)reader[6];
               template.Owner = !reader.IsDBNull(7) ? (string)reader[7] : "[System]";
               template.Renderer = !reader.IsDBNull(8) ? (string)reader[8] : "";

               templates.Add(template);
            }
            reader.Close();

            return templates;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(AssemblyInfo.Product(), "Cosmo.Workspace.PresentationTemplates.List", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve una lista de plantillas de presentación que se encuentran instaladas en el workspace
      /// </summary>
      /// <returns></returns>
      public List<PresentationTemplate> List()
      {
         string sql = "";
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         List<PresentationTemplate> templates = new List<PresentationTemplate>();

         try
         {
            _ws.DataSource.Connect();

            // Rellena el control
            sql = "SELECT id,name,description,platformtype,usepageheadders,created,updated,owner,renderer FROM systemplates ORDER BY name";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               PresentationTemplate template = new PresentationTemplate();
               template.ID = (int)reader[0];
               template.Name = (string)reader[1];
               template.Description = !reader.IsDBNull(2) ? (string)reader[2] : "";
               template.Type = (PresentationRule.DeviceTypes)((int)reader[3]);
               template.UsePageHeaders = (bool)reader[4];
               template.Created = (DateTime)reader[5];
               template.Updated = (DateTime)reader[6];
               template.Owner = !reader.IsDBNull(7) ? (string)reader[7] : "[System]";
               template.Renderer = !reader.IsDBNull(8) ? (string)reader[8] : "";

               templates.Add(template);
            }
            reader.Close();

            return templates;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(AssemblyInfo.Product(), "Cosmo.Workspace.PresentationTemplates.List", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Permite obtener una plantilla completa.
      /// </summary>
      /// <param name="id">Identificador de la plantilla.</param>
      /// <returns>Una instáncia de MWTemplate.</returns>
      public PresentationTemplate Item(int id)
      {
         int parts = 0;
         string sql = "";
         PresentationTemplate template = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            // Abre una conexión a la BBDD
            _ws.DataSource.Connect();

            // Obtiene las propiedades de la plantilla
            sql = "SELECT id,name,description,platformtype,usepageheadders,created,updated,owner,renderer " +
                  "FROM systemplates " +
                  "WHERE id=@id";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@id", id));
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               template = new PresentationTemplate();
               template.ID = (int)reader[0];
               template.Name = (string)reader[1];
               template.Description = !reader.IsDBNull(2) ? (string)reader[2] : "";
               template.Type = (PresentationRule.DeviceTypes)((int)reader[3]);
               template.UsePageHeaders = (bool)reader[4];
               template.Created = (DateTime)reader[5];
               template.Updated = (DateTime)reader[6];
               template.Owner = !reader.IsDBNull(7) ? (string)reader[7] : "[System]";
               template.Renderer = !reader.IsDBNull(8) ? (string)reader[8] : "";
            }
            reader.Close();

            if (template != null)
            {
               // Obtiene los fragmentos de plantilla
               sql = "SELECT Count(*) As regs FROM systemplateparts WHERE templateid=@templateid";
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               cmd.Parameters.Add(new SqlParameter("@templateid", id));
               parts = (int)cmd.ExecuteScalar();

               if (parts > 0)
               {
                  template.Parts = new List<PresentationTemplatePart>();

                  sql = "SELECT part,contents " +
                        "FROM systemplateparts " +
                        "WHERE templateid=@templateid " +
                        "ORDER BY part";
                  cmd = new SqlCommand(sql, _ws.DataSource.Connection);
                  cmd.Parameters.Add(new SqlParameter("@templateid", id));
                  reader = cmd.ExecuteReader();
                  while (reader.Read())
                  {
                     template.Parts.Add(new PresentationTemplatePart((PresentationTemplatePart.DOMTemplateParts)reader[0],
                                                           !reader.IsDBNull(1) ? (string)reader[1] : ""));
                  }
                  reader.Close();
               }
               else
                  template.Parts = new List<PresentationTemplatePart>();
            }

            return template;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(AssemblyInfo.Product(), "Cosmo.Workspace.PresentationTemplates.Item", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Agrega una nueva plantilla.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="PresentationTemplate"/> que contiene la plantilla a agregar.</param>
      public void Add(ref PresentationTemplate template)
      {
         string sql;
         SqlCommand cmd;

         template.Owner = (string.IsNullOrEmpty(template.Owner) ? AuthenticationService.ACCOUNT_SUPER : template.Owner.Trim().ToUpper());

         using (TransactionScope ts = new TransactionScope())
         {
            _ws.DataSource.Connect();

            // Averigua si ya existe una plantilla con el mismo nombre
            sql = "SELECT Count(*) AS nregs " +
                  "FROM systemplates " +
                  "WHERE Lower(name)=@name";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@name", template.Name.Trim().ToLower()));
            if ((int)cmd.ExecuteScalar() > 0)
            {
               cmd.Dispose();
               _ws.DataSource.Disconnect();
               throw new Exception("Ya existe una plantilla con el mismo nombre.");
            }

            // Añade el registro a la tabla SYSFORMATRULES
            sql = "INSERT INTO systemplates (name,description,platformtype,usepageheadders,created,updated,owner,renderer) " +
                  "VALUES (@name,@description,@platformtype,@usepageheadders,GetDate(),GetDate(),@owner,@renderer)";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@name", template.Name));
            cmd.Parameters.Add(new SqlParameter("@description", template.Description));
            cmd.Parameters.Add(new SqlParameter("@platformtype", (int)template.Type));
            cmd.Parameters.Add(new SqlParameter("@usepageheadders", template.UsePageHeaders));
            cmd.Parameters.Add(new SqlParameter("@owner", template.Owner));
            cmd.Parameters.Add(new SqlParameter("@renderer", template.Renderer));
            cmd.ExecuteNonQuery();

            // Averigua el ID de la nueva plantilla
            sql = "SELECT id " +
                  "FROM systemplates " +
                  "WHERE Lower(name)=@name";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@name", template.Name));
            template.ID = (int)cmd.ExecuteScalar();

            // Si dispone de partes, las añade
            if (template.Parts != null)
            {
               foreach (PresentationTemplatePart part in template.Parts)
               {
                  sql = "INSERT INTO systemplateparts (templateid,part,contents) " +
                        "VALUES (@templateid,@part,@contents)";
                  cmd = new SqlCommand(sql, _ws.DataSource.Connection);
                  cmd.Parameters.Add(new SqlParameter("@templateid", template.ID));
                  cmd.Parameters.Add(new SqlParameter("@part", part.ID));
                  cmd.Parameters.Add(new SqlParameter("@contents", part.Html));
                  cmd.ExecuteNonQuery();
               }
            }

            // CIerra la conexión con la BBDD
            _ws.DataSource.Disconnect();

            // Completa la transacción
            ts.Complete();
         }
      }

      /// <summary>
      /// Actualiza los datos de una plantilla.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="PresentationTemplate"/> que contiene los datos actualizados.</param>
      public void Update(PresentationTemplate template)
      {
         string sql;
         SqlCommand cmd;
         SqlTransaction trans = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();

            // Inicia la transacción
            trans = _ws.DataSource.Connection.BeginTransaction();

            // Averigua si ya existe una plantilla con el mismo agente
            sql = "SELECT Count(*) AS nregs " +
                  "FROM systemplates " +
                  "WHERE Lower(name)=@name And id<>@id";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
            cmd.Parameters.Add(new SqlParameter("@name", template.Name.ToLower()));
            cmd.Parameters.Add(new SqlParameter("@id", template.ID));
            if ((int)cmd.ExecuteScalar() > 0)
            {
                cmd.Dispose();
                trans.Rollback();
                _ws.DataSource.Disconnect();
                throw new Exception("Ya existe una plantilla con el mismo nombre.");
            }

            // Genera la senténcia SQL
            sql = "UPDATE systemplates " +
                  "SET name=@name," +
                      "description=@description," +
                      "platformtype=@platformtype," +
                      "usepageheadders=@usepageheadders," +
                      "renderer=@renderer," +
                      "updated=GetDate() " +
                  "WHERE id=@id";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
            cmd.Parameters.Add(new SqlParameter("@name", template.Name));
            cmd.Parameters.Add(new SqlParameter("@description", template.Description));
            cmd.Parameters.Add(new SqlParameter("@platformtype", (int)template.Type));
            cmd.Parameters.Add(new SqlParameter("@usepageheadders", template.UsePageHeaders));
            cmd.Parameters.Add(new SqlParameter("@renderer", template.Renderer));
            cmd.Parameters.Add(new SqlParameter("@id", template.ID));
            cmd.ExecuteNonQuery();

            // Actualiza los fragmentos de la plantilla
            if (template.Parts != null)
            {
               // Elimina los fragmentos actuales
               sql = "DELETE FROM systemplateparts " +
                     "WHERE templateid=@templateid";
               cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
               cmd.Parameters.Add(new SqlParameter("@templateid", template.ID));
               cmd.ExecuteNonQuery();

               // Añade los nuevos
               foreach (PresentationTemplatePart part in template.Parts)
               {
                  sql = "INSERT INTO systemplateparts (templateid,part,contents) " +
                        "VALUES (@templateid,@part,@contents)";
                  cmd = new SqlCommand(sql, _ws.DataSource.Connection, trans);
                  cmd.Parameters.Add(new SqlParameter("@templateid", template.ID));
                  cmd.Parameters.Add(new SqlParameter("@part", part.ID));
                  cmd.Parameters.Add(new SqlParameter("@contents", part.Html));
                  cmd.ExecuteNonQuery();
               }
            }

            // Confirma la transacción
            trans.Commit();
         }
         catch (Exception ex)
         {
            trans.Rollback();

            _ws.Logger.Add(new LogEntry(AssemblyInfo.Product(), "Cosmo.Workspace.PresentationTemplates.Update", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            trans.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina la plantilla y sus partes asociadas.
      /// </summary>
      /// <param name="id">Identificador de la plantilla.</param>
      public void Delete(int id)
      {
         string sql = "";
         SqlCommand cmd = null;
         SqlTransaction transaction = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _ws.DataSource.Connect();
            transaction = _ws.DataSource.Connection.BeginTransaction();

            // Elimina todas las partes de la plantilla
            sql = "DELETE FROM systemplateparts " +
                  "WHERE templateid=@templateid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@templateid", id));
            cmd.ExecuteNonQuery();

            // Elimina la plantilla
            sql = "DELETE FROM systemplates " +
                  "WHERE id=@id";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.ExecuteNonQuery();

            transaction.Commit();
         }
         catch (Exception ex)
         {
            transaction.Rollback();

            _ws.Logger.Add(new LogEntry(AssemblyInfo.Product(), "Cosmo.Workspace.PresentationTemplates.Delete", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            // Cierra la conexión a la BBDD
            transaction.Dispose();
            _ws.DataSource.Disconnect();
         }

         // Elimina la carpeta propiedad de la plantilla
         try
         {
            DirectoryInfo folder = new DirectoryInfo(Path.Combine(_ws.FileSystemService.ApplicationPath, Path.Combine("templates", id.ToString())));
            FileInfo[] files = folder.GetFiles();
            foreach (FileInfo file in files) file.Delete();
            folder.Delete();
         }
         catch
         { }
      }

      /// <summary>
      /// Instala la plantilla dentro del workspace.
      /// </summary>
      /// <param name="template">Una instáncia de <see cref="PresentationTemplate"/> que contiene los datos de la plantilla</param>
      public void Install(PresentationTemplate template)
      {
         try
         {
            // Registra la nueva plantilla
            PresentationTemplates templates = new PresentationTemplates(_ws);
            templates.Add(ref template);

            // Genera la carpeta de la plantilla
            DirectoryInfo folder = new DirectoryInfo(_ws.FileSystemService.ApplicationPath);
            if (!folder.Exists)
            {
               throw new Exception("No se encuentra o no está accesible la carpeta raíz del workspace (" + _ws.FileSystemService.ApplicationPath + ").");
            }

            folder.CreateSubdirectory("templates");
            folder.CreateSubdirectory(Path.Combine("templates", "shared"));
            folder.CreateSubdirectory(Path.Combine("templates", template.ID.ToString()));

            // Copia los archivos a las carpetas correspondientes
            foreach (PresentationTemplateFile tfile in template.Files)
            {
               // Se situa en la carpeta de destino
               switch (tfile.Destination)
               {
                  case WorkspaceFolders.ServerRoot:
                     folder = new DirectoryInfo(_ws.FileSystemService.ApplicationPath);
                     break;
                  case WorkspaceFolders.ServerBin:
                     folder = new DirectoryInfo(Path.Combine(_ws.FileSystemService.ApplicationPath, "bin"));
                     break;
                  case WorkspaceFolders.ServerTemplatesPrivate:
                     folder = new DirectoryInfo(Path.Combine(_ws.FileSystemService.ApplicationPath, Path.Combine("templates", template.ID.ToString())));
                     break;
                  default:
                     folder = null;
                     break;
               }

               // Si la carpeta de destino existe, guarda el archivo
               if (folder != null && folder.Exists) tfile.Save(folder.FullName);
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".Install()", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
      }

      /// <summary>
      /// Instala la plantilla dentro del workspace.
      /// </summary>
      /// <param name="filename">Nombre y ruta del paquete que contiene la plantilla.</param>
      public void Install(string filename)
      {
         try
         {
            // Carga la plantilla desde el archivo
            PresentationTemplate template = PresentationTemplates.Import(filename);

            // Instala la plantilla
            this.Install(template);
         }
         catch
         {
            throw;
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Exporta una plantilla de presentación a un archivo MWT (XML).
      /// </summary>
      /// <param name="template">Una instancia de <see cref="PresentationTemplate"/> que contiene los datos de la plantilla de presentación a exportar.</param>
      /// <param name="filename">Nombre y ruta del archivo de destino.</param>
      /// <remarks>
      /// - Para exportar, se deben especificar los archivos a empaquetar (propiedad Files).
      /// - Si el archivo existe, se sobreescribe.
      /// </remarks>
      public static void Export(PresentationTemplate template, string filename)
      {
         try
         {
            // Comprueba que el renderer haya sido incluido y se halle en la carpeta correcta
            bool found = false;
            if (!string.IsNullOrWhiteSpace(template.Renderer))
            {
               foreach (PresentationTemplateFile file in template.Files)
               {
                  if (template.Renderer.ToLower().Equals(file.Name.ToLower()))
                  {
                     if (file.Destination == WorkspaceFolders.ServerBin)
                     {
                        found = true;
                        break;
                     }
                  }
               }

               if (!found)
               {
                  throw new Exception("Se ha especificado una libreria de renderizado de la plantilla pero o bien no se ha incluido en el paquete o bien no se ha ubicado en la carpeta \bin del servidor.");
               }
            }

            // Abre el documento
            XmlTextWriter writer = new XmlTextWriter(filename, Encoding.UTF8);
            writer.WriteStartDocument();

            // Abre una cláusula OBJECT y agrega sus atributos
            writer.WriteStartElement("object");
            writer.WriteAttributeString("type", PresentationTemplate.OBJECT_ID);
            writer.WriteAttributeString("version", PresentationTemplate.OBJECT_VER);
            writer.WriteAttributeString("generator", Assembly.GetExecutingAssembly().FullName);

            // Características de la aplicación
            writer.WriteElementString("name", template.Name);
            writer.WriteElementString("version", template.Version);
            writer.WriteElementString("description", template.Description);
            writer.WriteElementString("author", template.Author);
            writer.WriteElementString("platform", ((int)template.Type).ToString());
            writer.WriteElementString("renderer", template.Renderer);

            // Datos de la licencia
            writer.WriteStartElement("license");
            writer.WriteEndElement();

            // Datos de los módulos exportados
            writer.WriteStartElement("parts");
            foreach (PresentationTemplatePart part in template.Parts)
            {
               writer.WriteStartElement("part");
               writer.WriteAttributeString("id", ((int)part.ID).ToString());
               writer.WriteValue(part.Html);
               writer.WriteEndElement();
            }
            writer.WriteEndElement();

            // Archivos
            writer.WriteStartElement("files");
            foreach (PresentationTemplateFile pfile in template.Files)
            {
               FileInfo file = new FileInfo(pfile.FileName);
               if (file.Exists)
               {
                  // Lee el archivo
                  // FileStream stream = file.OpenRead();
                  // byte[] buffer = new byte[stream.Length];
                  // stream.Read(buffer, 0, buffer.Length);
                  // stream.Close();

                  // Lee el archivo
                  byte[] buffer = FileReader.LoadTextFileToByteArray(file.FullName);

                  writer.WriteStartElement("appfile");
                  writer.WriteAttributeString("name", pfile.Name);
                  writer.WriteAttributeString("destination", ((int)pfile.Destination).ToString());
                  writer.WriteAttributeString("length", buffer.Length.ToString());
                  writer.WriteStartElement("bin");
                  writer.WriteAttributeString("xmlns:dt", "urn:schemas-microsoft-com:datatypes");
                  writer.WriteAttributeString("dt:dt", "bin.base64");
                  writer.WriteBase64(buffer, 0, buffer.Length);
                  writer.WriteEndElement();
                  writer.WriteEndElement();
               }
            }
            writer.WriteEndElement();

            // Cierra OBJECT
            writer.WriteEndElement();

            // Cierra el documento
            writer.WriteEndDocument();
            writer.Close();
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Importa una plantilla de presentación al workspace.
      /// </summary>
      /// <param name="filename">Nombre y ruta del paquete a importar.</param>
      /// <returns>Una instancia de WSTemplate que contiene los datos de la plantilla.</returns>
      public static PresentationTemplate Import(string filename)
      {
         PresentationTemplate template = new PresentationTemplate();

         try
         {
            // Comprueba que exista el archivo
            FileInfo file = new FileInfo(filename);
            if (!file.Exists)
               throw new Exception("No se encuentra el archivo " + filename);

            // Carga el archivo XML
            XmlTextReader reader = new XmlTextReader(filename);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            reader.Close();

            // Comprueba que el paquete corresponda a una definición correcta
            XmlNode xnod = xmlDoc.DocumentElement;
            if (!xnod.Name.ToLower().Equals("object"))
               throw new Exception("El archivo XML proporcionado no corresponde a una definición de paquete de instalación de Cosmo.");
            if (!xnod.Attributes["type"].Value.ToString().ToLower().Equals(PresentationTemplate.OBJECT_ID.ToLower()))
               throw new Exception("El archivo XML proporcionado no corresponde a una definición de paquete de instalación de Cosmo.");

            // Lee las propiedades principales
            foreach (XmlNode node in xnod.ChildNodes)
            {
               switch (node.Name.ToLower())
               {
                  case "name":
                     template.Name = node.FirstChild == null ? "" : node.FirstChild.Value.ToString();
                     break;

                  case "description":
                     template.Description = node.FirstChild == null ? "" : node.FirstChild.Value.ToString();
                     break;

                  case "version":
                     template.Version = node.FirstChild == null ? "" : node.FirstChild.Value.ToString();
                     break;

                  case "author":
                     template.Author = node.FirstChild == null ? "" : node.FirstChild.Value.ToString();
                     break;

                  case "renderer":
                     template.Renderer = node.FirstChild == null ? "" : node.FirstChild.Value.ToString();
                     break;

                  case "parts":
                     if (node.ChildNodes == null || node.ChildNodes.Count <= 0)
                     {
                        template.Parts = new List<PresentationTemplatePart>();
                        break;
                     }
                     template.Parts = new List<PresentationTemplatePart>();
                     foreach (XmlNode part in node.ChildNodes)
                     {
                        if (part.Name.ToLower().Equals("part"))
                        {
                           PresentationTemplatePart tpart = new PresentationTemplatePart();
                           foreach (XmlAttribute var in part.Attributes)
                           {
                              switch (var.Name.ToLower())
                              {
                                 case "id":
                                    tpart.ID = (PresentationTemplatePart.DOMTemplateParts)int.Parse(var.Value);
                                    break;
                              }
                           }
                           tpart.Html = part.FirstChild == null ? "" : part.FirstChild.Value.ToString();
                           template.Parts.Add(tpart);
                        }
                     }
                     break;

                  case "files":
                     if (node.ChildNodes == null || node.ChildNodes.Count <= 0)
                     {
                        template.Files = new List<PresentationTemplateFile>();
                        break;
                     }
                     template.Files = new List<PresentationTemplateFile>();
                     foreach (XmlNode xfile in node.ChildNodes)
                     {
                        PresentationTemplateFile tfile = new PresentationTemplateFile();
                        foreach (XmlAttribute fileatt in xfile.Attributes)
                        {
                           switch (fileatt.Name.ToLower())
                           {
                              case "name":
                                 tfile.Name = fileatt.Value;
                                 break;
                              case "destination":
                                 tfile.Destination = (WorkspaceFolders)int.Parse(fileatt.Value);
                                 break;
                              case "length":
                                 tfile.Length = int.Parse(fileatt.Value);
                                 break;
                           }
                        }
                        if (xfile.ChildNodes.Count > 0)
                        {
                           tfile.Buffer = new byte[tfile.Length];
                           tfile.Buffer = Convert.FromBase64String(xfile.ChildNodes[0].FirstChild.Value);
                        }
                        template.Files.Add(tfile);
                     }
                     break;
               }
            }

            return template;
         }
         catch
         {
            throw;
         }
      }

      #endregion

   }
}
