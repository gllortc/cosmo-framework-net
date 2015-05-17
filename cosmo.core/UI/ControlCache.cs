using Cosmo.UI.Controls;
using System;
using System.Web.Caching;

namespace Cosmo.UI
{
   /// <summary>
   /// Implements a control cache for implementatios of <see cref="ViewContainer"/> class.
   /// </summary>
   public class ControlCache
   {
      // Declare internal data
      private Cache cache;

      /// <summary>Decfine the default time (in minutes) for cache content.</summary>
      public const int DEFAULT_CACHE_TIME = 60;

      #region Constructors

      /// <summary>
      /// Create an instance of <see cref="ControlCache"/>.
      /// </summary>
      /// <param name="serverCache">An instance of <see cref="Cache"/> representing the server cache.</param>
      public ControlCache(Cache serverCache)
      {
         Initialize();

         cache = serverCache;
      }

      #endregion

      #region Methods

      /// <summary>
      /// Indicate if an object is stored in cache.
      /// </summary>
      /// <param name="key">Identifier for object stored.</param>
      /// <returns><c>true</c> if object exist, otherwise return <c>false</c>.</returns>
      public bool Exist(string key)
      {
         return (cache.Get(key) != null);
      }

      /// <summary>
      /// Add a control into cache with default time and priority.
      /// </summary>
      /// <param name="key">Identifier for object stored.</param>
      /// <param name="control">Control to store in cache.</param>
      public void Add(string key, Control control)
      {

         cache.Add(key,
                   control,
                   null,
                   System.Web.Caching.Cache.NoAbsoluteExpiration,
                   new TimeSpan(0, DEFAULT_CACHE_TIME, 0),
                   System.Web.Caching.CacheItemPriority.Default,
                   null);
      }

      /// <summary>
      /// Add a control into cache with default priority.
      /// </summary>
      /// <param name="key">Identifier for object stored.</param>
      /// <param name="control">Control to store in cache.</param>
      /// <param name="minutes">Amount of minutes that the control remain stored.</param>
      public void Add(string key, Control control, int minutes)
      {

         cache.Add(key,
                   control,
                   null,
                   System.Web.Caching.Cache.NoAbsoluteExpiration,
                   new TimeSpan(0, minutes, 0),
                   System.Web.Caching.CacheItemPriority.Default,
                   null);
      }

      /// <summary>
      /// Add a control into cache with default priority.
      /// </summary>
      /// <param name="key">Identifier for object stored.</param>
      /// <param name="control">Control to store in cache.</param>
      /// <param name="minutes">Amount of minutes that the control remain stored.</param>
      /// <param name="priority">Priority (to remain stored) of objects stored in cache.</param>
      public void Add(string key, Control control, int minutes, CacheItemPriority priority)
      {

         cache.Add(key,
                   control,
                   null,
                   System.Web.Caching.Cache.NoAbsoluteExpiration,
                   new TimeSpan(0, minutes, 0),
                   priority,
                   null);
      }

      public object Get(string key)
      {
         return cache.Get(key);
      }

      /// <summary>
      /// Remove a control from cache.
      /// </summary>
      /// <param name="key">Identifier for object stored.</param>
      public void Remove(string key)
      {
         cache.Remove(key);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initialize the instance.
      /// </summary>
      private void Initialize()
      {
         cache = null;
      }

      #endregion
   }
}
