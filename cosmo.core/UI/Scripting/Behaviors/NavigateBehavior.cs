namespace Cosmo.UI.Scripting.Behaviors
{
   /// <summary>
   /// JavaScript Behavior:
   /// Navega a una determinada URL.
   /// </summary>
   public class NavigateBehavior : Script
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="NavigateBehavior"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="url">Una cadena que contiene la URL dónde se debe navegar.</param>
      public NavigateBehavior(View parentView, string url)
         : base(parentView)
      {
         this.URL = url;
      }

      #endregion

      #region Script Implementation

      /// <summary>
      /// Devuelve el código de la senténcia JavaScript.
      /// </summary>
      /// <returns>Una cadena que contiene el código JavaScript solicitado.</returns>
      public override string GetSource()
      {
         return "window.location.href = '" + URL + "';";
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets la URL dónde se desea navegar.
      /// </summary>
      public string URL { get; set; }

      #endregion

   }
}
