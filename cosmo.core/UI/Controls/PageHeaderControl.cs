using System;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un control para incrustar en la cabecera de un documento o página y que permite mostrar
   /// el título del contenido, un breadcrumb, etc.
   /// </summary>
   public class PageHeaderControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="PageHeaderControl"/>.
      /// </summary>
      /// <param name="parentView">Owner view of the control.</param>
      public PageHeaderControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el título de la página.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Devuelve o establece el subtítulo de la página.
      /// </summary>
      public string SubTitle { get; set; }

      /// <summary>
      /// Devuelve o establece el icono que acompaña el título.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Devuelve o establece el componente que permite integrar una ruta a la cabecera de página.
      /// </summary>
      public BreadcrumbControl Breadcrumb { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance.
      /// </summary>
      private void Initialize()
      {
         this.Title = string.Empty;
         this.SubTitle = string.Empty;
         this.Icon = string.Empty;
         this.Breadcrumb = null;
      }

      #endregion

   }
}
