using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Cosmo.Net.Mail;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa un fragmento de contenido Mime del mensaje de correo para el servicio Smtp.
   /// </summary>
   public class SmtpContent : MimeContent
   {
      private List<SmtpContent> zContents;
      private static Dictionary<String, String> FileExtensionContentType = new Dictionary<String, String>();
      private String zBodyText = "";

      /// <summary>
      /// Devuelve una instancia de SmtpContent.
      /// </summary>
      static SmtpContent()
      {
         SmtpContent.InitializeFileExtenstionContentType();
      }

      /// <summary>
      /// Devuelve una instancia de SmtpContent.
      /// </summary>
      public SmtpContent() : base()
      {
         this.zContents = new List<SmtpContent>();
      }

      /// <summary>
      /// Nameの値を取得または設定します。
      /// </summary>
      public String Name
      {
         get { return this.ContentType.Name; }
         set { this.ContentType.Name = value; }
      }

      /// <summary>
      /// Nombre del archivo.
      /// </summary>
      public String FileName
      {
         get { return this.ContentDisposition.FileName; }
         set { this.ContentDisposition.FileName = value; }
      }

      /// <summary>
      /// Texto del cuerpo.
      /// </summary>
      public String BodyText
      {
         get { return this.zBodyText; }
         set { this.zBodyText = value; }
      }

      /// <summary>
      /// Una lista de las partes del mensaje que conforman la actual.
      /// </summary>
      public new List<SmtpContent> Contents
      {
         get { return this.zContents; }
      }

      /// <summary>
      /// Inicializa el objeto.
      /// </summary>
      private static void InitializeFileExtenstionContentType()
      {
         SmtpContent.FileExtensionContentType.Add("txt", "text/plain");
         SmtpContent.FileExtensionContentType.Add("css", "text/css");
         SmtpContent.FileExtensionContentType.Add("htm", "text/html");
         SmtpContent.FileExtensionContentType.Add("html", "text/html");
         SmtpContent.FileExtensionContentType.Add("jpg", "Image/jpeg");
         SmtpContent.FileExtensionContentType.Add("gif", "Image/gif");
         SmtpContent.FileExtensionContentType.Add("bmp", "image/x-ms-bmp");
         SmtpContent.FileExtensionContentType.Add("png", "Image/png");
         SmtpContent.FileExtensionContentType.Add("wav", "Audio/wav");
         SmtpContent.FileExtensionContentType.Add("doc", "application/msword");
         SmtpContent.FileExtensionContentType.Add("mdb", "application/msaccess");
         SmtpContent.FileExtensionContentType.Add("xls", "application/vnd.ms-excel");
         SmtpContent.FileExtensionContentType.Add("ppt", "application/vnd.ms-powerpoint");
         SmtpContent.FileExtensionContentType.Add("mpeg", "video/mpeg");
         SmtpContent.FileExtensionContentType.Add("mpg", "video/mpeg");
         SmtpContent.FileExtensionContentType.Add("avi", "video/x-msvideo");
         SmtpContent.FileExtensionContentType.Add("zip", "application/zip");
      }

      /// <summary>
      /// Obtiene el tipo de contenido.
      /// </summary>
      /// <param name="inExtension"></param>
      /// <returns></returns>
      private static String GetContentType(String inExtension)
      {
         String s = inExtension.Replace(".", "").ToLower();
         if (SmtpContent.FileExtensionContentType.ContainsKey(s.ToLower()) == true)
         {
            return SmtpContent.FileExtensionContentType[s.ToLower()];
         }
         return "application/octet-stream";
      }

      /// <summary>
      /// Carga los datos.
      /// </summary>
      /// <param name="inFilePath"></param>
      public void LoadData(String inFilePath)
      {
         FileInfo fi = null;
         Byte[] b = null;
         FileStream fsm = null;

         fi = new FileInfo(inFilePath);

         this.ContentType.Value = SmtpContent.GetContentType(Path.GetExtension(inFilePath).Replace(".", ""));
         this.ContentType.Name = fi.Name;
         this.ContentDisposition.FileName = fi.Name;
         this.ContentTransferEncoding = TransferEncoding.Base64;
         this.ContentDisposition.Value = "attachment";

         b = new Byte[fi.Length];
         using (fsm = new FileStream(inFilePath, FileMode.Open))
         {
            fsm.Read(b, 0, b.Length);
            this.BodyText = Convert.ToBase64String(b);
            fsm.Close();
         }
      }

      /// <summary>
      /// Carga los datos.
      /// </summary>
      /// <param name="inByte"></param>
      public void LoadData(Byte[] inByte)
      {
         Byte[] b = inByte;

         this.ContentTransferEncoding = TransferEncoding.Base64;
         this.ContentDisposition.Value = "attachment";
         this.BodyText = Convert.ToBase64String(b);
      }

      /// <summary>
      /// 実際に送信される文字列のデータを取得します。
      /// </summary>
      /// <returns></returns>
      public String GetDataText()
      {
         StringBuilder sb = new StringBuilder();
         String s = "";

         if (this.IsMultiPart == false &&
             this.Contents.Count > 0)
         {
            this.ContentType.Value = "multipart/mixed";
         }
         if (this.IsBody == true)
         {
            sb.AppendFormat("Content-Type: {0}; charset=\"{1}\"", this.ContentType.Value, this.ContentEncoding.WebName);
            sb.Append(MailParser.NewLine);
         }
         else
         {
            sb.AppendFormat("Content-Type: {0};", this.ContentType.Value);
            sb.Append(MailParser.NewLine);
            if (String.IsNullOrEmpty(this.ContentType.Name) == false)
            {
               sb.AppendFormat(" name=\"{0}\"", MailParser.EncodeToMailHeaderLine(this.ContentType.Name, this.ContentTransferEncoding, this.ContentEncoding
                   , MailParser.MaxCharCountPerRow - 8));
               sb.Append(MailParser.NewLine);
            }
         }
         sb.AppendFormat("Content-Transfer-Encoding: {0}", MailParser.ToTransferEncoding(this.ContentTransferEncoding));
         sb.Append(MailParser.NewLine);
         if (String.IsNullOrEmpty(this["Content-Disposition"]) == false)
         {
            sb.AppendFormat("Content-Disposition: {0};", this.ContentDisposition.Value);
            sb.Append(MailParser.NewLine);
            if (String.IsNullOrEmpty(this.ContentDisposition.FileName) == false)
            {
               sb.AppendFormat(" filename=\"{0}\"", MailParser.EncodeToMailHeaderLine(this.ContentDisposition.FileName, this.ContentTransferEncoding, this.ContentEncoding
                   , MailParser.MaxCharCountPerRow - 12));
               sb.Append(MailParser.NewLine);
            }
         }
         if (String.IsNullOrEmpty(this["Content-Description"]) == false)
         {
            sb.AppendFormat("Content-Description: {0}", this["Content-Description"]);
            sb.Append(MailParser.NewLine);
         }

         if (this.IsMultiPart == true)
         {
            for (int i = 0; i < this.zContents.Count; i++)
            {
               sb.Append(MailParser.NewLine);
               sb.Append("--");
               sb.Append(this.MultiPartBoundary);
               sb.Append(MailParser.NewLine);
               sb.Append(this.zContents[i].GetDataText());
               sb.Append(MailParser.NewLine);
            }
            sb.Append(MailParser.NewLine);
            sb.AppendFormat("--{0}--", this.MultiPartBoundary);
         }
         else
         {
            sb.Append(MailParser.NewLine);
            if (this.IsAttachment == true)
            {
               s = this.BodyText;
            }
            else
            {
               s = MailParser.EncodeToMailBody(this.BodyText, this.ContentTransferEncoding, this.ContentEncoding);
            }
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

         return sb.ToString();
      }
   }

}
