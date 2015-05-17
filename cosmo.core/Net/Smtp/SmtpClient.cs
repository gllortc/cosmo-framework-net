using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Cosmo.Net.Mail;
using Cosmo.Net.Pop3;
using Cosmo.Net.Smtp.Command;

namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Implementa el cliente Smtp.
   /// </summary>
   public class SmtpClient : IDisposable
   {
      private const Int32 MaxBufferReadSize = 128;
      private SmtpAuthenticateMode zMode = SmtpAuthenticateMode.Auto;
      private String zUserName = "";
      private String zPassword = "";
      private String zServerName = "127.0.0.1";
      private Int32 zPort = SmtpClient.DefaultPort;
      private String zHostName = "";
      private Boolean zSsl = false;
      private Boolean zTls = false;
      private Int32 zReceiveTimeout = 10000;
      private Int32 zReceiveBufferSize = 8192;
      private Boolean zPopBeforeSmtp = false;
      private Pop3Client zPop3Client = new Pop3Client();
      private TcpClient zTcpClient = null;
      private Stream zStream = null;
      private StreamReader zReader = null;
      private Int32 zThreadSleepMilliseconds = 10;
      private SmtpConnectionState zState = SmtpConnectionState.Disconnected;
      private Boolean zCommnicating = false;

      /// <summary>Puerto por defecto para el servicio Smtp.</summary>
      public const Int32 DefaultPort = 25;
      /// <summary>Puerto por defecto para el servicio Smtp usando SSL.</summary>
      public const Int32 DefaultSslPort = 443;

      /// <summary>
      /// Devuelve una instancia de SmtpClient.
      /// </summary>
      public SmtpClient() { }

      #region Properties

      /// <summary>
      /// 認証の方法を取得または設定します。
      /// </summary>
      public SmtpAuthenticateMode AuthenticateMode
      {
         get { return this.zMode; }
         set { this.zMode = value; }
      }

      /// <summary>
      /// 認証に使用するユーザー名を取得または設定します。
      /// </summary>
      public String UserName
      {
         get { return this.zUserName; }
         set { this.zUserName = value; }
      }

      /// <summary>
      /// 認証に使用するパスワードを取得または設定します。
      /// </summary>
      public String Password
      {
         get { return this.zPassword; }
         set { this.zPassword = value; }
      }

      /// <summary>
      /// SMTPメールサーバーのサーバー名を取得または設定します。
      /// </summary>
      public String ServerName
      {
         get { return this.zServerName; }
         set { this.zServerName = value; }
      }

      /// <summary>
      /// 通信に使用するPort番号を取得または設定します。
      /// </summary>
      public Int32 Port
      {
         get { return this.zPort; }
         set { this.zPort = value; }
      }

      /// <summary>
      /// 送信元マシンのホスト名を取得または設定します。
      /// </summary>
      public String HostName
      {
         get { return this.zHostName; }
         set { this.zHostName = value; }
      }

      /// <summary>
      /// 通信をSSLで暗号化するかどうかを示す値を取得または設定します。
      /// </summary>
      public Boolean Ssl
      {
         get { return this.zSsl; }
         set { this.zSsl = value; }
      }

      /// <summary>
      /// 通信にTLSを使用するかどうかを示す値を取得します。
      /// </summary>
      public Boolean Tls
      {
         get { return this.zTls; }
         set { this.zTls = value; }
      }

      /// <summary>
      /// 受信処理のタイムアウトの秒数をミリ秒単位で取得または設定します。
      /// </summary>
      public Int32 ReceiveTimeout
      {
         get { return this.zReceiveTimeout; }
         set
         {
            this.zReceiveTimeout = value;
            if (this.zTcpClient != null)
            {
               this.zTcpClient.ReceiveTimeout = this.zReceiveTimeout;
            }
         }
      }

      /// <summary>
      /// 受信データのバッファサイズを取得または設定します。
      /// </summary>
      public Int32 ReceiveBufferSize
      {
         get { return this.zReceiveBufferSize; }
         set
         {
            this.zReceiveBufferSize = value;
            if (this.zTcpClient != null)
            {
               this.zTcpClient.ReceiveBufferSize = this.zReceiveBufferSize;
            }
         }
      }

      /// <summary>
      /// 受信データを取得する際にサーバーからのレスポンスデータを待機する秒数をミリ秒単位で取得または設定します。
      /// </summary>
      public Int32 ThreadSleepMilliseconds
      {
         get { return this.zThreadSleepMilliseconds; }
         set { this.zThreadSleepMilliseconds = value; }
      }

      /// <summary>
      /// 接続の状態を示す値を取得します。
      /// </summary>
      public SmtpConnectionState State
      {
         get { return this.zState; }
      }

      /// <summary>
      /// サーバーへ接続済みかどうかを示す値を取得します。
      /// </summary>
      public Boolean Available
      {
         get { return this.zState != SmtpConnectionState.Disconnected; }
      }

      /// <summary>
      /// サーバーと通信中かどうかを示す値を取得します。
      /// サーバーにCommandを送信してから受信データを全て受信し終わるまでの間、trueを返します。
      /// </summary>
      public Boolean Commnicating
      {
         get { return this.zCommnicating; }
      }

      /// <summary>
      /// PopBeforeSmtp認証を行うかどうかを示す値を取得または設定します。
      /// </summary>
      public Boolean PopBeforeSmtp
      {
         get { return this.zPopBeforeSmtp; }
         set { this.zPopBeforeSmtp = value; }
      }

      /// <summary>
      /// PopBeforeSmtp認証を行う場合に使用されるPop3Clientを取得します。
      /// </summary>
      public Pop3Client Pop3Client
      {
         get { return this.zPop3Client; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// サーバーへの接続を開きます。
      /// </summary>
      public SmtpConnectionState Open()
      {
         SmtpCommandResult rs = null;

         this.zTcpClient = this.GetTcpClient();
         if (this.zTcpClient == null)
         {
            this.zState = SmtpConnectionState.Disconnected;
         }
         else
         {
            if (this.zSsl == true)
            {
               SslStream ssl = new SslStream(this.zTcpClient.GetStream(), true, SmtpClient.RemoteCertificateValidationCallback);
               ssl.AuthenticateAsClient(this.zServerName);
               if (ssl.IsAuthenticated == false)
               {
                  this.zState = SmtpConnectionState.Disconnected;
                  this.zTcpClient = null;
                  return this.zState;
               }
               this.zStream = ssl;
            }
            else
            {
               this.zStream = this.zTcpClient.GetStream();
            }
            this.zReader = new StreamReader(this.zStream, Encoding.ASCII);
            rs = this.GetResponse();
            if (rs.StatusCode == SmtpCommandResultCode.ServiceReady)
            {
               this.zState = SmtpConnectionState.Connected;
            }
            else
            {
               this.zTcpClient = null;
               this.zState = SmtpConnectionState.Disconnected;
            }
         }
         return this.zState;
      }

      /// <summary>
      /// サーバーへの接続が開かれていない場合、サーバーへの接続を開きます。
      /// </summary>
      public SmtpConnectionState EnsureOpen()
      {
         if (this.zTcpClient != null)
         { 
            return this.zState; 
         }
         
         return this.Open();
      }
      
      /// <summary>
      /// SMTPメールサーバーへログインします。
      /// </summary>
      public Boolean Authenticate()
      {
         SmtpCommandResult rs = null;

         if (this.zPopBeforeSmtp == true)
         {
            Boolean bl = this.zPop3Client.Authenticate();
            this.zPop3Client.Close();
            if (bl == false)
            { 
               return false; 
            }
         }
         if (this.zMode == SmtpAuthenticateMode.Auto)
         {
            if (this.EnsureOpen() == SmtpConnectionState.Connected)
            {
               rs = this.ExecuteEhlo();
               String s = rs.Message.ToUpper();

               if (s.Contains("AUTH") == true)
               {
                  if (s.Contains("PLAIN") == true)
                  { 
                     return this.AuthenticateByPlain(); 
                  }
                  if (s.Contains("LOGIN") == true)
                  { 
                     return this.AuthenticateByLogin(); 
                  }
                  if (s.Contains("CRAM-MD5") == true)
                  { 
                     return this.AuthenticateByCramMD5(); 
                  }
               }
               else
               {
                  rs = this.ExecuteEhlo();
                  return rs.StatusCode == SmtpCommandResultCode.ServiceReady;
               }

               if (this.Tls == true)
               {
                  if (s.Contains("STARTTLS") == false)
                  { throw new SmtpConnectException("TLS is not allowed."); }
                  this.StartTls();
                  rs = this.ExecuteEhlo();
                  return rs.StatusCode == SmtpCommandResultCode.ServiceReady;
               }
            }
         }
         else
         {
            switch (this.zMode)
            {
               case SmtpAuthenticateMode.None: return true;
               case SmtpAuthenticateMode.Plain: return this.AuthenticateByPlain();
               case SmtpAuthenticateMode.Login: return this.AuthenticateByLogin();
               case SmtpAuthenticateMode.Cram_MD5: return this.AuthenticateByCramMD5();
            }
         }
         return false;
      }

      /// <summary>
      /// SMTPメールサーバーへPlain認証でログインします。
      /// </summary>
      public Boolean AuthenticateByPlain()
      {
         SmtpCommandResult rs = null;

         if (this.EnsureOpen() == SmtpConnectionState.Connected)
         {
            rs = this.Execute("Auth Plain");
            if (rs.StatusCode != SmtpCommandResultCode.WaitingForAuthentication)
            { 
               return false; 
            }

            rs = this.Execute(MailParser.ToBase64String(String.Format("{0}\0{0}\0{1}", this.zUserName, this.zPassword)));
            if (rs.StatusCode == SmtpCommandResultCode.AuthenticationSuccessful)
            {
               this.zState = SmtpConnectionState.Authenticated;
            }
         }
         return this.zState == SmtpConnectionState.Authenticated;
      }

      /// <summary>
      /// SMTPメールサーバーへLogin認証でログインします。
      /// </summary>
      public Boolean AuthenticateByLogin()
      {
         SmtpCommandResult rs = null;

         if (this.EnsureOpen() == SmtpConnectionState.Connected)
         {
            rs = this.Execute("Auth Login");
            if (rs.StatusCode != SmtpCommandResultCode.WaitingForAuthentication)
            { 
               return false; 
            }

            rs = this.Execute(MailParser.ToBase64String(this.zUserName));
            if (rs.StatusCode != SmtpCommandResultCode.WaitingForAuthentication)
            { 
               return false; 
            }

            rs = this.Execute(MailParser.ToBase64String(this.zPassword));
            if (rs.StatusCode == SmtpCommandResultCode.AuthenticationSuccessful)
            {
               this.zState = SmtpConnectionState.Authenticated;
            }
         }
         return this.zState == SmtpConnectionState.Authenticated;
      }

      /// <summary>
      /// SMTPメールサーバーへCRAM-MD5認証でログインします。
      /// </summary>
      public Boolean AuthenticateByCramMD5()
      {
         SmtpCommandResult rs = null;

         if (this.EnsureOpen() == SmtpConnectionState.Connected)
         {
            rs = this.Execute("Auth CRAM-MD5");
            if (rs.StatusCode != SmtpCommandResultCode.WaitingForAuthentication)
            { return false; }

            String s = MailParser.ToCramMd5String(rs.Message, this.zUserName, this.zPassword);
            rs = this.Execute(s);
            if (rs.StatusCode == SmtpCommandResultCode.AuthenticationSuccessful)
            {
               this.zState = SmtpConnectionState.Authenticated;
            }
         }
         return this.zState == SmtpConnectionState.Authenticated;
      }

      /// <summary>
      /// 同期でSMTPメールサーバーへコマンドを送信し、コマンドの種類によってはレスポンスデータを受信して返します。
      /// </summary>
      public SmtpCommandResult Execute(SmtpCommand inCommand)
      {
         return this.Execute(inCommand.GetCommandString());
      }

      /// <summary>
      /// SMTPメールサーバーへHELOコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteHelo()
      {
         this.EnsureOpen();
         return this.Execute(new Helo(this.zHostName));
      }

      /// <summary>
      /// SMTPメールサーバーへEHLOコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteEhlo()
      {
         this.EnsureOpen();
         return this.Execute(new Ehlo(this.zHostName));
      }

      /// <summary>
      /// SMTPメールサーバーへMAILコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteMail(String inReversePath)
      {
         this.EnsureOpen();
         return this.Execute(new Smtp.Command.Mail(inReversePath));
      }

      /// <summary>
      /// SMTPメールサーバーへRCPTコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteRcpt(String inForwardPath)
      {
         this.EnsureOpen();
         return this.Execute(new Rcpt(inForwardPath));
      }

      /// <summary>
      /// SMTPメールサーバーへDATAコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteData()
      {
         this.EnsureOpen();
         return this.Execute(new Cosmo.Net.Smtp.Command.Data());
      }

      /// <summary>
      /// SMTPメールサーバーへRESETコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteReset()
      {
         this.EnsureOpen();
         return this.Execute(new Reset());
      }

      /// <summary>
      /// SMTPメールサーバーへVRFYコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteVrfy(String inUserName)
      {
         this.EnsureOpen();
         return this.Execute(new Vrfy(inUserName));
      }

      /// <summary>
      /// SMTPメールサーバーへEXPNコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteExpn(String inMailingList)
      {
         this.EnsureOpen();
         return this.Execute(new Expn(inMailingList));
      }

      /// <summary>
      /// SMTPメールサーバーへHELPコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteHelp()
      {
         this.EnsureOpen();
         return this.Execute(new Help());
      }

      /// <summary>
      /// SMTPメールサーバーへNOOPコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteNoop()
      {
         this.EnsureOpen();
         return this.Execute(new Noop());
      }

      /// <summary>
      /// SMTPメールサーバーへQUITコマンドを送信します。
      /// </summary>
      public SmtpCommandResult ExecuteQuit()
      {
         this.EnsureOpen();
         return this.Execute(new Quit());
      }

      /// <summary>
      /// Envia un mensaje de correo.
      /// </summary>
      public SendMailResult SendMail(SmtpMessage inMessage)
      {
         return this.SendMail(inMessage.From, inMessage.To, inMessage.Cc, inMessage.Bcc, inMessage.GetDataText());
      }

      /// <summary>
      /// メールを送信し、送信結果となるSendMailResultを取得します。
      /// </summary>
      public SendMailResult SendMail(String inFrom, String inTo, String inCc, String inBcc, String inText)
      {
         List<MailAddress> To = new List<MailAddress>();
         List<MailAddress> Cc = new List<MailAddress>();
         List<MailAddress> Bcc = new List<MailAddress>();
         String[] ss = null;

         ss = inTo.Split(',');
         for (int i = 0; i < ss.Length; i++)
         {
            if (String.IsNullOrEmpty(ss[i]) == true)
            { continue; }
            To.Add(MailAddress.Create(ss[i]));
         }
         ss = inCc.Split(',');
         for (int i = 0; i < ss.Length; i++)
         {
            if (String.IsNullOrEmpty(ss[i]) == true)
            { continue; }
            Cc.Add(MailAddress.Create(ss[i]));
         }
         ss = inBcc.Split(',');
         for (int i = 0; i < ss.Length; i++)
         {
            if (String.IsNullOrEmpty(ss[i]) == true)
            { continue; }
            Bcc.Add(MailAddress.Create(ss[i]));
         }
         return this.SendMail(inFrom, To, Cc, Bcc, inText);
      }

      /// <summary>
      /// メールを送信し、送信結果となるSendMailResultを取得します。
      /// </summary>
      public SendMailResult SendMail(String inFrom, List<MailAddress> inTo, List<MailAddress> inCc, List<MailAddress> inBcc, String inText)
      {
         SendMailResult r = new SendMailResult(false);
         SmtpCommandResult rs = null;
         Boolean HasRcpt = false;

         if (this.EnsureOpen() == SmtpConnectionState.Connected)
         {
            // サーバーへメールトランザクションの開始コマンドを送信
            rs = this.ExecuteEhlo();
            if (rs.StatusCode != SmtpCommandResultCode.RequestedMailActionOkay_Completed)
            {
               rs = this.ExecuteHelo();
               if (rs.StatusCode != SmtpCommandResultCode.RequestedMailActionOkay_Completed)
               { 
                  return r; 
               }
            }

            // TLS/SSL通信
            if (this.zTls == true)
            {
               this.StartTls();
            }

            // ログイン認証が必要とされるかチェック
            if (SmtpClient.NeedAuthenticate(rs.Message) == true)
            {
               if (this.Authenticate() == false)
               { 
                  return r; 
               }
            }

            // Mail Fromの送信
            rs = this.ExecuteMail(inFrom);
            if (rs.StatusCode != SmtpCommandResultCode.RequestedMailActionOkay_Completed)
            { 
               return r; 
            }

            // Rcpt Toの送信
            foreach (List<MailAddress> l in new List<MailAddress>[] { inTo, inCc, inBcc })
            {
               for (int i = 0; i < l.Count; i++)
               {
                  rs = this.ExecuteRcpt(l[i].ToString());
                  if (rs.StatusCode == SmtpCommandResultCode.RequestedMailActionOkay_Completed)
                  {
                     HasRcpt = true;
                  }
                  else
                  {
                     r.InvalidMailAddressList.Add(l[i]);
                  }
               }
            }
            if (HasRcpt == false)
            { 
               return r; 
            }
            
            // Dataの送信
            rs = this.ExecuteData();
            if (rs.StatusCode == SmtpCommandResultCode.StartMailInput)
            {
               this.SendCommand(inText + MailParser.NewLine);
            }
            else
            {
               return r;
            }
            rs = this.ExecuteQuit();

            r.SendSuccessful = true;
            return r;
         }
         return new SendMailResult(false);
      }

      /// <summary>
      /// 終了処理を実行し、システムリソースを解放します。
      /// </summary>
      public void Dispose()
      {
         GC.SuppressFinalize(this);
         this.Dispose(true);
      }

      /// <summary>
      /// Libera los recursos usados por esta clase.
      /// </summary>
      protected void Dispose(Boolean disposing)
      {
         if (disposing)
         { }
         if (this.zTcpClient != null)
         {
            ((IDisposable)this.zTcpClient).Dispose();
            this.zTcpClient = null;
         }
         if (this.zPop3Client != null)
         {
            this.zPop3Client.Dispose();
         }
      }

      /// <summary>
      /// Libera los recursos usados por esta clase en el caso de que no se haya hecho usando Dispose().
      /// </summary>
      ~SmtpClient()
      {
         this.Dispose(false);
      }

      #endregion

      #region Private members

      /// <summary>
      /// 接続先のサーバーと通信を行うためのTcpClientオブジェクトを取得します。
      /// </summary>
      private TcpClient GetTcpClient()
      {
         TcpClient tc = null;
         IPHostEntry hostEntry = null;
         IPEndPoint ipe = null;

         try
         {
            hostEntry = Dns.GetHostEntry(this.zServerName);

            foreach (IPAddress address in hostEntry.AddressList)
            {
               ipe = new IPEndPoint(address, this.zPort);
               tc = new TcpClient(ipe.AddressFamily);
               tc.Connect(ipe);
               if (tc.Connected)
               {
                  tc.ReceiveTimeout = this.zReceiveTimeout;
                  tc.ReceiveBufferSize = this.zReceiveBufferSize;
                  return tc;
               }
            }
         }
         catch
         {
         }
         if (tc == null)
         {
            this.zState = SmtpConnectionState.Disconnected;
            return null;
         }
         this.zState = SmtpConnectionState.Connected;
         return tc;
      }

      private static Boolean RemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
      {
         return true;
      }

      private void SendCommand(String inCommand)
      {
         if (this.zTcpClient == null)
         {
            throw new SmtpConnectException("Smtp connection is closed");
         }
         try
         {
            Byte[] bb = Encoding.ASCII.GetBytes(inCommand + MailParser.NewLine);
            this.zStream.Write(bb, 0, bb.Length);
            this.zStream.Flush();
         }
         catch (Exception e)
         {
            throw new SmtpSendException(e.ToString());
         }
      }

      private SmtpCommandResult GetResponse()
      {
         if (this.zTcpClient == null)
         {
            throw new SmtpConnectException("Connection to SMTP server is closed");
         }
         DateTime dtime = DateTime.Now;
         TimeSpan ts;
         SmtpCommandResultLine CurrentLine = null;
         List<SmtpCommandResultLine> l = new List<SmtpCommandResultLine>();

         while (true)
         {
            ts = DateTime.Now - dtime;
            if (ts.TotalMilliseconds > this.zReceiveTimeout)
            {
               throw new SmtpReceiveException("Response timeout");
            }
            
            this.ThreadSleep(this.zThreadSleepMilliseconds);
            CurrentLine = new SmtpCommandResultLine(this.zReader.ReadLine());
            if (CurrentLine.InvalidFormat == true)
            {
               throw new SmtpReceiveException("Response format is invalid");
            }
            l.Add(CurrentLine);
            
            if (CurrentLine.HasNextLine == false)
            { break; }
         }
         this.SetSmtpCommandState();
         return new SmtpCommandResult(l.ToArray());
      }

      private void ThreadSleep(Int32 inMilliseconds)
      {
         while (this.zReader.BaseStream.CanRead == false)
         {
            Thread.Sleep(inMilliseconds);
         }
      }

      /// <summary>
      /// SMTPコマンドの種類に基づいて状態を変化させます。
      /// </summary>
      private void SetSmtpCommandState(SmtpCommand inCommand)
      {
         if (inCommand is Command.Mail)
         {
            this.zState = SmtpConnectionState.MailFromCommandExecuting;
         }
         else if (inCommand is Command.Rcpt)
         {
            this.zState = SmtpConnectionState.RcptToCommandExecuting;
         }
         else if (inCommand is Command.Data)
         {
            this.zState = SmtpConnectionState.DataCommandExecuting;
         }
      }

      /// <summary>
      /// サーバーからのレスポンスの受信時に現在の状態に基づいて状態を変化させます。
      /// </summary>
      private void SetSmtpCommandState()
      {
         this.zCommnicating = false;
         switch (this.zState)
         {
            case SmtpConnectionState.MailFromCommandExecuting: this.zState = SmtpConnectionState.MailFromCommandExecuted; break;
            case SmtpConnectionState.RcptToCommandExecuted: this.zState = SmtpConnectionState.RcptToCommandExecuted; break;
            case SmtpConnectionState.DataCommandExecuting: this.zState = SmtpConnectionState.DataCommandExecuted; break;
         }
      }

      /// <summary>
      /// SMTPサーバーに認証が必要かどうかを示す値を取得します。
      /// </summary>
      private static Boolean NeedAuthenticate(String inText)
      {
         return inText.IndexOf("auth", StringComparison.InvariantCultureIgnoreCase) > -1;
      }

      /// <summary>
      /// StartTLSコマンドをサーバーに対して送信し、暗号化された通信を開始します。
      /// </summary>
      private void StartTls()
      {
         SmtpCommandResult rs = null;

         if (this.EnsureOpen() == SmtpConnectionState.Connected)
         {
            rs = this.Execute("STARTTLS");
            if (rs.StatusCode != SmtpCommandResultCode.ServiceReady)
            { return; }

            this.zSsl = true;
            this.zTls = true;
            SslStream ssl = new SslStream(this.zTcpClient.GetStream(), true, SmtpClient.RemoteCertificateValidationCallback, null);
            ssl.AuthenticateAsClient(this.zServerName);
            this.zStream = ssl;
         }
      }

      /// <summary>
      /// 同期でSMTPメールサーバーへコマンドを送信し、コマンドの種類によってはレスポンスデータを受信して返します。
      /// </summary>
      private SmtpCommandResult Execute(String inCommand)
      {
         this.SendCommand(inCommand);
         this.zCommnicating = true;
         return this.GetResponse();
      }

      #endregion

   }

}
