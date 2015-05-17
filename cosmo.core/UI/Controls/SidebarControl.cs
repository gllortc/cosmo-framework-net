using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el control Sidebar (barra lateral de menú).
   /// </summary>
   public class SidebarControl : Control
   {
      // Declaración de variables internas
      private List<SidebarButton> _buttons;

      /// <summary>
      /// Devuelve una instancia de <see cref="SidebarControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public SidebarControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve o establece el texto que se mostrará en la etiqueta (<c>badge</c>).
      /// </summary>
      public List<SidebarButton> Buttons
      {
         get { return _buttons; }
         set { _buttons = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _buttons = new List<SidebarButton>();
      }
   }
}
