using System;

namespace Cosmo.Security
{
   /// <summary>
   /// Implementa una sesión de usuario.
   /// </summary>
   public class UserSession
   {
      // Internal data declarations
      private string _sessionTiket;
      private Workspace _ws;
      private User _currentUser;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="UserSession"/>. 
      /// </summary>
      /// <param name="workspace">An instance of current Cosmo workspace</param>
      public UserSession(Workspace workspace)
      {
         Destroy();

         _ws = workspace;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve una cadena que contiene el identificador de la sesión de usuario.
      /// </summary>
      public string Tiket
      {
         get { return _sessionTiket; }
      }

      /// <summary>
      /// Gets a new instance of <see cref="User"/> que representa el usuario que ha generado la sesión.
      /// </summary>
      public User User
      {
         get { return _currentUser; }
      }

      /// <summary>
      /// Indica si existe una sessión de usuario iniciada.
      /// </summary>
      public bool IsAuthenticated
      {
         get { return (_currentUser != null); }
      }

      /// <summary>
      /// Gets or sets el workspace asociado a la sesión de usuario.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
         set 
         {
            Destroy();
            _ws = value; 
         }
      }

      /// <summary>
      /// Comprueba la autorización para la sesión autenticada actual.
      /// </summary>
      /// <remarks>
      /// De no existir sesión autenticada, devolverá <em>false</em>.
      /// If user have <c>admin</c> role this check returns <c>true</c> (authorized).
      /// </remarks>
      public bool CheckAuthorization(string role)
      {
         if (!IsAuthenticated)
         {
            return false;
         }

         foreach (Role userRole in _currentUser.Roles)
         {
            if (userRole.Name.Trim().ToLower().Equals(role.ToLower()))
            {
               return true;
            }
            else if (userRole.Name.Trim().ToLower().Equals(Workspace.ROLE_ADMINISTRATOR.ToLower()))
            {
               return true;
            }
         }

         return false;
      }

      /// <summary>
      /// Comprueba la autorización para la sesión autenticada actual.
      /// </summary>
      /// <remarks>
      /// De no existir sesión autenticada, devolverá <em>false</em>.
      /// </remarks>
      public bool CheckAuthorization(params string[] roles)
      {
         if (roles == null || roles.Length <= 0)
         {
            return true;
         }

         foreach (string role in roles)
         {
            if (CheckAuthorization(role))
            {
               return true;
            }
         }

         return false;
      }

      #endregion

      #region Methods

      /// <summary>
      /// Genera una nueva sesión de usuario.
      /// </summary>
      /// <param name="login">Login del usuario.</param>
      /// <param name="password">Contraseña del usuario.</param>
      /// <returns>Una instancia de <see cref="User"/> que representa el usuario autenticado actualmente.</returns>
      [Obsolete()]
      public User GenerateSession(string login, string password)
      {
         // Destruye cualquier sessión previa.
         Destroy();

         // Obtiene el perfil del usuario
         _currentUser = _ws.SecurityService.Autenticate(login, password);

         return _currentUser;
      }

      /// <summary>
      /// Genera una nueva sesión de usuario.
      /// </summary>
      /// <param name="user">Una instancia de <see cref="User"/> que representa el usuario para el que se dese agenerar la sesión.</param>
      /// <returns>Una instancia de <see cref="User"/> que representa el usuario autenticado actualmente.</returns>
      public User GenerateSession(User user)
      {
         // Destruye cualquier sessión previa.
         Destroy();

         // Obtiene el perfil del usuario
         _currentUser = user;

         return _currentUser;
      }

      /// <summary>
      /// Destruye la sesión de usuario.
      /// </summary>
      public void Destroy()
      {
         _currentUser = null;
         _sessionTiket = string.Empty;
      }

      /// <summary>
      /// Autentica un usuario y genera un tiket de sessión.
      /// </summary>
      /// <param name="login">Login del usuario</param>
      /// <param name="password">Contraseña del usuario.</param>
      /// <returns>Una cadena que identifica la sessión o null si se produce un error.</returns>
      /// <remarks>
      /// Este método se debe usar para la autenticación mediante ticket de sesión. La utenticación mediante ticket no precisa tener
      /// la sesión almacenada en memória (una variable en WinForms o en la Session para ASP.NET).
      /// </remarks>
      public string GenerateSessionTicket(string login, string password)
      {
         // Destruye cualquier sessión previa.
         Destroy();

         // Obtiene el perfil del usuario
         _currentUser = _ws.SecurityService.Autenticate(login, password);

         // Genera la ID de sesión y la devuelve
         string baseTicket = _currentUser.ID + "|" + _currentUser.Login + "|" + DateTime.Now.AddMinutes(60).ToFileTime();
         _sessionTiket = Cosmo.Security.Cryptography.Cryptography.Encrypt(baseTicket, _ws.SecurityService.EncriptionKey, true);

         return _sessionTiket;
      }

      /// <summary>
      /// Verifica que una sesión tenga validez.
      /// </summary>
      /// <param name="sessionTiket">Identificador de sesión actual.</param>
      /// <remarks>
      /// Este método se debe usar para la autenticación mediante ticket de sesión. La utenticación mediante ticket no precisa tener
      /// la sesión almacenada en memória (una variable en WinForms o en la Session para ASP.NET).
      /// </remarks>
      public void ValidateSessionTicket(string sessionTiket)
      {
         // Inicializaciones
         int id = 0;
         long time = 0;

         // Desencripta el ticket proporcionado
         string[] ticketParams = Cosmo.Security.Cryptography.Cryptography.Decrypt(sessionTiket, _ws.SecurityService.EncriptionKey, true).Split('|');

         if (ticketParams.Length != 3)
         {
            _sessionTiket = string.Empty;
            _currentUser = null;
            throw new InvalidSessionException("La sesión no es válida.");
         }

         // Comprueba el código de usuario
         if (!int.TryParse(ticketParams[0], out id)) id = 0;
         if (id != _currentUser.ID)
         {
            _sessionTiket = string.Empty;
            _currentUser = null;
            throw new InvalidSessionException("La sesión no es válida.");
         }

         // Comprueba el login de usuario
         if (_currentUser.Login.ToLower().Equals(ticketParams[1].Trim().ToLower()))
         {
            _sessionTiket = string.Empty;
            _currentUser = null;
            throw new InvalidSessionException("La sesión no es válida.");
         }

         // Comprueba la validez temporal de la sesión
         if (!long.TryParse(ticketParams[2], out time)) time = 0;
         if (DateTime.Now > new DateTime(time))
         {
            _sessionTiket = string.Empty;
            _currentUser = null;
            throw new InvalidSessionException("La sesión de usuario ha caducado. Debe identificarse de nuevo.");
         }
      }

      #endregion

   }
}
