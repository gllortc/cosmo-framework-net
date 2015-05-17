using System;

namespace Cosmo.Security
{
   /// <summary>
   /// Implementa una clase para especificar el filtro de búsqueda de usuarios.
   /// </summary>
   public class UserSearchCriteria
   {
      // Internal data declaration
      private int _country = 0;
      private DateTime _fromlastlogon = DateTime.MinValue;
      private DateTime _tolastlogon = DateTime.MinValue;
      private bool _external = false;
      private bool _internal = false;
      private string _mail = string.Empty;
      private string _city = string.Empty;

      #region Properties

      /// <summary>
      /// Ciudad.
      /// </summary>
      public string City
      {
         get { return _city; }
         set { _city = value; }
      }

      /// <summary>
      /// Identificador del pais.
      /// </summary>
      public int CountryId
      {
         get { return _country; }
         set { _country = value; }
      }

      /// <summary>
      /// Desde el último acceso.
      /// </summary>
      public DateTime FromLastLogon
      {
         get { return _fromlastlogon; }
         set { _fromlastlogon = value; }
      }

      /// <summary>
      /// Hasta el último acceso.
      /// </summary>
      public DateTime ToLastLogon
      {
         get { return _tolastlogon; }
         set { _tolastlogon = value; }
      }

      /// <summary>
      /// Permite el envio de comunicados externos al workspace.
      /// </summary>
      public bool ExternalMessages
      {
         get { return _external; }
         set { _external = value; }
      }

      /// <summary>
      /// Permite el envio de comunicados internos del workspace.
      /// </summary>
      public bool InternalMessages
      {
         get { return _internal; }
         set { _internal = value; }
      }

      /// <summary>
      /// Servidor de correo.
      /// </summary>
      public string MailPart
      {
         get { return _mail; }
         set { _mail = value; }
      }

      #endregion

   }
}
