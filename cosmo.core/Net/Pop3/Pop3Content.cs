using System;
using System.Collections.Generic;
using Cosmo.Net.Mail;

namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// Impelmenta un fragmento de contenido de un mensaje de correo Pop3.
   /// </summary>
   public class Pop3Content : MimeContent
   {
      private Pop3Message zMessage;
      private Pop3Content zParentContent = null;
      private String zData;
      private String zBodyText; // Field for decoded body data.
      private Boolean zBodyTextCreated = false;
      private List<Pop3Content> zContents = new List<Pop3Content>();

      /// <summary>
      /// Devuelve una instancia de Pop3Content.
      /// </summary>
      public Pop3Content(Pop3Message inMessage, String inText) : base(inText)
      {
         this.Initialize(inMessage, inText);
      }

      /// <summary>
      /// Get or set parent content object.
      /// </summary>
      public Pop3Content ParentContent
      {
         get { return this.zParentContent; }
         private set { this.zParentContent = value; }
      }

      /// <summary>
      /// Get name value.
      /// </summary>
      public String Name
      {
         get { return this.ContentType.Name; }
      }

      /// <summary>
      /// Get filename value.
      /// </summary>
      public String FileName
      {
         get { return this.ContentDisposition.FileName; }
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
      /// Get text data used to create this instance.
      /// </summary>
      public String Data
      {
         get { return this.zData; }
      }

      /// <summary>
      /// Get pop3 content collection of this mail.
      /// </summary>
      public new List<Pop3Content> Contents
      {
         get { return this.zContents; }
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
      /// 終nicializa el objeto.
      /// </summary>
      private void Initialize(Pop3Message inMessage, String inText)
      {
         Pop3Content ct = null;

         this.zMessage = inMessage;
         this.zContents = new List<Pop3Content>();
         this.zData = inText;
         this.zBodyText = string.Empty;
         if (this.IsMultiPart == true)
         {
            List<String> l = MimeContent.ParseToContentTextList(this.BodyData, this.MultiPartBoundary);
            for (int i = 0; i < l.Count; i++)
            {
               ct = new Pop3Content(this.zMessage, l[i]);
               ct.ParentContent = this;
               this.zContents.Add(ct);
            }
         }
      }

      /// <summary>
      /// Body部のテキストがセットされているか確認し、セットされてない場合はBody部の文字列をセットします。
      /// </summary>
      /// <returns></returns>
      protected virtual void EnsureBodyText()
      {
         if (this.BodyTextCreated == false)
         {
            if (this.ContentType.Value.IndexOf("message/rfc822") > -1)
            {
               this.BodyText = this.BodyData;
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
      /// Create SmtpContent instance with this instance value.
      /// </summary>
      /// <returns></returns>
      public Smtp.SmtpContent CreateSmtpContent()
      {
         Smtp.SmtpContent ct = new Cosmo.Net.Smtp.SmtpContent();
         Field f = null;

         for (int i = 0; i < this.Header.Count; i++)
         {
            f = this.Header[i];
            if (String.IsNullOrEmpty(f.Value) == true)
            { continue; }
            ct[f.Key] = MailParser.DecodeFromMailHeaderLine(f.Value);
         }
         for (int i = 0; i < this.ContentType.Fields.Count; i++)
         {
            f = this.ContentType.Fields[i];
            ct.ContentType.Fields.Add(new Field(f.Key, MailParser.DecodeFromMailHeaderLine(f.Value)));
         }
         for (int i = 0; i < this.ContentDisposition.Fields.Count; i++)
         {
            f = this.ContentDisposition.Fields[i];
            ct.ContentDisposition.Fields.Add(new Field(f.Key, MailParser.DecodeFromMailHeaderLine(f.Value)));
         }
         ct.BodyText = this.BodyText;
         for (int i = 0; i < this.Contents.Count; i++)
         {
            ct.Contents.Add(this.Contents[i].CreateSmtpContent());
         }
         return ct;
      }
   }

}
