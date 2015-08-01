using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente Table.
   /// Detalles: http://getbootstrap.com/css/#tables
   /// </summary>
   public class TableControl : Control
   {
      // Internal data declarations
      private TableRow _header;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="TableControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public TableControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="TableControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public TableControl(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si la tabla representa todos los bordes.
      /// </summary>
      public bool Bordered { get; set; }

      /// <summary>
      /// Indica si la tabla resalta la fila dónde se encuentra el puntero del mouse.
      /// </summary>
      public bool Hover { get; set; }

      /// <summary>
      /// Indica si la tabla se presenta en un formato más comprimido.
      /// </summary>
      public bool Condensed { get; set; }

      /// <summary>
      /// Indica si la tabla es <em>responsive</em> (se adapta frente al cambio dinámico de tamaño).
      /// </summary>
      public bool Responsive { get; set; }

      /// <summary>
      /// Indica si la tabla alterna colores de fila.
      /// </summary>
      public bool Striped { get; set; }

      /// <summary>
      /// Contiene la fila de títulos de la tabla.
      /// </summary>
      public TableRow Header
      {
         get { return _header; }
         set 
         {
            _header = value;
            _header.IsHeader = true;
         }
      }

      /// <summary>
      /// Contiene la lista de filas de la tabla.
      /// </summary>
      public List<TableRow> Rows { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Striped = false;
         this.Bordered = false;
         this.Hover = false;
         this.Condensed = false;
         this.Responsive = true;
         this.Rows = new List<TableRow>();
      }

      #endregion

   }
}
