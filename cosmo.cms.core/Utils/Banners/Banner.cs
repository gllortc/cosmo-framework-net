namespace Cosmo.Cms.Utils.Banners
{
   /// <summary>
   /// Represents a banner.
   /// </summary>
   public class Banner
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="Banner"/>.
      /// </summary>
      public Banner()
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="Banner"/>.
      /// </summary>
      public Banner(int id, string name, string file, string url)
      {
         Initialize();

         this.ID = id;
         this.Name = name;
         this.File = file;
         this.Url = url;
      }

      /// <summary>
      /// Gets a new instance of <see cref="Banner"/>.
      /// </summary>
      public Banner(int id, string name, string file, string url, int width, int height)
      {
         Initialize();

         this.ID = id;
         this.Name = name;
         this.File = file;
         this.Url = url;
         this.Width = width;
         this.Height = height;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the banner unique identifier (DB).
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Gets or sets the banner name.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets the filename (without path) of the banner (it can be GIF, JPG, PNG or Flash).
      /// </summary>
      public string File { get; set; }

      /// <summary>
      /// Gets or sets the link destination URL.
      /// </summary>
      public string Url { get; set; }

      /// <summary>
      /// Gets or sets the with (in pixels) of the banner.
      /// </summary>
      public int Width { get; set; }

      /// <summary>
      /// Gets or sets the height (in pixels) of the banner.
      /// </summary>
      public int Height { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initialize the instance.
      /// </summary>
      private void Initialize()
      {
         this.ID = 0;
         this.Name = string.Empty;
         this.File = string.Empty;
         this.Url = string.Empty;
         this.Width = 0;
         this.Height = 0;
      }

      #endregion

   }
}