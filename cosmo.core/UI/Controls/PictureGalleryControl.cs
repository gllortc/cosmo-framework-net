using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una galeria de imágenes.
   /// </summary>
   public class PictureGalleryControl : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public PictureGalleryControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId"></param>
      public PictureGalleryControl(ViewContainer container, string domId)
         : base(container)
      {
         Initialize();

         DomID = domId;
      }

      #endregion

      #region Properties

      public int Columns { get; set; }

      public List<PictureControl> Pictures { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         Columns = 3;
         Pictures = new List<PictureControl>();
      }

      #endregion

   }
}
