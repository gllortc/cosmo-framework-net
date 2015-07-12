namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un control de tipo <em>badge</em>, insertable en diversos controles.
   /// </summary>
   public class BadgeControl : Control
   {
      // Internal data declarations
      private ComponentColorScheme _type;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="BadgeControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public BadgeControl(View parentView) : 
         base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="BadgeControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="text">Texto que contiene el control.</param>
      /// <param name="type">Tipo (color) de mensaje a representar.</param>
      /// <param name="roundedBorders">Indica si se debe representar con los bordes redondeados.</param>
      public BadgeControl(View parentView, string text, ComponentColorScheme type, bool roundedBorders) :
         base(parentView)
      {
         Initialize();

         Text = text;
         RoundedBorders = roundedBorders;
         _type = type;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el contenido del <em>badge</em>.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Indica si el <em>badge</em> debe tener los extremos redondeados (<c>true</c>) o cuadrados (<c>false</c>).
      /// </summary>
      public bool RoundedBorders { get; set; }

      /// <summary>
      /// Gets or sets el color del componente.
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
         RoundedBorders = true;
         Text = string.Empty;
         _type = ComponentColorScheme.Normal;
      }

      #endregion

   }
}
