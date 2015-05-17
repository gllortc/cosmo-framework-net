using Cosmo.Cms.Forums.Model;
using Cosmo.Cms.Common;
using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Collections.Generic;
using Cosmo.Utils.Html;

namespace Cosmo.Cms.Forums
{
   /// <summary>
   /// ANUNCIOS CLASIFICADOS.
   /// Muestra el contenido de una categoria.
   /// </summary>
   public class ForumThreadView : CosmoPage
   {
      public override void LoadPage()
      {
         // Declaraciones
         // ForumsDAO ads = null;
         // ForumChannel folder = null;

         Title = ForumsDAO.SERVICE_NAME;

         PageHeader header = new PageHeader(this);
         header.Title = ForumsDAO.SERVICE_NAME;
         header.Icon = Glyphicon.ICON_SHOPPING_CART;
         MainContent.Add(header);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene los parámetros
         int channelid = Parameters.GetInteger(ForumsDAO.PARAM_CHANNEL_ID);
         int threadid = Parameters.GetInteger(ForumsDAO.PARAM_THREAD_ID);
         int page = Parameters.GetInteger(ForumsDAO.PARAM_PAGE_NUM, 1);

         if (channelid <= 0) Redirect(ForumsDAO.URL_HOME);
         if (threadid <= 0) Redirect(ForumsDAO.URL_CHANNEL + "?" + ForumsDAO.PARAM_CHANNEL_ID + "=" + channelid);

         // Recupera la información del canal, thread y mensajes
         ForumsDAO forums = new ForumsDAO(Workspace);
         ForumChannel forum = forums.GetForum(channelid);
         ForumThread thread = forums.GetThread(threadid);

         // Obtiene las propiedades particulares de la carpeta actual
         if (thread == null)
         {
            ShowError("Hilo no accesible",
                      "Este hilo no existe, ha sido eliminado o no está disponible.");
            return;
         }

         // Agrega la meta-información de la página
         Title = ForumsDAO.SERVICE_NAME + " - " + thread.Title;

         //--------------------------------------------------------------
         // Genera la lista de mensajes
         //--------------------------------------------------------------

         TimelineItem item = null;
         Timeline timeline = new Timeline(this);
         List<ForumMessage> messages = new List<ForumMessage>();
         ThreadMessagesOrder order = ThreadMessagesOrder.Ascending;

         timeline.Title = thread.Title;

         order = thread.GetMessageOrder(Workspace.Context.Request, Workspace.Context.Response);
         messages = forums.GetThreadMessages(thread.ID, order);

         if (messages.Count <= 0)
         {
            ShowError("Mensajes no accesibles",
                      "No se han podido recuperar los mensajes de este hilo.");
            return;
         }

         // Si el hilo está cerrado advierte al usuario
         if (thread.Closed)
         {
            MainContent.Add(new Alert(this, "Este hilo está cerrado y no admite más comentarios.", ComponentColorScheme.Warning));
         }

         // Muestra los mensajes
         Formatter formatter = new Formatter();
         foreach (ForumMessage message in messages)
         {
            item = new TimelineItem();
            item.Title = message.Name;
            item.Body = formatter.bbCodeParser(message.Body); // message.Body;
            item.Time = message.Date.ToString(Formatter.FORMAT_DATETIME);

            timeline.AddItem(item);
         }

         MainContent.Add(timeline);

         
      }

      public override void InitPage()
      {
         // Nothing to do
      }

      public override void FormDataReceived()
      {
         throw new NotImplementedException();
      }
   }
}