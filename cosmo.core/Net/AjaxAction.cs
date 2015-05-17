using System.Text;
using System.Web.Script.Serialization;

namespace Cosmo.Net
{
   public class AjaxAction
   {
      private string _name;
      private Url _url;

      /// <summary>
      /// Devuelve una instancia de <see cref="AjaxAction"/>.
      /// </summary>
      public AjaxAction(string name, string url)
      {
         Initialize();

         _name = name;
         _url = new Url(url);
      }

      public string GetJavascriptCall()
      {
         return string.Empty;
      }

      public string GetJavascript()
      {
         return string.Empty;
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _name = string.Empty;
         _url = null;
      }

      /// <summary>
      /// Genera el código JavaScript.
      /// </summary>
      private string GenerateJavascript()
      {
         StringBuilder js = new StringBuilder();
         JavaScriptSerializer json = new JavaScriptSerializer();

         js.AppendLine("function exportList() {");
         // js.AppendLine("  var profileToUse = document.getElementById('dropdown').value");
         js.AppendLine("  jQuery.ajax({");
         js.AppendLine("    url: \"" + _url + "\",");
         js.AppendLine("    type: \"POST\",");
         js.AppendLine("    data: {profile: profileToUse}");
         js.AppendLine("  });");
         js.AppendLine("}");

         return js.ToString();
      }
   }
}
