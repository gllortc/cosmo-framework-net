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
      /// Devuelve una instancia de <see cref="MediaItem"/>.
      /// </summary>
      public MediaItem() 
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el título que tendrá el elemento.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Devuelve o establece el texto usado a modo de descripción del elemento.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Devuelve o establece el icono que se debe mostrar junto al título.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Devuelve o establece la ruta y el nombre del archivo (relativo a la página actual) que se mostrará
      /// en formato miniatura.
      /// </summary>
      public string Image { get; set; }

      /// <summary>
      /// Devuelve o establece el ancho de la imagen (en píxels)
      /// </summary>
      public int ImageWidth { get; set; }

      /// <summary>
      /// Devuelve o establece la altura de la imagen (en píxels).
      /// </summary>
      public int ImageHeight { get; set; }

      /// <summary>
      /// Devuelve o establece la URL a la que se invocará al hacer clic en el elemento.
      /// </summary>
      public string LinkHref { get; set; }

      /// <summary>
      /// Devuelve o establece el texto visible en el enlace del elemento.
      /// </summary>
      public string LinkCaption { get; set; }

      internal MediaListControl.MediaListStyle Style { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
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
