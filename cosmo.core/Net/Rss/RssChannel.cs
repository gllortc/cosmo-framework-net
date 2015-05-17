using Cosmo.Utils.IO;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Web.Caching;
using System.Xml;

namespace Cosmo.Net.Rss
{

   /// <summary>
   /// Implementa un canal RSS
   /// </summary>
   public class RssChannel
   {
      /// <summary>Nombre del parámetro QueryString que indica el canal RSS (si hubiera más de un canal).</summary>
      public const string UrlParamChannel = "ch";
      /// <summary>Parámetro que permite forzar al refresco del contenido del canal sin usar el conteni9do en caché.</summary>
      public const string UrlParamForceUpdate = "force";

      private string _title;          // Required fields
      private string _link;
      private string _description;
      private string _language;       // Optional fields
      private string _copyright;
      private string _managingEditor;
      private string _webMaster;
      private DateTime _pubDate;
      private DateTime _lastBuildDate;
      private string _category;
      private string _generator;
      private RssChannelImage _image;
      private Collection<RssItem> _items;

      const string TagChannel = "channel";
      const string TagChannelTitle = "title";
      const string TagChannelLink = "link";
      const string TagChannelDescription = "description";
      const string TagChannelLanguage = "language";
      const string TagChannelCopyright = "copyright";
      const string TagChannelMEditor = "managingEditor";
      const string TagChannelWMaster = "webMaster";
      const string TagChannelPubdate = "pubDate";
      const string TagChannelBuildate = "lastBuildDate";
      const string TagChannelCategory = "category";
      const string TagChannelGenerator = "generator";
      const string TagChannelDocs = "docs";

      const string TagImage = "image";
      const string TagImageUrl = "url";
      const string TagImageTitle = "title";
      const string TagImageLink = "link";

      const string TagItem = "item";
      const string TagItemTitle = "title";
      const string TagItemLink = "link";
      const string TagItemDescription = "description";
      const string TagItemPubdate = "pubDate";
      const string TagItemCategory = "category";
      const string TagItemAuthor = "author";
      const string TagItemGuid = "guid";

      /// <summary>
      /// Devuelve una instancia de RssChannel
      /// </summary>
      public RssChannel()
      {
          ResetObject();
      }

      /// <summary>
      /// Devuelve una instancia de RssChannel
      /// </summary>
      public RssChannel(Uri url)
      {
          ResetObject();
          Read(url);
      }

      #region Settings

      /// <summary>
      /// Título del canal
      /// </summary>
      public string Title
      {
          get { return _title; }
          set { _title = value; }
      }

      /// <summary>
      /// Enlace del site que generó el canal
      /// </summary>
      public string Link
      {
          get { return _link; }
          set { _link = value; }
      }

      /// <summary>
      /// Descripción del canal
      /// </summary>
      public string Description
      {
          get { return _description; }
          set { _description = value; }
      }

      /// <summary>
      /// Idioma usado en el canal
      /// </summary>
      public string Language
      {
          get { return _description; }
          set { _description = value; }
      }

      /// <summary>
      /// Copyright legal del contenido
      /// </summary>
      public string Copyright
      {
          get { return _copyright; }
          set { _copyright = value; }
      }

      /// <summary>
      /// Editor del contenido
      /// </summary>
      public string ManagingEditor
      {
          get { return _managingEditor; }
          set { _managingEditor = value; }
      }

      /// <summary>
      /// Responsable del sitio que generó el canal
      /// </summary>
      public string Webmaster
      {
          get { return _webMaster; }
          set { _webMaster = value; }
      }

      /// <summary>
      /// Fecha de publicación
      /// </summary>
      public DateTime PubDate
      {
          get { return _pubDate; }
          set { _pubDate = value; }
      }

      /// <summary>
      /// Fecha de la última actualización del canal
      /// </summary>
      public DateTime LastBuildDate
      {
          get { return _lastBuildDate; }
          set { _lastBuildDate = value; }
      }

      /// <summary>
      /// Categoria del canal
      /// </summary>
      public string Category
      {
          get { return _category; }
          set { _category = value; }
      }

      /// <summary>
      /// Software que generó el canal
      /// </summary>
      public string Generator
      {
          get { return _generator; }
          set { _generator = value; }
      }

      /// <summary>
      /// Imagen (logotipo) asociada al canal
      /// </summary>
      public RssChannelImage Image
      {
          get { return _image; }
          set { _image = value; }
      }

      /// <summary>
      /// Lista de elementos que se muestran en el canal
      /// </summary>
      public Collection<RssItem> Items
      {
          get { return _items; }
          set { _items = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Convierte un canal en código XML en formato RSS 2.0
      /// </summary>
      /// <returns>Una cadena en formato RSS 2.0</returns>
      public void ToStream(System.IO.Stream stream)
      {
          XmlTextWriter xml = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
          xml.Indentation = 1;
          xml.IndentChar = ' ';

          xml.WriteStartDocument();

          // These are RSS Tags
          xml.WriteStartElement("rss");
          xml.WriteAttributeString("version", "2.0");

          xml.WriteStartElement(TagChannel);
          xml.WriteElementString(TagChannelTitle, _title);
          xml.WriteElementString(TagChannelLink, _link);
          xml.WriteElementString(TagChannelDescription, _description);
          if (!String.IsNullOrEmpty(_language)) xml.WriteElementString(TagChannelLanguage, _language);
          if (!String.IsNullOrEmpty(_copyright)) xml.WriteElementString(TagChannelCopyright, _copyright);
          if (!String.IsNullOrEmpty(_managingEditor)) xml.WriteElementString(TagChannelMEditor, _managingEditor);
          if (!String.IsNullOrEmpty(_webMaster)) xml.WriteElementString(TagChannelWMaster, _webMaster);
          if (_pubDate > DateTime.MinValue) xml.WriteElementString(TagChannelPubdate, _pubDate.ToString("r", CultureInfo.InvariantCulture));
          if (_lastBuildDate > DateTime.MinValue) xml.WriteElementString(TagChannelBuildate, _lastBuildDate.ToString("r", CultureInfo.InvariantCulture));
          if (!String.IsNullOrEmpty(_category)) xml.WriteElementString(TagChannelCategory, _category);
          if (!String.IsNullOrEmpty(_generator)) xml.WriteElementString(TagChannelGenerator, _generator);
          xml.WriteElementString(TagChannelDocs, "http://www.rssboard.org/rss-specification");

          // Imagen del canal
          if (_image != null)
          {
              xml.WriteStartElement(TagImage);
              xml.WriteElementString(TagImageUrl, _image.Url.ToString());
              xml.WriteElementString(TagImageTitle, _image.Title);
              if (!String.IsNullOrEmpty(_image.Link)) xml.WriteElementString(TagImageLink, _image.Link);
              xml.WriteEndElement();
          }

          // Write all Posts in the rss feed
          foreach (RssItem item in _items)
          {
              xml.WriteStartElement(TagItem);
              xml.WriteElementString(TagItemTitle, item.Title);
              xml.WriteElementString(TagItemDescription, item.Description);
              xml.WriteElementString(TagItemLink, item.Link);
              if (item.PubDate > DateTime.MinValue) xml.WriteElementString(TagItemPubdate, item.PubDate.ToString("r", CultureInfo.InvariantCulture));
              if (!String.IsNullOrEmpty(item.Author)) xml.WriteElementString(TagItemAuthor, item.Author);
              if (!String.IsNullOrEmpty(item.Category)) xml.WriteElementString(TagItemCategory, item.Category);
              if (!String.IsNullOrEmpty(item.Guid)) xml.WriteElementString(TagItemGuid, item.Guid);
              xml.WriteEndElement();
          }

          // Close all open tags tags
          xml.WriteEndElement();
          xml.WriteEndElement();
          xml.WriteEndDocument();
          xml.Flush();
          xml.Close();
      }

      /// <summary>
      /// Convierte un canal en código XML en formato RSS 2.0
      /// </summary>
      /// <returns>Una cadena en formato RSS 2.0</returns>
      public void ToStream(System.IO.Stream stream, Cache cache, string key, int seconds)
      {
          byte[] xml;

          if (seconds <= 0) seconds = 3600;

          try
          {
              if (cache[key] == null)
                  cache.Insert(key, ToXml(), null, DateTime.Now.AddSeconds(seconds), TimeSpan.Zero);

              System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
              xml = encoding.GetBytes(cache[key].ToString());

              stream.Write(xml, 0, xml.Length);
          }
          catch
          {
              throw;
          }
      }

      /// <summary>
      /// Convierte un canal en código XML en formato RSS 2.0
      /// </summary>
      /// <returns>Una cadena en formato RSS 2.0</returns>
      public string ToXml()
      {
         EncodedStringWriter sw = new EncodedStringWriter(new StringBuilder(), System.Text.Encoding.UTF8);
         XmlTextWriter xml = new XmlTextWriter(sw);

         xml.WriteStartDocument();

         // These are RSS Tags
         xml.WriteStartElement("rss");
         xml.WriteAttributeString("version", "2.0");

         xml.WriteStartElement(TagChannel);
         xml.WriteElementString(TagChannelTitle, _title);
         xml.WriteElementString(TagChannelLink, _link);
         xml.WriteElementString(TagChannelDescription, _description);
         if (!String.IsNullOrEmpty(_language)) xml.WriteElementString(TagChannelLanguage, _language);
         if (!String.IsNullOrEmpty(_copyright)) xml.WriteElementString(TagChannelCopyright, _copyright);
         if (!String.IsNullOrEmpty(_managingEditor)) xml.WriteElementString(TagChannelMEditor, _managingEditor);
         if (!String.IsNullOrEmpty(_webMaster)) xml.WriteElementString(TagChannelWMaster, _webMaster);
         if (_pubDate > DateTime.MinValue) xml.WriteElementString(TagChannelPubdate, _pubDate.ToString("r", CultureInfo.InvariantCulture));
         if (_lastBuildDate > DateTime.MinValue) xml.WriteElementString(TagChannelBuildate, _lastBuildDate.ToString("r", CultureInfo.InvariantCulture));
         if (!String.IsNullOrEmpty(_category)) xml.WriteElementString(TagChannelCategory, _category);
         if (!String.IsNullOrEmpty(_generator)) xml.WriteElementString(TagChannelGenerator, _generator);
         xml.WriteElementString(TagChannelDocs, "http://www.rssboard.org/rss-specification");

         // Imagen del canal
         if (_image != null)
         {
            xml.WriteStartElement(TagImage);
            xml.WriteElementString(TagImageUrl, _image.Url.ToString());
            xml.WriteElementString(TagImageTitle, _image.Title);
            if (!String.IsNullOrEmpty(_image.Link)) xml.WriteElementString(TagImageLink, _image.Link);
            xml.WriteEndElement();
         }

         // Write all Posts in the rss feed
         foreach (RssItem item in _items)
         {
            xml.WriteStartElement(TagItem);
            xml.WriteElementString(TagItemTitle, item.Title);
            xml.WriteElementString(TagItemDescription, item.Description);
            xml.WriteElementString(TagItemLink, item.Link);
            if (item.PubDate > DateTime.MinValue) xml.WriteElementString(TagItemPubdate, item.PubDate.ToString("r", CultureInfo.InvariantCulture));
            if (!String.IsNullOrEmpty(item.Author)) xml.WriteElementString(TagItemAuthor, item.Author);
            if (!String.IsNullOrEmpty(item.Category)) xml.WriteElementString(TagItemCategory, item.Category);
            if (!String.IsNullOrEmpty(item.Guid)) xml.WriteElementString(TagItemGuid, item.Guid);
            xml.WriteEndElement();
         }

         // Close all open tags tags
         xml.WriteEndElement();
         xml.WriteEndElement();
         xml.WriteEndDocument();
         xml.Flush();
         xml.Close();

         return sw.ToString();
      }

      /// <summary>
      /// Convierte un canal en código XML en formato RSS 2.0 usando caché de servidor
      /// </summary>
      /// <param name="cache">La instancia de Cache del servidor web</param>
      /// <param name="key">Clave para identificar el objeto (staring) en la caché</param>
      /// <param name="seconds">Número de segundos de validez de la caché</param>
      /// <returns>Una cadena en formato RSS 2.0</returns>
      public string ToXml(Cache cache, string key, int seconds)
      {
         if (seconds <= 0) seconds = 3600;

         try
         {
            if (cache[key] == null)
            {
               cache.Insert(key, ToXml(), null, DateTime.Now.AddSeconds(seconds), TimeSpan.Zero);
            }

            return cache[key].ToString();
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Lee un canal RSS y lo carga en la estructura de la clase
      /// </summary>
      /// <param name="url">Dirección de descarga del canal RSS</param>
      public void Read(Uri url)
      {
         XmlTextReader rssReader = null;
         XmlDocument rssDoc = null;
         XmlNode nodeRss = null;
         XmlNode nodeChannel = null;
         XmlNode nodeImage = null;
         XmlNode nodeItem = null;

         // Limpia el objeto
         ResetObject();

         try
         {
            // Lee el orígen de datos (archivo o URL)
            rssReader = new XmlTextReader(url.ToString());
            rssDoc = new XmlDocument();

            // Load the XML content into a XmlDocument
            rssDoc.Load(rssReader);

            // Loop for the <rss> tag
            for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
            {
               // If it is the rss tag
               if (rssDoc.ChildNodes[i].Name == "rss")
               {
                  // <rss> tag found
                  nodeRss = rssDoc.ChildNodes[i];
                  break;
               }
            }

            // Loop for the <channel> tag
            for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
            {
               // If it is the channel tag
               if (nodeRss.ChildNodes[i].Name == TagChannel)
               {
                  // <channel> tag found
                  nodeChannel = nodeRss.ChildNodes[i];
                  break;
               }
            }

            // Loop for the <image> tag
            for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
            {
               // If it is the channel tag
               if (nodeChannel.ChildNodes[i].Name == TagImage)
               {
                  // <channel> tag found
                  nodeImage = nodeChannel.ChildNodes[i];
                  break;
               }
            }

            // Propiedades obligatorias del canal
            this.Title = nodeChannel[TagChannelTitle].InnerText;
            this.Link = nodeChannel[TagChannelLink].InnerText;
            this.Description = nodeChannel[TagChannelDescription].InnerText;

            // Propiedades opcionales del canal
            if (nodeChannel[TagChannelLanguage] != null) this.Language = nodeChannel[TagChannelLanguage].InnerText;
            if (nodeChannel[TagChannelCopyright] != null) this.Copyright = nodeChannel[TagChannelCopyright].InnerText;
            if (nodeChannel[TagChannelMEditor] != null) this.ManagingEditor = nodeChannel[TagChannelMEditor].InnerText;
            if (nodeChannel[TagChannelWMaster] != null) this.Webmaster = nodeChannel[TagChannelWMaster].InnerText;
            if (nodeChannel[TagChannelCategory] != null) this.Category = nodeChannel[TagChannelCategory].InnerText;
            if (nodeChannel[TagChannelGenerator] != null) this.Generator = nodeChannel[TagChannelGenerator].InnerText;

            // Parsea las fechas
            this.PubDate = DateTime.Now;
            if (nodeChannel[TagChannelPubdate] != null)
               if (!DateTime.TryParse(nodeChannel[TagChannelPubdate].InnerText, out _pubDate))
                  this.PubDate = DateTime.Now;

            this.LastBuildDate = this.PubDate;
            if (nodeChannel[TagChannelBuildate] != null)
               if (!DateTime.TryParse(nodeChannel[TagChannelBuildate].InnerText, out _lastBuildDate))
                  this.LastBuildDate = this.PubDate;

            // Propiedades de la imagen
            if (nodeImage != null)
            {
               _image = new RssChannelImage();
               if (nodeImage[TagImageTitle] != null) _image.Title = nodeImage[TagImageTitle].InnerText;
               if (nodeImage[TagImageUrl] != null) _image.Url = new Uri(nodeImage[TagImageUrl].InnerText);
               if (nodeImage[TagImageLink] != null) _image.Link = nodeImage[TagImageLink].InnerText;
            }

            // Loop for the <title>, <link>, <description> and all the other tags
            for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
            {
               // If it is the item tag, then it has children tags which we will add as items to the ListView
               if (nodeChannel.ChildNodes[i].Name == TagItem)
               {
                  nodeItem = nodeChannel.ChildNodes[i];

                  RssItem item = new RssItem(nodeItem[TagItemLink].InnerText,
                                             nodeItem[TagItemTitle].InnerText,
                                             nodeItem[TagItemDescription].InnerText,
                                             this);
                  if (nodeItem[TagItemCategory] != null) item.Category = nodeItem[TagItemCategory].InnerText;
                  if (nodeItem[TagItemAuthor] != null) item.Author = nodeItem[TagItemAuthor].InnerText;
                  if (nodeItem[TagItemGuid] != null) item.Guid = nodeItem[TagItemGuid].InnerText;

                  // Obtiene la fecha de la entrada
                  if (nodeItem[TagItemPubdate] != null)
                  {
                     DateTime _tempDate = DateTime.Now;
                     DateTime.TryParse(nodeItem[TagItemPubdate].InnerText, out _tempDate);
                     item.PubDate = _tempDate;
                  }

                  _items.Add(item);
               }
            }
         }
         catch (Exception ex)
         {
            throw new Exception("El canal RSS no es válido (" + url + ").", ex);
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Limpia el contenido del objeto
      /// </summary>
      private void ResetObject()
      {
         _title = "";
         _link = "";
         _description = "";
         _language = "es-es";
         _copyright = "";
         _managingEditor = "";
         _webMaster = "";
         _pubDate = DateTime.Now;
         _lastBuildDate = DateTime.Now;
         _category = "";
         _generator = "GLCSoft ContentServer.NET 2009";
         _image = null;
         _items = new Collection<RssItem>();
      }

      #endregion

   }
}
