using Cosmo.Data.Connection;
using Cosmo.Diagnostics;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Cosmo.Data.Lists.Impl
{
   /// <summary>
   /// Implementa una lista de datos que se puede usar para rellenar campso de lista, etc.
   /// </summary>
   public class SqlDataListImpl : IDataList
   {
      // Internal data declarations
      private bool _loaded;
      private bool _cache;
      private List<KeyValue> _values;

      /// <summary>Define el separador de los valores múltiples en las etiquetas de cada valor.</summary>
      public const string LABEL_VALUES_SEPARATOR = "|||";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="SqlDataListImpl"/>.
      /// </summary>
      /// <param name="workspace">An instance of current Cosmo workspace</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene los datos del módulo y su configuración.</param>
      public SqlDataListImpl(Workspace workspace, Plugin plugin) 
         : base(workspace, plugin)
      {
         Initialize();

         _cache = plugin.GetBoolean("cache", true);
         
         this.SqlQuery = plugin.GetString("sql");
         this.DataModuleID = plugin.GetString("connection");
         this.DefaultValue = plugin.GetString("default.value");
      }

      #endregion

      #region IDataList Implementation

      /// <summary>
      /// Gets or sets la lista de valores que contiene la lista.
      /// </summary>
      public override List<KeyValue> Values
      {
         get
         {
            LoadData();

            return _values;
         }
         set { _values = value; }
      }

      /// <summary>
      /// Obtiene el valor asociado a una determinada clave.
      /// </summary>
      /// <param name="key">Clave para la que se desea obtener el valor.</param>
      /// <returns>Una cadena que contiene el valor asociado a la clave proporcionada.</returns>
      public override string GetValueByKey(string key)
      {
         LoadData();

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

      #region Properties

      /// <summary>
      /// Gets or sets el identificador único del <see cref="IDataModule"/> que se usará para obtener los datos de una lista dinámica.
      /// </summary>
      public string DataModuleID { get; set; }

      /// <summary>
      /// Gets or sets la senténcia SQL que usará una lista dinámica para obtener los datos.
      /// </summary>
      /// <remarks>
      /// El primer campo devuelto en la consulta corresponde al valor y el resto a la/s etiqueta/s.
      /// </remarks>
      public string SqlQuery { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Carga los valores desde la base de datos.
      /// </summary>
      public void LoadData()
      {
         KeyValue value;
         SqlCommand cmd = null;

         // Evita recargas innecesareas
         if (_loaded) return;

         // Resetea la lista
         Values = new List<KeyValue>();

         try
         {
            Workspace.DataService.GetDataSource(this.DataModuleID).Connect();

            cmd = new SqlCommand(this.SqlQuery, Workspace.DataService.GetDataSource(this.DataModuleID).Connection);
            // cmd.Parameters.Add(new SqlParameter("@id", folderId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  value = new KeyValue();
                  value.Value = reader[0].ToString();

                  for (int i = 1; i < reader.FieldCount; i++)
                  {
                     value.Label = (string.IsNullOrEmpty(value.Label) ? string.Empty : LABEL_VALUES_SEPARATOR) + reader[i].ToString().Trim();
                  }

                  _values.Add(value);
               }
            }

            _loaded = true;
         }
         catch (Exception ex)
         {
            Workspace.Logger.Add(new LogEntry(GetType().Name + ".LoadData()",
                                              ex.Message,
                                              LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            Workspace.DataService.GetDataSource(this.DataModuleID).Disconnect();
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.DataModuleID = string.Empty;
         this.SqlQuery = string.Empty;

         _values = new List<KeyValue>();
         _cache = true;
         _loaded = false;
      }

      #endregion

   }
}
