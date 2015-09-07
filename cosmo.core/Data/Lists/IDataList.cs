using Cosmo.Utils;
using System.Collections.Generic;

namespace Cosmo.Data.Lists
{
   /// <summary>
   /// Declara la estructura que deben implementar los proveedores de listas de datos dinámicas.
   /// </summary>
   public abstract class IDataList
   {
      // Internal data declarations
      private Workspace _ws;
      private Plugin _plugin;
      private string _defaultValue;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="IDataList"/>.
      /// </summary>
      /// <param name="workspace">An instance of current Cosmo workspace</param>
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
      /// Gets or sets el identificador único de la lista.
      /// </summary>
      public string ID
      {
         get { return _plugin.ID; }
      }

      /// <summary>
      /// Gets or sets el valor por defecto que debe aparecer seleccionado en la listas.
      /// </summary>
      public string DefaultValue
      {
         get { return _defaultValue; }
         set { _defaultValue = value; }
      }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Gets or sets la lista de valores que contiene la lista.
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
