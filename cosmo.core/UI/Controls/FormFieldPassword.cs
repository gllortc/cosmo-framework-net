using Cosmo.Net;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo de contraseña.
   /// </summary>
   public class FormFieldPassword : FormField
   {
      // Declara variables internas
      private string _value;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldPassword(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      public FormFieldPassword(View parentView, string domId, string label)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="value"></param>
      public FormFieldPassword(View parentView, string domId, string label, string value)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
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
      /// Indica si el campo requiere un segundo campo de verificación.
      /// </summary>
      public bool RewriteRequired { get; set; }

      /// <summary>
      /// Gets or sets el texto que mostrará la etiqueta del campo.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Gets or sets un texto en el control que desaparece cuando contiene algún valor.
      /// </summary>
      public string Placeholder { get; set; }

      /// <summary>
      /// Gets or sets un texto para mostrar en el control de verificación del valor.
      /// </summary>
      public string RewriteFieldPlaceholder { get; set; }

      /// <summary>
      /// Gets or sets una descripción que aparecerá en pequeño junto al campo (dependiendo de la plantilla de renderización).
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets la longitud (en carácteres) mínima que debe tener el valor.
      /// </summary>
      public int MinLength { get; set; }

      /// <summary>
      /// Gets or sets la longitud (en carácteres) máxima que debe tener el valor.
      /// </summary>
      public int MaxLength { get; set; }

      /// <summary>
      /// Gets or sets el valor del campo.
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
            this.Value = Url.GetString(ParentView.Workspace.Context.Request.Params, this.DomID);
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

         // Validación de campo requerido
         if (this.Required)
         {
            valid &= !string.IsNullOrWhiteSpace(_value);
         }

         // Validación de doble escritura
         if (this.RewriteRequired)
         {
            valid &= _value.Equals(ParentView.Workspace.Context.Request.Params[DomID + FormField.FIELD_CHECK_POST_DOMID]);
         }

         return valid;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Required = false;
         this.RewriteRequired = false;
         this.MinLength = -1;
         this.MaxLength = -1;
         this.Label = string.Empty;
         this.Placeholder = string.Empty;
         this.Description = string.Empty;
         this.RewriteFieldPlaceholder = string.Empty;

         _value = string.Empty;
      }

      #endregion

   }
}
