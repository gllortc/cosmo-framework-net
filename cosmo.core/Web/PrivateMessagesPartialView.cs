using Cosmo.Communications.PrivateMessages;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using System.Reflection;

namespace Cosmo.Web
{
   /// <summary>
   /// Implements a view of message thread with current user and another user.
   /// </summary>
   [AuthenticationRequired]
   [ViewParameter(ParameterName = Cosmo.Workspace.PARAM_USER_ID,
                  PropertyName = "RemoteUserID")]
   public class PrivateMessagesPartialView : PartialView
   {
      private ChatControl chat = null;

      // Modal element unique identifier
      private const string DOM_ID = "ads-th-list";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="PrivateMessagesPartialView"/>.
      /// </summary>
      public PrivateMessagesPartialView()
         : base(PrivateMessagesPartialView.DOM_ID)
      {
         this.RemoteUserID = 0;
      }

      /// <summary>
      /// Gets an instance of <see cref="PrivateMessagesPartialView"/>.
      /// </summary>
      /// <param name="remoteUserID">Remote user ID</param>
      public PrivateMessagesPartialView(int remoteUserID)
         : base(PrivateMessagesPartialView.DOM_ID)
      {
         this.RemoteUserID = remoteUserID;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the remote user ID.
      /// </summary>
      public int RemoteUserID { get; set; }

      #endregion

      #region PageView Implementation

      public override void InitPage()
      {
         //
         // Get URL parameters
         //
         if (this.RemoteUserID <= 0)
         {
            this.RemoteUserID = Parameters.GetInteger(Workspace.PARAM_USER_ID);
         }

         // If no user selected, shows the new conversation form
         if (this.RemoteUserID <= 0)
         {
            FormFieldAutocomplete txtAuto = new FormFieldAutocomplete(this, "txtName");
            txtAuto.SearchUrl = "serachUrl";

            FormControl frmNewThread = new FormControl(this, "frmNewTh");
            frmNewThread.UsePanel = true;
            frmNewThread.Content.Add(txtAuto);

            Content.Add(frmNewThread);

            CalloutControl callout = new CalloutControl(this);
            callout.Type = ComponentColorScheme.Information;
            callout.Title = "Seleccione una conversación";
            callout.Text = "Seleccione la conversación (en la parte derecha) para ver los mensajes con una determinada persona.";

            Content.Add(callout);

            return;
         }

         //
         // Chat control
         //
         chat = new ChatControl(this, this.RemoteUserID);
         chat.DomID = "pmChat";
         chat.Action = this.GetType().Name;

         User remoteUser = Workspace.SecurityService.GetUser(this.RemoteUserID);
         chat.Text = "Mensajes con " + remoteUser.GetDisplayName();

         Content.Add(chat);

         //
         // Message sending form
         //
         FormFieldText txtMsg = new FormFieldText(this, "txtMsg");
         txtMsg.Required = true;
         txtMsg.Placeholder = "Type message...";

         FormControl frmMessage = new FormControl(this, "frmChatMsg");
         frmMessage.UsePanel = true;
         frmMessage.AddFormSetting(Workspace.PARAM_USER_ID, this.RemoteUserID);
         frmMessage.Content.Add(txtMsg);
         frmMessage.FormButtons.Add(new ButtonControl(this, "cmdSendMsg", "Send", IconControl.ICON_SEND, ButtonControl.ButtonTypes.Submit));

         Content.Add(frmMessage);

         // Enables message sending by adding script
         Scripts.Add(new AjaxSendFormScript(this, frmMessage));
      }

      public override void FormDataReceived(UI.Controls.FormControl receivedForm)
      {
         // Get message data
         PrivateMessage pm = new PrivateMessage();
         pm.Body = Parameters.GetString(ChatControl.FIELD_MESSAGE_DOMID);
         pm.FromUserID = Workspace.CurrentUser.User.ID;
         pm.ToUserID = Parameters.GetInteger(ChatControl.FIELD_TOUSER_DOMID);
         pm.FromIP = Request.UserHostAddress;

         // Send the message
         PrivateMessageDAO pmDao = new PrivateMessageDAO(Workspace);
         pmDao.SendMessage(pm);
      }

      public override void LoadPage()
      {
         ChatMessage message;

         if (chat != null)
         {
            PrivateMessageDAO msgDao = new PrivateMessageDAO(Workspace);
            PrivateMessageThread th = msgDao.GetThread(Workspace.CurrentUser.User.ID, this.RemoteUserID);

            foreach (PrivateMessage msg in th.Messages)
            {
               message = new ChatMessage();
               message.DomID = "pmsg" + msg.ID;
               message.Author = Workspace.SecurityService.GetUser(msg.FromUserID).GetDisplayName();
               message.Time = msg.Sended.ToString(Formatter.FORMAT_DATETIME);
               message.Content = msg.Body;

               chat.Messages.Add(message);
            }
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);

         return url.ToString();
      }

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <param name="userId">User identifier.</param>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL(int userId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Workspace.PARAM_USER_ID, userId);

         return url.ToString();
      }

      #endregion

   }
}
