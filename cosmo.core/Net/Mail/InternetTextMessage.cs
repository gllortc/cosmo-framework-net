using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Cosmo.Net.Mail;

namespace Cosmo.Net
{

   /// <summary>
   /// Represent message defined RFC822,RFC2045-2049 as class.
   /// </summary>
   public class InternetTextMessage
   {
      private List<Field> pHeader;
      private String pBodyData = string.Empty;   // Field for body data (encoded by US-ASCII)
      private Boolean pDecodeHeaderText = true;
      private Encoding pContentEncoding = Encoding.Default;

      /// <summary>
      /// Devuelve una instancia de InternetTextMessage.
      /// </summary>
      public InternetTextMessage()
      {
         this.Initialize(string.Empty);
      }

      /// <summary>
      /// Devuelve una instancia de InternetTextMessage.
      /// </summary>
      public InternetTextMessage(String inText)
      {
         this.Initialize(inText);
      }

      /// <summary>
      /// Devuelve una instancia de InternetTextMessage.
      /// </summary>
      public String this[String inKey]
      {
         get
         {
            Field f = InternetTextMessage.Field.FindField(this.pHeader, inKey);
            if (f == null)
            {
               return string.Empty;
            }
            else
            {
               if (this.pDecodeHeaderText == true)
               {
                  return InternetTextMessage.DecodeHeader(f.Value);
               }
               else
               {
                  return f.Value;
               }
            }
         }
         set
         {
            Field f = InternetTextMessage.Field.FindField(this.pHeader, inKey);
            if (f == null)
            {
               f = new Field(inKey, value);
               this.pHeader.Add(f);
            }
            else
            {
               f.Value = value;
            }
         }
      }

      /// <summary>
      /// Get from value.
      /// </summary>
      public String From
      {
         get { return this["From"]; }
         set { this["From"] = value; }
      }

      /// <summary>
      /// Reply-Toを取得または設定します。
      /// </summary>
      public String ReplyTo
      {
         get { return this["Reply-To"]; }
         set { this["Reply-To"] = value; }
      }

      /// <summary>
      /// In-Reply-Toを取得または設定します。
      /// </summary>
      public String InReplyTo
      {
         get { return this["In-Reply-To"]; }
         set { this["In-Reply-To"] = value; }
      }

      /// <summary>
      /// 件名を取得または設定します。
      /// </summary>
      public String Subject
      {
         get { return this["Subject"]; }
         set { this["Subject"] = value; }
      }

      /// <summary>
      /// Dateを取得または設定します。
      /// </summary>
      public DateTime Date
      {
         get
         {
            DateTime dtime = DateTime.Now;
            DateTime.TryParse(this["Date"], out dtime);
            return dtime;
         }
         set { this["Date"] = MailParser.Date(value); }
      }

      /// <summary>
      /// MessageIDを取得または設定します。
      /// </summary>
      public String MessageID
      {
         get { return this["Message-ID"]; }
         set { this["Message-ID"] = value; }
      }

      /// <summary>
      /// Referencesを取得または設定します。
      /// </summary>
      public String References
      {
         get { return this["References"]; }
         set { this["References"] = value; }
      }

      /// <summary>
      /// ContentTypeを取得します。
      /// </summary>
      public ContentType ContentType
      {
         get
         {
            ContentType ff = null;
            ff = InternetTextMessage.Field.FindField(this.pHeader, "content-type") as ContentType;
            if (ff == null)
            {
               ff = new ContentType("text/plain");
               this.pHeader.Add(ff);
            }
            return ff;
         }
      }

      /// <summary>
      /// Encodingを取得します。
      /// </summary>
      public Encoding ContentEncoding
      {
         get { return this.pContentEncoding; }
         set { this.pContentEncoding = value; }
      }

      /// <summary>
      /// ContentDispositionを取得します。
      /// </summary>
      public ContentDisposition ContentDisposition
      {
         get
         {
            ContentDisposition ff = null;
            ff = InternetTextMessage.Field.FindField(this.pHeader, "content-disposition") as ContentDisposition;
            if (ff == null)
            {
               ff = new ContentDisposition(string.Empty);
               this.pHeader.Add(ff);
            }
            return ff;
         }
      }

      /// <summary>
      /// MultiPartBoundaryの文字列を取得または設定します。
      /// </summary>
      public String MultiPartBoundary
      {
         get { return this.ContentType.Boundary; }
         set { this.ContentType.Boundary = value; }
      }

      /// <summary>
      /// Body部がMIMEで構成されているかどうかを示す値を取得します。
      /// </summary>
      public Boolean IsMultiPart
      {
         get { return Regex.IsMatch(this.ContentType.Value, ".*multipart/.*", RegexOptions.IgnoreCase); }
      }

      /// <summary>
      /// このインスタンスがテキスト形式のデータを表す場合、Trueを返します。
      /// </summary>
      public Boolean IsBody
      {
         get
         {
            return (this.ContentType.Value.StartsWith("text/", StringComparison.CurrentCultureIgnoreCase) == true);
         }
      }

      /// <summary>
      /// このインスタンスがHTML形式のテキストデータを表す場合、Trueを返します。
      /// </summary>
      public Boolean IsHtml
      {
         get
         {
            return (this.ContentType.Value.StartsWith("text/html", StringComparison.CurrentCultureIgnoreCase) == true);
         }
      }

      /// <summary>
      /// このインスタンスが添付ファイルデータの場合、Trueを返します。
      /// </summary>
      public Boolean IsAttachment
      {
         get
         {
            if (String.IsNullOrEmpty(this.ContentDisposition.Value) == false)
            {
               return Regex.Match(this.ContentDisposition.Value, "^attachment.*$").Success;
            }
            return false;
         }
      }

      /// <summary>
      /// ContentDispositionを取得または設定します。
      /// </summary>
      public String ContentDescription
      {
         get { return this["Content-Description"]; }
         set { this["Content-Description"] = value; }
      }

      /// <summary>
      /// ContentTransferEncodingの値を取得または設定します。
      /// </summary>
      public TransferEncoding ContentTransferEncoding
      {
         get { return MailParser.ToTransferEncoding(this["Content-Transfer-Encoding"]); }
         set { this["Content-Transfer-Encoding"] = MailParser.ToTransferEncoding(value); }
      }

      /// <summary>
      /// CharSetの値を取得します。
      /// </summary>
      public String CharSet
      {
         get { return this.ContentEncoding.HeaderName; }
      }

      /// <summary>
      /// ヘッダーのコレクションを取得します。
      /// </summary>
      public List<Field> Header
      {
         get { return this.pHeader; }
      }

      /// <summary>
      /// ヘッダー部分のフィールドの値をデコードするかどうかを示す値を取得します。
      /// </summary>
      public Boolean DecodeHeaderText
      {
         get { return this.pDecodeHeaderText; }
         set { this.pDecodeHeaderText = value; }
      }

      /// <summary>
      /// Body部分のデータを取得します。
      /// </summary>
      protected String BodyData
      {
         get { return this.pBodyData; }
         set { this.pBodyData = value; }
      }

      /// <summary>
      /// 初期化処理を行います。
      /// </summary>
      private void Initialize(String inText)
      {
         this.pHeader = new List<Field>();

         this.Date = DateTime.Now;
         this.pHeader.Add(new Field("From", string.Empty));
         this.pHeader.Add(new Field("Subject", string.Empty));
         this.ContentType.Value = "text/plain";
         this.ContentTransferEncoding = TransferEncoding.SevenBit;
         this.ContentDisposition.Value = "inline";
         this.SetDefaultContentEncoding();

         this.Parse(inText);
      }

      /// <summary>
      /// 既定のContent-Encodingの値をセットします。
      /// </summary>
      private void SetDefaultContentEncoding()
      {
         if (System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja")
         {
            this.ContentEncoding = Encoding.GetEncoding("iso-2022-jp");
         }
      }

      /// <summary>
      /// テキストを解析します。
      /// </summary>
      protected void Parse(String inText)
      {
         StringReader sr = null;
         List<String> l = new List<String>();
         String CurrentLine = string.Empty;
         String FirstLine = string.Empty;
         Boolean IsConcating = false;
         Int32 c = 0;
         StringBuilder sb = new StringBuilder();

         using (sr = new StringReader(inText))
         {
            while (true)
            {
               CurrentLine = sr.ReadLine();
               if (IsConcating == true)
               {
                  // タブ文字のみを除去
                  l.Add(CurrentLine);
               }
               else
               {
                  l.Clear();
                  FirstLine = CurrentLine;
                  // ヘッダーとボディ部の区切り行かどうかチェック
                  if (FirstLine == string.Empty)
                  {
                     // 以降のデータはBody部のデータ
                     sb.Length = 0;
                     while (true)
                     {
                        CurrentLine = sr.ReadLine();
                        if (CurrentLine == null) { break; }
                        if (CurrentLine == "..") { CurrentLine = "."; }
                        if (CurrentLine == ".") { break; }

                        sb.Append(CurrentLine);
                        sb.Append(MailParser.NewLine);
                        if (sr.Peek() == -1) { break; }
                     }
                     this.BodyData = sb.ToString();
                     return;
                  }
               }
               // 次の行の先頭の文字を取得
               c = sr.Peek();
               // 次の行がなかったら終了
               if (c == -1)
               { break; }
               // 次の行の先頭の文字がタブ文字または半角スペースの場合、複数行のフィールドとして連結する
               if (c == 9 || c == 32)
               {
                  IsConcating = true;
                  continue;
               }
               else
               {
                  IsConcating = false;
                  this.ParseHeaderField(FirstLine, l);
                  l.Clear();
                  IsConcating = false;
               }
            }
         }
      }

      /// <summary>
      /// 行の文字列を解析し、フィールドのインスタンスを生成します。
      /// </summary>
      private void ParseHeaderField(String inLine, List<String> inLines)
      {
         Match m = Regex.Match(inLine, @"^(?<key>[^:]*):[\s]*(?<value>.*)");
         Match m1 = null;
         Regex rx = new Regex(@"(?<value>[^;]*)[;]*");
         Field f = null;
         List<String> l = inLines;
         StringBuilder sb = new StringBuilder();

         if (String.IsNullOrEmpty(m.Groups["key"].Value) == false)
         {
            m1 = rx.Match(m.Groups["value"].Value);
            if (m.Groups["key"].Value.ToLower() == "content-type" ||
                m.Groups["key"].Value.ToLower() == "content-disposition")
            {
               sb.Append(inLine);
               for (int i = 0; i < l.Count; i++)
               {
                  sb.Append(l[i].TrimStart('\t'));
               }
               this.ParseContentEncoding(sb.ToString());

               if (m.Groups["key"].Value.ToLower() == "content-type")
               {
                  InternetTextMessage.ParseContentType(this.ContentType, sb.ToString());
                  this.ContentType.Value = m1.Groups["value"].Value;
               }
               else if (m.Groups["key"].Value.ToLower() == "content-disposition")
               {
                  InternetTextMessage.ParseContentDisposition(this.ContentDisposition, sb.ToString());
                  this.ContentDisposition.Value = m1.Groups["value"].Value;
               }
            }
            else
            {
               f = Field.FindField(this.pHeader, m.Groups["key"].Value);
               if (f == null)
               {
                  f = new Field(m.Groups["key"].Value, m1.Groups["value"].Value);
                  this.Header.Add(f);
               }
               else
               {
                  f.Value = m1.Groups["value"].Value;
               }
               for (int i = 0; i < l.Count; i++)
               {
                  f.Value += l[i].TrimStart('\t');
               }
            }
         }
      }

      /// <summary>
      /// Decode header text by RFC2047.
      /// </summary>
      public static String DecodeHeader(String inLine)
      {
         Regex rg = new Regex(@"[\s]{0,1}[=][\?](?<Encode>[^?]+)[\?](?<BorQ>[B|b|Q|q])[\?](?<Subject>[^?]+)[\?][=][\s]{0,1}");
         MatchCollection mc = null;
         Byte[] b = null;
         String NewLine = string.Empty;
         Encoding en = Encoding.Default;

         mc = rg.Matches(inLine);
         // RFC2047のEncodingがされていない場合
         if (mc.Count == 0)
         { return inLine; }

         // RFC2047のEncodingがされている場合
         NewLine = inLine;
         foreach (Match m in mc)
         {
            en = Encoding.GetEncoding(m.Groups["Encode"].Value);
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
            NewLine = NewLine.Replace(m.Value, en.GetString(b));
         }
         return NewLine;
      }

      /// <summary>
      /// Content-Encodingの解析を行います。
      /// </summary>
      private void ParseContentEncoding(String inLine)
      {
         Match m = null;

         // charset=???;
         m = Regex.Match(inLine, ".*charset=[\"]*(?<Value>[^\"]*)[;\n\r]", RegexOptions.IgnoreCase);
         if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
         {
            this.pContentEncoding = this.GetEncoding(m.Groups["Value"].Value, this.ContentEncoding);
         }
         // charset=???
         m = Regex.Match(inLine, ".*charset=[\"]*(?<Value>[^\"]*).*", RegexOptions.IgnoreCase);
         if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
         {
            this.pContentEncoding = this.GetEncoding(m.Groups["Value"].Value, this.ContentEncoding);
         }
      }

      /// <summary>
      /// Parse content-type.
      /// </summary>
      public static void ParseContentType(ContentType inContentType, String inLine)
      {
         Match m = null;

         // name=???;
         m = Regex.Match(inLine, ".*name=[\"]*(?<Value>[^\"]*)[;\n\r]", RegexOptions.IgnoreCase);
         if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
         {
            inContentType.Name = m.Groups["Value"].Value;
         }
         // name=???
         m = Regex.Match(inLine, ".*name=[\"]*(?<Value>[^\"]*).*", RegexOptions.IgnoreCase);
         if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
         {
            inContentType.Name = m.Groups["Value"].Value;
         }
         // boundary
         m = Regex.Match(inLine, ".*boundary=[\"]*(?<Value>[^\"]*).*", RegexOptions.IgnoreCase);
         if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
         {
            inContentType.Boundary = m.Groups["Value"].Value;
         }
      }

      /// <summary>
      /// Get last field when parsing content-type.
      /// </summary>
      private static Field GetLastField(ContentType inContentType, String inLine)
      {
         Int32 NameOrdinal = inLine.IndexOf("Name=", StringComparison.InvariantCultureIgnoreCase);
         Int32 BoundaryOrdinal = inLine.IndexOf("Boundary=", StringComparison.InvariantCultureIgnoreCase);
         if (NameOrdinal < BoundaryOrdinal)
         {
            return Field.FindField(inContentType.Fields, "Boundary");
         }
         else if (NameOrdinal > BoundaryOrdinal)
         {
            return Field.FindField(inContentType.Fields, "Name");
         }
         return null;
      }

      /// <summary>
      /// Parse content-disposision.
      /// </summary>
      public static void ParseContentDisposition(ContentDisposition inContentDisposition, String inLine)
      {
         Match m = null;

         // filename=???;
         m = Regex.Match(inLine, "[;\t\\s]+filename=[\"]*(?<Value>[^\"]*)[;\n\r]", RegexOptions.IgnoreCase);
         if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
         {
            inContentDisposition.FileName = m.Groups["Value"].Value;
         }
         // filename=???
         m = Regex.Match(inLine, "[;\t\\s]+filename=[\"]*(?<Value>[^\"]*).*", RegexOptions.IgnoreCase);
         if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
         {
            inContentDisposition.FileName = m.Groups["Value"].Value;
         }
      }

      /// <summary>
      /// Content-Dispositionの解析を行ったときにFileNameのフィールドがある場合、FileNameのフィールドを取得します。
      /// </summary>
      private static Field GetLastField(ContentDisposition inContentDisposition, String inLine)
      {
         Int32 x = inLine.IndexOf("FileName=", StringComparison.InvariantCultureIgnoreCase);
         if (x > -1)
         {
            return Field.FindField(inContentDisposition.Fields, "FileName");
         }
         return null;
      }

      /// <summary>
      /// 文字列からEncodingを取得します。
      /// </summary>
      private Encoding GetEncoding(String inName, Encoding inDefaultEncoding)
      {
         Encoding en = null;
         try
         {
            en = Encoding.GetEncoding(inName);
         }
         catch
         {
            en = inDefaultEncoding;
         }
         return en;
      }

      /// <summary>
      /// キーと値のセットで構成されるフィールドを表すクラスです。
      /// RFC822で定義されます。
      /// </summary>
      public class Field
      {
         private String zKey;
         private String zValue;

         /// <summary>
         /// Devuelve una instancia de Field.
         /// </summary>
         public Field(String inKey, String inValue)
         {
            this.zKey = inKey;
            this.zValue = inValue;
         }

         /// <summary>
         /// Clave del campo.
         /// </summary>
         public String Key
         {
            get { return this.zKey; }
            set { this.zKey = value; }
         }

         /// <summary>
         /// Valor del campo.
         /// </summary>
         public String Value
         {
            get { return this.zValue; }
            set { this.zValue = value; }
         }

         /// <summary>
         /// Busca un campo.
         /// </summary>
         public static Field FindField(List<Field> inFields, String inKey)
         {
            List<Field> l = inFields.FindAll(delegate(Field f) { return String.Equals(f.Key, inKey, StringComparison.InvariantCultureIgnoreCase); });
            if (l.Count > 0)
            {
               return l[l.Count - 1];
            }
            return null;
         }

         /// <summary>
         /// Devuelve una cadena de texto que representa el valor del campo.
         /// </summary>
         public override string ToString()
         {
            return this.Value.ToString();
         }
      }
   }

}
