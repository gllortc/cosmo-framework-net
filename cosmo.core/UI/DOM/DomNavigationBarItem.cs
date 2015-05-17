namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa un elemento de barra de navegación <see cref="DomNavigationBar"/>.
   /// </summary>
   public class DomNavigationBarItem
   {
      private string _name = string.Empty;
      private string _href = string.Empty;
      private string _cssClass = string.Empty;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomNavigationBarItem"/>.
      /// </summary>
      public DomNavigationBarItem() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomNavigationBarItem"/>.
      /// </summary>
      public DomNavigationBarItem(string name)
      {
         _name = name;
         _href = string.Empty;
         _cssClass = string.Empty;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomNavigationBarItem"/>.
      /// </summary>
      public DomNavigationBarItem(string name, string href) 
      {
         _name = name;
         _href = href;
         _cssClass = string.Empty;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomNavigationBarItem"/>.
      /// </summary>
      public DomNavigationBarItem(string name, string href, string CssClass)
      {
         _name = name;
         _href = href;
         _cssClass = CssClass;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el texto visible del elemento.
      /// </summary>
      public string Caption
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Devuelve o establece el enlace (URL) del elemento.
      /// </summary>
      public string Href
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

      #region Methods

      /// <summary>
      /// Convierte el elemento en código XHTML.
      /// </summary>
      /// <returns></returns>
      public string ToXhtml()
      {
         if (string.IsNullOrEmpty(this.Href))
         {
            if (string.IsNullOrEmpty(this.CssClass))
            {
               return this.Caption;
            }
            else
            {
               return string.Format("<span class=\"{0}\">{1}</span>", this.CssClass, this.Caption);
            }
         }

         return string.Format("<a href=\"{0}\"{1}>{2}</a>", 
                              this.Href, 
                              (string.IsNullOrEmpty(this.CssClass) ? "" : string.Format(" class=\"{0}\"", this.CssClass)), 
                              this.Caption);
      }

      #endregion

   }
}
