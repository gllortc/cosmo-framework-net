namespace Cosmo.Cms.Utils.Banners
{
   /// <summary>
   /// Represents a banner.
   /// </summary>
   public class Banner
   {
      // Internal data declarations
      private int _id;
      private string _name;
      private string _file;
      private string _url;
      private int _width;
      private int _height;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="Banner"/>.
      /// </summary>
      public Banner()
      {
         _id = 0;
         _name = string.Empty;
         _file = string.Empty;
         _url = string.Empty;
         _width = 0;
         _height = 0;
      }

      /// <summary>
      /// Gets a new instance of <see cref="Banner"/>.
      /// </summary>
      public Banner(int id, string name, string file, string url)
      {
         _id = id;
         _name = name;
         _file = file;
         _url = url;
         _width = 0;
         _height = 0;
      }

      /// <summary>
      /// Gets a new instance of <see cref="Banner"/>.
      /// </summary>
      public Banner(int id, string name, string file, string url, int width, int height)
      {
         _id = id;
         _name = name;
         _file = file;
         _url = url;
         _width = width;
         _height = height;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the banner unique identifier (DB).
      /// </summary>
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Gets or sets the banner name.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Gets or sets the filename (without path) of the banner (it can be GIF, JPG, PNG or Flash).
      /// </summary>
      public string File
      {
         get { return _file; }
         set { _file = value; }
      }

      /// <summary>
      /// Gets or sets the link destination URL.
      /// </summary>
      public string Url
      {
         get { return _url; }
         set { _url = value; }
      }

      /// <summary>
      /// Gets or sets the with (in pixels) of the banner.
      /// </summary>
      public int Width
      {
         get { return _width; }
         set { _width = value; }
      }

      /// <summary>
      /// Gets or sets the height (in pixels) of the banner.
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
            xhtml = "<p><a href=\"" + BannerDAO.URL_REDIRECT + "?" + Cosmo.Workspace.PARAM_OBJECT_ID + "=" + _id + "\" target=\"_blank\"><img src=\"" + _url + "\" alt=\"" + _name + "\" /></a></p>\n";
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