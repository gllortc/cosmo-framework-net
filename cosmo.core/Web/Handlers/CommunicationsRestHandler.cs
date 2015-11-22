using Cosmo.Communications.PrivateMessages;
using Cosmo.Net.REST;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Cosmo.Web.Handlers
{
   /// <summary>
   /// Handler que implementa los servicios REST correspondientes al servicio UI de Cosmo.
   /// </summary>
   public class CommunicationsRestHandler : RestHandler
   {
      /// <summary>parámetro de llamada: Nombre de archivo.</summary>
      public const string PARAMETER_TEMPLATE_NAME = "_tid_";

      #region RestHandler Implementation

      /// <summary>
      /// Método invocado al recibir una petición.
      /// </summary>
      /// <param name="command">Comando a ejecutar pasado mediante el parámetro definido mediante <see cref="Cosmo.Workspace.PARAM_COMMAND"/>.</param>
      public override void ServiceRequest(string command)
      {
         switch (command)
         {
            case CommunicationsRestHandler.COMMAND_SEND:
               PrivateMessageSend();
               break;

            case CommunicationsRestHandler.COMMAND_DELETE:
               PrivateMessageDelete();
               break;

            case CommunicationsRestHandler.COMMAND_GETTHREADMSGS:
               PrivateMessageGetThreadMessages();
               break;

            // case CommApi.COMMAND_COMPOSE:
               // Redactar mensaje privado
               // MessageCompose(context);
               // break;

            // case CommApi.COMMAND_READ:
               // Abrir un mensaje privado
               // MessageRead(context);
               // break;

            default:
               break;
         }
      }

      #endregion

      #region Command: Send PMessage

      /// <summary>Identificador del parámetro QueryString para el comando SEND.</summary>
      public const string COMMAND_SEND = "ums";

      /// <summary>
      /// Envia un mensaje privado.
      /// </summary>
      private void PrivateMessageSend()
      {
         JavaScriptSerializer json = new JavaScriptSerializer();

         try
         {
            PrivateMessage pm = new PrivateMessage();
            pm.Subject = string.Empty;
            pm.Body = Parameters.GetString(PrivateMessageDAO.PARAM_BODY);
            pm.FromIP = Request.UserHostAddress;
            pm.FromUserID = Workspace.CurrentUser.User.ID;
            pm.ToUserID = Parameters.GetInteger(Workspace.PARAM_USER_ID);

            PrivateMessageDAO pmdao = new PrivateMessageDAO(Workspace);
            pmdao.SendMessage(pm);

            SendResponse(new AjaxResponse());
         }
         catch (Exception ex)
         {
            Workspace.Logger.Error(this, "PrivateMessageSend", ex);
            SendResponse(new AjaxResponse(0, "Se ha producido un error en el servidor y no ha sido posible entregar el mensaje."));
         }
      }

      #endregion

      #region Command: Read PMessage

      /// <summary>Identificador del parámetro QueryString para el comando READ.</summary>
      public const string COMMAND_READ = "umr";

      /// <summary>
      /// Muestra un mensaje privado.
      /// </summary>
      private void MessageRead()
      {
         JavaScriptSerializer json = new JavaScriptSerializer();

         // Obtiene el identificador del mensaje solicitado
         int msgid = Parameters.GetInteger(Workspace.PARAM_OBJECT_ID);

         if (msgid <= 0)
         {
            SendResponse(new AjaxResponse(0, "No se ha podido obtener el mensaje solicitado."));
         }
         else
         {
            try
            {
               // Inicializa los servidores
               PrivateMessageDAO msgserver = new PrivateMessageDAO(Workspace);
               PrivateMessage msg = msgserver.GetMessage(msgid);

               // Envia confirmación al cliente
               SendResponse(new AjaxResponse(msg));
            }
            catch (Exception e)
            {
               Workspace.Logger.Error(this, "MessageRead", e);
               SendResponse(new AjaxResponse(0, "El mensaje no se ha podido recuperar debido a un error interno. Inténtelo de nuevo y si el error persiste póngase en contacto con la administración de " + Workspace.Name));
            }
         }
      }

      #endregion

      #region Command: Delete PMessage

      /// <summary>Identificador del parámetro QueryString para el comando DELETE.</summary>
      public const string COMMAND_DELETE = "umd";

      /// <summary>
      /// Elimina un mensaje privado.
      /// </summary>
      private void PrivateMessageDelete()
      {
         JavaScriptSerializer json = new JavaScriptSerializer();

         // Eliminar un mensaje
         int msgid = Parameters.GetInteger(Workspace.PARAM_OBJECT_ID);

         try
         {
            // Elimina el mensaje
            PrivateMessageDAO msgserver = new PrivateMessageDAO(Workspace);
            msgserver.DeleteMessage(msgid);

            SendResponse(new AjaxResponse());
         }
         catch (Exception ex)
         {
            Workspace.Logger.Error(this, "PrivateMessageDelete", ex);
            SendResponse(new AjaxResponse(0, ex.Message));
         }
      }

      #endregion

      #region Command: Get PMessages Threads

      /// <summary>ID del comando GetThreadMessages.</summary>
      public const string COMMAND_GETTHREADMSGS = "umgth";

      /// <summary>
      /// Devuelve el código XHTML necesario para representar los mensajes de un determinado thread.
      /// </summary>
      private void PrivateMessageGetThreadMessages()
      {
         int ownerId = 0;
         int userId = 0;
         List<PrivateMessage> messages = null;

         try
         {
            ownerId = Workspace.CurrentUser.User.ID;
            userId = Parameters.GetInteger(Workspace.PARAM_USER_ID);

            PrivateMessageDAO pmdao = new PrivateMessageDAO(Workspace);
            messages = pmdao.GetThreadMessages(ownerId, userId);

            SendResponse(new AjaxResponse(messages));
         }
         catch (Exception ex)
         {
            Workspace.Logger.Error(this, "PrivateMessageSend", ex);
            SendResponse(new AjaxResponse(0, "Se ha producido un error interno al intentar recuperar los mensajes de esta conversa. Intenta recargar la página de nuevo."));
         }
      }

      #endregion
   }
}
