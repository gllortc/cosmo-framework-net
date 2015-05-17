using System;
using System.Collections.Generic;
using System.Web;

namespace Cosmo.Cms.Forums
{

   #region Enumerations

   /// <summary>
   /// Orden de recuperación de los mensajes de un thread
   /// </summary>
   public enum ThreadMessagesOrder : int
   {
      Ascending = 0,
      Descending = 1
   }

   #endregion

   /// <summary>
   /// Implementa una clase para gestionar los canales de los foros.
   /// </summary>
   public class ForumThread
   {
      private int _threadId;
      private int _forumId;
      private string _title;
      private string _usrname;
      private int _usrid;
      private DateTime _created;
      private DateTime _lastReply;
      private int _msgCount;
      private bool _closed;
      private string _channelName;
      private List<ForumMessage> _messages;

      public ForumThread()
      {
         _threadId = 0;
         _forumId = 0;
         _title = string.Empty;
         _msgCount = 0;
         _created = DateTime.Now;
         _lastReply = DateTime.Now;
         _closed = true;
         _channelName = string.Empty;
         _messages = new List<ForumMessage>();
      }

      #region Settings

      /// <summary>
      /// Identificador del hilo
      /// </summary>
      /// <remarks>Corresponde la Id del mensaje superior</remarks>
      public int ID
      {
         get { return _threadId; }
         set { _threadId = value; }
      }

      /// <summary>
      /// Identificador del foro (canal) al que pertenece
      /// </summary>
      public int ForumID
      {
         get { return _forumId; }
         set { _forumId = value; }
      }

      /// <summary>
      /// Título del hilo
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Fecha/Hora de creación del hilo
      /// </summary>
      public DateTime Created
      {
         get { return _created; }
         set { _created = value; }
      }

      /// <summary>
      /// Fecha/Hora del último mensaje recibido
      /// </summary>
      public DateTime LastReply
      {
         get { return _lastReply; }
         set { _lastReply = value; }
      }

      /// <summary>
      /// Número de respuestas en el hilo (no se cuenta el mensaje inicial, sólo respuestas)
      /// </summary>
      public int MessageCount
      {
         get { return _msgCount; }
         set { _msgCount = value; }
      }

      /// <summary>
      /// Login del autor
      /// </summary>
      public string AuthorName
      {
         get { return _usrname; }
         set { _usrname = value; }
      }

      /// <summary>
      /// Id del autor
      /// </summary>
      public int AuthorID
      {
         get { return _usrid; }
         set { _usrid = value; }
      }

      /// <summary>
      /// Indica si el hilo está cerrado
      /// </summary>
      public bool Closed
      {
         get { return _closed; }
         set { _closed = value; }
      }

      /// <summary>
      /// Devuelve o establece el nombre del canal.
      /// </summary>
      /// <remarks>
      /// Este campo sólo es informativo y no tiene nionguna función a nivel de base de datos.
      /// </remarks>
      public string ChannelName
      {
         get { return _channelName; }
         set { _channelName = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de mensajes del hilo.
      /// </summary>
      public List<ForumMessage> Messages
      {
         get { return _messages; }
         set { _messages = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene el modo de ordenación de los mensajes
      /// </summary>
      /// <param name="request">Instancia de HttpRequest del servidor</param>
      /// <param name="response">Instancia de HttpResponse del servidor</param>
      /// <returns>El tipo de ordenación a aplicar</returns>
      public ThreadMessagesOrder GetMessageOrder(HttpRequest request, HttpResponse response)
      {
         int torder = (int)ThreadMessagesOrder.Ascending;

         try
         {
            if (!int.TryParse(request[ForumsDAO.PARAM_ORDER], out torder))
            {
               if (request.Cookies["cs.forum"] != null)    // No ha podido recuperar el parámetro ex intenta recuperar la cookie
               {
                  if (request.Cookies["cs.forum"]["msgs.order"] != null)
                     if (!int.TryParse(request.Cookies["cs.forum"]["msgs.order"], out torder))
                        torder = (int)ThreadMessagesOrder.Ascending;
               }
               else
               {
                  torder = (int)ThreadMessagesOrder.Ascending;
               }
            }

            // Memoriza el tipo de ordenación obtenido de los parámetros
            HttpCookie cookie = new HttpCookie("cs.forum");
            cookie["msgs.order"] = torder.ToString();
            cookie.Expires = DateTime.Now.AddYears(1);
            response.Cookies.Add(cookie);

            return (ThreadMessagesOrder)torder;
         }
         catch
         {
            return ThreadMessagesOrder.Ascending;
         }
      }

      #endregion

   }
}
