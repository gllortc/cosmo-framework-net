using Cosmo.Net;
using Cosmo.Utils;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo booleano (CheckBox).
   /// </summary>
   public class FormFieldBoolean : FormField
   {
      // Internal data declarations
      private bool _value;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldBoolean"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldBoolean(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldBoolean"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label">Texto visible del control.</param>
      public FormFieldBoolean(View parentView, string domId, string label)
         : base(parentView)
      {
         Initialize();

         this.DomID = domId;
         this.Label = label;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldBoolean"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label">Texto visible del control.</param>
      /// <param name="value">Valor de inicialización que tendrá el campo.</param>
      public FormFieldBoolean(View parentView, string domId, string label, bool value)
         : base(parentView)
      {
         Initialize();

         this.DomID = domId;
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
      /// Gets or sets el texto que mostrará la etiqueta del campo.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Gets or sets una descripción que aparecerá en pequeño junto al campo (dependiendo de la plantilla de renderización).
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets el valor del campo.
      /// </summary>
      public override object Value
      {
         get { return _value; }
         set 
         {
            if (value.GetType() == typeof(bool))
            {
               _value = (bool)value;
            }
            else if (Number.IsIntegerType(value.GetType()))
            {
               _value = ((int) value != 0);
            }
            else
            {
               _value = !(value.ToString().Trim().ToLower().Equals("false") || value.ToString().Trim().ToLower().Equals("0")); 
            }
         }
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
            _value = Url.GetBoolean(ParentView.Workspace.Context.Request.Params, this.DomID);
         }
         catch
         {
            _value = false;
         }

         return Validate();
      }

      /// <summary>
      /// Valida el valor del campo.
      /// </summary>
      /// <returns><c>true</c> si el valor és aceptable o <c>false</c> si el valor no es válido.</returns>
      public override bool Validate()
      {
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
         this.Label = string.Empty;
         this.Label = string.Empty;
         this.Value = false;
      }

      #endregion

   }
}
