namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una fila de un componente Table de Bootstrap.
   /// </summary>
   public class TableRow
   {
      // Declaración de variables internas
      private bool _isHeader;
      private int _cols;
      private string _domId;
      private TableCell[] _row;

      /// <summary>
      /// Devuelve una instancia de <see cref="TableRow"/>.
      /// </summary>
      public TableRow(int columns)
      {
         Initialize();

         _row = new TableCell[columns];
         _cols = columns;

         for (int idx = 0; idx < columns; idx++)
         {
            _row[idx] = new TableCell(string.Empty);
            idx++;
         }
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="TableRow"/>.
      /// </summary>
      /// <param name="id">Identificador único de la fila en la página.</param>
      /// <param name="cellValues">Un array que contiene los valores de la fila (una objeto por celda).</param>
      public TableRow(string id, params object[] cellValues)
      {
         Initialize();

         this.DomID = id;
         _row = new TableCell[cellValues.Length];
         _cols = cellValues.Length;

         int idx = 0;
         foreach (object cellValue in cellValues)
         {
            _row[idx] = new TableCell(cellValue);
            idx++;
         }
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="TableRow"/>.
      /// </summary>
      public TableRow(params object[] cellValues)
      {
         Initialize();

         _row = new TableCell[cellValues.Length];
         _cols = cellValues.Length;

         int idx = 0;
         foreach (object cellValue in cellValues)
         {
            _row[idx] = new TableCell(cellValue);
            idx++;
         }
      }

      /// <summary>
      /// Indica si la columna es una cabecera o no.
      /// </summary>
      internal bool IsHeader
      {
         get { return _isHeader; }
         set { _isHeader = value; }
      }

      /// <summary>
      /// Devuelve o establece el identificador del elemento en la estructura DOM de la página.
      /// </summary>
      public string DomID
      {
         get { return _domId; }
         set { _domId = value; }
      }

      /// <summary>
      /// Devuelve la lista de celdas de la fila.
      /// </summary>
      public TableCell[] Cells
      {
         get { return _row; }
         set { _row = value; }
      }

      /// <summary>
      /// Devuelve el parámetro ID para incrustar en un TAG XHTML.
      /// </summary>
      internal string GetIdParameter()
      {
         if (string.IsNullOrWhiteSpace(_domId))
         {
            return string.Empty;
         }
         else
         {
            return " id=\"" + _domId + "\" ";
         }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _isHeader = false;
         _cols = 0;
         _row = null;
         _domId = string.Empty;
      }
   }
}
