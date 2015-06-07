using Cosmo.UI.Controls;
using System.Collections.Generic;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Implementation of script that open a Modal View.
   /// </summary>
   public class ModalViewOpenScript : Script
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ModalViewSendFormScript"/>.
      /// </summary>
      /// <param name="modalView">Una instancia del formulario que se desea enviar via AJAX.</param>
      public ModalViewOpenScript(ModalView modalView)
         : base(modalView) 
      {
         Initialize();

         this.ExecutionType = ScriptExecutionMethod.Standalone;
      }

      #endregion

      #region Properties

      public ModalView ModalView 
      {
         get { return (ModalView)this.ParentView; } 
      }

      #endregion

      #region IScript Implementation

      /// <summary>
      /// Genera y devuelve una cadena con el código JavaScript a incorporar a la vista.
      /// </summary>
      /// <returns>Una cadena que contiene el código JavasScript solicitado.</returns>
      public override string GetSource()
      {
         bool first = true;
         int paramCount = 0;
         string funcParams = string.Empty;
         string callParams = string.Empty;

         foreach (ViewParameter param in ModalView.GetType().GetCustomAttributes(typeof(ViewParameter), false))
         {
            // Generate function parameters
            if (!first) funcParams += ",";
            funcParams += param.ParameterName;

            // Generate AJAX call parameters
            if (!first) callParams += ",";
            callParams += param.ParameterName + ":arguments[" + paramCount + "]";

            first = false;
            paramCount++;
         }

         Source.AppendLine("function open" + Script.ConvertToFunctionName(this.ModalView.DomID) + "(" + funcParams + ") {");
         Source.AppendLine("  $('#" + ModalView.DomID + " .modal-dialog').html('<br/><br/><br/><br/><br/><br/><div class=\"overlay\"></div><div class=\"loading-img\"></div>');");
         Source.AppendLine("  $('#" + ModalView.DomID + "').modal('show');");
         Source.AppendLine("  $.ajax({");
         Source.AppendLine("    url: '" + ModalView.GetType().Name + "',");
         Source.AppendLine("    data: {" + callParams + "},");
         Source.AppendLine("    type: \"POST\",");
         Source.AppendLine("    success: function(data, textStatus, jqXHR) {");
         Source.AppendLine("      $('#" + ModalView.DomID + " .modal-dialog').html(data);");
         Source.AppendLine("    },");
         Source.AppendLine("    error: function(jqXHR, textStatus, errorThrown) {");
         Source.AppendLine("      bootbox.alert(\"Se ha producido un error y no ha sido posible enviar los datos al servidor.\");");
         Source.AppendLine("      ");
         Source.AppendLine("    }");
         Source.AppendLine("  });");
         Source.AppendLine("}");

         return Source.ToString();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         // this.ModalView = null;
      }

      #endregion

   }
}
