namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una pestaña que puede contener controles.
   /// </summary>
   public class TabItemControl : Control, IControlSingleContainer
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="TabbedContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public TabItemControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="TabbedContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="caption"></param>
      public TabItemControl(View parentView, string domId, string caption)
         : base(parentView)
      {
         Initialize();

         this.DomID = domId;
         this.Caption = caption;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el texto visible de la pestaña.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Indica si es la pestaña activa.
      /// </summary>
      public bool Active { get; set; }

      /// <summary>
      /// Gets or sets el color de la pestaña.
      /// </summary>
      public ComponentColorScheme Color { get; set; }

      /// <summary>
      /// Lista de controles que contiene la pestaña.
      /// </summary>
      public ControlCollection Content { get; set; }

      /// <summary>
      /// Gets or sets código del icono que aparecerá junto al título de la pestaña.
      /// </summary>
      public string Icon { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Active = false;
         this.Caption = string.Empty;
         this.Icon = string.Empty;
         this.Content = new ControlCollection();
         this.Color = ComponentColorScheme.Normal;
      }

      #endregion

   }
}
