using Cosmo.Net;
using Cosmo.Utils.Drawing;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo de comprobación para humanos.
   /// </summary>
   public class FormFieldCaptcha : FormField
   {
      /// <summary>Nombre del campo por defecto.</summary>
      public const string CAPTCHA_FIELD_NAME = "_hchk_";

      // Declara variables internas
      private string _value;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldCaptcha"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public FormFieldCaptcha(View parentView)
         : base(parentView)
      {
         Initialize();

         this.DomID = FormFieldCaptcha.CAPTCHA_FIELD_NAME;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldCaptcha"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldCaptcha(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldCaptcha"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      public FormFieldCaptcha(View parentView, string domId, string label)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldCaptcha"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="value"></param>
      public FormFieldCaptcha(View parentView, string domId, string label, string value)
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
         get { return FieldTypes.Check; }
      }

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
         try
         {
            return _value.Equals(ParentView.Workspace.Context.Session[Captcha.SESSION_CAPTCHA]);
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      /// Permite validar el campo CAPTCHA.
      /// </summary>
      /// <param name="fieldValue">Valor introducido por el usuario.</param>
      /// <param name="workspace">Workspace actual.</param>
      /// <returns><c>true</c> si el código es correcto o <c>false</c> en cualquier otro caso.</returns>
      public static bool Validate(string fieldValue, Workspace workspace)
      {
         try
         {
            return fieldValue.Equals(workspace.Context.Session[Captcha.SESSION_CAPTCHA]);
         }
         catch
         {
            return false;
         }
      }

      #endregion  

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Label = string.Empty;
         this.Placeholder = string.Empty;
         this.Description = string.Empty;
         this.Value = string.Empty;
      }

      #endregion

   }
}
