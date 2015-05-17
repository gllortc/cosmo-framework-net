using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cosmo.UI.AspControls
{

   /// <summary>
   /// Summary description for CSAjaxMessagebox
   /// </summary>
   [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    ToolboxData("<{0}:CSAjaxMbox runat=\"server\"> </{0}:CSAjaxMbox>")]
   public class AjaxMessageboxControl : WebControl
   {
      /// <summary>
      /// Deprecated
      /// </summary>
      public enum MessageType
      {
         /// <summary>Deprecated</summary>
         Information,
         /// <summary>Deprecated</summary>
         Error
      }

      private string _boxTitle = "";
      private string _boxHref = "";
      private string _linkText = "";

      private const string ACTIVATOR_ID = "CSAjaxMessageBox";

      /// <summary>Deprecated</summary>
      public AjaxMessageboxControl()
      {
         //
         // TODO: Add constructor logic here
         //
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el título a mostrar en el MessageBox.
      /// </summary>
      public string MessageboxTitle
      {
         get { return _boxTitle; }
         set { _boxTitle = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece la URL que proporciona el contenido del MessageBox.
      /// </summary>
      public string MessageboxContentHref
      {
         get { return _boxHref; }
         set { _boxHref = value.Trim(); }
      }

      /// <summary>
      /// Devuelve o establece el texto (o fragmento HTML) título a mostrar en el MessageBox.
      /// </summary>
      public string LinkText
      {
         get { return _linkText; }
         set { _linkText = value.Trim(); }
      }

      #endregion

      #region Event Handlers

      /// <summary>
      /// The contacts are rendered in an HTML table.
      /// </summary>
      protected override void RenderContents(HtmlTextWriter writer)
      {
         string xhtml = "";

         xhtml = "<a href=\"" + _boxHref + "\" rel=\"" + ACTIVATOR_ID + "\" title=\"" + HttpUtility.HtmlEncode(_boxTitle) + "\" >" + HttpUtility.HtmlEncode(_linkText) + "</a>";

         writer.Write(xhtml);
      }

      #endregion

   }
}
