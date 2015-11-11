using Cosmo.Net;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Implementa un script que permite actualizar un elemento del DOM con el contenido devuelto por una llamada AJAX.
   /// </summary>
   public class AjaxUpdateListScript : Script
   {
      // Internal data declarations
      private string _id;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="AjaxUpdateListScript"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public AjaxUpdateListScript(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets una cadena única para cada script.
      /// </summary>
      /// <remarks>
      /// Est acadena permite crear variables y funciones con un nombre distinto para cada 
      /// script y evitar duplicidad.
      /// </remarks>
      public string Id 
      {
         get { return _id; }
         set { _id = value.Trim().ToLower(); }
      }

      /// <summary>
      /// Gets or sets the control name (DOM ID) that must be updated with the contents of the list.
      /// </summary>
      public string ListControlName { get; set; }

      /// <summary>
      /// Gets or sets the URL that provide the list items.
      /// </summary>
      public Url Url { get; set; }

      /// <summary>
      /// Gets or sets the URL that provide the list items.
      /// </summary>
      public List<KeyValue> ListItems { get; set; }

      /// <summary>
      /// Gets or sets the URL that provide the list items.
      /// </summary>
      public string DefaultValue { get; set; }

      #endregion

      #region IScript Implementation

      public override void BuildSource()
      {
         if (Url == null)
         {
            Source.Clear();
            return;
         }

         // Genera los parámetros
         Source.Append("var " + _id + "params = { ");
         bool first = true;
         foreach (string key in Url.Parameters.AllKeys)
         {
            if (!first) Source.Append(" ,");
            Source.Append("  \"" + key + "\" : \"" + Url.Parameters.Get(key) + "\"");
            first = false;
         }
         Source.AppendLine(" };");

         Source.AppendLine("$.ajax({");
         Source.AppendLine("  url: '" + Url.Filename + "',");
         Source.AppendLine("  data: " + _id + "params,");
         Source.AppendLine("  type: \"post\",");
         Source.AppendLine("  success: function(response) {");
         Source.AppendLine("    var options = $('#" + this.ListControlName + "');");

         foreach (KeyValue value in ListItems)
         {
            Source.AppendLine("      options.append($('<option />')." +
                                            "val('" + value.Value + "')." +
                                            "text('" + value.Label + "'));");
         }

         Source.AppendLine("    $.each(response.Data, function() {");
         Source.AppendLine("      options.append($('<option />').val(this).text(this));");
         Source.AppendLine("    });");

         if (!string.IsNullOrWhiteSpace(this.DefaultValue))
         {
            Source.AppendLine("    $('select[name^=\"" + this.ListControlName + "\"] option[value=\"" + this.DefaultValue + "\"]').attr('selected','selected');");
         }
         else
         {
            Source.AppendLine("    $('select[name^=\"" + this.ListControlName + "\"] option:selected').attr('selected',null);");
         }

         Source.AppendLine("  },");
         Source.AppendLine("  error: function(jqXHR, textStatus, errorThrown) {");
         Source.AppendLine("      bootbox.alert(\"Se ha producido un error y no ha sido posible enviar los datos al servidor.\");");
         Source.AppendLine("    ");
         Source.AppendLine("  }");
         Source.AppendLine("});");
      }

      ///// <summary>
      ///// Genera el código JavaScript.
      ///// </summary>
      //public override string GetSource()
      //{
      //   if (Url == null)
      //   {
      //      return string.Empty;
      //   }

      //   // Genera el mensaje de error por si se produce un error AJAX
      //   CalloutControl callout = new CalloutControl(this.ParentView);
      //   callout.Title = "Se ha producido un error";
      //   callout.Text = "Se ha producido un error al ejecutar la acción y no se ha completado.";
      //   callout.Icon = IconControl.ICON_WARNING;

      //   StringBuilder js = new StringBuilder();

      //   if (!string.IsNullOrWhiteSpace(FunctionName) && this.ExecutionType == ScriptExecutionMethod.OnFunctionCall)
      //   {
      //      js.AppendLine("function " + FunctionName + "() {");
      //   }

      //   // Genera los parámetros
      //   js.AppendLine("var " + _id + "params = {");
      //   bool first = true;
      //   foreach (string key in Url.Parameters.AllKeys)
      //   {
      //      if (!first) js.AppendLine("  ,");
      //      js.AppendLine("  \"" + key + "\" : \"" + Url.Parameters.Get(key) + "\"");
      //      first = false;
      //   }
      //   js.AppendLine("};");

      //   js.AppendLine("var " + _id + "err = '" + this.ParentView.Workspace.UIService.Render(callout).Replace("'", "\\'") + "'");

      //   js.AppendLine("$.ajax({");
      //   js.AppendLine("  url: '" + Url.Filename + "',");
      //   js.AppendLine("  data: " + _id + "params,");
      //   js.AppendLine("  type: \"post\",");
      //   js.AppendLine("  success: function(response) {");
      //   js.AppendLine("    var options = $('#" + this.ListControlName + "');");
      //   js.AppendLine("    $.each(response, function() {");
      //   js.AppendLine("      options.append($('<option />').val(this.ImageFolderID).text(this.Name));");
      //   js.AppendLine("    }");
      //   js.AppendLine("  },");
      //   js.AppendLine("  error: function(jqXHR, textStatus, errorThrown) {");
      //   js.AppendLine("      bootbox.alert(\"Se ha producido un error y no ha sido posible enviar los datos al servidor.\");");
      //   js.AppendLine("    ");
      //   js.AppendLine("  }");
      //   js.AppendLine("});");

      //   if (!string.IsNullOrWhiteSpace(FunctionName))
      //   {
      //      js.AppendLine("}");
      //   }

      //   return js.ToString();
      //}

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _id = string.Empty;

         this.ListControlName = string.Empty;
         this.Url = null;
         this.FunctionName = string.Empty;
         this.DefaultValue = null;
         this.ListItems = new List<KeyValue>();
      }

      #endregion

   }
}
