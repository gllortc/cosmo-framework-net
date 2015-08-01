using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Reflection;

namespace Cosmo.Web
{
   /// <summary>
   /// Page that allows the user authenticate (login).
   /// </summary>
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
