using Cosmo.Utils;
using System.Collections.Generic;
using System.Xml;

namespace Cosmo.UI.Menu.Impl
{
   /// <summary>
   /// Implementa un proveedor de menús de aplicación que obtiene las distintas opciones del archivo de configuración del workspace.
   /// </summary>
   public class StaticMenuProviderImpl : IMenuProvider
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="StaticMenuProviderImpl"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public StaticMenuProviderImpl(Workspace workspace, Plugin plugin) 
         : base(workspace, plugin)
      {
         MenuItems = LoadMenu(plugin.XmlPluginNode);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Carga el menú a partir de la información XML contenida dentro de la definición de Plugin.
      /// </summary>
      private List<MenuItem> LoadMenu(XmlNode xml)
      {
         MenuItem item = null;
         List<MenuItem> menu = new List<MenuItem>();

         foreach (XmlNode node in xml.ChildNodes)
         {
            if (node.Name.Equals(IMenuProvider.XML_MENUITEM_TAG))
            {
               item = new MenuItem();
               item.ID = node.Attributes[IMenuProvider.XML_MENUITEM_ATTRIB_ID].Value;
               item.Name = node.Attributes[IMenuProvider.XML_MENUITEM_ATTRIB_NAME].Value;
               item.Href = node.Attributes[IMenuProvider.XML_MENUITEM_ATTRIB_HREF].Value;
               item.Icon = (node.Attributes[IMenuProvider.XML_MENUITEM_ATTRIB_ICON]) == null ? string.Empty : node.Attributes[IMenuProvider.XML_MENUITEM_ATTRIB_ICON].Value;
               item.SetRoles(node.Attributes[IMenuProvider.XML_MENUITEM_ATTRIB_ROLES] == null ? string.Empty : node.Attributes[IMenuProvider.XML_MENUITEM_ATTRIB_ROLES].Value);

               if (node.Attributes[IMenuProvider.XML_MENUITEM_ATTRIB_TYPE] != null)
               {
                  switch (node.Attributes[IMenuProvider.XML_MENUITEM_ATTRIB_TYPE].Value.Trim().ToLower())
                  {
                     case IMenuProvider.MENUITEM_TYPE_BRAND:
                        item.Type = MenuItem.MenuItemType.Brand;
                        break;
                     case IMenuProvider.MENUITEM_TYPE_PROFILE:
                        item.Type = MenuItem.MenuItemType.UserProfile;
                        break;
                     case IMenuProvider.MENUITEM_TYPE_PRIVATEMESSAGES:
                        item.Type = MenuItem.MenuItemType.PrivateMessages;
                        break;
                     default:
                        item.Type = MenuItem.MenuItemType.Standard;
                        break;
                  }
               }

               if (node.HasChildNodes)
               {
                  item.Subitems = LoadMenu(node);
               }

               menu.Add(item);
            }
         }

         return menu;
      }

      #endregion

   }
}
