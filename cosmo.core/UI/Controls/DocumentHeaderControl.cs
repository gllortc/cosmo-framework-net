namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un control para incrustar en la cabecera de un documento y que permite mostrar
   /// el título del contenido y una descripción.
   /// </summary>
   public class DocumentHeaderControl : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="PageHeaderControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public DocumentHeaderControl(ViewContainer container)
         : base(container)
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
      /// Devuelve o establece el código del icono que acompaña el título.
      /// </summary>
      public string Icon { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
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
