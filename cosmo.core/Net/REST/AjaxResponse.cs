using System.Web.Script.Serialization;

namespace Cosmo.Net.REST
{
   /// <summary>
   /// Implementa una respuesta estandarizada para cualquier llamada AJAX.
   /// </summary>
   public class AjaxResponse
   {

      #region Enumerations

      /// <summary>
      /// Tipos de respuesta a llamadas a métodos REST (que no devuelvan valores).
      /// </summary>
      public enum  JsonResponse : int
      {
         /// <summary>Indica que la llamada a un método REST ha tenido éxito.</summary>
         Successful = 1,
         /// <summary>Indica que la llamada a un método REST ha fallado.</summary>
         Fail = 0
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="AjaxResponse"/>.
      /// </summary>
      public AjaxResponse()
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="AjaxResponse"/>.
      /// </summary>
      /// <param name="errCode">Código de error.</param>
      /// <param name="errMessage">Descripción del error.</param>
      public AjaxResponse(int errCode, string errMessage)
      {
         Initialize();

         this.Result = JsonResponse.Fail;
         this.ErrorCode = errCode;
         this.ErrorMessage = errMessage;
      }

      /// <summary>
      /// Gets a new instance of <see cref="AjaxResponse"/>.
      /// </summary>
      /// <param name="xhtml">Código XHTML a devolver al cliente para ser representado.</param>
      public AjaxResponse(string xhtml)
      {
         Initialize();

         this.Xhtml = xhtml;
      }

      /// <summary>
      /// Gets a new instance of <see cref="AjaxResponse"/>.
      /// </summary>
      /// <param name="data">Datos solicitados en la llamada.</param>
      public AjaxResponse(object data)
      {
         Initialize();

         this.Data = data;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el resultado de la ejecución del comando solicitado.
      /// </summary>
      public JsonResponse Result { get; set; }

      /// <summary>
      /// Gets or sets el código de error que identifica el error producido.
      /// </summary>
      public int ErrorCode { get; set; }

      /// <summary>
      /// Gets or sets el mensaje de error producido.
      /// </summary>
      public string ErrorMessage { get; set; }

      /// <summary>
      /// Gets or sets el código XHTML que se debe reemplazar en el cliente como respuesta.
      /// </summary>
      public string Xhtml { get; set; }

      /// <summary>
      /// Gets or sets los datos solicitados.
      /// </summary>
      public object Data { get; set; }

      /// <summary>
      /// Gets or sets la URL dónde se debe navegar una vez se ha recibido la respuesta.
      /// </summary>
      public object ToURL { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve una cadena que contiene la instancia actual en formato JSON.
      /// </summary>
      /// <returns></returns>
      public string ToJSON()
      {
         JavaScriptSerializer json = new JavaScriptSerializer();
         return json.Serialize(this);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initialize the instance.
      /// </summary>
      private void Initialize()
      {
         this.Result = JsonResponse.Successful;
         this.ErrorCode = 0;
         this.ErrorMessage = string.Empty;
         this.Xhtml = string.Empty;
         this.Data = string.Empty;
      }

      #endregion

   }
}
