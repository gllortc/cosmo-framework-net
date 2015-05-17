namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa un enlace de cabecera de página HTML.
   /// </summary>
   public class DomPageLink
   {

      #region Enumerations

      /// <summary>
      /// Define los tipos de ralaciones de recursos externos con una pa´gina.
      /// </summary>
      public enum LinkRelation
      {
         /// <summary>Alternate.</summary>
         Alternate,
         /// <summary>Appendix.</summary>
         Appendix,
         /// <summary>Bookmark.</summary>
         Bookmark,
         /// <summary>Chapter.</summary>
         Chapter,
         /// <summary>Contents.</summary>
         Contents,
         /// <summary>Copyright.</summary>
         Copyright,
         /// <summary>Glossary.</summary>
         Glossary,
         /// <summary>Help.</summary>
         Help,
         /// <summary>Home.</summary>
         Home,
         /// <summary>Index.</summary>
         Index,
         /// <summary>Next.</summary>
         Next,
         /// <summary>Prev.</summary>
         Prev,
         /// <summary>Section.</summary>
         Section,
         /// <summary>Start.</summary>
         Start,
         /// <summary>Stylesheet.</summary>
         Stylesheet,
         /// <summary>Subsection.</summary>
         Subsection,
         /// <summary>No definido.</summary>
         Unknown
      }

      /// <summary>
      /// Define los tipos de medio en que se carga un recurso externo a una página.
      /// </summary>
      public enum LinkMedia
      {
         /// <summary>Pantalla.</summary>
         screen,
         /// <summary>TTY.</summary>
         tty,
         /// <summary>Televisión.</summary>
         tv,
         /// <summary>Proyector, pantalla de gran formato.</summary>
         projection,
         /// <summary>PDA/Smartphone.</summary>
         handheld,
         /// <summary>Impresión.</summary>
         print,
         /// <summary>Medios para discapacitados visuales (Braille).</summary>
         braille,
         /// <summary>Medios aurales.</summary>
         aural,
         /// <summary>Todos los medios.</summary>
         all,
         /// <summary>Desconocido.</summary>
         Unknown
      }

      #endregion

      string _charset;
      string _href;
      string _hreflang;
      LinkMedia _media;
      LinkRelation _rel;
      LinkRelation _rev;
      string _target;
      string _type;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomPageLink"/>.
      /// </summary>
      public DomPageLink()
      {
         Clear();
      }

      #region Properties

      /// <summary>
      /// Specifies the character encoding of the linked document.
      /// </summary>
      public string Charset
      {
         get { return _charset; }
         set { _charset = value; }
      }

      /// <summary>
      /// Specifies the location of the linked document.
      /// </summary>
      public string Href
      {
         get { return _href; }
         set { _href = value; }
      }

      /// <summary>
      /// Specifies the language of the text in the linked document.
      /// </summary>
      public string HrefLang
      {
         get { return _hreflang; }
         set { _hreflang = value; }
      }

      /// <summary>
      /// Specifies on what device the linked document will be displayed.
      /// </summary>
      public LinkMedia Media
      {
         get { return _media; }
         set { _media = value; }
      }

      /// <summary>
      /// Specifies the relationship between the current document and the linked document.
      /// </summary>
      public LinkRelation Rel
      {
         get { return _rel; }
         set { _rel = value; }
      }

      /// <summary>
      /// Specifies the relationship between the linked document and the current document.
      /// </summary>
      public LinkRelation Rev
      {
         get { return _rev; }
         set { _rev = value; }
      }

      /// <summary>
      /// Specifies where the linked document is to be loaded.
      /// </summary>
      public string Target
      {
         get { return _target; }
         set { _target = value; }
      }

      /// <summary>
      /// Specifies the MIME type of the linked document.
      /// </summary>
      public string Type
      {
         get { return _type; }
         set { _type = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Convierte este elemento a código XHTML para su inclusión en una página HTML.
      /// </summary>
      public string Render()
      {
         return "<link" + (!string.IsNullOrEmpty(_href) ? " href=\"" + _href + "\"" : "") +
                          (!string.IsNullOrEmpty(_hreflang) ? " hreflang=\"" + _hreflang + "\"" : "") +
                          (_rel != LinkRelation.Unknown ? " rel=\"" + _rel.ToString().ToLower() + "\"" : "") +
                          (_rev != LinkRelation.Unknown ? " rel=\"" + _rev.ToString().ToLower() + "\"" : "") +
                          (_media != LinkMedia.Unknown ? " media=\"" + _media.ToString().ToLower() + "\"" : "") +
                          (!string.IsNullOrEmpty(_target) ? " target=\"" + _target + "\"" : "") +
                          (!string.IsNullOrEmpty(_type) ? " type=\"" + _type + "\"" : "") + " />\n";
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve una instancia de <see cref="DomPageLink"/> que representa un enlace a un archivo CSS.
      /// </summary>
      /// <param name="url">URL (absoluta o relativa) al archivo CSS.</param>
      /// <returns>Una instancia de <see cref="DomPageLink"/> que representa el enlace al archivo CSS.</returns>
      public static DomPageLink GetCSS(string url)
      {
         DomPageLink link = new DomPageLink();
         link.Type = "text/css";
         link.Rel = LinkRelation.Stylesheet;
         link.Href = url.Trim();

         return link;
      }

      #endregion

      #region Private Members

      private void Clear()
      {
         _charset = string.Empty;
         _href = string.Empty;
         _hreflang = string.Empty;
         _media = LinkMedia.Unknown;
         _rel = LinkRelation.Unknown;
         _rev = LinkRelation.Unknown;
         _target = string.Empty;
         _type = string.Empty;
      }

      #endregion

   }
}
