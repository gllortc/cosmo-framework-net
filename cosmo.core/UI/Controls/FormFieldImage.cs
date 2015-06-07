namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo que permite seleccionar una imagen.
   /// </summary>
   public class FormFieldImage : Control //, IFormField
   {
      // Declaración de variables internas
      private string _value;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldImage"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      public FormFieldImage(View parentViewport, string domId)
         : base(parentViewport, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldImage"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      public FormFieldImage(View parentViewport, string domId, string label)
         : base(parentViewport, domId)
      {
         Initialize();

         this.Label = label;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldImage"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      /// <param name="value"></param>
      public FormFieldImage(View parentViewport, string domId, string label, string value)
         : base(parentViewport, domId)
      {
         Initialize();

         this.Label = label;
         this.Value = value;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldImage"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      /// <param name="description"></param>
      /// <param name="value"></param>
      public FormFieldImage(View parentViewport, string domId, string label, string description, string value)
         : base(parentViewport, domId)
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
      /// Devuelve o establece la URL (relativa o absoluta) de la imagen que se debe previsualizar.
      /// </summary>
      public string PreviewUrl { get; set; }

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
         this.PreviewUrl = string.Empty;

         _value = string.Empty;
      }

      #endregion

   }
}
