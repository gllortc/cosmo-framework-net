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
   public class User
   {

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

      /// <summary>
      /// Devuelve una instancia de <see cref="User"/>.
      /// </summary>
      public User()
      {
         Initialize();
      }

      /// <summary>
      /// Identificador de la cuenta.
      /// </summary>
      [MappingField(Cosmo.Workspace.PARAM_USER_ID)]
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Login
      /// </summary>
      [MappingField("lo")]
      public string Login
      {
         get { return _login; }
         set { _login = value; }
      }

      /// <summary>
      /// Contraseña
      /// </summary>
      public string Password
      {
         get { return _password; }
         set { _password = value; }
      }

      /// <summary>
      /// Cuenta de correo electrónico principal
      /// </summary>
      public string Mail
      {
         get { return _mail; }
         set { _mail = value; }
      }

      /// <summary>
      /// Nombre (y apellidos) del usuario
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Ciudad
      /// </summary>
      public string City
      {
         get { return _city; }
         set { _city = value; }
      }

      /// <summary>
      /// Identificador del pais al que pertenece
      /// </summary>
      public int CountryID
      {
         get { return _countryid; }
         set { _countryid = value; }
      }

      /// <summary>
      /// Teléfono de contacto
      /// </summary>
      public string Phone
      {
         get { return _phone; }
         set { _phone = value; }
      }

      /// <summary>
      /// Cuenta de correo electrónico alternativa
      /// </summary>
      public string MailAlternative
      {
         get { return _mail2; }
         set { _mail2 = value; }
      }

      /// <summary>
      /// Descripción (texto libre)
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Indica si desea recibir notificaciones procedentes del Workspace.
      /// </summary>
      public bool CanReceiveInternalMessages
      {
         get { return _canrecvinternalmsg; }
         set { _canrecvinternalmsg = value; }
      }

      /// <summary>
      /// Indica si desea recibir notificaciones procedentes de terceras partes o aplicaciones.
      /// </summary>
      public bool CanReceiveExternalMessages
      {
         get { return _canrecvexternalmsg; }
         set { _canrecvexternalmsg = value; }
      }

      /// <summary>
      /// Indica si desea recibir notificaciones procedentes de terceras partes o aplicaciones.
      /// </summary>
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
         catch (Exception ex)
         {
            throw ex;
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
   }
}
