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
using Cosmo.Net.Pop3.Command;

namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// Represent and probide functionality about pop3 command.
   /// </summary>
   public class Pop3Client : IDisposable
   {
      private const int MaxBufferReadSize = 128;
      private Pop3AuthenticateMode zMode = Pop3AuthenticateMode.Pop;
      private String zUserName = string.Empty;
      private String zPassword = string.Empty;
      private String zServerName = string.Empty;
      private Int32 zPort = 110;
      private Boolean zSsl = false;
      private Int32 zReceiveTimeout = 10000;
      private Int32 zReceiveBufferSize = 8192;
      private TcpClient zTcpClient = null;
      private Stream zStream = null;
      private StreamReader zReader = null;
      private Int32 zThreadSleepMilliseconds = 10;
      private Pop3ConnectionState zState = Pop3ConnectionState.Disconnected;
      private Boolean zCommnicating = false;

      /// <summary>
      /// Devuelve una instancia de Pop3Client.
      /// </summary>
      public Pop3Client()
      {
      }

      /// <summary>
      /// Devuelve una instancia de Pop3Client.
      /// </summary>
      public Pop3Client(String inUserName, String inPassword, String inServerName)
      {
         this.zUserName = inUserName;
         this.zPassword = inPassword;
         this.zServerName = inServerName;
      }

      /// <summary>
      /// Get or set how authenticate to server.
      /// </summary>
      public Pop3AuthenticateMode AuthenticateMode
      {
         get { return this.zMode; }
         set { this.zMode = value; }
      }

      /// <summary>
      /// Get or set UserName.
      /// </summary>
      public String UserName
      {
         get { return this.zUserName; }
         set { this.zUserName = value; }
      }

      /// <summary>
      /// Get or set password.
      /// </summary>
      public String Password
      {
         get { return this.zPassword; }
         set { this.zPassword = value; }
      }

      /// <summary>
      /// Get or set server.
      /// </summary>
      public String ServerName
      {
         get { return this.zServerName; }
         set { this.zServerName = value; }
      }

      /// <summary>
      /// Get or set port.
      /// </summary>
      public Int32 Port
      {
         get { return this.zPort; }
         set { this.zPort = value; }
      }

      /// <summary>
      /// Get or set use ssl protocol.
      /// </summary>
      public Boolean Ssl
      {
         get { return this.zSsl; }
         set { this.zSsl = value; }
      }

      /// <summary>
      /// Get or set timeout milliseconds.
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
      /// Get or set buffer size to receive.
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
      /// Get or set milliseconds to wait response data from server.
      /// </summary>
      public Int32 ThreadSleepMilliseconds
      {
         get { return this.zThreadSleepMilliseconds; }
         set { this.zThreadSleepMilliseconds = value; }
      }

      /// <summary>
      /// Get connection state.
      /// </summary>
      public Pop3ConnectionState State
      {
         get { return this.zState; }
      }

      /// <summary>
      /// Get connection is ready.
      /// </summary>
      public Boolean Available
      {
         get { return this.zState != Pop3ConnectionState.Disconnected; }
      }

      /// <summary>
      /// Get specify value whether communicating to server or not.
      /// Between send command and finish get all response data,this property get true.
      /// </summary>
      public Boolean Commnicating
      {
         get { return this.zCommnicating; }
      }

      /// <summary>
      /// Open connection to a server.
      /// </summary>
      public Pop3ConnectionState Open()
      {
         this.zTcpClient = this.GetTcpClient();
         if (this.zTcpClient == null)
         {
            this.zState = Pop3ConnectionState.Disconnected;
         }
         else
         {
            if (this.zSsl == true)
            {
               SslStream ssl = new SslStream(this.zTcpClient.GetStream(), false, Pop3Client.RemoteCertificateValidationCallback);
               ssl.AuthenticateAsClient(this.zServerName);
               if (ssl.IsAuthenticated == false)
               {
                  this.zState = Pop3ConnectionState.Disconnected;
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
            if (this.GetResponse().StartsWith("+OK") == true)
            {
               this.zState = Pop3ConnectionState.Connected;
            }
            else
            {
               this.zTcpClient = null;
               this.zState = Pop3ConnectionState.Disconnected;
            }
         }
         return this.zState;
      }

      /// <summary>
      /// Ensure connection is opened.
      /// </summary>
      public Pop3ConnectionState EnsureOpen()
      {
         if (this.zTcpClient != null)
         { return this.zState; }

         return this.Open();
      }

      /// <summary>
      /// Log in to pop3 server.
      /// </summary>
      public Boolean Authenticate()
      {
         if (this.zMode == Pop3AuthenticateMode.Auto)
         {
            if (this.AuthenticateByPop() == true)
            {
               this.zMode = Pop3AuthenticateMode.Pop;
               return true;
            }
            else if (this.AuthenticateByAPop() == true)
            {
               this.zMode = Pop3AuthenticateMode.APop;
               return true;
            }
            return false;
         }
         else
         {
            switch (this.zMode)
            {
               case Pop3AuthenticateMode.Pop: return this.AuthenticateByPop();
               case Pop3AuthenticateMode.APop: return this.AuthenticateByAPop();
            }
         }
         return false;
      }

      /// <summary>
      /// Log in to pop3 server by POP authenticate.
      /// </summary>
      public Boolean AuthenticateByPop()
      {
         String s = string.Empty;

         if (this.EnsureOpen() == Pop3ConnectionState.Connected)
         {
            s = this.Execute("user " + this.zUserName, false);
            if (s.StartsWith("+OK") == true)
            {
               s = this.Execute("pass " + this.zPassword, false);
               if (s.StartsWith("+OK") == true)
               {
                  this.zState = Pop3ConnectionState.Authenticated;
               }
            }
         }
         return this.zState == Pop3ConnectionState.Authenticated;
      }

      /// <summary>
      /// Log in to pop3 server by A-POP authenticate.
      /// </summary>
      public Boolean AuthenticateByAPop()
      {
         String s = string.Empty;
         String TimeStamp = string.Empty;
         Int32 StartIndex = 0;
         Int32 EndIndex = 0;

         if (this.EnsureOpen() == Pop3ConnectionState.Connected)
         {
            s = this.Execute("user " + this.zUserName, false);
            if (s.StartsWith("+OK") == true)
            {
               if (s.IndexOf("<") > -1 &&
                  s.IndexOf(">") > -1)
               {
                  StartIndex = s.IndexOf("<");
                  EndIndex = s.IndexOf(">");
                  TimeStamp = s.Substring(StartIndex, EndIndex - StartIndex + 1);

                  s = this.Execute("pass " + MailParser.ToMd5DigestString(TimeStamp + this.zPassword), false);
                  if (s.StartsWith("+OK") == true)
                  {
                     this.zState = Pop3ConnectionState.Authenticated;
                  }
               }
            }
         }
         return this.zState == Pop3ConnectionState.Authenticated;
      }

      /// <summary>
      /// Send a command with synchronous and get response data as string text if the command is a type to get response.
      /// </summary>
      public String Execute(Pop3Command inCommand)
      {
         Boolean IsResponseMultiLine = false;

         if (inCommand is Top ||
            inCommand is Retr ||
            inCommand is List ||
            inCommand is Uidl)
         {
            IsResponseMultiLine = true;
         }
         return this.Execute(inCommand.GetCommandString(), IsResponseMultiLine);
      }

      /// <summary>
      /// Send a command with asynchronous and get response text by first parameter of inFunction.
      /// </summary>
      public void BeginExecute(Pop3Command inCommand, EndGetResponse inFunction)
      {
         AsynchronousContext cx = new AsynchronousContext(this.zReceiveBufferSize, MailParser.IsReceiveCompleted, inFunction);
         this.SendCommand(inCommand.GetCommandString());
         this.zStream.BeginRead(cx.Data, 0, cx.Data.Length, this.BeginExecuteCallBack, cx);
      }
      
      /// <summary>
      /// Send list command to pop3 server.
      /// </summary>
      public List.Result[] ExecuteList(List inCommand)
      {
         List.Result[] rs = null;
         if (inCommand.MailIndex.HasValue == true)
         {
            rs = new List.Result[1];
            rs[0] = this.ExecuteList(inCommand.MailIndex.Value);
         }
         else
         {
            rs = this.ExecuteList();
         }
         return rs;
      }

      /// <summary>
      /// Send list command to pop3 server.
      /// </summary>
      public List.Result ExecuteList(Int64 inMailIndex)
      {
         List cm = new List(inMailIndex);
         List.Result rs = null;
         String s = string.Empty;

         this.EnsureOpen();
         s = this.Execute(cm);
         rs = new List.Result(s);
         return rs;
      }

      /// <summary>
      /// Send list command to pop3 server.
      /// </summary>
      public List.Result[] ExecuteList()
      {
         List cm = new List();
         List.Result[] rs = null;
         String[] ss = null;
         String s = string.Empty;

         this.EnsureOpen();
         s = this.Execute(cm);

         ss = s.Split(new String[] { MailParser.NewLine }, StringSplitOptions.None);
         rs = new List.Result[ss.Length];
         for (int i = 0; i < ss.Length; i++)
         {
            if (ss[i] == ".")
            { break; }

            rs[i] = new List.Result(ss[i]);
         }
         return rs;
      }

      /// <summary>
      /// Send uidl command to pop3 server.
      /// </summary>
      public Uidl.Result ExecuteUidl(Int64 inMailIndex)
      {
         Uidl cm = new Uidl(inMailIndex);
         Uidl.Result rs = null;
         String s = string.Empty;

         this.EnsureOpen();
         s = this.Execute(cm);
         if (Uidl.Result.CheckFormat(s) == true)
         {
            rs = new Uidl.Result(s);
         }
         return rs;
      }

      /// <summary>
      /// Send uidl command to pop3 server.
      /// </summary>
      public Uidl.Result[] ExecuteUidl()
      {
         Uidl cm = new Uidl();
         List<Uidl.Result> l = new List<Uidl.Result>();
         String[] ss = null;
         String s = string.Empty;

         this.EnsureOpen();
         s = this.Execute(cm);

         ss = s.Split(new String[] { MailParser.NewLine }, StringSplitOptions.None);
         for (int i = 0; i < ss.Length; i++)
         {
            if (ss[i] == ".")
            { break; }
            if (ss[i].ToLower().StartsWith("+ok") == true)
            { continue; }
            if (Uidl.Result.CheckFormat(ss[i]) == false)
            { continue; }

            l.Add(new Uidl.Result(ss[i]));
         }
         return l.ToArray();
      }

      /// <summary>
      /// Send retr command to pop3 server.
      /// </summary>
      public Pop3Message ExecuteRetr(Int64 inMailIndex)
      {
         return this.GetMessage(inMailIndex, Int32.MaxValue);
      }

      /// <summary>
      /// Send top command to pop3 server.
      /// </summary>
      public Pop3Message ExecuteTop(Int64 inMailIndex, Int32 inLineCount)
      {
         return this.GetMessage(inMailIndex, inLineCount);
      }

      /// <summary>
      /// Send dele command to pop3 server.
      /// </summary>
      public Pop3CommandResult ExecuteDele(Int64 inMailIndex)
      {
         Dele cm = new Dele(inMailIndex);
         Pop3CommandResult rs = null;
         String s = string.Empty;

         this.EnsureOpen();
         s = this.Execute(cm);
         rs = new Pop3CommandResult(s);
         return rs;
      }

      /// <summary>
      /// Send stat command to pop3 server.
      /// </summary>
      public Stat.Result ExecuteStat()
      {
         Stat cm = new Stat();
         Stat.Result rs = null;
         String s = string.Empty;

         this.EnsureOpen();
         s = this.Execute(cm);
         rs = new Stat.Result(s);
         return rs;
      }

      /// <summary>
      /// Send noop command to pop3 server.
      /// </summary>
      public Pop3CommandResult ExecuteNoop()
      {
         Noop cm = new Noop();
         Pop3CommandResult rs = null;
         String s = string.Empty;

         this.EnsureOpen();
         s = this.Execute(cm);
         rs = new Pop3CommandResult(s);
         return rs;
      }

      /// <summary>
      /// Send reset command to pop3 server.
      /// </summary>
      public Pop3CommandResult ExecuteReset()
      {
         Reset cm = new Reset();
         Pop3CommandResult rs = null;
         String s = string.Empty;

         this.EnsureOpen();
         s = this.Execute(cm);
         rs = new Pop3CommandResult(s);
         return rs;
      }

      /// <summary>
      /// Send quit command to pop3 server.
      /// </summary>
      public Pop3CommandResult ExecuteQuit()
      {
         Quit cm = new Quit();
         Pop3CommandResult rs = null;
         String s = string.Empty;

         this.EnsureOpen();
         s = this.Execute(cm);
         rs = new Pop3CommandResult(s);
         return rs;
      }

      /// <summary>
      /// Get total mail count at mailbox.
      /// </summary>
      public Int64 GetTotalMessageCount()
      {
         Stat.Result rs = null;
         rs = this.ExecuteStat();
         return rs.TotalMessageCount;
      }

      /// <summary>
      /// Get mail data of specified mail index.
      /// </summary>
      public Pop3Message GetMessage(Int64 inMailIndex)
      {
         Pop3Message pm = null;
         Retr cm = null;

         try
         {
            this.EnsureOpen();
            cm = new Retr(inMailIndex);
            String MailData = this.Execute(cm);
            pm = new Pop3Message(MailData, inMailIndex);
         }
         catch (SocketException)
         {
            return pm;
         }
         catch (Exception ex1)
         {
            throw new Pop3ReceiveException(ex1.ToString());
         }
         return pm;
      }

      /// <summary>
      /// Get mail data of specified mail index with indicate body line count.
      /// </summary>
      public Pop3Message GetMessage(Int64 inMailIndex, Int32 inLineCount)
      {
         Pop3Message pm = null;
         Top cm = null;

         this.EnsureOpen();
         try
         {
            cm = new Top(inMailIndex, inLineCount);
            String MailData = this.Execute(cm);
            pm = new Pop3Message(MailData, inMailIndex);
         }
         catch (SocketException)
         {
            return pm;
         }
         catch (Exception ex1)
         {
            throw new Pop3ReceiveException(ex1.ToString());
         }
         return pm;
      }

      /// <summary>
      /// Get mail text of specified mail index.
      /// </summary>
      public String GetMessageText(Int64 inMailIndex)
      {
         Retr cm = null;

         try
         {
            this.EnsureOpen();
            cm = new Retr(inMailIndex);
            return this.Execute(cm);
         }
         catch (SocketException)
         {
            return string.Empty;
         }
         catch (Exception ex1)
         {
            throw new Pop3ReceiveException(ex1.ToString());
         }

         // return string.Empty;
      }

      /// <summary>
      /// Get mail text of specified mail index with indicate body line count.
      /// </summary>
      public String GetMessageText(Int64 inMailIndex, Int32 inLineCount)
      {
         Top cm = null;

         this.EnsureOpen();
         try
         {
            cm = new Top(inMailIndex, inLineCount);
            return this.Execute(cm);
         }
         catch (SocketException)
         {
            return string.Empty;
         }
         catch (Exception ex1)
         {
            throw new Pop3ReceiveException(ex1.ToString());
         }

         // return string.Empty;
      }

      /// <summary>
      /// Get mail text of specified mail index by asynchronous request.
      /// </summary>
      public void GetMessageText(Int64 inMailIndex, EndGetResponse inFunction)
      {
         Retr cm = null;
         EndGetResponse md = inFunction;

         this.EnsureOpen();
         cm = new Retr(inMailIndex);
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Set delete flag to specify mail index.
      /// To complete delete execution,call quit command after calling dele command.
      /// </summary>
      public Boolean DeleteEMail(Int64 inMailIndex)
      {
         Dele cm = new Dele(inMailIndex);
         String s = string.Empty;

         s = this.Execute(cm);
         return MailParser.IsResponseOk(s);
      }

      /// <summary>
      /// Send asynchronous list command to pop3 server.
      /// </summary>
      public void ExecuteList(Int64 inMailIndex, List.Callback inFunction)
      {
         List cm = new List(inMailIndex);
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            List.Result[] rs = new List.Result[1];
            rs[0] = new List.Result(inResponseString);
            inFunction(rs);
         });
         this.EnsureOpen();
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Send asynchronous list command to pop3 server.
      /// </summary>
      public void ExecuteList(List.Callback inFunction)
      {
         List cm = new List();
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            List<List.Result> l = new List<List.Result>();
            String[] ss = null;
            String s = string.Empty;

            ss = s.Split(new String[] { MailParser.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < ss.Length; i++)
            {
               if (ss[i] == ".")
               { break; }

               l.Add(new List.Result(ss[i]));
            }
            inFunction(l.ToArray());
         });
         this.EnsureOpen();
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Send asynchronous uidl command to pop3 server.
      /// </summary>
      public void ExecuteUidl(Int64 inMailIndex, Uidl.Callback inFunction)
      {
         Uidl cm = new Uidl(inMailIndex);
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            Uidl.Result[] rs = new Uidl.Result[1];
            rs[0] = new Uidl.Result(inResponseString);
            inFunction(rs);
         });
         this.EnsureOpen();
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Send asynchronous uidl command to pop3 server.
      /// </summary>
      public void ExecuteUidl(Uidl.Callback inFunction)
      {
         Uidl cm = new Uidl();
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            List<Uidl.Result> l = new List<Uidl.Result>();
            String[] ss = null;
            String s = string.Empty;

            ss = s.Split(new String[] { MailParser.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < ss.Length; i++)
            {
               if (ss[i] == ".")
               { break; }
               if (ss[i].ToLower().StartsWith("+ok") == true)
               { continue; }
               if (Uidl.Result.CheckFormat(ss[i]) == false)
               { continue; }

               l.Add(new Uidl.Result(ss[i]));
            }
            inFunction(l.ToArray());
         });
         this.EnsureOpen();
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Get mail data by asynchronous request.
      /// </summary>
      public void GetMessage(Int64 inMailIndex, Retr.Callback inFunction)
      {
         Retr cm = null;
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            Pop3Message pm = null;
            pm = new Pop3Message(inResponseString, inMailIndex);
            inFunction(pm);
         });
         this.EnsureOpen();
         cm = new Retr(inMailIndex);
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Send asynchronous retr command to pop3 server.
      /// </summary>
      public void ExecuteRetr(Int64 inMailIndex, Retr.Callback inFunction)
      {
         Retr cm = null;
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            Pop3Message pm = null;
            pm = new Pop3Message(inResponseString, inMailIndex);
            inFunction(pm);
         });
         this.EnsureOpen();
         cm = new Retr(inMailIndex);
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Send asynchronous stat command to pop3 server.
      /// </summary>
      public void ExecuteStat(Stat.Callback inFunction)
      {
         Stat cm = null;
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            Stat.Result rs = new Stat.Result(inResponseString);
            inFunction(rs);
         });
         this.EnsureOpen();
         cm = new Stat();
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Send asynchronous noop command to pop3 server.
      /// </summary>
      public void ExecuteNoop(Pop3CommandCallback inFunction)
      {
         Noop cm = null;
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            Pop3CommandResult rs = new Pop3CommandResult(inResponseString);
            inFunction(rs);
         });
         this.EnsureOpen();
         cm = new Noop();
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Send asynchronous reset command to pop3 server.
      /// </summary>
      public void ExecuteReset(Pop3CommandCallback inFunction)
      {
         Reset cm = null;
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            Pop3CommandResult rs = new Pop3CommandResult(inResponseString);
            inFunction(rs);
         });
         this.EnsureOpen();
         cm = new Reset();
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// Send asynchronous quit command to pop3 server.
      /// </summary>
      public void ExecuteQuit(Pop3CommandCallback inFunction)
      {
         Quit cm = null;
         EndGetResponse md = null;

         md = new EndGetResponse(delegate(String inResponseString)
         {
            Pop3CommandResult rs = new Pop3CommandResult(inResponseString);
            inFunction(rs);
         });
         this.EnsureOpen();
         cm = new Quit();
         this.BeginExecute(cm, md);
      }

      /// <summary>
      /// disconnect connection to pop3 server.
      /// </summary>
      public void Close()
      {
         this.SendCommand("quit");
         this.zState = Pop3ConnectionState.Disconnected;
         this.Dispose();
      }

      /// <summary>
      /// Get string about mail account information.
      /// </summary>
      public override string ToString()
      {
         StringBuilder sb = new StringBuilder();

         sb.AppendFormat("UserName:{0}", this.UserName);
         sb.AppendLine();
         sb.AppendFormat("ServerName:{0}", this.ServerName);
         sb.AppendLine();
         sb.AppendFormat("Port:{0}", this.Port);
         sb.AppendLine();
         sb.AppendFormat("SSL:{0}", this.Ssl);

         return sb.ToString();
      }

      /// <summary>
      /// Dipose and release system resoures.
      /// </summary>
      public void Dispose()
      {
         GC.SuppressFinalize(this);
         this.Dispose(true);
      }

      /// <summary>
      /// Dipose and release system resoures.
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
      }

      /// <summary>
      /// Llama al método Dispose en el caso que no se haya llamado a Dispose al destruir el objeto.
      /// </summary>
      ~Pop3Client()
      {
         this.Dispose(false);
      }

      #region Private members

      /// <summary>
      /// Get TcpClient object to communicate to server.
      /// </summary>
      private TcpClient GetTcpClient()
      {
         TcpClient tc = null;
         IPHostEntry hostEntry = null;

         try
         {
            hostEntry = Dns.GetHostEntry(this.zServerName);
            foreach (IPAddress address in hostEntry.AddressList)
            {
               IPEndPoint ipe = new IPEndPoint(address, this.zPort);

               tc = new TcpClient(ipe.AddressFamily);
               tc.Connect(ipe);
               if (tc.Connected == true)
               {
                  tc.ReceiveTimeout = this.zReceiveTimeout;
                  tc.ReceiveBufferSize = this.zReceiveBufferSize;
                  return tc;
               }
            }
         }
         catch
         {
            tc = null;
         }
         if (tc == null)
         {
            this.zState = Pop3ConnectionState.Disconnected;
            return null;
         }
         this.zState = Pop3ConnectionState.Connected;
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
            throw new Pop3ConnectException("Pop3 connection is closed");
         }
         try
         {
            Byte[] bb = Encoding.ASCII.GetBytes(inCommand + MailParser.NewLine);
            this.zStream.Write(bb, 0, bb.Length);
            this.zStream.Flush();
         }
         catch (Exception e)
         {
            throw new Pop3SendException(e.ToString());
         }
      }

      private String GetResponse()
      {
         return this.GetResponse(false);
      }

      private String GetResponse(Boolean inIsMultiLine)
      {
         if (this.zTcpClient == null)
         {
            throw new Pop3ConnectException("Connection to POP3 server is closed");
         }
         DateTime dtime = DateTime.Now;
         TimeSpan ts;
         StringBuilder sb = new StringBuilder();
         String CurrentLine = string.Empty;
         Int64 LineIndex = 0;

         while (true)
         {
            ts = DateTime.Now - dtime;
            if (ts.TotalMilliseconds > this.zReceiveTimeout)
            {
               throw new Pop3ReceiveException("Response timeout");
            }
            
            this.ThreadSleep(this.zThreadSleepMilliseconds);
            CurrentLine = this.zReader.ReadLine();
            sb.AppendLine(CurrentLine);

            LineIndex += 1;
            if (inIsMultiLine == false)
            { 
               break; 
            }
            
            if (CurrentLine == ".")
            { 
               break; 
            }
         }
         this.zCommnicating = false;
         return sb.ToString();
      }

      private void ThreadSleep(Int32 inMilliseconds)
      {
         while (this.zReader.BaseStream.CanRead == false)
         {
            Thread.Sleep(inMilliseconds);
         }
      }

      /// <summary>
      /// Send a command with synchronous and get response data as string text if the command is a type to get response.
      /// </summary>
      private String Execute(String inCommand, Boolean inIsMultiLine)
      {
         this.SendCommand(inCommand);
         this.zCommnicating = true;
         return this.GetResponse(inIsMultiLine);
      }

      /// <summary>
      /// Send a command with asynchronous and get response text by first parameter of inFunction.
      /// If there is more data to receive,continously call BeginExecuteCallback method and get response data.
      /// </summary>
      private void BeginExecuteCallBack(IAsyncResult inResult)
      {
         AsynchronousContext cx = inResult.AsyncState as AsynchronousContext;

         this.ThreadSleep(this.zThreadSleepMilliseconds);
         Int32 x = this.zStream.EndRead(inResult);
         if (cx.Parse(x) == false)
         {
            this.zStream.BeginRead(cx.Data, 0, cx.Data.Length, this.BeginExecuteCallBack, cx);
         }
      }

      #endregion
   }

}