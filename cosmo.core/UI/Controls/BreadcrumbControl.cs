using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente <strong>Breadcrumb</strong>.
   /// Detalles: http://getbootstrap.com/components/#breadcrumbs
   /// </summary>
   public class BreadcrumbControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="BreadcrumbControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public BreadcrumbControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets la lista de elementos de la ruta.
      /// </summary>
      public List<BreadcrumbItem> Items { get; set; }

      #endregion

      #region Public Methods

      /// <summary>
      /// Establece el último elemento de la ruta como activo.
      /// </summary>
      public void SetLastActive()
      {
         if (Items.Count > 1)
         {
            Items[Items.Count - 1].Href = string.Empty;
            Items[Items.Count - 1].IsActive = true;
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         Items = new List<BreadcrumbItem>();
      }

      #endregion

   }
}
