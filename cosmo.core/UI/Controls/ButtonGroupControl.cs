using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una barra de herramientas.
   /// http://getbootstrap.com/components/#btn-groups
   /// </summary>
   public class ButtonGroupControl : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ButtonGroupControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public ButtonGroupControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si la barra está alineada verticalmente.
      /// </summary>
      public bool Vertical { get; set; }

      /// <summary>
      /// Devuelve o establece el tamaño que deben tener los botones de la barra.
      /// </summary>
      public ButtonControl.ButtonSizes Size { get; set; }

      /// <summary>
      /// Contiene la lista de botones de la barra de herramientas.
      /// </summary>
      public List<ButtonControl> Buttons { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Vertical = false;
         this.Size = ButtonControl.ButtonSizes.Default;
         this.Buttons = new List<ButtonControl>();
      }

      #endregion

   }
}
