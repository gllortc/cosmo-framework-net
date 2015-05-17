using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el control Timeline.
   /// </summary>
   public class TimelineControl : Control
   {
      // Declaración de variables internas
      private List<TimelineItem> _items;

      /// <summary>
      /// Devuelve una instancia de <see cref="TimelineControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public TimelineControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Contiene la lista de elementos de la línea de timepo.
      /// </summary>
      public List<TimelineItem> Items
      {
         get { return _items; }
         set { _items = value; }
      }

      /// <summary>
      /// Limpia todos los elementos de la línea de tiempo.
      /// </summary>
      public void clear()
      {
         _items.Clear();
      }

      /// <summary>
      /// Añade un nuevo elemento a la línea de tiempo.
      /// </summary>
      /// <param name="item">La instancia de <see cref="TimelineItem"/> a añadir.</param>
      public void AddItem(TimelineItem item)
      {
         _items.Add(item);
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _items = new List<TimelineItem>();
      }
   }
}
