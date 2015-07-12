using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Componente UI que implementa un árbol desplegable.
   /// </summary>
   public class TreeViewControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="TreeViewControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public TreeViewControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="TreeViewControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public TreeViewControl(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si se debe renderizar el control con los nodos colapsados.
      /// </summary>
      public bool Collapsed { get; set; }

      /// <summary>
      /// Gets or sets la lista de elementos hijo que penden de la rama principal del árbol.
      /// </summary>
      public List<TreeViewChildItemControl> ChildItems { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Collapsed = false;
         this.ChildItems = new List<TreeViewChildItemControl>();
      }

      #endregion

   }
}
