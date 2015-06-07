using Cosmo.Net;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo de texto simple (Input).
   /// </summary>
   public class FormFieldText : FormField // IControl
   {
      // Declara variables internas
      private string _value;

      #region Enumerations

      /// <summary>
      /// Enumera los distintos tipos de contenido que permite albergar el control
      /// </summary>
      public enum FieldDataType
      {
         /// <summary>Color RGB</summary>
         Color,
         /// <summary>Correo electrónico</summary>
         Email,
         /// <summary>Número</summary>
         Number,
         /// <summary>Rango de valores</summary>
         Range,
         /// <summary>Texto</summary>
         Text,
         /// <summary>Dirección URL</summary>
         Url,
         /// <summary>Teléfono</summary>
         Phone
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador único del componente dentro de la vista.</param>
      public FormFieldText(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador único del componente dentro de la vista.</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      public FormFieldText(View parentView, string domId, string label, FieldDataType type)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
         this.Type = type;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador único del componente dentro de la vista.</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      /// <param name="value"></param>
      public FormFieldText(View parentView, string domId, string label, FieldDataType type, string value)
         : base(parentView, domId)
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
      /// Indictes if the field is read only or can be edited.
      /// </summary>
      public bool ReadOnly { get; set; }

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
      /// Devuelve o establece la longitud (en carácteres) mínima que debe tener el valor.
      /// </summary>
      public int MinLength { get; set; }

      /// <summary>
      /// Devuelve o establece la longitud (en carácteres) máxima que debe tener el valor.
      /// </summary>
      public int MaxLength { get; set; }

      /// <summary>
      /// Devuelve o establece el valor del campo.
      /// </summary>
      public FieldDataType Type { get; set; }

      /// <summary>
      /// Devuelve o establece el icono que debe aparecer como <em>addon</em>.
      /// </summary>
      public string AddonIcon { get; set; }

      /// <summary>
      /// Devuelve o establece el valor del campo.
      /// </summary>
      public override object Value
      {
         get { return _value; }
         set { _value = value.ToString(); }
      }

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
         if (this.Required)
         {
            return !string.IsNullOrWhiteSpace(_value);
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
         this.MinLength = -1;
         this.MaxLength = -1;
         this.Label = string.Empty;
         this.Placeholder = string.Empty;
         this.Description = string.Empty;
         this.Type = FieldDataType.Text;
         this.AddonIcon = string.Empty;
         this.ReadOnly = false;

         _value = string.Empty;
      }

      #endregion

   }
}
