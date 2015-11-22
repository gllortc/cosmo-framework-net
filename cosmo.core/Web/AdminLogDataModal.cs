using Cosmo.Diagnostics;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System;
using System.Collections.Generic;

namespace Cosmo.Web
{

   /// <summary>
   /// Implements a view to show log entry data.
   /// </summary>
   [AuthorizationRequired(Cosmo.Workspace.ROLE_ADMINISTRATOR)]
   [ViewParameter(ParameterName = Workspace.PARAM_OBJECT_ID,
                  PropertyName = "LogEntryID")]
   public class AdminLogDataModal : Cosmo.UI.ModalView
   {
      // Modal element unique identifier
      private const string DOM_ID = "log-entry-modal";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="AdminLogDataModal"/>.
      /// </summary>
      public AdminLogDataModal()
         : base(AdminLogDataModal.DOM_ID)
      {

      }

      /// <summary>
      /// Gets an instance of <see cref="AdminLogDataModal"/>.
      /// </summary>
      /// <param name="entryId">User identifier.</param>
      public AdminLogDataModal(int entryId)
         : base(AdminLogDataModal.DOM_ID)
      {
         this.LogEntryID = entryId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the log entry unique identifier.
      /// </summary>
      public int LogEntryID { get; set; }

      #endregion

      #region ModalViewContainer Implementation

      public override void InitPage()
      {
         LogEntry entry = null;
         this.LogEntryID = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);

         Closeable = true;

         if (this.LogEntryID > 0)
         {
            entry = Workspace.Logger.GetByID(this.LogEntryID);
         }

         if (entry == null)
         {
            Icon = IconControl.ICON_PROHIBITED_CIRCLE;
            Title = "Log entry not found";

            CalloutControl alert = new CalloutControl(this);
            alert.Type = ComponentColorScheme.Error;
            alert.Icon = Icon;
            alert.Title = Title;
            alert.Text = "Requested entry not found (#" + this.LogEntryID + ").";
            Content.Add(alert);

            return;
         }

         switch (entry.Type)
         {
            case LogEntry.LogEntryType.EV_INFORMATION:
               Icon = IconControl.ICON_CHECK;
               Title = "Information entry";
               break;
            case LogEntry.LogEntryType.EV_WARNING:
               Icon = IconControl.ICON_WARNING;
               Title = "Warning entry";
               break;
            case LogEntry.LogEntryType.EV_ERROR:
               Icon = IconControl.ICON_PROHIBITED_CIRCLE;
               Title = "Error entry";
               break;
            case LogEntry.LogEntryType.EV_SECURITY:
               Icon = IconControl.ICON_LOCK;
               Title = "Security entry";
               break;
         }

         List<KeyValue> values = new List<KeyValue>();
         values.Add(new KeyValue("Application", entry.ApplicationName));
         values.Add(new KeyValue("Context", entry.Context));
         values.Add(new KeyValue("Details", entry.Message.Replace(Environment.NewLine, "<br />")));
         values.Add(new KeyValue("Date", entry.Date.ToString(Calendar.FORMAT_DATETIME)));
         values.Add(new KeyValue("User", entry.UserLogin));

         HtmlContentControl data = new HtmlContentControl(this);
         data.AppendDataList(values);

         Content.Add(data);
      }

      #endregion

   }
}
