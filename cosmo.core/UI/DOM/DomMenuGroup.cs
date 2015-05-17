using System.Collections.Generic;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un grupo de elementos de menú.
   /// </summary>
   public class DomMenuGroup
   {
      private string _id;
      private string _caption;
      private List<DomMenuItem> _menuitems;

      /// <summary>
      /// Devuelve una instancia de MWDomMenuGroup.
      /// </summary>
      public DomMenuGroup()
      {
         _id = string.Empty;
         _caption = string.Empty;
         _menuitems = new List<DomMenuItem>();
      }

      /// <summary>
      /// Devuelve una instancia de MWDomMenuGroup.
      /// </summary>
      public DomMenuGroup(string ID, string Caption)
      {
         _id = ID;
         _caption = Caption;
         _menuitems = new List<DomMenuItem>();
      }

      #region Properties

      /// <summary>
      /// Identificador del grupo.
      /// </summary>
      public string ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Nombre (texto descriptivo) del grupo.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Elementos de menú que contiene el grupo.
      /// </summary>
      public List<DomMenuItem> MenuItems
      {
         get { return _menuitems; }
         set { _menuitems = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Añade un elemento de menú al grupo.
      /// </summary>
      /// <param name="menuitem">Instáncia de MWDomMenuItem que contiene los datos del elemento de menú.</param>
      public void AddMenuItem(DomMenuItem menuitem)
      {
         _menuitems.Add(menuitem);
      }

      /// <summary>
      /// Limpia la lista de elementos de menú del grupo.
      /// </summary>
      public void ClearMenuItems()
      {
         _menuitems = new List<DomMenuItem>();
      }

      #endregion

   }

}
