namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una imagen representable en un control <see cref="PictureGalleryControl"/>.
   /// </summary>
   public class PictureControl : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="PictureControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public PictureControl(ViewContainer container) : 
         base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="PictureControl"/>.
      /// </summary>
      /// <param name="id">Identificador del elemento en el documento (DOM).</param>
      public PictureControl(ViewContainer container, string domId) :
         base(container)
      {
         Initialize();

         DomID = domId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece en ancho (en píxeles) la imagen miniatura.
      /// </summary>
      public int Width { get; set; }

      /// <summary>
      /// Devuelve o establece la URL del enlace que representa la imagen miniatura.
      /// </summary>
      public string ImageHref { get; set; }

      /// <summary>
      /// Devuelve o establece la URL de la imagen miniatura.
      /// </summary>
      public string ImageUrl { get; set; }

      /// <summary>
      /// Devuelve o establece el texto alternativo que se mostrará en lugar de la imagen (propiedad Alt).
      /// </summary>
      public string ImageAlternativeText { get; set; }

      /// <summary>
      /// Devuelve o establece el texto a mostrar bajo la imagen.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Devuelve o establece el texto a mostrar al pie de la imagen.
      /// </summary>
      public string Footer { get; set; }

      /// <summary>
      /// Devuelve o establece el color del cuadro de la imagen (dependerá del renderizador dónde se aplique el color).
      /// </summary>
      public ComponentColorScheme Type { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
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
      }

      #endregion

   }
}
