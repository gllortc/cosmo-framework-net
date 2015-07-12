namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente PopOver de Bootstrap.
   /// </summary>
   public class PopoverControl : Control
   {

      #region Enumerations

      /// <summary>
      /// Enumera las direcciones posibles del elemento.
      /// </summary>
      public enum PopoverDirections
      {
         /// <summary>Orientación derecha</summary>
         Right,
         /// <summary>Orientación izquierda</summary>
         Left
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PopoverControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public PopoverControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el título visible del elemento.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or sets the text in popover control.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets la dirección en la que debe aparecer el elemento.
      /// </summary>
      public PopoverDirections Direction { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Text = string.Empty;
         this.Description = string.Empty;
         this.Direction = PopoverDirections.Left;
      }

      /// <summary>
      /// Convierte el tipo de dirección en una cadena de texto que corresponde a la clase CSS.
      /// </summary>
      private string ConvertPopoverDirectionToString(PopoverDirections direction)
      {
         switch (direction)
         {
            case PopoverDirections.Left:
               return "left";

            case PopoverDirections.Right:
               return "right";

            default:
               return string.Empty;
         }
      }

      #endregion

   }
}
