namespace Cosmo.Templates
{

   /// <summary>
   /// Implementa una clase que describe las capacidades y características básicas de
   /// un dispositivo de acceso Web a un workspace.
   /// </summary>
   public class BrowserCapabilities
   {
      private string _browserName;
      private string _browserAgent;
      private bool _cookies;
      private bool _download;
      private bool _upload;
      private bool _javascript;
      private int _templateID;
      private int _id;

      /// <summary>
      /// Devuelve una instancia de BrowserCapabilities
      /// </summary>
      public BrowserCapabilities()
      {
         _browserName = "";
         _browserAgent = "";
         _cookies = false;
         _download = false;
         _upload = false;
         _javascript = false;
         _templateID = 0;
         _id = 0;
      }

      #region Properties

      /// <summary>
      /// Identificador del tipo de dispositivo.
      /// </summary>
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Identificador de la plantilla a aplicar.
      /// </summary>
      public int TemplateID
      {
         get { return _templateID; }
         set { _templateID = value; }
      }

      /// <summary>
      /// Nombre del tipo de dispositivo.
      /// </summary>
      public string BrowserName
      {
         get { return _browserName; }
         set { _browserName = value; }
      }

      /// <summary>
      /// Cadena entregada por el navegador para permitir saber de qué navegador se trata.
      /// </summary>
      public string BrowserAgent
      {
         get { return _browserAgent; }
         set { _browserAgent = value; }
      }

      /// <summary>
      /// Indica si el navegador acepta Cookies.
      /// </summary>
      public bool Cookies
      {
         get { return _cookies; }
         set { _cookies = value; }
      }

      /// <summary>
      /// Indica si el navegador permite descargar archivos.
      /// </summary>
      public bool DownloadFiles
      {
         get { return _download; }
         set { _download = value; }
      }

      /// <summary>
      /// Indica si el navegador permite la ejecución de secuencias JavaScript.
      /// </summary>
      public bool JavaScript
      {
         get { return _javascript; }
         set { _javascript = value; }
      }

      /// <summary>
      /// Indica si el navegador permite enviar archivos.
      /// </summary>
      public bool UploadFiles
      {
         get { return _upload; }
         set { _upload = value; }
      }

      #endregion

   }
}
