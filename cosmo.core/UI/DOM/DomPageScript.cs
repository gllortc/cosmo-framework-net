using System.Text;
using System.Xml;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa un enlace de cabecera de página HTML.
   /// </summary>
   public class DomPageScript
   {

      #region Enumerations

      /// <summary>
      /// Lenguaje del script.
      /// </summary>
      public enum ScriptType
      {
         /// <summary>Código JavaScript.</summary>
         JavaScript,
         /// <summary>Código VBScript (sólo para clientes IE).</summary>
         VBScript,
      }

      #endregion

      ScriptType _type;
      string _charset;
      bool _defer;
      string _url;
      string _source;

      #region Constants

      /// <summary>Clave de plantilla que engloba un enlace a un script.</summary>
      public const string KEY_SCRIPT = "script";
      /// <summary>Nombre del atributo del tag SCRIPT que contiene el tipo de script.</summary>
      public const string ATTR_SCRIPT_TYPE = "type";
      /// <summary>Nombre del atributo del tag SCRIPT que contiene el enlace al código.</summary>
      public const string ATTR_SCRIPT_URL = "src";
      /// <summary>Nombre del atributo del tag SCRIPT que contiene la codificación del código fuente.</summary>
      public const string ATTR_SCRIPT_CHARSET = "charset";
      /// <summary>Nombre del atributo del tag SCRIPT que indica que debe ejecutarse cuando se termine el renderizado de la página.</summary>
      public const string ATTR_SCRIPT_DEFER = "defer";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomPageScript"/>.
      /// </summary>
      public DomPageScript() 
      {
         Clear();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomPageScript"/>.
      /// </summary>
      /// <remarks>
      /// Este constructor habilita un javaScript estándar.
      /// </remarks>
      public DomPageScript(string url) 
      {
         Clear();

         // Inicializaciones
         _url = url;
      }

      #region Properties

      /// <summary>
      /// Specifies the MIME type of a script.
      /// </summary>
      public ScriptType Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Specifies the character encoding used in an external script file.
      /// </summary>
      public string Charset
      {
         get { return _charset; }
         set { _charset = value.Trim(); }
      }

      /// <summary>
      /// Specifies that the execution of a script should be deferred (delayed) until after the page has been loaded.
      /// </summary>
      /// <remarks>
      /// This property (<c>defer</c>) is only supported by Internet Explorer.
      /// </remarks>
      public bool ExecutionDelay
      {
         get { return _defer; }
         set { _defer = value; }
      }

      /// <summary>
      /// Specifies the URL of an external script file.
      /// </summary>
      public string Url
      {
         get { return _url; }
         set { _url = value.Trim(); }
      }

      /// <summary>
      /// Specifies the source code to execute.
      /// </summary>
      public string Source
      {
         get { return _source; }
         set { _source = value.Trim(); }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Convierte este elemento a código XHTML para su inclusión en una página HTML.
      /// </summary>
      public string Render()
      {
         StringBuilder xhtml = new StringBuilder();

         // Genera el include
         if (!string.IsNullOrEmpty(_url))
         {
            xhtml.AppendFormat("<script{0}{1}{2}{3}></script>\n",
                               GetScriptType(),
                               !string.IsNullOrEmpty(_url) ? " src=\"" + _url + "\"" : string.Empty,
                               !string.IsNullOrEmpty(_charset) ? " charset=\"" + _charset + "\"" : string.Empty,
                               _defer ? " defer=\"defer\"" : string.Empty);
         }

         // Genera el código fuente
         if (!string.IsNullOrEmpty(_source))
         {
            xhtml.AppendFormat("<script{0}{1}>\n{2}</script>\n",
                               GetScriptType(),
                               !string.IsNullOrEmpty(_charset) ? " charset=\"" + _charset + "\"" : string.Empty,
                               _source);
         }

         return xhtml.ToString();
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Obtiene el tipo de script a partir de su tipo MIME.
      /// </summary>
      /// <param name="type">Cadena que contiene el tipo MIME.</param>
      public static ScriptType GetType(string type)
      {
         if (type.ToLower().Contains("vbscript") || type.ToLower().Contains("basic"))
            return ScriptType.VBScript;

         if (type.ToLower().Contains("javascript") || type.ToLower().Contains("jscript"))
            return ScriptType.JavaScript;

         return ScriptType.JavaScript;
      }

      /// <summary>
      /// Obtiene la definición de un script a partir de un nodo XML.
      /// </summary>
      public static DomPageScript ReadFromXml(XmlNode node)
      {
         DomPageScript script = new DomPageScript();
         script.Source = (node.Value != null ? node.Value.Trim() : string.Empty);
         foreach (XmlAttribute scriptattr in node.Attributes)
         {
            switch (scriptattr.Name.ToLower())
            {
               case DomPageScript.ATTR_SCRIPT_TYPE: script.Type = DomPageScript.GetType(scriptattr.Value); break;
               case DomPageScript.ATTR_SCRIPT_URL: script.Url = scriptattr.Value; break;
               case DomPageScript.ATTR_SCRIPT_CHARSET: script.Charset = scriptattr.Value; break;
               case DomPageScript.ATTR_SCRIPT_DEFER: script.ExecutionDelay = true; break;
            }
         }

         return script;
      }

      #endregion

      #region Private Members

      private void Clear()
      {
         _type = ScriptType.JavaScript;
         _charset = string.Empty;
         _defer = false;
         _url = string.Empty;
         _source = string.Empty;
      }

      private string GetScriptType()
      {
         switch (_type)
         {
            case ScriptType.JavaScript: return " type=\"text/javascript\"";
            case ScriptType.VBScript: return " type=\"text/vbscript\"";
         }

         return string.Empty;
      }

      #endregion

   }
}
