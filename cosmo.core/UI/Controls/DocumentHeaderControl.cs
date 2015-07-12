﻿namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un control para incrustar en la cabecera de un documento y que permite mostrar
   /// el título del contenido y una descripción.
   /// </summary>
   public class DocumentHeaderControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="DocumentHeaderControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public DocumentHeaderControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el título de la página.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or sets el subtítulo de la página.
      /// </summary>
      public string SubTitle { get; set; }

      /// <summary>
      /// Gets or sets el código del icono que acompaña el título.
      /// </summary>
      public string Icon { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Title = string.Empty;
         this.SubTitle = string.Empty;
         this.Icon = string.Empty;
      }

      #endregion

   }
}
