﻿using Cosmo.Utils;
using System.Collections.Generic;

namespace Cosmo.Data.Lists.Impl
{
   /// <summary>
   /// Implementa un proveedor de listas de datos estáticas.
   /// </summary>
   public class StaticDataListImpl : IDataList
   {
      // Internal data declarations
      private List<KeyValue> _values;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="StaticDataListImpl"/>.
      /// </summary>
      /// <param name="workspace">An instance of current Cosmo workspace</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene los datos del módulo y su configuración.</param>
      public StaticDataListImpl(Workspace workspace, Plugin plugin) 
         : base(workspace, plugin)
      {
         Initialize();
         LoadData(plugin);
      }

      #endregion

      #region IDataList Implementation

      /// <summary>
      /// Gets or sets la lista de valores que contiene la lista.
      /// </summary>
      public override List<Utils.KeyValue> Values
      {
         get { return _values; }
         set { _values = value; }
      }

      /// <summary>
      /// Obtiene el valor asociado a una determinada clave.
      /// </summary>
      /// <param name="key">Clave para la que se desea obtener el valor.</param>
      /// <returns>Una cadena que contiene el valor asociado a la clave proporcionada.</returns>
      public override string GetValueByKey(string key)
      {
         foreach (KeyValue item in _values)
         {
            if (item.Value.Equals(key))
            {
               return item.Label;
            }
         }

         return null;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _values = new List<KeyValue>();
      }

      /// <summary>
      /// Carga los valores de la lista desde la configuración del módulo.
      /// </summary>
      private void LoadData(Plugin plugin)
      {
         foreach (string key in plugin.Settings.AllKeys)
         {
            if (!key.Trim().ToLower().Equals("default.value"))
            {
               _values.Add(new KeyValue(key, plugin.Settings[key]));
            }
         }

         DefaultValue = plugin.GetString("default.value");
      }

      #endregion

   }
}
