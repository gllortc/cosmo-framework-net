namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un elemento que muestra un thumbnail. Está pensado para imágenes pero se usa para
   /// cualquier elemento como documentos, etc.
   /// </summary>
   public class MediaItem 
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="MediaItem"/>.
      /// </summary>
      public MediaItem() 
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el título que tendrá el elemento.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or sets el texto usado a modo de descripción del elemento.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets el icono que se debe mostrar junto al título.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Gets or sets la ruta y el nombre del archivo (relativo a la página actual) que se mostrará
      /// en formato miniatura.
      /// </summary>
      public string Image { get; set; }

      /// <summary>
      /// Gets or sets el ancho de la imagen (en píxels)
      /// </summary>
      public int ImageWidth { get; set; }

      /// <summary>
      /// Gets or sets la altura de la imagen (en píxels).
      /// </summary>
      public int ImageHeight { get; set; }

      /// <summary>
      /// Gets or sets la URL a la que se invocará al hacer clic en el elemento.
      /// </summary>
      public string LinkHref { get; set; }

      /// <summary>
      /// Gets or sets el texto visible en el enlace del elemento.
      /// </summary>
      public string LinkCaption { get; set; }

      internal MediaListControl.MediaListStyle Style { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Icon = string.Empty;
         this.Image = string.Empty;
         this.ImageWidth = 0;
         this.ImageHeight = 0;
         this.Title = string.Empty;
         this.Description = string.Empty;
         this.LinkHref = string.Empty;
         this.LinkCaption = string.Empty;
         this.Style = MediaListControl.MediaListStyle.Media;
      }

      #endregion

   }
}
