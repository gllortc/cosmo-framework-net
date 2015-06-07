namespace Cosmo.UI.Controls
{
   public class AjaxControl : Control
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="ownerView"></param>
      /// <param name="domId"></param>
      public AjaxControl(View ownerView, string domId) 
         : base(ownerView, domId)
      {
         Initialize();
      }

      public string URL { get; set; }

      private void Initialize()
      {
         
      }
   }
}
