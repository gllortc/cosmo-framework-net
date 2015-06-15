using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cosmo.UI
{
   /// <summary>
   /// Implementa una vista que permite generarse sin estar dentro de una página.
   /// </summary>
   public abstract class PartialView : View 
   {
      // Internal data declarations
      private List<ModalView> _modals;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="PartialView"/>.
      /// </summary>
      protected PartialView()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the unique identifier in DOM for this element.
      /// </summary>
      /// <remarks>
      /// This property have a protected <c>setter</c> because every modal view must have a 
      /// constant DOM unique identifier. You can set this property only in a implementations
      /// of the abstract class <see cref="ModalView"/>.
      /// </remarks>
      public string DomID { get; protected set; }

      /// <summary>
      /// Devuelve o establece el contenido de la página.
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
      public string GetInvokeFunctionWithParameters(params object[] parameters)
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
         catch
         {
            return string.Empty;
         }
      }

      /// <summary>
      /// Generate JS call from partial view. 
      /// </summary>
      /// <returns></returns>
      /// <remarks>
      /// Partial view must have initialized properties.
      /// </remarks>
      public string GetInvokeFunction()
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
      public Script GetInvokeScriptWithParameters(Script.ScriptExecutionMethod executionType, params object[] parameters)
      {
         Script script = new SimpleScript(this);
         script.ExecutionType = executionType;
         script.AppendSourceLine(GetInvokeFunctionWithParameters(parameters));

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
         script.AppendSourceLine(GetInvokeFunction());

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

      /// <summary>
      /// Muestra un mensaje de error.
      /// </summary>
      /// <param name="title">Título del error.</param>
      /// <param name="description">Descripción del error.</param>
      public void ShowError(string title, string description)
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
      public void ShowError(Exception exception)
      {
         StringBuilder xhtml = new StringBuilder();

         // Limpia el contenido de la zona principal dónde se va a mostrar el mensaje
         Content.Clear();
         Content.Clear();

         // Agrega el mensaje al contenido
         Content.Add(new ErrorControl(this, exception));
      }

      #endregion

      #region View Implementation

      /// <summary>
      /// Inicia el ciclo de vida de la vista.
      /// </summary>
      internal override void StartViewLifecycle()
      {
         string receivedFormID = string.Empty;
         var watch = Stopwatch.StartNew();

         try
         {
            // Inicialización de la página
            InitPage();

            // Process form data
            foreach (FormControl form in Content.GetControlsByType(typeof(FormControl)))
            {
               if (IsFormReceived && form.DomID.Equals(FormReceivedDomID))
               {
                  form.ProcessForm(Parameters);
                  receivedFormID = form.DomID;

                  // If data is valid, raise FormDataReceived() event
                  if (form.IsValid == FormControl.ValidationStatus.ValidData)
                  {
                     FormDataReceived(form);
                  }
               }

               // Lanza el evento FormDataLoad()
               if (form.IsValid != FormControl.ValidationStatus.InvalidData)
               {
                  FormDataLoad(form.DomID);
               }
            }

            // Finish page load
            LoadPage();
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }

         // Renderiza la página
         Response.ContentType = "text/html";
         Response.Write(Workspace.UIService.Render(Content, receivedFormID));

         watch.Stop();
         Response.Write("<!-- Content created in " + watch.ElapsedMilliseconds + "mS -->");
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.DomID = string.Empty;
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
