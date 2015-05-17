using Cosmo.Utils;
using System.Collections.Generic;

namespace Cosmo.UI.Menu.Impl
{
   /// <summary>
   /// Implementa un proveedor de menús de aplicación que obtiene las opciones de una base de datos.
   /// </summary>
   public class DbMenuProviderImpl : IMenuProvider
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="DbMenuProviderImpl"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public DbMenuProviderImpl(Workspace workspace, Plugin plugin) 
         : base(workspace, plugin)
      {
         // Obtiene las secciones del site
         MenuDAO sdao = new MenuDAO(this.Workspace);
         List<MenuItem> sections = sdao.GetSectionsMenu();
      }
   }
}
