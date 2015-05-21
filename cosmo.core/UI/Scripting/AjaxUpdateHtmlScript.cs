using Cosmo.Net;
using Cosmo.UI.Controls;
using System.Text;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Implementa un script que permite actualizar un elemento del DOM con el contenido devuelto por una llamada AJAX.
   /// </summary>
   public class AjaxUpdateHtmlScript : Script
   {
      // Declaración de variables internas
      private string _id;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="AjaxUpdateHtmlScript"/>.
      /// </summary>
      /// <param name="viewport">Página o contenedor dónde se representará el control.</param>
      public AjaxUpdateHtmlScript(ViewContainer viewport)
         : base(viewport)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece una cadena única para cada script.
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
      /// Devuelve o establece el contenedor DIV que se actualizará cuando el tipo de script sea <c>UpdateHtml</c>.
      /// </summary>
      public string UpdateDiv { get; set; }

      /// <summary>
      /// Devuelve o establece el nombre de la función que encapsula el script.
      /// </summary>
      public string FunctionName { get; set; }


      /// <summary>
      /// Devuelve o establece la URL y los parámetros de la llamada a realizar para la acción AJAX.
      /// </summary>
      public Url Url { get; set; }

      #endregion

      #region IScript Implementation

      /// <summary>
      /// Genera el código JavaScript.
      /// </summary>
      public override string GetSource()
      {
         if (Url == null)
         {
            return string.Empty;
         }

         // Genera el mensaje de error por si se produce un error AJAX
         CalloutControl callout = new CalloutControl(this.Container);
         callout.Title = "Se ha producido un error";
         callout.Text = "Se ha producido un error al ejecutar la acción y no se ha completado.";
         callout.Icon = IconControl.ICON_WARNING;

         StringBuilder js = new StringBuilder();

         if (!string.IsNullOrWhiteSpace(FunctionName) && this.ExecutionType == ScriptExecutionMethod.OnFunctionCall)
         {
            js.AppendLine("function " + FunctionName + "() {");
         }

         // Genera los parámetros
         js.AppendLine("var " + _id + "params = {");
         bool first = true;
         foreach (string key in Url.Parameters.AllKeys)
         {
            if (!first) js.AppendLine("  ,");
            js.AppendLine("  \"" + key + "\" : \"" + Url.Parameters.Get(key) + "\"");
            first = false;
         }
         js.AppendLine("};");

         js.AppendLine("var " + _id + "err = '" + this.Container.Workspace.UIService.Render(callout).Replace("'", "\\'") + "'");

         js.AppendLine("$.ajax({");
         js.AppendLine("  url: '" + Url.Filename + "',");
         js.AppendLine("  data: " + _id + "params,");
         js.AppendLine("  type: \"post\",");
         js.AppendLine("  success: function(response) {");
         js.AppendLine("    $('#" + UpdateDiv + "').html(response.Xhtml);");
         js.AppendLine("  },");
         js.AppendLine("  error: function(jqXHR, textStatus, errorThrown) {");
         js.AppendLine("    $('#" + UpdateDiv + "').html(" + _id + "err);");
         js.AppendLine("    ");
         js.AppendLine("  }");
         js.AppendLine("});");

         if (!string.IsNullOrWhiteSpace(FunctionName))
         {
            js.AppendLine("}");
         }

         return js.ToString();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
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
