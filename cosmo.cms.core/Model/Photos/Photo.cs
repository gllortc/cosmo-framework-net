using Cosmo.Cms.Common;
using Cosmo.Security.Auth;
using System;

namespace Cosmo.Cms.Model.Photos
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
   /// Representa una imágen.
   /// </summary>
   public class Photo : IPublishable
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="Photo"/>.
      /// </summary>
      public Photo()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Identificador único de la imagen.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Identificador de la carpeta contenedora.
      /// </summary>
      public int FolderId { get; set; }

      /// <summary>
      /// Nombre de archivo de la plantilla de presentación.
      /// </summary>
      [Obsolete()]
      public string Template { get; set; }

      /// <summary>
      /// Nombre del archivo (sin ruta).
      /// </summary>
      public string PictureFile { get; set; }

      /// <summary>
      /// Ancho en píxels.
      /// </summary>
      public int PictureWidth { get; set; }

      /// <summary>
      /// Altura en píxels.
      /// </summary>
      public int PictureHeight { get; set; }

      /// <summary>
      /// Nombre del archivo miniatura.
      /// </summary>
      public string ThumbnailFile { get; set; }

      /// <summary>
      /// Ancho en píxels de la imagen miniatura.
      /// </summary>
      public int ThumbnailWidth { get; set; }

      /// <summary>
      /// Alto en píxels de la imagen miniatura.
      /// </summary>
      public int ThumbnailHeight { get; set; }

      /// <summary>
      /// Gets or sets la descripción de la imagen (puede contener tags HTML sólo a nivel de formato o cambio de línea).
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Firma digital del autor.
      /// </summary>
      public string Author { get; set; }

      /// <summary>
      /// Identificador del usuario que colgó la fotografia.
      /// </summary>
      /// <remarks>
      /// Esta propiedad toma el valor 0 si la fotografia no pertenece a un usuario del workspace
      /// </remarks>
      public int UserID { get; set; }

      /// <summary>
      /// Gets or sets el login del propietario del objeto.
      /// </summary>
      /// <remarks>
      /// Por defecto, el propietario del objeto es el usuario creador del mismo.
      /// </remarks>
      public string Owner
      {
         get { return SecurityService.ACCOUNT_SUPER; }
         set { }
      }

      /// <summary>
      /// Fecha de creación del objeto.
      /// </summary>
      public DateTime Created { get; set; }

      /// <summary>
      /// Fecha de la última modificación del objeto.
      /// </summary>
      public DateTime Updated
      {
         get { return this.Created; }
         set {  }
      }

      /// <summary>
      /// Número de veces que se ha mostrado la imagen.
      /// </summary>
      public int Shows { get; set; }

      /// <summary>
      /// Indica si está publicado (visible públicamente) o no
      /// </summary>
      public CmsPublishStatus.PublishStatus Status
      {
         get { return CmsPublishStatus.PublishStatus.Published; }
         set { }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Importa la imagen a la carpeta del servicio, genera la imagen miniatura
      /// y rellena los campso correspondientes de la classe
      /// </summary>
      /// <param name="filename">Nombre y path del archivo original</param>
      public void SetPicture(string filename)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Serializa el objeto a un archivo XML.
      /// </summary>
      /// <param name="filename">Archivo de salida.</param>
      public void Save(string filename)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Desserializa un objeto serializado en un archivo XML y carga los datos en la instancia actual.
      /// </summary>
      /// <param name="filename">Archivo a cargar.</param>
      public void Load(string filename)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Valida los datos del objeto.
      /// </summary>
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
         this.Template = string.Empty;
         this.PictureFile = string.Empty;
         this.PictureWidth = 0;
         this.PictureHeight = 0;
         this.ThumbnailFile = string.Empty;
         this.ThumbnailWidth = 0;
         this.ThumbnailHeight = 0;
         this.Description = string.Empty;
         this.Author = string.Empty;
         this.UserID = 0;
         this.Created = DateTime.Now;
         this.Shows = 0;
      }

      #endregion

   }
}
