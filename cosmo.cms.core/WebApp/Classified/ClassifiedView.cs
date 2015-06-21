using Cosmo.Cms.Classified;
using Cosmo.Cms.Content;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using Cosmo.Utils.Html;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace Cosmo.WebApp.Classified
{
   /// <summary>
   /// Muestra el contenido de un artículo.
   /// </summary>
   public class ClassifiedView : PageView
   {
      // Internal datadeclarations
      int classifiedId = -1;

      #region PageView Implementation

      public override void InitPage()
      {
         // Gets the parameters data
         classifiedId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);

         //-------------------------------
         // Gets the classified data and folder
         //-------------------------------

         // Inicializaciones
         ClassifiedAdsDAO classifiedDao = new ClassifiedAdsDAO(Workspace);

         // Obtiene el documento y la carpeta
         ClassifiedAd classified = classifiedDao.Item(classifiedId);
         ClassifiedAdsSection folder = classifiedDao.GetFolder(classified.FolderID);

         // Get the author
         User user = Workspace.SecurityService.GetUser(classified.UserID);

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = classified.Title + " | " + DocumentDAO.SERVICE_NAME;
         // ActiveMenuId = folder.MenuId;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Icon = IconControl.ICON_SHOPPING_CART;
         header.Title = ClassifiedAdsDAO.SERVICE_NAME;
         header.SubTitle = folder.Name;
         MainContent.Add(header);

         // Contact data
         List<KeyValue> contactData = new List<KeyValue>();
         contactData.Add(new KeyValue("Nombre de contacto", user.GetDisplayName()));
         if (!string.IsNullOrWhiteSpace(classified.Phone))
         {
            contactData.Add(new KeyValue("Teléfono", IconControl.GetIcon(this, IconControl.ICON_PHONE) + HtmlContentControl.HTML_SPACE + classified.Phone));
         }
         contactData.Add(new KeyValue("Localización", IconControl.GetIcon(this, IconControl.ICON_MAP_MARKER) + HtmlContentControl.HTML_SPACE + user.City + HtmlContentControl.HTML_SPACE + "(" + Workspace.DataService.GetDataList("country").GetValueByKey(user.CountryID.ToString()) + ")"));
         contactData.Add(new KeyValue("Fecha de publicación", classified.Updated.ToString(Formatter.FORMAT_DATE)));

         PanelControl adPanel = new PanelControl(this);
         adPanel.Caption = classified.Title;
         adPanel.CaptionIcon = IconControl.ICON_TAG;
         adPanel.Content.Add(new HtmlContentControl(this, classified.Body));
         adPanel.Footer.Add(new HtmlContentControl(this).AppendDataTable(contactData));

         MainContent.Add(adPanel);

         //-------------------------------
         // Right column
         //-------------------------------

         // Modals
         ClassifiedContactModal contactModal = new ClassifiedContactModal(classifiedId);
         Modals.Add(contactModal);

         // Contact
         PanelControl contactPanel = new PanelControl(this);
         contactPanel.Caption = "Contacto";
         contactPanel.CaptionIcon = IconControl.ICON_ENVELOPE;
         contactPanel.Content.Add(new HtmlContentControl(this, "¿Interesado en el anuncio? Puedes ponerte en contacto con el autor del anuncio mediante el formulario de contacto."));
         contactPanel.Footer.Add(new ButtonControl(this, "cmdContact", "Contacto", IconControl.ICON_ENVELOPE, contactModal));

         RightContent.Add(contactPanel);

         // Panel de herramientas administrativas
         if (Workspace.CurrentUser.CheckAuthorization(DocumentDAO.ROLE_CONTENT_EDITOR) ||
             (IsAuthenticated && (classified.UserID == Workspace.CurrentUser.User.ID)))
         {
            ButtonControl btnTool;

            PanelControl adminPanel = new PanelControl(this);
            adminPanel.Caption = "Administrar";

            btnTool = new ButtonControl(this);
            btnTool.Icon = IconControl.ICON_EDIT;
            btnTool.Text = "Editar";
            btnTool.Color = ComponentColorScheme.Success;
            btnTool.IsBlock = true;
            btnTool.Href = ClassifiedEdit.GetURL(classified.FolderID, classified.ID);
            adminPanel.Content.Add(btnTool);

            RightContent.Add(adminPanel);
         }

         // Share box
         PanelControl sharePanel = new PanelControl(this);
         sharePanel.Caption = "Compartir";

         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-facebook\" href=\"https://www.facebook.com/sharer/sharer.php?u=" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\" target=\"_blank\"><i class=\"fa fa-facebook\"></i> Facebook</a>"));
         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-google-plus\" href=\"https://plus.google.com/share?url=" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\" target=\"_blank\"><i class=\"fa fa-google-plus\"></i> Google+</a>"));
         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-twitter\"><i class=\"fa fa-twitter\"></i> Twitter</a>"));

         RightContent.Add(sharePanel);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // Nothing to do
      }

      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      public override void LoadPage()
      {
         // Nothing to do 
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Gets an URL to view an existing ad.
      /// </summary>
      /// <param name="classifiedId">Ad unique identifier.</param>
      /// <returns>A string representing the requested URL.</returns>
      public static string GetURL(int classifiedId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, classifiedId);

         return url.ToString();
      }

      #endregion

   }
}
