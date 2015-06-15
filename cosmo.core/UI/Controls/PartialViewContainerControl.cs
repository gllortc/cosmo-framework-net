namespace Cosmo.UI.Controls
{
   public class PartialViewContainerControl : Control
   {
      // Internal data declarations
      private PartialView view;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PartialViewContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador único del componente dentro de la vista.</param>
      public PartialViewContainerControl(View parentView, string domId)
         : base(parentView, domId)
      {
         this.View = null;
      }

      /// <summary>
      /// Gets a new instance of <see cref="PartialViewContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="view">An instance of partial view which will be shown in this control.</param>
      public PartialViewContainerControl(View parentView, PartialView view)
         : base(parentView, view.DomID)
      {
         this.View = view;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the partial view associated with the control.
      /// </summary>
      public PartialView View { get; set; }

      #endregion

   }
}
