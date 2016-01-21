using Cosmo.Net;
using Cosmo.Net.REST;
using Cosmo.Utils.Drawing;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace Cosmo.Web.Handlers
{
   /// <summary>
   /// Handler que implementa los servicios REST correspondientes al servicio UI de Cosmo.
   /// </summary>
   public class UIRestHandler : RestHandler
   {
      /// <summary>parámetro de llamada: Nombre de archivo.</summary>
      public const string PARAMETER_TEMPLATE_NAME = "_tid_";

      #region RestHandler Implementation

      /// <summary>
      /// Método invocado al recibir una petición.
      /// </summary>
      /// <param name="command">Comando a ejecutar pasado mediante el parámetro definido mediante <see cref="Properties.Workspace.PARAM_COMMAND"/>.</param>
      public override void ServiceRequest(string command)
      {
         switch (command)
         {
            case COMMAND_GET_CAPTCHA:
               GetCaptcha();
               break;

            case COMMAND_RENDER_TEMPLATE:
               RenderTemplate(Parameters.GetString(PARAMETER_TEMPLATE_NAME));
               break;

            default:
               break;
         }
      }

      #endregion

      #region Command: Get Captcha

      /// <summary>Devuelve una imagen CAPTCHA y coloca su equivalente de texto en la sesión de usuario.</summary>
      public const string COMMAND_GET_CAPTCHA = "_getCaptcha_";

      /// <summary>
      /// Genera una URL válida para descargar un archivo asociado a un objeto.
      /// </summary>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static Url GetCaptchaUrl()
      {
         return new Url(UIRestHandler.ServiceUrl).
                        AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_GET_CAPTCHA);
      }

      /// <summary>
      /// Procesa la petición al ser recibida y sirve la imagen Captcha.
      /// </summary>
      private void GetCaptcha()
      {
         // Crea la imágen CAPTCHA usando el valor almacenado en CAPTCHA
         Captcha captcha = new Captcha();

         // Establece la cabecera del Result para devolver una imágen
         Response.Clear();
         Response.ContentType = "image/jpeg";
         Response.CacheControl = "Private";
         Response.Expires = 0;
         Response.AddHeader("pragma", "no-cache");

         // Escribe la imágen JPEG en el objeto RESPONSE
         Bitmap bitmap = captcha.GenerateCaptcha(Session);
         bitmap.Save(Response.OutputStream, ImageFormat.Gif);
      }

      #endregion

      #region Command: Render Template

      /// <summary>Renderiza una template y devuelve el código XHTML.</summary>
      public const string COMMAND_RENDER_TEMPLATE = "_rtmpl_";

      /// <summary>
      /// Genera una URL válida para descargar un archivo asociado a un objeto.
      /// </summary>
      /// <param name="templateName">Nombre de la plantilla a renderizar.</param>
      /// <returns>Una cadena que representa la URL solicitada.</returns>
      public static string GetRenderTemplateUrl(string templateName)
      {
         Url url = new Url(UIRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_RENDER_TEMPLATE)
            .AddParameter(PARAMETER_TEMPLATE_NAME, templateName);

         return url.ToString(true);
      }

      /// <summary>
      /// Descarga archivo asociado a un objeto.
      /// 
      /// Ejemplo de llamada:
      /// www.company.com/APIFileSystem?_cmd_=_download_&oid=0001&_fn_=filename.txt
      /// </summary>
      /// <param name="objectId">Identificador del objeto.</param>
      /// <param name="filename">Nombre del archivo a descargar.</param>
      private void RenderTemplate(string templateName)
      {
         Response.Clear();
         Response.ClearHeaders();
         Response.ClearContent();
         Response.ContentType = "text/html";
         Response.Flush();
         Response.Write("");
         Response.End();
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve la URL a la que se debe atacar para realizar operaciones REST sobre el servicio.
      /// </summary>
      public static string ServiceUrl
      {
         get { return MethodBase.GetCurrentMethod().DeclaringType.Name; }
      }

      #endregion

   }
}
