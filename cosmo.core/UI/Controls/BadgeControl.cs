namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un control de tipo <em>badge</em>, insertable en diversos controles.
   /// </summary>
   public class BadgeControl : Control
   {
      // Declaración de variables internas
      private ComponentColorScheme _type;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="BadgeControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public BadgeControl(ViewContainer container) : 
         base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="BadgeControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="text">Texto que contiene el control.</param>
      /// <param name="type">Tipo (color) de mensaje a representar.</param>
      /// <param name="roundedBorders">Indica si se debe representar con los bordes redondeados.</param>
      public BadgeControl(ViewContainer container, string text, ComponentColorScheme type, bool roundedBorders) :
         base(container)
      {
         Initialize();

         Text = text;
         RoundedBorders = roundedBorders;
         _type = type;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el contenido del <em>badge</em>.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Indica si el <em>badge</em> debe tener los extremos redondeados (<c>true</c>) o cuadrados (<c>false</c>).
      /// </summary>
      public bool RoundedBorders { get; set; }

      /// <summary>
      /// Devuelve o establece el color del componente.
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
         RoundedBorders = true;
         Text = string.Empty;
         _type = ComponentColorScheme.Normal;
      }

      #endregion

   }
}
