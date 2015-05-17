using Cosmo.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un menú DOM.
   /// </summary>
   public class DomMenu : DomLayoutComponentBase
   {
      string _title;
      string _description;
      List<DomMenuGroup> _groups;
      System.Web.Caching.Cache _cache;

      #region Constants

      /// <summary>Cabecera del menú</summary>
      internal const string SECTION_HEAD = "menu-header";
      /// <summary>Cabecera de un grupo del menú</summary>
      internal const string SECTION_GROUP_HEAD = "menu-grp-header";
      /// <summary>Elemento de menú</summary>
      internal const string SECTION_MENUITEM = "menu-item";
      /// <summary>Pié de un grupo del menú</summary>
      internal const string SECTION_GROUP_FOOT = "menu-grp-footer";
      /// <summary>Pié del menú</summary>
      internal const string SECTION_FOOT = "menu-footer";

      /// <summary>Tag: ID de plantilla</summary>
      public const string TAG_TEMPLATE_ID = "tid";
      /// <summary>Tag: Título a mostrar del menú</summary>
      public const string TAG_MENU_NAME = "mname";
      /// <summary>Tag: Descripción del menú</summary>
      public const string TAG_MENU_DESCRIPTION = "mdesc";
      /// <summary>Tag: Título del grupo</summary>
      public const string TAG_GROUP_CAPTION = "gtitle";
      /// <summary>Tag: Descripción del grupo</summary>
      public const string TAG_GROUP_DESCRIPTION = "gdesc";
      /// <summary>Tag: Número de elemento (en grupo)</summary>
      public const string TAG_GROUP_ITEM = "gitem";
      /// <summary>Tag: Número de elementos (en grupo)</summary>
      public const string TAG_GROUP_ITEMS = "gitems";
      /// <summary>Tag: Título del elemento</summary>
      public const string TAG_ITEM_CAPTION = "icaption";
      /// <summary>Tag: URL del elemento de menú</summary>
      public const string TAG_ITEM_HREF = "ihref";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomMenu"/>.
      /// </summary>
      public DomMenu() : base()
      {
         _title = string.Empty;
         _description = string.Empty;
         _groups = new List<DomMenuGroup>();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomMenu"/>.
      /// </summary>
      /// <param name="cache">Una instancia de <see cref="System.Web.Caching.Cache"/> que permite cachear el componente si este es cacheable.</param>
      public DomMenu(System.Web.Caching.Cache cache) : base()
      {
         _cache = cache;
         _title = string.Empty;
         _description = string.Empty;
         _groups = new List<DomMenuGroup>();
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "menu"; }
      }

      /// <summary>
      /// Devuelve o establece el titulo del menú.
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Descripción del menú.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Grupos de opciones contenidos en el menú.
      /// </summary>
      public List<DomMenuGroup> Groups
      {
         get { return _groups; }
         set { _groups = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Añade un nuevo grupo al menú.
      /// </summary>
      /// <param name="group"></param>
      public void AddGroup(DomMenuGroup group) 
      {
         _groups.Add(group);
      }

      /// <summary>
      /// Elimina todos los grupos del menú.
      /// </summary>
      public void ClearGroups()
      {
         _groups = new List<DomMenuGroup>();
      }

      /// <summary>
      /// Reinicializa el menú y lo deja listo para crear uno nuevo.
      /// </summary>
      public void Clear()
      {
         _title = string.Empty;
         _groups = new List<DomMenuGroup>();
      }

      /// <summary>
      /// Carga el menú desde el Workspace.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el workspace para el que se desea generar el menú.</param>
      public void LoadMenu(Workspace workspace)
      {
         LoadMenu(workspace, null, 0);
      }

      /// <summary>
      /// Carga el menú desde el Workspace.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el workspace para el que se desea generar el menú.</param>
      /// <param name="cache">Una instancia de <see cref="System.Web.Caching.Cache"/> que permite cachear el componente.</param>
      /// <param name="cacheExpiration">Número de segundos que se mantendrá el menú en la cache antes de volver a renderizar el componente.</param>
      public void LoadMenu(Workspace workspace, System.Web.Caching.Cache cache, int cacheExpiration)
      {
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         DomMenuGroup grp = null;

         // Inicializaciones
         _groups.Clear();
         if (cache != null && cacheExpiration > 0)
         {
            _cache = cache;
            this.CacheEnabled = true;
            this.CacheExpiration = cacheExpiration;
         }
         else
         {
            _cache = null;
            this.CacheEnabled = false;
            this.CacheExpiration = 0;
         }

         grp = new DomMenuGroup("menu", (string.IsNullOrEmpty(this.Title) ? "Contenidos" : this.Title));

         try
         {
            workspace.DataSource.Connect();

            cmd = new SqlCommand("cs_Sections_GetMenu", workspace.DataSource.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               grp.AddMenuItem(new DomMenuItem((string)reader["sectionname"], (string)reader["sectionfile"]));
            }
            reader.Close();

            this.AddGroup(grp);
         }
         catch (Exception ex)
         {
            workspace.Logger.Add(new LogEntry("Cosmo.Framework", "DomMenu.LoadMenu(Workspace)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
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

            // Cabecera del menú
            part = component.GetFragment(DomMenu.SECTION_HEAD);
            part = DomContentComponentBase.ReplaceTag(part, DomMenu.TAG_MENU_NAME, this.Title);
            part = DomContentComponentBase.ReplaceTag(part, DomMenu.TAG_MENU_DESCRIPTION, this.Description);
            xhtml = string.Format("{0}{1}", xhtml, part);

            // Añade las opciones agrupadas
            foreach (DomMenuGroup group in _groups)
            {
               if (group.MenuItems.Count > 0)
               {
                  // Representa la cabecera del menú
                  part = component.GetFragment(DomMenu.SECTION_GROUP_HEAD);
                  part = DomContentComponentBase.ReplaceTag(part, DomMenu.TAG_GROUP_CAPTION, group.Caption);
                  xhtml = string.Format("{0}{1}", xhtml, part);
  
                  // Representa las opciones de menú
                  int count = 1;
                  foreach (DomMenuItem menuitem in group.MenuItems)
                  {
                     part = component.GetFragment(DomMenu.SECTION_MENUITEM);
                     part = DomContentComponentBase.ReplaceTag(part, DomMenu.TAG_ITEM_CAPTION, menuitem.Caption);
                     part = DomContentComponentBase.ReplaceTag(part, DomMenu.TAG_ITEM_HREF, menuitem.HREF);
                     part = DomContentComponentBase.ReplaceTag(part, DomMenu.TAG_GROUP_ITEM, count.ToString());
                     xhtml = string.Format("{0}{1}", xhtml, part);

                     count++;
                  }
  
                  // Representa el pie del menú
                  xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomMenu.SECTION_GROUP_FOOT));
               }

               // Reemplaza los TAGs comunes del grupo
               xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomMenu.TAG_GROUP_ITEMS, group.MenuItems.Count.ToString());
            }

            // Pie del menú
            xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomMenu.SECTION_FOOT));

            // Reemplaza los TAGs comunes del elemento
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomMenu.TAG_TEMPLATE_ID, template.ID.ToString());

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
