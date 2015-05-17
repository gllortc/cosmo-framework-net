using Cosmo.Utils;
using System.Collections.Generic;

namespace Cosmo.Data.Lists
{
   /// <summary>
   /// Declara la estructura que deben implementar los proveedores de listas de datos dinámicas.
   /// </summary>
   public abstract class IDataList
   {
      // Declaración de variables internas
      private Workspace _ws;
      private Plugin _plugin;
      private string _defaultValue;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="IDataList"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene los datos del módulo y su configuración.</param>
      protected IDataList(Workspace workspace, Plugin plugin)
      {
         _ws = workspace;
         _plugin = plugin;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el <see cref="Workspace"/> actual.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
      }

      /// <summary>
      /// Devuelve o establece el identificador único de la lista.
      /// </summary>
      public string ID
      {
         get { return _plugin.ID; }
      }

      /// <summary>
      /// Devuelve o establece el valor por defecto que debe aparecer seleccionado en la listas.
      /// </summary>
      public string DefaultValue
      {
         get { return _defaultValue; }
         set { _defaultValue = value; }
      }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Devuelve o establece la lista de valores que contiene la lista.
      /// </summary>
      public abstract List<KeyValue> Values { get; set; }

      /// <summary>
      /// Obtiene el valor asociado a una determinada clave.
      /// </summary>
      /// <param name="key">Clave para la que se desea obtener el valor.</param>
      /// <returns>Una cadena que contiene el valor asociado a la clave proporcionada.</returns>
      public abstract string GetValueByKey(string key);

      #endregion

   }
}
