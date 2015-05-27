using Cosmo.Cms.Classified;
using Cosmo.Cms.Content;
using Cosmo.Cms.WebApp.Classified;
using Cosmo.Communications;
using Cosmo.Data.ORM;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using Cosmo.Utils.Html;
using Cosmo.WebApp.UserServices;
using System;
using System.Collections.Generic;
using System.Web;

namespace Cosmo.WebApp.Classified
{
   /// <summary>
   /// Muestra el contenido de un artículo.
   /// </summary>
   public class ClassifiedView : PageViewContainer
   {
      int classifiedId = -1;
      FormControl contactForm = null;

      public override void InitPage()
      {
         // Obtiene los parámetros de llamada
         classifiedId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);

         ClassifiedContactRequest request = new ClassifiedContactRequest();
         request.ClassifiedAdId = classifiedId;

         OrmEngine orm = new OrmEngine();
         contactForm = orm.CreateForm(this, request, true);
         contactForm.Action = "ClassifiedView";
      }

      public override void LoadPage()
      {
         //-------------------------------
         // Obtención de datos
         //-------------------------------

         // Inicializaciones
         ClassifiedAdsDAO classifiedDao = new ClassifiedAdsDAO(Workspace);

         // Obtiene el documento y la carpeta
         ClassifiedAd classified = classifiedDao.Item(classifiedId);
         ClassifiedAdsSection folder = classifiedDao.GetFolder(classified.FolderId);

         // Obtiene la cuenta del propietario
         User user = Workspace.AuthenticationService.GetUser(classified.UserID);

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

         // Cabecera del cocumento
         DocumentHeaderControl docHead = new DocumentHeaderControl(this);
         docHead.Title = classified.Title;

         // Datos de contacto
         List<KeyValue> contactData = new List<KeyValue>();
         contactData.Add(new KeyValue("Nombre de contacto", user.GetDisplayName()));

         if (!string.IsNullOrWhiteSpace(classified.Phone))
         {
            contactData.Add(new KeyValue("Teléfono", IconControl.GetIcon(this, IconControl.ICON_PHONE) + HtmlContentControl.HTML_SPACE + classified.Phone));
         }

         contactData.Add(new KeyValue("Localización", IconControl.GetIcon(this, IconControl.ICON_MAP_MARKER) + HtmlContentControl.HTML_SPACE + user.City + HtmlContentControl.HTML_SPACE + "(" + Workspace.DataService.GetDataList("country").GetValueByKey(user.CountryID.ToString()) + ")"));
         contactData.Add(new KeyValue("Fecha de publicación", classified.Updated.ToString(Formatter.FORMAT_DATE)));

         // Contenido
         PanelControl docPanel = new PanelControl(this);
         docPanel.CaptionIcon = IconControl.ICON_TAG;
         docPanel.Caption = classified.Title;
         docPanel.Content.Add(new HtmlContentControl(this, classified.Body));
         docPanel.Footer.Add(new HtmlContentControl(this).AppendDataTable(contactData));
         docPanel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnReturn", "Volver", IconControl.ICON_REPLY, "#", "history.go(-1);return false;"));

         MainContent.Add(docPanel);
         
         // Formulario de contacto
         MainContent.Add(contactForm);

         // Compartir
         PanelControl sharePanel = new PanelControl(this);
         sharePanel.Caption = "Compartir";

         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-facebook\" href=\"https://www.facebook.com/sharer/sharer.php?u=" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\" target=\"_blank\"><i class=\"fa fa-facebook\"></i> Facebook</a>"));
         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-google-plus\" href=\"https://plus.google.com/share?url=" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\" target=\"_blank\"><i class=\"fa fa-google-plus\"></i> Google+</a>"));
         sharePanel.Content.Add(new HtmlContentControl(this, "<a class=\"btn btn-block btn-social btn-twitter\"><i class=\"fa fa-twitter\"></i> Twitter</a>"));

         RightContent.Add(sharePanel);
         
         // Panel de herramientas administrativas
         if (Workspace.CurrentUser.CheckAuthorization("admin", "publisher"))
         {
            ButtonControl btnTool;

            PanelControl adminPanel = new PanelControl(this);
            adminPanel.Caption = "Administrar";

            btnTool = new ButtonControl(this);
            btnTool.Icon = IconControl.ICON_EDIT;
            btnTool.Caption = "Editar";
            btnTool.Color = ComponentColorScheme.Success;
            btnTool.IsBlock = true;
            btnTool.Href = new Url("ContentEdit").AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, classified.Id).
                                                  AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_EDIT).
                                                  ToString(true);
            adminPanel.Content.Add(btnTool);

            RightContent.Add(adminPanel);
         }
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         OrmEngine orm = new OrmEngine();
         ClassifiedContactRequest request = new ClassifiedContactRequest();

         try
         {
            if (orm.ProcessForm(request, contactForm, Parameters))
            {
               request.IpAddress = HttpContext.Current.Request.UserHostAddress;

               // Si el objeto es válido se realiza la acción
               ClassifiedAdsDAO ads = new ClassifiedAdsDAO(Workspace);
               ads.SendContactRequest(request);
            }
         }
         catch (CommunicationsException)
         {
            MainContent.Add(new AlertControl(this, "Se ha producido un problema al enviar el mensaje de contacto.", ComponentColorScheme.Error));
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }
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
