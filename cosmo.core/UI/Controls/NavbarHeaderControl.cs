namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Representa la cabecera de una barra de herramientas.
   /// </summary>
   public class NavbarHeaderControl : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="NavbarHeaderControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public NavbarHeaderControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="NavbarHeaderControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
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
      /// Devuelve o establece el texto visible que se mostrará en el elemento.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Devuelve o establece la URL para el enlace del elemento.
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Devuelve o establece el texto que aparecerá en el botón de desplegar el menú cuando la
      /// barra esté contraida (en dispositivos de pantalla pequeña).
      /// </summary>
      public string ToggleNavigationText { get; set; }

      /// <summary>
      /// Devuelve o establece la URL correspondiente a la imagen usada como logotipo del workspace.
      /// </summary>
      public string LogoImageUrl { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
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
