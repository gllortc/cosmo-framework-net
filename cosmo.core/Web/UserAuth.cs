using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Reflection;

namespace Cosmo.Web
{
   public class UserAuth : PageView
   {

      #region PageView Implementation

      public override void LoadPage()
      {
         Title = "Control de acceso de usuarios";
         FadeBackground = true;

         LoginFormControl frmLogin = new LoginFormControl(this, "frmLogin");
         frmLogin.UserJoinUrl = UserJoin.GetURL();
         frmLogin.UserRememberPasswordUrl = UserPasswordRecovery.GetURL();
         frmLogin.RedirectionUrl = Parameters.GetString(Cosmo.Workspace.PARAM_LOGIN_REDIRECT);
         
         MainContent.Add(frmLogin);
      }

      public override void InitPage()
      {

      }

      /// <param name="receivedForm">Una instancia de <see cref="FormControl"/> que representa el formulario recibido. El formulario está actualizado con los datos recibidos.</param>
      public override void FormDataReceived(FormControl receivedForm)
      {
         // Nothing todo
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing todo
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL()
      {
         return UserAuth.GetURL(string.Empty);
      }

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL(string toUrl)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         if (!string.IsNullOrWhiteSpace(toUrl))
         {
            url.AddParameter(Cosmo.Workspace.PARAM_LOGIN_REDIRECT, toUrl);
         }

         return url.ToString();
      }

      #endregion

   }
}
