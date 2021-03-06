﻿using Cosmo.Net;
using Cosmo.UI.Controls;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Implementa un script que permite actualizar un elemento del DOM con el contenido devuelto por una llamada AJAX.
   /// </summary>
   public class AjaxUpdateHtmlScript : Script
   {
      // Internal data declarations
      private string _id;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="AjaxUpdateHtmlScript"/>.
      /// </summary>
      /// <param name="viewport">Parent <see cref="View"/> which acts as a container of the control.</param>
      public AjaxUpdateHtmlScript(View viewport)
         : base(viewport)
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
      /// Gets or sets el contenedor DIV que se actualizará cuando el tipo de script sea <c>UpdateHtml</c>.
      /// </summary>
      public string UpdateDiv { get; set; }

      /// <summary>
      /// Gets or sets el nombre de la función que encapsula el script.
      /// </summary>
      public string FunctionName { get; set; }


      /// <summary>
      /// Gets or sets la URL y los parámetros de la llamada a realizar para la acción AJAX.
      /// </summary>
      public Url Url { get; set; }

      #endregion

      #region IScript Implementation

      public override void BuildSource()
      {
         if (Url == null)
         {
            Source.Clear();
            return;
         }

         // Genera el mensaje de error por si se produce un error AJAX
         CalloutControl callout = new CalloutControl(this.ParentView);
         callout.Title = "Se ha producido un error";
         callout.Text = "Se ha producido un error al ejecutar la acción y no se ha completado.";
         callout.Icon = IconControl.ICON_WARNING;

         if (!string.IsNullOrWhiteSpace(FunctionName) && this.ExecutionType == ScriptExecutionMethod.OnFunctionCall)
         {
            Source.AppendLine("function " + FunctionName + "() {");
         }

         // Genera los parámetros
         Source.AppendLine("var " + _id + "params = {");
         bool first = true;
         foreach (string key in Url.Parameters.AllKeys)
         {
            if (!first) Source.AppendLine("  ,");
            Source.AppendLine("  \"" + key + "\" : \"" + Url.Parameters.Get(key) + "\"");
            first = false;
         }
         Source.AppendLine("};");

         Source.AppendLine("var " + _id + "err = '" + this.ParentView.Workspace.UIService.Render(callout).Replace("'", "\\'") + "'");

         Source.AppendLine("$.ajax({");
         Source.AppendLine("  url: '" + Url.Filename + "',");
         Source.AppendLine("  data: " + _id + "params,");
         Source.AppendLine("  type: \"post\",");
         Source.AppendLine("  success: function(response) {");
         Source.AppendLine("    $('#" + UpdateDiv + "').html(response.Xhtml);");
         Source.AppendLine("  },");
         Source.AppendLine("  error: function(jqXHR, textStatus, errorThrown) {");
         Source.AppendLine("    $('#" + UpdateDiv + "').html(" + _id + "err);");
         Source.AppendLine("    ");
         Source.AppendLine("  }");
         Source.AppendLine("});");

         if (!string.IsNullOrWhiteSpace(FunctionName))
         {
            Source.AppendLine("}");
         }
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
      //   js.AppendLine("    $('#" + UpdateDiv + "').html(response.Xhtml);");
      //   js.AppendLine("  },");
      //   js.AppendLine("  error: function(jqXHR, textStatus, errorThrown) {");
      //   js.AppendLine("    $('#" + UpdateDiv + "').html(" + _id + "err);");
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

         this.UpdateDiv = string.Empty;
         this.Url = null;
         this.FunctionName = string.Empty;
      }

      #endregion

   }
}
