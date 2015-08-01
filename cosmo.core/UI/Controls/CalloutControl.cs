namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente Callout de Bootstrap.
   /// </summary>
   public class CalloutControl : Control
   {
      // Internal data declarations
      private ComponentColorScheme _type;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="CalloutControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public CalloutControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="CalloutControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="type">Tipo de mensaje a mostrar.</param>
      public CalloutControl(View parentView, ComponentColorScheme type)
         : base(parentView)
      {
         Initialize();

         this.Type = type;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el título del control.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or sets el texto del control.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or sets el código del icono a mostrar junto al título.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Gets or sets el estilo de presentación del control.
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
      /// Initializes the instance data.
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
