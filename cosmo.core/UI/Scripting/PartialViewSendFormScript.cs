using Cosmo.UI.Controls;
using System.Collections.Generic;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Implementa un script para enviar el contenido de un formulario via AJAX a un 
   /// <see cref="Cosmo.REST.RestHandler"/>, <see cref="Cosmo.UI.PageView"/> o 
   /// <see cref="Cosmo.UI.PartialView"/> .
   /// </summary>
   public class ModalViewSendFormScript : Script
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ModalViewSendFormScript"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="form">Una instancia del formulario que se desea enviar via AJAX.</param>
      public ModalViewSendFormScript(ModalView parentView, FormControl form) 
         : base(parentView) 
      {
         Initialize();

         this.Form = form;
         this.ExecutionType = ScriptExecutionMethod.Standalone;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el formulario para el que se desea generar el script.
      /// </summary>
      public FormControl Form { get; set; }

      /// <summary>
      /// Llista de acciones a ejecutar si la llamada tiene éxito (evento <c>success</c> de la llamada AJAX, 
      /// con respuesta de éxito por parte del handler).
      /// </summary>
      public List<Script> OnCallSuccess { get; set; }

      /// <summary>
      /// Llista de acciones a ejecutar si la llamada falla (evento <c>success</c> de la llamada AJAX, 
      /// con respuesta de error por parte del handler).
      /// </summary>
      public List<Script> OnCallFail { get; set; }

      /// <summary>
      /// Llista de acciones a ejecutar si no se puede realizar la llamada (evento <c>error</c> de la llamada AJAX).
      /// </summary>
      public List<Script> OnCallError { get; set; }

      #endregion

      #region IScript Implementation

      /// <summary>
      /// Genera y devuelve una cadena con el código JavaScript a incorporar a la vista.
      /// </summary>
      /// <returns>Una cadena que contiene el código JavasScript solicitado.</returns>
      public override string GetSource()
      {
         // Declara el evento Submit
         Source.AppendLine("$('#" + Form.DomID + "').submit(function(e) {");

         // Recoge los datos del formulario
         if (!Form.IsMultipart)
         {
            Source.AppendLine("  var fData = $(this).serializeArray();");
         }
         else
         {
            Source.AppendLine("  var fData = new FormData(this);");
         }

         Source.AppendLine("  $.ajax({");
         Source.AppendLine("    url: '" + Form.Action + "',");
         Source.AppendLine("    type: 'POST',");
         Source.AppendLine("    data: fData,");
         if (Form.IsMultipart)
         {
            Source.AppendLine("    mimeType: 'multipart/form-data',");
            Source.AppendLine("    cache: false,");
            Source.AppendLine("    processData: false,");
         }
         Source.AppendLine("    success: function(data, textStatus, jqXHR) {");
         Source.AppendLine("      $('#" + ((ModalView)Form.ParentView).DomID + " .modal-dialog').html(data);");
         Source.AppendLine("    },");
         Source.AppendLine("    error: function(jqXHR, textStatus, errorThrown) {");
         Source.AppendLine("      bootbox.alert(\"Se ha producido un error y no ha sido posible enviar los datos al servidor.\");");
         Source.AppendLine("    }");
         Source.AppendLine("  });");
         Source.AppendLine("  e.preventDefault();");
         Source.AppendLine("});");

         return Source.ToString();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Form = null;
      }

      #endregion

   }
}
