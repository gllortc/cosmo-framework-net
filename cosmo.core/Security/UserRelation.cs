using System;

namespace Cosmo.Security
{
   /// <summary>
   /// Implementa una relación entre dos usuarios.
   /// </summary>
   public class UserRelation
   {
      // Internal data declarations
      int _fromUserID;
      int _toUserID;
      UserRelationStatus _status;
      DateTime _created;
      DateTime _responded;

      #region Enumerations

      /// <summary>
      /// Estados de las relaciones entre usuaruios
      /// </summary>
      public enum UserRelationStatus : int
      {
         /// <summary>El usuario orígen ha solicitado amistad con el destinatario sin obtener aun respuesta</summary>
         Unverified = 0,
         /// <summary>La relación ha sido aceptada por el destinatario</summary>
         Verified = 1,
         /// <summary>La petición de relación ha sido rechazada por el destinatario</summary>
         Rejected = 2
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="UserRelation"/>.
      /// </summary>
      public UserRelation()
      {
         _fromUserID = 0;
         _toUserID = 0;
         _created = DateTime.Now;
         _responded = DateTime.MinValue;
         _status = UserRelationStatus.Unverified;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Identificador del usuario que solicita relación con el destinatario.
      /// </summary>
      public int FromUserID
      {
         get { return _fromUserID; }
         set { _fromUserID = value; }
      }

      /// <summary>
      /// Identificador del usuario que recibe la solicitud de relación.
      /// </summary>
      public int ToUserID
      {
         get { return _toUserID; }
         set { _toUserID = value; }
      }

      /// <summary>
      /// Estado de la petición de relación entre usuarios.
      /// </summary>
      public UserRelationStatus Status
      {
         get { return _status; }
         set { _status = value; }
      }

      /// <summary>
      /// Fecha de la solicitud de relación.
      /// </summary>
      public DateTime Created
      {
         get { return _created; }
         set { _created = value; }
      }

      /// <summary>
      /// Fecha de la respuesta por parte del destinatario.
      /// </summary>
      public DateTime Updated
      {
         get { return _responded; }
         set { _responded = value; }
      }

      #endregion

   }
}
