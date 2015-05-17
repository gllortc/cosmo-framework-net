using Cosmo.UI.Controls;
using System.Collections.Generic;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Implementa un script para enviar el contenido de un formulario via AJAX a un 
   /// <see cref="Cosmo.REST.RestHandler"/>, <see cref="Cosmo.UI.PageViewContainer"/> o 
   /// <see cref="Cosmo.UI.PartialViewContainer"/> .
   /// </summary>
   public class AjaxSendFormScript : Script
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="AjaxSendFormScript"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="form">Una instancia del formulario que se desea enviar via AJAX.</param>
      public AjaxSendFormScript(ViewContainer container, FormControl form) 
         : base(container) 
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
         Source.AppendLine("      if (data.Result == " + (int)AjaxResponse.JsonResponse.Successful + ") {");
         Source.AppendLine("        $('#" + Form.DomID + "').html(data.Xhtml);");
         Source.AppendLine("      } else {");
         Source.AppendLine("        if (data.Xhtml != '') { $('#" + Form.DomID + "').html(data.Xhtml); }");
         Source.AppendLine("        else { bootbox.alert(\"ERROR: \" + data.ErrorMessage); }");
         Source.AppendLine("      }");
         Source.AppendLine("    },");
         Source.AppendLine("    error: function(jqXHR, textStatus, errorThrown) {");
         Source.AppendLine("      bootbox.alert(\"Se ha producido un error y no ha sido posible enviar los datos al servidor.\");");
         Source.AppendLine("    }");
         Source.AppendLine("  });");
         Source.AppendLine("  e.preventDefault();");
         Source.AppendLine("});");

         /*
         Source.AppendLine("<script type=\"text/javascript\">");
         Source.AppendLine("  $(\"#btn-login\").click(function() {");
         Source.AppendLine("    $(\"#" + loginForm.DomID + "-form\").submit(function(e) {");
         Source.AppendLine("      var postData = $(this).serializeArray();");
         Source.AppendLine("      $.ajax({");
         Source.AppendLine("        url     : \"" + SecurityRestHandler.ServiceUrl + "\",");
         Source.AppendLine("        type    : \"POST\",");
         Source.AppendLine("        data    : postData,");
         Source.AppendLine("        success : function(data, textStatus, jqXHR) {");
         Source.AppendLine("                    if (data.Result == " + (int)AjaxResponse.JsonResponse.Successful + ") {");
         Source.AppendLine("                      $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-success\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-check\"></i>Autenticación correcta</div>');");
         Source.AppendLine("                      window.location = data.ToURL;");
         Source.AppendLine("                    }");
         Source.AppendLine("                    else {");
         Source.AppendLine("                      if (data.ErrorCode == '1001') {");
         Source.AppendLine("                        $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-danger\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-ban\"></i>Esta cuenta actualmente no tiene acceso.</div>');");
         Source.AppendLine("                      } else if (data.ErrorCode == '1002') {");
         Source.AppendLine("                        $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-warning\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-warning\"></i>Esta cuenta está pendiente de verificación y aun no tiene acceso. Revise su correo, debe tener un correo con las instrucciones para verificar esta cuenta.</div>');");
         Source.AppendLine("                      } else {");
         Source.AppendLine("                        $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-danger\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-ban\"></i>El usuario y/o la contraseña son incorrectos.</div>');");
         Source.AppendLine("                      }");
         Source.AppendLine("                    }");
         Source.AppendLine("                  },");
         Source.AppendLine("        error   : function(jqXHR, textStatus, errorThrown) {");
         Source.AppendLine("                     $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-danger\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-ban\"></i><strong>Ooooops!</strong> No se ha podido realizar la autenticación a causa de un error.</div>');");
         Source.AppendLine("                  }");
         Source.AppendLine("      });");
         Source.AppendLine("      e.preventDefault();");
         Source.AppendLine("    });");
         Source.AppendLine("    $(\"#" + loginForm.DomID + "-form\").submit();");
         Source.AppendLine("  });");
         Source.AppendLine("</script>");
         */




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
