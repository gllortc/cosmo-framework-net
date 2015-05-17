using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente Table.
   /// Detalles: http://getbootstrap.com/css/#tables
   /// </summary>
   public class TableControl : Control
   {
      private bool _isStriped;
      private bool _isBordered;
      private bool _isHover;
      private bool _isCondensed;
      private bool _isResponsive;
      private TableRow _header;
      private List<TableRow> _rows;

      /// <summary>
      /// Devuelve una instancia de <see cref="TableControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public TableControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Indica si la tabla representa todos los bordes.
      /// </summary>
      public bool Bordered
      {
         get { return _isBordered; }
         set { _isBordered = value; }
      }

      /// <summary>
      /// Indica si la tabla resalta la fila dónde se encuentra el puntero del mouse.
      /// </summary>
      public bool Hover
      {
         get { return _isHover; }
         set { _isHover = value; }
      }

      /// <summary>
      /// Indica si la tabla se presenta en un formato más comprimido.
      /// </summary>
      public bool Condensed
      {
         get { return _isCondensed; }
         set { _isCondensed = value; }
      }

      /// <summary>
      /// Indica si la tabla es <em>responsive</em> (se adapta frente al cambio dinámico de tamaño).
      /// </summary>
      public bool Responsive
      {
         get { return _isResponsive; }
         set { _isResponsive = value; }
      }

      /// <summary>
      /// Indica si la tabla alterna colores de fila.
      /// </summary>
      public bool Striped
      {
         get { return _isStriped; }
         set { _isStriped = value; }
      }

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
      public List<TableRow> Rows
      {
         get { return _rows; }
         set { _rows = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _isStriped = false;
         _isBordered = false;
         _isHover = false;
         _isCondensed = false;
         _isResponsive = true;
         _rows = new List<TableRow>();
      }
   }
}
