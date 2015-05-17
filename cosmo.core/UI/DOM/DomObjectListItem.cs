namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa un elemento de listado de objetos.
   /// </summary>
   public class DomObjectListItem
   {
      private string _caption = string.Empty;
      private string _desc = string.Empty;
      private string _author = string.Empty;
      private string _href = string.Empty;
      private string _thumb = string.Empty;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomObjectListItem"/>.
      /// </summary>
      public DomObjectListItem() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomObjectListItem"/>.
      /// </summary>
      public DomObjectListItem(string caption, string href) 
      {
         _caption = caption;
         _href = href;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomObjectListItem"/>.
      /// </summary>
      public DomObjectListItem(string caption, string description, string href)
      {
         _caption = caption;
         _desc = description;
         _href = href;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomObjectListItem"/>.
      /// </summary>
      public DomObjectListItem(string caption, string description, string thumbUrl, string href)
      {
         _caption = caption;
         _desc = description;
         _href = href;
         _thumb = thumbUrl;
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
      /// Devuelve o establece la entrada que informa del autor, fecha d epublicación, etc.
      /// </summary>
      public string Author
      {
         get { return _author; }
         set { _author = value; }
      }

      /// <summary>
      /// Devuelve o establece la URL de destino.
      /// </summary>
      public string Href
      {
         get { return _href; }
         set { _href = value; }
      }

      /// <summary>
      /// Devuelve o establece la URL (absoluta o relativa) de la imagen miniatura.
      /// </summary>
      public string Thumbnail
      {
         get { return _thumb; }
         set { _thumb = value; }
      }

      #endregion

   }
}
