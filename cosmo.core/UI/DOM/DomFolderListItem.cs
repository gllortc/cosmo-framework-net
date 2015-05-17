namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa un elemento de listado de carpetas.
   /// </summary>
   public class DomFolderListItem
   {
      private string _caption = string.Empty;
      private string _desc = string.Empty;
      private string _href = string.Empty;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderListItem"/>.
      /// </summary>
      public DomFolderListItem() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderListItem"/>.
      /// </summary>
      public DomFolderListItem(string caption, string href) 
      {
         _caption = caption;
         _href = href;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFolderListItem"/>.
      /// </summary>
      public DomFolderListItem(string caption, string description, string href)
      {
         _caption = caption;
         _desc = description;
         _href = href;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el texto visible del elemento.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Devuelve o establece la descripción visible del elemento.
      /// </summary>
      public string Description
      {
         get { return _desc; }
         set { _desc = value; }
      }

      /// <summary>
      /// Devuelve o establece la URL de destino.
      /// </summary>
      public string Href
      {
         get { return _href; }
         set { _href = value; }
      }

      #endregion

   }
}
