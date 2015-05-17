using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Represent a read-only collection of fax _jobs.
   /// </summary>
   public class FaxJobCollection : ReadOnlyCollection<FaxJob>
   {
      internal FaxJobCollection() : base(new List<FaxJob>()) { }

      /// <summary>
      /// Add a <see cref="FaxJob"/> object in the collection.
      /// </summary>
      /// <param name="faxJob"></param>
      internal void Add(FaxJob faxJob)
      {
         int index;

         for (index = 0; index < this.Count && this[index].ID < faxJob.ID; index++) ;

         this.Items.Insert(index, faxJob);
      }

      /// <summary>
      /// Find a fax job by ID and return it.
      /// </summary>
      /// <param name="id">ID of fax job to be find.</param>
      /// <returns>The fax job found or null in otherwise.</returns>
      internal FaxJob GetJob(uint id)
      {
         foreach (FaxJob job in this)
         {
            if (job.ID == id) return job;
         }

         return null;
      }

      /// <summary>
      /// Remove a fax job.
      /// </summary>
      /// <param name="id">ID of fax job to be delete.</param>
      internal void Remove(uint id)
      {
         this.Items.Remove(this.GetJob(id));
      }

      /// <summary>
      /// Get all fax _jobs for the current user.
      /// </summary>
      /// <returns>All fax _jobs of the current user.</returns>
      /// <remarks>The current user is obtained by <see cref="Environment.UserDomainName"/> and <see cref="Environment.UserName"/></remarks>
      public FaxJob[] GetJobsUser()
      {
         return this.GetJobsUser(string.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName));
      }

      /// <summary>
      /// Get all fax _jobs for an user.
      /// </summary>
      /// <param name="userName">Windows user name to be found (Domain\\UserName)</param>
      /// <returns>All fax _jobs of the <paramref name="userName"/></returns>
      public FaxJob[] GetJobsUser(string userName)
      {
         List<FaxJob> jobs;

         jobs = new List<FaxJob>();

         foreach (FaxJob job in this)
         {
            if (string.Compare(job.UserName, userName, true, CultureInfo.CurrentCulture) == 0)
               jobs.Add(job);
         }

         return jobs.ToArray();
      }
   }

}
