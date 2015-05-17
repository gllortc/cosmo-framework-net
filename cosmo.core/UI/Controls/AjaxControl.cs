namespace Cosmo.UI.Controls
{
   public class AjaxControl : Control
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="container"></param>
      /// <param name="domId"></param>
      public AjaxControl(ViewContainer container, string domId) 
         : base(container, domId)
      {
         Initialize();
      }

      public string URL { get; set; }

      private void Initialize()
      {
         
      }
   }
}
