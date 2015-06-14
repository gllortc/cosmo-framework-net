using Cosmo.Cms.Forums;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils.Html;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.WebApp.Forums
{
   /// <summary>
   /// Muestra los threads en los que ha participado un determinado usuario.
   /// </summary>
   [AuthenticationRequired]
   public class ForumThreadsByUser : PageView
   {

      #region PageView Implementation

      public override void LoadPage()
      {
         // Declaraciones
         ForumsDAO forumDao = null;

         Title = ForumsDAO.SERVICE_NAME;
         ActiveMenuId = "forum";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene los parámetros
         int folderid = Parameters.GetInteger(ForumsDAO.PARAM_CHANNEL_ID, 0);
         int pageIdx = Parameters.GetInteger(ForumsDAO.PARAM_PAGE_NUM, 1);

         PageHeaderControl header = new PageHeaderControl(this);
         header.Icon = IconControl.ICON_COMMENTS;
         header.Title = ForumsDAO.SERVICE_NAME;
         MainContent.Add(header);

         // Agrega la meta-información de la página
         Title = ForumsDAO.SERVICE_NAME + " - Mis hilos";

         //--------------------------------------------------------------
         // Genera la lista de anuncios a mostrar
         //--------------------------------------------------------------

         forumDao = new ForumsDAO(Workspace);
         List<ForumThread> threads = forumDao.GetThreadsByUser(Workspace.CurrentUser.User.ID, pageIdx);

         if (threads.Count > 0)
         {
            string thUrl;
            TableRow row;

            TableControl table = new TableControl(this);
            table.Hover = true;
            table.Header = new TableRow("row-head", "Tema", "Autor", "Respuestas", "Última aportación");

            foreach (ForumThread thread in threads)
            {
               // Genera la URL del anuncio
               thUrl = ForumThreadView.GetURL(thread.ID, thread.ForumID, pageIdx);

               // Genera el elemento de la lista
               row = new TableRow("row-ad-" + thread.ID,
                                  IconControl.GetIcon(this, IconControl.ICON_COMMENT) + " " + HtmlContentControl.Link(thUrl, thread.Title, false),
                                  IconControl.GetIcon(this, IconControl.ICON_USER) + " " + thread.AuthorName,
                                  (thread.MessageCount - 1).ToString(),
                                  thread.LastReply.ToString(Formatter.FORMAT_DATETIME));

               table.Rows.Add(row);
            }

            HtmlContentControl html = new HtmlContentControl(this);
            html.AppendParagraph("El siguiente listado contiene los hilos que has abierto y todos los hilos en los que has participado.");

            PaginationControl pages = new PaginationControl(this);
            pages.Current = pageIdx;
            pages.Min = 1;
            pages.Max = forumDao.GetChannelThreadsCount(folderid) / ForumsDAO.MaxThreadsPerPage;
            pages.UrlPattern = "ForumThreadsByUser?" + ForumsDAO.PARAM_PAGE_NUM + "=" + PaginationControl.URL_PAGEID_TAG;

            PanelControl panel = new PanelControl(this);
            panel.CaptionIcon = IconControl.ICON_USER;
            panel.Caption = "Mis hilos";
            panel.Content.Add(html);
            panel.Content.Add(table);
            panel.Footer.Add(pages);

            panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnAddComment", "Abrir nuevo tema", IconControl.ICON_PLUS, "#", string.Empty));
            panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnLegal", "Normas del foro", IconControl.ICON_LEGAL, Workspace.Settings.GetString("cs.forum.rules", "#"), string.Empty));

            MainContent.Add(panel);
         }
         else
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Type = ComponentColorScheme.Information;
            callout.Title = "No hemos encontrado ningún hilo...";
            callout.Text = "Esto significa que aún no has escrito en el foro. ¡Animate!";

            MainContent.Add(callout);
         }
      }

      public override void InitPage()
      {
         // Nothing to do
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // Nothing to do
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      #endregion

      #region Static members

      /// <summary>
      /// Permite obtener una URL relativa a un canal concreto de los foros.
      /// </summary>
      /// <param name="channelId">Identificador del canal.</param>
      /// <param name="pageNum">Número de página (para listado de threads paginados).</param>
      public static string GetURL()
      {
         return GetURL(1);
      }

      /// <summary>
      /// Permite obtener una URL relativa a un canal concreto de los foros.
      /// </summary>
      /// <param name="channelId">Identificador del canal.</param>
      /// <param name="pageNum">Número de página (para listado de threads paginados).</param>
      public static string GetURL(int pageNum)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(ForumsDAO.PARAM_PAGE_NUM, pageNum);

         return url.ToString();
      }

      #endregion

   }
}