using System;
using System.Collections.Generic;

namespace Cosmo.UI.Menu
{
   /// <summary>
   /// Representa un elemento del menú de secciones del site.
   /// </summary>
   public class MenuItem
   {

      #region Enumerations

      /// <summary>
      /// Enumera los tipos de elemento de menú disponibles
      /// </summary>
      public enum MenuItemType
      {
         /// <summary>Simple menu item (link).</summary>
         Standard,
         /// <summary>Menu item that contains the logo of the site or application.</summary>
         Brand,
         /// <summary>Menu item that allows to acces to user profile (login, logoff, etc).</summary>
         UserProfile,
         /// <summary>Menu item that enable to show new private messages.</summary>
         PrivateMessages
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="MenuItem"/>.
      /// </summary>
      public MenuItem()
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="MenuItem"/>.
      /// </summary>
      /// <param name="name">Nombre de la sección.</param>
      /// <param name="href">URL de acceso a la sección.</param>
      public MenuItem(string name, string href)
      {
         Initialize();

         this.Name = name;
         this.Href = href;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the unique identifier for the menu item.
      /// </summary>
      public string ID { get; set; }

      /// <summary>
      /// Gets or sets the visible name for the menu item.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets the URL (relative or absolute) for the menu item.
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Indicate if the current menu item is currently selected.
      /// </summary>
      public bool Active { get; set; }

      /// <summary>
      /// Gets or sets the URL or the code of icon that the menu item must show.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Gets or sets the list of child menu items of the current item.
      /// </summary>
      public List<MenuItem> Subitems { get; set; }

      /// <summary>
      /// Gets or sets the type of menu item.
      /// </summary>
      public MenuItemType Type { get; set; }

      /// <summary>
      /// Gets or sets a list of profiles allowed to view the current menu item.
      /// </summary>
      public string[] Roles { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Set the roles list from a comma separated values list.
      /// </summary>
      /// <param name="rolesCsvList">Vomma separated values list of roles.</param>
      public void SetRoles(string rolesCsvList)
      {
         if (string.IsNullOrWhiteSpace(rolesCsvList))
         {
            this.Roles = new string[0];
         }
         else
         {
            this.Roles = rolesCsvList.Split(new string[1] { "," }, 
                                            StringSplitOptions.RemoveEmptyEntries);
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Convert a string value to <see cref="MenuItem.MenuItemType"/> enumeration element.
      /// </summary>
      /// <param name="strType">String value to convert.</param>
      /// <returns>An element of <see cref="MenuItem.MenuItemType"/> enumeration.</returns>
      public static MenuItem.MenuItemType ToMenuItemType(string strType)
      {
         try
         {
            return (MenuItem.MenuItemType)Enum.Parse(typeof(MenuItem.MenuItemType), strType);
         }
         catch
         {
            return MenuItem.MenuItemType.Standard;
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicialización de la instancia.
      /// </summary>
      private void Initialize()
      {
         this.ID = string.Empty;
         this.Name = string.Empty;
         this.Href = string.Empty;
         this.Icon = string.Empty;
         this.Active = false;
         this.Subitems = new List<MenuItem>();
         this.Type = MenuItemType.Standard;
      }

      #endregion

   }
}
