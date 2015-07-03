using Cosmo.Cms.Common;
using Cosmo.Security.Auth;
using System;

namespace Cosmo.Cms.Classified
{
   /// <summary>
   /// Implementa un anuncio clasificado.
   /// </summary>
   public class ClassifiedAd
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ClassifiedAd"/>.
      /// </summary>
      public ClassifiedAd()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del objeto.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Devuelve o establece la fecha de creación del objeto.
      /// </summary>
      public DateTime Created { get; set; }

      /// <summary>
      /// Devuelve o establece la fecha de la última modificación del objeto.
      /// </summary>
      public DateTime Updated
      {
         get { return this.Created; }
         set { }
      }

      /// <summary>
      /// Devuelve o establece el identificador de la rama del repositorio Cosmo al que pertenece.
      /// </summary>
      public int FolderID { get; set; }

      /// <summary>
      /// Devuelve o establece el título del anuncio clasificado.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Devuelve o establece el contenido del anuncio (formato HTML).
      /// </summary>
      public string Body { get; set; }

      /// <summary>
      /// Devuelve o establece el identificador de cuenta del autor del anuncio.
      /// </summary>
      public int UserID { get; set; }

      /// <summary>
      /// Devuelve o establece el login del autor del anuncio.
      /// </summary>
      public string UserLogin { get; set; }

      /// <summary>
      /// Devuelve o establece el número de teléfono de contacto.
      /// </summary>
      public string Phone { get; set; }

      /// <summary>
      /// Devuelve o establece la cuenta de correo de contacto.
      /// </summary>
      public string Mail { get; set; }

      /// <summary>
      /// Devuelve o establece la URL que puede acompañar a un anuncio clasificado.
      /// </summary>
      public string URL { get; set; }

      /// <summary>
      /// Devuelve o establece el precio del artículo en venta (o compra).
      /// </summary>
      public decimal Price { get; set; }

      /// <summary>
      /// Devuelve o establece la clave de borrado del anuncio.
      /// </summary>
      [Obsolete()]
      public string DeleteKey { get; set; }

      /// <summary>
      /// Indica si el anuncio está o no eliminado por el usuario.
      /// </summary>
      public bool Deleted { get; set; }

      /// <summary>
      /// Devuelve la plantilla de visualización del objeto.
      /// </summary>
      [Obsolete()]
      public string Template { get; set; }

      /// <summary>
      /// Devuelve o establece el login del propietario del objeto.
      /// </summary>
      /// <remarks>
      /// Por defecto, el propietario del objeto es el usuario creador del mismo.
      /// </remarks>
      public string Owner { get; set; }

      /// <summary>
      /// Indica si está publicado (visible públicamente) o no
      /// </summary>
      public CmsPublishStatus.PublishStatus Status
      {
         get { return CmsPublishStatus.PublishStatus.Published; }
         set { }
      }

      /// <summary>
      /// Devuelve el número de dias que quedan al anuncio en publicación.
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
               return (this.PublishedDays > 30 ? 0 : this.PublishedDays);
            } 
         }
      }

      /// <summary>
      /// Devuelve el número de dias que han transcurrido desde la publicación del anuncio.
      /// </summary>
      public int PublishedDays { get; internal set; }

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
      /// Inicializa la instancia.
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
         this.DeleteKey = string.Empty;
         this.Deleted = false;
         this.Owner = SecurityService.ACCOUNT_SUPER;
         this.Template = "cs_ads_viewer_std.aspx";
      }

      #endregion

   }
}
