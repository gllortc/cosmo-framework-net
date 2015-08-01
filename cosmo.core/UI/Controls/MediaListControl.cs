using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una lista de imágenes que puede usarse como lista de cualquier elemento que contenga un <em>thumbnail</em>.
   /// </summary>
   public class MediaListControl : Control
   {

      #region Enumerations

      /// <summary>
      /// Enumera los estilos de presentación del componente <see cref="MediaListControl"/>.
      /// </summary>
      public enum MediaListStyle
      {
         /// <summary>Una lista de imágenes miniatura sin nada más. Adecuado para galerias de imágenes.</summary>
         Thumbnail,
         /// <summary>Una lista de imágenes miniatura con texto asociado.</summary>
         Media
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="MediaListControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public MediaListControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si se debe usar un separador de elementos (dependerá de la plantilla y/o renderizador).
      /// </summary>
      public bool UseItemSeparator { get; set; }

      /// <summary>
      /// Gets or sets el estilo de representación de la lista.
      /// </summary>
      public MediaListStyle Style { get; set; }

      /// <summary>
      /// Gets or sets la lista de elementos.
      /// </summary>
      public List<MediaItem> Items { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Limpia la lista de contenido y la deja completamente vacía.
      /// </summary>
      public void Clear()
      {
         this.Items.Clear();
      }

      /// <summary>
      /// Agrega un elemento al final de la lista.
      /// </summary>
      /// <param name="thumbnail">Una instancia de <see cref="MediaItem"/> que representa el elemento a insertar.</param>
      public void Add(MediaItem thumbnail)
      {
         this.Items.Add(thumbnail);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.UseItemSeparator = false;
         this.Style = MediaListStyle.Media;
         this.Items = new List<MediaItem>();
      }

      #endregion

   }
}
