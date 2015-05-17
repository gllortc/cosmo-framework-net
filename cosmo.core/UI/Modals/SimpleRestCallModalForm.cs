using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using System.Collections.Generic;

namespace Cosmo.UI.Modals
{
   /// <summary>
   /// Implementa un formulario modal cuyos datos se mandan mediante una llamada AJAX a la
   /// API REST de Cosmo. La llamada debe ser simple, es decir, devolverá un token JSON
   /// indicando el resultado satisfactorio o erróneo de la llamada.
   /// </summary>
   public class SimpleRestCallModalForm : IModalForm
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="SimpleRestCallModalForm"/>.
      /// </summary>
      /// <param name="container">Instancia de <see cref="ViewContainer"/> que contiene el control.</param>
      /// <param name="id">Identificador único (DOM) del formulario modal.</param>
      public SimpleRestCallModalForm(ViewContainer parentViewport, string domId)
         : base(parentViewport, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el nombre del script a invocar.
      /// </summary>
      public string RestURL { get; set; }

      /// <summary>
      /// Devuelve o establece el nombre del script a invocar.
      /// </summary>
      public List<Script> OnSuccess { get; set; }

      /// <summary>
      /// Devuelve o establece el identificador único (DOM) del formulario que permite recoger los 
      /// datos a enviar via AJAX.
      /// </summary>
      public string FormDomId { get; set; }

      #endregion

      #region IModalContainer Implementation

      /// <summary>
      /// Procesa el formulario y prepara todos los componentes para el renderizado. 
      /// </summary>
      public override void PreRenderForm()
      {
         string param = string.Empty;
         string scriptInvokeName = "invoke" + DomID;

         // Agrega los botones de envio del formulario
         ButtonControl cmdSend = new ButtonControl(Container, "cmdSend" + DomID, "Aceptar", ButtonControl.ButtonTypes.Normal);
         cmdSend.Color = ComponentColorScheme.Primary;
         cmdSend.JavaScriptAction = scriptInvokeName + "();";
         Form.FormButtons.Add(cmdSend);

         ButtonControl cmdCancel = new ButtonControl(Container, "cmdCancel" + DomID, "Cancelar", ButtonControl.ButtonTypes.CloseModalForm);
         Form.FormButtons.Add(cmdCancel);

         // Genera el JavaScript necesario para gestionar el formulario
         foreach (FormField field in Form.Content.GetByControlType(typeof(FormField)))
         {
            if (!string.IsNullOrWhiteSpace(param)) param += ", ";
            param += field.DomID + ": " + Script.GetFieldValue(Form.DomID, field);
         }
         param = "{ " + param + " }";

         SimpleScript script = new SimpleScript(Container);
         script.AppendSourceLine("function " + scriptInvokeName + "() {");
         script.AppendSourceLine("  $('#cmdSend" + DomID + "').prop('disabled', true);");
         script.AppendSourceLine("  $('#cmdCancel" + DomID + "').prop('disabled', true);");
         script.AppendSourceLine("  var formData = " + param + ";");
         script.AppendSourceLine("  $.ajax({");
         script.AppendSourceLine("    url: '" + RestURL + "',");
         script.AppendSourceLine("    type: 'POST',");
         script.AppendSourceLine("    data: formData,");
         script.AppendSourceLine("    cache: false,");
         script.AppendSourceLine("    success: function(data, textStatus, jqXHR) {");
         script.AppendSourceLine("      if (data.Result == '" + (int)AjaxResponse.JsonResponse.Successful + "') {");

         foreach (Script behavior in OnSuccess)
         {
            script.AppendSourceLine(behavior.GetSource());
         }
         
         script.AppendSourceLine("      } else {");
         script.AppendSourceLine("        bootbox.alert('ERROR: ' + data.ErrorMessage);");
         script.AppendSourceLine("      }");
         script.AppendSourceLine("    },");
         script.AppendSourceLine("    error: function(jqXHR, textStatus, errorThrown) {");
         script.AppendSourceLine("      bootbox.alert('Ha fallado la llamada al servidor y no ha sido posible completar la acción solicitada.');");
         script.AppendSourceLine("    },");
         script.AppendSourceLine("    complete: function(jqXHR, textStatus) {");
         script.AppendSourceLine("      $('#cmdSend" + DomID + "').prop('disabled', false);");
         script.AppendSourceLine("      $('#cmdCancel" + DomID + "').prop('disabled', false);");
         script.AppendSourceLine("    }");
         script.AppendSourceLine("  });");
         script.AppendSourceLine("  return false;");
         script.AppendSourceLine("}");

         Scripts.Add(script);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         RestURL = string.Empty;
         FormDomId = string.Empty;

         if (OnSuccess == null)
         {
            OnSuccess = new List<Script>();
         }
      }

      #endregion

   }
}
