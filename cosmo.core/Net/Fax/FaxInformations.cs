using System;
using System.Runtime.InteropServices;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Contains all data informations for sending a fax (recepient, sender,...etc) for sending a fax.
   /// </summary>
   public class FaxInformations
   {

      #region Enumerations

      /// <summary>
      /// Represent the different schedules options for sending a fax document.
      /// </summary>
      public enum FaxScheduleAction
      {
         /// <summary>Send fax now.</summary>
         Now = 0,
         /// <summary>Send fax at a specific time.</summary>
         SpecificTime = 1,
         /// <summary>Send fax during a discount period.</summary>
         DiscountPeriod = 2
      }

      #endregion

      private string senderName;
      private string senderCompany;
      private string senderDepartment;
      private string recipientName;
      private string documentName;
      private string billingCode;
      private FaxCoverPage coverPage;
      private string recipientNumber;

      /// <summary>
      /// Create an instance of FaxInformations with empty values.
      /// </summary>
      public FaxInformations() { }

      #region Properties

      /// <summary>
      /// Get or set the recipient name.
      /// </summary>
      public string RecipientName
      {
         get { return this.recipientName; }
         set { this.recipientName = value; }
      }

      /// <summary>
      /// Get or set the recipient number
      /// </summary>
      public string RecipientNumber
      {
         get { return this.recipientNumber; }
         set { this.recipientNumber = value; }
      }

      /// <summary>
      /// Get or set the document name
      /// </summary>
      public string DocumentName
      {
         get { return this.documentName; }
         set { this.documentName = value; }
      }

      /// <summary>
      /// Get or set the sender name
      /// </summary>
      public string SenderName
      {
         get { return this.senderName; }
         set { this.senderName = value; }
      }

      /// <summary>
      /// Get or set the sender department
      /// </summary>
      public string SenderCompany
      {
         get { return this.senderCompany; }
         set { this.senderCompany = value; }
      }

      /// <summary>
      /// Get or set the sender department
      /// </summary>
      public string SenderDepartment
      {
         get { return this.senderDepartment; }
         set { this.senderDepartment = value; }
      }

      /// <summary>
      /// Get or set the billing code
      /// </summary>
      public string BillingCode
      {
         get { return this.billingCode; }
         set { this.billingCode = value; }
      }

      /// <summary>
      /// Get or set the cover page.
      /// </summary>
      public FaxCoverPage CoverPage
      {
         get { return this.coverPage; }
         set { this.coverPage = value; }
      }

      /*/// <summary>
      /// Get or set the schedule time.
      /// </summary>
      /// <exception cref="System.ArgumentException">If value is not null and FaxScheduleAction is not SpecificTime</exception>
      /// <exception cref="System.ArgumentNullException">If value is null and FaxScheduleAction is SpecificTime</exception>
      public DateTime? ScheduleTime
      {
          get { return this.scheduleTime; }
          set
          {
              if (this.scheduleAction != FaxScheduleAction.SpecificTime)
              {
                  if (value != null)
                      throw new ArgumentException(FaxRessources.SpecificationHeureIncorrect, "value");
              }
              else
              {
                  if (value == null)
                      throw new ArgumentNullException("value");
              }

              this.scheduleTime = value;
          }
      }*/

      #endregion

      #region Methods

      /*/// <summary>
      /// Get or set the schedule action.
      /// </summary>
      /// <remarks>If ScheduleAction value is not <see cref="FaxScheduleAction.SpecificTime"/>, the
      /// ScheduteTime will be set to null.</remarks>
      public FaxScheduleAction ScheduleAction
      {
          get { return this.scheduleAction; }
          set
          {
              if (this.scheduleAction != FaxScheduleAction.SpecificTime)
                  this.ScheduleTime = null;

              this.scheduleAction = value;
          }
      }*/

      #endregion

      #region Internal Members

      internal void Fill(NativeMethods.FAX_JOB_PARAM jobParameters)
      {
         this.recipientName = jobParameters.RecipientName;
         this.recipientNumber = jobParameters.RecipientNumber;

         this.senderName = jobParameters.SenderName;
         //informations.ScheduleAction = (FaxScheduleAction)Enum.ToObject(typeof(FaxScheduleAction), jobParameters.ScheduleAction);
         //informations.ScheduleTime = SystemTimeToDateTime(jobParameters.ScheduleTime);
         this.senderCompany = jobParameters.SenderCompany;
         this.senderDepartment = jobParameters.SenderDept;

         this.documentName = jobParameters.DocumentName;

         this.billingCode = jobParameters.BillingCode;
      }

      internal NativeMethods.FAX_JOB_PARAM CreateFaxJobParamStructure()
      {
         NativeMethods.FAX_JOB_PARAM jobParam;

         jobParam = new NativeMethods.FAX_JOB_PARAM();
         jobParam.SizeOfStruct = Convert.ToUInt32(Marshal.SizeOf(jobParam));

         this.FillFaxJobParamStructure(jobParam);

         return jobParam;
      }

      internal void FillFaxJobParamStructure(NativeMethods.FAX_JOB_PARAM jobParam)
      {
         jobParam.SenderCompany = this.senderCompany;
         jobParam.SenderDept = this.senderDepartment;
         jobParam.SenderName = this.senderName;

         jobParam.RecipientName = this.recipientName;
         jobParam.RecipientNumber = this.recipientNumber;

         jobParam.DocumentName = this.documentName;

         jobParam.BillingCode = this.billingCode;
      }

      internal NativeMethods.FAX_PRINT_INFO CreateFaxPrintInfoStructure()
      {
         NativeMethods.FAX_PRINT_INFO printInformations;

         printInformations = new NativeMethods.FAX_PRINT_INFO();

         printInformations.SenderCompany = this.senderCompany;
         printInformations.SenderDept = this.senderDepartment;
         printInformations.SenderName = this.senderName;

         printInformations.RecipientName = this.recipientName;
         printInformations.RecipientNumber = this.recipientNumber;

         printInformations.DocName = this.documentName;

         printInformations.SenderBillingCode = this.billingCode;

         return printInformations;
      }

      #endregion

   }

}
