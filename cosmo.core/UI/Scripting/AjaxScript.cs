using Cosmo.Net;
using System.Text;

namespace Cosmo.UI.Scripting
{
   public class AjaxScript : IScript
   {
      // Declaración de variables internas
      private string _id;
      private string _divRefresh;
      private Url _url;

      /// <summary>
      /// Devuelve una instancia de <see cref="AjaxScript"/>.
      /// </summary>
      public AjaxScript(ICosmoViewport viewport)
         : base(viewport)
      {
         Initialize();
      }

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
      public string UpdateDiv
      {
         get { return _divRefresh; }
         set { _divRefresh = value; }
      }

      /// <summary>
      /// Devuelve o establece la URL y los parámetros de la llamada a realizar para la acción AJAX.
      /// </summary>
      public Url Url
      {
         get { return _url; }
         set { _url = value; }
      }

      /// <summary>
      /// Genera el código JavaScript.
      /// </summary>
      public override string GetSource()
      {
         if (_url == null)
         {
            return string.Empty;
         }

         StringBuilder js = new StringBuilder();

         // Genera los parámetros
         js.AppendLine("var " + _id + "params = {");
         bool first = true;
         foreach (string key in _url.Parameters.AllKeys)
         {
            if (!first) js.AppendLine("  ,");
            js.AppendLine("  \"" + key + "\" : \"" + _url.Parameters.Get(key) + "\"");
            first = false;
         }
         js.AppendLine("};");

         js.AppendLine("$.ajax({");
         js.AppendLine("  url: '" + _url.Filename + "',");
         js.AppendLine("  data: " + _id + "params,");
         js.AppendLine("  type: \"post\",");
         js.AppendLine("  success: function(response) {");
         js.AppendLine("  },");
         js.AppendLine("  error: function(jqXHR, textStatus, errorThrown) {");
         js.AppendLine("  }");
         js.AppendLine("});");

         return js.ToString();
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _id = string.Empty;
         _divRefresh = string.Empty;
         _url = null;
      }
   }
}
