using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Reflection;

namespace Cosmo.WebApp.UserServices
{
   /// <summary>
   /// Cosmo Common Pages
   /// Implementa el espacio dónde el usuario puede modificar los datos de su suscripción.
   /// </summary>
   /// <remarks>
   /// Author : Gerard Llort
   /// Version: 1.0.0
   /// Copyright (c) InforGEST
   /// </remarks>
   [AuthenticationRequired]
   public class UserData : PageView
   {
      // Internal data declarations
      private FormControl publicData = null;
      private FormControl notifyData = null;
      private FormControl contactData = null;
      private FormControl securityData = null;

      #region PageView Implementation

      public override void InitPage()
      {
         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = "Mi perfil";
         ActiveMenuId = string.Empty;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = "Mi perfil";
         header.Icon = IconControl.ICON_USER;
         MainContent.Add(header);

         // Formulario para editar datos públicos
         publicData = new FormControl(this, "frmPData");
         publicData.Method = "POST";
         publicData.UsePanel = false;

         HtmlContentControl pdDescription = new HtmlContentControl(this);
         pdDescription.AppendParagraph("Los siguientes datos son visibles para todos los usuarios registrados.");
         publicData.Content.Add(pdDescription);

         FormFieldText pdName = new FormFieldText(this, "pdname", "Nombre completo", FormFieldText.FieldDataType.Text);
         pdName.Required = true;
         publicData.Content.Add(pdName);

         FormFieldEditor pdDesc = new FormFieldEditor(this, "pddesc", "Descripción", FormFieldEditor.FieldEditorType.Simple);
         publicData.Content.Add(pdDesc);

         FormFieldText pdCity = new FormFieldText(this, "pdcity", "Ciudad", FormFieldText.FieldDataType.Text);
         publicData.Content.Add(pdCity);

         FormFieldList pdCountry = new FormFieldList(this, "pdc", "Pais", FormFieldList.ListType.Single);
         pdCountry.Required = true;
         publicData.Content.Add(pdCountry);

         ButtonControl pdSend = new ButtonControl(this, "pdSend", "Guardar", ButtonControl.ButtonTypes.Submit);
         publicData.FormButtons.Add(pdSend);

         TabItemControl publicDataTab = new TabItemControl(this);
         publicDataTab.DomID = "pdTab";
         publicDataTab.Caption = "Datos públicos";
         publicDataTab.Icon = IconControl.ICON_USER;
         publicDataTab.Content.Add(publicData);

         // Formulario para editar datos públicos
         notifyData = new FormControl(this, "frmNotify");
         notifyData.Method = "POST";
         notifyData.UsePanel = false;

         HtmlContentControl nDescription = new HtmlContentControl(this);
         nDescription.AppendParagraph("En esta sección puede configurar que notificaciones desea recibir.");
         notifyData.Content.Add(nDescription);

         FormFieldBoolean intNotify = new FormFieldBoolean(this, "chkInternal", "Notificaciones sobre ofertas y noticias del portal");
         notifyData.Content.Add(intNotify);

         FormFieldBoolean extNotify = new FormFieldBoolean(this, "chkExternal", "Notificaciones sobre ofertas y noticias de terceras empresas");
         notifyData.Content.Add(extNotify);

         FormFieldBoolean pmNotify = new FormFieldBoolean(this, "chkPM", "Notificación de mensajes privados recibidos");
         notifyData.Content.Add(pmNotify);

         ButtonControl notifySend = new ButtonControl(this, "notifySend", "Guardar", ButtonControl.ButtonTypes.Submit);
         notifyData.FormButtons.Add(notifySend);

         TabItemControl notifyTab = new TabItemControl(this);
         notifyTab.DomID = "notifyTab";
         notifyTab.Caption = "Notificaciones";
         notifyTab.Icon = IconControl.ICON_BELL;
         notifyTab.Content.Add(notifyData);

         // Formulario para editar datos de contacto
         contactData = new FormControl(this, "frmContact");
         contactData.Method = "POST";
         contactData.UsePanel = false;

         HtmlContentControl cDescription = new HtmlContentControl(this);
         cDescription.AppendParagraph("Aquí puedes configurar los datos de contacto. Estos datos son <strong>PRIVADOS</strong> y únicamente son usados para rellenar de forma automática los formularios de servicios que usan estos datos (actualmente sólo <em>Anuncios Clasificados</em>).");
         contactData.Content.Add(cDescription);

         FormFieldText contactMail = new FormFieldText(this, "cmail", "Correo electrónico de contacto", FormFieldText.FieldDataType.Email);
         contactData.Content.Add(contactMail);

         FormFieldText contactPhone = new FormFieldText(this, "cphone", "Teléfono de contacto", FormFieldText.FieldDataType.Phone);
         contactData.Content.Add(contactPhone);

         ButtonControl contactSend = new ButtonControl(this, "contactSend", "Guardar", ButtonControl.ButtonTypes.Submit);
         contactData.FormButtons.Add(contactSend);

         TabItemControl contactTab = new TabItemControl(this);
         contactTab.DomID = "contactTab";
         contactTab.Caption = "Datos de contacto";
         contactTab.Icon = IconControl.ICON_ENVELOPE;
         contactTab.Content.Add(contactData);

         // Formulario para modificar contrasenya
         securityData = new FormControl(this, "frmSecurity");
         securityData.Method = "POST";
         securityData.UsePanel = false;

         HtmlContentControl sDescription = new HtmlContentControl(this);
         sDescription.AppendParagraph("Aquí puedes configurar el acceso seguro a tu cuenta de suscriptor.");
         securityData.Content.Add(sDescription);

         FormFieldPassword securityCPwd = new FormFieldPassword(this, "scurpwd", "Contraseña actual");
         securityCPwd.RewriteRequired = false;
         securityData.Content.Add(securityCPwd);

         FormFieldPassword securityPwd = new FormFieldPassword(this, "spwd", "Nueva contraseña");
         securityPwd.RewriteRequired = true;
         securityData.Content.Add(securityPwd);

         ButtonControl securitySend = new ButtonControl(this, "secSend", "Guardar", ButtonControl.ButtonTypes.Submit);
         securityData.FormButtons.Add(securitySend);

         TabItemControl securityTab = new TabItemControl(this);
         securityTab.DomID = "securityTab";
         securityTab.Caption = "Contraseña";
         securityTab.Icon = IconControl.ICON_LOCK;
         securityTab.Content.Add(securityData);

         // Coloca los formularios en pestañas
         TabbedContainerControl tabs = new TabbedContainerControl(this);
         tabs.TabItems.Add(publicDataTab);
         tabs.TabItems.Add(notifyTab);
         tabs.TabItems.Add(contactTab);
         tabs.TabItems.Add(securityTab);
         MainContent.Add(tabs);

         /*
         // Genera el formulario para objetos del tipo User
         OrmEngine orm = new OrmEngine();
         orm.DiscardFields.Add("lo");
         orm.DiscardFields.Add("pw");
         orm.DiscardFields.Add("ml");
         orm.FormStyle = OrmEngine.OrmFormStyle.Tabbed;
         modal = orm.CreateForm(this, Workspace.CurrentUser.User, false);
         MainContent.Add(modal);
         */


         // Carga la lista de paises
         ((FormFieldList)publicData.GetField("pdc")).LoadValuesFromDataList("country");
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         /*
         OrmEngine orm = new OrmEngine();

         try
         {
            // Rellena una instancia con los datos
            User user = Workspace.CurrentUser.User;
            if (orm.ProcessForm(user, modal, Parameters))
            {
               // Genera la nueva cuenta de usuario
               Workspace.AuthenticationService.Update(user);

               // Redirige al usuario a su página persnal
               Redirect("Home");
            }
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }
         */

         try
         {
            if (receivedForm.DomID.Equals("frmPData"))
            {
               // Obtiene los datos del formulario enviado
               Workspace.CurrentUser.User.Name = receivedForm.GetStringFieldValue("pdname");
               Workspace.CurrentUser.User.Description = receivedForm.GetStringFieldValue("pddesc");
               Workspace.CurrentUser.User.City = receivedForm.GetStringFieldValue("pdcity");
               Workspace.CurrentUser.User.CountryID = receivedForm.GetIntFieldValue("pdc");

               // Actualiza los datos del usuario actual
               Workspace.SecurityService.Update(Workspace.CurrentUser.User);
            }
            else if (receivedForm.DomID.Equals("frmNotify"))
            {
               // Obtiene los datos del formulario enviado
               Workspace.CurrentUser.User.CanReceiveInternalMessages = receivedForm.GetBoolFieldValue("chkInternal");
               Workspace.CurrentUser.User.CanReceiveExternalMessages = receivedForm.GetBoolFieldValue("chkExternal");
               Workspace.CurrentUser.User.CanReceivePrivateMessagesNotify = receivedForm.GetBoolFieldValue("chkPM");

               // Actualiza los datos del usuario actual
               Workspace.SecurityService.Update(Workspace.CurrentUser.User);
            }
            else if (receivedForm.DomID.Equals("frmContact"))
            {
               // Obtiene los datos del formulario enviado
               Workspace.CurrentUser.User.MailAlternative = receivedForm.GetStringFieldValue("cmail");
               Workspace.CurrentUser.User.Phone = receivedForm.GetStringFieldValue("cphone");

               // Actualiza los datos del usuario actual
               Workspace.SecurityService.Update(Workspace.CurrentUser.User);
            }
            else if (receivedForm.DomID.Equals("frmSecurity"))
            {
               // Actualiza la contraseña
               Workspace.SecurityService.SetPassword(Workspace.CurrentUser.User.ID,
                                                           receivedForm.GetStringFieldValue("scurpwd"),
                                                           receivedForm.GetStringFieldValue("spwd"),
                                                           receivedForm.GetStringFieldValue("spwd"));
            }
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
         // Carga los datos del usuario
         switch (formDomID)
         {
            case "frmPData":
               publicData.SetFieldValue("pdname", Workspace.CurrentUser.User.Name);
               publicData.SetFieldValue("pddesc", Workspace.CurrentUser.User.Description);
               publicData.SetFieldValue("pdcity", Workspace.CurrentUser.User.City);
               publicData.SetFieldValue("pdc", Workspace.CurrentUser.User.CountryID);
               break;

            case "frmNotify":
               notifyData.SetFieldValue("chkInternal", Workspace.CurrentUser.User.CanReceiveInternalMessages);
               notifyData.SetFieldValue("chkExternal", Workspace.CurrentUser.User.CanReceiveExternalMessages);
               notifyData.SetFieldValue("chkPM", Workspace.CurrentUser.User.CanReceivePrivateMessagesNotify);
               break;

            case "frmContact":
               contactData.SetFieldValue("cmail", Workspace.CurrentUser.User.MailAlternative);
               contactData.SetFieldValue("cphone", Workspace.CurrentUser.User.Phone);
               break;
         }
      }

      public override void LoadPage()
      {
         // Nothing to do
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         return url.ToString();
      }

      #endregion

   }
}
