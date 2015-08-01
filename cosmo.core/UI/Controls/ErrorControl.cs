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
      /// Gets a new instance of <see cref="ErrorControl"/>
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public ErrorControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ErrorControl"/>
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
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
      /// Gets or sets la instancia de <see cref="Exception"/> que contiene los detalles a mostrar.
      /// </summary>
      public Exception Exception { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {

      }

      #endregion

   }
}
