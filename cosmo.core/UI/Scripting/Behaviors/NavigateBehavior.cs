﻿namespace Cosmo.UI.Scripting.Behaviors
{
   /// <summary>
   /// JavaScript Behavior:
   /// Navega a una determinada URL.
   /// </summary>
   public class NavigateBehavior : Script
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="NavigateBehavior"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="url">Una cadena que contiene la URL dónde se debe navegar.</param>
      public NavigateBehavior(ViewContainer container, string url)
         : base(container)
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
      /// Devuelve o establece la URL dónde se desea navegar.
      /// </summary>
      public string URL { get; set; }

      #endregion

   }
}
