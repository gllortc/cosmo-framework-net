using Cosmo.Net.REST;
using System.Collections.Specialized;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Implementa un script que permite realizar una llamada a un método simple de la API REST
   /// de Cosmo y que devuelve un token JSON. La llamada se realiza siempre por POST.
   /// </summary>
   public class AjaxRestSimpleCallScript : Script
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="AjaxRestSimpleCallScript"/>.
      /// </summary>
      /// <param name="viewport">Página o contenedor dónde se representará el control.</param>
      public AjaxRestSimpleCallScript(View viewport)
         : base(viewport)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el nombre (identificador) de la función.
      /// </summary>
      public string FunctionName { get; set; }

      /// <summary>
      /// Gets or sets el nombre del script a invocar.
      /// </summary>
      public string URL { get; set; }

      /// <summary>
      /// Colección de parámetros que se pasarán en la llamada al método REST.
      /// </summary>
      /// <remarks>
      /// Dado que el valor de cada parámetro puede ser una variable, una constante o cualquier
      /// llamada a otra función, se debe tener cuidado en como se especifican, sobretodo, las
      /// cadenas (el valor se debe especificar entre comillas: "string").
      /// </remarks>
      public NameValueCollection Parameters { get; set; }

      #endregion

      #region IScript Implementation

      /// <summary>
      /// Genera el código fuente (JavaScript).
      /// </summary>
      /// <returns>Una cadena de texto que contiene el código JavaScript generado.</returns>
      public override string GetSource()
      {
         string param = string.Empty;

         foreach (string key in this.Parameters)
         {
            if (!string.IsNullOrWhiteSpace(param)) param += ", ";
            param += key + ": " + this.Parameters[key];
         }
         param = "{ " + param + " }";

         Source.Clear();
         Source.AppendLine("function " + this.FunctionName + "() {");
         Source.AppendLine("  $.ajax({");
         Source.AppendLine("    url: '" + this.URL + "',");
         Source.AppendLine("    type: 'POST',");
         Source.AppendLine("    data: " + param + ",");
         Source.AppendLine("    cache: false,");
         Source.AppendLine("    success: function(data, textStatus, jqXHR) {");
         Source.AppendLine("      if (data.Result == '" + (int)AjaxResponse.JsonResponse.Successful + "') {");
         Source.AppendLine("        ");
         Source.AppendLine("      } else {");
         Source.AppendLine("        bootbox.alert(data.ErrorMessage);");
         Source.AppendLine("      }");
         Source.AppendLine("    },");
         Source.AppendLine("    error: function(jqXHR, textStatus, errorThrown) {");
         Source.AppendLine("      bootbox.alert('Ha fallado la llamada al servidor y no ha sido posible completar la acción solicitada.');");
         Source.AppendLine("    }");
         Source.AppendLine("  });");
         Source.AppendLine("  return false;");
         Source.AppendLine("}");

         return Source.ToString();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.FunctionName = string.Empty;
         this.URL = string.Empty;
         this.Parameters = new NameValueCollection();
      }

      #endregion

   }
}
