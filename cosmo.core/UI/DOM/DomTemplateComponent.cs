using System.Collections;
using System.Collections.Generic;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un elemento de plantilla.
   /// </summary>
   public class DomTemplateComponent
   {
      bool _show;
      string _key;
      Hashtable _fragments;
      Hashtable _attributes;
      List<DomPageScript> _scripts;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplateComponent"/>.
      /// </summary>
      public DomTemplateComponent()
      {
         Clear();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplateComponent"/>.
      /// </summary>
      public DomTemplateComponent(string key)
      {
         Clear();

         _key = key;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del componente.
      /// </summary>
      public string Key
      {
         get { return _key; }
         set { _key = value; }
      }

      /// <summary>
      /// Indica si el componente se debe mostrar cuando se aplica la plantilla actual.
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
      /// Contiene los fragmentos de código que usa el componente.
      /// </summary>
      public Hashtable Fragments
      {
         get { return _fragments; }
         set { _fragments = value; }
      }

      /// <summary>
      /// Contiene los atributos del componente.
      /// </summary>
      public Hashtable Attributes
      {
         get { return _attributes; }
         set { _attributes = value; }
      }

      /// <summary>
      /// Contiene la lista de scripts (código y/o referencias externas) que usa el componente.
      /// </summary>
      public List<DomPageScript> Scripts
      {
         get { return _scripts; }
         set { _scripts = value; }
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
      /// Devuelve el código XHTML sin renderizar correspondiente a un fragmento de componente.
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

      #region Private Members

      private void Clear()
      {
         _show = true;
         _key = string.Empty;
         _fragments = new Hashtable();
         _attributes = new Hashtable();
         _scripts = new List<DomPageScript>();
      }

      #endregion

   }

}