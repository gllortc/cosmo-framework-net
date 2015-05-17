using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa una cabecera de página.
   /// </summary>
   public class DomPageHeader
   {
      string _title;
      string _description;
      string _keywords;
      string _copyright;
      string _publisher;
      string _author;
      string _charset;
      string _generator;
      string _language;
      List<DomPageLink> _links;
      List<DomPageMeta> _meta;
      List<DomPageScript> _scripts;
      DomPage _page;

      #region Constants

      /// <summary>Codificación UTF-8.</summary>
      public static readonly string CHARSET_UTF8 = "utf-8";
      /// <summary>Codificación ISO-8859-1.</summary>
      public static readonly string CHARSET_ISO8859_1 = "iso-8859-1";

      /// <summary>Idioma de Abkhazian.</summary>
      public static readonly string LANGUAGE_ABKHAZIAN = "ab";
      /// <summary>Idioma de Afar.</summary>
      public static readonly string LANGUAGE_AFAR = "aa";
      /// <summary>Idioma Afrikaans.</summary>
      public static readonly string LANGUAGE_AFRIKAANS = "af";
      /// <summary>Idioma Albanés.</summary>
      public static readonly string LANGUAGE_ALBANIAN = "sq";
      /// <summary>Idioma Amharic.</summary>
      public static readonly string LANGUAGE_AMHARIC = "am";
      /// <summary>Idioma Árabe.</summary>
      public static readonly string LANGUAGE_ARABIC = "ar";
      /// <summary>Idioma Armenio.</summary>
      public static readonly string LANGUAGE_ARMENIAN = "hy";
      /// <summary>Idioma Assamese.</summary>
      public static readonly string LANGUAGE_ASSAMESE = "as";
      /// <summary>Idioma Aymara.</summary>
      public static readonly string LANGUAGE_AYMARA = "ay";
      /// <summary>Idioma Azerbaijano.</summary>
      public static readonly string LANGUAGE_AZERBAIJANI = "az";
      /// <summary>Idioma Bashkir.</summary>
      public static readonly string LANGUAGE_BASHKIR = "ba";
      /// <summary>Idioma Vasco.</summary>
      public static readonly string LANGUAGE_BASQUE = "eu";
      /// <summary>Idioma Bengali.</summary>
      public static readonly string LANGUAGE_BENGALI = "bn";
      /// <summary>Idioma Bhutani.</summary>
      public static readonly string LANGUAGE_BHUTANI = "dz";
      /// <summary>Idioma Bihari.</summary>
      public static readonly string LANGUAGE_BIHARI = "bh";
      /// <summary>Idioma Bislama.</summary>
      public static readonly string LANGUAGE_BISLAMA = "bi";
      /// <summary>Idioma Bretón.</summary>
      public static readonly string LANGUAGE_BRETON = "br";
      /// <summary>Idioma Búlgaro.</summary>
      public static readonly string LANGUAGE_BULGARIAN = "bg";
      /// <summary>Idioma Burmese.</summary>
      public static readonly string LANGUAGE_BURMESE = "my";
      /// <summary>Idioma Bieloruso.</summary>
      public static readonly string LANGUAGE_BELARUSIAN = "be";
      /// <summary>Idioma Cambodian.</summary>
      public static readonly string LANGUAGE_CAMBODIAN = "km";
      /// <summary>Idioma Català.</summary>
      public static readonly string LANGUAGE_CATALAN = "ca";
      /// <summary>Idioma Chino simplificado.</summary>
      public static readonly string LANGUAGE_CHINESE_SIMPLIFIED = "zh";
      /// <summary>Idioma Chino tradicional.</summary>
      public static readonly string LANGUAGE_CHINESE_TRADITIONAL = "zh";
      /// <summary>Idioma Corso.</summary>
      public static readonly string LANGUAGE_CORSICAN = "co";
      /// <summary>Idioma Croata.</summary>
      public static readonly string LANGUAGE_CROATIAN = "hr";
      /// <summary>Idioma Checo.</summary>
      public static readonly string LANGUAGE_CZECH = "cs";
      /// <summary>Idioma Danés.</summary>
      public static readonly string LANGUAGE_DANISH = "da";
      /// <summary>Idioma Holandés.</summary>
      public static readonly string LANGUAGE_DUTCH = "nl";
      /// <summary>Idioma Inglés.</summary>
      public static readonly string LANGUAGE_ENGLISH = "en";
      /// <summary>Idioma Esperanto.</summary>
      public static readonly string LANGUAGE_ESPERANTO = "eo";
      /// <summary>Idioma Estónio.</summary>
      public static readonly string LANGUAGE_ESTONIAN = "et";
      /// <summary>Idioma Faeroese.</summary>
      public static readonly string LANGUAGE_FAEROESE = "fo";
      /// <summary>Idioma Farsi.</summary>
      public static readonly string LANGUAGE_FARSI = "fa";
      /// <summary>Idioma Fiji.</summary>
      public static readonly string LANGUAGE_FIJI = "fj";
      /// <summary>Idioma Finlandés.</summary>
      public static readonly string LANGUAGE_FINNISH = "fi";
      /// <summary>Idioma Francés.</summary>
      public static readonly string LANGUAGE_FRENCH = "fr";
      /// <summary>Idioma Frisiano.</summary>
      public static readonly string LANGUAGE_FRISIAN = "fy";
      /// <summary>Idioma Gallego.</summary>
      public static readonly string LANGUAGE_GALICIAN = "gl";
      /// <summary>Idioma Gaélico.</summary>
      public static readonly string LANGUAGE_GAELIC = "gd";
      /// <summary>Idioma Georgiano.</summary>
      public static readonly string LANGUAGE_GEORGIAN = "ka";
      /// <summary>Idioma Alemán.</summary>
      public static readonly string LANGUAGE_GERMAN = "de";
      /// <summary>Idioma Griego.</summary>
      public static readonly string LANGUAGE_GREEK = "el";
      /// <summary>Idioma Groenlándico.</summary>
      public static readonly string LANGUAGE_GREENLANDIC = "kl";
      /// <summary>Idioma Guaraní.</summary>
      public static readonly string LANGUAGE_GUARANI = "gn";
      /// <summary>Idioma Gujarati.</summary>
      public static readonly string LANGUAGE_GUJARATI = "gu";
      /// <summary>Idioma Hausa.</summary>
      public static readonly string LANGUAGE_HAUSA = "ha";
      /// <summary>Idioma Hebreo.</summary>
      public static readonly string LANGUAGE_HEBREW = "he";
      /// <summary>Idioma Hindi.</summary>
      public static readonly string LANGUAGE_HINDI = "hi";
      /// <summary>Idioma Húngaro.</summary>
      public static readonly string LANGUAGE_HUNGARIAN = "hu";
      /// <summary>Idioma Islándico.</summary>
      public static readonly string LANGUAGE_ICELANDIC = "is";
      /// <summary>Idioma Indonesio.</summary>
      public static readonly string LANGUAGE_INDONESIAN = "id";
      /// <summary>Idioma Inuktitut.</summary>
      public static readonly string LANGUAGE_INUKTITUT = "iu";
      /// <summary>Idioma Inupiak.</summary>
      public static readonly string LANGUAGE_INUPIAK = "ik";
      /// <summary>Idioma Irlandés.</summary>
      public static readonly string LANGUAGE_IRISH = "ga";
      /// <summary>Idioma Italiano.</summary>
      public static readonly string LANGUAGE_ITALIAN = "it";
      /// <summary>Idioma Japonés.</summary>
      public static readonly string LANGUAGE_JAPANESE = "ja";
      /// <summary>Idioma Javanese.</summary>
      public static readonly string LANGUAGE_JAVANESE = "jv";
      /// <summary>Idioma Kannada.</summary>
      public static readonly string LANGUAGE_KANNADA = "kn";
      /// <summary>Idioma Kashmiri.</summary>
      public static readonly string LANGUAGE_KASHMIRI = "ks";
      /// <summary>Idioma Kazakh.</summary>
      public static readonly string LANGUAGE_KAZAKH = "kk";
      /// <summary>Idioma Kinyarwanda.</summary>
      public static readonly string LANGUAGE_KINYARWANDA = "rw";
      /// <summary>Idioma Kirghiz.</summary>
      public static readonly string LANGUAGE_KIRGHIZ = "ky";
      /// <summary>Idioma Kirundi.</summary>
      public static readonly string LANGUAGE_KIRUNDI = "rn";
      /// <summary>Idioma Coreano.</summary>
      public static readonly string LANGUAGE_KOREAN = "ko";
      /// <summary>Idioma Kurdish.</summary>
      public static readonly string LANGUAGE_KURDISH = "ku";
      /// <summary>Idioma Laothian.</summary>
      public static readonly string LANGUAGE_LAOTHIAN = "lo";
      /// <summary>Idioma Latín.</summary>
      public static readonly string LANGUAGE_LATIN = "la";
      /// <summary>Idioma Latvian.</summary>
      public static readonly string LANGUAGE_LATVIAN = "lv";
      /// <summary>Idioma Limburgish.</summary>
      public static readonly string LANGUAGE_LIMBURGISH = "li";
      /// <summary>Idioma Lingala.</summary>
      public static readonly string LANGUAGE_LINGALA = "ln";
      /// <summary>Idioma Lituanés.</summary>
      public static readonly string LANGUAGE_LITHUANIAN = "lt";
      /// <summary>Idioma Macedónio.</summary>
      public static readonly string LANGUAGE_MACEDONIAN = "mk";
      /// <summary>Idioma Malagasy.</summary>
      public static readonly string LANGUAGE_MALAGASY = "mg";
      /// <summary>Idioma Malay.</summary>
      public static readonly string LANGUAGE_MALAY = "ms";
      /// <summary>Idioma Malayalam.</summary>
      public static readonly string LANGUAGE_MALAYALAM = "ml";
      /// <summary>Idioma Maltese.</summary>
      public static readonly string LANGUAGE_MALTESE = "mt";
      /// <summary>Idioma Maorí.</summary>
      public static readonly string LANGUAGE_MAORI = "mi";
      /// <summary>Idioma Marathi.</summary>
      public static readonly string LANGUAGE_MARATHI = "mr";
      /// <summary>Idioma Moldavian.</summary>
      public static readonly string LANGUAGE_MOLDAVIAN = "mo";
      /// <summary>Idioma Mongolio.</summary>
      public static readonly string LANGUAGE_MONGOLIAN = "mn";
      /// <summary>Idioma Nauru.</summary>
      public static readonly string LANGUAGE_NAURU = "na";
      /// <summary>Idioma Nepalí.</summary>
      public static readonly string LANGUAGE_NEPALI = "ne";
      /// <summary>Idioma Noruego.</summary>
      public static readonly string LANGUAGE_NORWEGIAN = "no";
      /// <summary>Idioma Occitano.</summary>
      public static readonly string LANGUAGE_OCCITAN = "oc";
      /// <summary>Idioma Oriya.</summary>
      public static readonly string LANGUAGE_ORIYA = "or";
      /// <summary>Idioma Pashto.</summary>
      public static readonly string LANGUAGE_PASHTO = "ps";
      /// <summary>Idioma Polaco.</summary>
      public static readonly string LANGUAGE_POLISH = "pl";
      /// <summary>Idioma Portugués.</summary>
      public static readonly string LANGUAGE_PORTUGUESE = "pt";
      /// <summary>Idioma Punjabi.</summary>
      public static readonly string LANGUAGE_PUNJABI = "pa";
      /// <summary>Idioma Quechua.</summary>
      public static readonly string LANGUAGE_QUECHUA = "qu";
      /// <summary>Idioma Rhaeto.</summary>
      public static readonly string LANGUAGE_RHAETO_ROMANCE = "rm";
      /// <summary>Idioma Rumano.</summary>
      public static readonly string LANGUAGE_ROMANIAN = "ro";
      /// <summary>Idioma Ruso.</summary>
      public static readonly string LANGUAGE_RUSSIAN = "ru";
      /// <summary>Idioma Samoan.</summary>
      public static readonly string LANGUAGE_SAMOAN = "sm";
      /// <summary>Idioma Sangro.</summary>
      public static readonly string LANGUAGE_SANGRO = "sg";
      /// <summary>Idioma Sanskrit.</summary>
      public static readonly string LANGUAGE_SANSKRIT = "sa";
      /// <summary>Idioma Serbio.</summary>
      public static readonly string LANGUAGE_SERBIAN = "sr";
      /// <summary>Idioma Serbo-croata.</summary>
      public static readonly string LANGUAGE_SERBO_CROATIAN = "sh";
      /// <summary>Idioma Sesotho.</summary>
      public static readonly string LANGUAGE_SESOTHO = "st";
      /// <summary>Idioma Setswana.</summary>
      public static readonly string LANGUAGE_SETSWANA = "tn";
      /// <summary>Idioma Shona.</summary>
      public static readonly string LANGUAGE_SHONA = "sn";
      /// <summary>Idioma Sindhi.</summary>
      public static readonly string LANGUAGE_SINDHI = "sd";
      /// <summary>Idioma Sinhalese.</summary>
      public static readonly string LANGUAGE_SINHALESE = "si";
      /// <summary>Idioma Siswati.</summary>
      public static readonly string LANGUAGE_SISWATI = "ss";
      /// <summary>Idioma Eslovaco.</summary>
      public static readonly string LANGUAGE_SLOVAK = "sk";
      /// <summary>Idioma Esloveno.</summary>
      public static readonly string LANGUAGE_SLOVENIAN = "sl";
      /// <summary>Idioma Somalí.</summary>
      public static readonly string LANGUAGE_SOMALI = "so";
      /// <summary>Idioma Castellano.</summary>
      public static readonly string LANGUAGE_SPANISH = "es";
      /// <summary>Idioma Sudanés.</summary>
      public static readonly string LANGUAGE_SUNDANESE = "su";
      /// <summary>Idioma Swahili.</summary>
      public static readonly string LANGUAGE_SWAHILI = "sw";
      /// <summary>Idioma Sueco.</summary>
      public static readonly string LANGUAGE_SWEDISH = "sv";
      /// <summary>Idioma Tagalog.</summary>
      public static readonly string LANGUAGE_TAGALOG = "tl";
      /// <summary>Idioma Tajik.</summary>
      public static readonly string LANGUAGE_TAJIK = "tg";
      /// <summary>Idioma Tamil.</summary>
      public static readonly string LANGUAGE_TAMIL = "ta";
      /// <summary>Idioma Tatar.</summary>
      public static readonly string LANGUAGE_TATAR = "tt";
      /// <summary>Idioma Telugu.</summary>
      public static readonly string LANGUAGE_TELUGU = "te";
      /// <summary>Idioma Tailandés.</summary>
      public static readonly string LANGUAGE_THAI = "th";
      /// <summary>Idioma Tibetano.</summary>
      public static readonly string LANGUAGE_TIBETAN = "bo";
      /// <summary>Idioma Tigrinya.</summary>
      public static readonly string LANGUAGE_TIGRINYA = "ti";
      /// <summary>Idioma Tonga.</summary>
      public static readonly string LANGUAGE_TONGA = "to";
      /// <summary>Idioma Tsonga.</summary>
      public static readonly string LANGUAGE_TSONGA = "ts";
      /// <summary>Idioma Turco.</summary>
      public static readonly string LANGUAGE_TURKISH = "tr";
      /// <summary>Idioma Turkmen.</summary>
      public static readonly string LANGUAGE_TURKMEN = "tk";
      /// <summary>Idioma Twi.</summary>
      public static readonly string LANGUAGE_TWI = "tw";
      /// <summary>Idioma Uighur.</summary>
      public static readonly string LANGUAGE_UIGHUR = "ug";
      /// <summary>Idioma Ucraniano.</summary>
      public static readonly string LANGUAGE_UKRAINIAN = "uk";
      /// <summary>Idioma Urdu.</summary>
      public static readonly string LANGUAGE_URDU = "ur";
      /// <summary>Idioma Uzbek.</summary>
      public static readonly string LANGUAGE_UZBEK = "uz";
      /// <summary>Idioma Vietnamita.</summary>
      public static readonly string LANGUAGE_VIETNAMESE = "vi";
      /// <summary>Idioma Volapuk.</summary>
      public static readonly string LANGUAGE_VOLAPUK = "vo";
      /// <summary>Idioma Welsh.</summary>
      public static readonly string LANGUAGE_WELSH = "cy";
      /// <summary>Idioma Wolof.</summary>
      public static readonly string LANGUAGE_WOLOF = "wo";
      /// <summary>Idioma Xhosa.</summary>
      public static readonly string LANGUAGE_XHOSA = "xh";
      /// <summary>Idioma Yiddish.</summary>
      public static readonly string LANGUAGE_YIDDISH = "yi";
      /// <summary>Idioma Yoruba.</summary>
      public static readonly string LANGUAGE_YORUBA = "yo";
      /// <summary>Idioma Zulú.</summary>
      public static readonly string LANGUAGE_ZULU = "zu";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomPageHeader"/>.
      /// </summary>
      public DomPageHeader(DomPage page)
      {
         Clear();

         _page = page;
      }

      #region Properties

      /// <summary>
      /// Devuelve la instancia de <see cref="DomPage"/> a la cual pertenece la cabecera.
      /// </summary>
      public DomPage OwnerPage
      {
         get { return _page; }
      }

      /// <summary>
      /// Devuelve o establece el título de la página.
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece la descripción del contenido de la página.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece la lista de palabras clave que describen el contenido de la página.
      /// </summary>
      public string Keywords
      {
         get { return _keywords; }
         set { _keywords = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece la información de copyright del contenido de la página.
      /// </summary>
      public string Copyright
      {
         get { return _copyright; }
         set { _copyright = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece el editor del contenido de la página.
      /// </summary>
      public string Publisher
      {
         get { return _publisher; }
         set { _publisher = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece el autor del contenido de la página.
      /// </summary>
      public string Author
      {
         get { return _author; }
         set { _author = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece la descripción del software usado para generar la página.
      /// </summary>
      public string Generator
      {
         get { return (!string.IsNullOrEmpty(_generator) ? _generator : "MetaObjects (Cosmo Framework " + Workspace.Version + ")"); }
         set { _generator = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece el código de idioma del contenido de la página.
      /// </summary>
      public string Language
      {
         get { return (!string.IsNullOrEmpty(_language) ? _language : DomPageHeader.LANGUAGE_ENGLISH); }
         set { _language = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece la codificación usada en la página.
      /// </summary>
      public string Charset
      {
         get { return (!string.IsNullOrEmpty(_charset) ? _charset : DomPageHeader.CHARSET_UTF8); }
         set { _charset = value.Trim(); }
      }

      /// <summary>
      /// Contiene todos los scripts de la página y referencias a scripts externos.
      /// </summary>
      public List<DomPageScript> Scripts
      {
         get { return _scripts; }
         set { _scripts = value; }
      }

      /// <summary>
      /// Contiene todos los enlaces a recursos de la página (css, rss, etc).
      /// </summary>
      public List<DomPageLink> Links
      {
         get { return _links; }
         set { _links = value; }
      }

      /// <summary>
      /// Contiene toda la metainformación de la página.
      /// </summary>
      public List<DomPageMeta> Meta
      {
         get { return _meta; }
         set { _meta = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      public void Clear()
      {
         // Si no hay plantilla, se genera un error
         _publisher = string.Empty;
         _copyright = string.Empty;
         _author = string.Empty;
         _charset = string.Empty;
         _language = string.Empty;
         _generator = string.Empty;
         _title = string.Empty;
         _description = string.Empty;
         _keywords = string.Empty;

         _meta = new List<DomPageMeta>();
         _links = new List<DomPageLink>();
         _scripts = new List<DomPageScript>();
      }

      /// <summary>
      /// Renderiza la cabecera de la página y produce el código XHTML a enviar al cliente.
      /// </summary>
      public string Render()
      {
         StringBuilder sb = new StringBuilder(string.Empty);

         sb.AppendFormat("<?xml version=\"1.0\" encoding=\"{0}\"?>\n", this.Charset);
         sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
         sb.AppendFormat("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"{0}\" lang=\"{1}\">\n", this.Language, this.Language);
         sb.AppendLine("<head>");

         // Agrega el título de la página
         sb.AppendFormat("<title>{0}</title>\n", this.Title);
         if (!string.IsNullOrEmpty(this.Description)) sb.AppendFormat("<meta http-equiv=\"description\" content=\"{0}\" />\n", HttpUtility.HtmlEncode(this.Description));
         if (!string.IsNullOrEmpty(this.Keywords)) sb.AppendFormat("<meta http-equiv=\"keywords\" content=\"{0}\" />\n", this.Keywords);

         // Agrega los META-TAGs
         sb.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />\n", this.Charset);
         sb.AppendLine("<meta http-equiv=\"cache-control\" content=\"no-cache\" />");
         sb.AppendLine("<meta http-equiv=\"expires\" content=\"3600\" />");
         sb.AppendLine("<meta http-equiv=\"revisit-after\" content=\"2 days\" />");
         sb.AppendLine("<meta http-equiv=\"robots\" content=\"index,follow\" />");
         if (!string.IsNullOrEmpty(this.Publisher)) sb.AppendFormat("<meta http-equiv=\"publisher\" content=\"{0}\" />\n", this.Publisher);
         if (!string.IsNullOrEmpty(_copyright)) sb.AppendFormat("<meta http-equiv=\"copyright\" content=\"{0}\" />\n", _copyright);
         if (!string.IsNullOrEmpty(_author)) sb.AppendFormat("<meta http-equiv=\"author\" content=\"{0}\" />\n", _author);
         sb.AppendFormat("<meta http-equiv=\"generator\" content=\"{0}\" />\n", _generator);
         sb.AppendLine("<meta http-equiv=\"distribution\" content=\"global\" />");

         // Agrega enlaces a recursos externos
         sb.AppendFormat("<link href=\"templates/{0}/template.css\" rel=\"stylesheet\" type=\"text/css\" />\n", _page.Template.ID.ToString());
         foreach (DomPageLink link in _links)
            sb.Append(link.Render());

         // Agrega los enlaces a RSS
         sb.AppendLine("<link href=\"rss.do?ch=1\" rel=\"alternate\" type=\"application/rss+xml\" title=\"Railwaymania.com - Documentos\" />");
         sb.AppendLine("<link href=\"rss.do?ch=5\" rel=\"alternate\" type=\"application/rss+xml\" title=\"Railwaymania.com - Anuncios clasificados\" />");
         sb.AppendLine("<link href=\"rss.do?ch=2\" rel=\"alternate\" type=\"application/rss+xml\" title=\"Railwaymania.com - Imágenes\" />");
         sb.AppendLine("<link href=\"rss.do?ch=4\" rel=\"alternate\" type=\"application/rss+xml\" title=\"Railwaymania.com - Foros de discusión\" />");
         sb.AppendLine("<link href=\"rss.do?ch=3\" rel=\"alternate\" type=\"application/rss+xml\" title=\"Railwaymania.com - Biblioteca\" />");

         // Agrega scripts JS
         sb.AppendLine("<script type=\"text/javascript\" src=\"http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js\"></script>");
         foreach (DomPageScript script in _scripts)
            sb.Append(script.Render());

         sb.AppendLine("</head>");
         sb.AppendLine("<body>");
         sb.AppendLine(DomContentComponentBase.GetTag(DomPage.TAG_BODY));
         sb.AppendLine("</body>");
         sb.AppendLine("</html>");

         return sb.ToString();
      }

      #endregion

   }

}
