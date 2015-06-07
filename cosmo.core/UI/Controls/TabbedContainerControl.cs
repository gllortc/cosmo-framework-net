using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un panel con pestañas.
   /// </summary>
   public class TabbedContainerControl : Control, IControlCollectionContainer
   {
      // Internal data declarations
      private List<TabItemControl> _tabs;

      /// <summary>
      /// Devuelve una instancia de <see cref="TabbedContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public TabbedContainerControl(View parentView) 
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Lista de pestañas que contiene el control.
      /// </summary>
      public List<TabItemControl> TabItems
      {
         get { return _tabs; }
         set { _tabs = value; }
      }

      /// <summary>
      /// Colección de controles contenedores que son la base del control.
      /// </summary>
      public List<IControlSingleContainer> NestedContainers 
      { 
         get 
         {
            List<IControlSingleContainer> containers = new List<IControlSingleContainer>();
            foreach (IControlSingleContainer container in _tabs)
            {
               containers.Add(container);
            }
            return containers;
         } 
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _tabs = new List<TabItemControl>();
      }
   }
}
