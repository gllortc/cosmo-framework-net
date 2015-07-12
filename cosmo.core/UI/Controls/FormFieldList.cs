﻿using Cosmo.Net;
using Cosmo.Utils;
using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo de texto simple (Input).
   /// </summary>
   public class FormFieldList : FormField
   {

      #region Enumerations

      /// <summary>
      /// Enumera los distintos tipos de lista que permite albergar el control
      /// </summary>
      public enum ListType
      {
         /// <summary>Permite seleccionar múltiples elementos</summary>
         Multiple,
         /// <summary>Permite seleccionar sólo un elemento</summary>
         Single
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldList"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldList(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldList"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      public FormFieldList(View parentView, string domId, string label, ListType type)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
         this.Type = type;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldList"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="type"></param>
      /// <param name="value"></param>
      public FormFieldList(View parentView, string domId, string label, ListType type, string value)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
         this.Type = type;
         this.Value = value;
      }

      // TODO: Fer que es pugui inicialitzar amb un a llista estàtica del DataService

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
      public override object Value { get; set; }

      /// <summary>
      /// Gets or sets el valor del campo.
      /// </summary>
      public ListType Type { get; set; }

      /// <summary>
      /// Contiene la lista de valores del campo.
      /// </summary>
      public List<KeyValue> Values { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Carga los valores de un <c>DataList</c> como valores de la lista.
      /// </summary>
      /// <param name="dataListID">Identificador del <c>DataList</c>.</param>
      public void LoadValuesFromDataList(string dataListID)
      {
         Values = ParentView.Workspace.DataService.GetDataList(dataListID).Values;
         Value = ParentView.Workspace.DataService.GetDataList(dataListID).DefaultValue;
      }

      /// <summary>
      /// Obtiene el valor del campo a partir de los datos recibidos mediante GET o POST.
      /// </summary>
      public override bool LoadValueFromRequest()
      {
         try
         {
            Value = Url.GetString(ParentView.Workspace.Context.Request.Params, this.DomID);
         }
         catch
         {
            Value = string.Empty;
         }

         return Validate();
      }

      /// <summary>
      /// Valida el valor del campo.
      /// </summary>
      /// <returns><c>true</c> si el valor és aceptable o <c>false</c> si el valor no es válido.</returns>
      public override bool Validate()
      {
         if (Required)
         {
            return !string.IsNullOrWhiteSpace(Value.ToString());
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
         Required = false;
         Label = string.Empty;
         Placeholder = string.Empty;
         Description = string.Empty;
         Value = string.Empty;
         Type = ListType.Single;
         Values = new List<KeyValue>();
      }

      #endregion

   }
}
