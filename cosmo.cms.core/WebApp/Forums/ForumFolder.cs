using Cosmo.Cms.Forums;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils.Html;
using System.Collections.Generic;

namespace Cosmo.WebApp.Forums
{
   /// <summary>
   /// FOROS.
   /// Muestra los threads de un canal del foro.
   /// </summary>
   public class ForumFolder : PageViewContainer
   {
      public override void LoadPage()
      {
         // Declaraciones
         ForumsDAO ads = null;
         ForumChannel folder = null;

         Title = ForumsDAO.SERVICE_NAME;
         ActiveMenuId = "forum";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene los parámetros
         int folderid = Parameters.GetInteger(ForumsDAO.PARAM_CHANNEL_ID, 0);
         int pageIdx = Parameters.GetInteger(ForumsDAO.PARAM_PAGE_NUM, 1);

         // Obtiene las propiedades particulares de la carpeta actual
         ads = new ForumsDAO(Workspace);
         folder = ads.GetForum(folderid);
         if (folder == null)
         {
            ShowError("Categoria no encontrada",
                      "El foro solicitado no existe o bien no se encuentra disponible en estos momentos.");
            return;
         }

         PageHeaderControl header = new PageHeaderControl(this);
         header.Icon = IconControl.ICON_COMMENTS;
         header.Title = ForumsDAO.SERVICE_NAME;
         header.SubTitle = folder.Name;
         MainContent.Add(header);

         // Agrega la meta-información de la página
         Title = ForumsDAO.SERVICE_NAME + " - " + folder.Name;

         //--------------------------------------------------------------
         // Genera la lista de anuncios a mostrar
         //--------------------------------------------------------------

         List<ForumThread> threads = ads.GetChannelThreads(folder.ID, pageIdx);

         if (threads.Count > 0)
         {
            // Url thUrl;
            TableRow row;

            TableControl table = new TableControl(this);
            table.Hover = true;
            table.Header = new TableRow("row-head", "Tema", "Autor", "Respuestas", "Última aportación");

            foreach (ForumThread thread in threads)
            {
               // Genera la URL del thread
               //thUrl = new Url(ForumsDAO.URL_THREAD);
               //thUrl.AddParameter(ForumsDAO.PARAM_CHANNEL_ID, folder.ID);
               //thUrl.AddParameter(ForumsDAO.PARAM_THREAD_ID, thread.ID);
               //thUrl.AddParameter(ForumsDAO.PARAM_PAGE_NUM, pageIdx);

               // Genera el elemento de la lista
               row = new TableRow("row-th-" + thread.ID,
                                  IconControl.GetIcon(this, IconControl.ICON_COMMENT, thread.Closed) + " " + HtmlContentControl.Link(ForumsDAO.GetThreadUrl(thread.ID, folder.ID, pageIdx), thread.Title, false),
                                  IconControl.GetIcon(this, IconControl.ICON_USER) + " " + thread.AuthorName,
                                  (thread.MessageCount - 1).ToString(),
                                  thread.LastReply.ToString(Formatter.FORMAT_DATETIME));

               table.Rows.Add(row);
            }

            HtmlContentControl html = new HtmlContentControl(this);
            html.AppendParagraph(folder.Description);

            PaginationControl pages = new PaginationControl(this);
            pages.Current = pageIdx;
            pages.Min = 1;
            pages.Max = ads.GetChannelThreadsCount(folderid) / ForumsDAO.MaxThreadsPerPage;
            pages.UrlPattern = ForumsDAO.GetChannelUrl(folderid, PaginationControl.URL_PAGEID_TAG);

            PanelControl panel = new PanelControl(this);
            panel.CaptionIcon = IconControl.ICON_FOLDER_CLOSE;
            panel.Caption = folder.Name;
            panel.Content.Add(html);
            panel.Content.Add(table);
            panel.Footer.Add(pages);

            panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnAddComment", "Abrir nuevo tema", IconControl.ICON_PLUS, ForumsDAO.GetNewMessageUrl(folderid, pageIdx), string.Empty));
            panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnMyThreads", "Mis temas", IconControl.ICON_USER, ForumsDAO.GetThreadsByUserUrl(), string.Empty));
            panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnLegal", "Normas del foro", IconControl.ICON_LEGAL, Workspace.Settings.GetString("cs.forum.rules", "#"), string.Empty));

            MainContent.Add(panel);
         }
         else
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Type = ComponentColorScheme.Information;
            callout.Title = "No hay ningún anuncio en esta categoria";
            callout.Text = "Actualmente no hay ningún anuncio en esta categoria. Vuelve regularmente para comprobar si hay novedades.";

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
   }
}