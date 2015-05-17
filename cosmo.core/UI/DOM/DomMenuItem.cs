namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un elemento de menú.
   /// </summary>
   /// <remarks>
   /// Un elemento de menú siempre pertenecerá a un grupo.
   /// </remarks>
   public class DomMenuItem
   {
      private string _caption;
      private string _href;
      private string _cssClass;

      /// <summary>
      /// Devuelve una instancia de MWDomMenuItem.
      /// </summary>
      public DomMenuItem()
      {
         _caption = string.Empty;
         _href = string.Empty;
         _cssClass = string.Empty;
      }

      /// <summary>
      /// Devuelve una instancia de MWDomMenuItem.
      /// </summary>
      public DomMenuItem(string Caption, string HREF)
      {
         _caption = Caption;
         _href = HREF;
         _cssClass = string.Empty;
      }

      /// <summary>
      /// Devuelve una instancia de MWDomMenuItem.
      /// </summary>
      public DomMenuItem(string Caption, string HREF, string CssClass)
      {
         _caption = Caption;
         _href = HREF;
         _cssClass = CssClass;
      }

      #region Properties

      /// <summary>
      /// Texto que muestra el elemento de menú.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// URL de destino del elemento de menú.
      /// </summary>
      public string HREF
      {
         get { return _href; }
         set { _href = value; }
      }

      /// <summary>
      /// Devuelve o establece la clase CSS a aplicar al elemento.
      /// </summary>
      public string CssClass
      {
         get { return _cssClass; }
         set { _cssClass = value; }
      }

      #endregion

   }

}
