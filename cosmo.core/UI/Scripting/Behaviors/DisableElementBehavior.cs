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
      /// Devuelve una instancia de <see cref="DisableElementBehavior"/>.
      /// </summary>
      /// <param name="viewport">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador del elemento.</param>
      /// <param name="disabled"><c>true</c> para inhabilitar el elemento o <c>false</c> para habilitarlo.</param>
      public DisableElementBehavior(ViewContainer parentViewport, string domId, bool disabled)
         : base(parentViewport)
      {
         this.DomId = domId;
         this.Disabled = disabled;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del elemento a habilitar/deshabilitar.
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
