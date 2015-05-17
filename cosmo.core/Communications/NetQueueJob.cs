using System;
using MailBee.Mime;

namespace Cosmo.Communications
{

   /// <summary>
   /// Representa un trabajo de cola de salida del workspace.
   /// </summary>
   public class NetQueueJob
   {
      private string _id;
      private DateTime _timestamp;
      private int _retry;
      private DateTime _lastretry;
      private MailPriority _priority;
      private MWNetQueueType _type;
      private string _owner;

      /// <summary>Extensión de los objetos en cola (archivos).</summary>
      public const string QUEUE_FILE_EXTENSION = ".mwq";

      /// <summary>
      /// Devuelve una instancia de NetQueueJob
      /// </summary>
      public NetQueueJob()
      {
         _id = "";
         _timestamp = DateTime.Now;
         _retry = 0;
         _priority = MailPriority.Normal;
         _type = MWNetQueueType.Mail;
         _owner = "SYS";
      }

      #region Properties

      /// <summary>
      /// Identificador del elemento de la cola de salida
      /// </summary>
      public string ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Fecha/hora en la que se añadió en trabajo a la cola
      /// </summary>
      public DateTime TimeStamp
      {
         get { return _timestamp; }
         set { _timestamp = value; }
      }

      /// <summary>
      /// Intento de salida (0 al insertar el elemento a la cola)
      /// </summary>
      public int Retry
      {
         get { return _retry; }
         set { _retry = value; }
      }

      /// <summary>
      /// Fecha/hora en la que se intentó el envio por última vez
      /// </summary>
      public DateTime LastRetry
      {
         get { return _lastretry; }
         set { _lastretry = value; }
      }

      /// <summary>
      /// Prioridad del elemento en la cola de salida
      /// </summary>
      public MailPriority Priority
      {
         get { return _priority; }
         set { _priority = value; }
      }

      /// <summary>
      /// Tipo de elemento
      /// </summary>
      public MWNetQueueType Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Usuario remoto (login) que ha insertado el elemento en la cola
      /// Si no procede (trabajo de distema), se informa SYS.
      /// </summary>
      public string Owner
      {
         get { return _owner; }
         set { _owner = value; }
      }

      #endregion

   }
}
