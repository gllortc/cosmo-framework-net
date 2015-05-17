using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cosmo.Net.Mail;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa un mensaje de correo electrico para el servicio Smtp.
   /// </summary>
   public class SmtpMessage : InternetTextMessage
   {
      private List<SmtpContent> pContents;
      private List<String> pEncodeHeaderKeys = new List<String>();
      private List<MailAddress> pTo = new List<MailAddress>();
      private List<MailAddress> pCc = new List<MailAddress>();
      private List<MailAddress> pBcc = new List<MailAddress>();
      private String pBodyText = "";
      private Encoding pHeaderEncoding = Encoding.ASCII;
      private TransferEncoding pHeaderTransferEncoding = TransferEncoding.SevenBit;
      private Encoding pContentEncoding = Encoding.ASCII;

      /// <summary>
      /// Devuelve una instancia de SmtpMessage.
      /// </summary>
      public SmtpMessage()
      {
         this.Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de SmtpMessage.
      /// </summary>
      public SmtpMessage(String inMailFrom, String inTo, String inCc, String inSubject, String inBodyText)
      {
         this.Initialize();
         this.From = inMailFrom;
         this.To.Add(new MailAddress(inTo));
         this.Cc.Add(new MailAddress(inCc));
         this.Subject = inSubject;
         this.BodyText = inBodyText;
      }

      /// <summary>
      /// Contiene la lista de direcciones de correo de destino.
      /// </summary>
      public List<MailAddress> To
      {
         get { return this.pTo; }
      }

      /// <summary>
      /// Contiene la lista de direcciones de correo para recepci de copia.
      /// </summary>
      public List<MailAddress> Cc
      {
         get { return this.pCc; }
      }

      /// <summary>
      /// Contiene la lista de direcciones de correo para recepci de copia oculta.
      /// </summary>
      public List<MailAddress> Bcc
      {
         get { return this.pBcc; }
      }

      /// <summary>
      /// Contiene el tipo de codificaci del encabezado del mensaje.
      /// </summary>
      public Encoding HeaderEncoding
      {
         get { return this.pHeaderEncoding; }
         set { this.pHeaderEncoding = value; }
      }

      /// <summary>
      /// Contiene el tipo de codificaci usada para la transmisi del encabezado del mensaje.
      /// </summary>
      public TransferEncoding HeaderTransferEncoding
      {
         get { return this.pHeaderTransferEncoding; }
         set { this.pHeaderTransferEncoding = value; }
      }

      /// <summary>
      /// Texto del cuerpo del mensaje.
      /// </summary>
      public String BodyText
      {
         get { return this.pBodyText; }
         set { this.pBodyText = value; }
      }

      /// <summary>
      /// Contiene las distintas partes que conforman el contenido del mensaje.
      /// </summary>
      public List<SmtpContent> Contents
      {
         get { return this.pContents; }
      }

      /// <summary>
      /// 終nicializa el mensaje de correo.
      /// </summary>
      private void Initialize()
      {
         this.pContents = new List<SmtpContent>();
         if (CultureInfo.CurrentCulture.Name.StartsWith("ja") == true)
         {
            this.HeaderEncoding = Encoding.GetEncoding("iso-2022-jp");
            this.HeaderTransferEncoding = TransferEncoding.Base64;
            this.ContentEncoding = Encoding.GetEncoding("iso-2022-jp");
            this.ContentTransferEncoding = TransferEncoding.Base64;
         }
         this.pEncodeHeaderKeys.Add("subject");
      }

      /// <summary>
      /// 実際に送信される文字列のデータを取得します。
      /// </summary>
      public String GetDataText()
      {
         StringBuilder sb = new StringBuilder();
         CultureInfo ci = CultureInfo.CurrentCulture;
         Field f = null;
         SmtpContent ct = null;
         String s = "";

         if (this.IsMultiPart == false &&
             this.Contents.Count > 0)
         {
            this.ContentType.Value = "multipart/mixed";
         }

         // ContentTransferEncoding
         f = InternetTextMessage.Field.FindField(this.Header, "Content-Transfer-Encoding");
         if (f == null)
         {
            f = new Field("Content-Transfer-Encoding", MailParser.ToTransferEncoding(this.ContentTransferEncoding));
            this.Header.Add(f);
         }
         else
         {
            f.Value = MailParser.ToTransferEncoding(this.ContentTransferEncoding);
         }

         // TO
         f = Field.FindField(this.Header, "To");
         if (f == null)
         {
            s = this.CreateMailAddressListText(this.pTo);
            if (String.IsNullOrEmpty(s) == false)
            {
               sb.Append("To: ");
               sb.Append(s);
            }
         }
         // CC
         f = Field.FindField(this.Header, "Cc");
         if (f == null)
         {
            s = this.CreateMailAddressListText(this.pCc);
            if (String.IsNullOrEmpty(s) == false)
            {
               sb.Append("Cc: ");
               sb.Append(s);
            }
         }

         for (int i = 0; i < this.Header.Count; i++)
         {
            f = this.Header[i];
            if (this.pEncodeHeaderKeys.Contains(f.Key.ToLower()) == true)
            {
               sb.AppendFormat("{0}: {1}{2}", f.Key
                   , MailParser.EncodeToMailHeaderLine(f.Value, this.HeaderTransferEncoding, this.HeaderEncoding
                   , MailParser.MaxCharCountPerRow - f.Key.Length - 2), MailParser.NewLine);
            }
            else if (f.Key.ToLower() != "content-type")
            {
               sb.AppendFormat("{0}: {1}{2}", f.Key, f.Value, MailParser.NewLine);
            }
         }

         if (this.IsMultiPart == true)
         {
            if (String.IsNullOrEmpty(this.MultiPartBoundary) == true)
            {
               this.MultiPartBoundary = MailParser.GenerateBoundary();
            }
            // Add BodyText Content
            if (String.IsNullOrEmpty(this.BodyText) == false)
            {
               ct = new SmtpContent();
               ct.BodyText = this.BodyText;
               ct.ContentEncoding = this.ContentEncoding;
               ct.ContentTransferEncoding = this.ContentTransferEncoding;
               if (this.Contents.Exists(c => c.IsBody) == false)
               {
                  this.Contents.Insert(0, ct);
               }
            }

            // Multipartboundary
            sb.AppendFormat("Content-Type: {0}; boundary=\"{1}\"", this.ContentType.Value, this.MultiPartBoundary);
            sb.Append(MailParser.NewLine);
            sb.Append(MailParser.NewLine);

            // This is multi-part message in MIME format.
            sb.Append(MailParser.ThisIsMultiPartMessageInMIMEFormat);
            sb.Append(MailParser.NewLine);
            for (int i = 0; i < this.pContents.Count; i++)
            {
               sb.Append("--");
               sb.Append(this.MultiPartBoundary);
               sb.Append(MailParser.NewLine);
               sb.Append(this.Contents[i].GetDataText());
               sb.Append(MailParser.NewLine);
            }
            sb.Append(MailParser.NewLine);
            sb.AppendFormat("--{0}--", this.MultiPartBoundary);
         }
         else
         {
            sb.AppendFormat("Content-Type: {0}; charset=\"{1}\"", this.ContentType.Value, this.ContentEncoding.WebName);
            sb.Append(MailParser.NewLine);
            sb.Append(MailParser.NewLine);
            s = MailParser.EncodeToMailBody(this.BodyText, this.ContentTransferEncoding, this.ContentEncoding);
            for (int i = 0; i < s.Length; i++)
            {
               if (i > 0 &&
                   i % 76 == 0)
               {
                  sb.Append(MailParser.NewLine);
               }
               sb.Append(s[i]);
            }
         }

         sb.Append(MailParser.NewLine);
         sb.Append(MailParser.NewLine);
         sb.Append(".");
         sb.Append(MailParser.NewLine);

         return sb.ToString();
      }

      /// <summary>
      /// ユーザー名とメールアドレスをFromにセットします。
      /// </summary>
      public void SetFromMailAddress(String inUserName, String inMailAddress)
      {
         this.From = SmtpMessage.CreateFromMailAddress(inUserName, inMailAddress); ;
      }

      /// <summary>
      /// ユーザー名とメールアドレスを示す文字列を生成します。
      /// </summary>
      public static String CreateFromMailAddress(String inUserName, String inMailAddress)
      {
         return String.Format("\"{0}\" <{1}>", inUserName, inMailAddress);
      }

      /// <summary>
      /// メールアドレスの一覧データからメールアドレスの文字列を生成します。
      /// </summary>
      private String CreateMailAddressListText(List<MailAddress> inMailAddressList)
      {
         StringBuilder sb = new StringBuilder();
         List<MailAddress> l = inMailAddressList;
         String s = "";

         for (int i = 0; i < l.Count; i++)
         {
            sb.AppendFormat("{0}{1}", s, l[i].ToEncodeString().Trim());
            sb.Append(MailParser.NewLine);

            s = "\t, ";
         }
         return sb.ToString();
      }
   }

}
