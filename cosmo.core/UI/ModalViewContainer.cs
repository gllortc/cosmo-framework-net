using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using System;
using System.Diagnostics;
using System.Text;

namespace Cosmo.UI
{
   /// <summary>
   /// Implementa una vista que permite generarse sin estar dentro de una página.
   /// </summary>
   public abstract class ModalViewContainer : ViewContainer
   {

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="ModalViewContainer"/>.
      /// </summary>
      protected ModalViewContainer()
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
      /// of the abstract class <see cref="ModalViewContainer"/>.
      /// </remarks>
      public string DomID { get; protected set; }

      /// <summary>
      /// Gets or sets the modal title.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or sets the modal title icon.
      /// </summary>
      /// <remarks>
      /// This property must be set with an icon code. You have all codes in <see cref="IconControl"/>
      /// as a <c>ICON_xxxx</c> constants
      /// </remarks>
      public string Icon { get; set; }

      /// <summary>
      /// Devuelve o establece el contenido de la página.
      /// </summary>
      public ControlCollection Content { get; set; }
      
      /// <summary>
      /// Indica si la vista modal debe contener un botón que permita cerrarla.
      /// </summary>
      public bool Closeable { get; set; }

      #endregion

      #region Methods

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

      /// <summary>
      /// Generate JS call from modal. 
      /// </summary>
      /// <returns></returns>
      /// <remarks>
      /// Modal must have initialized properties.
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

            return "open" + Script.ConvertToFunctionName(this.DomID) + "(" + js + ");";
         }
         catch
         {
            return string.Empty;
         }
      }

      /// <summary>
      /// Generates the script that allow load and show modal view.
      /// </summary>
      /// <returns>A <see cref="Script"/> instance containing the requestes script.</returns>
      public Script GetOpenModalScript()
      {
         return new ModalViewOpenScript(this);
      }

      #endregion

      #region #ViewContainer Implementation

      /// <summary>
      /// Inicia el ciclo de vida de la vista.
      /// </summary>
      internal override void StartViewLifecycle()
      {
         bool canLoadData = true;
         string receivedFormID = string.Empty;
         var watch = Stopwatch.StartNew();

         try
         {
            // Inicialización de la página
            InitPage();

            // Comprueba si la llamada corresponde a un envio de datos desde un formulario
            if (IsFormReceived)
            {
               FormControl recvForm = GetProcessedForm();
               if (recvForm != null)
               {
                  receivedFormID = recvForm.DomID;
                  FormDataReceived(recvForm);
               }
               else
               {
                  canLoadData = false;
               }
            }

            // Carga la página
            if (canLoadData)
            {
               LoadPage();
            }
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }

         // Renderiza la página
         Response.ContentType = "text/html";
         Response.Write(Workspace.UIService.RenderPage(this, receivedFormID));

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
         this.Closeable = false;
         this.Content = new ControlCollection();
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

      #region Disabled Code

      /*
      /// <summary>
      /// Generates the script that allow send form without reloading the entire page.
      /// </summary>
      /// <returns>A <see cref="Script"/> instance containing the requestes script.</returns>
      public Script GetSendFormScript(FormControl form)
      {
         SimpleScript js = new SimpleScript(this);
         js.ExecutionType = Script.ScriptExecutionMethod.Standalone;

         // Declara el evento Submit
         js.AppendSourceLine("$('#" + form.DomID + "').submit(function(e) {");

         // Recoge los datos del formulario
         if (!form.IsMultipart)
         {
            js.AppendSourceLine("  var fData = $(this).serializeArray();");
         }
         else
         {
            js.AppendSourceLine("  var fData = new FormData(this);");
         }

         js.AppendSourceLine("  $.ajax({");
         js.AppendSourceLine("    url: '" + form.Action + "',");
         js.AppendSourceLine("    type: 'POST',");
         js.AppendSourceLine("    data: fData,");
         if (form.IsMultipart)
         {
            js.AppendSourceLine("    mimeType: 'multipart/form-data',");
            js.AppendSourceLine("    cache: false,");
            js.AppendSourceLine("    processData: false,");
         }
         js.AppendSourceLine("    success: function(data, textStatus, jqXHR) {");
         js.AppendSourceLine("      $('#" + form.DomID + " .modal-dialog').html(data);");
         js.AppendSourceLine("    },");
         js.AppendSourceLine("    error: function(jqXHR, textStatus, errorThrown) {");
         js.AppendSourceLine("      bootbox.alert(\"Se ha producido un error y no ha sido posible enviar los datos al servidor.\");");
         js.AppendSourceLine("    }");
         js.AppendSourceLine("  });");
         js.AppendSourceLine("  e.preventDefault();");
         js.AppendSourceLine("});");

         return js;
      }
      */

      #endregion

   }
}
