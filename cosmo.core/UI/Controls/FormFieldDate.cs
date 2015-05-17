﻿using Cosmo.Net;
using System;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo de tipo fecha/hora.
   /// </summary>
   public class FormFieldDate : FormField
   {
      // Declara variables internas
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
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      public FormFieldDate(ViewContainer parentViewport, string domId)
         : base(parentViewport, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      public FormFieldDate(ViewContainer parentViewport, string domId, string label, FieldDateType type)
         : base(parentViewport, domId)
      {
         Initialize();

         this.Label = label;
         this.Type = type;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      /// <param name="value"></param>
      public FormFieldDate(ViewContainer parentViewport, string domId, string label, FieldDateType type, string value)
         : base(parentViewport, domId)
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
      /// Devuelve o establece el valor del campo.
      /// </summary>
      public FieldDateType Type { get; set; }

      /// <summary>
      /// Devuelve o establece el valor del campo.
      /// </summary>
      public override object Value
      {
         get { return _value; }
         set { _value = (DateTime)value; }
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
            this.Value = DateTime.Parse(Url.GetString(Container.Workspace.Context.Request.Params, this.DomID));
         }
         catch
         {
            this.Value = DateTime.Now;
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
      /// Inicializa la instancia.
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
