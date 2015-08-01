using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un panel con pestañas.
   /// </summary>
   public class TabbedContainerControl : Control, IControlCollectionContainer
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="TabbedContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public TabbedContainerControl(View parentView) 
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Lista de pestañas que contiene el control.
      /// </summary>
      public List<TabItemControl> TabItems { get; set; }

      /// <summary>
      /// Colección de controles contenedores que son la base del control.
      /// </summary>
      public List<IControlSingleContainer> NestedContainers 
      { 
         get 
         {
            List<IControlSingleContainer> containers = new List<IControlSingleContainer>();
            foreach (IControlSingleContainer container in this.TabItems)
            {
               containers.Add(container);
            }
            return containers;
         } 
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.TabItems = new List<TabItemControl>();
      }

      #endregion

   }
}
