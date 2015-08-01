namespace Cosmo.UI.Scripting.Behaviors
{
   /// <summary>
   /// JavaScript Behavior:
   /// Actualiza el contenido (XHTML) de un determinado elemento.
   /// </summary>
   public class UpdateHtmlBehavior : Script
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="NavigateBehavior"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Una cadena que contiene la URL dónde se debe navegar.</param>
      public UpdateHtmlBehavior(View parentView, string domId)
         : base(parentView)
      {
         this.DomID = domId;
      }

      #endregion
      
      #region Properties

      /// <summary>
      /// Gets or sets el identificador del elemento a actualizar.
      /// </summary>
      public string DomID { get; set; }

      public bool UseAjaxResponseContent { get; set; }

      #endregion

      #region Script Implementation

      /// <summary>
      /// Devuelve el código de la senténcia JavaScript.
      /// </summary>
      /// <returns>Una cadena que contiene el código JavaScript solicitado.</returns>
      public override string GetSource()
      {
         Source.AppendLine("$('#" + DomID + "').html();");

         return Source.ToString();
      }

      #endregion

   }
}
