namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una fila de un componente Table de Bootstrap.
   /// </summary>
   public class TableRow
   {
      // Internal data declarations
      private bool _isHeader;
      private int _cols;
      private string _domId;
      private TableCell[] _row;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="TableRow"/>.
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
      /// Gets a new instance of <see cref="TableRow"/>.
      /// </summary>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="cellValues">Un array que contiene los valores de la fila (una objeto por celda).</param>
      public TableRow(string domId, params object[] cellValues)
      {
         Initialize();

         this.DomID = domId;
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
      /// Gets a new instance of <see cref="TableRow"/>.
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

      #endregion

      #region Properties

      /// <summary>
      /// Indica si la columna es una cabecera o no.
      /// </summary>
      internal bool IsHeader
      {
         get { return _isHeader; }
         set { _isHeader = value; }
      }

      /// <summary>
      /// Gets or sets el identificador del elemento en la estructura DOM de la página.
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

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _isHeader = false;
         _cols = 0;
         _row = null;
         _domId = string.Empty;
      }

      #endregion

   }
}
