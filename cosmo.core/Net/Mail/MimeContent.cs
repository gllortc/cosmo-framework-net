using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cosmo.Net.Mail;

namespace Cosmo.Net
{

   /// <summary>
   /// Representa el contenido MIME de un mensaje de correo.
   /// </summary>
   public class MimeContent : InternetTextMessage
   {
      private List<MimeContent> zContents = null;

      /// <summary>
      /// Devuelve una instancia de MimeContent.
      /// </summary>
      public MimeContent() : base() 
      {
         this.Initialize(string.Empty);
      }

      /// <summary>
      /// Devuelve una instancia de MimeContent.
      /// </summary>
      public MimeContent(String inText) : base(inText)
      {
         this.Initialize(inText);
      }

      /// <summary>
      /// Get mime content collection.
      /// </summary>
      public List<MimeContent> Contents
      {
         get { return this.zContents; }
      }

      /// <summary>
      /// Parse body text and separate as text foe each mime content.
      /// </summary>
      public static List<String> ParseToContentTextList(String inText, String inMultiPartBoundary)
      {
         StringReader sr = null;
         StringBuilder sb = new StringBuilder();
         String CurrentLine = string.Empty;
         String StartOfBoundary = "--" + inMultiPartBoundary;
         String EndOfBoundary = "--" + inMultiPartBoundary + "--";
         List<String> l = new List<string>();
         Boolean IsBegin = false;

         using (sr = new StringReader(inText))
         {
            while (true)
            {
               CurrentLine = sr.ReadLine();
               if (CurrentLine == null)
               { break; }
               if (IsBegin == false)
               {
                  if (CurrentLine == StartOfBoundary)
                  {
                     IsBegin = true;
                     sb.Length = 0;
                  }
                  continue;
               }
               if (CurrentLine == StartOfBoundary ||
                   CurrentLine == EndOfBoundary)
               {
                  if (sb.Length > 0)
                  {
                     l.Add(sb.ToString());
                  }
                  sb.Length = 0;
                  if (CurrentLine == EndOfBoundary)
                  { break; }
               }
               else
               {
                  sb.Append(CurrentLine);
                  sb.Append(MailParser.NewLine);
               }
               if (sr.Peek() == -1)
               {
                  if (IsBegin == true)
                  {
                     l.Add(sb.ToString());
                  }
                  break;
               }
            }
         }
         return l;
      }

      /// <summary>
      /// Decode binary data and output as file to specify file path.
      /// </summary>
      public void DecodeData(String inFilePath)
      {
         Byte[] bb = null;

         if (String.IsNullOrEmpty(this.ContentDisposition.Value) == true)
         { 
            return; 
         }

         if (this.ContentTransferEncoding == TransferEncoding.Base64)
         {
            bb = Convert.FromBase64String(this.BodyData.Replace("\n", string.Empty).Replace("\r", string.Empty));
            using (BinaryWriter sw = new BinaryWriter(new FileStream(inFilePath, FileMode.Create)))
            {
               sw.Write(bb);
               sw.Flush();
               sw.Close();
            }
         }
         else if (this.ContentTransferEncoding == TransferEncoding.QuotedPrintable)
         {
            using (StreamWriter sw = File.CreateText(inFilePath))
            {
               sw.Write(this.ContentEncoding.GetString(MailParser.FromQuotedPrintable(this.BodyData)));
               sw.Flush();
               sw.Close();
            }
         }
         else if (this.ContentTransferEncoding == TransferEncoding.SevenBit)
         {
            bb = Encoding.ASCII.GetBytes(this.BodyData);
            using (BinaryWriter sw = new BinaryWriter(new FileStream(inFilePath, FileMode.Create)))
            {
               sw.Write(bb);
               sw.Flush();
               sw.Close();
            }
         }

      }

      /// <summary>
      /// Decode binary data and output to specify stream.
      /// </summary>
      public void DecodeData(Stream inStream, Boolean inIsClose)
      {
         Byte[] bb = null;

         if (this.IsAttachment == true)
         {
            if (this.ContentTransferEncoding == TransferEncoding.Base64)
            {
               bb = Convert.FromBase64String(this.BodyData.Replace("\n", string.Empty));
               BinaryWriter sw = null;
               try
               {
                  sw = new BinaryWriter(inStream);
                  sw.Write(bb);
                  sw.Flush();
               }
               finally
               {
                  if (inIsClose == true)
                  {
                     sw.Close();
                  }
               }
            }
            else if (this.ContentTransferEncoding == TransferEncoding.QuotedPrintable)
            {
               StreamWriter sw = null;
               try
               {
                  sw = new StreamWriter(inStream);
                  sw.Write(this.ContentEncoding.GetString(MailParser.FromQuotedPrintable(this.BodyData)));
                  sw.Flush();
               }
               finally
               {
                  if (inIsClose == true)
                  {
                     sw.Close();
                  }
               }
            }
            else if (this.ContentTransferEncoding == TransferEncoding.SevenBit)
            {
               bb = Encoding.ASCII.GetBytes(this.BodyData);
               BinaryWriter sw = null;
               try
               {
                  sw = new BinaryWriter(inStream);
                  sw.Write(bb);
                  sw.Flush();
               }
               finally
               {
                  if (inIsClose == true)
                  {
                     sw.Close();
                  }
               }
            }
         }
      }

      /// <summary>
      /// Inicializa el objeto.
      /// </summary>
      private void Initialize(String inText)
      {
         this.zContents = new List<MimeContent>();
         if (this.IsMultiPart == true)
         {
            List<String> l = MimeContent.ParseToContentTextList(this.BodyData, this.MultiPartBoundary);
            for (int i = 0; i < l.Count; i++)
            {
               this.zContents.Add(new MimeContent(l[i]));
            }
         }
      }
   }

}
