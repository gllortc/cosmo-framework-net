using System;

namespace Cosmo.Net.Rss
{

   /// <summary>
   /// Implementa la imagen associada a un canal RSS
   /// </summary>
   public class RssChannelImage
   {
      private Uri _url;
      private string _title;
      private string _link;

      /// <summary>
      /// Gets a new instance of RssChannelImage
      /// </summary>
      public RssChannelImage()
      {
         _title = "";
         _link = "";
      }

      /// <summary>
      /// Gets a new instance of RssChannelImage
      /// </summary>
      /// <param name="url">Url de la imagen</param>
      /// <param name="title">Título del canal</param>
      /// <param name="link">Enlace asociado a la imagen</param>
      public RssChannelImage(Uri url, string title, string link)
      {
         _url = url;
         _title = title;
         _link = link;
      }

      #region Settings

      /// <summary>
      /// Dirección URL de la imagen
      /// </summary>
      public Uri Url
      {
         get { return _url; }
         set { _url = value; }
      }

      /// <summary>
      /// Título de la imagen (equivalente al parámetro ALT del tag IMG)
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Enlace asociado a la imagen
      /// </summary>
      public string Link
      {
         get { return _link; }
         set { _link = value; }
      }

      #endregion

   }

}
