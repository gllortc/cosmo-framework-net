using System;
using System.Text;
using System.Web;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el formulario de inicio de sesión estandarizado de Cosmo.
   /// </summary>
   public class LoginFormControl : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="LoginFormControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Una cadena que contiene el identificador único del formulario dentro del DOM.</param>
      public LoginFormControl(ViewContainer container, string domId)
         : base(container, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="LoginFormControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Una cadena que contiene el identificador único del formulario dentro del DOM.</param>
      /// <param name="openButtonText">Texto que aparece en el botón de abrir el formulario.</param>
      public LoginFormControl(ViewContainer container, string domId, string openButtonText)
         : base(container, domId)
      {
         Initialize();

         this.OpenButtonCaption = openButtonText;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el título del formulario.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Devuelve o establece el texto que debe contener el botón de apertura del Login.
      /// </summary>
      public string OpenButtonCaption { get; set; }

      /// <summary>
      /// Devuelve o establece la URL a la que se debe acceder cuando se complete con éxito la autenticación.
      /// </summary>
      public string RedirectionUrl { get; set; }

      /// <summary>
      /// Devuelve o establece la URL a la que se debe acceder para crear una nueva cuenta de usuario.
      /// </summary>
      public string UserJoinUrl { get; set; }

      /// <summary>
      /// Devuelve o establece la URL a la que se debe acceder para recuperar la contraseña.
      /// </summary>
      public string UserRememberPasswordUrl { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene un enlace que abre el formulario de <em>login</em>.
      /// </summary>
      [Obsolete]
      public string GetOpenButton()
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<button class=\"btn btn-primary btn-lg\" data-toggle=\"modal\" data-target=\"#" + this.DomID + "\">");
         xhtml.AppendLine("  " + HttpUtility.HtmlDecode(this.OpenButtonCaption));
         xhtml.AppendLine("</button>");

         return xhtml.ToString();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Title = "Identificación de usuario";
         this.OpenButtonCaption = "Iniciar sesión";
         this.RedirectionUrl = string.Empty;
         this.UserJoinUrl = string.Empty;
         this.UserRememberPasswordUrl = string.Empty;
      }

      #endregion

      #region Disabled Code

      /*
       
      /// <summary>
      /// Obtiene un elemento de barra menús que se puede insertar en un elemento NavBar.
      /// </summary>
      [Obsolete]
      public string GetNavbarButton()
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<li><a data-toggle=\"modal\" data-target=\"#" + this.DomID + "\"><span class=\"glyphicon glyphicon-user\"></span> " + HttpUtility.HtmlDecode(_btnText) + "</a></li>");

         return xhtml.ToString();
      }
      
      /// <summary>
      /// Genera el código XHTML (y JavaScript) necesario para incrustar el formulario como parte de una página.
      /// </summary>
      /// <param name="showCloseButton">Indica si se debe mostrar el botón de cerrar formulario.</param>
      /// <returns>Una cadena XHTML (y JavaScript) que permite representar el formulario en el cliente.</returns>
      public string GetLoginForm(bool showCloseButton)
      {
         StringBuilder xhtml = new StringBuilder();

         if (string.IsNullOrEmpty(_toUrl) && _request != null)
         {
            _toUrl = _request.RawUrl;
         }

         xhtml.AppendLine("  <div class=\"modal-dialog\" style=\"width:400px;\">");
         xhtml.AppendLine("    <div class=\"modal-content\">");
         xhtml.AppendLine("      <div class=\"modal-header\">");
         if (showCloseButton) xhtml.AppendLine("        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>");
         xhtml.AppendLine("        <h4 class=\"modal-title\" id=\"" + this.DomID + "Label\">" + Glyphicon.GetIcon(Glyphicon.ICON_USER) + "&nbsp;&nbsp;Iniciar sesión de usuario</h4>");
         xhtml.AppendLine("      </div>");
         xhtml.AppendLine("      <div class=\"modal-body\">");
         xhtml.AppendLine("        <div class=\"well\" style=\"max-width:400px;margin:0 auto 10px;\">");
         xhtml.AppendLine("          <a href=\"" + Workspace.COSMO_URL_JOIN + "\" class=\"btn btn-primary btn-block\">Crear una nueva cuenta</a>");
         xhtml.AppendLine("        </div>");
         xhtml.AppendLine("      </div>");
         xhtml.AppendLine("      <hr style=\"margin:0;\" />");
         xhtml.AppendLine("      <modal id=\"" + this.DomID + "HForm\" class=\"modal-horizontal\" role=\"modal\">");
         xhtml.AppendLine("        <input type=\"hidden\" id=\"" + Workspace.PARAM_COMMAND + "\" name=\"" + Workspace.PARAM_COMMAND + "\" value=\"" + UserHandler.COMMAND_USER_AUTHENTICATION + "\" />");
         xhtml.AppendLine("        <input type=\"hidden\" id=\"" + Workspace.PARAM_LOGIN_REDIRECT + "\" name=\"" + Workspace.PARAM_LOGIN_REDIRECT + "\" value=\"" + (string.IsNullOrEmpty(_toUrl) ? Workspace.COSMO_URL_DEFAULT : _toUrl) + "\" />");
         xhtml.AppendLine("        <div class=\"modal-body\">");
         xhtml.AppendLine("          <div id=\"" + this.DomID + "Msgbox\"></div>");
         xhtml.AppendLine("          <div class=\"input-group\" style=\"margin-bottom:15px;\">");
         xhtml.AppendLine("            <span class=\"input-group-addon\">" + Glyphicon.GetIcon(Glyphicon.ICON_USER) + "</span>");
         xhtml.AppendLine("            <input type=\"text\" class=\"modal-control\" id=\"txtLogin\" name=\"txtLogin\" placeholder=\"Tu nombre de usuario\" required=\"required\" />");
         xhtml.AppendLine("          </div>");
         xhtml.AppendLine("          <div class=\"input-group\">");
         xhtml.AppendLine("            <span class=\"input-group-addon\">" + Glyphicon.GetIcon(Glyphicon.ICON_LOCK) + "</span>");
         xhtml.AppendLine("            <input type=\"password\" class=\"modal-control\" id=\"txtPwd\" name=\"txtPwd\" placeholder=\"Tu contraseña\" required=\"required\" />");
         xhtml.AppendLine("          </div>");
         xhtml.AppendLine("          <span class=\"help-block\"><small><a href=\"" + Workspace.COSMO_URL_PASSWORD_RECOVERY + "\">¿Has olvidado tu contraseña?</a></small></span>");
         xhtml.AppendLine("        </div>");
         xhtml.AppendLine("        <div class=\"modal-footer\">");
         xhtml.AppendLine("          <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Cerrar</button>");
         xhtml.AppendLine("          <button type=\"button\" class=\"btn btn-primary\" id=\"cmdLogin\">Iniciar sesión</button>");
         xhtml.AppendLine("        </div>");
         xhtml.AppendLine("      </modal>");
         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine(this.GetJScript());

         return xhtml.ToString();
      }
      
      /// <summary>
      /// Convierte el componente en código XHTML.
      /// </summary>
      /// <returns>Una cadena que contiene el código XHTML necesario para representar el componente en un navegador.</returns>
      public string ToXhtml()
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + GetIdParameter() + "class=\"modal fade\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"" + this.DomID + "Label\" aria-hidden=\"true\">");
         xhtml.AppendLine(this.GetLoginForm(true));
         xhtml.AppendLine("</div>");
         xhtml.AppendLine(this.GetJScript());

         return xhtml.ToString();
      }

      /// <summary>
      /// Genera un script JavaScript que permite controlar el formulario.
      /// </summary>
      private string GetJScript()
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<script type=\"text/javascript\">");
         xhtml.AppendLine("  $(\"#cmdLogin\").click(function() {");
         xhtml.AppendLine("    $(\"#" + this.DomID + "HForm\").submit(function(e) {");
         xhtml.AppendLine("      var postData = $(this).serializeArray();");
         xhtml.AppendLine("      $.ajax({");
         xhtml.AppendLine("        url     : \"users.do\",");
         xhtml.AppendLine("        type    : \"POST\",");
         xhtml.AppendLine("        data    : postData,");
         xhtml.AppendLine("        success : function(data, textStatus, jqXHR) {");
         xhtml.AppendLine("                    if (data.response == 'ok') {");
         xhtml.AppendLine("                      $('#" + this.DomID + "Msgbox').Html('<div class=\"alert alert-success\">Autenticación correcta!</div>');");
         xhtml.AppendLine("                      window.location = data.tourl;");
         xhtml.AppendLine("                    }");
         xhtml.AppendLine("                    else if (data.response == 'err') {");
         xhtml.AppendLine("                      if (data.code == '1001') {");
         xhtml.AppendLine("                        $('#" + this.DomID + "Msgbox').Html('<div class=\"alert alert-danger\">" + ParentViewport.Workspace.UIService. Icon.GetIcon(Icon.ICON_WARNING) + "&nbsp;&nbsp;Esta cuenta actualmente no tiene acceso.</div>');");
         xhtml.AppendLine("                      } else if (data.code == '1002') {");
         xhtml.AppendLine("                        $('#" + this.DomID + "Msgbox').Html('<div class=\"alert alert-warning\">" + Icon.GetIcon(Icon.ICON_WARNING) + "&nbsp;&nbsp;Esta cuenta está pendiente de verificación y aun no tiene acceso. Revise su correo, debe tener un correo con las instrucciones para verificar esta cuenta.</div>');");
         xhtml.AppendLine("                      } else {");
         xhtml.AppendLine("                        $('#" + this.DomID + "Msgbox').Html('<div class=\"alert alert-danger\">" + Icon.GetIcon(Icon.ICON_WARNING) + "&nbsp;&nbsp;El usuario y/o la contraseña son incorrectos.</div>');");
         xhtml.AppendLine("                      }");
         xhtml.AppendLine("                    }");
         xhtml.AppendLine("                  },");
         xhtml.AppendLine("        error   : function(jqXHR, textStatus, errorThrown) {");
         xhtml.AppendLine("                     $('#" + this.DomID + "Msgbox').Html('<div class=\"alert alert-danger\">" + Icon.GetIcon(Icon.ICON_WARNING) + "&nbsp;&nbsp;<strong>Ooooops!</strong> No se ha podido realizar la autenticación a causa de un error.</div>');");
         xhtml.AppendLine("                  }");
         xhtml.AppendLine("      });");
         xhtml.AppendLine("      e.preventDefault();");
         xhtml.AppendLine("      e.unbind();");
         xhtml.AppendLine("    });");
         xhtml.AppendLine("    $(\"#" + this.DomID + "HForm\").submit();");
         xhtml.AppendLine("  });");
         xhtml.AppendLine("</script>");

         return xhtml.ToString();
      }*/

      #endregion

   }
}
