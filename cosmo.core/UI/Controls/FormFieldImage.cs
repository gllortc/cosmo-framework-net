namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo que permite seleccionar una imagen.
   /// </summary>
   public class FormFieldImage : Control //, IFormField
   {
      // Internal data declarations
      private string _value;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldImage"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldImage(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldImage"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      public FormFieldImage(View parentView, string domId, string label)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldImage"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="value"></param>
      public FormFieldImage(View parentView, string domId, string label, string value)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
         this.Value = value;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldImage"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="description"></param>
      /// <param name="value"></param>
      public FormFieldImage(View parentView, string domId, string label, string description, string value)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
         this.Value = value;
         this.Description = description;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si el campo es obligatorio.
      /// </summary>
      public bool Required { get; set; }

      /// <summary>
      /// Gets or sets el texto que mostrará la etiqueta del campo.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Gets or sets un texto en el control que desaparece cuando contiene algún valor.
      /// </summary>
      public string Placeholder { get; set; }

      /// <summary>
      /// Gets or sets una descripción que aparecerá en pequeño junto al campo (dependiendo de la plantilla de renderización).
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets la URL (relativa o absoluta) de la imagen que se debe previsualizar.
      /// </summary>
      public string PreviewUrl { get; set; }

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
      public object Value
      {
         get { return _value; }
         set { _value = value.ToString(); }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Valida el valor del campo.
      /// </summary>
      /// <returns><c>true</c> si el valor és aceptable o <c>false</c> si el valor no es válido.</returns>
      public bool Validate()
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
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Required = false;
         this.MinLength = -1;
         this.MaxLength = -1;
         this.Label = string.Empty;
         this.Placeholder = string.Empty;
         this.Description = string.Empty;
         this.PreviewUrl = string.Empty;

         _value = string.Empty;
      }

      #endregion

   }
}
