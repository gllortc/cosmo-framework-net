using Cosmo.UI.Controls;
using Cosmo.Utils;
using System.Collections.Generic;

namespace Cosmo.UI.Menu
{
   /// <summary>
   /// Abstract class that must be implemented by menu provider modules.
   /// </summary>
   public abstract class MenuProvider
   {
      // Internal data declarations
      private Plugin _plugin;
      private Workspace _ws;
      private List<MenuItem> _menu;

      #region Constants

      /// <summary>Tag XML que contiene la definición del elemento de menú</summary>
      public const string XML_MENUITEM_TAG = "menuitem";
      /// <summary>Atributo XML que contiene el identificador del elemento de menú</summary>
      public const string XML_MENUITEM_ATTRIB_ID = "id";
      /// <summary>Atributo XML que contiene el nombre del elemento de menú</summary>
      public const string XML_MENUITEM_ATTRIB_NAME = "name";
      /// <summary>Atributo XML que contiene el enlace del elemento de menú</summary>
      public const string XML_MENUITEM_ATTRIB_HREF = "href";
      /// <summary>Atributo XML que contiene el icono del elemento de menú</summary>
      public const string XML_MENUITEM_ATTRIB_ICON = "icon";
      /// <summary>Atributo XML que contiene el tipo de elemento de menú</summary>
      public const string XML_MENUITEM_ATTRIB_TYPE = "type";
      /// <summary>XML attribute that contains the roles authorized to view the element.</summary>
      public const string XML_MENUITEM_ATTRIB_ROLES = "roles";

      /// <summary>Tipo de elemento de menú estándar</summary>
      public const string MENUITEM_TYPE_NORMAL = "standard";
      /// <summary>Tipo de elemento de menú que contiene el logotipo de la aplicación</summary>
      public const string MENUITEM_TYPE_BRAND = "brand";
      /// <summary>Tipo de elemento de menú para perfil de usuario</summary>
      public const string MENUITEM_TYPE_PROFILE = "usrprofile";
      /// <summary>Tipo de elemento de menú para mensajes privados</summary>
      public const string MENUITEM_TYPE_PRIVATEMESSAGES = "usrmessages";

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="MenuProvider"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      protected MenuProvider(Workspace workspace, Plugin plugin)
      {
         _ws = workspace;
         _plugin = plugin;
         _menu = new List<MenuItem>();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve una instancia del workspace actual.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
      }

      /// <summary>
      /// Devuelve una instancia del workspace actual.
      /// </summary>
      public Plugin Plugin 
      {
         get { return _plugin; }
      }

      /// <summary>
      /// Devuelve el nombre (ID) del módulo.
      /// </summary>
      public string ID
      {
         get { return _plugin.ID; }
      }

      /// <summary>
      /// Gets or sets los elementos de menú que ha obtenido el proveedor.
      /// </summary>
      public List<MenuItem> MenuItems
      {
         get { return _menu; }
         set { _menu = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Genera un menú usando un control <see cref="SidebarControl"/>.
      /// </summary>
      /// <param name="parentViewport">Owner view that contains the current menu.</param>
      /// <param name="activeMenuItemId">Identificador del elemento activo actualmente.</param>
      /// <returns>Una instancia de <see cref="SidebarControl"/> que representa el menú solicitado.</returns>
      public SidebarControl GenerateSidebar(View parentViewport, string activeMenuItemId)
      {
         SidebarButton btn;

         SidebarControl sidebar = new SidebarControl(parentViewport);

         foreach (MenuItem item in _menu)
         {
            btn = ConvertToSidebarButton(parentViewport, item, activeMenuItemId);
            if (btn != null)
            {
               sidebar.Buttons.Add(btn);
            }
         }

         return sidebar;
      }

      /// <summary>
      /// Genera un menú usando un control <see cref="NavbarControl"/>.
      /// </summary>
      /// <param name="parentViewport">Owner view that contains the current menu.</param>
      /// <param name="activeMenuItemId">Una cadena que contiene el identificador del elemento de menú que debe aparecer activo.</param>
      /// <returns>Una instancia de <see cref="NavbarControl"/> convenientemente configurada y lista para ser renderizada.</returns>
      public NavbarControl GenerateNavbar(View parentViewport, string activeMenuItemId)
      {
         NavbarIButtonControl btn;

         NavbarControl navbar = new NavbarControl(parentViewport, true);

         // Obtiene el elemento Brand (si lo tiene)
         MenuItem brand = GetBrand(_menu);
         if (brand != null)
         {
            navbar.Header = new NavbarHeaderControl(parentViewport, this.Workspace.Name, Workspace.COSMO_URL_DEFAULT);
            navbar.Header.LogoImageUrl = brand.Icon;
         }

         // Añade el resto de elementos de menú
         foreach (MenuItem item in _menu)
         {
            btn = ConvertToNavbarButton(parentViewport, item, activeMenuItemId);
            if (btn != null)
            {
               navbar.AddItem(btn);
            }
         }

         return navbar;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Convierte la instancia <see cref="MenuItem"/> en <see cref="SidebarButton"/>.
      /// </summary>
      private SidebarButton ConvertToSidebarButton(View parentViewport, MenuItem item, string activeMenuItemId)
      {
         SidebarButton sbtn;

         // Convierte la instancia MenuItem en SidebarButton
         SidebarButton btn = new SidebarButton(parentViewport);
         btn.Text = item.Name;
         btn.Href = item.Href;
         btn.Icon = item.Icon;
         btn.Active = (activeMenuItemId.Equals(item.ID));
         btn.Roles = item.Roles;

         // Agrega las subopciones mediante recursividad
         foreach (MenuItem subitem in item.Subitems)
         {
            sbtn = ConvertToSidebarButton(parentViewport, subitem, activeMenuItemId);
            if (sbtn != null)
            {
               if (sbtn.Active) btn.Active = true;
               btn.SubItems.Add(sbtn);
            }
         }

         return btn;
      }

      /// <summary>
      /// Convert a <see cref="MenuItem"/> instante to another instance of <see cref="NavbarIButtonControl"/> to use in navbar control.
      /// </summary>
      private NavbarIButtonControl ConvertToNavbarButton(View parentViewport, MenuItem item, string activeMenuItemId)
      {
         NavbarIButtonControl btn;
         NavbarIButtonControl sbtn;

         switch (item.Type)
         {
            case MenuItem.MenuItemType.Brand:
               // No devuelve nada (se trata a parte mediante el método GetBrand())
               return null;
            case MenuItem.MenuItemType.UserProfile:
               btn = new NavbarLoginItem(parentViewport);
               break;
            case MenuItem.MenuItemType.PrivateMessages:
               btn = new NavbarPrivateMessagesItem(parentViewport);
               break;
            default:
               btn = new NavbarIButtonControl(parentViewport);
               break;
         }

         btn.Caption = item.Name;
         btn.Href = item.Href;
         btn.Icon = item.Icon;
         btn.Active = (activeMenuItemId.Equals(item.ID));

         // Agrega las subopciones mediante recursividad
         foreach (MenuItem subitem in item.Subitems)
         {
            sbtn = ConvertToNavbarButton(parentViewport, subitem, activeMenuItemId);
            if (sbtn != null)
            {
               if (sbtn.Active) btn.Active = true;
               btn.AddSubitem(sbtn);
            }
         }

         return btn;
      }

      /// <summary>
      /// Obtiene el elemento de menú que contiene el logotipo de la aplicación.
      /// </summary>
      private MenuItem GetBrand(List<MenuItem> menu)
      {
         MenuItem brand = null;

         foreach (MenuItem item in menu)
         {
            if (item.Type == MenuItem.MenuItemType.Brand)
            {
               return item;
            }

            if (item.Subitems.Count > 0)
            {
               brand = GetBrand(item.Subitems);

               if (brand != null)
               {
                  return brand;
               }
            }
         }

         return null;
      }

      #endregion

   }
}
