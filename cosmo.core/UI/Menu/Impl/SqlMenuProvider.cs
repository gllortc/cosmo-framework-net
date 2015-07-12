using Cosmo.Utils;
using System.Collections.Generic;

namespace Cosmo.UI.Menu.Impl
{
   /// <summary>
   /// Implementa un proveedor de menús de aplicación que obtiene las opciones de una base de datos.
   /// </summary>
   public class SqlMenuProvider : MenuProvider
   {
      /// <summary>
      /// Gets a new instance of <see cref="SqlMenuProvider"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public SqlMenuProvider(Workspace workspace, Plugin plugin) 
         : base(workspace, plugin)
      {
         // Obtiene las secciones del site
         MenuDAO sdao = new MenuDAO(this.Workspace);
         List<MenuItem> sections = sdao.GetSectionsMenu();
      }
   }
}
