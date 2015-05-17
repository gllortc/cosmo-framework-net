using System.Text;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa un banner insertable en espacio publicitario.
   /// </summary>
   public class DomBanner
   {

      #region Enumeraciones

      /// <summary>
      /// Enumera las distintas posiciones que puede adoptar un banner.
      /// </summary>
      public enum BannerPositions
      {
         /// <summary>Posición de banner centro-superior.</summary>
         CenterTop = 3,
         /// <summary>Posición de banner centro-medio.</summary>
         CenterMiddle = 1,
         /// <summary>Posición de banner centro-inferior.</summary>
         CenterBottom = 4,
         /// <summary>Posición de banner izquierda.</summary>
         Left = 2,
         /// <summary>Posición de banner derecha.</summary>
         Right = 5
      }

      #endregion

      bool _newWindow;
      int _id;
      BannerPositions _position;
      int _width;
      int _height;
      string _name;
      string _filename;
      string _url;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomBanner"/>.
      /// </summary>
      /// <param name="id">Un identificador numérico único para el banner.</param>
      public DomBanner(int id)
      {
         _id = id;
         _position = BannerPositions.CenterTop; 
         _width = 0;
         _height = 0;
         _name = string.Empty;
         _filename = string.Empty;
         _url = string.Empty;
         _newWindow = true;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomBanner"/>.
      /// </summary>
      /// <param name="id">Un identificador numérico único para el banner.</param>
      /// <param name="position">Posición del banner dentro de la página.</param>
      /// <param name="name">Nombre del banner o campaña publicitaria.</param>
      /// <param name="filename">Devuelve o establece el nombre del archivo que se debe mostrar.</param>
      /// <param name="url">Devuelve o establece la URL de destino del espacio publicitario.</param>
      /// <param name="width">Devuelve o establece el ancho del espacio publicitario (en píxels).</param>
      /// <param name="height">Devuelve o establece la altura del espacio publicitario (en píxels).</param>
      public DomBanner(int id, BannerPositions position, string name, string filename, string url, int width, int height)
      {
         _id = id;
         _position = position;
         _width = width;
         _height = height;
         _name = name;
         _filename = filename;
         _url = url;
         _newWindow = true;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador numérico único del banner.
      /// </summary>
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Devuelve o establece la posición del banner dentro de la página.
      /// </summary>
      public BannerPositions Position
      {
         get { return _position; }
         set { _position = value; }
      }

      /// <summary>
      /// Devuelve o establece el nombre del banner o campaña publicitaria.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Devuelve o establece el nombre del archivo que se debe mostrar.
      /// </summary>
      public string Filename
      {
         get { return _filename; }
         set { _filename = value; }
      }

      /// <summary>
      /// Devuelve o establece la URL de destino del espacio publicitario.
      /// </summary>
      public string Url
      {
         get { return _url; }
         set { _url = value; }
      }

      /// <summary>
      /// Devuelve o establece el ancho del espacio publicitario (en píxels).
      /// </summary>
      public int Width
      {
         get { return _width; }
         set { _width = value; }
      }

      /// <summary>
      /// Devuelve o establece la altura del espacio publicitario (en píxels).
      /// </summary>
      public int Height
      {
         get { return _height; }
         set { _height = value; }
      }

      /// <summary>
      /// Indica si el enlace se debe abrir en un nuevo navegador.
      /// </summary>
      public bool OpenInNewBrowser
      {
         get { return _newWindow; }
         set { _newWindow = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Convierte el banner publicitario en código XHTML para presentarlo en una página web.
      /// </summary>
      /// <returns>Una cadena en formato XHTML.</returns>
      public string ToXhtml()
      {
         StringBuilder xhtml = null;

         if (_filename.EndsWith("gif", true, System.Globalization.CultureInfo.CurrentCulture) ||
             _filename.EndsWith("jpg", true, System.Globalization.CultureInfo.CurrentCulture) ||
             _filename.EndsWith("jpeg", true, System.Globalization.CultureInfo.CurrentCulture) ||
             _filename.EndsWith("png", true, System.Globalization.CultureInfo.CurrentCulture))
         {
            xhtml = new StringBuilder();
            xhtml.AppendFormat("<a href=\"{0}\"{1}><img src=\"{2}\" alt=\"{3}\" /></a>", _url, _newWindow ? " target=\"_blank\"" : string.Empty, _filename, _name);
         }
         else if (_filename.EndsWith("swf", true, System.Globalization.CultureInfo.CurrentCulture))
         {
            xhtml = new StringBuilder();
            xhtml.AppendFormat("<object type=\"application/x-shockwave-flash\" data=\"{0}\" width=\"{1}\" height=\"{2}\">\n" +
                               "<param name=\"movie\" value=\"{3}\" />\n" +
                               "<param name=\"quality\" value=\"high\" />\n" +
                               "<a title=\"Descargar Adobe Flash Player\" href=\"http://get.adobe.com/es/flashplayer/\">Descargar Adobe Flash Player</a>\n" +
                               "</object>", _filename, _width, _height, _filename);
         }

         return (xhtml != null ? xhtml.ToString() : string.Empty);
      }

      #endregion

   }
}
