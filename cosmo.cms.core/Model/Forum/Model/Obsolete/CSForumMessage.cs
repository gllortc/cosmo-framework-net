using System;
using Cosmo.Cms.Forums.Model.Parsers;

namespace Cosmo.Cms.Forums.Model.Obsolete
{

   /// <summary>
   /// Implementa una clase para gestionar los canales de los foros.
   /// </summary>
   public class CSForumMessage
   {
      private int _ID;
      private int _userID;
      private int _forumID;
      private int _parentMessageID;
      private string _name;
      private string _mail;
      private string _title;
      private string _body;
      private string _IP;
      private DateTime _date;
      private bool _closed;
      private bool _bbcodes;

      public CSForumMessage()
      {
         _ID = 0;
         _userID = 0;
         _forumID = 0;
         _parentMessageID = 0;
         _name = string.Empty;
         _mail = string.Empty;
         _title = string.Empty;
         _body = string.Empty;
         _IP = string.Empty;
         _date = DateTime.Now;
         _closed = true;
         _bbcodes = false;
      }

      #region Properties

      /// <summary>
      /// Identificador único del mensaje.
      /// </summary>
      public int ID
      {
         get { return _ID; }
         set { _ID = value; }
      }

      /// <summary>
      /// Identificador del usuario.
      /// </summary>
      public int UserID
      {
         get { return _userID; }
         set { _userID = value; }
      }

      /// <summary>
      /// Identificador del foro (canal) al que pertenece.
      /// </summary>
      public int ForumID
      {
         get { return _forumID; }
         set { _forumID = value; }
      }

      /// <summary>
      /// Identificador del mensaje al que responde este mensaje.
      /// </summary>
      /// <remarks>
      /// Si es 0 indica que es un enunciado.
      /// </remarks>
      public int ParentMessageID
      {
         get { return _parentMessageID; }
         set { _parentMessageID = value; }
      }

      /// <summary>
      /// Login del usuario.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Correo del usuario.
      /// </summary>
      [Obsolete("Desde que ContentServer obliga a participar sólo a usuarios registrados, este dato ya lo tiene la cuenta de usuario.")]
      public string Mail
      {
         get { return _mail; }
         set { _mail = value; }
      }

      /// <summary>
      /// Título del mensaje.
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Texto del mensaje.
      /// </summary>
      public string Body
      {
         get { return _body; }
         set { _body = value; }
      }

      /// <summary>
      /// Dirección IP desde dónde se envió el mensaje.
      /// </summary>
      public string IP
      {
         get { return _IP; }
         set { _IP = value; }
      }

      /// <summary>
      /// Fecha/hora de la inserción del mensaje en el foro.
      /// </summary>
      public DateTime Date
      {
         get { return _date; }
         set { _date = value; }
      }

      /// <summary>
      /// Indica si el thread al que pertenece está cerrado.
      /// </summary>
      public bool ThreadClosed
      {
         get { return _closed; }
         set { _closed = value; }
      }

      /// <summary>
      /// Indica si el mensaje contiene BBCodes o es texto llano
      /// </summary>
      public bool BBCodes
      {
         get { return _bbcodes; }
         set { _bbcodes = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Traduce el cuerpo del mensaje a código XHTML.
      /// </summary>
      /// <returns>Una cadena XHTML a enviar al cliente.</returns>
      public string GetHTMLBody()
      {
         // return CSForumMessageParser.Format(_body, _bbcodes);
         if (_bbcodes)
         {
            return CSForumBBCodeParser.Parse(_body);
         }
         else
         {
            return CSForumPlainTextParser.Parse(_body);
         }
      }

      #endregion

   }
}
