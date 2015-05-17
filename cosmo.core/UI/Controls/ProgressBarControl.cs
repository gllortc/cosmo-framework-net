namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente PopOver de Bootstrap.
   /// </summary>
   public class ProgressBarControl : Control
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

      // Declaración de variables sinternas
      private int _percentage;
      private ComponentColorScheme _color;
      private string _description;

      /// <summary>
      /// Devuelve una instancia de <see cref="PopoverControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public ProgressBarControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="PopoverControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="percentage"></param>
      /// <param name="color"></param>
      public ProgressBarControl(ViewContainer container, int percentage, ComponentColorScheme color)
         : base(container)
      {
         Initialize();

         _percentage = percentage;
         _color = color;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="PopoverControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="percentage"></param>
      /// <param name="color"></param>
      /// <param name="description"></param>
      public ProgressBarControl(ViewContainer container, int percentage, ComponentColorScheme color, string description)
         : base(container)
      {
         Initialize();

         _percentage = percentage;
         _color = color;
         _description = description;
      }

      /// <summary>
      /// Devuelve o establece el título visible del elemento.
      /// </summary>
      public int Percentage
      {
         get { return _percentage; }
         set { _percentage = value; }
      }

      /// <summary>
      /// Devuelve o establece el color del elemento.
      /// </summary>
      public ComponentColorScheme Color
      {
         get { return _color; }
         set { _color = value; }
      }

      /// <summary>
      /// Devuelve o establece el texto descriptivo que aparece junto a la barra del progreso.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _color = ComponentColorScheme.Normal;
         _percentage = 50;
         _description = string.Empty;
      }
   }
}
