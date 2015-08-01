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
      /// Gets a new instance of <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public PictureGalleryControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
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
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         Columns = 3;
         Pictures = new List<PictureControl>();
      }

      #endregion

   }
}
