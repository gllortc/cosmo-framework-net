using Cosmo.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Abstract class that must be implemented by all scripting elements.
   /// </summary>
   public abstract class Script
   {

      #region Enumerations

      /// <summary>
      /// Types of script execution.
      /// </summary>
      public enum ScriptExecutionMethod
      {
         /// <summary>The script is executen on document ready event.</summary>
         OnDocumentReady,
         /// <summary>The script is executed when an concrete event is raised.</summary>
         OnEvent,
         /// <summary>The script is encapsulated in a function.</summary>
         OnFunctionCall,
         /// <summary>The script is included in page and is invoked by other script or object.</summary>
         Standalone
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="Script"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      protected Script(View parentView)
      {
         Initialize();

         this.ParentView = parentView;
      }

      /// <summary>
      /// Gets a new instance of <see cref="Script"/>.
      /// </summary>
      /// <param name="viewport">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="source"´>Una cadena que contiene código JavaScript.</param>
      protected Script(View parentView, string source)
      {
         Initialize();

         this.ParentView = parentView;
         this.Source.AppendLine(source);
      }

      /// <summary>
      /// Gets a new instance of <see cref="Script"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="source">Una instancia de <see cref="StringBuilder"/> que contiene código JavaScript.</param>
      protected Script(View parentView, StringBuilder source)
      {
         Initialize();

         this.ParentView = parentView;
         this.Source = source;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets a value indicating if the script must be invoked on page complete load.
      /// </summary>
      public bool InvokeOnLoad { get; set; }

      /// <summary>
      /// Indica si el script se debe ejecutar en el evento <c>$( document ).ready()</c> 
      /// de <em>jQuery</em>.
      /// </summary>
      public ScriptExecutionMethod ExecutionType { get; set; }

      /// <summary>
      /// Devuelve la pParent <see cref="View"/> which acts as a container of the control.
      /// </summary>
      public View ParentView { get; internal set; }

      /// <summary>
      /// Gets or sets the function name to use with encapsulate the script into a function.
      /// </summary>
      public string FunctionName { get; set; }

      /// <summary>
      /// Gets or sets el código fuente del script.
      /// </summary>
      internal StringBuilder Source { get; set; }

      /// <summary>
      /// Gets or sets the DOM element ID to attach the source script.
      /// </summary>
      internal string EventDomID { get; set; }

      /// <summary>
      /// Gets or sets the event name to attach the source script.
      /// </summary>
      internal string EventName { get; set; }

      #endregion 

      #region Methods

      /// <summary>
      /// Añade un comentario al código.
      /// </summary>
      /// <param name="comment"></param>
      public Script AppendComment(string comment)
      {
         Source.AppendLine("<!-- " + comment + " -->");
         return this;
      }

      /// <summary>
      /// Añade una línea de código fuente.
      /// </summary>
      /// <param name="code"></param>
      public Script AppendSourceLine(string code)
      {
         Source.AppendLine(code);
         return this;
      }

      /// <summary>
      /// Añade código fuente.
      /// </summary>
      /// <param name="code"></param>
      public Script AppendSource(string code)
      {
         Source.Append(code);
         return this;
      }

      /// <summary>
      /// Carga el código del script desde un archivo.
      /// </summary>
      /// <param name="filename">Nombre (con ruta) del archivo a cargar.</param>
      public void LoadFromFile(string filename)
      {
         string line;
         StringBuilder sb = new StringBuilder();

         StreamReader file = new StreamReader(filename);
         while ((line = file.ReadLine()) != null)
         {
            sb.AppendLine(line);
         }

         Source = sb;
      }

      /// <summary>
      /// Attach the source of script to an event in DOM element.
      /// </summary>
      /// <param name="domId">DOM element ID.</param>
      /// <param name="eventName">Event name.</param>
      public void AttachToEvent(string domId, string eventName)
      {
         this.EventDomID = domId;
         this.EventName = eventName;
      }

      #endregion

      #region Abstract Members

      // <summary>
      /// Make the JavaScript source code of script.
      /// </summary>
      /// <returns>A string containing the requestes source code of script.</returns>
      public abstract void BuildSource();

      /// <summary>
      /// Make the JavaScript source code of script.
      /// </summary>
      /// <returns>A string containing the requestes source code of script.</returns>
      public virtual string GetSource()
      {
         // Build JavaScript source using the instance data
         BuildSource();

         if (this.ExecutionType == ScriptExecutionMethod.OnDocumentReady || this.ExecutionType == ScriptExecutionMethod.Standalone)
         {
            return this.Source.ToString();
         }
         else if (this.ExecutionType == ScriptExecutionMethod.OnFunctionCall)
         {
            if (string.IsNullOrWhiteSpace(this.FunctionName))
            {
               throw new ArgumentException("Can't generatoe JS function: function name is missing.");
            }

            StringBuilder js = new StringBuilder();
            js.AppendLine("function " + Script.ConvertToFunctionName(this.FunctionName) + "() {");
            js.AppendLine(Source.ToString());
            js.AppendLine("}");

            return js.ToString();
         }
         else if (this.ExecutionType == ScriptExecutionMethod.OnEvent)
         {
            if (string.IsNullOrWhiteSpace(this.EventDomID) || string.IsNullOrWhiteSpace(this.EventName))
            {
               throw new ArgumentException("Can't attach JS script to event: DOM element or event name is missing.");
            }

            StringBuilder js = new StringBuilder();
            js.AppendLine("$('#" + this.EventDomID + "').on('" + this.EventName + "', function () {");
            js.AppendLine(Source.ToString());
            js.AppendLine("});");

            return js.ToString();
         }

         throw new ArgumentException("Script execution type not supported.");
      }

      /// <summary>
      /// Generate a simple JavaScript call without parameters. 
      /// </summary>
      /// <returns></returns>
      public string GetInvokeCall()
      {
         return Script.ConvertToFunctionName(this.FunctionName) + "();";
      }

      /// <summary>
      /// Generate JS call for modal using data passed as method parameters. 
      /// </summary>
      /// <returns></returns>
      /// <remarks>
      /// This method allows to get invoke call without initialize modal properties.
      /// </remarks>
      public string GetInvokeCall(View parentView, Dictionary<string, object> parameters)
      {
         int index = 0;
         string js = string.Empty;
         string value = string.Empty;

         try
         {
            foreach (ViewParameter param in parentView.GetType().GetCustomAttributes(typeof(ViewParameter), false))
            {
               if (parameters.ContainsKey(param.ParameterName))
               {
                  if (parameters[param.ParameterName] is bool)
                  {
                     value = ((bool)parameters[param.ParameterName] ? "true" : "false");
                  }
                  else if (parameters[param.ParameterName] is Int16 ||
                           parameters[param.ParameterName] is Int32 ||
                           parameters[param.ParameterName] is Int64)
                  {
                     value = string.Empty + (int)parameters[param.ParameterName];
                  }
                  else
                  {
                     value = "'" + parameters[param.ParameterName].ToString() + "'";
                  }

                  js += (string.IsNullOrEmpty(js) ? string.Empty : ",") + value;
                  index++;
               }
            }

            return Script.ConvertToFunctionName(this.FunctionName) + "(" + js + ");";
         }
         catch (Exception ex)
         {
            throw new ArgumentException("ERROR generating JavaScript invocation call in " + parentView.GetType().Name, ex);
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve el valor de un campo en el formulario via <c>scripting</c>.
      /// </summary>
      /// <param name="fieldDomId">Nombre del campo (DOM ID) para el que se desea obtener el valor.</param>
      /// <returns>Una secuéncia JavaScript (JQuery) que obtiene el valor del campo.</returns>
      public static string GetFieldValue(string fieldDomId)
      {
         return "$('#" + fieldDomId + "').val()";
      }

      /// <summary>
      /// Devuelve el valor de un campo en el formulario via <c>scripting</c>.
      /// </summary>
      /// <param name="formName">Nombre del formulario al que pertenece el campo.</param>
      /// <param name="field">Campo para el que se desea obtener el valor (actualmente sólo funciona para campos de tipo INPUT y SELECT).</param>
      /// <returns>Una secuéncia JavaScript (JQuery) que obtiene el valor del campo.</returns>
      public static string GetFieldValue(string formName, FormField field)
      {
         if (field is FormFieldList)
         {
            return "$('#" + formName + " select[name=" + field.DomID + "]').val()";
         }
         else
         {
            return "$('#" + formName + " input[name=" + field.DomID + "]').val()";
         }
      }

      /// <summary>
      /// Transform a DOM ID to a string that can be used as a part of a JavaScript function name.
      /// </summary>
      /// <param name="domId">DOM ID to be converted.</param>
      /// <returns>A string that can be used as a part of a JavaScript function name.</returns>
      public static string ConvertToFunctionName(string domId)
      {
         return domId.Trim().Replace("_", string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);
      }

      /// <summary>
      /// Convert a JavaScript method call to link.
      /// </summary>
      /// <param name="jsInvokeCall">The JavaScript method call to transform.</param>
      /// <returns>A string containing the requested link.</returns>
      public static string ConvertInvokeCallToLink(string jsInvokeCall)
      {
         if (string.IsNullOrWhiteSpace(jsInvokeCall))
         {
            return "#";
         }

         return "javascript:" + jsInvokeCall.Trim();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.InvokeOnLoad = false;
         this.ExecutionType = ScriptExecutionMethod.Standalone;
         this.ParentView = null;
         this.Source = new StringBuilder();
      }
      
      #endregion

      #region Child classes

      /// <summary>
      /// Static class that contains the event constants.
      /// </summary>
      public static class Events
      {
         /// <summary>Bootstrap/jQuery event: On Modal Hide</summary>
         public const string EVENT_ON_MODAL_CLOSE = "hidden.bs.modal";
      }

      #endregion

   }
}
