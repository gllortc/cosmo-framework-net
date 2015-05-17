namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente Jumbotron.
   /// </summary>
   /// <remarks>
   /// http://getbootstrap.com/components/#jumbotron
   /// </remarks>
   public class JumbotronControl : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="JumbotronControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public JumbotronControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="JumbotronControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador del componente en una vista (DOM).</param>
      public JumbotronControl(ViewContainer container, string domId)
         : base(container, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el título que tendrá el elemento.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Devuelve o establece el texto usado a modo de descripción del elemento.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Devuelve o establece el título que tendrá el elemento.
      /// </summary>
      public string BackgroundImage { get; set; }

      /// <summary>
      /// Devuelve o establece el color del texto.
      /// </summary>
      public string Color { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Title = string.Empty;
         this.Description = string.Empty;
         this.BackgroundImage = string.Empty;
         this.Color = string.Empty;
      }

      #endregion

   }
}
