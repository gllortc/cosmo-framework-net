using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el control Sidebar (barra lateral de menú).
   /// </summary>
   public class SidebarControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="SidebarControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public SidebarControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region properties

      /// <summary>
      /// Gets or sets el texto que se mostrará en la etiqueta (<c>badge</c>).
      /// </summary>
      public List<SidebarButton> Buttons{ get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Buttons = new List<SidebarButton>();
      }

      #endregion

   }
}
