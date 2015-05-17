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
      /// Devuelve una instancia de <see cref="NavigateBehavior"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="url">Una cadena que contiene la URL dónde se debe navegar.</param>
      public UpdateHtmlBehavior(ViewContainer container, string domId)
         : base(container)
      {
         this.DomID = domId;
      }

      #endregion
      
      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del elemento a actualizar.
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
