using Cosmo.UI.Controls;
using System.Collections.Generic;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Generates a script that sends form data via AJAX.
   /// </summary>
   public class AjaxSendFormScript : Script
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="AjaxSendFormScript"/>.
      /// </summary>
      /// <param name="parentView">View where the script resides.</param>
      /// <param name="form">Form to send.</param>
      public AjaxSendFormScript(View parentView, FormControl form)
         : base(parentView)
      {
         Initialize();

         this.FormDomID = form.DomID;
         this.FormAction = form.Action;
         this.FormIsMultipart = form.IsMultipart;
      }

      /// <summary>
      /// Gets a new instance of <see cref="AjaxSendFormScript"/>.
      /// </summary>
      /// <param name="parentView">View where the script resides.</param>
      /// <param name="formDomID">DOM ID of the form to send.</param>
      /// <param name="formAction">Action of the form to send.</param>
      /// <param name="formMultipart">Multipart indication of the form to send.</param>
      public AjaxSendFormScript(View parentView, string formDomID, string formAction, bool formMultipart) 
         : base(parentView) 
      {
         Initialize();

         this.FormDomID = formDomID;
         this.FormAction = formAction;
         this.FormIsMultipart = formMultipart;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the form DOM unique ID.
      /// </summary>
      public string FormDomID { get; set; }

      /// <summary>
      /// Gets or sets the form action (URL).
      /// </summary>
      public string FormAction { get; set; }

      /// <summary>
      /// Gets or sets a boolean value indicating if the form is multipart.
      /// </summary>
      public bool FormIsMultipart { get; set; }

      #endregion

      #region IScript Implementation

      public override void BuildSource()
      {
         // Declara el evento Submit
         Source.AppendLine("$('#" + this.FormDomID + "').submit(function(e) {");

         // Recoge los datos del formulario
         if (!this.FormIsMultipart)
         {
            Source.AppendLine("  var fData = $(this).serializeArray();");
         }
         else
         {
            Source.AppendLine("  var fData = new FormData(this);");
         }

         Source.AppendLine("  $.ajax({");
         Source.AppendLine("    url: '" + this.FormAction + "',");
         Source.AppendLine("    type: 'POST',");
         Source.AppendLine("    data: fData,");
         if (this.FormIsMultipart)
         {
            Source.AppendLine("    mimeType: 'multipart/form-data',");
            Source.AppendLine("    cache: false,");
            Source.AppendLine("    processData: false,");
         }
         Source.AppendLine("    success: function(data, textStatus, jqXHR) {");
         Source.AppendLine("      $('#" + this.ParentView.DomID + "').html(data);");
         Source.AppendLine("    },");
         Source.AppendLine("    error: function(jqXHR, textStatus, errorThrown) {");
         Source.AppendLine("      bootbox.alert(\"Se ha producido un error y no ha sido posible enviar los datos al servidor.\");");
         Source.AppendLine("    }");
         Source.AppendLine("  });");
         Source.AppendLine("  e.preventDefault();");
         Source.AppendLine("});");
      }

      ///// <summary>
      ///// Genera y devuelve una cadena con el código JavaScript a incorporar a la vista.
      ///// </summary>
      ///// <returns>Una cadena que contiene el código JavasScript solicitado.</returns>
      //public override string GetSource()
      //{
      //   // Declara el evento Submit
      //   Source.AppendLine("$('#" + this.FormDomID + "').submit(function(e) {");

      //   // Recoge los datos del formulario
      //   if (!this.FormIsMultipart)
      //   {
      //      Source.AppendLine("  var fData = $(this).serializeArray();");
      //   }
      //   else
      //   {
      //      Source.AppendLine("  var fData = new FormData(this);");
      //   }

      //   Source.AppendLine("  $.ajax({");
      //   Source.AppendLine("    url: '" + this.FormAction + "',");
      //   Source.AppendLine("    type: 'POST',");
      //   Source.AppendLine("    data: fData,");
      //   if (this.FormIsMultipart)
      //   {
      //      Source.AppendLine("    mimeType: 'multipart/form-data',");
      //      Source.AppendLine("    cache: false,");
      //      Source.AppendLine("    processData: false,");
      //   }
      //   Source.AppendLine("    success: function(data, textStatus, jqXHR) {");
      //   Source.AppendLine("      $('#" + this.ParentView.DomID + "').html(data);");
      //   Source.AppendLine("    },");
      //   Source.AppendLine("    error: function(jqXHR, textStatus, errorThrown) {");
      //   Source.AppendLine("      bootbox.alert(\"Se ha producido un error y no ha sido posible enviar los datos al servidor.\");");
      //   Source.AppendLine("    }");
      //   Source.AppendLine("  });");
      //   Source.AppendLine("  e.preventDefault();");
      //   Source.AppendLine("});");

      //   return Source.ToString();
      //}

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.FormDomID = string.Empty;
         this.FormAction = string.Empty;
         this.FormIsMultipart = false;

         this.ExecutionType = ScriptExecutionMethod.Standalone;
      }

      #endregion

   }
}
