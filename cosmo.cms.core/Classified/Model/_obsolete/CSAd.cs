using Cosmo.MetaObjects;
using Cosmo.Security.Auth;
using System;

namespace Cosmo.Cms.Classified.Model
{
   /// <summary>
   /// Implementa un anuncio clasificado.
   /// </summary>
   public class CSAd : IStandardObject
   {
      private int _id;
      private int _usrid;
      private int _folderid;
      private DateTime _created;
      private string _title;
      private string _body;
      private string _usrlogin;
      private string _phone;
      private string _mail;
      private string _url;
      private string _deleteKey;
      private bool _deleted;
      private string _owner;
      private decimal _price;
      private string _template;

      /// <summary>
      /// Devuelve una instancia de CSAd
      /// </summary>
      public CSAd()
      {
         _id = 0;
         _usrid = 0;
         _folderid = 0;
         _created = System.DateTime.Now;
         _title = string.Empty;
         _body = string.Empty;
         _usrlogin = string.Empty;
         _phone = string.Empty;
         _mail = string.Empty;
         _url = string.Empty;
         _price = 0;
         _deleteKey = string.Empty;
         _deleted = false;
         _owner = AuthenticationService.ACCOUNT_SUPER;
         _template = "cs_ads_viewer_std.aspx";
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del objeto.
      /// </summary>
      public int Id
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Devuelve o establece la fecha de creación del objeto.
      /// </summary>
      public DateTime Created
      {
         get { return _created; }
         set { _created = value; }
      }

      /// <summary>
      /// Devuelve o establece la fecha de la última modificación del objeto.
      /// </summary>
      public DateTime Updated
      {
         get { return _created; }
         set { }
      }

      /// <summary>
      /// Devuelve o establece el identificador de la rama del repositorio Cosmo al que pertenece.
      /// </summary>
      public int FolderId
      {
         get { return _folderid; }
         set { _folderid = value; }
      }

      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      public string Body
      {
         get { return _body; }
         set { _body = value; }
      }

      /// <summary>
      /// Devuelve o establece el identificador de cuenta del autor del anuncio.
      /// </summary>
      public int UserID
      {
         get { return _usrid; }
         set { _usrid = value; }
      }

      /// <summary>
      /// Devuelve o establece el login del autor del anuncio.
      /// </summary>
      public string UserLogin
      {
         get { return _usrlogin; }
         set { _usrlogin = value; }
      }

      /// <summary>
      /// Devuelve o establece el número de teléfono de contacto.
      /// </summary>
      public string Phone
      {
         get { return _phone; }
         set { _phone = value; }
      }

      /// <summary>
      /// Devuelve o establece la cuenta de correo de contacto.
      /// </summary>
      public string Mail
      {
         get { return _mail; }
         set { _mail = value; }
      }

      public string URL
      {
         get { return _url; }
         set { _url = value; }
      }

      /// <summary>
      /// Devuelve o establece el precio del artículo en venta (o compra).
      /// </summary>
      public decimal Price
      {
         get { return _price; }
         set { _price = value; }
      }

      [Obsolete()]
      public string DeleteKey
      {
         get { return _deleteKey; }
         set { _deleteKey = value; }
      }

      /// <summary>
      /// Indica si el anuncio está o no eliminado por el usuario.
      /// </summary>
      public bool Deleted
      {
         get { return _deleted; }
         set { _deleted = value; }
      }

      /// <summary>
      /// Devuelve la plantilla de visualización del objeto.
      /// </summary>
      public string Template
      {
         get { return _template; }
         internal set { _template = value; }
      }

      /// <summary>
      /// Devuelve o establece el login del propietario del objeto.
      /// </summary>
      /// <remarks>
      /// Por defecto, el propietario del objeto es el usuario creador del mismo.
      /// </remarks>
      public string Owner
      {
         get { return _owner; }
         set { _owner = value; }
      }

      /// <summary>
      /// Indica si está publicado (visible públicamente) o no
      /// </summary>
      public PublishStatus Status
      {
         get { return PublishStatus.Published; }
         set { }
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

   }
}
