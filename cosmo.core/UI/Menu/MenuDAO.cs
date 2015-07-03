using Cosmo.Data.Connection;
using Cosmo.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Cosmo.UI.Menu
{
   /// <summary>
   /// Gestor de las secciones del site.
   /// </summary>
   public class MenuDAO
   {
      private const string SQL_TABLE_NAME = "sections";
      private const string SQL_SELECT_SECTION = "SECTIONID, SECTIONNAME, SECTIONFILE, SECTPUBLIC, SECTHTML, SECTORDER, SECTOWNER, SECTCREATED";

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="MenuDAO"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      public MenuDAO(Workspace workspace)
      {
         this.Workspace = workspace;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Una instancia que hace referencia al workspace actual.
      /// </summary>
      private Workspace Workspace { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve las secciones del site.
      /// </summary>
      /// <returns>Una lista de instancias de <see cref="MenuItem"/>.</returns>
      public List<MenuItem> GetSectionsMenu()
      {
         string sql;
         MenuItem section = null;
         List<MenuItem> sections = new List<MenuItem>();
         SqlCommand cmd = null;

         sql = "SELECT " + SQL_SELECT_SECTION + " " +
               "FROM " + SQL_TABLE_NAME + " " +
               "WHERE " +
               " SECTPUBLIC=1 " +
               "ORDER BY " +
               " SECTORDER ASC, " +
               " SECTIONID ASC";

         try
         {
            Workspace.DataSource.Connect();

            cmd = new SqlCommand(sql, Workspace.DataSource.Connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  section = ReadSection(reader);
                  if (section != null) sections.Add(section);
               }
            }

            return sections;
         }
         catch (Exception ex)
         {
            LoggerService.Add(Workspace, new LogEntry(this.GetType().Name + ".GetSectionsMenu()", 
                                                      ex.Message, 
                                                      LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            Workspace.DataSource.Disconnect();
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicialización de la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Workspace = null;
      }

      /// <summary>
      /// Lee una sección de la base de datos.
      /// </summary>
      private MenuItem ReadSection(SqlDataReader reader)
      {
         MenuItem section = null;

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
         else
         {  
            section = new MenuItem();
            section.ID = reader.GetInt32(0).ToString();
            section.Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            section.Href = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            // section.IsPublished = reader.GetBoolean(3);
            section.Icon = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            // section.SortOrder = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
            // section.Owner = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
            // section.Created = reader.IsDBNull(7) ? DateTime.Now : reader.GetDateTime(7);
         }

         return section;
      }

      #endregion

      #region Disabled Code

      // /// <summary>
      // /// Devuelve el código XHTML para generar el menú de la aplicación.
      // /// </summary>
      // /// <param name="cache">Una instancia de la caché del servidor web.</param>
      // /// <returns>El código XHTML para enviar al cliente</returns>
      // /// <remarks>
      // /// Esta forma usa la caché de servidor web para ahorrar consultas a la BBDD.
      // /// </remarks>
      /*public string GetSectionsMenu(System.Web.Caching.Cache cache)
      {
         // Comprueba si se debe usar la caché
         if (!_ws.Settings.GetBoolean(CSWebsiteSettings.HomeUseCache, false)) return GetSectionsMenu();

         try
         {
            if (cache[CSWebsiteCache.CACHE_HOME_MENU] == null)
            {
               cache.Insert(CSWebsiteCache.CACHE_HOME_MENU,
                            GetSectionsMenu(),
                            null,
                            DateTime.Now.AddSeconds(_ws.Settings.GetInt(CSWebsiteSettings.HomeCacheTimeout, 500)),
                            TimeSpan.Zero);
            }

            return cache[CSWebsiteCache.CACHE_HOME_MENU].ToString();
         }
         catch (Exception ex)
         {
            Logger.Add(_ws, new LogEntry(CSWebsite.ProductName, "CSHomePage.GetSectionsMenu(System.Web.Caching.Cache)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
      }*/

      #endregion

   }
}