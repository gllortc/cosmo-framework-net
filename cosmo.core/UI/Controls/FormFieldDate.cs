using Cosmo.Net;
using System;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo de tipo fecha/hora.
   /// </summary>
   public class FormFieldDate : FormField
   {
      // Internal data declarations
      private DateTime _value;

      #region Enumerations

      /// <summary>
      /// Enumera los distintos tipos de contenido que permite albergar el control
      /// </summary>
      public enum FieldDateType
      {
         /// <summary>Fecha</summary>
         Date,
         /// <summary>Hora</summary>
         Time,
         /// <summary>Fecha y hora</summary>
         Datetime
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldDate(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      public FormFieldDate(View parentView, string domId, string label, FieldDateType type)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
         this.Type = type;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      /// <param name="value"></param>
      public FormFieldDate(View parentView, string domId, string label, FieldDateType type, string value)
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
      /// Gets or sets a boolean value indicating if the field is required to validate the form.
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
      /// Gets or sets el valor del campo.
      /// </summary>
      public FieldDateType Type { get; set; }

      /// <summary>
      /// Gets or sets el valor del campo.
      /// </summary>
      public override object Value
      {
         get { return _value; }
         set { _value = (DateTime)value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Gets the field value from the request.
      /// </summary>
      public override bool LoadValueFromRequest()
      {
         try
         {
            this.Value = DateTime.Parse(Url.GetString(ParentView.Workspace.Context.Request.Params, this.DomID));
         }
         catch
         {
            this.Value = DateTime.Now;
         }

         return Validate();
      }

      /// <summary>
      /// Validate the field value.
      /// </summary>
      /// <c>true</c> if the field value is valid or <c>false</c> if the value is not valid.
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
         this.Placeholder = string.Empty;
         this.Description = string.Empty;
         this.Value = DateTime.Now;
         this.Type = FieldDateType.Date;
      }

      #endregion

   }
}
