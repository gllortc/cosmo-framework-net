using Cosmo.Security.Auth;
using System;

namespace Cosmo.Cms.Model.Ads
{
   /// <summary>
   /// Implementa un anuncio clasificado.
   /// </summary>
   public class Ad
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="Ad"/>.
      /// </summary>
      public Ad()
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
      /// Gets or sets la fecha de creación del objeto.
      /// </summary>
      public DateTime Created { get; set; }

      /// <summary>
      /// Gets or sets la fecha de la última modificación del objeto.
      /// </summary>
      public DateTime Updated
      {
         get { return this.Created; }
         set { }
      }

      /// <summary>
      /// Gets or sets el identificador de la rama del repositorio Cosmo al que pertenece.
      /// </summary>
      public int FolderID { get; set; }

      /// <summary>
      /// Gets or sets el título del anuncio clasificado.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or sets el contenido del anuncio (formato HTML).
      /// </summary>
      public string Body { get; set; }

      /// <summary>
      /// Gets or sets el identificador de cuenta del autor del anuncio.
      /// </summary>
      public int UserID { get; set; }

      /// <summary>
      /// Gets or sets el login del autor del anuncio.
      /// </summary>
      public string UserLogin { get; set; }

      /// <summary>
      /// Gets or sets el número de teléfono de contacto.
      /// </summary>
      public string Phone { get; set; }

      /// <summary>
      /// Gets or sets la cuenta de correo de contacto.
      /// </summary>
      public string Mail { get; set; }

      /// <summary>
      /// Gets or sets la URL que puede acompañar a un anuncio clasificado.
      /// </summary>
      public string URL { get; set; }

      /// <summary>
      /// Gets or sets el precio del artículo en venta (o compra).
      /// </summary>
      public decimal Price { get; set; }

      /// <summary>
      /// Gets or sets a boolean indicating if the ad is unpublished.
      /// </summary>
      public bool Deleted { get; set; }

      /// <summary>
      /// Gets or sets the login of the ad owner.
      /// </summary>
      /// <remarks>
      /// By default, the ad owner is the author.
      /// </remarks>
      public string Owner { get; set; }

      /// <summary>
      /// Gets or sets the publishing status of the ad.
      /// </summary>
      public CmsPublishStatus.PublishStatus Status
      {
         get { return (IsPublished ? CmsPublishStatus.PublishStatus.Published : CmsPublishStatus.PublishStatus.Unpublished); }
         set { }
      }

      /// <summary>
      /// Gets the number of days remaining to unpublish the ad.
      /// </summary>
      public int RemainingDays
      {
         get 
         {
            if (Deleted)
            {
               return 0;
            }
            else
            {
               return (this.PublishedDays > 30 ? 0 : 30 - this.PublishedDays);
            } 
         }
      }

      /// <summary>
      /// Gets the number of days that the ad is published.
      /// </summary>
      public int PublishedDays { get; internal set; }

      /// <summary>
      /// Gets a boolean indicating if the ad is currently published (visible).
      /// </summary>
      public bool IsPublished 
      {
         get { return Deleted ? false : (PublishedDays < 30); }
      }

      #endregion

      #region Methods

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
         this.PublishedDays = 0;
         this.ID = 0;
         this.UserID = 0;
         this.FolderID = 0;
         this.Created = System.DateTime.Now;
         this.Title = string.Empty;
         this.Body = string.Empty;
         this.UserLogin = string.Empty;
         this.Phone = string.Empty;
         this.Mail = string.Empty;
         this.URL = string.Empty;
         this.Price = 0;
         this.Deleted = false;
         this.Owner = SecurityService.ACCOUNT_SUPER;
      }

      #endregion

   }
}
