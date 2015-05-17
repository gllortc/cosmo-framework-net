using System.Collections;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa la plantilla de un componente de página.
   /// </summary>
   public class DomTemplateContentComponent
   {
      string _key;
      Hashtable _fragments;
      Hashtable _attributes;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplateComponent"/>.
      /// </summary>
      public DomTemplateContentComponent()
      {
         _key = string.Empty;
         _fragments = new Hashtable();
         _attributes = new Hashtable();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplateComponent"/>.
      /// </summary>
      public DomTemplateContentComponent(string key)
      {
         _key = key;
         _fragments = new Hashtable();
         _attributes = new Hashtable();
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador único del componente.
      /// </summary>
      public string Key
      {
         get { return _key; }
         set { _key = value; }
      }

      /// <summary>
      /// Contiene los atributos del fragmento.
      /// </summary>
      public Hashtable Attributes
      {
         get { return _attributes; }
         set { _attributes = value; }
      }

      /// <summary>
      /// Contiene los fragmentos de código del componente.
      /// </summary>
      public Hashtable Fragments
      {
         get { return _fragments; }
         set { _fragments = value; }
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
      /// Devuelve el código XHTML sin renderizar correspondiente a un fragmento del componente.
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