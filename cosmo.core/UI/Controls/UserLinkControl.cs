using Cosmo.Security;

namespace Cosmo.UI.Controls
{

   /// <summary>
   /// Implements a control that allow to open a modal form with its public data.
   /// </summary>
   public class UserLinkControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="UserLinkControl"/>.
      /// </summary>
      /// <param name="view">Container view of the control.</param>
      public UserLinkControl(View view)
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
      /// <param name="modalView">Modal view that the link opens.</param>
      public UserLinkControl(View view, int userId, string userDisplayName, ModalView modalView)
         : base(view)
      {
         Initialize();

         this.UserID = userId;
         this.UserDisplayName = userDisplayName;
         this.ModalView = modalView;
      }

      /// <summary>
      /// Gets an instance of <see cref="UserLinkControl"/>.
      /// </summary>
      /// <param name="view">Container view of the control.</param>
      /// <param name="user">An instance of <see cref="User"/> representing the user.</param>
      /// <param name="modalView">Modal view that the link opens.</param>
      public UserLinkControl(View view, User user, ModalView modalView)
         : base(view)
      {
         Initialize();

         this.UserID = user.ID;
         this.UserDisplayName = user.GetDisplayName();
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
      public ModalView ModalView { get; set; }

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
