using Cosmo.Utils.Html;
using System;

namespace Cosmo.Cms.Forums
{

   /// <summary>
   /// Implementa una clase para gestionar los canales de los foros.
   /// </summary>
   public class ForumMessage
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ForumMessage"/>.
      /// </summary>
      public ForumMessage()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Identificador único del mensaje.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Identificador del usuario.
      /// </summary>
      public int UserID { get; set; }

      /// <summary>
      /// Identificador del foro (canal) al que pertenece.
      /// </summary>
      public int ForumID { get; set; }

      /// <summary>
      /// Identificador del mensaje al que responde este mensaje.
      /// </summary>
      /// <remarks>
      /// Si es 0 indica que es un enunciado.
      /// </remarks>
      public int ParentMessageID { get; set; }

      /// <summary>
      /// Login del usuario.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Correo del usuario.
      /// </summary>
      [Obsolete("Desde que ContentServer obliga a participar sólo a usuarios registrados, este dato ya lo tiene la cuenta de usuario.")]
      public string Mail { get; set; }

      /// <summary>
      /// Título del mensaje.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Texto del mensaje.
      /// </summary>
      public string Body { get; set; }

      /// <summary>
      /// Dirección IP desde dónde se envió el mensaje.
      /// </summary>
      public string IP { get; set; }

      /// <summary>
      /// Fecha/hora de la inserción del mensaje en el foro.
      /// </summary>
      public DateTime Date { get; set; }

      /// <summary>
      /// Indica si el thread al que pertenece está cerrado.
      /// </summary>
      public bool ThreadClosed { get; set; }

      /// <summary>
      /// Indica si el mensaje contiene BBCodes o es texto llano
      /// </summary>
      public bool BBCodes { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Traduce el cuerpo del mensaje a código XHTML.
      /// </summary>
      /// <returns>Una cadena XHTML a enviar al cliente.</returns>
      public string GetHTMLBody()
      {
         Formatter bbParser = new Formatter();
         return bbParser.bbCodeParser(this.Body);
      }


      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.ID = 0;
         this.UserID = 0;
         this.ForumID = 0;
         this.ParentMessageID = 0;
         this.Name = string.Empty;
         this.Mail = string.Empty;
         this.Title = string.Empty;
         this.Body = string.Empty;
         this.IP = string.Empty;
         this.Date = DateTime.Now;
         this.ThreadClosed = true;
         this.BBCodes = false;
      }

      #endregion

   }
}
