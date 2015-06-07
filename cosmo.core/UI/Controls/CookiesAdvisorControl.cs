using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmo.UI.Controls
{
   public class CookiesAdvisorControl : Control
   {
      public const string SETTINGS_ENABLED = "cosmo.ui.cookiesadvisor.enabled";
      public const string SETTINGS_CONTENTID = "cosmo.ui.cookiesadvisor.infocontentid";

      public CookiesAdvisorControl(View parentView, string domId) 
         : base(parentView, domId)
      {

      }

      /// <summary>
      /// Return or set the message thes show this control to user.
      /// </summary>
      public string Message { get; set; }

      /// <summary>
      /// Return or set the content ID that contains more detailed information about legal aspects.
      /// </summary>
      public string InformationHref { get; set; }
   }
}
