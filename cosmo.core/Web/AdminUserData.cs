using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Reflection;

namespace Cosmo.Web
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
   [AuthorizationRequired(Cosmo.Workspace.ROLE_ADMINISTRATOR)]
   public class AdminUserData : PageView
   {
      // Internal data declarartions
      private FormControl userData = null;
      private FormControl publicData = null;
      private FormControl notifyData = null;
      private FormControl contactData = null;
      private FormControl securityData = null;
      private User user;

      #region PageView Implementation

      public override void InitPage()
      {
         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = "Administrar usuarios";
         ActiveMenuId = string.Empty;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         //-------------------------------
         // Obtención del usuario
         //-------------------------------

         int usrId = Parameters.GetInteger(Cosmo.Workspace.PARAM_USER_ID);
         user = Workspace.SecurityService.GetUser(usrId);

         if (user == null)
         {
            CalloutControl alert = new CalloutControl(this);
            alert.Title = "Usuario no encontrado";
            alert.Text = "No se puede encontrar el usuario solicitado [#" + usrId + "].";
            alert.Type = ComponentColorScheme.Error;
            MainContent.Add(alert);

            return;
         }

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = "Administrar usuarios";
         header.Icon = IconControl.ICON_USER;
         MainContent.Add(header);

         //-------------------------------
         // User identification form
         //-------------------------------

         // Formulario to show/edit user id data
         userData = new FormControl(this, "frmUsrData");
         userData.Method = "POST";
         userData.UsePanel = false;
         userData.AddFormSetting(Cosmo.Workspace.PARAM_USER_ID, user.ID);

         HtmlContentControl usrDesc = new HtmlContentControl(this);
         usrDesc.AppendParagraph("Datos de identificación del usuario.");
         userData.Content.Add(usrDesc);

         HtmlContentControl usrLogin = new HtmlContentControl(this);
         usrLogin.AppendHeader(3, IconControl.GetIcon(this, IconControl.ICON_USER) + 
                               HtmlContentControl.HTML_SPACE + 
                               user.Login);
         userData.Content.Add(usrLogin);

         FormFieldText email = new FormFieldText(this, "email", "eMail", FormFieldText.FieldDataType.Email);
         email.Required = true;
         userData.Content.Add(email);

         FormFieldList status = new FormFieldList(this, "status", "Estado", FormFieldList.ListType.Single);
         status.Values.Add(new Utils.KeyValue("Activo", ((int)User.UserStatus.Enabled).ToString()));
         status.Values.Add(new Utils.KeyValue("No verificado", ((int)User.UserStatus.NotVerified).ToString()));
         status.Values.Add(new Utils.KeyValue("Deshabilitado", ((int)User.UserStatus.Disabled).ToString()));
         status.Values.Add(new Utils.KeyValue("Bloqueado por seguridad", ((int)User.UserStatus.SecurityBloqued).ToString()));
         userData.Content.Add(status);

         HtmlContentControl usrProperties = new HtmlContentControl(this);
         usrProperties.AppendParagraph("Creado" + 
                                       HtmlContentControl.HTML_NEW_LINE +
                                       HtmlContentControl.BoldText(user.Created.ToString(Cosmo.Utils.Calendar.FORMAT_SHORTDATE)));
         usrProperties.AppendParagraph("Último acceso" +
                                       HtmlContentControl.HTML_NEW_LINE +
                                       HtmlContentControl.BoldText(user.LastLogon.ToString(Cosmo.Utils.Calendar.FORMAT_SHORTDATE)));
         usrProperties.AppendParagraph("Accesos" +
                                       HtmlContentControl.HTML_NEW_LINE +
                                       HtmlContentControl.BoldText(user.LogonCount));
         userData.Content.Add(usrProperties);

         ButtonControl usrSend = new ButtonControl(this, "usrSend", "Guardar", ButtonControl.ButtonTypes.Submit);
         userData.FormButtons.Add(usrSend);

         // Tab to show user data
         TabItemControl userDataTab = new TabItemControl(this);
         userDataTab.DomID = "userTab";
         userDataTab.Caption = "Datos de identificación";
         userDataTab.Icon = IconControl.ICON_TAG;
         userDataTab.Content.Add(userData);

         //-------------------------------
         // User public data form
         //-------------------------------

         publicData = new FormControl(this, "frmPData");
         publicData.Method = "POST";
         publicData.UsePanel = false;
         publicData.AddFormSetting(Cosmo.Workspace.PARAM_USER_ID, user.ID);

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
         publicDataTab.Icon = IconControl.ICON_EYE;
         publicDataTab.Content.Add(publicData);

         //-------------------------------
         // User notifications form
         //-------------------------------

         notifyData = new FormControl(this, "frmNotify");
         notifyData.Method = "POST";
         notifyData.UsePanel = false;
         notifyData.AddFormSetting(Cosmo.Workspace.PARAM_USER_ID, user.ID);

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

         //-------------------------------
         // User contact data form
         //-------------------------------

         contactData = new FormControl(this, "frmContact");
         contactData.Method = "POST";
         contactData.UsePanel = false;
         contactData.AddFormSetting(Cosmo.Workspace.PARAM_USER_ID, user.ID);

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

         //-------------------------------
         // User security data form
         //-------------------------------

         // Formulario para modificar contrasenya
         securityData = new FormControl(this, "frmSecurity");
         securityData.Method = "POST";
         securityData.UsePanel = false;
         securityData.AddFormSetting(Cosmo.Workspace.PARAM_USER_ID, user.ID);

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

         // Add tabs to tabbed control
         TabbedContainerControl tabs = new TabbedContainerControl(this);
         tabs.TabItems.Add(userDataTab);
         tabs.TabItems.Add(publicDataTab);
         tabs.TabItems.Add(notifyTab);
         tabs.TabItems.Add(contactTab);
         tabs.TabItems.Add(securityTab);
         MainContent.Add(tabs);

         //-------------------------------
         // Actions over user
         //-------------------------------

         ButtonControl btnTool;

         PanelControl adminPanel = new PanelControl(this);
         adminPanel.Text = "Opciones";

         btnTool = new ButtonControl(this);
         btnTool.Icon = IconControl.ICON_REMOVE;
         btnTool.Text = "Cancelar cuenta";
         btnTool.Color = ComponentColorScheme.Error;
         btnTool.IsBlock = true;
         btnTool.Href = Cosmo.Web.Handlers.SecurityRestHandler.GetCancelAccountUrl(user.ID).ToString();

         adminPanel.Content.Add(btnTool);

         RightContent.Add(adminPanel);

         //-------------------------------
         // Control data filling
         //-------------------------------

         // Load country list
         ((FormFieldList)publicData.GetField("pdc")).LoadValuesFromDataList("country");
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         try
         {
            if (receivedForm.DomID.Equals("frmUsrData"))
            {
               // Obtiene los datos del formulario enviado
               user.Mail = receivedForm.GetStringFieldValue("email");
               user.Status = (User.UserStatus)receivedForm.GetIntFieldValue("status");

               // Actualiza los datos del usuario actual
               Workspace.SecurityService.Update(user);
            }
            else if (receivedForm.DomID.Equals("frmPData"))
            {
               // Obtiene los datos del formulario enviado
               user.Name = receivedForm.GetStringFieldValue("pdname");
               user.Description = receivedForm.GetStringFieldValue("pddesc");
               user.City = receivedForm.GetStringFieldValue("pdcity");
               user.CountryID = receivedForm.GetIntFieldValue("pdc");

               // Actualiza los datos del usuario actual
               Workspace.SecurityService.Update(user);
            }
            else if (receivedForm.DomID.Equals("frmNotify"))
            {
               // Obtiene los datos del formulario enviado
               user.CanReceiveInternalMessages = receivedForm.GetBoolFieldValue("chkInternal");
               user.CanReceiveExternalMessages = receivedForm.GetBoolFieldValue("chkExternal");
               user.CanReceivePrivateMessagesNotify = receivedForm.GetBoolFieldValue("chkPM");

               // Actualiza los datos del usuario actual
               Workspace.SecurityService.Update(user);
            }
            else if (receivedForm.DomID.Equals("frmContact"))
            {
               // Obtiene los datos del formulario enviado
               user.MailAlternative = receivedForm.GetStringFieldValue("cmail");
               user.Phone = receivedForm.GetStringFieldValue("cphone");

               // Actualiza los datos del usuario actual
               Workspace.SecurityService.Update(user);
            }
            else if (receivedForm.DomID.Equals("frmSecurity"))
            {
               // Actualiza la contraseña
               Workspace.SecurityService.SetPassword(user.ID,
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
            case "frmUsrData":
               userData.SetFieldValue("email", user.Mail);
               userData.SetFieldValue("status", (int)user.Status);
               break;

            case "frmPData":
               publicData.SetFieldValue("pdname", user.Name);
               publicData.SetFieldValue("pddesc", user.Description);
               publicData.SetFieldValue("pdcity", user.City);
               publicData.SetFieldValue("pdc", user.CountryID);
               break;

            case "frmNotify":
               notifyData.SetFieldValue("chkInternal", user.CanReceiveInternalMessages);
               notifyData.SetFieldValue("chkExternal", user.CanReceiveExternalMessages);
               notifyData.SetFieldValue("chkPM", user.CanReceivePrivateMessagesNotify);
               break;

            case "frmContact":
               contactData.SetFieldValue("cmail", user.MailAlternative);
               contactData.SetFieldValue("cphone", user.Phone);
               break;
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <param name="userId">User identifier.</param>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL(int userId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_USER_ID, userId);

         return url.ToString();
      }

      #endregion

   }
}
