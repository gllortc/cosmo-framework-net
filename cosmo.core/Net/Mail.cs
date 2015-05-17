namespace Cosmo.Net
{

   /// <summary>
   /// Implementa una clase helper para tratar direcciones de correo electrónico.
   /// </summary>
   public class Mail
   {

      #region Static Members

      /// <summary>
      /// Permite codificar una dirección de _message con los equivalentes de los códigos numéricos de los carácteres.
      /// </summary>
      /// <param name="mail">Dirección de correo electrónico.</param>
      /// <returns>La dirección de correo electrónico (no enlace) codificado.</returns>
      public static string Obfuscate(string mail)
      {
         int i = 0;
         int acode = 0;
         string repl = string.Empty;
         string tempHTMLEncode = null;

         tempHTMLEncode = mail;
         for (i = tempHTMLEncode.Length; i >= 1; i--)
         {
            acode = System.Convert.ToInt32(tempHTMLEncode[i - 1]);
            if (acode == 32)
               repl = " ";
            else if (acode == 34)
               repl = "\"";
            else if (acode == 38)
               repl = "&";
            else if (acode == 60)
               repl = "<";
            else if (acode == 62)
               repl = ">";
            else if (acode >= 32 || acode <= 127)
               repl = "&#" + System.Convert.ToString(acode) + ";";
            else
               repl = "&#" + System.Convert.ToString(acode) + ";";

            if (repl.Length > 0)
            {
               tempHTMLEncode = tempHTMLEncode.Substring(0, i - 1) +
               repl + tempHTMLEncode.Substring(i);
               repl = string.Empty;
            }
         }

         return tempHTMLEncode;
      }

      /// <summary>
      /// Permite codificar una dirección de _message con los equivalentes de los códigos numéricos de los carácteres.
      /// </summary>
      /// <param name="mail">Dirección de correo electrónico.</param>
      /// <param name="linkText">Texto que aparece en en enlace.</param>
      /// <returns>El enlace al correo electrónico codificado.</returns>
      public static string Obfuscate(string mail, string linkText)
      {
         return "<a href=\"mailto:" + Mail.Obfuscate(mail) + "\">" + Mail.Obfuscate(linkText) + "</a>";
      }

      /// <summary>
      /// Devuelve un valor que indica si una dirección es válida
      /// </summary>
      /// <param name="address">Dirección de correo electrónico a verificar</param>
      /// <returns>Un valor que indica si una dirección es válida</returns>
      public static bool IsValidAddress(string address)
      {
         try
         {
            System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(address);
            return true;
         }
         catch
         {
            return false;
         }
      }

      #endregion

      #region Disabled Code

      /*
        
       private String zValue = string.Empty;
      private String zDisplayName = string.Empty;
      private Boolean zIsDoubleQuote = false;
      private Encoding zEncoding = Encoding.ASCII;
      private TransferEncoding zTransferEncoding = TransferEncoding.Base64;

      /// <summary>
      /// Devuelve una instancia de MailAddress.
      /// </summary>
      public MailAddress(String inMailAddress)
      {
         if (String.IsNullOrEmpty(inMailAddress) == true)
         { throw new ArgumentException(); }

         this.zValue = inMailAddress;
         this.InitializeProperty();
      }

      /// <summary>
      /// Devuelve una instancia de MailAddress.
      /// </summary>
      public MailAddress(String inMailAddress, String inDisplayName)
      {
         if (String.IsNullOrEmpty(inMailAddress) == true)
         { throw new ArgumentException(); }

         this.zValue = inMailAddress;
         this.zDisplayName = inDisplayName;
         this.InitializeProperty();
      }

      /// <summary>
      /// Get or set mailaddress value.
      /// </summary>
      public String Value
      {
         get { return this.zValue; }
         set { this.zValue = value; }
      }

      /// <summary>
      /// 表示名を取得または設定します。
      /// </summary>
      public String DisplayName
      {
         get { return this.zDisplayName; }
         set { this.zDisplayName = value; }
      }

      /// <summary>
      /// 表示名をダブルコーテーションで囲うどうかを示す値を取得または設定します。
      /// </summary>
      public Boolean IsDoubleQuote
      {
         get { return this.zIsDoubleQuote; }
         set { this.zIsDoubleQuote = value; }
      }

      /// <summary>
      /// 表示名のエンコードに使われるEncodingを取得または設定します。
      /// </summary>
      public Encoding Encoding
      {
         get { return this.zEncoding; }
         set { this.zEncoding = value; }
      }

      /// <summary>
      /// 表示名のエンコードに使われるTransferEncodingを取得または設定します。
      /// </summary>
      public TransferEncoding TransferEncoding
      {
         get { return this.zTransferEncoding; }
         set { this.zTransferEncoding = value; }
      }

      /// <summary>
      /// Convierte la dirección de correo a una cadena.
      /// </summary>
      public override string ToString()
      {
         if (String.IsNullOrEmpty(this.zDisplayName) == true)
         {
            return this.zValue;
         }
         if (this.zIsDoubleQuote == true)
         {
            return String.Format("\"{0}\" <{1}>", this.zDisplayName, this.zValue);
         }
         else
         {
            return String.Format("{0} <{1}>", this.zDisplayName, this.zValue);
         }
      }

      /// <summary>
      /// Convierte la dirección de correo a una cadena codificada.
      /// </summary>
      public String ToEncodeString()
      {
         return MailAddress.ToMailAddressText(this.zEncoding, this.zTransferEncoding, this.zValue, this.zDisplayName, this.zIsDoubleQuote);
      }

      /// <summary>
      /// Get mail address text encoded by specify encoding.
      /// </summary>
      public static String ToMailAddressText(String inMailAddress, String inDisplayName, Boolean inIsDoubleQuote)
      {
         if (CultureInfo.CurrentCulture.PropertyName.StartsWith("ja") == true)
         {
            return MailAddress.ToMailAddressText(Encoding.GetEncoding("iso-2022-jp"), TransferEncoding.Base64
                , inMailAddress, inDisplayName, inIsDoubleQuote);
         }
         return MailAddress.ToMailAddressText(Encoding.ASCII, TransferEncoding.Base64, inMailAddress, inDisplayName, inIsDoubleQuote);
      }

      /// <summary>
      /// Get mail address text encoded by specify encoding.
      /// </summary>
      public static String ToMailAddressText(Encoding inEncoding, TransferEncoding inTransferEncoding, String inMailAddress, String inDisplayName, Boolean inIsDoubleQuote)
      {
         if (String.IsNullOrEmpty(inDisplayName) == true)
         {
            return inMailAddress;
         }
         else
         {
            if (inIsDoubleQuote == true)
            {
               return String.Format("\"{0}\" <{1}>", inDisplayName, inMailAddress);
            }
            else
            {
               return String.Format("{0} <{1}>"
                   , MailParser.EncodeToMailHeaderLine(inDisplayName, inTransferEncoding, inEncoding, MailParser.MaxCharCountPerRow - inMailAddress.Length - 3)
                   , inMailAddress);
            }
         }
      }

      /// <summary>
      /// Create MailAddress object by mail address text.
      /// </summary>
      public static MailAddress Create(String inMailAddress)
      {
         Regex rx = new Regex("(?<DisplayName>.*)<(?<MailAddress>[^>]*)>");
         Match m = null;

         m = rx.Match(inMailAddress);
         if (String.IsNullOrEmpty(m.Value) == true)
         {
            rx = new Regex("<(?<MailAddress>[^>]*)>");
            m = rx.Match(inMailAddress);
            if (String.IsNullOrEmpty(m.Value) == true)
            {
               return new MailAddress(inMailAddress);
            }
            else
            {
               return new MailAddress(m.Groups["MailAddress"].Value);
            }
         }
         else
         {
            return new MailAddress(m.Groups["MailAddress"].Value, m.Groups["DisplayName"].Value);
         }
      }

      /// <summary>
      /// Get mailaddress list from mail address list text.
      /// </summary>
      public static List<MailAddress> GetMailAddressList(String inMailAddressListText)
      {
         List<MailAddress> l = new List<MailAddress>();
         String[] ss = null;

         ss = inMailAddressListText.Split(',');
         for (int i = 0; i < ss.Length; i++)
         {
            if (String.IsNullOrEmpty(ss[i].Trim()) == true)
            { continue; }

            l.Add(MailAddress.Create(ss[i].Trim()));
         }
         return l;
      }        
        
      */

      #endregion

   }

}
