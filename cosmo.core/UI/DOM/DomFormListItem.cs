namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un elemento de lista de un control de formulario DOM.
   /// </summary>
   public class DomFormListItem
   {
      string _caption;
      string _value;
      bool _selected;

      /// <summary>
      /// Devuelve una instancia de MWDomFormListItem.
      /// </summary>
      public DomFormListItem()
      {
         _caption = "";
         _value = "";
         _selected = false;
      }

      /// <summary>
      /// Devuelve una instancia de MWDomFormListItem.
      /// </summary>
      public DomFormListItem(string Caption, string Value, bool Selected)
      {
         _caption = Caption;
         _value = Value;
         _selected = Selected;
      }

      /// <summary>
      /// Devuelve una instancia de MWDomFormListItem.
      /// </summary>
      public DomFormListItem(string Caption, string Value)
      {
         _caption = Caption;
         _value = Value;
         _selected = false;
      }

      #region Properties

      /// <summary>
      /// Título visible del elemento.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Valor asociado al elemento.
      /// </summary>
      public string Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Indica si el elemento debe aparecer seleccionado por defecto en la lista o lista desplegable.
      /// </summary>
      public bool Selected
      {
         get { return _selected; }
         set { _selected = true; }
      }

      #endregion

   }

}
