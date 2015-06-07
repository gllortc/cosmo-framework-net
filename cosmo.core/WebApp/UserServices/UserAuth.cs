using Cosmo.UI;
using Cosmo.UI.Controls;

namespace Cosmo.WebApp.UserServices
{
   public class UserAuth : PageView
   {
      public override void LoadPage()
      {
         Title = "Control de acceso de usuarios";
         FadeBackground = true;

         LoginFormControl frmLogin = new LoginFormControl(this, "frmLogin");
         frmLogin.UserJoinUrl = Cosmo.Workspace.COSMO_URL_JOIN;
         frmLogin.UserRememberPasswordUrl = Cosmo.Workspace.COSMO_URL_PASSWORD_RECOVERY;
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
   }
}
