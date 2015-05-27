using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmo.UI.Controls
{
   public class UserLinkControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="UserLinkControl"/>.
      /// </summary>
      /// <param name="view">Container view of the control.</param>
      public UserLinkControl(ViewContainer view)
         : base(view)
      {
         Initialize();
      }

      /// <summary>
      /// Gets an instance of <see cref="UserLinkControl"/>.
      /// </summary>
      /// <param name="view">Container view of the control.</param>
      /// <param name="userId">User unique identifier.</param>
      /// <param name="userDisplayName">User display name (usually the <c>login</c>.</param>
      public UserLinkControl(ViewContainer view, int userId, string userDisplayName, ModalViewContainer modalView)
         : base(view)
      {
         Initialize();

         this.UserID = userId;
         this.UserDisplayName = userDisplayName;
         this.ModalView = modalView;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the user unique identifier.
      /// </summary>
      public int UserID { get; set; }

      /// <summary>
      /// Gets or sets the user display name (usually the <c>login</c>.
      /// </summary>
      public string UserDisplayName { get; set; }

      /// <summary>
      /// Gets or sets the modal view used to show the user data.
      /// </summary>
      public ModalViewContainer ModalView { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance.
      /// </summary>
      private void Initialize()
      {
         this.UserID = 0;
         this.UserDisplayName = string.Empty;
         this.ModalView = null;
      }

      #endregion

   }
}
