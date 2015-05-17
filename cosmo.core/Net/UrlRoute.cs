using Cosmo.Utils;
using System.IO;
using System.Web;

namespace Cosmo.Net
{
   /// <summary>
   /// Implementa un gestor de rutas URL <em>friendly</em>.
   /// Concretamente esta clase permite recuperar de forma transparente los datos de una URL 
   /// con el siguiente formato:
   /// 
   /// [ServerAddress]/ServiceName/Action/ID
   /// 
   /// Por ejemplo:
   /// www.myserver.com/Photos/ByUser/12
   /// 
   /// ...dónde:
   /// Action -> <c>ByUser</c>
   /// ID -> <c>12</c>
   /// </summary>
   public class UrlRoute
   {
      // Declaración de variables internas
      private bool _isValid;
      private string _service;
      private string _action;
      private string _id;
      private string _params;

      /// <summary>
      /// Devuelve una instancia de <see cref="UrlRoute"/>.
      /// </summary>
      /// <param name="request">El contexto de la llamada.</param>
      public UrlRoute(HttpRequest request)
      {
         Initiazlize();

         string url = request.RawUrl.Trim();

         if (url.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
         {
            url = url.Substring(1, url.Length - 1);
         }

         // Obtiene las partes de la URL
         string[] parts = url.Split(Path.AltDirectorySeparatorChar);

         switch (parts.Length)
         {
            case 1:
               _service = parts[0];
               _action = string.Empty;
               _id = string.Empty;
               _isValid = true;
               break;

            case 2:
               _service = parts[0];
               _action = parts[1];
               _id = string.Empty;
               _isValid = true;
               break;
            
            case 3:
               _service = parts[0];
               _action = parts[1];
               _id = parts[2];
               _isValid = true;
               break;

            default:
               _isValid = false;
               return;
         }
         
         // Trata el caso de que el ID contenga parámetros
         if (_id.Contains("?"))
         {
            _params = _id.Substring(_id.IndexOf('?')).Replace("?", string.Empty);
            _id = _id.Substring(0, _id.Length - _id.IndexOf('?'));
         }

         _isValid = true;
      }

      /// <summary>
      /// Devuelve el nombre del servicio.
      /// </summary>
      public string GetService
      {
         get { return _service; }
      }

      /// <summary>
      /// Devuelve el nombre de la acción.
      /// </summary>
      public string GetAction
      {
         get { return _action; }
      }

      /// <summary>
      /// Devuelve el identificador de la acción (en formato <em>string</em>).
      /// </summary>
      public string GetStringID
      {
         get { return _id; }
      }

      /// <summary>
      /// Devuelve el identificador de la acción (en formato <em>int</em>).
      /// </summary>
      public int GetIntegerID
      {
         get { return Number.StrToInt(_id); }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initiazlize()
      {
         _isValid = false;
         _service = string.Empty;
         _action = string.Empty;
         _id = string.Empty;
         _params = string.Empty;
      }
   }
}
