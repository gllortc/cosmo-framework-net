using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;

namespace Cosmo.Cms.Model.Content
{
   /// <summary>
   /// Implements a content folder.
   /// </summary>
   public class DocumentFolder : IPublishable
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="DocumentFolder"/>.
      /// </summary>
      public DocumentFolder()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del objeto.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Gets or sets el identificador del objeto superior (padre).
      /// </summary>
      public int ParentID { get; set; }

      /// <summary>
      /// Gets or sets el nombre de la carpeta.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets la descripción del contenido de la carpeta.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Indica si se debe mostrar el título de la carpeta.
      /// </summary>
      public bool ShowTitle { get; set; }

      /// <summary>
      /// Gets or sets el órden en que se debe mostrar la carpeta.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or sets la fecha de creación de la carpeta.
      /// </summary>
      public DateTime Created { get; set; }

      /// <summary>
      /// Indica si el objeto está o no publicado.
      /// </summary>
      public bool Enabled { get; set; }

      /// <summary>
      /// Gets or sets el número de elementos que contiene la carpeta.
      /// </summary>
      public int Objects { get; set; }

      /// <summary>
      /// Lista de documentos que contiene la carpeta.
      /// </summary>
      public List<Document> Documents { get; set; }

      /// <summary>
      /// Lista de carpetas que contiene la carpeta.
      /// </summary>
      public List<DocumentFolder> Subfolders { get; set; }

      /// <summary>
      /// Gets or sets the ID of associated <see cref="MenuItem"/> (declared in <c>cosmo.config.xml</c>) that should 
      /// appear as a selected when the user is placed in this folder.
      /// </summary>
      public string MenuId { get; set; }

      /// <summary>
      /// Gets or sets the publish status of the document.
      /// </summary>
      public CmsPublishStatus.PublishStatus Status
      {
         get { return (this.Enabled ? CmsPublishStatus.PublishStatus.Published : CmsPublishStatus.PublishStatus.Unpublished); }
         set { this.Enabled = (value == CmsPublishStatus.PublishStatus.Published); }
      }

      /// <summary>
      /// Gets the datetime of the document last change.
      /// </summary>
      public DateTime Updated { get; internal set; }

      /// <summary>
      /// Gets the object owner's login.
      /// </summary>
      public string Owner
      {
         get { return SecurityService.ACCOUNT_SUPER; }
         set { }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Save current document to a XML file (serialize).
      /// </summary>
      /// <param name="filename">Filename and path of the file.</param>
      public void Save(string filename)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Load a document stored in a XML file (unserialize).
      /// </summary>
      /// <param name="filename">Filename and path of the file.</param>
      public void Load(string filename)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Check if the folder is valid and can be stored in database.
      /// </summary>
      /// <returns><c>true</c> if the object can be stored in database or <c>false</c> in all other cases.</returns>
      public bool Validate()
      {
         return !string.IsNullOrWhiteSpace(this.Name) & (this.ParentID >= 0);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.ID = 0;
         this.ParentID = 0;
         this.Name = string.Empty;
         this.Description = string.Empty;
         this.ShowTitle = true;
         this.Enabled = true;
         this.Order = 0;
         this.Created = System.DateTime.Now;
         this.Objects = 0;
         this.Subfolders = new List<DocumentFolder>();
         this.Documents = new List<Document>();
         this.MenuId = string.Empty;
      }

      #endregion

   }
}
