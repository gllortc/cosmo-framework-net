using Cosmo.Utils;
using System;
using System.Collections.Generic;

namespace Cosmo
{
   /// <summary>
   /// Base class to develop standard Cosmo service managers.
   /// </summary>
   /// <typeparam name="T">Generic type for the service modules.</typeparam>
   public abstract class WorkspaceService<T>
   {
      // Internal data declarations
      private Dictionary<String, T> pluginInstances;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="WorkspaceService{T}"/>.
      /// </summary>
      /// <param name="workspace">Current workspace.</param>
      /// <param name="modules">Collection of plugin modules available to service.</param>
      protected WorkspaceService(Workspace workspace, PluginCollection modules)
      {
         this.Modules = modules;
         this.Workspace = workspace;

         CreateModuleInstances();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the plugin modules.
      /// </summary>
      public Workspace Workspace { get; private set; }

      /// <summary>
      /// Gets the plugin modules.
      /// </summary>
      internal PluginCollection Modules { get; private set; }

      /// <summary>
      /// Gets the default module for the plugin.
      /// </summary>
      public T DefaultModule
      {
         get
         {
            if (this.Modules.ContainsPlugin(this.Modules.DefaultPluginId))
            {
               return pluginInstances[this.Modules.DefaultPluginId];
            }
            else if (this.Modules.Count > 0)
            {
               // Return the first available
               foreach (T t in pluginInstances.Values)
               {
                  return t;
               }
            }

            return default(T);
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Load all defined plugin modules.
      /// </summary>
      private void CreateModuleInstances()
      {
         string applyRule = string.Empty;
         Type type = null;
         T _module;

         // Initialize the instances dictionary
         pluginInstances = new Dictionary<string, T>();

         foreach (Plugin plugin in this.Modules.GetList())
         {
            try
            {
               Object[] args = new Object[2];
               args[0] = this.Workspace;
               args[1] = plugin;

               type = Type.GetType(plugin.Class, true, true);
               _module = (T)Activator.CreateInstance(type, args);

               if (_module != null)
               {
                  pluginInstances.Add(plugin.ID, _module);
               }
            }
            catch
            {
               Workspace.Logger.Warning(this.GetType().Name, "Can't load " + plugin.Class + " service module.");
            }
         }
      }

      #endregion

   }
}
