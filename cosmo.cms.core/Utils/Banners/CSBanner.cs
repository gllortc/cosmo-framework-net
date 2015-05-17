namespace Cosmo.Cms.Utils.Banners
{
   /// <summary>
   /// Summary description for CSBanner
   /// </summary>
   public class CSBanner
   {
      private int _id;
      private string _name;
      private string _file;
      private string _url;
      private int _width;
      private int _height;

      /// <summary>
      /// Devuelve una instancia de <see cref="CSBanner"/>.
      /// </summary>
      public CSBanner()
      {
         _id = 0;
         _name = string.Empty;
         _file = string.Empty;
         _url = string.Empty;
         _width = 0;
         _height = 0;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="CSBanner"/>.
      /// </summary>
      public CSBanner(int id, string name, string file, string url)
      {
         _id = id;
         _name = name;
         _file = file;
         _url = url;
         _width = 0;
         _height = 0;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="CSBanner"/>.
      /// </summary>
      public CSBanner(int id, string name, string file, string url, int width, int height)
      {
         _id = id;
         _name = name;
         _file = file;
         _url = url;
         _width = width;
         _height = height;
      }

      #region Settings

      /// <summary>
      /// Identificador único del banner
      /// </summary>
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Nombre del banner
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Archivo correspondiente al banner
      /// </summary>
      public string File
      {
         get { return _file; }
         set { _file = value; }
      }

      /// <summary>
      /// Url correspondiente al banner
      /// </summary>
      public string Url
      {
         get { return _url; }
         set { _url = value; }
      }

      /// <summary>
      /// Devuelve o establece el ancho del banner en pixels.
      /// </summary>
      public int Width
      {
         get { return _width; }
         set { _width = value; }
      }

      /// <summary>
      /// Devuelve o establece la altura del banner en pixels.
      /// </summary>
      public int Height
      {
         get { return _height; }
         set { _height = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Convierte el banner publicitario en código XHTML para presentarlo en una página web
      /// </summary>
      /// <returns>Una cadena en formato XHTML</returns>
      public string ToXhtml()
      {
         string xhtml = string.Empty;

         if (_file.EndsWith("gif", true, System.Globalization.CultureInfo.CurrentCulture) ||
             _file.EndsWith("jpg", true, System.Globalization.CultureInfo.CurrentCulture) ||
             _file.EndsWith("jpeg", true, System.Globalization.CultureInfo.CurrentCulture) ||
             _file.EndsWith("png", true, System.Globalization.CultureInfo.CurrentCulture))
         {
            xhtml = "<p><a href=\"" + CSBanners.URL_REDIRECT + "?" + Cosmo.Workspace.PARAM_OBJECT_ID + "=" + _id + "\" target=\"_blank\"><img src=\"" + _url + "\" alt=\"" + _name + "\" /></a></p>\n";
         }
         else if (_file.EndsWith("swf", true, System.Globalization.CultureInfo.CurrentCulture))
         {
            xhtml += "<p>\n";
            xhtml += "<object type=\"application/x-shockwave-flash\" data=\"" + _url + "\" width=\"" + _width + "\" height=\"" + _height + "\">\n";
            xhtml += "<param name=\"movie\" value=\"" + _url + "\" />\n";
            xhtml += "<param name=\"quality\" value=\"high\" />\n";
            xhtml += "<a title=\"Descargar Adobe Flash Player\" href=\"http://get.adobe.com/es/flashplayer/\">Descargar Adobe Flash Player</a>\n";
            xhtml += "</object>\n";
            xhtml += "</p>\n";
         }

         return xhtml;
      }

      #endregion

   }
}