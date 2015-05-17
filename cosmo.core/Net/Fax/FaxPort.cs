using System;
using System.Runtime.InteropServices;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Describe the configuration of one fax port.
   /// </summary>
   public class FaxPort
   {

      #region Enumerations

      internal enum FaxPortOpenType
      {
         Query = 1,
         Modify
      }

      /// <summary>
      /// Represents the capability of fax port.
      /// </summary>
      [Flags]
      public enum FaxPortCapability
      {
         /// <summary>The device can receive faxes.</summary>
         Receive = 0x00000001,
         /// <summary>The device can send faxes.</summary>
         Send = 0x00000002,
         /// <summary>The device is a virtual fax device.</summary>
         Virtual = 0x00000004
      }

      #endregion

      private FaxServer server;
      private uint deviceID;
      private string deviceName;
      private FaxPortCapability capability;
      private int rings;
      private string tsid;
      private string csid;
      private int priority;

      internal FaxPort(FaxServer server, NativeMethods.FAX_PORT_INFO portInfo)
      {
         this.server = server;
         this.deviceID = portInfo.DeviceId;

         this.Refresh(portInfo);
      }

      #region Properties

      /// <summary>
      /// Get the internal device ID.
      /// </summary>
      internal uint DeviceId
      {
         get { return this.deviceID; }
      }

      /// <summary>
      /// Get attached fax server.
      /// </summary>
      public FaxServer Server
      {
         get { return this.server; }
      }

      /// <summary>
      /// Get the device name.
      /// </summary>
      public string DeviceName
      {
         get { return this.deviceName; }
      }
      /// <summary>
      /// Get the capability of the fax port.
      /// </summary>
      public FaxPortCapability Capability
      {
         get { return this.capability; }
      }

      /// <summary>
      /// Get or set the number of times an incoming fax call should ring before the device answers the call. 
      /// </summary>
      public int Rings
      {
         get { return this.rings; }
         set
         {
            if (this.rings != value)
            {
               NativeMethods.FAX_PORT_INFO configuration;

               configuration = this.GetConfiguration(true);
               configuration.Rings = Convert.ToUInt32(value);

               this.UpdateConfiguration(configuration);
            }
         }
      }

      /// <summary>
      /// Get or set transmitting station identifier (TSID). This identifier is usually a telephone number.
      /// </summary>
      public string Tsid
      {
         get { return this.tsid; }
         set
         {
            if (this.tsid != value)
            {
               NativeMethods.FAX_PORT_INFO configuration;

               configuration = this.GetConfiguration(true);
               configuration.Tsid = value;

               this.UpdateConfiguration(configuration);
            }
         }
      }

      /// <summary>
      /// Get or set called station identifier (CSID). This identifier is usually a telephone number.
      /// </summary>
      public string Csid
      {
         get { return this.csid; }
         set
         {
            if (this.csid != value)
            {
               NativeMethods.FAX_PORT_INFO configuration;

               configuration = this.GetConfiguration(true);
               configuration.Csid = value;

               this.UpdateConfiguration(configuration);
            }
         }
      }

      /// <summary>
      /// Get or set the the relative order in which available fax devices send outgoing transmissions.
      /// </summary>
      public int Priority
      {
         get { return this.priority; }
         set
         {
            if (this.priority != value)
            {
               NativeMethods.FAX_PORT_INFO configuration;

               configuration = this.GetConfiguration(true);
               configuration.Priority = Convert.ToUInt32(value);

               this.UpdateConfiguration(configuration);
            }
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Refresh fax server configuration
      /// </summary>
      private void Refresh()
      {
         NativeMethods.FAX_PORT_INFO configuration;

         configuration = this.GetConfiguration(false);
         this.Refresh(configuration);
      }

      internal void Refresh(NativeMethods.FAX_PORT_INFO portInfo)
      {
         this.deviceName = portInfo.DeviceName;
         this.capability = (FaxPortCapability)Enum.ToObject(typeof(FaxPortCapability), portInfo.Flags);

         this.rings = Convert.ToInt32(portInfo.Rings);
         this.tsid = portInfo.Tsid;
         this.csid = portInfo.Csid;
         this.priority = Convert.ToInt32(portInfo.Priority);
      }

      private NativeMethods.FAX_PORT_INFO GetConfiguration(bool refresh)
      {
         IntPtr ptr;
         IntPtr portHandle;
         NativeMethods.FAX_PORT_INFO configuration;

         if (this.server.CheckIsDisposed() == true)
            throw new FaxException(FaxResources.ExceptionFaxServerDisposed);

         if (NativeMethods.FaxOpenPort(this.server._faxHandle, this.deviceID, Convert.ToUInt32(FaxPortOpenType.Query), out portHandle) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         try
         {
            if (NativeMethods.FaxGetPort(portHandle, out ptr) == false)
               throw FaxTools.CreateFaxException(string.Empty);

            configuration = new NativeMethods.FAX_PORT_INFO();
            Marshal.PtrToStructure(ptr, configuration);

            NativeMethods.FaxFreeBuffer(ptr);

            if (refresh == true)
               this.Refresh();
         }
         finally
         {
            NativeMethods.FaxClose(portHandle);
         }

         return configuration;
      }

      private void UpdateConfiguration(NativeMethods.FAX_PORT_INFO configuration)
      {
         IntPtr portHandle;

         if (NativeMethods.FaxOpenPort(this.server._faxHandle, this.deviceID, Convert.ToUInt32(FaxPortOpenType.Modify), out portHandle) == false)
            throw FaxTools.CreateFaxException(string.Empty);

         try
         {
            if (NativeMethods.FaxSetPort(portHandle, configuration) == false)
               throw FaxTools.CreateFaxException(string.Empty);

            this.Refresh(configuration);
         }
         finally
         {
            NativeMethods.FaxClose(portHandle);
         }
      }

      #endregion

   }

}
