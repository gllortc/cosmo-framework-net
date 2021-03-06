﻿using Cosmo.Data.ORM;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Reflection;

namespace Cosmo.Web
{
   public class UserPasswordRecovery : PageView
   {
      // Internal data declarations
      private bool showForm;

      #region PageView Implementation

      public override void InitPage()
      {
         showForm = true;

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = "Crear nueva cuenta de usuario";
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
         header.Title = "Recuperar contraseña";
         header.Icon = IconControl.ICON_USER;
         MainContent.Add(header);

         FormControl pwdform = new FormControl(this, "pwdform");
         pwdform.Text = "Recuperar datos de conexión";

         HtmlContentControl hDesc = new HtmlContentControl(this);
         hDesc.AppendParagraph("Por favor proporciona la dirección de correo que usaste al registrar tu cuenta. Te enviaremos un correo que te permitirá reinicializar tu contraseña.");
         pwdform.Content.Add(hDesc);

         FormFieldText fldMail = new FormFieldText(this, "mail", "Correo electrónico", FormFieldText.FieldDataType.Email);
         fldMail.Description = "Esta cuenta debe ser la cuenta con la que te registraste.";
         fldMail.Required = true;
         pwdform.Content.Add(fldMail);

         FormFieldCaptcha fldCaptcha = new FormFieldCaptcha(this, "ver", "Código de verificación");
         pwdform.Content.Add(fldCaptcha);

         pwdform.FormButtons.Add(new ButtonControl(this, "cmdSend", "Solicitar datos", ButtonControl.ButtonTypes.Submit));
         
         MainContent.Add(pwdform);
         /*
         // Genera el formulario para objetos del tipo PasswordRecoveryData
         OrmEngine orm = new OrmEngine();
         modal = orm.CreateForm(this, typeof(PasswordRecoveryData), true);
         */
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         OrmEngine orm = new OrmEngine();

         try
         {
            // Rellena una instancia con los datos
            /*PasswordRecoveryData user = new PasswordRecoveryData();
            if (orm.ProcessForm(user, modal, Parameters))
            {*/
               // Genera la nueva cuenta de usuario
            Workspace.SecurityService.SendData(receivedForm.GetStringFieldValue("mail"));

               CalloutControl callout = new CalloutControl(this, ComponentColorScheme.Success);
               callout.Icon = IconControl.ICON_CHECK;
               callout.Title = "Datos enviados";
               callout.Text = "En breves instantes recibirás un correo electrónico que contiene los " +
                              "datos para que puedas acceder nuevamente a tu cuenta de usuario de " + 
                              "<strong>" + Workspace.Name + "</strong>. Si tienes algún problema o duda, " +
                              "puedes  <a href=\"" + Cosmo.Workspace.COSMO_URL_CONTACT + "\">contactar nosotros</a>.";

               ButtonGroupControl btnBar = new ButtonGroupControl(this);
               btnBar.Buttons.Add(new ButtonControl(this, "btnLogin", "Iniciar sesión", IconControl.ICON_USER, "UserAuth", string.Empty));
               btnBar.Buttons.Add(new ButtonControl(this, "btnHome", "Volver al inicio", IconControl.ICON_HOME, "Home", string.Empty));

               PanelControl panel = new PanelControl(this);
               panel.Content.Add(callout);
               panel.Footer.Add(btnBar);
               MainContent.Add(panel);

               showForm = false;
            // }
         }
         catch (UserNotFoundException ex)
         {
            CalloutControl callout = new CalloutControl(this, ComponentColorScheme.Warning);
            callout.Icon = IconControl.ICON_WARNING;
            callout.Title = "Dirección de correo electrónico no encontrada";
            callout.Text = ex.Message;

            ButtonGroupControl btnBar = new ButtonGroupControl(this);
            btnBar.Buttons.Add(new ButtonControl(this, "btnRepeat", "Repetir petición", IconControl.ICON_REPLY, "UserPasswordRecovery", string.Empty));
            btnBar.Buttons.Add(new ButtonControl(this, "btnHome", "Volver al inicio", IconControl.ICON_HOME, "Home", string.Empty));

            PanelControl panel = new PanelControl(this);
            panel.Content.Add(callout);
            panel.Footer.Add(btnBar);
            MainContent.Add(panel);

            showForm = false;
         }
         catch (Exception)
         {
            CalloutControl callout = new CalloutControl(this, ComponentColorScheme.Error);
            callout.Icon = IconControl.ICON_WARNING;
            callout.Title = "Opssss! Se ha producido un error";
            callout.Text = "Se ha producido un error durante el procesado de su petición de datos y no se ha podido enviar el correo.";

            ButtonGroupControl btnBar = new ButtonGroupControl(this);
            btnBar.Buttons.Add(new ButtonControl(this, "btnRepeat", "Repetir petición", IconControl.ICON_REPLY, "UserPasswordRecovery", string.Empty));
            btnBar.Buttons.Add(new ButtonControl(this, "btnHome", "Volver al inicio", IconControl.ICON_HOME, "Home", string.Empty));

            PanelControl panel = new PanelControl(this);
            panel.Content.Add(callout);
            panel.Footer.Add(btnBar);
            MainContent.Add(panel);

            showForm = false;
         }
      }

      public override void LoadPage()
      {
         if (!showForm)
         {
            // MainContent.Add(modal);
            MainContent.Remove("pwdform");
         }
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
