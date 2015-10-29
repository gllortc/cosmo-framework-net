using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.UI
{
   /// <summary>
   /// Implements a view that generates a partial page, that can be included into other views.
   /// </summary>
   public abstract class PartialView : View 
   {
      // Internal data declarations
      private List<ModalView> _modals;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PartialView"/>.
      /// </summary>
      protected PartialView(string domId)
         : base(domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el contenido de la página.
      /// </summary>
      public ControlCollection Content { get; set; }

      /// <summary>
      /// Gets a modal views used by this view.
      /// </summary>
      public List<ModalView> Modals
      {
         get
         {
            if (_modals == null) _modals = new List<ModalView>();
            return _modals;
         }
         set { _modals = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Generate JS call for partial view using data passed as method parameters. 
      /// </summary>
      /// <returns></returns>
      /// <remarks>
      /// This method allows to get invoke call without initialize partial view properties.
      /// </remarks>
      [Obsolete]
      public string GetInvokeCall(params object[] parameters)
      {
         try
         {
            int index = 0;
            string js = string.Empty;

            foreach (ViewParameter param in this.GetType().GetCustomAttributes(typeof(ViewParameter), false))
            {
               js += (string.IsNullOrEmpty(js) ? string.Empty : ",") + "'" + parameters[index] + "'";
               index++;
            }

            return "load" + Script.ConvertToFunctionName(this.DomID) + "(" + js + ");";
         }
         catch (Exception ex)
         {
            throw new ArgumentException("ERROR generating JavaScript invocation call in partial view " + GetType().Name, ex);
         }
      }

      /// <summary>
      /// Generate JS call for partial view using data passed as method parameters. 
      /// </summary>
      /// <returns></returns>
      /// <remarks>
      /// This method allows to get invoke call without initialize partial view properties.
      /// </remarks>
      public string GetInvokeCall(Dictionary<string, object> parameters)
      {
         int index = 0;
         string js = string.Empty;
         string value = string.Empty;

         try
         {
            foreach (ViewParameter param in this.GetType().GetCustomAttributes(typeof(ViewParameter), false))
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

            return "load" + Script.ConvertToFunctionName(this.DomID) + "(" + js + ");";
         }
         catch (Exception ex)
         {
            throw new ArgumentException("ERROR generating JavaScript invocation call in partial view " + GetType().Name, ex);
         }
      }

      /// <summary>
      /// Generate JS call from partial view. 
      /// </summary>
      /// <returns></returns>
      /// <remarks>
      /// Partial view must have initialized properties.
      /// </remarks>
      public string GetInvokeCall()
      {
         try
         {
            string js = string.Empty;

            foreach (ViewParameter param in this.GetType().GetCustomAttributes(typeof(ViewParameter), false))
            {
               js += (string.IsNullOrEmpty(js) ? string.Empty : ",") +
                     this.GetType().GetProperty(param.PropertyName).GetValue(this, null).ToString();
            }

            return "load" + Script.ConvertToFunctionName(this.DomID) + "(" + js + ");";
         }
         catch
         {
            return string.Empty;
         }
      }

      /// <summary>
      /// Generates a script that load the partial view into a <see cref="PartialViewContainerControl"/> control.
      /// </summary>
      /// <param name="executionType">Type of script execution.</param>
      /// <param name="parameters">Partial view parameters.</param>
      /// <returns>The requestes script instance.</returns>
      public Script GetInvokeScript(Script.ScriptExecutionMethod executionType, params object[] parameters)
      {
         Script script = new SimpleScript(this);
         script.ExecutionType = executionType;
         script.AppendSourceLine(GetInvokeCall(parameters));

         return script;
      }

      /// <summary>
      /// Generates a script that load the partial view into a <see cref="PartialViewContainerControl"/> control.
      /// </summary>
      /// <param name="executionType">Type of script execution.</param>
      /// <returns>The requestes script instance.</returns>
      public Script GetInvokeScript(Script.ScriptExecutionMethod executionType)
      {
         Script script = new SimpleScript(this);
         script.ExecutionType = executionType;
         script.AppendSourceLine(GetInvokeCall());

         return script;
      }

      /// <summary>
      /// Generates the script that allow load and show partial view.
      /// </summary>
      /// <returns>A <see cref="Script"/> instance containing the requestes script.</returns>
      public Script GetLoadPartialViewScript()
      {
         return new PartialViewLoadScript(this);
      }

      #endregion

      #region View Implementation

      /// <summary>
      /// Gets the control container used to store the view controls.
      /// </summary>
      public override IControlContainer ControlContainer
      {
         get { return this.Content; }
      }

      /// <summary>
      /// Muestra un mensaje de error.
      /// </summary>
      /// <param name="title">Título del error.</param>
      /// <param name="description">Descripción del error.</param>
      public override void ShowError(string title, string description)
      {
         StringBuilder xhtml = new StringBuilder();

         // Limpia el contenido de la zona principal dónde se va a mostrar el mensaje
         Content.Clear();
         Content.Clear();

         // Genera el mensaje de error
         CalloutControl callout = new CalloutControl(this);
         callout.Type = ComponentColorScheme.Error;
         callout.Title = title;
         callout.Text = description;

         // Agrega el mensaje al contenido
         Content.Add(callout);
      }

      /// <summary>
      /// Muestra un mensaje de error.
      /// </summary>
      /// <param name="exception">Excepción a mostrar.</param>
      public override void ShowError(Exception exception)
      {
         StringBuilder xhtml = new StringBuilder();

         // Limpia el contenido de la zona principal dónde se va a mostrar el mensaje
         Content.Clear();
         Content.Clear();

         // Agrega el mensaje al contenido
         Content.Add(new ErrorControl(this, exception));
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Content = new ControlCollection();
         this._modals = null;
      }
      
      /// <summary>
      /// Devuelve la instancia de <see cref="FormControl"/> correspondiente al formulario recibido (con los datos actualizados) si la validación 
      /// es correcta o <c>null</c> en cualquier otro caso.
      /// </summary>
      private FormControl GetProcessedForm()
      {
         FormControl recvForm = null;
         string formDomId = Parameters.GetString(FormControl.FORM_ID);

         // Obtiene el formulario
         recvForm = (FormControl)Content.Get(formDomId);

         // Si condigue encontrar el formulario, lo proceso y lo devuelve
         if (recvForm.ProcessForm(Parameters))
         {
            return recvForm;
         }
         else
         {
            return null;
         }
      }

      #endregion

   }
}
