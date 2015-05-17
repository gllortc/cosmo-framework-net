using System;
using System.Collections.Generic;
using Cosmo.Net.Mail;

namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// Implementa un mensaje de correo para el servicio Pop3.
   /// </summary>
   public class Pop3Message : InternetTextMessage
   {
      private Boolean zInvalidFormat = false;
      private String zData;
      private String zBodyText; // Field for decoded body data.
      private Boolean zBodyTextCreated = false;
      private Pop3Content zBodyContent = null;
      private List<Pop3Content> zContents = new List<Pop3Content>();
      private Int64? zIndex = 0;
      private Int32 zSize = 0;

      /// <summary>
      /// Devuelve una instancia de Pop3Message.
      /// </summary>
      public Pop3Message(String inText) : base(inText)
      {
         this.Initialize(inText);
      }

      /// <summary>
      /// Devuelve una instancia de Pop3Message.
      /// </summary>
      public Pop3Message(String inText, Int64 inIndex) : base(inText)
      {
         this.zIndex = inIndex;
         this.Initialize(inText);
      }

      /// <summary>
      /// Get mail index of this mailbox.
      /// </summary>
      public Int64? Index
      {
         get { return this.zIndex; }
      }

      /// <summary>
      /// Get text data used to create this instance.
      /// </summary>
      public String Data
      {
         get { return this.zData; }
      }

      /// <summary>
      /// Get TO value of this mail.
      /// </summary>
      public String To
      {
         get { return this["To"]; }
      }

      /// <summary>
      /// Get CC value of this mail.
      /// </summary>
      public String Cc
      {
         get { return this["Cc"]; }
      }

      /// <summary>
      /// Get BCC value of this mail.
      /// </summary>
      public String Bcc
      {
         get { return this["Bcc"]; }
      }

      /// <summary>
      /// Get body text of this mail.
      /// </summary>
      public String BodyText
      {
         get
         {
            this.EnsureBodyText();
            return this.zBodyText;
         }
         set { this.zBodyText = value; }
      }

      /// <summary>
      /// Get mail size of this mail.
      /// </summary>
      public Int32 Size
      {
         get { return this.zSize; }
         set { this.zSize = value; }
      }

      /// <summary>
      /// Get content of this mail message.
      /// </summary>
      public Pop3Content BodyContent
      {
         get
         {
            this.EnsureBodyContent(this.zContents);
            return this.zBodyContent;
         }
      }

      /// <summary>
      /// Get pop3 content collection of this mail.
      /// </summary>
      public List<Pop3Content> Contents
      {
         get { return this.zContents; }
      }

      /// <summary>
      /// Get a value that specify this mail format is valid or invalid.
      /// </summary>
      public Boolean InvalidFormat
      {
         get { return this.zInvalidFormat; }
      }

      /// <summary>
      /// Get value that indicate body text is created or not.
      /// </summary>
      protected Boolean BodyTextCreated
      {
         get { return this.zBodyTextCreated; }
         set { this.zBodyTextCreated = value; }
      }

      /// <summary>
      /// Inicializa erl objeto.
      /// </summary>
      private void Initialize(String inText)
      {
         this.zData = inText;
         this.zSize = inText.Length;
         if (this.IsMultiPart == true)
         {
            List<String> l = MimeContent.ParseToContentTextList(this.BodyData, this.MultiPartBoundary);
            for (int i = 0; i < l.Count; i++)
            {
               this.zContents.Add(new Pop3Content(this, l[i]));
            }
         }
      }

      /// <summary>
      /// Ensure that body data is set or not,and set body data if body data is not set.
      /// </summary>
      /// <param name="inContents"></param>
      /// <returns></returns>
      private Boolean EnsureBodyContent(List<Pop3Content> inContents)
      {
         for (int i = 0; i < inContents.Count; i++)
         {
            if (inContents[i].IsBody == true)
            {
               this.zBodyContent = inContents[i];
               return true;
            }

            if (this.EnsureBodyContent(inContents[i].Contents) == true)
            { 
               return true; 
            }
         }
         return false;
      }

      /// <summary>
      /// Get all pop3 content collection.
      /// </summary>
      /// <returns></returns>
      public static List<Pop3Content> GetAllContents(Pop3Message inPop3Message)
      {
         if (inPop3Message == null)
         { throw new ArgumentNullException("inPop3Message"); }
         List<Pop3Content> l = new List<Pop3Content>();
         l = Pop3Message.GetAttachedContents(inPop3Message.Contents, delegate(Pop3Content c) { return true; });

         return l;
      }

      /// <summary>
      /// Get pop3 content collection that IsAttachment property is true.
      /// </summary>
      public static List<Pop3Content> GetAttachedContents(Pop3Message inPop3Message)
      {
         if (inPop3Message == null)
         { throw new ArgumentNullException("inPop3Message"); }
         List<Pop3Content> l = new List<Pop3Content>();
         l = Pop3Message.GetAttachedContents(inPop3Message.Contents, delegate(Pop3Content c) { return c.IsAttachment; });

         return l;
      }

      /// <summary>
      /// Get pop3 content collection that specify predicate is true.
      /// </summary>
      public static List<Pop3Content> GetAttachedContents(List<Pop3Content> inContents, Predicate<Pop3Content> inPredicate)
      {
         List<Pop3Content> l = new List<Pop3Content>();
         for (int i = 0; i < inContents.Count; i++)
         {
            if (inPredicate(inContents[i]) == true)
            {
               l.Add(inContents[i]);
            }
            l.AddRange(Pop3Message.GetAttachedContents(inContents[i].Contents, inPredicate).ToArray());
         }
         return l;
      }

      /// <summary>
      /// Ensure that body text is set or not,and set body text if body text is not set.
      /// </summary>
      protected virtual void EnsureBodyText()
      {
         if (this.BodyTextCreated == false)
         {
            if (this.ContentType.Value.IndexOf("message/rfc822") > -1)
            {
               this.BodyText = this.BodyData;
            }
            else if (this.IsMultiPart == true)
            {
               if (this.BodyContent == null)
               {
                  this.BodyText = string.Empty;
               }
               else
               {
                  this.BodyText = this.BodyContent.BodyText;
               }
            }
            else if (this.IsBody == true)
            {
               this.BodyText = MailParser.DecodeFromMailBody(this.BodyData, this.ContentTransferEncoding, this.ContentEncoding);
            }
            else
            {
               this.BodyText = this.BodyData;
            }
         }
         this.BodyTextCreated = true;
      }

      /// <summary>
      /// Create SmtpMessage instance with this instance value.
      /// </summary>
      /// <returns></returns>
      public Smtp.SmtpMessage CreateSmtpMessage()
      {
         Smtp.SmtpMessage mg = new Cosmo.Net.Smtp.SmtpMessage();
         Field f = null;

         mg.To.AddRange(MailAddress.GetMailAddressList(this.To));
         mg.Cc.AddRange(MailAddress.GetMailAddressList(this.Cc));
         for (int i = 0; i < this.Header.Count; i++)
         {
            f = this.Header[i];
            if (String.IsNullOrEmpty(f.Value) == true)
            { continue; }
            if (f.Key.ToLower() == "to" ||
                f.Key.ToLower() == "cc")
            { continue; }
            mg[f.Key] = MailParser.DecodeFromMailHeaderLine(f.Value);
         }
         for (int i = 0; i < this.ContentType.Fields.Count; i++)
         {
            f = this.ContentType.Fields[i];
            mg.ContentType.Fields.Add(new Field(f.Key, MailParser.DecodeFromMailHeaderLine(f.Value)));
         }
         for (int i = 0; i < this.ContentDisposition.Fields.Count; i++)
         {
            f = this.ContentDisposition.Fields[i];
            mg.ContentDisposition.Fields.Add(new Field(f.Key, MailParser.DecodeFromMailHeaderLine(f.Value)));
         }
         mg.BodyText = this.BodyText;
         for (int i = 0; i < this.Contents.Count; i++)
         {
            mg.Contents.Add(this.Contents[i].CreateSmtpContent());
         }
         return mg;
      }
   }
}
