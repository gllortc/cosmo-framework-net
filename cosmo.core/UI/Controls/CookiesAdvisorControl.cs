namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implements a control that show a message to user that can be hidden permanently by the user.
   /// </summary>
   public class CookiesAdvisorControl : Control
   {
      public const string SETTINGS_ENABLED = "cosmo.ui.cookiesadvisor.enabled";
      public const string SETTINGS_CONTENTID = "cosmo.ui.cookiesadvisor.infocontentid";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="CookiesAdvisorControl"/>.
      /// </summary>
      /// <param name="parentView"></param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public CookiesAdvisorControl(View parentView, string domId) 
         : base(parentView, domId)
      {

      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the message thes show this control to user.
      /// </summary>
      public string Message { get; set; }

      /// <summary>
      /// Gets or sets the content ID that contains more detailed information about legal aspects.
      /// </summary>
      public string InformationHref { get; set; }

      #endregion

   }
}
