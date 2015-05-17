using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cosmo.UI.AspControls
{
   /// <summary>
   /// Implementa el control TAB.
   /// </summary>
   [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Tabs"),
    ParseChildren(true, "tabs"),
    ToolboxData("<{0}:CSTab runat=\"server\"> </{0}:CSTab>")]
   public class CSTabControl : WebControl
   {
      private List<Tab> _tabs;

      [Category("Behavior"),
       Description("Tab Collection"),
       DesignerSerializationVisibility(
       DesignerSerializationVisibility.Content),
       Editor(typeof(TabCollectionEditor), typeof(UITypeEditor)),
       PersistenceMode(PersistenceMode.InnerDefaultProperty)]
      public List<Tab> Tabs
      {
         get
         {
            if (_tabs == null)
            {
               _tabs = new List<Tab>();
            }
            return _tabs;
         }
      }
      
      /// <summary>
      /// The contacts are rendered in an HTML table.
      /// </summary>
      protected override void RenderContents(HtmlTextWriter writer)
      {
         bool selected = false;
         string xhtml = "";

         xhtml += "<div id=\"tab\">\n";
         xhtml += "<ul>\n";
         foreach (Tab tab in _tabs)
         {
            xhtml += "   <li" + (tab.Selected && !selected ? " id=\"selected\"" : "") + "><a href=\"" + tab.Link + "\">" + tab.Title + "</a></li>\n";
            if (tab.Selected) selected = true;
         }
         xhtml += "</ul>\n";
         xhtml += "</div>\n";

         writer.Write(xhtml);
      }
   }
}

