namespace Cosmo.UI.Scripting.Behaviors
{
   /// <summary>
   /// JavaScript Behavior:
   /// Habilita/deshabilita un determinado elemento del DOM.
   /// </summary>
   public class DisableElementBehavior : Script
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="DisableElementBehavior"/>.
      /// </summary>
      /// <param name="viewport">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Identificador del elemento.</param>
      /// <param name="disabled"><c>true</c> para inhabilitar el elemento o <c>false</c> para habilitarlo.</param>
      public DisableElementBehavior(View parentViewport, string domId, bool disabled)
         : base(parentViewport)
      {
         this.DomId = domId;
         this.Disabled = disabled;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del elemento a habilitar/deshabilitar.
      /// </summary>
      public string DomId { get; set; }

      /// <summary>
      /// Indica si el elemento seleccionado se debe habilitar o deshabilitar.
      /// </summary>
      public bool Disabled { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve el código de la senténcia JavaScript.
      /// </summary>
      /// <returns>Una cadena que contiene el código JavaScript solicitado.</returns>
      public override string GetSource()
      {
         return "$('#" + DomId + "').prop('disabled', " + (Disabled ? "true" : "false") + ");";
      }

      #endregion

   }
}
