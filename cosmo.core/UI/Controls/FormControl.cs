﻿using Cosmo.Net;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un formulario web.
   /// </summary>
   public class FormControl : Control, IControlSingleContainer
   {
      /// <summary>Acción: Send Form</summary>
      internal const string FORM_ACTION_SEND = "_frm-send_";
      /// <summary>Parámetro que contiene el identificador de formulario enviado.</summary>
      internal const string FORM_ID = "_frm-id_";

      #region Enumerations

      /// <summary>
      /// Enumera los tipos de envio de datos soportados por el control.
      /// </summary>
      public enum FormSendDataMethod
      {
         /// <summary>Envia el formulario mediante un envio estándard mediante botón tipo <c>Submit</c>.</summary>
         ButtonSubmit,
         /// <summary>Envia el formulario mediante AJAX.</summary>
         JSSubmit
      }

      /// <summary>
      /// Enumerate the validation status of the form.
      /// </summary>
      public enum ValidationStatus
      {
         /// <summary>The form isn't yet validated.</summary>
         NotValidated,
         /// <summary>The form data is valid.</summary>
         ValidData,
         /// <summary>The form data isn't valid.</summary>
         InvalidData
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public FormControl(View parentView)
         : base(parentView)
      {
         Initialize();

         // Genera un ID de componente aleatorio
         Random rnd = new Random();
         this.DomID = "frmId" + rnd.Next(1, 10000);
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormControl(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el código de icono a mostrar junto al título del formulario.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Gets or sets el título visible del formulario.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or sets la URL dónde se mandarán los datos del formulario.
      /// </summary>
      public string Action { get; set; }

      /// <summary>
      /// Gets or sets el método de envio de datos, <em>get</em> o <em>post</em>.
      /// </summary>
      public string Method { get; set; }

      /// <summary>
      /// Indica si el formulario se debe mandar como <c>multipart/modal-data</c>.
      /// </summary>
      public bool IsMultipart { get; set; }

      /// <summary>
      /// Contiene la lista de botones del formulario.
      /// </summary>
      public List<ButtonControl> FormButtons { get; set; }

      /// <summary>
      /// Contiene la lista de campos (o controles) del formulario.
      /// </summary>
      public ControlCollection Content { get; set; }

      /// <summary>
      /// Indica si se debe colocar el formulario dentro de un control panel.
      /// </summary>
      public bool UsePanel { get; set; }

      /// <summary>
      /// Gets or sets el método de envio de los datos del formulario.
      /// </summary>
      public FormSendDataMethod SendDataMethod { get; set; }

      /// <summary>
      /// Gets or sets the validation status of this control.
      /// </summary>
      public ValidationStatus IsValid { get; internal set; }

      /// <summary>
      /// Gets the number of files uploaded in the form.
      /// </summary>
      public int UploadedFiles { get; internal set; }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega un valor de configuración al formulario.
      /// Estos valores equivalen a los campos <em>hidden</em>.
      /// </summary>
      /// <param name="key">Clave del valor.</param>
      /// <param name="value">Valor.</param>
      public void AddFormSetting(string key, string value)
      {
         Content.Add(new FormFieldHidden(ParentView, key, value));
      }

      /// <summary>
      /// Agrega un valor de configuración al formulario.
      /// Estos valores equivalen a los campos <em>hidden</em>.
      /// </summary>
      /// <param name="key">Clave del valor.</param>
      /// <param name="value">Valor.</param>
      public void AddFormSetting(string key, int value)
      {
         AddFormSetting(key, value.ToString());
      }

      /// <summary>
      /// Agrega un valor de configuración al formulario.
      /// Estos valores equivalen a los campos <em>hidden</em>.
      /// </summary>
      /// <param name="key">Clave del valor.</param>
      /// <param name="value">Valor.</param>
      public void AddFormSetting(string key, bool value)
      {
         AddFormSetting(key, (value ? "1" : "0"));
      }

      /// <summary>
      /// Establece el valor de un campo del formulario.
      /// </summary>
      /// <param name="domId">Identificador único del campo (parámetro ID del DOM).</param>
      /// <param name="value">Valor a establecer.</param>
      public void SetFieldValue(string domId, string value)
      {
         try
         {
            // Busca el campo en el contenido
            FormField control = (FormField)Content.Get(domId);
            if (control != null)
            {
               if (control.GetType() == typeof(FormFieldDate))
               {
                  control.Value = DateTime.Parse(value);
               }
               else
               {
                  control.Value = value;
               }
            }
         }
         catch
         {
            throw new Exception("El control #" + domId + " no implementa el interface IFormField.");
         }
      }

      /// <summary>
      /// Establece el valor de un campo del formulario.
      /// </summary>
      /// <param name="domId">Identificador único del campo (parámetro ID del DOM).</param>
      /// <param name="value">Valor a establecer.</param>
      public void SetFieldValue(string domId, bool value)
      {
         SetFieldValue(domId, value ? "1" : "0");
      }

      /// <summary>
      /// Establece el valor de un campo del formulario.
      /// </summary>
      /// <param name="domId">Identificador único del campo (parámetro ID del DOM).</param>
      /// <param name="value">Valor a establecer.</param>
      public void SetFieldValue(string domId, int value)
      {
         SetFieldValue(domId, value.ToString());
      }

      /// <summary>
      /// Devuelve un determinado campo del formulario.
      /// </summary>
      /// <param name="domId">Identificador único del campo.</param>
      /// <returns>Una instancia de <see cref="FormField"/> que representa el campo solicitado o <c>null</c> si no se encuentra el campo.</returns>
      public FormField GetField(string domId)
      {
         FormField field;

         // Obtiene el control estándard
         field = (FormField)Content.Get(domId);
         if (field != null)
         {
            return field;
         }

         return null;
      }

      /// <summary>
      /// Obtiene el valor de un campo como cadena de texto.
      /// </summary>
      /// <param name="domId">Identificador (DOM) del campo solicitado.</param>
      /// <returns>Devuelve la cadena de texto solicitada o <c>string.Empty</c> si se produce cualquier error de transformación.</returns>
      public string GetStringFieldValue(string domId)
      {
         return GetStringFieldValue(domId, string.Empty);
      }

      /// <summary>
      /// Obtiene el valor de un campo como cadena de texto.
      /// </summary>
      /// <param name="domId">Identificador (DOM) del campo solicitado.</param>
      /// <param name="defaultValue">Valor a devolver en caso de producirse un error de transformación.</param>
      /// <returns>Devuelve la cadena de texto solicitada o <c>defaultValue</c> si se produce cualquier error de transformación.</returns>
      public string GetStringFieldValue(string domId, string defaultValue)
      {
         try
         {
            FormField field = (FormField)Content.Get(domId);
            if (field == null) return defaultValue;

            return field.Value.ToString();
         }
         catch
         {
            return defaultValue;
         }
      }

      /// <summary>
      /// Gets the value from a form field as a boolean value.
      /// </summary>
      /// <param name="domId">Identificador (DOM) del campo solicitado.</param>
      /// <returns>Devuelve el valor booleano o <c>false</c> si se produce cualquier error de transformación.</returns>
      public bool GetBoolFieldValue(string domId)
      {
         return BooleanUtils.ToBoolean(((FormField)Content.Get(domId)).Value.ToString(), false);
      }

      /// <summary>
      /// Obtiene el valor de un campo como número entero.
      /// </summary>
      /// <param name="domId">Identificador (DOM) del campo solicitado.</param>
      /// <returns>Devuelve el número entero solicitado o <c>0</c> si se produce cualquier error de transformación.</returns>
      public int GetIntFieldValue(string domId)
      {
         return GetIntFieldValue(domId, 0);
      }

      /// <summary>
      /// Obtiene el valor de un campo como número entero.
      /// </summary>
      /// <param name="domId">Identificador (DOM) del campo solicitado.</param>
      /// <param name="defaultValue">Valor a devolver en caso de producirse un error de transformación.</param>
      /// <returns>Devuelve el número entero solicitado o <c>defaultValue</c> si se produce cualquier error de transformación.</returns>
      public int GetIntFieldValue(string domId, int defaultValue)
      {
         return Number.ToInteger(((FormField)Content.Get(domId)).Value.ToString(), defaultValue);
      }

      /// <summary>
      /// Obtiene el valor de un campo como fecha/hora.
      /// </summary>
      /// <param name="domId">Identificador (DOM) del campo solicitado.</param>
      /// <returns>Devuelve la hora/fecha solicitada o la fecha/hora actual si se produce cualquier error de transformación.</returns>
      public DateTime GetDateFieldValue(string domId)
      {
         return GetDateFieldValue(domId, DateTime.Now);
      }

      /// <summary>
      /// Obtiene el valor de un campo como fecha/hora.
      /// </summary>
      /// <param name="domId">Identificador (DOM) del campo solicitado.</param>
      /// <param name="defaultValue">Valor a devolver en caso de producirse un error de transformación.</param>
      /// <returns>Devuelve la hora/fecha solicitada o <c>defaultValue</c> si se produce cualquier error de transformación.</returns>
      public DateTime GetDateFieldValue(string domId, DateTime defaultValue)
      {
         try
         {
            FormField field = (FormField)Content.Get(domId);
            if (field == null)
            {
               return defaultValue;
            }

            if (field.Value.GetType() == typeof(DateTime))
            {
               return (DateTime)field.Value;
            }

            return defaultValue;
         }
         catch
         {
            return defaultValue;
         }
      }

      /// <summary>
      /// Para un campo de archivo, descarga el archivo, lo almacena y devuelve una referencia.
      /// </summary>
      /// <param name="domId">Identificador (DOM) del campo solicitado.</param>
      /// <returns></returns>
      public FileInfo GetFileFieldValue(string domId)
      {
         try
         {
            FormField field = (FormField)Content.Get(domId);
            if (field == null)
            {
               return null;
            }

            return (FileInfo)field.Value;
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// Procesa los datos recibidos.
      /// </summary>
      /// <param name="parameters">Una instancia de <see cref="Url"/> que contiene los datos recibidos al formulario.</param>
      /// <returns><c>true</c> si el contenido del formulario es correcto o <c>false</c> en cualquier otro caso.</returns>
      public bool ProcessForm(Url parameters)
      {
         bool isValidForm = true;
         bool hasComplexFields = false;
         List<FormField> controls = Content.GetFormFields();

         try
         {
            // Procesa los campos simples (texto, números, etc)
            foreach (FormField field in controls)
            {
               // Obtiene el valor y recoge el resultado de la validación
               if (field.FieldType != FormField.FieldTypes.Upload)
               {
                  isValidForm = isValidForm && field.LoadValueFromRequest();
               }
               else
               {
                  hasComplexFields = true;
               }
            }

            // Procesa los campos complejos (archivos)
            if (isValidForm && hasComplexFields)
            {
               foreach (FormField field in controls)
               {
                  // Obtiene el valor y recoge el resultado de la validación
                  if (field.FieldType == FormField.FieldTypes.Upload)
                  {
                     isValidForm = isValidForm && field.LoadValueFromRequest();

                     // Compute the number of uploaded files
                     if (field.Value is FileInfo && ((FileInfo)field.Value).Exists)
                     {
                        this.UploadedFiles++;
                     }

                  }
               }
            }

            // Update the validation status
            this.IsValid = isValidForm ? ValidationStatus.ValidData : ValidationStatus.InvalidData;

            return isValidForm;
         }
         catch (Exception ex)
         {
            ParentView.Workspace.Logger.Error(this, "ProcessForm", ex);
            throw ex;
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.IsMultipart = false;
         this.UsePanel = true;
         this.Icon = string.Empty;
         this.Text = string.Empty;
         this.Action = ParentView.Workspace.Context.Request.Path;
         this.Method = "post";
         this.Content = new ControlCollection();
         this.FormButtons = new List<ButtonControl>();
         this.SendDataMethod = FormSendDataMethod.ButtonSubmit;
         this.IsValid = ValidationStatus.NotValidated;
         this.UploadedFiles = 0;
      }

      #endregion

   }
}
