using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Class for calling natives methods for Fax Service.
   /// </summary>
   internal class NativeMethods
   {
      private const string DllGdi32 = "gdi32.dll";
      internal const string DllWinFax = "WinFax.dll";

      private const int MAX_COMPUTERNAME_LENGTH = 16;

      private const int KEY_QUERY_VALUE = 0x0001;
      private const int KEY_WOW64_64KEY = 0x0100;

      [StructLayout(LayoutKind.Sequential)]
      internal struct SYSTEMTIME
      {
         public ushort wYear;
         public ushort wMonth;
         public ushort wDayOfWeek;
         public ushort wDay;
         public ushort wHour;
         public ushort wMinute;
         public ushort wSecond;
         public ushort wMilliseconds;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      internal class FAX_PRINT_INFO
      {
         internal FAX_PRINT_INFO()
         {
            this.SizeOfStruct = Convert.ToUInt32(Marshal.SizeOf(typeof(FAX_PRINT_INFO)));
         }

         public uint SizeOfStruct;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string DocName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecipientName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecipientNumber;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SenderName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SenderCompany;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SenderDept;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SenderBillingCode;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string Reserved;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string DrEmailAddress;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string OutputFileName;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      internal struct FAX_CONTEXT_INFO
      {
         public uint SizeOfStruct;
         public IntPtr hDC;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_COMPUTERNAME_LENGTH)]
         public string ServerName;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      internal class FAX_JOB_PARAM
      {
         public uint SizeOfStruct;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecipientNumber;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecipientName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string Tsid;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SenderName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SenderCompany;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SenderDept;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string BillingCode;
         public uint ScheduleAction;
         public SYSTEMTIME ScheduleTime;
         public uint DeliveryReportType;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string DeliveryReportAddress;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string DocumentName;
         private IntPtr CallHandle;
         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
         private UIntPtr[] Reserved;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      internal class FAX_COVERPAGE_INFO
      {
         public uint SizeOfStruct;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string CoverPageName;
         public int UseServerCoverPage;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecFaxNumber;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecCompany;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecStreetAddress;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecCity;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecState;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecZip;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecCountry;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecTitle;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecDepartment;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecOfficeLocation;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecHomePhone;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string RecOfficePhone;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SdrName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SdrFaxNumber;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SdrCompany;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SdrAddress;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SdrTitle;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SdrDepartment;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SdrOfficeLocation;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SdrHomePhone;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string SdrOfficePhone;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string Note;
         [MarshalAs(UnmanagedType.LPTStr)]
         public string Subject;
         public SYSTEMTIME TimeSent;
         public uint PageCount;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      internal class FAX_JOB_ENTRY
      {
         public uint SizeOfStruct;
         public uint JobId;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String UserName;
         public uint JobType;
         public uint QueueStatus;
         public uint Status;
         public uint Size;
         public uint PageCount;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String RecipientNumber;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String RecipientName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String Tsid;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String SenderName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String SenderCompany;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String SenderDept;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String BillingCode;
         public uint ScheduleAction;
         public SYSTEMTIME ScheduleTime;
         public uint DeliveryReportType;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String DeliveryReportAddress;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String DocumentName;
      }

      [StructLayout(LayoutKind.Sequential)]
      internal struct FAX_TIME
      {
         public ushort Hour;
         public ushort Minute;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      internal class FAX_CONFIGURATION
      {
         internal FAX_CONFIGURATION()
         {
            this.SizeOfStruct = Convert.ToUInt32(Marshal.SizeOf(this));
         }

         public uint SizeOfStruct;
         public uint Retries;
         public uint RetryDelay;
         public int Branding;
         public uint DirtyDays;
         public int UseDeviceTsid;
         public int ServerCp;
         public int PauseServerQueue;
         public FAX_TIME StartCheapTime;
         public FAX_TIME StopCheapTime;
         public int ArchiveOutgoingFaxes;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String ArchiveDirectory;
         [MarshalAs(UnmanagedType.LPStr)]
         public String InboundProfile;
      }


      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      internal class FAX_PORT_INFO
      {
         public uint SizeOfStruct;
         public uint DeviceId;
         public uint State;
         public uint Flags;
         public uint Rings;
         public uint Priority;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String DeviceName;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String Tsid;
         [MarshalAs(UnmanagedType.LPTStr)]
         public String Csid;
      }

      #region Extern Methods

      [DllImport(DllGdi32)]
      internal static extern int EndDoc(IntPtr hdc);

      [DllImport(DllGdi32)]
      internal static extern int AbortDoc(IntPtr hdc);

      [DllImport(DllGdi32)]
      internal static extern int EndPage(IntPtr hdc);

      [DllImport(DllGdi32)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeleteDC(IntPtr hdc);

      [DllImport(DllWinFax, SetLastError = true, EntryPoint = "FaxStartPrintJob", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxStartPrintJob(string PrinterName, [In] FAX_PRINT_INFO PrintInfo, out uint FaxJobId, out IntPtr FaxContextInfo);

      [DllImport(DllWinFax, SetLastError = true, EntryPoint = "FaxCompleteJobParams", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxCompleteJobParams(out IntPtr JobParams, out IntPtr CoverpageInfo);

      [DllImport(DllWinFax, SetLastError = true, EntryPoint = "FaxConnectFaxServer", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxConnectFaxServer(string MachineName, out IntPtr FaxHandle);

      [DllImport(DllWinFax, SetLastError = true, EntryPoint = "FaxClose")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxClose(IntPtr FaxHandle);

      [DllImport(DllWinFax, SetLastError = true, EntryPoint = "FaxEnumJobs", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxEnumJobs(IntPtr FaxHandle, out IntPtr JobEntry, out uint JobsReturned);

      [DllImport(DllWinFax, SetLastError = true, EntryPoint = "FaxGetJob", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxGetJob(IntPtr FaxHandle, uint SpoolId, out IntPtr JobEntry);

      [DllImport(DllWinFax, SetLastError = true, EntryPoint = "FaxSetJob")]
      [return: MarshalAs(UnmanagedType.Bool)]
      private static extern bool FaxSetJob(IntPtr FaxHandle, uint JobId, uint Command, IntPtr NoUseMustZero);

      [DllImport(DllWinFax, SetLastError = true, EntryPoint = "FaxSendDocument", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxSendDocument(IntPtr FaxHandle, string FileName, FAX_JOB_PARAM JobParams, [In] FAX_COVERPAGE_INFO CoverpageInfo, out uint FaxJobId);

      [DllImport(DllWinFax, SetLastError = true, EntryPoint = "FaxGetPageData")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxGetPageData(IntPtr FaxHandle, uint JobId, out IntPtr Buffer, out uint BufferSize, out uint ImageWidth, out uint ImageHeight);

      [DllImport(DllWinFax, EntryPoint = "FaxFreeBuffer")]
      internal static extern void FaxFreeBuffer(IntPtr Buffer);

      [DllImport(DllWinFax, EntryPoint = "FaxGetConfiguration", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxGetConfiguration(IntPtr FaxHandle, out IntPtr FaxConfig);

      [DllImport(DllWinFax, EntryPoint = "FaxSetConfiguration", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxSetConfiguration(IntPtr FaxHandle, [In] FAX_CONFIGURATION FaxConfig);

      [DllImport(DllWinFax, EntryPoint = "FaxPrintCoverPage", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxPrintCoverPage([In] FAX_CONTEXT_INFO FaxContextInfo, [In] FAX_COVERPAGE_INFO CoverPageInfo);

      [DllImport(DllWinFax, EntryPoint = "FaxOpenPort")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxOpenPort(IntPtr FaxHandle, uint DeviceId, uint Flags, out IntPtr FaxPortHandle);

      [DllImport(DllWinFax, EntryPoint = "FaxGetPort", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxGetPort(IntPtr FaxPortHandle, out IntPtr PortInfo);

      [DllImport(DllWinFax, EntryPoint = "FaxSetPort", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxSetPort(IntPtr FaxPortHandle, [In] FAX_PORT_INFO PortInfo);

      [DllImport(DllWinFax, EntryPoint = "FaxEnumPorts", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FaxEnumPorts(IntPtr FaxHandle, out IntPtr PortInfo, out uint PortsReturned);

      [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegOpenKeyEx")]
      private static extern int RegOpenKeyEx(IntPtr hKey, string subKey, uint options, int sam, out IntPtr phkResult);

      #endregion

      internal static bool FaxSetJob(IntPtr faxHandle, uint jobId, FaxJobCommand command)
      {
         return FaxSetJob(faxHandle, jobId, Convert.ToUInt32(command), IntPtr.Zero);
      }

      internal static TimeSpan FaxTimeToTimeSpan(FAX_TIME faxTime)
      {
         return new TimeSpan(faxTime.Hour, faxTime.Minute, 0);
      }

      internal static void TimeSpanToFaxTime(TimeSpan ts, FAX_TIME faxTime)
      {
         faxTime.Hour = Convert.ToUInt16(ts.Hours);
         faxTime.Minute = Convert.ToUInt16(ts.Minutes);
      }

      internal static SYSTEMTIME DateTimeToSystemTime(DateTime dateTime)
      {
         SYSTEMTIME systemTime;

         systemTime = new SYSTEMTIME();

         systemTime.wDay = Convert.ToUInt16(dateTime.Day);
         systemTime.wDayOfWeek = Convert.ToUInt16(dateTime.DayOfWeek);
         systemTime.wHour = Convert.ToUInt16(dateTime.Hour);
         systemTime.wMilliseconds = Convert.ToUInt16(dateTime.Millisecond);
         systemTime.wMinute = Convert.ToUInt16(dateTime.Minute);
         systemTime.wMonth = Convert.ToUInt16(dateTime.Month);
         systemTime.wSecond = Convert.ToUInt16(dateTime.Second);
         systemTime.wYear = Convert.ToUInt16(dateTime.Year);

         return systemTime;
      }

      internal static DateTime? SystemTimeToDateTime(SYSTEMTIME time)
      {
         if (time.wYear == 0 && time.wMonth == 0 && time.wDay == 0 && time.wHour == 0 && time.wMinute == 0 && time.wSecond == 0 && time.wMilliseconds == 0)
            return null;

         return new DateTime(time.wYear, time.wMonth, time.wDay, time.wHour, time.wMinute, time.wSecond, time.wMilliseconds);
      }

      internal static readonly FieldInfo HkeyField = typeof(RegistryKey).GetField("hkey", BindingFlags.NonPublic | BindingFlags.Instance);

      private static SafeHandle CreateRegistrySafeHandle(IntPtr handle)
      {
         Type type;

         type = typeof(SafeHandle).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");

         return (SafeHandle)Activator.CreateInstance(type,
                                                     BindingFlags.Instance | BindingFlags.NonPublic,
                                                     null,
                                                     new object[] { handle, true },
                                                     null);
      }

      internal static RegistryKey OpenSubKey(RegistryKey key, string name)
      {
         SafeHandle keyHandle;
         IntPtr keyPtr;
         IntPtr hTargetKey;

         keyHandle = (SafeHandle)HkeyField.GetValue(key);
         keyPtr = keyHandle.DangerousGetHandle();

         if (RegOpenKeyEx(keyPtr, name, 0, KEY_QUERY_VALUE | KEY_WOW64_64KEY, out hTargetKey) != 0)
         {
            throw FaxTools.CreateFaxException(FaxResources.ExceptionUnableOpenRegistryKey);
         }

         keyHandle = CreateRegistrySafeHandle(hTargetKey);

         return (RegistryKey)Activator.CreateInstance(typeof(RegistryKey),
                                                      BindingFlags.Instance | BindingFlags.NonPublic,
                                                      null,
                                                      new object[] { keyHandle, true },
                                                      null);
      }
   }
}
