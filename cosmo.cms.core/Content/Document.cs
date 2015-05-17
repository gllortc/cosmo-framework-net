using Cosmo.Cms.Common;
using Cosmo.Cms.Photos;
using Cosmo.Net;
using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Cosmo.Cms.Content
{

   #region Enumerations

   /// <summary>
   /// Tipo de transformación a XHTML
   /// </summary>
   public enum TransformType
   {
      /// <summary>Completo</summary>
      Full,
      /// <summary>Resumen</summary>
      Summary
   }

   #endregion

   /// <summary>
   /// Representa un documento de contenido XHTML.
   /// </summary>
   public class Document : IPublishable
   {
      // Variables privadas
      private string _owner;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="Document"/>.
      /// </summary>
      public Document()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Identificador único del documento.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Identificador de la carpeta a la que pertenece.
      /// </summary>
      public int FolderId { get; set; }

      /// <summary>
      /// Título del documento.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Descripción corta del documento.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Contenido principal del documento.
      /// </summary>
      public string Content { get; set; }

      /// <summary>
      /// Archivo adjunto al objeto.
      /// </summary>
      public string Attachment { get; set; }

      /// <summary>
      /// Imagen en miniatura.
      /// </summary>
      public string Thumbnail { get; set; }

      /// <summary>
      /// Plantilla de presentación.
      /// </summary>
      /// <remarks>
      /// Nombre del script ASPX a usar para cargar el objeto.
      /// </remarks>
      [Obsolete()]
      public string Template { get; set; }

      /// <summary>
      /// Indica si el documento es destacado.
      /// </summary>
      public bool Hightlight { get; set; }

      /// <summary>
      /// Indica si está publicado (visible públicamente) o no.
      /// </summary>
      public bool Published { get; set; }

      /// <summary>
      /// Indica si está publicado (visible públicamente) o no.
      /// </summary>
      public PublishStatus Status
      {
         get { return (this.Published ? PublishStatus.Published : PublishStatus.Unpublished); }
         set { this.Published = (value == PublishStatus.Published); }
      }

      /// <summary>
      /// Fecha de creación del documento.
      /// </summary>
      public DateTime Created { get; set; }

      /// <summary>
      /// Fecha de la última actualización.
      /// </summary>
      public DateTime Updated { get; set; }

      /// <summary>
      /// Número de veces que se ha mostrado el documento.
      /// </summary>
      public int Shows { get; set; }

      /// <summary>
      /// Una lista de los documentos relacionados.
      /// </summary>
      public List<Document> RelatedDocuments { get; set; }

      /// <summary>
      /// Una lista de los imágenes relacionados.
      /// </summary>
      public List<Photo> RelatedPictures { get; set; }

      /// <summary>
      /// Devuelve o establece el login del propietario del objeto.
      /// </summary>
      public string Owner
      {
         get { return (string.IsNullOrEmpty(_owner) ? AuthenticationService.ACCOUNT_SUPER : _owner); }
         set { _owner = value; }
      }

      /// <summary>
      /// Devuelve la URL relativa al documento.
      /// </summary>
      public string Href
      {
         get 
         {
            Url url = new Url(this.Template);
            url.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, this.ID);
            return url.ToString(true); 
         }
      }

      #endregion

      #region Methods

      /*
      /// <summary>
      /// Convierte un documento a código XHTML para presentar en un navegador
      /// </summary>
      /// <param name="type">Tipo de transformación a efectuar</param>
      /// <returns>Una cadena el formato XHTML</returns>
      public string ToXhtml(TransformType type)
      {
         string xhtml = string.Empty;

         if (type == TransformType.Summary)
         {
            string url = this.Template + "?" + Cosmo.Workspace.PARAM_OBJECT_ID + "=" + this.Id;

            xhtml += "<div class=\"doc-entry\">\n";
            xhtml += "<div class=\"thumnail\"><a href=\"" + url + "\"><img src=\"" + this.Thumbnail + "\" alt=\"" + HttpUtility.HtmlDecode(this.Title) + "\" /></a></div>\n";
            xhtml += "<p><span class=\"title\"><a href=\"" + url + "\">" + HttpUtility.HtmlDecode(this.Title) + "</a></span><br />" + HttpUtility.HtmlDecode(this.Description) + "</p>\n";
            xhtml += "<p class=\"info\">Publicado: <span class=\"alt\">" + this.Created.ToString("dd/MM/yyyy") + "</span></p>\n";
            xhtml += "</div>\n";
         }
         else
         {
            // COntenido del documento
            xhtml += "<h1>" + HttpUtility.HtmlDecode(this.Title) + "</h1>\n";
            xhtml += "<div class=\"text\">\n";
            xhtml += this.Content;
            xhtml += "</div>\n";

            // Imágenes relacionadas
            if (this.RelatedPictures.Count > 0)
            {
               xhtml += "<h2>Imágenes</h2>\n";
               foreach (Picture picture in this.RelatedPictures)
                  xhtml += picture.ToXhtml();
            }

            // Documentos relacionadas
            if (this.RelatedDocuments.Count > 0)
            {
               xhtml += "<h2>Documentos relacionados</h2>\n";
               foreach (Document document in this.RelatedDocuments)
                  xhtml += document.ToXhtml(TransformType.Summary);
            }
         }

         return xhtml;
      }

      /// <summary>
      /// Convierte un documento a código XHTML para presentar en un navegador
      /// </summary>
      /// <returns>Una cadena el formato XHTML</returns>
      public string ToXhtml()
      {
         return ToXhtml(TransformType.Full);
      }
      */

      /// <summary>
      /// Serializa el objeto a un archivo XML.
      /// </summary>
      /// <param name="filename">Archivo de salida.</param>
      public void Save(string filename)
      {
         // Comprueba que exista el archivo
         FileInfo file = new FileInfo(filename);
         if (!file.Exists)
         {
            throw new FileNotFoundException("Archivo de configuración no encontrado [" + filename + "]");
         }

         XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
         XmlDocument document = new XmlDocument();

         throw new NotImplementedException();
      }

      /// <summary>
      /// Desserializa un objeto serializado en un archivo XML y carga los datos en la instancia actual.
      /// </summary>
      /// <param name="filename">Archivo a cargar.</param>
      public void Load(string filename)
      {
         // Comprueba que exista el archivo
         FileInfo file = new FileInfo(filename);
         if (!file.Exists)
         {
            throw new FileNotFoundException("Archivo de configuración no encontrado [" + filename + "]");
         }
      }

      /// <summary>
      /// Valida los datos del objeto.
      /// </summary>
      public bool Validate()
      {
         throw new NotImplementedException();
      }

      /*
      /// <summary>
      /// Genera una URL (relativa) para el acceso a un determinado documento.
      /// </summary>
      /// <param name="folderId">Identificador único del documento.</param>
      public static string GetUrl(int documentId)
      {
         return DocumentDAO.URL_CONTENT_VIEW + "?" + Cosmo.Workspace.PARAM_OBJECT_ID + "=" + documentId;
      }
      */

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.ID = 0;
         this.FolderId = 0;
         this.Title = string.Empty;
         this.Description = string.Empty;
         this.Content = string.Empty;
         this.Attachment = string.Empty;
         this.Thumbnail = string.Empty;
         this.Template = string.Empty;
         this.Hightlight = false;
         this.Published = true;
         this.Created = System.DateTime.Now;
         this.Updated = System.DateTime.Now;
         this.Shows = 0;
         this.RelatedDocuments = new List<Document>();
         this.RelatedPictures = new List<Photo>();

         _owner = AuthenticationService.ACCOUNT_SUPER;
      }

      #endregion
   }
}
