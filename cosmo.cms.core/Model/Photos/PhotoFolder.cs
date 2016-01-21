using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;

namespace Cosmo.Cms.Model.Photos
{
   /// <summary>
   /// Implementa una carpeta de imágenes.
   /// </summary>
   public class PhotoFolder : Cosmo.Cms.Model.IPublishable
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PhotoFolder"/>.
      /// </summary>
      public PhotoFolder()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Identificador único de la carpeta.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Identificador de la carpeta padre.
      /// </summary>
      public int ParentID { get; set; }

      /// <summary>
      /// Nombre de la carpeta.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Descripción del contenido de la carpeta.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets a value indicating if the folder can contain objects or is only a structural element.
      /// </summary>
      public bool IsContainer { get; set; }

      /// <summary>
      /// Gets or sets el patrón para nombrar nuevos archivos de imagen.
      /// </summary>
      public string FilePattern { get; set; }

      /// <summary>
      /// Orden de la carpeta.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Indica si la carpeta está publicada.
      /// </summary>
      public bool Enabled { get; set; }

      /// <summary>
      /// Gets or sets the owner login.
      /// </summary>
      /// <remarks>
      /// By default, this login is the same of the creator account.
      /// </remarks>
      public string Owner { get; set; }

      /// <summary>
      /// Gets or sets the publish status of the object.
      /// </summary>
      public CmsPublishStatus.PublishStatus Status
      {
         get { return (this.Enabled ? CmsPublishStatus.PublishStatus.Published : CmsPublishStatus.PublishStatus.Unpublished); }
         set { this.Enabled = (value == CmsPublishStatus.PublishStatus.Published); }
      }

      /// <summary>
      /// Gets or sets the creation timestamp.
      /// </summary>
      public System.DateTime Created
      {
         get { return DateTime.Now; }
      }

      /// <summary>
      /// Gets or sets the last modification timestamp.
      /// </summary>
      public System.DateTime Updated
      {
         get { return DateTime.Now; }
      }

      /// <summary>
      /// Save the object to XML a file.
      /// </summary>
      /// <param name="filename">File (and path) to output file.</param>
      public void Save(string filename)
      {
         throw new System.NotImplementedException();
      }

      /// <summary>
      /// Loads an object from XML a file (created previously by the method <c>Save()</c>).
      /// </summary>
      /// <param name="filename">File (and path) to file to load.</param>
      public void Load(string filename)
      {
         throw new System.NotImplementedException();
      }

      /// <summary>
      /// Check instance data for validate its content.
      /// </summary>
      /// <returns><c>true</c> if data is correct or <c>false</c> in all other cases.</returns>
      public bool Validate()
      {
         return true;
      }

      /// <summary>
      /// Devuelve el número de objetos que contiene la carpeta.
      /// </summary>
      public int Objects { get; set; }

      /// <summary>
      /// Devuelve la lista de subcarpetas.
      /// </summary>
      public List<PhotoFolder> Subfolders { get; set; }

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
         this.IsContainer = false;
         this.FilePattern = string.Empty;
         this.Order = 0;
         this.Enabled = false;
         this.Owner = SecurityService.ACCOUNT_SUPER;
         this.Objects = 0;
         this.Subfolders = new List<PhotoFolder>();
      }

      #endregion

   }
}
