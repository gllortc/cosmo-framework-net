using System;
using System.Collections.Generic;

namespace Cosmo.Cms.Model.Content
{
   /// <summary>
   /// Implementa una carpeta contenedora de documentos.
   /// </summary>
   public class DocumentFolder
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
      /// Gets or sets el menú que se debe mostrar activo cuando se accede a la carpeta o algun documento que contenga.
      /// </summary>
      public string MenuId { get; set; }

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
