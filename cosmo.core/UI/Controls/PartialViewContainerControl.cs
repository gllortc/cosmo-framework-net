using Cosmo.UI.Scripting;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implements a control that can show a partial view.
   /// </summary>
   public class PartialViewContainerControl : Control
   {
      // Internal data declarations
      private PartialView view;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PartialViewContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public PartialViewContainerControl(View parentView, string domId)
         : base(parentView, domId)
      {
         this.View = null;
      }

      /// <summary>
      /// Gets a new instance of <see cref="PartialViewContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="view">An instance of partial view which will be shown in this control.</param>
      public PartialViewContainerControl(View parentView, PartialView view)
         : base(parentView, view.DomID)
      {
         this.View = view;
      }

      /// <summary>
      /// Gets a new instance of <see cref="PartialViewContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="view">An instance of partial view which will be shown in this control.</param>
      /// <param name="executionType">Type of script execution.</param>
      /// <param name="parameters">Partial view parameters.</param>
      public PartialViewContainerControl(View parentView, PartialView view, Script.ScriptExecutionMethod executionType, params object[] parameters)
         : base(parentView, view.DomID)
      {
         this.View = view;

         // Adds the script to view to avoid load partial view on view load
         this.ParentView.Scripts.Add(view.GetInvokeScript(Script.ScriptExecutionMethod.OnDocumentReady, parameters));
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the partial view associated with the control.
      /// </summary>
      public PartialView View { get; set; }

      #endregion

   }
}
