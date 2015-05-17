using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Cosmo.Net.Fax
{

   #region Enumerations

   internal enum FaxJobCommand
   {
      Unknown = 0,
      Delete,
      Pause,
      Resume
   }

   #endregion

   /// <summary>
   /// Represent a connection to a fax server.
   /// For sending a fax you should do :
   /// <list type="number">
   /// <item>Check if Windows Fax Service is installed (<see cref="IsFaxServiceInstalled"/>)</item>
   /// <item>Connect to a fax server with the fax _printer (by example : <see cref="FaxServer(String, PrinterSettings)"/>)</item>
   /// <item>Call <see cref="GetDefaultInformations()"/> for create and fill a <see cref="FaxInformations"/> class, with default value from fax server.</item>
   /// <item>Use one construction of Send methode for sending a <see cref="PrintDocument"/> or a file. (By exemple <see cref="Send(FaxInformations, PrintDocument)"/></item>
   /// <item>Get the <see cref="FaxJob"/> object for having informations about that.</item>
   /// <item>Call <see cref="RefreshJobs"/> for updating the fax _jobs data informations.</item>
   /// <item>Dispose fax server object (<see cref="Dispose()"/>).</item>
   /// </list>
   /// 
   /// <example>For sending a fax with a <see cref="PrintDocument"/> :
   /// <code>
   /// 
   /// //Connect to FaxServer
   /// using (FaxServer server = new FaxServer("Beer-Server", "Fax"))
   /// {
   ///     FaxInformations info;
   ///     FaxJob job;
   ///     info = server.GetDefaultInformations();  //Get default data informations on Windows Fax Service
   /// 
   ///     info.RecipientName = "Gilles TOURREAU"; //Fill the recipient name
   ///     info.RecipientNumber = "+33116641664";  //Fill the recipient number (1664 is good french beer !!!)
   /// 
   ///     job = server.Send(info, @"C:\French\Beer\1664.jpg");
   ///     
   ///     Console.WriteLine("Current status : " + job.Status);
   /// }
   /// 
   /// //Here, all fax ressources are free !
   /// </code>
   /// </example>
   /// </summary>
   public class FaxServer : IDisposable
   {
      internal IntPtr _faxHandle;

      private string _serverName;
      private PrinterSettings _printer;
      private FaxJobCollection _jobs;
      private FaxPortCollection _ports;
      private int _retriesNumber;
      private int _retryDelay;
      private string _archiveDirectory;
      private bool _archiveOutgoingFaxes;
      private TimeSpan _startCheapTime;
      private TimeSpan _stopCheapTime;

      /// <summary>
      /// Connect to the Fax Server.
      /// </summary>
      /// <param name="serverName">Name of fax server (You can put null or empty string for localhost).</param>
      /// <param name="printerName">Fax Printer name, where documents can be printed</param>
      /// <exception cref="System.ArgumentException">If the <paramref name="printerName"/> is incorrect.</exception>
      public FaxServer(string serverName, string printerName) : this(serverName, GetPrinter(printerName)) { }

      /// <summary>
      /// Connect to the Fax Server.
      /// </summary>
      /// <param name="serverName">Name of fax server (You can put null or empty string for localhost).</param>
      /// <param name="printer">Fax Printer where documents can be printed</param>
      /// <exception cref="ArgumentNullException">If <paramref name="printer"/> is null.</exception>
      /// <exception cref="ArgumentException">If <paramref name="printer"/> is an invalid printer.</exception>
      /// <exception cref="FaxException">If Windows Fax Service is not installed on the current system.</exception>
      public FaxServer(string serverName, PrinterSettings printer)
      {
         if (printer == null)
            throw new ArgumentNullException("printer");

         if (printer.IsValid == false)
            throw new ArgumentException(string.Format(FaxResources.ExceptionPrinterIncorrect, printer.PrinterName), "printer");

         if (IsFaxServiceInstalled == false)
            throw new FaxException(FaxResources.ExceptionFaxServiceNotInstalled);

         string temp;

         if (_serverName == null)
            temp = "localhost";
         else
            temp = _serverName;

         IntPtr ptr;

         ptr = Connect(serverName);
         if (ptr == IntPtr.Zero)
            throw new FaxException(string.Format(FaxResources.ExceptionUnableConnect, temp));

         // Fill members
         _faxHandle = ptr;
         _serverName = temp;
         _printer = printer;

         // Get configuration, jobs and ports device...
         _jobs = new FaxJobCollection();
         _ports = new FaxPortCollection();

         this.RefreshConfiguration();
         this.RefreshJobs();
         this.RefreshPorts();
      }

      #region Properties

      /// <summary>
      /// Get the server name.
      /// </summary>
      public string ServerName
      {
         get { return _serverName; }
      }

      /// <summary>
      /// Get the fax printer name.
      /// </summary>
      public string PrinterName
      {
         get { return _printer.PrinterName; }
      }

      /// <summary>
      /// Get the read-only collection of job where are in the fax server.
      /// </summary>
      public FaxJobCollection Jobs
      {
         get { return _jobs; }
      }

      /// <summary>
      /// Get or set the number of times the fax server will attempt to retransmit an outgoing fax il the initial transmission fails.
      /// </summary>
      public int RetriesNumber
      {
         get { return _retriesNumber; }
         set
         {
            if (this._retriesNumber != value)
            {
               NativeMethods.FAX_CONFIGURATION configuration;

               configuration = this.GetConfiguration(true);
               configuration.Retries = Convert.ToUInt32(value);

               this.UpdateConfiguration(configuration);
            }
         }
      }

      /// <summary>
      /// Get or set the number of minutes that will elapse between retransmission attempts by the fax server.
      /// </summary>
      public int RetryDelay
      {
         get { return _retryDelay; }
         set
         {
            if (this._retryDelay != value)
            {
               NativeMethods.FAX_CONFIGURATION configuration;

               configuration = this.GetConfiguration(true);
               configuration.RetryDelay = Convert.ToUInt32(value);

               this.UpdateConfiguration(configuration);
            }
         }
      }

      /// <summary>
      /// Get or set the hour and minute at which the discount period begins.
      /// </summary>
      /// <remarks>The discount period applies only to outgoing transmissions. Seconds and millisecondes members are ignored</remarks>
      public TimeSpan StartCheapTime
      {
         get { return _startCheapTime; }
         set
         {
            if (_startCheapTime != value)
            {
               NativeMethods.FAX_CONFIGURATION configuration;

               configuration = this.GetConfiguration(true);
               NativeMethods.TimeSpanToFaxTime(value, configuration.StartCheapTime);

               this.UpdateConfiguration(configuration);
            }
         }
      }

      /// <summary>
      /// Get or set the hour and minute at which the discount period ends.
      /// </summary>
      /// <remarks>The discount period applies only to outgoing transmissions.Seconds and millisecondes members are ignored</remarks>
      public TimeSpan StopCheapTime
      {
         get { return _stopCheapTime; }
         set
         {
            if (_stopCheapTime != value)
            {
               NativeMethods.FAX_CONFIGURATION configuration;

               configuration = this.GetConfiguration(true);
               NativeMethods.TimeSpanToFaxTime(value, configuration.StopCheapTime);

               this.UpdateConfiguration(configuration);
            }
         }
      }

      /// <summary>
      /// Get or set the fully qualified path of the directory in which outgoing fax transmissions will be archived.
      /// </summary>
      public string ArchiveDirectory
      {
         get { return _archiveDirectory; }
         set
         {
            if (_archiveDirectory != value)
            {
               NativeMethods.FAX_CONFIGURATION configuration;

               configuration = this.GetConfiguration(true);
               configuration.ArchiveDirectory = value;

               this.UpdateConfiguration(configuration);
            }
         }
      }

      /// <summary>
      /// Get or set if the fax server should archive outgoing fax transmissions.
      /// </summary>
      public bool ArchiveOutgoingFaxes
      {
         get { return _archiveOutgoingFaxes; }
         set
         {
            if (_archiveOutgoingFaxes != value)
            {
               NativeMethods.FAX_CONFIGURATION configuration;

               configuration = this.GetConfiguration(true);
               configuration.ArchiveOutgoingFaxes = Convert.ToInt32(value);

               this.UpdateConfiguration(configuration);
            }
         }
      }

      /// <summary>
      /// Get a read-only collection of device port in the fax server.
      /// </summary>
      public FaxPortCollection Ports
      {
         get { return _ports; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Send only a cover page by fax.
      /// </summary>
      /// <param name="informations">Data informations of fax document.</param>
      /// <returns>The new fax job create</returns>
      public FaxJob Send(FaxInformations informations)
      {
         return this.Send(informations, (PrintDocument)null);
      }

      /// <summary>
      /// Print a document to fax _printer and send this with a cover page.
      /// </summary>
      /// <param name="informations">Data informations of fax document</param>
      /// <param name="document">Document to be send</param>
      /// <returns>The new fax job create</returns>
      public FaxJob Send(FaxInformations informations, PrintDocument document)
      {
         if (informations == null)
            throw new ArgumentNullException("informations");

         if (CheckIsDisposed() == true)
            throw new FaxException(FaxResources.ExceptionFaxServerDisposed);

         NativeMethods.FAX_PRINT_INFO faxInfo;
         IntPtr ptrContext;
         uint spoolId;       //BE CAREFUL ! It is NOT an ID job !
         FaxController controller;
         NativeMethods.FAX_CONTEXT_INFO context;
         FaxJob[] jobs;

         //Convert data informations...
         faxInfo = informations.CreateFaxPrintInfoStructure();

         //Start print job and get DC
         if (NativeMethods.FaxStartPrintJob(_printer.PrinterName, faxInfo, out spoolId, out ptrContext) == false)
            throw FaxTools.CreateFaxException(FaxResources.ExceptionPrintingFail);


         //Get FAX_CONTEXT_INFO structure an free in non-managed memory
         context = new NativeMethods.FAX_CONTEXT_INFO();
         Marshal.PtrToStructure(ptrContext, context);
         try
         {
            NativeMethods.FaxFreeBuffer(ptrContext);

            // Imprime la cubierta del FAX (si se ha especificado y no es nulo)
            if (informations.CoverPage != null)
            {
               if (NativeMethods.FaxPrintCoverPage(context, informations.CoverPage.CreateCoverPageStructure()) == false)
                  throw new FaxException(string.Empty);
            }

            if (document != null)
            {
               //Create controller with the DC
               controller = new FaxController(context.hDC);

               //Launch print process...
               document.PrintController = controller;
               document.Print();
            }
            else
            {
               NativeMethods.EndDoc(context.hDC);
            }

            //Get last user job
            this.RefreshJobs();
            jobs = this.Jobs.GetJobsUser();

            return jobs[jobs.Length - 1];
         }
         finally
         {
            NativeMethods.DeleteDC(context.hDC);
         }
      }

      /// <summary>
      /// Send file by fax with cover page.
      /// </summary>
      /// <param name="informations">Data that contains the information necessary for the fax server to send the fax transmission.</param>
      /// <param name="fileName">Path of file name to be send</param>
      /// <returns>The fax job create</returns>
      /// <exception cref="ArgumentNullException">If <paramref name="informations"/> is null.</exception>
      /// <exception cref="FileNotFoundException">If <paramref name="fileName"/> could not be found.</exception>
      /// <remarks>The file name mus be printable with Windows.</remarks>
      public FaxJob Send(FaxInformations informations, string fileName)
      {
         NativeMethods.FAX_JOB_PARAM parameters;
         NativeMethods.FAX_COVERPAGE_INFO strCoverPage;
         uint jobID;

         // Comprobaciones
         if (informations == null)
            throw new ArgumentNullException("informations");

         if (FaxTools.IsNullOrEmpty(informations.RecipientNumber) == true)
            throw new ArgumentException(FaxResources.ExceptionNoRecipientNumber, "informations");

         if (File.Exists(fileName) == false)
            throw new FileNotFoundException(string.Empty, fileName);

         if (CheckIsDisposed() == true)
            throw new FaxException(FaxResources.ExceptionFaxServerDisposed);

         // Si existe portada se agrega
         if (informations.CoverPage != null)
            strCoverPage = informations.CoverPage.CreateCoverPageStructure();
         else
            strCoverPage = null;

         parameters = informations.CreateFaxJobParamStructure();

         if (NativeMethods.FaxSendDocument(_faxHandle, fileName, parameters, strCoverPage, out jobID) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         // Refresca los trabajos (jobs)
         this.RefreshJobs();

         return this.Jobs.GetJob(jobID);
      }

      /// <summary>
      /// Send a file name for multiple recipients.
      /// </summary>
      /// <param name="informations">Array of multiple recipients</param>
      /// <param name="fileName">Name of file to be send.</param>
      /// <returns>Array of fax _jobs created</returns>
      /// <exception cref="ArgumentNullException">If <paramref name="informations"/> is null.</exception>
      /// <exception cref="ArgumentNullException">If <paramref name="fileName"/> is null.</exception>
      /// <exception cref="FileNotFoundException">If the file is not found.</exception>
      public FaxJob[] Send(FaxInformations[] informations, string fileName)
      {
         if (informations == null)
            throw new ArgumentNullException("informations");

         FaxJob[] faxJobs;

         faxJobs = new FaxJob[informations.Length];

         for (int i = 0; i < informations.Length; i++)
            faxJobs[i] = this.Send(informations[i], fileName);

         return faxJobs;
      }

      /// <summary>
      /// Send a <see cref="PrintDocument"/> for multiple recipients.
      /// </summary>
      /// <param name="informations">Array of multiple recipients</param>
      /// <param name="document">Document to be print.</param>
      /// <returns>Array of fax _jobs created</returns>
      /// <remarks><paramref name="document"/> will be print for each recipient in <paramref name="informations"/> array.</remarks>
      /// <exception cref="ArgumentNullException">If <paramref name="informations"/> is null.</exception>
      /// <exception cref="ArgumentNullException">If <paramref name="document"/> is null.</exception>
      public FaxJob[] Send(FaxInformations[] informations, PrintDocument document)
      {
         if (informations == null)
            throw new ArgumentNullException("informations");

         FaxJob[] faxJobs;

         faxJobs = new FaxJob[informations.Length];

         for (int i = 0; i < informations.Length; i++)
            faxJobs[i] = this.Send(informations[i], document);

         return faxJobs;
      }

      /// <summary>
      /// Get default data informations configuring on the fax server.
      /// </summary>
      /// <returns>The default data informations configurating on the fax server.</returns>
      public FaxInformations GetDefaultInformations()
      {
         FaxInformations informations;
         FaxCoverPage coverPage;

         informations = new FaxInformations();
         coverPage = new FaxCoverPage();

         this.FillDefaultInformations(informations, coverPage);

         informations.CoverPage = coverPage;

         return informations;
      }

      /// <summary>
      /// Get default data informations on the fax server, and fill it on <paramref name="informations"/> parameters.
      /// </summary>
      /// <param name="informations">Informations which will be fill.</param>
      /// <exception cref="ArgumentNullException">If <paramref name="informations"/> is null.</exception>
      public void FillDefaultInformations(FaxInformations informations)
      {
         if (informations == null)
            throw new ArgumentNullException("informations");

         this.FillDefaultInformations(informations, null);
      }

      /// <summary>
      /// Get default cover page data informations on the fax server, and fill it on <paramref name="coverPage"/> parameters.
      /// </summary>
      /// <param name="coverPage">Cover page which will be fill.</param>
      /// <exception cref="ArgumentNullException">If <paramref name="coverPage"/> is null.</exception>
      public void FillDefaultInformations(FaxCoverPage coverPage)
      {
         if (coverPage == null)
            throw new ArgumentNullException("coverPage");

         this.FillDefaultInformations(null, coverPage);
      }

      /// <summary>
      /// Refresh _jobs. This methode do automaticly :
      /// <list type="bullet">
      /// <item>Update status of existing _jobs</item>
      /// <item>Delete finished _jobs</item>
      /// <item>Add new _jobs</item>
      /// </list>
      /// </summary>
      public void RefreshJobs()
      {
         IntPtr jobs;
         uint count;
         List<uint> currentJobs;
         IntPtr ptrJob;
         NativeMethods.FAX_JOB_ENTRY jobEntry;
         FaxJob job;

         if (this.CheckIsDisposed() == true)
            throw new FaxException(FaxResources.ExceptionFaxServerDisposed);

         if (NativeMethods.FaxEnumJobs(this._faxHandle, out jobs, out count) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         //Get all current _jobs ID in the collection
         //This list is using for knowing which job will be remove
         currentJobs = this.GetJobsID();

         //Read all _jobs structures...
         ptrJob = jobs;
         for (int i = 0; i < count; i++)
         {
            jobEntry = GetJob(ptrJob);

            //We have a JobEntry, find it in the current collection :
            job = this.Jobs.GetJob(jobEntry.JobId);
            if (job == null)
            {
               //The job doesn't exist in the collection, so create it !
               this.Jobs.Add(new FaxJob(this, jobEntry));
            }
            else
            {
               //The job exist, so refresh and remove it on the list of current _jobs id
               job.Refresh(jobEntry);
               currentJobs.Remove(jobEntry.JobId);
            }

            //Next structure...
            ptrJob = (IntPtr)((int)ptrJob + Marshal.SizeOf(typeof(NativeMethods.FAX_JOB_ENTRY)));
         }

         //Free array...
         NativeMethods.FaxFreeBuffer(jobs);

         //Now remove job in CurrentJobs
         foreach (uint id in currentJobs)
            this.Jobs.Remove(id);
      }

      /// <summary>
      /// Pause the <paramref name="job"/>. If the fax job is active, the fax service pauses the job when it returns to the queued state.
      /// </summary>
      /// <param name="job">Job to pause.</param>
      /// <exception cref="ArgumentNullException">If <paramref name="job"/> parameter is null.</exception>
      /// <exception cref="ArgumentException">If <paramref name="job"/> is not on the <see cref="FaxServer"/> instance.</exception>
      /// <example>
      /// <code>
      /// //Get the first job on the server
      /// FaxJob job = this.server.Jobs[0];
      /// 
      /// //Pause it !
      /// this.server.Pause(job);
      /// 
      /// //Going to bed for 1 second
      /// Thread.Sleep(1000);
      /// 
      /// //Resume it !
      /// this.server.Resume(job);
      /// 
      /// //Pause and restart it !
      /// this.server.Pause(job);
      /// this.server.Restart(job);
      /// 
      /// //Pause and delete it !
      /// this.server.Pause(job);
      /// this.server.Delete(job);
      /// </code>
      /// </example>
      public void Pause(FaxJob job)
      {
         this.SetJobCommand(job, FaxJobCommand.Pause);
      }

      /// <summary>
      /// Resume the paused <paramref name="job"/>.
      /// </summary>
      /// <param name="job">Job to resume.</param>
      /// <exception cref="ArgumentNullException">If <paramref name="job"/> parameter is null.</exception>
      /// <exception cref="ArgumentException">If <paramref name="job"/> is not on the <see cref="FaxServer"/> instance.</exception>
      /// <example>
      /// See the example in <see cref="Pause"/> method.
      /// </example>
      public void Resume(FaxJob job)
      {
         this.SetJobCommand(job, FaxJobCommand.Resume);
      }

      /// <summary>
      /// Cancel the specified <paramref name="job"/>. The job can be active or queued.
      /// </summary>
      /// <param name="job">Job to delete.</param>
      /// <exception cref="ArgumentNullException">If <paramref name="job"/> parameter is null.</exception>
      /// <exception cref="ArgumentException">If <paramref name="job"/> is not on the <see cref="FaxServer"/> instance.</exception>
      /// <example>
      /// See the example in <see cref="Pause"/> method.
      /// </example>
      public void Delete(FaxJob job)
      {
         this.SetJobCommand(job, FaxJobCommand.Delete);
      }

      /// <summary>
      /// Restart the specified <paramref name="job"/>.
      /// </summary>
      /// <param name="job">Job to restart.</param>
      /// <exception cref="ArgumentNullException">If <paramref name="job"/> parameter is null.</exception>
      /// <exception cref="ArgumentException">If <paramref name="job"/> is not on the <see cref="FaxServer"/> instance.</exception>
      /// <example>
      /// See the example in <see cref="Pause"/> method.
      /// </example>
      public void Restart(FaxJob job)
      {
         this.Resume(job);
      }

      /// <summary>
      /// Free the ressources using by fax server.
      /// </summary>
      public void Dispose()
      {
         this.Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <overloads></overloads>
      ~FaxServer()
      {
         this.Dispose(false);
      }

      /// <summary>
      /// Free the ressources using by fax server.
      /// </summary>
      /// <param name="managed">true, if managed ressources must be free.</param>
      protected virtual void Dispose(bool managed)
      {
         if (this._faxHandle != IntPtr.Zero)
         {
            NativeMethods.FaxClose(this._faxHandle);
            this._faxHandle = IntPtr.Zero;
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Indicate if Windows Fax Service is installed on the current system.
      /// </summary>
      /// <remarks>This property looking for (in this order) :
      /// <list type="bullet">
      /// <item>If Windows Fax Service is intalled (Value "Installed" of "HKLM\SOFTWARE\Microsoft\Fax\Setup" key is "1")</item>
      /// <item>If WinFax.dll is installed on the system.</item>
      /// </list>
      /// </remarks>
      public static bool IsFaxServiceInstalled
      {
         get
         {
            if (IsInstalledInRegistry() == true)
            {
               string s = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), NativeMethods.DllWinFax);

               return File.Exists(s);
            }

            return false;
         }
      }

      /// <summary>
      /// Test connection to a fax server.
      /// </summary>
      /// <param name="serverName">Server name to connect (null or empty for localhost)</param>
      /// <returns>true if connection is possible</returns>
      /// <exception cref="FaxException">If Windows Fax Service is not installed on the current system.</exception>
      public static bool TestConnection(string serverName)
      {
         if (IsFaxServiceInstalled == false)
            throw new FaxException(FaxResources.ExceptionFaxServiceNotInstalled);

         IntPtr handle;

         handle = Connect(serverName);

         if (handle != IntPtr.Zero)
         {
            NativeMethods.FaxFreeBuffer(handle);
            return true;
         }

         return false;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Connect to a fax server.
      /// </summary>
      /// <param name="serverName">Server name to be connect.</param>
      /// <returns>The Windows handle of the Windows Fax Service. <see cref="IntPtr.Zero"/> if impossible.</returns>
      /// <remarks>
      /// BE CAREFUL : Free the handle ressource if the result is different of <see cref="IntPtr.Zero"/>.
      /// Use <see cref="NativeMethods.FaxClose(IntPtr)"/> native method.
      /// </remarks>
      private static IntPtr Connect(string serverName)
      {
         IntPtr handle;

         if (NativeMethods.FaxConnectFaxServer(GetLocalHostName(serverName), out handle) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         return handle;
      }

      /// <summary>
      /// Return null if <paramref name="hostName"/> parameter is "localhost".
      /// </summary>
      /// <param name="hostName">The hostname to be tested.</param>
      /// <returns>null if <paramref name="hostName"/> parameter is "localhost", the value of <paramref name="hostName"/> parameter in otherwise.</returns>
      private static string GetLocalHostName(string hostName)
      {
         if (string.IsNullOrEmpty(hostName) == true)
            return null;

         if (string.Compare(hostName, "localhost", true, CultureInfo.CurrentCulture) == 0)
            return null;

         return hostName;
      }

      /// <summary>
      /// Indicate if Windows Fax Service is installed (Checked by the registry).
      /// </summary>
      /// <returns>True if Windows Fax Service is installed (Checked by the registry).</returns>
      /// <remarks>
      /// <list type="bullet">
      /// <item>Key : "HKLM\SOFTWARE\Microsoft\Fax\Setup"</item>
      /// <item>Value name : Installed</item>
      /// <item>Value : "1"</item>
      /// </list>
      /// </remarks>
      private static bool IsInstalledInRegistry()
      {
         RegistryKey reg;

         reg = NativeMethods.OpenSubKey(Registry.LocalMachine, @"SOFTWARE\Microsoft\Fax\Setup");
         if (reg != null)
         {
            try
            {
               object o;

               o = reg.GetValue("Installed");
               if (o != null && Convert.ToBoolean(o) == true)
                  return true;
            }
            finally
            {
               reg.Close();
            }
         }

         return false;
      }

      /// <summary>Return an PrinterSettings object of a _printer name.</summary>
      /// <param name="printerName">Printer name</param>
      /// <returns>The _printer name of fax-_printer</returns>
      private static PrinterSettings GetPrinter(string printerName)
      {
         PrinterSettings printer;

         printer = new PrinterSettings();
         printer.PrinterName = printerName;

         return printer;
      }

      /// <summary>
      /// Get default data informations and the default cover page on the fax server, and fill it on differents parameters.
      /// </summary>
      /// <param name="informations">Informations which will be fill.</param>
      /// <param name="coverPage">Data cover page to be fill.</param>
      private void FillDefaultInformations(FaxInformations informations, FaxCoverPage coverPage)
      {
         if (CheckIsDisposed() == true)
            throw new FaxException(FaxResources.ExceptionFaxServerDisposed);

         IntPtr ptrJobParam;
         IntPtr ptrConverPage;

         NativeMethods.FAX_JOB_PARAM parameters;
         NativeMethods.FAX_COVERPAGE_INFO strCoverPage;

         if (NativeMethods.FaxCompleteJobParams(out ptrJobParam, out ptrConverPage) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         parameters = GetJobParameters(ptrJobParam);
         strCoverPage = GetCoverPage(ptrConverPage);

         if (coverPage != null)
            coverPage.Fill(strCoverPage);

         if (informations != null)
            informations.Fill(parameters);

         NativeMethods.FaxFreeBuffer(ptrJobParam);
         NativeMethods.FaxFreeBuffer(ptrConverPage);
      }

      private static NativeMethods.FAX_JOB_PARAM GetJobParameters(IntPtr ptr)
      {
         NativeMethods.FAX_JOB_PARAM parameters;

         parameters = new NativeMethods.FAX_JOB_PARAM();
         Marshal.PtrToStructure(ptr, parameters);

         return parameters;
      }

      private static NativeMethods.FAX_COVERPAGE_INFO GetCoverPage(IntPtr ptr)
      {
         NativeMethods.FAX_COVERPAGE_INFO coverPage;

         coverPage = new NativeMethods.FAX_COVERPAGE_INFO();
         Marshal.PtrToStructure(ptr, coverPage);

         return coverPage;
      }

      /// <summary>
      /// Return a list of all current id _jobs in the <see cref="Jobs"/> collection.
      /// </summary>
      /// <returns>A list of all current id _jobs in the <see cref="Jobs"/> collection.</returns>
      private List<uint> GetJobsID()
      {
         List<uint> list;

         list = new List<uint>(this.Jobs.Count);

         for (int i = 0; i < this.Jobs.Count; i++)
            list.Add(this.Jobs[i].ID);

         return list;
      }

      private NativeMethods.FAX_JOB_ENTRY GetJob(uint jobId)
      {
         IntPtr jobEntry;
         NativeMethods.FAX_JOB_ENTRY job;

         jobEntry = IntPtr.Zero;

         if (NativeMethods.FaxGetJob(this._faxHandle, jobId, out jobEntry) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         job = GetJob(jobEntry);

         NativeMethods.FaxFreeBuffer(jobEntry);

         return job;
      }

      private static NativeMethods.FAX_JOB_ENTRY GetJob(IntPtr ptr)
      {
         NativeMethods.FAX_JOB_ENTRY entry;

         entry = new NativeMethods.FAX_JOB_ENTRY();
         Marshal.PtrToStructure(ptr, entry);

         return entry;
      }

      /// <summary>
      /// Sent a command to a fax job.
      /// </summary>
      /// <param name="job">Job to be send the command.</param>
      /// <param name="command">Command to send.</param>
      private void SetJobCommand(FaxJob job, FaxJobCommand command)
      {
         if (job == null)
            throw new ArgumentNullException("job");

         if (job.Server != this)
            throw new ArgumentException(FaxResources.ExceptionBadOwnerServerForJob, "job");

         if (this.CheckIsDisposed() == true)
            throw new FaxException(FaxResources.ExceptionFaxServerDisposed);

         if (NativeMethods.FaxSetJob(this._faxHandle, job.ID, command) == false)
            throw FaxTools.CreateFaxException(string.Empty);
      }

      /// <summary>
      /// Refresh fax server configuration
      /// </summary>
      private void RefreshConfiguration()
      {
         NativeMethods.FAX_CONFIGURATION configuration;

         configuration = this.GetConfiguration(false);
         this.RefreshConfiguration(configuration);
      }

      private void RefreshConfiguration(NativeMethods.FAX_CONFIGURATION configuration)
      {
         this._retriesNumber = Convert.ToInt32(configuration.Retries);
         this._retryDelay = Convert.ToInt32(configuration.RetryDelay);

         this._archiveDirectory = configuration.ArchiveDirectory;
         this._archiveOutgoingFaxes = Convert.ToBoolean(configuration.ArchiveOutgoingFaxes);

         this._startCheapTime = NativeMethods.FaxTimeToTimeSpan(configuration.StartCheapTime);
         this._stopCheapTime = NativeMethods.FaxTimeToTimeSpan(configuration.StopCheapTime);
      }

      private NativeMethods.FAX_CONFIGURATION GetConfiguration(bool refresh)
      {
         IntPtr ptr;
         NativeMethods.FAX_CONFIGURATION configuration;

         if (NativeMethods.FaxGetConfiguration(this._faxHandle, out ptr) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         configuration = new NativeMethods.FAX_CONFIGURATION();
         Marshal.PtrToStructure(ptr, configuration);

         NativeMethods.FaxFreeBuffer(ptr);

         if (refresh == true)
            this.RefreshConfiguration();

         return configuration;
      }

      private void UpdateConfiguration(NativeMethods.FAX_CONFIGURATION configuration)
      {
         if (NativeMethods.FaxSetConfiguration(this._faxHandle, configuration) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         this.RefreshConfiguration(configuration);
      }

      /// <summary>
      /// Refresh devices _ports.
      /// </summary>
      private void RefreshPorts()
      {
         IntPtr ports;
         uint count;
         List<uint> currentPorts;
         IntPtr ptrPort;
         NativeMethods.FAX_PORT_INFO portInfo;
         FaxPort port;

         if (NativeMethods.FaxEnumPorts(this._faxHandle, out ports, out count) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         //Get all current port ID in the collection
         //This list is using for knowing which job will be remove
         currentPorts = this.GetPortsID();

         //Read all _ports structures...
         ptrPort = ports;
         for (int i = 0; i < count; i++)
         {
            portInfo = new NativeMethods.FAX_PORT_INFO();

            Marshal.PtrToStructure(ptrPort, portInfo);

            //We have a portInfo, find it in the current collection :
            port = this.Ports.GetPort(portInfo.DeviceId);
            if (port == null)
            {
               //The port doesn't exist in the collection, so create it !
               this.Ports.Add(new FaxPort(this, portInfo));
            }
            else
            {
               //The port exist, so refresh and remove it on the list of current port id
               port.Refresh(portInfo);
               currentPorts.Remove(portInfo.DeviceId);
            }

            //Next structure...
            ptrPort = (IntPtr)((int)ptrPort + Marshal.SizeOf(typeof(NativeMethods.FAX_PORT_INFO)));
         }

         //Free array...
         NativeMethods.FaxFreeBuffer(ports);

         //Now remove port in CurrentPorts
         foreach (uint id in currentPorts)
            this.Jobs.Remove(id);
      }

      private List<uint> GetPortsID()
      {
         List<uint> list;

         list = new List<uint>(this.Ports.Count);

         for (int i = 0; i < this.Ports.Count; i++)
            list.Add(this.Ports[i].DeviceId);

         return list;
      }

      /// <summary>
      /// This methode check if <see cref="FaxServer"/> object is disposed.
      /// </summary>
      /// <returns>True if <see cref="FaxServer"/> object is disposed.</returns>
      /// <remarks>Use this method on all public method in the library.</remarks>
      internal bool CheckIsDisposed()
      {
         if (this._faxHandle == IntPtr.Zero)
            return true;

         return false;
      }

      #endregion
      
   }

}
