using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente ListGroup de Bootstrap.
   /// </summary>
   public class ListGroupControl : Control
   {
      // Declaración de variables internas
      private bool _paddingDescription;
      private List<ListItem> _items;
      private ListGroupStyle _style;

      #region Enumerations

      /// <summary>
      /// Enumera los estilos de presentación del componente ListGroup.
      /// </summary>
      public enum ListGroupStyle
      {
         /// <summary>Lista simple dónde cada elemento sólo muestra su título.</summary>
         Simple,
         /// <summary>Lista compleja dónde cada elemento es acompañado de cósido XHTML.</summary>
         CustomContent
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ListGroupControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public ListGroupControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece la lista de elementos que mostrará el componente.
      /// </summary>
      public List<ListItem> ListItems
      {
         get { return _items; }
         set { _items = value; }
      }

      /// <summary>
      /// Indica si se debe alinear la descripción con el título cuando se inserta un icono.
      /// </summary>
      public bool AlignDescription
      {
         get { return _paddingDescription; }
         set { _paddingDescription = value; }
      }

      /// <summary>
      /// Devuelve o establece el estilo de visualización del componente.
      /// </summary>
      public ListGroupStyle Style
      {
         get { return _style; }
         set { _style = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Elimina todos los elementos de la lista.
      /// </summary>
      public void Clear()
      {
         _items.Clear();
      }

      /// <summary>
      /// Agrega un nuevo elemento a la lista.
      /// </summary>
      /// <param name="item">Una instancia de <see cref="ListItem"/> que representa el nuevo elemento.</param>
      public void Add(ListItem item)
      {
         _items.Add(item);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _items = new List<ListItem>();
         _style = ListGroupStyle.Simple;
         _paddingDescription = false;
      }

      /// <summary>
      /// Indica si todos los elementos disponen de enlace.
      /// </summary>
      private bool IsLinkList()
      {
         bool isLink = true;

         foreach (ListItem item in _items)
         {
            isLink = isLink && !string.IsNullOrEmpty(item.Href);
         }

         return isLink;
      }

      #endregion

   }
}
