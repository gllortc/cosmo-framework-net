namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un control de tiopo Pagination para recorrer una lista por páginas.
   /// </summary>
   public class PaginationControl : Control
   {
      /// <summary>
      /// TAG que debe incluir el patrón URL que será sustituydo por el número de página.
      /// </summary>
      public const string URL_PAGEID_TAG = "--PAGID--";

      // Internal data declarations
      private int _min;
      private int _max;
      private int _current;
      private string _url;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PaginationControl"/>
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public PaginationControl(View parentView) 
         : base(parentView) 
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el número mínimo de página.
      /// </summary>
      public int Min
      {
         get { return _min; }
         set { _min = value; }
      }

      /// <summary>
      /// Gets or sets el número máximo de página.
      /// </summary>
      public int Max
      {
         get { return _max; }
         set { _max = value; }
      }

      /// <summary>
      /// Gets or sets el número de página actual.
      /// </summary>
      public int Current
      {
         get { return _current; }
         set { _current = value; }
      }

      /// <summary>
      /// Gets or sets la URL base que servirá para generar la URL particular para cada página.
      /// Para ello se usa el tag <see cref="URL_PAGEID_TAG"/> como indicador de número de página.
      /// </summary>
      public string UrlPattern
      {
         get { return _url; }
         set { _url = value; }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _min = 1;
         _max = 25;
         _current = 1;
         _url = string.Empty;
      }

      #endregion

   }
}
