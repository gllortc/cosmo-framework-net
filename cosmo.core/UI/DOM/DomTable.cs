using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa una tabla DOM.
   /// </summary>
   public class DomTable : DomContentComponentBase
   {
      private string _caption;
      private string _desc;
      private bool _titlerow;
      private List<DomTableCell> _headrow = null;
      private DataTable _table = null;

      #region Constants

      /// <summary>Título del grid.</summary>
      internal const string SECTION_TITLE = "grid-title";
      /// <summary>Estructura básica del grid.</summary>
      internal const string SECTION_GRID_BODY = "grid-body";
      /// <summary>Estructura básica de la(s) fila(s) de títulos.</summary>
      internal const string SECTION_ROWTITLE_BODY = "grid-rowtitle-body";
      /// <summary>Estructura básica de una celda de la(s) fila(s) de títulos.</summary>
      internal const string SECTION_ROWTITLE_CELL = "grid-rowtitle-cell";
      /// <summary>Estructura básica de la(s) fila(s) de valores.</summary>
      internal const string SECTION_ROW_BODY = "grid-row-body";
      /// <summary>Estructura básica de una celda de la(s) fila(s) de valores.</summary>
      internal const string SECTION_ROW_CELL = "grid-row-cell";

      /// <summary>Identificador del bloque XHTML generado.</summary>
      public const string TAG_HTML_ID = "oid";
      /// <summary>Tag: ID de plantilla.</summary>
      public const string TAG_TEMPLATE_ID = "tid";
      /// <summary>Tag: Título a mostrar del menú.</summary>
      public const string TAG_GRID_NAME = "title";
      /// <summary>Tag: Descripción del menú.</summary>
      public const string TAG_GRID_DESCRIPTION = "description";
      /// <summary>Tag: Indentificador del elemento HTML.</summary>
      public const string TAG_GRID_ID = "id";
      /// <summary>Tag: Conjunto de filas del título.</summary>
      public const string TAG_TITLE_ROWS = "titlerows";
      /// <summary>Tag: Número de elemento (en grupo)</summary>
      public const string TAG_TITLE_CELLS = "cells";
      /// <summary>Tag: Número de elementos (en grupo)</summary>
      public const string TAG_DATA_ROWS = "rows";
      /// <summary>Tag: Título del elemento</summary>
      public const string TAG_DATA_CELLS = "cells";
      /// <summary>Tag: URL del elemento de menú</summary>
      public const string TAG_CELL_VALUE = "value";
      /// <summary>Tag: Número de fila de datos.</summary>
      public const string TAG_GRID_NUMROW = "item";
      /// <summary>Tag: Número total de filas de datos.</summary>
      public const string TAG_GRID_NUMROWS = "items";

      #endregion

      /// <summary>
      /// Devuelve una instancia de MWDomTable.
      /// </summary>
      public DomTable()
      {
         Clear();
      }

      /// <summary>
      /// Devuelve una instancia de MWDomTable.
      /// </summary>
      public DomTable(DataTable table)
      {
         Clear();

         _table = table;
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "grid"; }
      }

      /// <summary>
      /// Devuelve o establece el título de la tabla.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Descripción del contenido de la tabla.
      /// </summary>
      public string Description
      {
         get { return _desc; }
         set { _desc = value; }
      }

      /// <summary>
      /// Fila de títulos.
      /// </summary>
      public List<DomTableCell> TitleRow
      {
         get { return _headrow; }
         set { _headrow = value; }
      }

      /// <summary>
      /// Datos de la tabla.
      /// </summary>
      public DataTable Table
      {
         get { return _table; }
         set { _table = value; }
      }

      /// <summary>
      /// Indica si se debe añadir una fila superior que muestre los títulos de columna (en la propiedad Name de la celda).
      /// </summary>
      public bool AddTitleRow
      {
         get { return _titlerow; }
         set { _titlerow = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Limpia la tabla y la deja a punto para generar una nueva tabla.
      /// </summary>
      public void Clear()
      {
         this.ID = "grid1";
         _caption = string.Empty;
         _desc = string.Empty;
         _titlerow = false;
         _table = new DataTable();
         _headrow = new List<DomTableCell>();
      }

      /// <summary>
      /// Rellena una tabla a partir de los datos que contiene un SqlDataReader.
      /// </summary>
      /// <param name="reader">La instancia SqlDataReader que contiene los datos.</param>
      /// <remarks>
      /// Si hubiera datos en las celdas de la instancia actual se eliminarán.
      /// Basado en: http://netrsc.blogspot.com/2005/09/net-c-howto-convert-datareader-to.html
      /// </remarks>
      public void FillTableFromDataReader(SqlDataReader reader)
      {
         try
         {
            DataSet dataSet = new DataSet();
            DataTable schemaTable = reader.GetSchemaTable();
            DataTable dataTable = new DataTable();

            for (int cntr = 0; cntr < schemaTable.Rows.Count; ++cntr)
            {
               DataRow dataRow = schemaTable.Rows[cntr];
               string columnName = dataRow["ColumnName"].ToString();
               DataColumn column = new DataColumn(columnName, dataRow.GetType());
               dataTable.Columns.Add(column);
            }

            dataSet.Tables.Add(dataTable);

            while (reader.Read())
            {
               DataRow dataRow = dataTable.NewRow();
               for (int cntr = 0; cntr < reader.FieldCount; ++cntr)
               {
                  dataRow[cntr] = reader.GetValue(cntr);
               }
            }

            // Guarda el DataTable
            _table = dataTable;
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el componente.</returns>
      public override string Render(DomTemplate template, DomPage.ContentContainer container)
      {
         int rownum = 1;
         StringBuilder sbrows = null;
         StringBuilder sbcells = null;
         StringBuilder sbhtml = null;

         try
         {
            // Obtiene la plantilla del componente
            DomTemplateComponent component = template.GetContentComponent(this.ELEMENT_ROOT, container);
            if (component == null) return string.Empty;

            // Representa el título del grid
            sbhtml = new StringBuilder(component.GetFragment(DomTable.SECTION_TITLE));
            sbhtml.Replace(GetTag(DomTable.TAG_GRID_NAME), _caption);
            sbhtml.Replace(GetTag(DomTable.TAG_GRID_DESCRIPTION), _desc);

            // Representa la estructura del grid
            sbhtml.Append(component.GetFragment(DomTable.SECTION_GRID_BODY));
            sbrows = new StringBuilder();

            // Representa la fila de títulos
            sbcells = new StringBuilder();
            foreach (DataColumn col in _table.Columns)
            {
               sbcells.Append(component.GetFragment(DomTable.SECTION_ROWTITLE_CELL));
               sbcells.Replace(GetTag(DomTable.TAG_CELL_VALUE), col.ColumnName);
            }
            sbrows = new StringBuilder(component.GetFragment(DomTable.SECTION_ROWTITLE_BODY));
            sbrows.Replace(GetTag(DomTable.TAG_TITLE_CELLS), sbcells.ToString());
            sbhtml.Replace(GetTag(DomTable.TAG_TITLE_ROWS), sbrows.ToString());

            sbrows = new StringBuilder();

            // Representa las celdas de valores
            foreach (DataRow row in _table.Rows)
            {
               sbcells = new StringBuilder();

               foreach (DataColumn col in _table.Columns)
               {
                  if (col.DataType == typeof(DomTableCell))
                  {
                     sbcells.Append(component.GetFragment(DomTable.SECTION_ROW_CELL));
                     sbcells.Replace(GetTag(DomTable.TAG_CELL_VALUE), (string)((DomTableCell)row.ItemArray[col.Ordinal]).Value);
                     sbcells.Replace(GetTag(DomTable.TAG_GRID_NUMROW), rownum.ToString());

                     rownum++;
                  }
               }

               sbrows.Append(component.GetFragment(DomTable.SECTION_ROW_BODY));
               sbrows.Replace(GetTag(DomTable.TAG_DATA_CELLS), sbcells.ToString());
            }
            sbhtml.Replace(GetTag(DomTable.TAG_DATA_ROWS), sbrows.ToString());

            // Reemplaza los TAGs comunes a todo el elemento
            sbhtml.Replace(GetTag(DomTable.TAG_TEMPLATE_ID), template.ID.ToString());
            sbhtml.Replace(GetTag(DomTable.TAG_GRID_ID), (string.IsNullOrEmpty(this.ID) ? "grid1" : this.ID));
            sbcells.Replace(GetTag(DomTable.TAG_GRID_NUMROWS), rownum.ToString());

            // Devuelve el código html
            return sbhtml.ToString();
         }
         catch
         {
            throw;
         }
      }

      #endregion

   }

}
