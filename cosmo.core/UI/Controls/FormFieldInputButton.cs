using Cosmo.Net;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Input control with a button integrated at the right side.
   /// </summary>
   public class FormFieldInputButton : FormField
   {
      // Internal data declarations
      private string _value;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldInputButton"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldInputButton(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldInputButton"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label">Label text.</param>
      public FormFieldInputButton(View parentView, string domId, string label)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldInputButton"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label">Label text.</param>
      /// <param name="value">Predefined value of the input control.</param>
      public FormFieldInputButton(View parentView, string domId, string label, string value)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
         this.Value = value;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the type of implemented field.
      /// </summary>
      public override FieldTypes FieldType
      {
         get { return FieldTypes.Standard; }
      }

      /// <summary>
      /// Gets or sets a boolean value indicating if the field is read only or can be edited.
      /// </summary>
      public bool ReadOnly { get; set; }

      /// <summary>
      /// Gets or sets a boolean value indicating if the field is required to validate the form.
      /// </summary>
      public bool Required { get; set; }

      /// <summary>
      /// Gets or sets the label text.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Gets or sets the descriptive text that appears inside the input control.
      /// </summary>
      public string Placeholder { get; set; }

      /// <summary>
      /// Gets or sets the descriptive text for the control.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets the minimum length (in characters) that must contains the control value.
      /// </summary>
      public int MinLength { get; set; }

      /// <summary>
      /// Gets or sets the maximum length (in characters) that must contains the control value.
      /// </summary>
      public int MaxLength { get; set; }

      /// <summary>
      /// Gets or sets the button icon.
      /// </summary>
      public string ButtonIcon { get; set; }

      /// <summary>
      /// Gets or sets a boolean value indicating if the button is a submit button.
      /// </summary>
      public bool IsButtonSubmit { get; set; }

      /// <summary>
      /// Gets or sets the field value.
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
         this.ButtonIcon = string.Empty;
         this.ReadOnly = false;
         this.IsButtonSubmit = false;

         _value = string.Empty;
      }

      #endregion

   }
}
