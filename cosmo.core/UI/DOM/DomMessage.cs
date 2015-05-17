using System.Web;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa una caja de mensaje.
   /// </summary>
   public class DomMessage : DomContentComponentBase
   {

      #region Enumerations

      /// <summary>
      /// Tipos de mensaje admitidos.
      /// </summary>
      public enum MessageBoxTypes
      {
         /// <summary>Mensaje informativo</summary>
         Information,
         /// <summary>Mensaje de advertencia</summary>
         Warning,
         /// <summary>Mensaje de error</summary>
         Error,
         /// <summary>Ninguna indicación del tipo de mensaje</summary>
         None
      }

      #endregion

      private string _title;
      private string _message;
      private MessageBoxTypes _type;

      #region Constants

      /// <summary>Estructura del mensaje informativo.</summary>
      internal const string SECTION_MSG_INFO = "info-box-info";
      /// <summary>Estructura del mensaje de advertencia.</summary>
      internal const string SECTION_MSG_WARNING = "info-box-warning";
      /// <summary>Estructura del mensaje de error.</summary>
      internal const string SECTION_MSG_ERROR = "info-box-error";

      /// <summary>Identificador del bloque XHTML generado.</summary>
      public const string TAG_HTML_ID = "oid";
      /// <summary>Identificador de la plantilla.</summary>
      public const string TAG_TEMPLATE_ID = "tid";
      /// <summary>Título del mensaje.</summary>
      public const string TAG_MSG_TITLE = "title";
      /// <summary>Texto del mensaje.</summary>
      public const string TAG_MSG_TEXT = "text";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomMessage"/>.
      /// </summary>
      public DomMessage()
      {
         this.ID = "msg1";
         _title = "";
         _message = "";
         _type = MessageBoxTypes.Information;

         switch (_type)
         {
            case MessageBoxTypes.Error:
               _title = "Se produjo un error:";
               break;
            case MessageBoxTypes.Information:
               _title = "Información:";
               break;
            case MessageBoxTypes.Warning:
               _title = "Atención:";
               break;
         }
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomMessage"/>.
      /// </summary>
      /// <param name="message">Cuerpo del mensaje.</param>
      /// <param name="type">Tipo de mensaje.</param>
      public DomMessage(string message, MessageBoxTypes type)
      {
         this.ID = "msg1";
         _message = message;
         _type = type;

         switch (type)
         {
            case MessageBoxTypes.Error:
               _title = "Se produjo un error:";
               break;
            case MessageBoxTypes.Information:
               _title = "Información:";
               break;
            case MessageBoxTypes.Warning:
               _title = "Atención:";
               break;
         }
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomMessage"/>.
      /// </summary>
      /// <param name="title">Título del mensaje.</param>
      /// <param name="message">Cuerpo del mensaje.</param>
      /// <param name="type">Tipo de mensaje.</param>
      public DomMessage(string title, string message, MessageBoxTypes type)
      {
         this.ID = "msg1";
         _title = title;
         _message = message;
         _type = type;
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "info-box"; }
      }

      /// <summary>
      /// Título del mensaje
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Mensaje a mostrar
      /// </summary>
      public string Message
      {
         get { return _message; }
         set { _message = value; }
      }

      /// <summary>
      /// Tipo de mensaje a mostrar.
      /// </summary>
      public MessageBoxTypes Type
      {
         get { return _type; }
         set { _type = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el componente.</returns>
      public override string Render(DomTemplate template, DomPage.ContentContainer container)
      {
         string xhtml = string.Empty;

         // Obtiene la plantilla del componente
         DomTemplateComponent component = template.GetContentComponent(this.ELEMENT_ROOT, container);
         if (component == null) return string.Empty;

         switch (this.Type)
         {
            case MessageBoxTypes.Error:
               xhtml = component.GetFragment(DomMessage.SECTION_MSG_ERROR);
               break;
            case MessageBoxTypes.Warning:
               xhtml = component.GetFragment(DomMessage.SECTION_MSG_WARNING);
               break;
            case MessageBoxTypes.Information:
               xhtml = component.GetFragment(DomMessage.SECTION_MSG_INFO);
               break;
            default:
               xhtml = string.Empty; 
               break;
         }

         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomMessage.TAG_HTML_ID, this.ID);
         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomMessage.TAG_TEMPLATE_ID, template.ID.ToString());
         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomMessage.TAG_MSG_TITLE, HttpUtility.HtmlEncode(this.Title));
         xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomMessage.TAG_MSG_TEXT, this.Message);

         return xhtml;
      }

      #endregion

      #region Private members

      /// <summary>
      /// Devuelve un mensaje formateado por defecto para mostrar en caso que no se tenga acceso a ninguna plantilla.
      /// </summary>
      private string GetDefaultMessage(Workspace ws)
      {
         string xhtml = string.Empty;

         xhtml += "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n";
         xhtml += "<html>\n";
         xhtml += "<head>\n";
         xhtml += "	<title>" + ws.Name + " - " + DomMessage.TAG_MSG_TITLE + "</title>\n";
         xhtml += "	<style type=\"text/css\">\n";
         xhtml += "		a:link, a:visited, a:active {color:#03C;}\n";
         xhtml += "		body, h1, h2, h3 { font-family:Arial,Helvetica,sans-serif;color:#000; }\n";
         xhtml += "		h1 { font-size:24px;font-weight:normal; }\n";
         xhtml += "		h2 { font-size:18px;font-weight:normal; }\n";
         xhtml += "		h3 { font-size:14px;font-weight:bold; }\n";
         xhtml += "		hr { margin-top:10px;margin-bottom:10px;height:1px;color:#999;background-color:#999;border:0; }\n";
         xhtml += "		p, td, li { font-size:13px;line-height:16px;color:#000; }\n";
         xhtml += "		.video_box { width:120;border:1px solid #CCC;padding:5px; }\n";
         xhtml += "	</style>\n";
         xhtml += "</head>\n";
         xhtml += "<body style=\"background-color:#FFF;margin:0px;padding:0px;\">\n";
         xhtml += "	<div style=\"padding:10px;background-color:rgb(0,52,102);margin-bottom:25px;\">\n";
         xhtml += "		<h1 style=\"color:#FFF;margin-top:0px;margin-bottom:0px;\">" + DomMessage.TAG_MSG_TITLE + "</h1>\n";
         xhtml += "	</div>\n";
         xhtml += "	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" style=\"margin-top:10px;margin-bottom:10px;\">\n";
         xhtml += "		<tr valign=\"top\">\n";
         xhtml += "			<td>\n";
         xhtml += "				<div style=\"padding-left:25px;padding-right:25px;padding-bottom:25px;\">\n";
         xhtml += "           " + DomMessage.TAG_MSG_TEXT + "\n";
         xhtml += "           </div>\n";
         xhtml += "			</td>\n";
         xhtml += "		</tr>\n";
         xhtml += "	</table>\n";
         xhtml += "	<div style=\"padding:10px;background-color:#EEE;\">\n";
         xhtml += "		<p style=\"color:#666;margin:0px;\">Powered by <em>Cosmo Framework " + Workspace.Version + "</em></p>\n";
         xhtml += "	</div>\n";
         xhtml += "</body>\n";
         xhtml += "</html>\n";

         return xhtml;
      }

      #endregion
      
   }

}
