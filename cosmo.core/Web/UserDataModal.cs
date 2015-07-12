using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System.Collections.Generic;

namespace Cosmo.Web
{

   /// <summary>
   /// Implements a view that show the user public data.
   /// </summary>
   [AuthenticationRequired]
   [ViewParameter(ParameterName = Workspace.PARAM_USER_ID,
                  PropertyName = "ThreadID")]
   public class UserDataModal : Cosmo.UI.ModalView
   {
      // Modal element unique identifier
      private const string DOM_ID = "user-data-modal";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="UserDataModal"/>.
      /// </summary>
      public UserDataModal()
      {
         this.DomID = UserDataModal.DOM_ID;
      }

      /// <summary>
      /// Gets an instance of <see cref="UserDataModal"/>.
      /// </summary>
      /// <param name="userId">User identifier.</param>
      public UserDataModal(int userId)
      {
         this.DomID = UserDataModal.DOM_ID;
         this.UserID = userId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the user identifier.
      /// </summary>
      public int UserID { get; set; }

      #endregion

      #region ModalViewContainer Implementation

      public override void InitPage()
      {
         User user = null;
         this.UserID = Parameters.GetInteger(Cosmo.Workspace.PARAM_USER_ID);

         Closeable = true;

         if (this.UserID > 0)
         {
            user = Workspace.SecurityService.GetUser(this.UserID);
         }

         if (user == null || user.Status != User.UserStatus.Enabled)
         {
            Icon = IconControl.ICON_PROHIBITED_CIRCLE;
            Title = "Usuario no encontrado";

            CalloutControl alert = new CalloutControl(this);
            alert.Type = ComponentColorScheme.Error;
            alert.Icon = IconControl.ICON_WARNING;
            alert.Title = "Usuario no encontrado";
            alert.Text = "No se encuentra (o no está disponible) el usuario solicitado y no pueden mostrarse sus datos públicos.";
            Content.Add(alert);

            return;
         }

         Icon = IconControl.ICON_USER;
         Title = user.Login;

         List<KeyValue> values = new List<KeyValue>();
         values.Add(new KeyValue("Nombre", user.GetDisplayName()));
         values.Add(new KeyValue("Localización", Workspace.SecurityService.GetUserLocation(user.ID)));
         values.Add(new KeyValue("Descripción", user.Description));
         values.Add(new KeyValue("Fecha de alta", user.Created.ToString(Formatter.FORMAT_SHORTDATE)));

         HtmlContentControl data = new HtmlContentControl(this);
         data.AppendDataList(values);

         Content.Add(data);
      }

      public override void FormDataReceived(UI.Controls.FormControl receivedForm)
      {
         // throw new NotImplementedException();
      }

      public override void FormDataLoad(string formDomID)
      {
         // throw new NotImplementedException();
      }

      public override void LoadPage()
      {
         // throw new NotImplementedException();
      }

      #endregion

   }
}
