using Cosmo.Cms.Model.Photos;
using Cosmo.Net;
using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Cosmo.Cms.Model.Content
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
   /// Implements a content document.
   /// </summary>
   public class Document : IPublishable
   {
      // Variables privadas
      private string _owner;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="Document"/>.
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
      /// Gets or sets the publish status of the document.
      /// </summary>
      public CmsPublishStatus.PublishStatus Status
      {
         get { return (this.Published ? CmsPublishStatus.PublishStatus.Published : CmsPublishStatus.PublishStatus.Unpublished); }
         set { this.Published = (value == CmsPublishStatus.PublishStatus.Published); }
      }

      /// <summary>
      /// Gets the creation datetime.
      /// </summary>
      public DateTime Created { get; internal set; }

      /// <summary>
      /// Gets the datetime of the document last change.
      /// </summary>
      public DateTime Updated { get; internal set; }

      /// <summary>
      /// Gets the number of shows.
      /// </summary>
      public int Shows { get; internal set; }

      /// <summary>
      /// Gets or sets a list of document related content.
      /// </summary>
      public List<Document> RelatedDocuments { get; set; }

      /// <summary>
      /// Gets or sets a list of document related pictures.
      /// </summary>
      public List<Photo> RelatedPictures { get; set; }

      /// <summary>
      /// Gets or sets the object owner's login.
      /// </summary>
      public string Owner
      {
         get { return (string.IsNullOrEmpty(_owner) ? SecurityService.ACCOUNT_SUPER : _owner); }
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

      /// <summary>
      /// Gets a boolean value indicating if the current document has attachments.
      /// </summary>
      public bool HasAttachments
      {
         get { return !string.IsNullOrWhiteSpace(this.Attachment); }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Save current document to a XML file (serialize).
      /// </summary>
      /// <param name="filename">Filename and path of the file.</param>
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
      /// Load a document stored in a XML file (unserialize).
      /// </summary>
      /// <param name="filename">Filename and path of the file.</param>
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
      /// Check if the folder is valid and can be stored in database.
      /// </summary>
      /// <returns><c>true</c> if the object can be stored in database or <c>false</c> in all other cases.</returns>
      public bool Validate()
      {
         throw new NotImplementedException();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
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

         _owner = SecurityService.ACCOUNT_SUPER;
      }

      #endregion

   }
}
