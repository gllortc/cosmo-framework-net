namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un elemento insertable en un componente ListGroup de Bootstrap.
   /// </summary>
   public class ListItem
   {
      // Internal data declarations.
      private string _caption;
      private string _description;
      private string _badge;
      private string _href;
      private string _icon;
      private bool _paddingDescription;
      private ListGroupControl.ListGroupStyle _style;
      private bool _isActive;
      private ComponentColorScheme _type;

      /// <summary>
      /// Gets a new instance of <see cref="ListItem"/>.
      /// </summary>
      public ListItem() 
      {
         Initialize();
      }

      /// <summary>
      /// Gets or sets el título visible del elemento.
      /// </summary>
      public string Text
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Gets or sets el texto descriptivo del elemento.
      /// No soporta XHTML, sólo texto.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Gets or sets la URL asociada al elemento.
      /// </summary>
      public string Href
      {
         get { return _href; }
         set { _href = value; }
      }

      /// <summary>
      /// Gets or sets el código del icono a mostrar junto al título del elemento.
      /// </summary>
      public string Icon
      {
         get { return _icon; }
         set { _icon = value; }
      }

      /// <summary>
      /// Gets or sets el texto que aparecerá como <em>badge</em> (usualmente para indicar, por ejemplo, el número de elementos en un categoria).
      /// </summary>
      public string BadgeText
      {
         get { return _badge; }
         set { _badge = value; }
      }

      /// <summary>
      /// Indica si el elemento debe mostrarse como activo (resaltado).
      /// </summary>
      public bool IsActive
      {
         get { return _isActive; }
         set { _isActive = value; }
      }

      /// <summary>
      /// Gets or sets el estilo de marcado del elemento.
      /// </summary>
      public ComponentColorScheme Type
      {
         get { return _type; }
         set { _type = value; }
      }

      internal ListGroupControl.ListGroupStyle Style
      {
         get { return _style; }
         set { _style = value; }
      }

      /// <summary>
      /// Indica si se debe alinear la descripción con el título cuando se inserta un icono.
      /// </summary>
      internal bool AlignDescription
      {
         get { return _paddingDescription; }
         set { _paddingDescription = value; }
      }

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _caption = string.Empty;
         _description = string.Empty;
         _badge = string.Empty;
         _href = string.Empty;
         _icon = string.Empty;
         _isActive = false;
         _paddingDescription = false;
         _type = ComponentColorScheme.Normal;
         _style = ListGroupControl.ListGroupStyle.Simple;
      }
   }
}
