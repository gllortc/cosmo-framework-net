using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Represent a read-only collection of fax port devices on a <see cref="FaxServer"/>.
   /// </summary>
   public class FaxPortCollection : ReadOnlyCollection<FaxPort>
   {
      internal FaxPortCollection() : base(new List<FaxPort>()) { }

      internal void Add(FaxPort port)
      {
         this.Items.Add(port);
      }

      internal FaxPort GetPort(uint id)
      {
         foreach (FaxPort port in this)
         {
            if (port.DeviceId == id)
               return port;
         }

         return null;
      }

      /// <summary>
      /// Remove a port device.
      /// </summary>
      /// <param name="id">ID of port device to be delete.</param>
      internal void Remove(uint id)
      {
         this.Items.Remove(this.GetPort(id));
      }
   }

}
