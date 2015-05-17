using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Cosmo.Net.Mail
{

   /// <summary>
   /// Implementa un parser para el contenido de los mensajes de correo electrónico.
   /// </summary>
   public class MailParser
   {
      private static TimeSpan zTimeZoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
      private static String zDateTimeFormatString = "ddd, dd MMM yyyy HH:mm:ss +0000";

      /// <summary>This is multi-part message in MIME format</summary>
      public const String ThisIsMultiPartMessageInMIMEFormat = "This is multi-part message in MIME format.";
      /// <summary>Cambio de línea</summary>
      public const String NewLine = "\r\n";
      /// <summary>Número de carácteres máximo por línea.</summary>
      public const Int32 MaxCharCountPerRow = 76;

      /// <summary>
      /// Devuelve una instancia de MailParser.
      /// </summary>
      static MailParser()
      {
         MailParser.SetDateTimeFormatString();
      }

      /// <summary>
      /// 日付文字列の書式を設定する文字列です。
      /// </summary>
      public static String DateTimeFormatString
      {
         get { return MailParser.zDateTimeFormatString; }
      }

      /// <summary>
      /// 日付文字列のUTCからのオフセットをセットするための値を取得または設定します。
      /// この値を変更することによりDateTimeFormatStringのオフセットの値を変更可能です。
      /// </summary>
      public static TimeSpan TimeZoneOffset
      {
         get { return MailParser.zTimeZoneOffset; }
         set
         {
            MailParser.zTimeZoneOffset = value;
            MailParser.SetDateTimeFormatString();
         }
      }

      /// <summary>
      /// レスポンスが+OKを含むかどうかを取得します。
      /// </summary>
      public static Boolean IsResponseOk(String inText)
      {
         if (Regex.IsMatch(inText, @"^.*\+OK.*$", RegexOptions.IgnoreCase) == true)
         {
            return true;
         }

         return false;
      }

      /// <summary>
      /// Fromの文字列から送信先メールアドレスとして使用可能な文字列を取得します。
      /// </summary>
      public static String MailAddress(String inFrom)
      {
         Regex rg = new Regex("[<]{1}(?<MailAddress>[^>]+)[>]{1}");
         Match m = null;

         m = rg.Match(inFrom);
         if (String.IsNullOrEmpty(m.Value) == true)
         {
            return inFrom;
         }
         return m.Groups["MailAddress"].Value;
      }

      /// <summary>
      /// 日付データからメールのヘッダーで使用する日付文字列を生成して取得します。
      /// </summary>
      public static String Date(DateTime inDateTime)
      {
         return inDateTime.ToString(MailParser.DateTimeFormatString, new CultureInfo("en-US"));
      }

      /// <summary>
      /// 文字列からTransferEncodingの値を取得します。
      /// </summary>
      public static TransferEncoding ToTransferEncoding(String inText)
      {
         switch (inText.ToLower())
         {
            case "7bit": return TransferEncoding.SevenBit;
            case "base64": return TransferEncoding.Base64;
            case "quoted-printable": return TransferEncoding.QuotedPrintable;
         }
         return TransferEncoding.SevenBit;
      }

      /// <summary>
      /// TransferEncodingから文字列を取得します。
      /// </summary>
      public static String ToTransferEncoding(TransferEncoding inEncoding)
      {
         switch (inEncoding)
         {
            case TransferEncoding.SevenBit: return "7bit";
            case TransferEncoding.Base64: return "Base64";
            case TransferEncoding.QuotedPrintable: return "Quoted-Printable";
         }
         return "7bit";
      }

      /// <summary>
      /// メールヘッダーの文字列をRFC2047の仕様に従ってエンコードします。
      /// </summary>
      public static String EncodeToMailHeaderLine(String inText, TransferEncoding inEncodeType, Encoding inEncoding)
      {
         return MailParser.EncodeToMailHeaderLine(inText, inEncodeType, inEncoding, 78);
      }

      /// <summary>
      /// メールヘッダーの文字列をRFC2047の仕様に従ってエンコードします。
      /// </summary>
      public static String EncodeToMailHeaderLine(String inText, TransferEncoding inEncodeType, Encoding inEncoding, Int32 inMaxCharCount)
      {
         Byte[] bb = null;
         StringBuilder sb = new StringBuilder();
         Int32 StartIndex = 0;
         Int32 CharCountPerRow = 0;
         Int32 ByteCount = 0;

         if (inMaxCharCount > MailParser.MaxCharCountPerRow)
         { throw new ArgumentException("inMaxCharCount must less than MailParser.MaxCharCountPerRow."); }

         if (String.IsNullOrEmpty(inText) == true)
         { 
            return string.Empty; 
         }

         if (MailParser.AsciiCharOnly(inText) == true)
         {
            StartIndex = 0;
            CharCountPerRow = inMaxCharCount;
            for (int i = 0; i < inText.Length; i++)
            {
               sb.Append(inText[i]);
               if (StartIndex == CharCountPerRow)
               {
                  sb.Append(MailParser.NewLine);
                  StartIndex = 0;
                  CharCountPerRow = MailParser.MaxCharCountPerRow;
                  if (i < inText.Length - 1)
                  {
                     sb.Append("\t");
                  }
               }
               else
               {
                  StartIndex += 1;
               }
            }
            return sb.ToString();
         }
         if (inEncodeType == TransferEncoding.Base64)
         {
            CharCountPerRow = (Int32)Math.Floor((inMaxCharCount - (inEncoding.WebName.Length + 10)) * 0.75);
            for (int i = 0; i < inText.Length; i++)
            {
               ByteCount = inEncoding.GetByteCount(inText.Substring(StartIndex, (i + 1) - StartIndex));
               if (ByteCount > CharCountPerRow)
               {
                  bb = inEncoding.GetBytes(inText.Substring(StartIndex, i - StartIndex));
                  sb.AppendFormat("=?{0}?B?{1}?={2}\t", inEncoding.WebName, Convert.ToBase64String(bb), MailParser.NewLine);
                  StartIndex = i;
                  CharCountPerRow = (Int32)Math.Floor((MailParser.MaxCharCountPerRow - (inEncoding.WebName.Length + 10)) * 0.75);
               }
            }
            bb = inEncoding.GetBytes(inText.Substring(StartIndex));
            sb.AppendFormat("=?{0}?B?{1}?=", inEncoding.WebName, Convert.ToBase64String(bb));

            return sb.ToString();
         }
         else if (inEncodeType == TransferEncoding.QuotedPrintable)
         {
            CharCountPerRow = (Int32)Math.Floor((inMaxCharCount - (Double)(inEncoding.WebName.Length + 10)) / 3);
            for (int i = 0; i < inText.Length; i++)
            {
               ByteCount = inEncoding.GetByteCount(inText.Substring(StartIndex, (i + 1) - StartIndex));
               if (ByteCount > CharCountPerRow)
               {
                  bb = inEncoding.GetBytes(inText.Substring(StartIndex, i - StartIndex));
                  sb.AppendFormat("=?{0}?Q?{1}?={2}\t", inEncoding.WebName, MailParser.ToQuotedPrintable(Encoding.ASCII.GetString(bb)), MailParser.NewLine);
                  StartIndex = i;
                  CharCountPerRow = (Int32)Math.Floor((MailParser.MaxCharCountPerRow - (inEncoding.WebName.Length + 10)) * 0.75);
               }
            }
            bb = inEncoding.GetBytes(inText.Substring(StartIndex));
            sb.AppendFormat("=?{0}?Q?{1}?=", inEncoding.WebName, MailParser.ToQuotedPrintable(Encoding.ASCII.GetString(bb)));

            return sb.ToString();
         }
         else
         {
            return inText;
         }
      }

      /// <summary>
      /// メールヘッダーの文字列をRFC2047の仕様に従ってデコードします。
      /// </summary>
      public static String DecodeFromMailHeaderLine(String inLine)
      {
         Regex rg = new Regex(@"[=\?]{2}(?<Encode>[^?]+)[\?](?<BorQ>[B|b|Q|q])[\?](?<Subject>[^?]+)[\?=]{2}");
         MatchCollection mc = null;
         Byte[] b = null;
         String NewLine = inLine;

         mc = rg.Matches(NewLine);
         foreach (Match m in mc)
         {
            if (m.Groups.Count < 3)
            {
               continue;
            }
            if (m.Groups["BorQ"].Value.ToUpper() == "B")
            {
               b = Convert.FromBase64String(m.Groups["Subject"].Value);
            }
            else if (m.Groups["BorQ"].Value.ToUpper() == "Q")
            {
               b = MailParser.FromQuotedPrintable(m.Groups["Subject"].Value);
            }
            else
            {
               continue;
            }
            NewLine = NewLine.Replace(m.Value, Encoding.GetEncoding(m.Groups["Encode"].Value).GetString(b));
         }
         return NewLine;
      }

      /// <summary>
      /// メール本文の文字列をメールの仕様に従ってエンコードします。
      /// </summary>
      public static String EncodeToMailBody(String inText, TransferEncoding inEncodeType, Encoding inEncoding)
      {
         Byte[] bb = null;
         bb = inEncoding.GetBytes(inText);
         if (inEncodeType == TransferEncoding.Base64)
         {
            return Convert.ToBase64String(bb);
         }
         else if (inEncodeType == TransferEncoding.QuotedPrintable)
         {
            return MailParser.ToQuotedPrintable(inEncoding.GetString(bb));
         }
         else
         {
            return inEncoding.GetString(bb);
         }
      }

      /// <summary>
      /// メール本文の文字列を解析し、デコードされたメール本文の文字列を取得します。
      /// </summary>
      public static String DecodeFromMailBody(String inText, TransferEncoding inEncodeType, Encoding inEncoding)
      {
         Byte[] b = null;

         if (inEncodeType == TransferEncoding.Base64)
         {
            b = Convert.FromBase64String(inText);
         }
         else if (inEncodeType == TransferEncoding.QuotedPrintable)
         {
            b = MailParser.FromQuotedPrintable(inText);
         }
         else
         {
            b = inEncoding.GetBytes(inText);
         }
         return inEncoding.GetString(b);
      }

      /// <summary>
      /// Boundary文字列を生成します。
      /// </summary>
      public static string GenerateBoundary()
      {
         String s = String.Format("NextPart_{0}", Guid.NewGuid().ToString("D"));
         return s;
      }

      /// <summary>
      /// QuotedPrintableでデコードされた文字列をエンコードして文字列を取得します。
      /// </summary>
      public static String ToQuotedPrintable(String inText)
      {
         StringReader sr = new StringReader(inText);
         StringBuilder sb = new StringBuilder();
         Int32 i;

         while ((i = sr.Read()) > 0)
         {
            if ((i > 32 && i < 127) ||
                i == 13 ||
                i == 10 ||
                i == 9 ||
                i == 32)
            {
               sb.Append(Convert.ToChar(i));
            }
            else
            {
               sb.Append("=");
               sb.Append(Convert.ToString(i, 16).ToUpper());
            }
         }
         return sb.ToString();
      }

      /// <summary>
      /// QuotedPrintableでエンコードされた文字列をデコードして文字列を取得します。
      /// </summary>
      public static Byte[] FromQuotedPrintable(String inText)
      {
         if (inText == null)
         { throw new ArgumentNullException(); }

         MemoryStream ms = new MemoryStream();
         String line;
         Boolean AddNewLine = false;
         Int32 i = 0;

         using (StringReader sr = new StringReader(inText))
         {
            while ((line = sr.ReadLine()) != null)
            {
               // 行の最後の文字が=の場合、行が継続していることを示す。
               if (line.EndsWith("="))
               {
                  // =を取り除く
                  line = line.Substring(0, line.Length - 1);
                  AddNewLine = false;
               }
               else
               {
                  AddNewLine = true;
               }
               i = 0;
               while (i < line.Length)
               {
                  // 現在位置の文字が"="である場合
                  if (line.Substring(i, 1) == "=")
                  {
                     // 16進文字列を取得
                     String target = line.Substring(i + 1, 2);
                     ms.WriteByte(Convert.ToByte(target, 16));
                     i += 3;
                  }
                  // 現在位置の文字が"="ではない場合
                  else
                  {
                     String target = line.Substring(i, 1);
                     ms.WriteByte(Convert.ToByte(Char.Parse(target)));
                     i = i + 1;
                  }
               }

               if (AddNewLine == true)
               {
                  ms.WriteByte(13);
                  ms.WriteByte(10);
               }
            }

         }
         return ms.ToArray();
      }

      /// <summary>
      /// 本文の終端までデータを受信したかどうかを示す値を取得します。
      /// </summary>
      public static Boolean IsReceiveCompleted(String inText)
      {
         Regex rx = new Regex(String.Format(@"{0}\.{0}", MailParser.NewLine));
         if (rx.IsMatch(inText) == true)
         {
            return true;
         }
         return false;
      }

      /// <summary>
      /// 文字列をBase64文字列に変更します。
      /// </summary>
      public static String ToBase64String(String inText)
      {
         Byte[] b = null;
         b = Encoding.ASCII.GetBytes(inText);
         return Convert.ToBase64String(b, 0, b.Length);
      }

      /// <summary>
      /// MD5ダイジェストに従って文字列を変換します。
      /// </summary>
      public static String ToMd5DigestString(String inText)
      {
         Byte[] bb = null;
         StringBuilder sb = new StringBuilder();

         bb = Encoding.Default.GetBytes(inText);
         MD5 md5 = new MD5CryptoServiceProvider();
         bb = md5.ComputeHash(bb);
         for (int i = 0; i < bb.Length; i++)
         {
            sb.Append(bb[i].ToString("X2"));
         }
         return sb.ToString().ToLower();
      }

      /// <summary>
      /// Cram-MD5に従って文字列を変換します。
      /// </summary>
      public static String ToCramMd5String(String inChallenge, String inUserName, String inPassword)
      {
         StringBuilder sb = new StringBuilder();
         Byte[] bb = null;
         HMACMD5 md5 = new HMACMD5(Encoding.ASCII.GetBytes(inPassword));

         // Base64デコードしたチャレンジコードに対してパスワードをキーとしたHMAC-MD5ハッシュ値を計算する
         bb = md5.ComputeHash(Convert.FromBase64String(inChallenge));

         // 計算したHMAC-MD5ハッシュ値のbyte[]を16進表記の文字列に変換する
         for (int i = 0; i < bb.Length; i++)
         {
            sb.Append(bb[i].ToString("x02"));
         }
         
         // ユーザ名と計算したHMAC-MD5ハッシュ値をBase64エンコードしてレスポンスとして返す
         bb = Encoding.ASCII.GetBytes(String.Format("{0} {1}", inUserName, sb.ToString()));
         return Convert.ToBase64String(bb);
      }

      /// <summary>
      /// 指定した文字がASCII文字列のみで構成されているかどうかを示す値を取得します。
      /// </summary>
      public static Boolean AsciiCharOnly(String inText)
      {
         Regex rx = new Regex("[^\x00-\x7F]");
         if (rx.IsMatch(inText) == true)
         {
            return false;
         }
         return true;
      }

      #region Private members

      /// <summary>
      /// Implementa una clase estática para la codificación hexadecimal.
      /// </summary>
      private class RegexList
      {
         public static readonly Regex HexDecoder = new Regex("((\\=([0-9A-F][0-9A-F]))*)", RegexOptions.IgnoreCase);
      }

      private static void SetDateTimeFormatString()
      {
         MailParser.zDateTimeFormatString = String.Format("ddd, dd MMM yyyy HH:mm:ss +{0:00}{1:00}", MailParser.zTimeZoneOffset.Hours, MailParser.zTimeZoneOffset.Minutes);
      }

      #endregion

   }

}