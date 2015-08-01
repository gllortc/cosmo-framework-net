using Cosmo.Net;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo oculto.
   /// </summary>
   public class FormFieldHidden : FormField
   {
      // Internal data declarations
      private string _value;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldHidden(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="value">Valor inicial del campo.</param>
      public FormFieldHidden(View parentView, string domId, string value)
         : base(parentView, domId)
      {
         Initialize();

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
      /// Validate the field value.
      /// </summary>
      /// <c>true</c> if the field value is valid or <c>false</c> if the value is not valid.
      public override bool Validate()
      {
         return true;
      }

      /// <summary>
      /// Gets the field value from the request.
      /// </summary>
      /// <returns><c>true</c> si el valor obtenido es válido o <c>false</c> si el valor no puede ser aceptado.</returns>
      public override bool LoadValueFromRequest()
      {
         try
         {
            _value = Url.GetString(ParentView.Workspace.Context.Request.Params, this.DomID);
            return true;
         }
         catch
         {
            _value = string.Empty;
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
         _value = string.Empty;
      }

      #endregion

   }
}
