using System;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un aviso de error.
   /// </summary>
   public class ErrorControl : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ErrorControl"/>
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public ErrorControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ErrorControl"/>
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="exception">La excepción que contiene los detalles a mostrar.</param>
      public ErrorControl(View parentView, Exception exception)
         : base(parentView)
      {
         Initialize();

         Exception = exception;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece la instancia de <see cref="Exception"/> que contiene los detalles a mostrar.
      /// </summary>
      public Exception Exception { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {

      }

      #endregion

   }
}
