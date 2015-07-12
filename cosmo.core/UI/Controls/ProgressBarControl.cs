namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente PopOver de Bootstrap.
   /// </summary>
   public class ProgressBarControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PopoverControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public ProgressBarControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="PopoverControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="percentage"></param>
      /// <param name="color"></param>
      public ProgressBarControl(View parentView, int percentage, ComponentColorScheme color)
         : base(parentView)
      {
         Initialize();

         this.Percentage = percentage;
         this.Color = color;
      }

      /// <summary>
      /// Gets a new instance of <see cref="PopoverControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="percentage"></param>
      /// <param name="color"></param>
      /// <param name="description"></param>
      public ProgressBarControl(View parentView, int percentage, ComponentColorScheme color, string description)
         : base(parentView)
      {
         Initialize();

         this.Percentage = percentage;
         this.Color = color;
         this.Text = description;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el título visible del elemento.
      /// </summary>
      public int Percentage { get; set; }

      /// <summary>
      /// Gets or sets el color del elemento.
      /// </summary>
      public ComponentColorScheme Color { get; set; }

      /// <summary>
      /// Gets or sets el texto descriptivo que aparece junto a la barra del progreso.
      /// </summary>
      public string Text { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Color = ComponentColorScheme.Normal;
         this.Percentage = 50;
         this.Text = string.Empty;
      }

      #endregion

   }
}
