using Cosmo.Communications.PrivateMessages;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
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

      // Modal element unique identifier
      private const string DOM_ID = "ads-th-list";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="PrivateMessagesPartialView"/>.
      /// </summary>
      public PrivateMessagesPartialView()
         : base()
      {
         this.DomID = PrivateMessagesPartialView.DOM_ID;
         this.RemoteUserID = 0;
      }

      /// <summary>
      /// Gets an instance of <see cref="PrivateMessagesPartialView"/>.
      /// </summary>
      /// <param name="remoteUserID">Remote user ID</param>
      public PrivateMessagesPartialView(int remoteUserID)
         : base()
      {
         this.DomID = PrivateMessagesPartialView.DOM_ID;
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
         PrivateMessageDAO msgDao = new PrivateMessageDAO(Workspace);

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         if (this.RemoteUserID > 0)
         {
            ChatMessage message;
            ChatControl chat = new ChatControl(this);

            User remoteUser = Workspace.SecurityService.GetUser(this.RemoteUserID);
            chat.Caption = "Mensajes con " + remoteUser.GetDisplayName();

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

            Content.Add(chat);
         }
         else
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Type = ComponentColorScheme.Information;
            callout.Title = "Seleccione una conversación";
            callout.Text = "Seleccione la conversación (en la parte derecha) para ver los mensajes con una determinada persona.";

            Content.Add(callout);
         }
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
