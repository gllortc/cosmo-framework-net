namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implements the Jumbotron control.
   /// </summary>
   /// <remarks>
   /// http://getbootstrap.com/components/#jumbotron
   /// </remarks>
   public class JumbotronControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="JumbotronControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public JumbotronControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="JumbotronControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public JumbotronControl(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the title of Jumbotron.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or sets the subtitle text.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets the background image.
      /// </summary>
      public string BackgroundImage { get; set; }

      /// <summary>
      /// Gets or sets the text color.
      /// </summary>
      public string ForeColor { get; set; }

      /// <summary>
      /// Gets or sets the button text.
      /// </summary>
      public string ButtonText { get; set; }

      /// <summary>
      /// Gets or sets the button URL (to generate the button link).
      /// </summary>
      public string ButtonHref { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance.
      /// </summary>
      private void Initialize()
      {
         this.Title = string.Empty;
         this.Description = string.Empty;
         this.BackgroundImage = string.Empty;
         this.ForeColor = string.Empty;
         this.ButtonText = string.Empty;
         this.ButtonHref = string.Empty;
      }

      #endregion

   }
}
