using Cosmo.Data.ORM;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Reflection;

namespace Cosmo.Web
{
   /// <summary>
   /// Formulario de alta para nuevos usuarios.
   /// </summary>
   /// <remarks>
   /// Author : Gerard Llort
   /// Version: 1.0.0
   /// Copyright (c) InforGEST
   /// </remarks>
   public class UserJoin : PageView
   {
      private FormControl form;

      #region PageView Implementation

      public override void InitPage()
      {
         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = "Crear nueva cuenta de usuario";
         ActiveMenuId = string.Empty;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = "Crear nueva cuenta de usuario";
         header.Icon = IconControl.ICON_USER;
         MainContent.Add(header);

         // Genera el formulario para objetos del tipo User
         OrmEngine orm = new OrmEngine();
         form = orm.CreateForm(this, "frmJoin", typeof(User), true);
         
         MainContent.Add(form);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         OrmEngine orm = new OrmEngine();

         try
         {
            // Rellena una instancia con los datos
            User user = new User();
            if (orm.ProcessForm(user, form, Parameters))
            {
               // Genera la nueva cuenta de usuario
               Workspace.SecurityService.Create(user);

               MainContent.Clear();

               CalloutControl waitBox = new CalloutControl(this);
               waitBox.DomID = "result-box";
               waitBox.Title = "Suscripción correcta!";
               waitBox.Icon = IconControl.ICON_CHECK;
               waitBox.Text = "Para finalizar el registro hemos enviado un correo a la cuenta " + user.Mail + " con un enlace que sirve para verificar los datos. Mira en tu buzón de correo y sigue las instrucciones que encontrarán en dicho correo.";
               waitBox.Type = ComponentColorScheme.Success;

               PanelControl panel = new PanelControl(this);
               panel.Text = "Verificación de cuentas de usuario";
               panel.Content.Add(waitBox);
               panel.Footer.Add(new ButtonControl(this, "btnLogin", "Ir al inicio", IconControl.ICON_HOME, "Home", string.Empty));

               MainContent.Add(panel);
            }
         }
         catch (Exception ex)
         {
            ShowError(ex);
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
