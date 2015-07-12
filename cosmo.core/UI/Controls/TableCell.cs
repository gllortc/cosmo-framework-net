namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una celda del componente Table de Bootstrap.
   /// </summary>
   public class TableCell
   {
      // Internal data declarations
      private object val;
      private string href;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="TableCell"/>.
      /// </summary>
      /// <param name="value"></param>
      public TableCell(object value)
      {
         Initialize();

         this.val = value;
      }

      /// <summary>
      /// Gets a new instance of <see cref="TableCell"/>.
      /// </summary>
      /// <param name="value">The value of the cell.</param>
      /// <param name="href">A string that contains a url to provide a link for the current cell.</param>
      public TableCell(object value, string href)
      {
         Initialize();

         this.val = value;
         this.href = href;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el contenido de la celda.
      /// </summary>
      public object Value
      {
         get { return val; }
         set { val = value; }
      }

      /// <summary>
      /// Returns or sets the url to provide a link for the current cell.
      /// </summary>
      public string Href
      {
         get { return href; }
         set { href = value; }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa al instancia.
      /// </summary>
      private void Initialize()
      {
         this.val = null;
         this.href = string.Empty;
      }

      #endregion

   }
}
