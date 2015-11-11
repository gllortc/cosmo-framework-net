using Cosmo.Net.REST;

namespace Cosmo.UI.Scripting.Behaviors
{
   /// <summary>
   /// JavaScript Behavior:
   /// Muestra la información devuelta en una instancia de <see cref="AjaxResponse"/> por una llamada 
   /// a un <see cref="RestHandler"/>.
   /// </summary>
   public class ShowAjaxResponseBehavior : Script
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ShowAjaxResponseBehavior"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Una cadena que contiene la URL dónde se debe navegar.</param>
      public ShowAjaxResponseBehavior(View parentView, string domId)
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

      #endregion

      #region Script Implementation

      /// <summary>
      /// Devuelve el código de la senténcia JavaScript.
      /// </summary>
      /// <returns>Una cadena que contiene el código JavaScript solicitado.</returns>
      public override string GetSource()
      {
         Source.AppendLine("if (data.Result == " + (int)AjaxResponse.JsonResponse.Successful + ") {");
         Source.AppendLine("  $('#" + DomID + "').html(data.Xhtml);");
         Source.AppendLine("} else {");
         Source.AppendLine("  if (data.Xhtml != '') { $('#" + DomID + "').html(data.Xhtml); }");
         Source.AppendLine("  else { bootbox.alert(\"ERROR: \" + data.ErrorMessage); }");
         Source.AppendLine("}");

         return Source.ToString();
      }

      #endregion

   }
}
