﻿using Cosmo.Net;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo de texto simple (Input).
   /// </summary>
   public class FormFieldText : FormField 
   {
      // Internal data declarations
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
      /// Gets a new instance of <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldText(View parentView, string domId)
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
      public FormFieldText(View parentView, string domId, string label, FieldDataType type)
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
      public FieldDataType Type { get; set; }

      /// <summary>
      /// Gets or sets el icono que debe aparecer como <em>addon</em>.
      /// </summary>
      public string AddonIcon { get; set; }

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
      /// Gets the field value from the request.
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
      /// Validate the field value.
      /// </summary>
      /// <c>true</c> if the field value is valid or <c>false</c> if the value is not valid.
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
         this.Type = FieldDataType.Text;
         this.AddonIcon = string.Empty;
         this.ReadOnly = false;

         _value = string.Empty;
      }

      #endregion

   }
}
