using System;
using System.Drawing;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Represent a fax job which is on a fax server.
   /// </summary>
   /// <remarks>Fax job are deleted after an fax was transmitted succesfully.</remarks>
   public class FaxJob
   {

      #region Enumerations

      /// <summary>
      /// Represents a fax type job.
      /// </summary>
      public enum FaxJobType
      {
         /// <summary>The job type is unknown. This value indicates that the fax server has not yet scheduled the job.</summary>
         Unknown = 0,
         /// <summary>The job is an outgoing fax transmission.</summary>
         Send = 1,
         /// <summary>The job is an incoming fax transmission.</summary>
         Receive = 2,
         /// <summary>The fax server tried to route the fax transmission, but routing failed. The fax server will attempt to route the job again.</summary>
         Routing = 3,
         /// <summary>The fax server did not route the fax because it did not receive the entire transmission. The fax server saves the partial transmission in a temporary directory.</summary>
         FailReceive = 4
      }

      /// <summary>
      /// Represents the different status of a device fax.
      /// </summary>
      public enum FaxStatus
      {
         /// <summary>There is no status for device or fax job.</summary>
         None = 0,
         /// <summary>The device is dialing a fax number.</summary>
         Dialing = 0x20000001,
         /// <summary>The device is sending a fax document.</summary>
         Sending = 0x20000002,
         /// <summary>The device is receiving a fax document.</summary>
         Receiving = 0x20000004,
         /// <summary>The device completed sending or receiving a fax transmission.</summary>
         Completed = 0x20000008,
         /// <summary>The fax service processed the outbound fax document; the fax service provider will transmit the document.</summary>
         Handled = 0x20000010,
         /// <summary>The device is not available because it is in use by another application.</summary>
         Unvailable = 0x20000020,
         /// <summary>The device encountered a busy signal.</summary>
         Busy = 0x20000040,
         /// <summary>The receiving device did not answer the call.</summary>
         NoAnswer = 0x20000080,
         /// <summary>The device dialed an invalid fax number.</summary>
         BadAddress = 0x20000100,
         /// <summary>The sending device cannot complete the call because it does not detect a dial tone.</summary>
         NoDialTone = 0x20000200,
         /// <summary>The fax call was disconnected by the sender or the caller.</summary>
         Disconnected = 0x20000400,
         /// <summary>The device has encountered a fatal protocol error.</summary>
         FatalError = 0x20000800,
         /// <summary>The device received a call that was a data call or a voice call.</summary>
         NotFaxCall = 0x20001000,
         /// <summary>
         /// The device delayed a fax call because the sending device received a busy signal multiple times.
         /// The device cannot retry the call because dialing restrictions exist.
         /// (Some countries/regions restrict the number of retry attempts when a number is busy.
         /// </summary>
         CallDelayed = 0x20002000,
         /// <summary>The device is initializing a call.</summary>
         CallBlackListed = 0x20004000,
         /// <summary>The device is initializing a call.</summary>
         Initializing = 0x20008000,
         /// <summary>The device is offline and unavailable.</summary>
         OffLine = 0x20010000,
         /// <summary>The device is ringing.</summary>
         Ringing = 0x20020000,
         /// <summary>The device is available.</summary>
         Available = 0x20100000,
         /// <summary>The device is aborting a fax job.</summary>
         Abording = 0x20200000,
         /// <summary>The device is routing a received fax document.</summary>
         Routing = 0x20400000,
         /// <summary>The device answered a new call.</summary>
         Answered = 0x20800000,
      }

      /// <summary>
      /// Represents an the differents queue status of a <see cref="FaxJob"/>.
      /// </summary>
      [Flags]
      public enum FaxQueueStatus
      {
         /// <summary>The fax job is in the queue and pending service.</summary>
         Pending = 0x00000000,
         /// <summary>The fax job is in progress.</summary>
         InProgress = 0x00000001,
         /// <summary>The fax server is deleting the fax job.</summary>
         Deleting = 0x00000002,
         /// <summary>The fax job failed.</summary>
         Failed = 0x00000004,
         /// <summary>The fax server paused the fax job.</summary>
         Paused = 0x00000008,
         /// <summary>There is no line available to send the fax. The fax server will send the transmission when a line is available.</summary>
         NoLine = 0x00000010,
         /// <summary>The fax job failed. The fax server will attempt to retransmit the fax after a specified interval.</summary>
         Retrying = 0x00000020,
         /// <summary>The fax server exceeded the maximum number of retransmission attempts allowed. The fax will not be sent.</summary>
         RetriesExceed = 0x00000040
      }

      #endregion

      private string recipientName;
      private string recipientNumber;
      private string senderCompany;
      private string senderDepartment;
      private string senderName;
      private string billingCode;
      private FaxJobType type;
      private FaxStatus status;
      private FaxQueueStatus queueStatus;
      private string userName;
      private int size;
      private int pageCount;
      private uint id;
      private FaxServer server;

      /// <summary>
      /// Create a fax job.
      /// </summary>
      /// <param name="server">Fax server attaching with the new job.</param>
      /// <param name="entry">Data that will fill members of FaxJob.</param>
      internal FaxJob(FaxServer server, NativeMethods.FAX_JOB_ENTRY entry)
      {
         this.server = server;

         this.recipientName = entry.RecipientName;
         this.recipientNumber = entry.RecipientNumber;

         this.senderCompany = entry.SenderCompany;
         this.senderDepartment = entry.SenderDept;
         this.senderName = entry.SenderName;

         this.userName = entry.UserName;
         this.billingCode = entry.BillingCode;

         this.size = Convert.ToInt32(entry.Size);
         this.pageCount = Convert.ToInt32(entry.PageCount);

         this.id = entry.JobId;
         this.type = (FaxJobType)Enum.ToObject(typeof(FaxJobType), entry.JobType);

         this.Initialize(entry);
      }

      #region Properties

      /// <summary>
      /// Get the recipient number.
      /// </summary>
      public string RecipientNumber
      {
         get { return this.recipientNumber; }
      }

      /// <summary>
      /// Get the recipient name.
      /// </summary>
      public string RecipientName
      {
         get { return this.recipientName; }
      }

      /// <summary>
      /// Get the sender company.
      /// </summary>
      public string SenderCompany
      {
         get { return this.senderCompany; }
      }

      /// <summary>
      /// Get the sender department.
      /// </summary>
      public string SenderDepartment
      {
         get { return this.senderDepartment; }
      }

      /// <summary>
      /// Get the sender name.
      /// </summary>
      public string SenderName
      {
         get { return this.senderName; }
      }

      /// <summary>
      /// Get the fax job type.
      /// </summary>
      public FaxJobType Type
      {
         get { return this.type; }
      }

      /// <summary>
      /// Get the size of fax job.
      /// </summary>
      public int Size
      {
         get { return this.size; }
      }

      /// <summary>
      /// Get the page count of fax job.
      /// </summary>
      public int PageCount
      {
         get { return this.pageCount; }
      }

      /// <summary>
      /// Get the current status of fax job.
      /// </summary>
      public FaxStatus Status
      {
         get { return this.status; }
      }

      /// <summary>
      /// Get the billing code of fax job.
      /// </summary>
      public string BillingCode
      {
         get { return this.billingCode; }
      }

      /// <summary>
      /// Get the name of user who submitted the fax job
      /// </summary>
      public string UserName
      {
         get { return this.userName; }
      }

      /*public Image Image
      {
          get
          {
              if (this.image == null)
                  this.image = this.server.GetImage(this.ID);

              return this.image;
          }
      }*/

      /// <summary>
      /// Get the server where the fax job is.
      /// </summary>
      public FaxServer Server
      {
         get
         {
            if (this.server.CheckIsDisposed() == true)
               throw new FaxException(FaxResources.ExceptionFaxServerDisposed);

            return this.server;
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initialise members variables which can be change.
      /// </summary>
      /// <param name="entry"></param>
      /// <remarks>This method was created for update members which can change (For example the Status of a fax job).
      /// This method is called by the constructor and the Refresh method.</remarks>
      private void Initialize(NativeMethods.FAX_JOB_ENTRY entry)
      {
         this.status = (FaxStatus)Enum.ToObject(typeof(FaxStatus), entry.Status);
         this.queueStatus = (FaxQueueStatus)Enum.ToObject(typeof(FaxQueueStatus), entry.QueueStatus);
      }

      /// <summary>
      /// Get the id job.
      /// </summary>
      /// <remarks>This ID must be use only on internal assembly.</remarks>
      internal uint ID
      {
         get { return this.id; }
      }

      /// <summary>
      /// Refresh some members of fax job.
      /// </summary>
      /// <param name="entry">Windows API structure of fax job entry.</param>
      internal void Refresh(NativeMethods.FAX_JOB_ENTRY entry)
      {
         this.Initialize(entry);
      }

      #endregion

   }

}
