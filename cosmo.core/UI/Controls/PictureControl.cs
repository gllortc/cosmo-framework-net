namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una imagen representable en un control <see cref="PictureGalleryControl"/>.
   /// </summary>
   public class PictureControl : Control
   {
      // Internal data declarations
      private SplitButtonControl menu;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PictureControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public PictureControl(View parentView) : 
         base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="PictureControl"/>.
      /// </summary>
      /// <param name="parentView">The view that contains the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public PictureControl(View parentView, string domId) :
         base(parentView)
      {
         Initialize();

         DomID = domId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets en ancho (en píxeles) la imagen miniatura.
      /// </summary>
      public int Width { get; set; }

      /// <summary>
      /// Gets or sets la URL del enlace que representa la imagen miniatura.
      /// </summary>
      public string ImageHref { get; set; }

      /// <summary>
      /// Gets or sets la URL de la imagen miniatura.
      /// </summary>
      public string ImageUrl { get; set; }

      /// <summary>
      /// Gets or sets el texto alternativo que se mostrará en lugar de la imagen (propiedad Alt).
      /// </summary>
      public string ImageAlternativeText { get; set; }

      /// <summary>
      /// Gets or sets el texto a mostrar bajo la imagen.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or sets the content to show in the footer section.
      /// </summary>
      /// <remarks>
      /// If a <see cref="Control"/> is used, it will be rendered, otherwise the render shown
      /// a <c>ToString()</c> result.
      /// </remarks>
      public object Footer { get; set; }

      /// <summary>
      /// Gets or sets el color del cuadro de la imagen (dependerá del renderizador dónde se aplique el color).
      /// </summary>
      public ComponentColorScheme Type { get; set; }

      /// <summary>
      /// Gets a boolean value indicatin if action menú is used.
      /// </summary>
      public bool HasSplitButton
      {
         get { return (this.menu != null); }
      }

      /// <summary>
      /// Gets or sets the SplitButton (menú) associated with the control.
      /// </summary>
      public SplitButtonControl SplitButton
      {
         get 
         {
            if (this.menu == null)
            {
               this.menu = new SplitButtonControl(this.ParentView);
            }
            return this.menu;
         }
         set { this.menu = value; }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         Width = 250;
         ImageHref = string.Empty;
         ImageUrl = string.Empty;
         ImageAlternativeText = string.Empty;
         Text = string.Empty;
         Footer = string.Empty;
         Type = ComponentColorScheme.Normal;

         this.menu = null;
      }

      #endregion

   }
}
