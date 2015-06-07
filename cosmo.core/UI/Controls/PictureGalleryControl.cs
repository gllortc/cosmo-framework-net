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
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public PictureGalleryControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId"></param>
      public PictureGalleryControl(View parentView, string domId)
         : base(parentView)
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
