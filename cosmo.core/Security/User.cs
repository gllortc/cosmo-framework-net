using Cosmo.Data.ORM;
using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Cosmo.Security
{
   /// <summary>
   /// Implementa la cuenta de un usuario del workspace
   /// </summary>
   [MappingObject(DatabaseTableName = "USERS",
                  FormStyle = OrmEngine.OrmFormStyle.Standard)]
   [MappingFieldGroup(ID = "basData", 
                      Title = "1. Datos básicos de la cuenta",
                      Description = "Los siguientes datos son privados (nadie puede verlos excepto usted) y son los datos básicos de suscripción. Tenga presente que el proceso de suscripción necesita una cuenta de correo válida para poder ser completado.")]
   [MappingFieldGroup(ID = "pubData", 
                      Title = "2. Datos públicos",
                      Description = "Los siguientes datos son visibles para todos los usuarios registrados en " + OrmEngine.TAG_WORKSPACE_NAME + ".")]
   [MappingFieldGroup(ID = "adsData", 
                      Title = "3. Datos de contacto (anuncios clasificados)",
                      Description = "Los siguientes datos no son visibles públicamente y sólo se usan para rellenar de forma automática datos al crear un nuevo anuncio clasificado. Si no va a usar el servicio (o quizá lo use más adelante) puede dejarlos en blanco.")]
   [MappingFieldGroup(ID = "opsData", 
                      Title = "4. Opciones de notificación")]
   public class User
   {
      // Internal data declarations
      private int _id;
      private string _login;
      private string _mail;
      private string _password;
      private string _name;
      private string _city;
      private int _countryid;
      private string _phone;
      private string _mail2;
      private string _description;
      private bool _canrecvinternalmsg;
      private bool _canrecvexternalmsg;
      private bool _canrecvprivatemsgnotify;
      private bool _disableprivatemsg;
      private UserStatus _status;
      private DateTime _created;
      private DateTime _lastlogon;
      private int _logoncount;
      private string _owner;
      private List<UserRelation> _relations;
      private List<Role> _roles;

      #region Enumerations

      /// <summary>
      /// Estados de usuario
      /// </summary>
      public enum UserStatus : int
      {
         /// <summary>Todos los usuarios (no usar este valor en código)</summary>
         AllStates = -1,
         /// <summary>Inhabilitado</summary>
         Disabled = 0,
         /// <summary>Pendiente de verificación de la cuenta de correo principal</summary>
         NotVerified = 1,
         /// <summary>Activa</summary>
         Enabled = 2,
         /// <summary>En cuarentena (sirve para marcar las cuentas pendientes de revisión por causas de seguridad)</summary>
         SecurityBloqued = 3
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="User"/>.
      /// </summary>
      public User()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Identificador de la cuenta.
      /// </summary>
      [MappingField(FieldName = Cosmo.Workspace.PARAM_USER_ID,
                    DataType = MappingDataType.Hidden,
                    IsPrimaryKey = true)]
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Login
      /// </summary>
      [MappingField(FieldName = "lo",
                    Label = "Nombre de usuario",
                    DataType = MappingDataType.Login,
                    GroupID = "basData",
                    Required = true)]
      public string Login
      {
         get { return _login; }
         set { _login = value; }
      }

      /// <summary>
      /// Contraseña
      /// </summary>
      [MappingField(FieldName = "pw",
                    Label = "Contraseña",
                    DataType = MappingDataType.Password,
                    GroupID = "basData",
                    Required = true,
                    RewriteRequired = true)]
      public string Password
      {
         get { return _password; }
         set { _password = value; }
      }

      /// <summary>
      /// Cuenta de correo electrónico principal
      /// </summary>
      [MappingField(FieldName = "ml",
                    Label = "Correo electrónico",
                    DataType = MappingDataType.Mail,
                    GroupID = "basData",
                    Required = true)]
      public string Mail
      {
         get { return _mail; }
         set { _mail = value; }
      }

      /// <summary>
      /// Nombre (y apellidos) del usuario
      /// </summary>
      [MappingField(FieldName = "na",
                    Label = "Nombre completo",
                    Required = true,
                    DataType = MappingDataType.String,
                    GroupID = "pubData")]
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Descripción (texto libre)
      /// </summary>
      [MappingField(FieldName = "de",
                    Label = "Descripción",
                    DataType = MappingDataType.MultilineString,
                    GroupID = "pubData")]
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Ciudad
      /// </summary>
      [MappingField(FieldName = "ci",
                    Label = "Ciudad",
                    DataType = MappingDataType.String,
                    GroupID = "pubData")]
      public string City
      {
         get { return _city; }
         set { _city = value; }
      }

      /// <summary>
      /// Identificador del pais al que pertenece
      /// </summary>
      [MappingField(FieldName = "co",
                    Label = "Pais",
                    Required = true,
                    DataType = MappingDataType.Integer,
                    DataListID = "country",
                    GroupID = "pubData")]
      public int CountryID
      {
         get { return _countryid; }
         set { _countryid = value; }
      }

      /// <summary>
      /// Cuenta de correo electrónico alternativa
      /// </summary>
      [MappingField(FieldName = "ma",
                    Label = "Correo electrónico de contacto",
                    DataType = MappingDataType.Mail,
                    GroupID = "adsData")]
      public string MailAlternative
      {
         get { return _mail2; }
         set { _mail2 = value; }
      }

      /// <summary>
      /// Teléfono de contacto
      /// </summary>
      [MappingField(FieldName = "ph",
                    Label = "Teléfono de contacto",
                    DataType = MappingDataType.String,
                    GroupID = "adsData")]
      public string Phone
      {
         get { return _phone; }
         set { _phone = value; }
      }

      /// <summary>
      /// Indica si desea recibir notificaciones procedentes del Workspace.
      /// </summary>
      [MappingField(FieldName = "rim",
                    Label = "Recibir ofertas y noticias del portal",
                    DataType = MappingDataType.Boolean,
                    GroupID = "opsData")]
      public bool CanReceiveInternalMessages
      {
         get { return _canrecvinternalmsg; }
         set { _canrecvinternalmsg = value; }
      }

      /// <summary>
      /// Indica si desea recibir notificaciones procedentes de terceras partes o aplicaciones.
      /// </summary>
      [MappingField(FieldName = "rem",
                    Label = "Recibir ofertas y noticias de terceras empresas",
                    DataType = MappingDataType.Boolean,
                    GroupID = "opsData")]
      public bool CanReceiveExternalMessages
      {
         get { return _canrecvexternalmsg; }
         set { _canrecvexternalmsg = value; }
      }

      /// <summary>
      /// Indica si desea recibir notificaciones procedentes de terceras partes o aplicaciones.
      /// </summary>
      [MappingField(FieldName = "rpm",
                    Label = "Recibir notificación de mensajes privados",
                    DataType = MappingDataType.Boolean,
                    GroupID = "opsData")]
      public bool CanReceivePrivateMessagesNotify
      {
         get { return _canrecvprivatemsgnotify; }
         set { _canrecvprivatemsgnotify = value; }
      }

      /// <summary>
      /// Indica si el usuario tiene desactivada la opción de mensajes privados.
      /// </summary>
      public bool DisablePrivateMessages
      {
         get { return _disableprivatemsg; }
         set { _disableprivatemsg = value; }
      }

      /// <summary>
      /// Estado de la cuenta.
      /// </summary>
      public UserStatus Status
      {
         get { return _status; }
         set { _status = value; }
      }

      /// <summary>
      /// Lista de contactos relacionados con el usuario dentro del mismo workspace.
      /// </summary>
      public List<UserRelation> Relations
      {
         get { return _relations; }
         set { _relations = value; }
      }

      /// <summary>
      /// Indica la fecha de creación de la cuenta de usuario.
      /// </summary>
      public DateTime Created
      {
         get { return _created; }
         set { _created = value; }
      }

      /// <summary>
      /// Indica la última vez que la cuenta se validó.
      /// </summary>
      public DateTime LastLogon
      {
         get { return _lastlogon; }
         set { _lastlogon = value; }
      }

      /// <summary>
      /// Idica el número de veces que el usuario se ha validado
      /// </summary>
      public int LogonCount
      {
         get { return _logoncount; }
         set { _logoncount = value; }
      }

      /// <summary>
      /// Propietario del objeto
      /// </summary>
      public string Owner
      {
         get { return _owner; }
         set { _owner = value; }
      }

      /// <summary>
      /// Devuelve la lista de roles del usuario.
      /// </summary>
      public List<Role> Roles
      {
         get { return _roles; }
         internal set { _roles = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve un nombre para mostrar del usuario.
      /// </summary>
      public string GetDisplayName()
      {
         if (string.IsNullOrEmpty(_name))
         {
            return _login;
         }

         return _name;
      }

      /// <summary>
      /// Devuelve la imagen correspondiente al avatar lista para ser incluyda en código XHTML.
      /// P. ej. "images/usr_12344.png"
      /// </summary>
      public string GetAvatarImage()
      {
         return "images/user_no_avatar.gif";
      }

      /// <summary>
      /// Indica si el usuario posee un determinado rol.
      /// </summary>
      /// <param name="roleName">Nombre del rol.</param>
      /// <returns><c>true</c> si el usuario posee el rol o <c>false</c> en cualquier otro caso.</returns>
      public bool IsInRole(string roleName)
      {
         foreach (Role role in _roles)
         {
            if (role.Name.Trim().ToLower().Equals(roleName.Trim().ToLower()))
            {
               return true;
            }
         }

         return false;
      }

      /// <summary>
      /// Devuelve información pública del usuario en formato XML
      /// </summary>
      /// <param name="stream">Una instancia de Stream dónde se escribirá el código XML</param>
      /// <remarks>
      /// El formato del archivo XML generado es el siguiente:
      /// &lt;code&gt;
      /// &lt;contentserver&gt;
      ///     &lt;userinfo id          = "_ContactID_"
      ///               login       = "_Login_"
      ///               name        = "_Name_"
      ///               city        = "_City_"
      ///               country     = "_CountryName_"
      ///               description = "_Description_"
      ///               created     = "_DateCreated_" /&gt;
      /// &lt;/locations&gt;
      /// &lt;/code&gt;
      /// </remarks>
      public void ToXml(Stream stream)
      {
         try
         {
            // Create a new XmlTextWriter instance
            XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);

            // Inicia el bloque LOCATIONS
            writer.WriteStartDocument();
            writer.WriteStartElement("contentserver");

            // Escribe los elementos LOCATION
            writer.WriteStartElement("userinfo");
            writer.WriteAttributeString("id", this.ID.ToString());
            writer.WriteAttributeString("login", this.Login);
            writer.WriteAttributeString("name", this.Name);
            writer.WriteAttributeString("city", this.City);
            writer.WriteAttributeString("country", this.CountryID.ToString());
            writer.WriteAttributeString("description", this.Description);
            writer.WriteAttributeString("created", this.Created.ToString("dd/MM/yyyy hh:mm"));
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Permite validar los datos de la instancia.
      /// </summary>
      /// <returns><c>true</c> si los datos que contiene la instancia son válidos para describir el usuario o <c>false</c> en cualquir otro caso.</returns>
      public bool IsValid()
      {
         bool isValid = true;

         isValid = isValid & !string.IsNullOrWhiteSpace(this.Login);
         isValid = isValid & !string.IsNullOrWhiteSpace(this.Mail);

         isValid = isValid & AuthenticationService.IsValidLogin(this.Login);

         return isValid;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _id = 0;
         _login = string.Empty;
         _mail = string.Empty;
         _password = string.Empty;
         _name = string.Empty;
         _city = string.Empty;
         _countryid = 0;
         _phone = string.Empty;
         _mail2 = string.Empty;
         _description = string.Empty;
         _canrecvinternalmsg = false;
         _canrecvexternalmsg = false;
         _canrecvprivatemsgnotify = false;
         _disableprivatemsg = false;
         _status = UserStatus.Disabled;
         _created = DateTime.Now;
         _lastlogon = DateTime.Now;
         _logoncount = 0;
         _owner = string.Empty;
         _roles = new List<Role>();
      }

      #endregion

   }
}
