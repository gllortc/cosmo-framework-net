namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente PopOver de Bootstrap.
   /// </summary>
   public class PopoverControl : Control
   {
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

      // Internal data declarations
      private string _caption;
      private string _content;
      private PopoverDirections _direction;

      /// <summary>
      /// Devuelve una instancia de <see cref="PopoverControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public PopoverControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve o establece el título visible del elemento.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Devuelve o establece el texto visible del elemento.
      /// </summary>
      public string Text
      {
         get { return _content; }
         set { _content = value; }
      }

      /// <summary>
      /// Devuelve o establece la dirección en la que debe aparecer el elemento.
      /// </summary>
      public PopoverDirections Direction
      {
         get { return _direction; }
         set { _direction = value; }
      }
      
      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _caption = string.Empty;
         _content = string.Empty;
         _direction = PopoverDirections.Left;
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
   }
}
