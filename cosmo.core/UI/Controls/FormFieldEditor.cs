using Cosmo.Net;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo editor de texto (TextArea) que permite editar texto sencillo, HTML o BBCode.
   /// </summary>
   public class FormFieldEditor : FormField
   {
      // Declara variables internas
      private string _value;

      #region Enumerations

      /// <summary>
      /// Enumera los distintos tipos de editor que implementa el control.
      /// </summary>
      public enum FieldEditorType
      {
         /// <summary>Permite editar texto simple.</summary>
         Simple,
         /// <summary>Permite editar texto en formato HTML.</summary>
         HTML,
         /// <summary>Permite editar texto en formato BBCode (usualmente para foros).</summary>
         BBCode
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldEditor"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      public FormFieldEditor(View parentViewport, string domId)
         : base(parentViewport, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      public FormFieldEditor(View parentViewport, string domId, string label, FieldEditorType type)
         : base(parentViewport, domId)
      {
         Initialize();

         this.Label = label;
         this.Type = type;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      /// <param name="value"></param>
      public FormFieldEditor(View parentViewport, string domId, string label, FieldEditorType type, string value)
         : base(parentViewport, domId)
      {
         Initialize();

         this.Label = label;
         this.Type = type;
         this.Value = value;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el tipo de campo implementado.
      /// </summary>
      public override FieldTypes FieldType
      {
         get { return FieldTypes.Standard; }
      }

      /// <summary>
      /// Indica si el campo es obligatorio.
      /// </summary>
      public bool Required { get; set; }

      /// <summary>
      /// Devuelve o establece el texto que mostrará la etiqueta del campo.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Devuelve o establece un texto en el control que desaparece cuando contiene algún valor.
      /// </summary>
      public string Placeholder { get; set; }

      /// <summary>
      /// Devuelve o establece una descripción que aparecerá en pequeño junto al campo (dependiendo de la plantilla de renderización).
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Devuelve o establece el valor del campo.
      /// </summary>
      public override object Value
      {
         get { return _value; }
         set { _value = value.ToString(); }
      }

      /// <summary>
      /// Devuelve o establece la longitud (en carácteres) mínima que debe tener el valor.
      /// </summary>
      public int MinLength { get; set; }

      /// <summary>
      /// Devuelve o establece la longitud (en carácteres) máxima que debe tener el valor.
      /// </summary>
      public int MaxLength { get; set; }

      /// <summary>
      /// Devuelve o establece el tipo de editor de texto a mostrar.
      /// </summary>
      public FieldEditorType Type { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene el valor del campo a partir de los datos recibidos mediante GET o POST.
      /// </summary>
      public override bool LoadValueFromRequest()
      {
         try
         {
            _value = Url.GetString(ParentView.Workspace.Context.Request.Params, this.DomID);
         }
         catch
         {
            _value = string.Empty;
         }

         return Validate();
      }

      /// <summary>
      /// Valida el valor del campo.
      /// </summary>
      /// <returns><c>true</c> si el valor és aceptable o <c>false</c> si el valor no es válido.</returns>
      public override bool Validate()
      {
         bool valid = true;

         if (this.Required)
         {
            valid = valid & !string.IsNullOrWhiteSpace(_value);
         }

         if (this.MinLength >= 0)
         {
            valid = valid & (_value.Length >= this.MinLength);
         }

         if (this.MaxLength > 0)
         {
            valid = valid & (_value.Length <= this.MaxLength);
         }

         return true;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Required = false;
         this.MinLength = 0;
         this.MaxLength = 0;
         this.Label = string.Empty;
         this.Placeholder = string.Empty;
         this.Description = string.Empty;
         this.Value = string.Empty;
         this.Type = FieldEditorType.Simple;
      }

      #endregion

   }
}
