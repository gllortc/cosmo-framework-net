namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente Callout de Bootstrap.
   /// </summary>
   public class CalloutControl : Control
   {
      // Declaración de variables internas
      private ComponentColorScheme _type;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="CalloutControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public CalloutControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="CalloutControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="type">Tipo de mensaje a mostrar.</param>
      public CalloutControl(ViewContainer container, ComponentColorScheme type)
         : base(container)
      {
         Initialize();

         this.Type = type;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el título del control.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Devuelve o establece el texto del control.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Devuelve o establece el código del icono a mostrar junto al título.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Devuelve o establece el estilo de presentación del control.
      /// </summary>
      public ComponentColorScheme Type
      {
         get { return _type; }
         set 
         {
            if (value == ComponentColorScheme.Normal)
            {
               _type = ComponentColorScheme.Information;
            }
            else
            {
               _type = value;
            }
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Title = string.Empty;
         this.Text = string.Empty;
         this.Icon = null;
         this.Type = ComponentColorScheme.Information;
      }

      #endregion

   }
}
