using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Reflection;

namespace Cosmo.Web
{
   /// <summary>
   /// Formulario de verificación de nuevas cuentas de usuario.
   /// </summary>
   public class UserJoinVerification : PageView
   {
      /// <summary>Parámetro que contiene la clave de verificación de una cuenta de correo.</summary>
      private const string PARAM_VERIFY_KEY = "data";

      #region PageView Implementation

      public override void InitPage()
      {
         string verificationKey;

         //-------------------------------
         // Obtiene los parámetros de la llamada
         //-------------------------------

         verificationKey = Parameters.GetString(UserJoinVerification.PARAM_VERIFY_KEY);


         //-------------------------------
         // Configuración de la página
         //-------------------------------

         Title = "Verificación de una nueva cuenta de usuario";
         ActiveMenuId = string.Empty;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = "Crear nueva cuenta de usuario";
         header.Icon = IconControl.ICON_USER;
         MainContent.Add(header);

         if (!string.IsNullOrWhiteSpace(verificationKey))
         {
            try
            {
               // User user = auth.Verify(Request.QueryString);
               User user = Workspace.SecurityService.Verify(Request.QueryString);

               Workspace.Logger.Security("Suscripción verificada de " + user.Login + " desde " + 
                                         Request.ServerVariables["REMOTE_ADDR"]);

               CalloutControl waitBox = new CalloutControl(this);
               waitBox.DomID = "result-box";
               waitBox.Title = "Verificación correcta";
               waitBox.Icon = IconControl.ICON_CHECK;
               waitBox.Text = "Enhorabuena! El registro de la cuenta <strong>" + user.Login.ToUpper() + "</strong> se ha completado correctamente. Ya puede entrar como usuario a <strong>" + Workspace.Name + "</strong>";
               waitBox.Type = ComponentColorScheme.Success;

               PanelControl panel = new PanelControl(this);
               panel.Text = "Verificación de cuentas de usuario";
               panel.Content.Add(waitBox);
               panel.Footer.Add(new ButtonControl(this, "btnLogin", "Iniciar sesión", IconControl.ICON_USER, "UserAuth", string.Empty));

               MainContent.Add(panel);
            }
            catch (Exception ex)
            {
               CalloutControl waitBox = new CalloutControl(this);
               waitBox.Title = "Upssss! Se ha producido un error";
               waitBox.Icon = IconControl.ICON_WARNING;
               waitBox.Text = "La cuenta de usuario no ha podido ser verificada debido a un error.<br /><br />Puede probar de repetir la acción y si el error persiste ponerse en contacto con nosotros para verificar su cuenta manualmente.";
               waitBox.Type = ComponentColorScheme.Error;
               MainContent.Add(waitBox);
            }
         }
         else
         {
            CalloutControl waitBox = new CalloutControl(this);
            waitBox.Title = "Upssss";
            waitBox.Icon = IconControl.ICON_WARNING;
            waitBox.Text = "No se ha recibido la información necesaria para verificar la cuenta. Se ha cancelado la verificación de la cuenta de usuario.";
            waitBox.Type = ComponentColorScheme.Error;
            MainContent.Add(waitBox);
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL(string qs)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter("mode", "verify");
         url.AddParameter("data", qs);

         return url.ToString();
      }

      #endregion

   }
}
