using Cosmo.Utils.Html.Parsers;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa un fragmento de código XHTML (libre).
   /// </summary>
   public class DomXhtml : DomContentComponentBase
   {
      private string _xhtml = string.Empty;

      /// <summary>Identificador del bloque XHTML generado.</summary>
      public const string TAG_HTML_ID = "oid";
      /// <summary>Tag: ID de plantilla</summary>
      public const string TAG_TEMPLATE_ID = "tid";

      /// <summary>
      /// Devuelve una instancia de <see cref="DomXhtml"/>.
      /// </summary>
      public DomXhtml() : base() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomXhtml"/>.
      /// </summary>
      public DomXhtml(string xhtml) : base() 
      {
         _xhtml = xhtml;
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "xhtml"; }
      }

      /// <summary>
      /// Devuelve o establece el código XHTML.
      /// </summary>
      public string Xhtml
      {
         get { return _xhtml; }
         set { _xhtml = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el elemento.</returns>
      public override string Render(DomTemplate template, DomPage.ContentContainer container)
      {
         return DomContentComponentBase.ReplaceTag(Xhtml, DomXhtml.TAG_TEMPLATE_ID, template.ID.ToString());
      }

      /// <summary>
      /// Convierte un texto con códigos BBCode a un texto XHTML.
      /// </summary>
      /// <param name="text">Texto original con BBCode.</param>
      public void FromBBCode(string text)
      {
         FromBBCode(text, string.Empty, string.Empty);
      }

      /// <summary>
      /// Convierte un texto con códigos BBCode a un texto XHTML.
      /// </summary>
      /// <param name="text">Texto original con BBCode.</param>
      /// <param name="enclosingTag">TAG XHTML que encerrará el texto formateado (usualmente <p></p>).</param>
      public void FromBBCode(string text, string enclosingTag)
      {
         FromBBCode(text, enclosingTag, string.Empty);
      }

      /// <summary>
      /// Convierte un texto con códigos BBCode a un texto XHTML.
      /// </summary>
      /// <param name="text">Texto original con BBCode.</param>
      /// <param name="enclosingTag">TAG XHTML que encerrará el texto formateado (usualmente <p></p>).</param>
      /// <param name="cssClass">Clase CSS a aplicar al TAG que engloba el texto formateado.</param>
      public void FromBBCode(string text, string enclosingTag, string cssClass)
      {
         _xhtml = string.Format("{0}{1}{2}", (!string.IsNullOrEmpty(enclosingTag) ? string.Format("{0}{1}{2}{3}", "<", enclosingTag.ToLower().Trim(), (!string.IsNullOrEmpty(cssClass) ? string.Format("{0}{1}{2}", " class=\"", cssClass, "\"") : ""), ">\n") : ""), 
                                             BBCodeParserOld.Parse(text),
                                             (!string.IsNullOrEmpty(enclosingTag) ? string.Format("{0}{1}{2}", "</", enclosingTag.ToLower().Trim(), ">\n") : ""));
      }

      #endregion

   }
}
