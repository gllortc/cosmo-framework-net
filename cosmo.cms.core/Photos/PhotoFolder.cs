using Cosmo.Security.Auth;
using System.Collections.Generic;

namespace Cosmo.Cms.Photos
{
   /// <summary>
   /// Implementa una carpeta de imágenes.
   /// </summary>
   public class PhotoFolder
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="PhotoFolder"/>.
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
      /// Indica si la carpeta admite la subida de imágenes por parte de los usuarios.
      /// </summary>
      public bool CanUpload { get; set; }

      /// <summary>
      /// Devuelve o establece el patrón para nombrar nuevos archivos de imagen.
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
      /// Propietario/creador del objeto.
      /// </summary>
      public string Owner { get; set; }

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
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.ID = 0;
         this.ParentID = 0;
         this.Name = string.Empty;
         this.Description = string.Empty;
         this.CanUpload = false;
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
