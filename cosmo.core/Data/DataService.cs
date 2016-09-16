using Cosmo.Data.Connection;
using Cosmo.Data.Lists;
using Cosmo.Utils;
using System;
using System.Collections.Generic;

namespace Cosmo.Data
{
   /// <summary>
   /// Implementa el sericio de acceso a datos de Cosmo.
   /// </summary>
   public class DataService
   {
      // Declaración de variable sinternas
      private string _defaultName;
      private Workspace _ws;
      private Dictionary<string, DataModule> _dataSources;
      private Dictionary<string, IDataList> _dataLists;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="DataService"/>.
      /// </summary>
      /// <param name="workspace">An instance of current Cosmo workspace</param>
      public DataService(Workspace workspace)
      {
         Initialize();

         _ws = workspace;

         LoadDataModules();
         LoadDataLists();
      }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve el DataSource por defecto.
      /// </summary>
      /// <returns>La instancia solicitada de <see cref="DataModule"/>.</returns>
      public DataModule GetDataSource()
      {
         return _dataSources[_defaultName];
      }

      /// <summary>
      /// Devuelve el DataSource solicitado.
      /// </summary>
      /// <param name="name">Identificador de la conexión que se desea obtener.</param>
      /// <returns>La instancia solicitada de <see cref="DataModule"/>.</returns>
      public DataModule GetDataSource(string name)
      {
         return _dataSources[name];
      }

      /// <summary>
      /// Obtiene una determinada lista.
      /// </summary>
      /// <param name="name">Identificador de la lista que se desea obtener.</param>
      /// <returns>La instancia solicitada de <see cref="IDataList"/>.</returns>
      public IDataList GetDataList(string name)
      {
         return _dataLists[name];
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _ws = null;
         _defaultName = string.Empty;
         _dataSources = new Dictionary<string, DataModule>();
         _dataLists = new Dictionary<string, IDataList>();
      }

      /// <summary>
      /// Carga los datasources definidos en la configuración y selecciona el datasource por defecto.
      /// </summary>
      private void LoadDataModules()
      {
         string applyRule = string.Empty;
         Type type = null;
         DataModule _module;

         // Carga los módulos
         foreach (Plugin plugin in _ws.Settings.DataModules.GetList())
         {
            Object[] args = new Object[2];
            args[0] = _ws;
            args[1] = plugin;

            type = Type.GetType(plugin.Class, true, true);
            _module = (DataModule)Activator.CreateInstance(type, args);

            if (_module != null)
            {
               _dataSources.Add(_module.ID, _module);
            }
         }

         // Obtiene el índice del DataSource por defecto
         if (_ws.Settings.DataModules.GetPlugin(_ws.Settings.DataModules.DefaultPluginId) != null)
         {
            _defaultName = _ws.Settings.DataModules.DefaultPluginId;
         }
         else if (_ws.Settings.DataModules.Count > 0)
         {
            // TODO: Get first
            _defaultName = string.Empty; // _ws.Settings.DataModules.GetList(). [0].ID;
         }
         else
         {
            _defaultName = string.Empty;
         }
      }

      /// <summary>
      /// Carga las listas de datos definidas en la configuración.
      /// </summary>
      private void LoadDataLists()
      {
         Type type = null;
         IDataList _module;

         // Carga los módulos
         foreach (Plugin plugin in _ws.Settings.DataLists.GetList())
         {
            Object[] args = new Object[2];
            args[0] = _ws;
            args[1] = plugin;

            type = Type.GetType(plugin.Class, true, true);
            _module = (IDataList)Activator.CreateInstance(type, args);

            if (_module != null)
            {
               _dataLists.Add(_module.ID, _module);
            }
         }
      }

      /// <summary>
      /// Obtiene la instancia de un determinado plugin.
      /// </summary>
      private DataModule CheckInstance(Plugin plugin)
      {
         if (plugin.Instance != null)
         {
            return (DataModule)plugin.Instance;
         }

         Object[] args = new Object[1];
         args[0] = plugin;

         Type type = Type.GetType(plugin.Class, true, true);
         plugin.Instance = (DataModule)Activator.CreateInstance(type, args);

         return (DataModule)plugin.Instance;
      }

      #endregion

   }
}
