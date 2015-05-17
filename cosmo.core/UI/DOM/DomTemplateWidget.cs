using System.Collections;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un elemento de plantilla.
   /// </summary>
   public class DomTemplateWidget
   {
      bool _show;
      string _key;
      Hashtable _fragments;
      Hashtable _attributes;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplateWidget"/>.
      /// </summary>
      public DomTemplateWidget()
      {
         _show = true;
         _key = string.Empty;
         _fragments = new Hashtable();
         _attributes = new Hashtable();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplateWidget"/>.
      /// </summary>
      public DomTemplateWidget(string key)
      {
         _show = true;
         _key = key;
         _fragments = new Hashtable();
         _attributes = new Hashtable();
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del widget.
      /// </summary>
      public string Key
      {
         get { return _key; }
         set { _key = value; }
      }

      /// <summary>
      /// Indica si el widget se debe mostrar cuando se aplica la plantilla actual.
      /// </summary>
      /// <remarks>
      /// Si esta propiedad es <c>false</c> el widget no será renderizado.
      /// </remarks>
      public bool Show
      {
         get { return _show; }
         set { _show = value; }
      }

      /// <summary>
      /// Contiene los fragmentos de código que usa el widget.
      /// </summary>
      public Hashtable Fragments
      {
         get { return _fragments; }
         set { _fragments = value; }
      }

      /// <summary>
      /// Contiene los atributos del widget.
      /// </summary>
      public Hashtable Attributes
      {
         get { return _attributes; }
         set { _attributes = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve el valor de un atributo.
      /// </summary>
      /// <param name="key">Identificador del atributo.</param>
      /// <remarks>
      /// Si no existe, devuelve una cadena vacía.
      /// </remarks>
      public string GetAttribute(string key)
      {
         if (!_attributes.ContainsKey(key))
            return string.Empty;
         else
            return (string)_attributes[key];
      }

      /// <summary>
      /// Devuelve el código XHTML sin renderizar correspondiente a un fragmento de widget.
      /// </summary>
      /// <param name="key">Identificador del fragmento.</param>
      /// <remarks>
      /// Si no existe, devuelve una cadena vacía.
      /// </remarks>
      public string GetFragment(string key)
      {
         if (!_fragments.ContainsKey(key))
            return string.Empty;
         else
            return (string)_fragments[key];
      }

      #endregion

   }

}