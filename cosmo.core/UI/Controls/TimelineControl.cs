using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el control Timeline.
   /// </summary>
   public class TimelineControl : Control
   {

      #region Constructors
      
      /// <summary>
      /// Gets a new instance of <see cref="TimelineControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public TimelineControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="TimelineControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public TimelineControl(View parentView, string domId)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Contiene la lista de elementos de la línea de timepo.
      /// </summary>
      public List<TimelineItem> Items { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Limpia todos los elementos de la línea de tiempo.
      /// </summary>
      public void clear()
      {
         this.Items.Clear();
      }

      /// <summary>
      /// Añade un nuevo elemento a la línea de tiempo.
      /// </summary>
      /// <param name="item">La instancia de <see cref="TimelineItem"/> a añadir.</param>
      public void AddItem(TimelineItem item)
      {
         this.Items.Add(item);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Items = new List<TimelineItem>();
      }

      #endregion

   }
}
