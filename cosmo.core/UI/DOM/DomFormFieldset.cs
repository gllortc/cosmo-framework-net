using System.Collections.Generic;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un grupo de elementos de menú.
   /// </summary>
   public class DomFormFieldset
   {
      private string _title;
      private string _desc;
      private List<DomFormField> _fields;

      /// <summary>
      /// Devuelve una instancia de MWDomMenuGroup.
      /// </summary>
      public DomFormFieldset()
      {
         _title = string.Empty;
         _desc = string.Empty;
         _fields = new List<DomFormField>();
      }

      /// <summary>
      /// Devuelve una instancia de MWDomMenuGroup.
      /// </summary>
      public DomFormFieldset(string Caption, string Description)
      {
         _title = Caption;
         _desc = Description;
         _fields = new List<DomFormField>();
      }

      #region Properties

      /// <summary>
      /// Nombre (texto descriptivo) del grupo.
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Descripción de los datos del grupo.
      /// </summary>
      public string Description
      {
         get { return _desc; }
         set { _desc = value; }
      }

      /// <summary>
      /// Elementos de menú que contiene el grupo.
      /// </summary>
      public List<DomFormField> Fields
      {
         get { return _fields; }
         set { _fields = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Añade un campo al grupo de campos.
      /// </summary>
      /// <param name="field">Instáncia de <see cref="DomFormField"/> que contiene los datos del campo.</param>
      public void AddField(DomFormField field)
      {
         _fields.Add(field);
      }

      /// <summary>
      /// Limpia la lista de campos del grupo.
      /// </summary>
      public void ClearFields()
      {
         _fields = new List<DomFormField>();
      }

      #endregion

   }

}
