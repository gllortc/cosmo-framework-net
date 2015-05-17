using System;
using System.Runtime.InteropServices;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Contains all data to display on a fax coverpage.
   /// </summary>
   public class FaxCoverPage
   {
      private string subject;
      private string coverPageName;
      private string recipientNumber;
      private string recipientCity;
      private string recipientCompany;
      private string recipientCountry;
      private string recipientDepartment;
      private string recipientHomePhone;
      private string recipientName;
      private string recipientOfficePhone;
      private string recipientOfficeLocation;
      private string recipientState;
      private string recipientStreetAddress;
      private string recipientTitle;
      private string recipientPostalZipCode;
      private string senderAddress;
      private string senderCompany;
      private string senderDepartment;
      private string senderFaxNumber;
      private string senderHomePhone;
      private string senderName;
      private string senderOfficeLocation;
      private string senderOfficePhone;
      private string senderTitle;

      /// <summary>
      /// Create a instance with empty data.
      /// For filling data, use <see cref="FaxServer.FillDefaultInformations(FaxInformations, FaxCoverPage)"/> method.
      /// </summary>
      public FaxCoverPage() { }

      #region Properties

      /// <summary>
      /// Get or set the cover page name to use.
      /// </summary>
      public string CoverPageName
      {
         get { return this.coverPageName; }
         set { this.coverPageName = value; }
      }

      /// <summary>
      /// Get or set the recipient city to put in the cover page.
      /// </summary>
      public string RecipientCity
      {
         get { return this.recipientCity; }
         set { this.recipientCity = value; }
      }

      /// <summary>
      /// Get or set the recipient company to put in the cover page.
      /// </summary>
      public string RecipientCompany
      {
         get { return this.recipientCompany; }
         set { this.recipientCompany = value; }
      }

      /// <summary>
      /// Get or set the recipient country to put in the cover page.
      /// </summary>
      public string RecipientCountry
      {
         get { return this.recipientCountry; }
         set { this.recipientCountry = value; }
      }

      /// <summary>
      /// Get or set the recipient department to put in the cover page.
      /// </summary>
      public string RecipientDepartment
      {
         get { return this.recipientDepartment; }
         set { this.recipientDepartment = value; }
      }

      /// <summary>
      /// Get or set the recipient home phone to put in the cover page.
      /// </summary>
      public string RecipientHomePhone
      {
         get { return this.recipientHomePhone; }
         set { this.recipientHomePhone = value; }
      }

      /// <summary>
      /// Get or set the recipient name to put in the cover page.
      /// </summary>
      public string RecipientName
      {
         get { return this.recipientName; }
         set { this.recipientName = value; }
      }

      /// <summary>
      /// Get or set the recipient fax number to put in the cover page.
      /// </summary>
      public string RecipientNumber
      {
         get { return this.recipientNumber; }
         set { this.recipientNumber = value; }
      }

      /// <summary>
      /// Get or set the recipient office location to put in the cover page.
      /// </summary>
      public string RecipientOfficeLocation
      {
         get { return this.recipientOfficeLocation; }
         set { this.recipientOfficeLocation = value; }
      }

      /// <summary>
      /// Get or set the recipient office phone to put in the cover page.
      /// </summary>
      public string RecipientOfficePhone
      {
         get { return this.recipientOfficePhone; }
         set { this.recipientOfficePhone = value; }
      }

      /// <summary>
      /// Get or set the recipient state to put in the cover page.
      /// </summary>
      public string RecipientState
      {
         get { return this.recipientState; }
         set { this.recipientState = value; }
      }

      /// <summary>
      /// Get or set the recipient street address to put in the cover page.
      /// </summary>
      public string RecipientStreetAddress
      {
         get { return this.recipientStreetAddress; }
         set { this.recipientStreetAddress = value; }
      }

      /// <summary>
      /// Get or set the recipient title to put in the cover page.
      /// </summary>
      public string RecipientTitle
      {
         get { return this.recipientTitle; }
         set { this.recipientTitle = value; }
      }

      /// <summary>
      /// Get or set the recipient postal ZIP code to put in the cover page.
      /// </summary>
      public string RecipientPostalZipCode
      {
         get { return this.recipientPostalZipCode; }
         set { this.recipientPostalZipCode = value; }
      }

      /// <summary>
      /// Get or set the sender address to put in the cover page.
      /// </summary>
      public string SenderAddress
      {
         get { return this.senderAddress; }
         set { this.senderAddress = value; }
      }

      /// <summary>
      /// Get or set the sender company to put in the cover page.
      /// </summary>
      public string SenderCompany
      {
         get { return this.senderCompany; }
         set { this.senderCompany = value; }
      }

      /// <summary>
      /// Get or set the sender department to put in the cover page.
      /// </summary>
      public string SenderDepartment
      {
         get { return this.senderDepartment; }
         set { this.senderDepartment = value; }
      }

      /// <summary>
      /// Get or set the sender fax number to put in the cover page.
      /// </summary>
      public string SenderFaxNumber
      {
         get { return this.senderFaxNumber; }
         set { this.senderFaxNumber = value; }
      }

      /// <summary>
      /// Get or set the sender home phone to put in the cover page.
      /// </summary>
      public string SenderHomePhone
      {
         get { return this.senderHomePhone; }
         set { this.senderHomePhone = value; }
      }

      /// <summary>
      /// Get or set the sender name to put in the cover page.
      /// </summary>
      public string SenderName
      {
         get { return this.senderName; }
         set { this.senderName = value; }
      }

      /// <summary>
      /// Get or set the sender office location to put in the cover page.
      /// </summary>
      public string SenderOfficeLocation
      {
         get { return this.senderOfficeLocation; }
         set { this.senderOfficeLocation = value; }
      }

      /// <summary>
      /// Get or set the sender office phone to put in the cover page.
      /// </summary>
      public string SenderOfficePhone
      {
         get { return this.senderOfficePhone; }
         set { this.senderOfficePhone = value; }
      }

      /// <summary>
      /// Get or set the sender title phone to put in the cover page.
      /// </summary>
      public string SenderTitle
      {
         get { return this.senderTitle; }
         set { this.senderTitle = value; }
      }

      /// <summary>
      /// Get or set the subject to put in the cover page.
      /// </summary>
      public string Subject
      {
         get { return this.subject; }
         set { this.subject = value; }
      }

      #endregion

      #region Internal Members

      /// <summary>
      /// Fill the members with a <see cref="NativeMethods.FAX_COVERPAGE_INFO"/> structure.
      /// </summary>
      /// <param name="coverPage">The Windows structure contains data.</param>
      internal void Fill(NativeMethods.FAX_COVERPAGE_INFO coverPage)
      {
         this.recipientCity = coverPage.RecCity;
         this.recipientCompany = coverPage.RecCompany;
         this.recipientCountry = coverPage.RecCountry;
         this.recipientDepartment = coverPage.RecDepartment;
         this.recipientNumber = coverPage.RecFaxNumber;
         this.recipientHomePhone = coverPage.RecHomePhone;
         this.recipientName = coverPage.RecName;
         this.recipientOfficeLocation = coverPage.RecOfficeLocation;
         this.recipientOfficePhone = coverPage.RecOfficePhone;
         this.recipientState = coverPage.RecState;
         this.recipientStreetAddress = coverPage.RecStreetAddress;
         this.recipientTitle = coverPage.RecTitle;
         this.recipientPostalZipCode = coverPage.RecZip;

         this.senderAddress = coverPage.SdrAddress;
         this.senderCompany = coverPage.SdrCompany;
         this.senderDepartment = coverPage.SdrDepartment;
         this.senderFaxNumber = coverPage.SdrFaxNumber;
         this.senderHomePhone = coverPage.SdrHomePhone;
         this.senderName = coverPage.SdrName;
         this.senderOfficeLocation = coverPage.SdrOfficeLocation;
         this.senderOfficePhone = coverPage.SdrOfficePhone;
         this.senderTitle = coverPage.SdrTitle;

         this.subject = coverPage.Subject;
      }

      /// <summary>
      /// Create a Windows <see cref="NativeMethods.FAX_COVERPAGE_INFO"/> structure for using with native functions.
      /// </summary>
      /// <returns></returns>
      internal NativeMethods.FAX_COVERPAGE_INFO CreateCoverPageStructure()
      {
         NativeMethods.FAX_COVERPAGE_INFO str;

         str = new NativeMethods.FAX_COVERPAGE_INFO();
         str.SizeOfStruct = Convert.ToUInt32(Marshal.SizeOf(str));

         str.RecCity = this.recipientCity;
         str.RecCompany = this.recipientCompany;
         str.RecCountry = this.recipientCountry;
         str.RecDepartment = this.recipientDepartment;
         str.RecFaxNumber = this.recipientNumber;
         str.RecHomePhone = this.recipientHomePhone;
         str.RecName = this.recipientName;
         str.RecOfficeLocation = this.recipientOfficeLocation;
         str.RecOfficePhone = this.recipientOfficePhone;
         str.RecState = this.recipientState;
         str.RecStreetAddress = this.recipientStreetAddress;
         str.RecTitle = this.recipientTitle;
         str.RecZip = this.recipientPostalZipCode;

         str.SdrAddress = this.senderAddress;
         str.SdrCompany = this.senderCompany;
         str.SdrDepartment = this.senderDepartment;
         str.SdrFaxNumber = this.senderFaxNumber;
         str.SdrHomePhone = this.senderHomePhone;
         str.SdrName = this.senderName;
         str.SdrOfficeLocation = this.senderOfficeLocation;
         str.SdrOfficePhone = this.senderOfficePhone;
         str.SdrTitle = this.senderTitle;

         str.Subject = this.subject;

         return str;
      }

      #endregion

   }

}
