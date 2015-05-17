using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Cosmo.Data.Validation;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un campo de formulario del modelo de documentos DOM.
   /// </summary>
   public class DomFormField
   {

      #region Enumerations

      /// <summary>
      /// Tipos de campo del formulario
      /// </summary>
      public enum FormFieldTypes : int
      {
         /// <summary>Campo hidden</summary>
         Hidden = 0,
         /// <summary>Texto normal</summary>
         Text = 1,
         /// <summary>Texto en Textarea</summary>
         LargeText = 2,
         /// <summary>Checkbox</summary>
         Boolean = 4,
         /// <summary>Texto enmascarado (password)</summary>
         Password = 8,
         /// <summary>ComboBox (lista desplegable)</summary>
         Combo = 16,
         /// <summary>ListBox (lista)</summary>
         List = 32,
         /// <summary>Una lista de opciones (RadioButton)</summary>
         Option = 64,
         /// <summary>Casilla de selección de archivo</summary>
         File = 128,
         /// <summary>Casilla de verificación de captcha.</summary>
         Captcha = 256
      }

      #endregion

      string _name;
      string _label;
      string _description;
      DomFormField.FormFieldTypes _type;
      string _value;
      bool _readonly;
      bool _checked;
      int _cols;
      int _rows;
      List<DomFormListItem> _list;
      object _validationRule;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFormField"/>.
      /// </summary>
      public DomFormField()
      {
         _cols = 0;
         _rows = 0;
         _name = string.Empty;
         _label = string.Empty;
         _description = string.Empty;
         _type = FormFieldTypes.Text;
         _value = string.Empty;
         _readonly = false;
         _checked = false;
         _list = new List<DomFormListItem>();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFormField"/>.
      /// </summary>
      public DomFormField(string hiddenname, string hiddenvalue)
      {
         _cols = 0;
         _rows = 0;
         _name = hiddenname;
         _label = string.Empty;
         _description = string.Empty;
         _type = FormFieldTypes.Hidden;
         _value = hiddenvalue;
         _readonly = false;
         _checked = false;
         _list = new List<DomFormListItem>();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFormField"/>.
      /// </summary>
      public DomFormField(FormFieldTypes type, string name)
      {
         Initialize();

         // Inicializaciones
         _type = type;
         _name = name;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFormField"/>.
      /// </summary>
      public DomFormField(FormFieldTypes type, string name, string label)
      {
         Initialize();

         // Inicializaciones
         _type = type;
         _name = name;
         _label = label;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFormField"/>.
      /// </summary>
      public DomFormField(FormFieldTypes type, string name, string label, string value)
      {
         Initialize();

         // Inicializaciones
         _type = type;
         _name = name;
         _label = label;
         _value = value;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFormField"/>.
      /// </summary>
      public DomFormField(FormFieldTypes type, string name, string label, string value, string description)
      {
         Initialize();

         // Inicializaciones
         _type = type;
         _name = name;
         _label = label;
         _value = value;
         _description = description;
         _type = type;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece la regla de validación que se usará para validar el valor del campo.
      /// </summary>
      public object ValidationRule
      {
         get { return _validationRule; }
         set 
         {
            if (!(value is Cosmo.Data.Validation.ValidationRuleBase))
               throw new ArgumentException("La regla de validación debe ser una instancia de Cosmo.Data.Validation.ValidationRuleBase.");

            _validationRule = value;
         }
      }

      /// <summary>
      /// Nombre identificativo del campo.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Texto (etiqueta) que aparece asociado al campo.
      /// </summary>
      public string Label
      {
         get { return _label; }
         set { _label = value; }
      }

      /// <summary>
      /// Texto descriptivo del campo.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Tipo de campo.
      /// </summary>
      public FormFieldTypes Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Número de columnas para campos de texto largo
      /// </summary>
      public int LargeTextColumns
      {
         get { return _cols; }
         set { _cols = value; }
      }

      /// <summary>
      /// Número de filas para campos de texto largo.
      /// </summary>
      public int LargeTextRows
      {
         get { return _rows; }
         set { _rows = value; }
      }

      /// <summary>
      /// Indica si el campo es de sólo lectura.
      /// </summary>
      public bool ReadOnly
      {
         get { return _readonly; }
         set { _readonly = value; }
      }

      /// <summary>
      /// En caso de ser un campo booleano (checkbox), indica si está marcado.
      /// </summary>
      public bool Checked
      {
         get { return _checked; }
         set { _checked = value; }
      }

      /// <summary>
      /// Valor del campo.
      /// </summary>
      public string Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Contiene la lista de elementos para los campos de tipo lista (ListBox o ComboBox).
      /// </summary>
      public List<DomFormListItem> ListValues
      {
         get { return _list; }
         set { _list = value; }
      }

      /// <summary>
      /// Permite agregar un elemento a la lista para campos de tipo lista.
      /// </summary>
      /// <param name="listitem">Una instancia de MWDomFormListItem que describe el elemento.</param>
      [Obsolete()]
      public void AddListItem(DomFormListItem listitem)
      {
         _list.Add(listitem);
      }

      /// <summary>
      /// Limpia la lista de elementos del campo.
      /// </summary>
      [Obsolete()]
      public void ClearListItems()
      {
         _list = new List<DomFormListItem>();
      }

      #endregion

      #region Methods

      /// <summary>
      /// Rellena una lista de valores a partir de una instancia de <see cref="SqlDataReader"/>.
      /// </summary>
      /// <param name="reader">Una instancia de <see cref="SqlDataReader"/> que contiene la lista de opciones.</param>
      /// <param name="captionfield">Nombre del campo que contiene el valor que se usará como texto del valor.</param>
      /// <param name="valuefield">Nombre del campo que contiene el valor.</param>
      /// <param name="selectedvalue">Valor del elemento a seleccionar por defecto.</param>
      public void AddListFromDataReader(SqlDataReader reader, string captionfield, string valuefield, string selectedvalue)
      {
         bool bSel = false;

         // Inicializaciones
         selectedvalue = selectedvalue.Trim().ToUpper();

         // Evita objetos no instanciados o de diferente tipo al requerido
         if (reader == null) throw new Exception("Objeto SqlDataReader no válido.");
         if (!reader.HasRows) throw new Exception("El objeto SqlDataReader no dispone de datos.");

         // Rellena la lista
         while (reader.Read())
         {
            bSel = false;
            if (!selectedvalue.Equals("") && selectedvalue.Equals(reader[valuefield].ToString().Trim().ToUpper())) bSel = true;
            _list.Add(new DomFormListItem(reader[captionfield].ToString(), reader[valuefield].ToString(), bSel));
         }
      }

      /// <summary>
      /// Rellena una lista de valores a partir de un SqlDataReader
      /// </summary>
      /// <param name="reader">Objeto SqlDataReader con datos.</param>
      /// <param name="captionfield">Nombre del campo que contiene el valor que se usará como texto del valor.</param>
      /// <param name="valuefield">Nombre del campo que contiene el valor.</param>
      public void AddListFromDataReader(SqlDataReader reader, string captionfield, string valuefield)
      {
         AddListFromDataReader(reader, captionfield, valuefield, "");
      }

      /// <summary>
      /// Agrega un elemento a la lista de opciones del control.
      /// </summary>
      /// <param name="text">Texto visible de la opción.</param>
      /// <param name="value">Valor de la opción.</param>
      public void AddListItem(string text, string value)
      {
         _list.Add(new DomFormListItem(text, value));
      }

      /// <summary>
      /// Agrega un elemento a la lista de opciones del control.
      /// </summary>
      /// <param name="text">Texto visible de la opción.</param>
      /// <param name="value">Valor de la opción.</param>
      /// <param name="selected">Indica si el elemento estará seleccionado por defecto.</param>
      public void AddListItem(string text, string value, bool selected)
      {
         _list.Add(new DomFormListItem(text, value, selected));
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo oculto.
      /// </summary>
      public static DomFormField HiddenField(string name, string value)
      {
         DomFormField field = new DomFormField();
         field.Type = FormFieldTypes.Hidden;
         field.Name = name;
         field.Value = value;

         return field;
      }

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo de texto.
      /// </summary>
      public static DomFormField TextField(string name, string value)
      {
         return TextField(name, value, null);
      }

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo de texto.
      /// </summary>
      public static DomFormField TextField(string name, string label, ValidationRuleBase validationRule)
      {
         return TextField(name, label, string.Empty, validationRule);
      }

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo de texto.
      /// </summary>
      public static DomFormField TextField(string name, string label, string description, ValidationRuleBase validationRule)
      {
         DomFormField field = new DomFormField();
         field.Type = FormFieldTypes.Text;
         field.Name = name;
         field.Label = label;
         field.Description = description;
         field.ValidationRule = validationRule;

         return field;
      }

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo de texto largo.
      /// </summary>
      public static DomFormField LargeTextField(string name, string value)
      {
         return LargeTextField(name, value, null);
      }

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo de texto largo.
      /// </summary>
      public static DomFormField LargeTextField(string name, string label, ValidationRuleBase validationRule)
      {
         return LargeTextField(name, label, string.Empty, validationRule);
      }

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo de texto largo.
      /// </summary>
      public static DomFormField LargeTextField(string name, string label, string description, ValidationRuleBase validationRule)
      {
         DomFormField field = new DomFormField();
         field.Type = FormFieldTypes.LargeText;
         field.Name = name;
         field.Label = label;
         field.Description = description;
         field.ValidationRule = validationRule;

         return field;
      }

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo de verificación de Captcha.
      /// </summary>
      public static DomFormField CaptchaField(string name)
      {
         DomFormField field = new DomFormField();
         field.Type = FormFieldTypes.Captcha;
         field.Name = name;

         return field;
      }

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo booleano (checkbox).
      /// </summary>
      public static DomFormField BooleanField(string name, string label)
      {
         return BooleanField(name, label, string.Empty);
      }

      /// <summary>
      /// Genera una instancia de <see cref="DomFormField"/> que corresponde a un campo booleano (checkbox).
      /// </summary>
      public static DomFormField BooleanField(string name, string label, string description)
      {
         DomFormField field = new DomFormField();
         field.Type = FormFieldTypes.Boolean;
         field.Name = name;
         field.Label = label;
         field.Description = description;

         return field;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa el componente.
      /// </summary>
      private void Initialize()
      {
         _name = string.Empty;
         _label = string.Empty;
         _description = string.Empty;
         _type = FormFieldTypes.Text;
         _value = string.Empty;
         _readonly = false;
         _checked = false;
         _cols = 0;
         _rows = 0;
         _list = new List<DomFormListItem>();
         _validationRule = null;
      }

      #endregion

   }

}
