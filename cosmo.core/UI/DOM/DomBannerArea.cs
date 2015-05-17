using Cosmo.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un menú DOM.
   /// </summary>
   public class DomBannerArea : DomLayoutComponentBase
   {
      DomBanner.BannerPositions _position;
      List<DomBanner> _banners;
      System.Web.Caching.Cache _cache;

      #region Constants

      /// <summary>Cabecera del menú</summary>
      internal const string SECTION_HEAD = "banners-header";
      /// <summary>Elemento de menú</summary>
      internal const string SECTION_ITEM = "banner-item";
      /// <summary>Pié del menú</summary>
      internal const string SECTION_FOOT = "banners-footer";

      /// <summary>Tag: ID de plantilla</summary>
      public const string TAG_TEMPLATE_ID = "tid";
      /// <summary>Tag: Título a mostrar del menú</summary>
      public const string TAG_BANNER_ID = "banner-id";
      /// <summary>Tag: Descripción del menú</summary>
      public const string TAG_BANNER_OBJECT = "banner-object";

      private const int BANNER_ACTION_SHOW = 0;
      private const int BANNER_ACTION_CLIC = 1;

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomMenu"/>.
      /// </summary>
      public DomBannerArea() : base()
      {
         _position = DomBanner.BannerPositions.CenterTop;
         _cache = null;
         _banners = new List<DomBanner>();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomMenu"/>.
      /// </summary>
      /// <param name="cache">Una instancia de <see cref="System.Web.Caching.Cache"/> que permite cachear el componente si este es cacheable.</param>
      public DomBannerArea(System.Web.Caching.Cache cache) : base()
      {
         _position = DomBanner.BannerPositions.CenterTop;
         _cache = cache;
         _banners = new List<DomBanner>();
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "banner-area"; }
      }

      /// <summary>
      /// Devuelve o establece la posición del area a renderizar.
      /// </summary>
      public DomBanner.BannerPositions CurrentPosition
      {
         get { return _position; }
         set { _position = value; }
      }

      /// <summary>
      /// Contiene los banners a mostrar en la zona.
      /// </summary>
      public List<DomBanner> Banners
      {
         get { return _banners; }
         set { _banners = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Reinicializa el menú y lo deja listo para crear uno nuevo.
      /// </summary>
      public void Clear()
      {
         _banners = new List<DomBanner>();
      }

      /// <summary>
      /// Carga las distintas zonas publicitarias desde el workspace.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el workspace para el que se desea generar el menú.</param>
      public void LoadBanners(Workspace workspace)
      {
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         SqlParameter param = null;
         DomBanner banner = null;
         DomBanner.BannerPositions[] numbanners = new DomBanner.BannerPositions[10];

         try
         {
            workspace.DataSource.Connect();

            // Obtiene los banners activos
            cmd = new SqlCommand("cs_Banners_GetPageBanners", workspace.DataSource.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               // Obtiene el banner
               banner = new DomBanner((Int32)reader["banid"]);
               banner.Position = (DomBanner.BannerPositions)((Int32)reader["bantype"]);
               banner.Name = (string)reader["banclient"];
               banner.Filename = workspace.FileSystemService.GetFilePath("ads" + Path.DirectorySeparatorChar + ((int)reader["banid"]).ToString(), (string)reader["banimage"]);
               banner.Url = workspace.FileSystemService.GetFileURL("ads/" + ((int)reader["banid"]).ToString(), (string)reader["banimage"]);
               banner.Height = (int)reader["banheight"];
               banner.Width = (int)reader["banwidth"];
               banner.OpenInNewBrowser = (bool)reader["bandirectlink"];

               _banners.Add(banner);

               numbanners[(Int32)reader["bantype"]]++;
            }
            reader.Close();

            // Actualiza las visualizaciones
            foreach (DomBanner cbanner in _banners)
            {
               cmd = new SqlCommand("cs_Banners_DoCount", workspace.DataSource.Connection);
               cmd.CommandType = CommandType.StoredProcedure;

               param = new SqlParameter("@banner_id", SqlDbType.Int);
               param.Value = cbanner.ID;
               cmd.Parameters.Add(param);

               param = new SqlParameter("@click", SqlDbType.Int);
               param.Value = BANNER_ACTION_SHOW;
               cmd.Parameters.Add(param);

               cmd.ExecuteNonQuery();
            }
         }
         catch (Exception ex)
         {
            workspace.Logger.Add(new LogEntry("Cosmo.Framework", "DomBannerArea.LoadBanners(Workspace, Cache, int)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el elemento.</returns>
      public override string Render(DomTemplate template)
      {
         string xhtml = string.Empty;
         string part = string.Empty;

         try
         {
            // Si el elemento está en caché lo devuelve, sin comprobar si es cacheable o no
            if (_cache != null && _cache[template.GetCacheKey(ELEMENT_ROOT)] != null)
               return (string)_cache[template.GetCacheKey(ELEMENT_ROOT)];

            // Obtiene la plantilla del componente
            DomTemplateComponent component = template.GetLayoutComponent(ELEMENT_ROOT);
            if (component == null) return string.Empty;

            // Cabecera de la zona de banners
            part = component.GetFragment(DomBannerArea.SECTION_HEAD);
            part = DomContentComponentBase.ReplaceTag(part, DomBannerArea.TAG_BANNER_ID, "banners-" + _position.ToString().ToLower());
            xhtml = string.Format("{0}{1}", xhtml, part);

            // Añade las opciones agrupadas
            foreach (DomBanner banner in _banners)
            {
               // Representa la cabecera del menú
               if (banner.Position == _position)
               {
                  part = component.GetFragment(DomBannerArea.SECTION_ITEM);
                  part = DomContentComponentBase.ReplaceTag(part, DomBannerArea.TAG_BANNER_OBJECT, banner.ToXhtml());
                  xhtml = string.Format("{0}{1}", xhtml, part);
               }
            }

            // Pie del menú
            xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomBannerArea.SECTION_FOOT));

            // Reemplaza los TAGs comunes del elemento
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomBannerArea.TAG_TEMPLATE_ID, template.ID.ToString());

            // Cachea el componente si es necesario
            if (_cache != null && this.CacheEnabled)
            {
               if (_cache[template.GetCacheKey(ELEMENT_ROOT)] == null)
               {
                  _cache.Insert(template.GetCacheKey(ELEMENT_ROOT), xhtml, null, DateTime.Now.AddSeconds(this.CacheExpiration), TimeSpan.Zero);
               }
            }

            return xhtml;
         }
         catch
         {
            throw;
         }
      }

      #endregion

   }

}
