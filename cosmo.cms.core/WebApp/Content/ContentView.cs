using Cosmo.Cms.Content;
using Cosmo.Cms.Utils;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Web;

namespace Cosmo.WebApp.Content
{
   /// <summary>
   /// Muestra el contenido de un artículo.
   /// </summary>
   public class ContentView : PageViewContainer
   {
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

         // Inicializaciones
         DocumentDAO docs = new DocumentDAO(Workspace);

         // Obtiene el documento y la carpeta
         Document doc = docs.Item(docId);
         DocumentFolder folder = docs.GetFolder(doc.FolderId, false);

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = doc.Title + " | " + DocumentDAO.SERVICE_NAME;
         ActiveMenuId = folder.MenuId;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_BOOK;
         MainContent.Add(header);

         // Cabecera del cocumento
         DocumentHeaderControl docHead = new DocumentHeaderControl(this);
         docHead.Title = doc.Title;
         docHead.SubTitle = doc.Description;

         // Contenido
         HtmlContentControl html = new HtmlContentControl(this, doc.Content);

         PanelControl docPanel = new PanelControl(this);
         docPanel.Content.Add(docHead);
         docPanel.Content.Add(html);

         MainContent.Add(docPanel);

         // Documentos relacionados
         if (doc.RelatedDocuments.Count > 0)
         {
            MediaListControl relatedDocs = LayoutAdapter.Documents.ConvertToMediaList(this, doc.RelatedDocuments);

            PanelControl relPanel = new PanelControl(this);
            relPanel.Caption = "Artículos relacionados";
            relPanel.Content.Add(relatedDocs);

            MainContent.Add(relPanel);
         }

         // Archivos adjuntos
         if (!string.IsNullOrWhiteSpace(doc.Attachment))
         {
            ListItem attachment = new ListItem();
            attachment.Caption = "Descargar archivo";
            attachment.Icon = "fa-download";
            attachment.Href = doc.Attachment;

            ListGroupControl attachments = new ListGroupControl(this);
            attachments.Add(attachment);

            PanelControl attachPanel = new PanelControl(this);
            attachPanel.Caption = "Archivos adjuntos";
            attachPanel.Content.Add(attachments);

            RightContent.Add(attachPanel);
         }
         
         // Compartir
         PanelControl sharePanel = new PanelControl(this);
         sharePanel.Caption = "Compartir";

         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-facebook\" href=\"https://www.facebook.com/sharer/sharer.php?u=" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\" target=\"_blank\"><i class=\"fa fa-facebook\"></i> Facebook</a>"));
         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-google-plus\" href=\"https://plus.google.com/share?url=" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\" target=\"_blank\"><i class=\"fa fa-google-plus\"></i> Google+</a>"));
         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-twitter\"><i class=\"fa fa-twitter\"></i> Twitter</a>"));

         RightContent.Add(sharePanel);
         
         // Panel de herramientas administrativas
         if (Workspace.CurrentUser.CheckAuthorization("admin", DocumentDAO.ROLE_CONTENT_EDITOR))
         {
            ButtonControl btnTool;

            PanelControl adminPanel = new PanelControl(this);
            adminPanel.Caption = "Administrar";

            btnTool = new ButtonControl(this);
            btnTool.Icon = IconControl.ICON_EDIT;
            btnTool.Caption = "Editar";
            btnTool.Color = ComponentColorScheme.Success;
            btnTool.IsBlock = true;
            btnTool.Href = DocumentDAO.GetDocumentEditURL(doc.ID);

            adminPanel.Content.Add(btnTool);

            RightContent.Add(adminPanel);
         }
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
   }
}
