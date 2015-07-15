using Cosmo.Cms.Model.Content;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Reflection;
using System.Web;

namespace Cosmo.Cms.Web
{
   /// <summary>
   /// Muestra el contenido de un artículo.
   /// </summary>
   public class ContentView : PageView
   {

      #region PageView Implementation

      public override void LoadPage()
      {
         //-------------------------------
         // Obtención de parámetros
         //-------------------------------

         // Obtiene los parámetros de llamada
         int docId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);

         //-------------------------------
         // Obtención de datos
         //-------------------------------

         // Initialize the persistence managers
         DocumentDAO docs = new DocumentDAO(Workspace);

         // Get document 
         Document doc = docs.Item(docId);
         if (doc == null)
         {
            ShowError("Documento no encontrado",
                      "El documento solicitado no existe o bien no se encuentra disponible en estos momentos." + HtmlContentControl.HTML_NEW_LINE + "Disculpe las moléstias.");
            return;
         }

         // Get folder
         DocumentFolder folder = docs.GetFolder(doc.FolderId, false);
         if (folder == null)
         {
            ShowError("Documento no disponible",
                      "El documento solicitado no se encuentra disponible en estos momentos." + HtmlContentControl.HTML_NEW_LINE + "Disculpe las moléstias.");
            return;
         }

         //-------------------------------
         // View construction
         //-------------------------------

         Title = doc.Title + " | " + DocumentDAO.SERVICE_NAME;
         ActiveMenuId = folder.MenuId;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_BOOK;
         MainContent.Add(header);

         // Cabecera del cocumento
         DocumentHeaderControl docHead = new DocumentHeaderControl(this);
         docHead.Text = doc.Title;
         docHead.Description = doc.Description;

         // Content
         HtmlContentControl html = new HtmlContentControl(this, doc.Content);

         PanelControl docPanel = new PanelControl(this);
         docPanel.Content.Add(docHead);
         docPanel.Content.Add(html);

         MainContent.Add(docPanel);

         // Documentos relacionados
         if (doc.RelatedDocuments.Count > 0)
         {
            MediaListControl relatedDocs = DocumentUI.ConvertToMediaList(this, doc.RelatedDocuments);

            PanelControl relPanel = new PanelControl(this);
            relPanel.Caption = "Artículos relacionados";
            relPanel.Content.Add(relatedDocs);

            MainContent.Add(relPanel);
         }

         // Add information about the publish of content
         User author = Workspace.SecurityService.GetUser(doc.Owner);
         if (author != null)
         {
            Cosmo.Web.UserDataModal userData = new Cosmo.Web.UserDataModal();
            Modals.Add(userData);

            HtmlContentControl authContent = new HtmlContentControl(this);
            authContent.AppendParagraph(Workspace.UIService.Render(new UserLinkControl(this, author, userData)));
            if (doc.Created != DateTime.MinValue)
            {
               authContent.AppendParagraph(HtmlContentControl.BoldText("Publicado el") + HtmlContentControl.HTML_NEW_LINE +
                                           doc.Created.ToString(Formatter.FORMAT_SHORTDATE));
            }
            if (doc.Updated != DateTime.MinValue)
            {
               authContent.AppendParagraph(HtmlContentControl.BoldText("Actualizado el") + HtmlContentControl.HTML_NEW_LINE +
                                           doc.Updated.ToString(Formatter.FORMAT_SHORTDATE));
            }
            
            PanelControl authPanel = new PanelControl(this);
            authPanel.Caption = "Autor";
            authPanel.Content.Add(authContent);

            RightContent.Add(authPanel);
         }

         // Archivos adjuntos
         if (doc.HasAttachments)
         {
            ButtonControl btnAttach = new ButtonControl(this);
            btnAttach.Href = doc.Attachment;
            btnAttach.IsBlock = true;
            btnAttach.Text = "Descargar archivo";
            btnAttach.Icon = IconControl.ICON_DOWNLOAD;
            btnAttach.Color = ComponentColorScheme.Primary;

            PanelControl attachPanel = new PanelControl(this);
            attachPanel.Caption = "Archivos adjuntos";
            attachPanel.Content.Add(btnAttach);

            RightContent.Add(attachPanel);
         }

         // Panel de herramientas administrativas
         if (Workspace.CurrentUser.CheckAuthorization(DocumentDAO.ROLE_CONTENT_EDITOR))
         {
            ButtonControl btnTool;

            PanelControl adminPanel = new PanelControl(this);
            adminPanel.Caption = "Administrar";

            btnTool = new ButtonControl(this);
            btnTool.Icon = IconControl.ICON_EDIT;
            btnTool.Text = "Editar";
            btnTool.Color = ComponentColorScheme.Success;
            btnTool.IsBlock = true;
            btnTool.Href = ContentEdit.GetURL(doc.FolderId, doc.ID);

            adminPanel.Content.Add(btnTool);

            RightContent.Add(adminPanel);
         }

         // Compartir
         PanelControl sharePanel = new PanelControl(this);
         sharePanel.Caption = "Compartir";

         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-facebook\" href=\"https://www.facebook.com/sharer/sharer.php?u=" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\" target=\"_blank\"><i class=\"fa fa-facebook\"></i> Facebook</a>"));
         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-google-plus\" href=\"https://plus.google.com/share?url=" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\" target=\"_blank\"><i class=\"fa fa-google-plus\"></i> Google+</a>"));
         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-twitter\" href=\"https://twitter.com/intent/tweet?text=" + HttpUtility.UrlEncode(doc.Title) + "&url=" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\"><i class=\"fa fa-twitter\"></i> Twitter</a>"));

         RightContent.Add(sharePanel);
      }

      public override void InitPage()
      {
         // Nothing to do
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         
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

      #region Static Members

      /// <summary>
      /// Gets the URL to show the content.
      /// </summary>
      /// <param name="contentId">Content unique identifier.</param>
      /// <returns>A string representing the relative URL requested.</returns>
      public static string GetURL(int contentId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, contentId);

         return url.ToString();
      }

      #endregion

   }
}
