namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa una metainformación de página HTML.
   /// </summary>
   public class DomPageMeta
   {

      #region Enumerations

      /// <summary>
      /// Define los tipos de ralaciones de recursos externos con una pa´gina.
      /// </summary>
      public enum HtmlContentDirection
      {
         /// <summary>Right to Left.</summary>
         RTL,
         /// <summary>Left to Right.</summary>
         LTR,
         /// <summary>Sin definir.</summary>
         Unknown
      }

      #endregion

      string _content;
      string _httpeq;
      string _name;
      string _scheme;
      HtmlContentDirection _dir;
      string _lang;
      string _xmllang;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomPageMeta"/>.
      /// </summary>
      public DomPageMeta()
      {
         Clear();
      }

      #region Properties

      /// <summary>
      /// Specifies the content of the meta information.
      /// </summary>
      public string Content
      {
         get { return _content; }
         set { _content = value; }
      }

      /// <summary>
      /// Provides an HTTP header for the information in the content attribute.
      /// </summary>
      public string HttpEquiv
      {
         get { return _httpeq; }
         set { _httpeq = value; }
      }

      /// <summary>
      /// Provides a name for the information in the content attribute.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Specifies a scheme to be used to interpret the value of the content attribute.
      /// </summary>
      public string Media
      {
         get { return _scheme; }
         set { _scheme = value; }
      }

      /// <summary>
      /// Specifies the text direction for the content in an element.
      /// </summary>
      public HtmlContentDirection Dir
      {
         get { return _dir; }
         set { _dir = value; }
      }

      /// <summary>
      /// Specifies a language code for the content in an element.
      /// </summary>
      public string Lang
      {
         get { return _lang; }
         set { _lang = value; }
      }

      /// <summary>
      /// Specifies a language code for the content in an element, in XHTML documents.
      /// </summary>
      public string XmlLang
      {
         get { return _xmllang; }
         set { _xmllang = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Convierte este elemento a código XHTML para su inclusión en una página HTML.
      /// </summary>
      public string Render()
      {
         return "<meta" + (!string.IsNullOrEmpty(_content) ? " content=\"" + _content + "\"" : "") +
                          (!string.IsNullOrEmpty(_httpeq) ? " http-equiv=\"" + _httpeq + "\"" : "") +
                          (!string.IsNullOrEmpty(_name) ? " name=\"" + _name + "\"" : "") +
                          (!string.IsNullOrEmpty(_scheme) ? " scheme=\"" + _scheme + "\"" : "") +
                          (_dir != HtmlContentDirection.Unknown ? " dir=\"" + _dir.ToString().ToLower() + "\"" : "") +
                          (!string.IsNullOrEmpty(_lang) ? " lang=\"" + _lang + "\"" : "") +
                          (!string.IsNullOrEmpty(_xmllang) ? " xml:lang=\"" + _xmllang + "\"" : "") + " />\n";
      }

      #endregion

      #region Private Members

      private void Clear()
      {
         _content = string.Empty;
         _httpeq = string.Empty;
         _name = string.Empty;
         _scheme = string.Empty;
         _dir = HtmlContentDirection.Unknown;
         _lang = string.Empty;
         _xmllang = string.Empty;
      }

      #endregion

   }
}
