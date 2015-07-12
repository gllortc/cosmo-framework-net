using System;

namespace Cosmo.Net.Rss
{
   /// <summary>
   /// Implementa una entrada de un canal RSS
   /// </summary>
   public class RssItem
   {
      private string _link;
      private string _title;
      private string _description;
      private string _category;
      private string _author;
      private string _guid;
      private DateTime _pubdate;
      private RssChannel _owner;

      /// <summary>
      /// Gets a new instance of RssItem
      /// </summary>
      public RssItem()
      {
         _link = "";
         _title = "";
         _description = "";
         _category = "";
         _author = "";
         _guid = "";
         _pubdate = DateTime.Now;
         _owner = null;
      }

      /// <summary>
      /// Gets a new instance of RssItem
      /// </summary>
      public RssItem(RssChannel ownerChannel)
      {
         _link = "";
         _title = "";
         _description = "";
         _category = "";
         _author = "";
         _guid = "";
         _pubdate = DateTime.Now;
         _owner = ownerChannel;
      }

      /// <summary>
      /// Gets a new instance of RssItem
      /// </summary>
      public RssItem(string link, string title, string description)
      {
         _link = link;
         _title = title;
         _description = description;
         _pubdate = DateTime.Now;
         _owner = null;
      }

      /// <summary>
      /// Gets a new instance of RssItem
      /// </summary>
      public RssItem(string link, string title, string description, RssChannel parentFeed)
      {
         _link = link;
         _title = title;
         _description = description;
         _pubdate = DateTime.Now;
         _owner = parentFeed;
      }

      /// <summary>
      /// Gets a new instance of RssItem
      /// </summary>
      public RssItem(string link, string title, string description, DateTime pubDate)
      {
         _link = link;
         _title = title;
         _description = description;
         _pubdate = pubDate;
      }

      /// <summary>
      /// Gets a new instance of RssItem
      /// </summary>
      public RssItem(string link, string title, string description, DateTime pubDate, RssChannel parentFeed)
      {
         _link = link;
         _title = title;
         _description = description;
         _pubdate = pubDate;
         _owner = parentFeed;
      }

      #region Settings

      /// <summary>
      /// Permite acceder al feed al que pertenece el elemento.
      /// </summary>
      public RssChannel ParentFeed
      {
         get { return _owner; }
         set { _owner = value; }
      }

      /// <summary>
      /// Enlace al contenido
      /// </summary>
      public string Link
      {
         get { return _link; }
         set { _link = value; }
      }

      /// <summary>
      /// Título de la entrada
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Descripción de la entrada
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Fecha de publicación
      /// </summary>
      public DateTime PubDate
      {
         get { return _pubdate; }
         set { _pubdate = value; }
      }

      /// <summary>
      /// Categoria (Tag) de la entrada
      /// </summary>
      public string Category
      {
         get { return _category; }
         set { _category = value; }
      }

      /// <summary>
      /// Email del autor del contenido
      /// </summary>
      public string Author
      {
         get { return _author; }
         set { _author = value; }
      }

      /// <summary>
      /// Cadena que identifica de forma única el elemento
      /// </summary>
      public string Guid
      {
         get { return _guid; }
         set { _guid = value; }
      }

      #endregion

   }
}
