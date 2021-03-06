﻿namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Representa la cabecera de una barra de herramientas.
   /// </summary>
   public class NavbarHeaderControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="NavbarHeaderControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public NavbarHeaderControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="NavbarHeaderControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="caption">Texto visible del elemento.</param>
      /// <param name="href">URL para el enlace del elemento.</param>
      public NavbarHeaderControl(View parentView, string caption, string href)
         : base(parentView)
      {
         Initialize();

         this.Caption = caption;
         this.Href = href;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el texto visible que se mostrará en el elemento.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Gets or sets la URL para el enlace del elemento.
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Gets or sets el texto que aparecerá en el botón de desplegar el menú cuando la
      /// barra esté contraida (en dispositivos de pantalla pequeña).
      /// </summary>
      public string ToggleNavigationText { get; set; }

      /// <summary>
      /// Gets or sets la URL correspondiente a la imagen usada como logotipo del workspace.
      /// </summary>
      public string LogoImageUrl { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Caption = string.Empty;
         this.Href = string.Empty;
         this.ToggleNavigationText = "Toggle navigation";
         this.LogoImageUrl = string.Empty;
      }

      #endregion

   }
}
