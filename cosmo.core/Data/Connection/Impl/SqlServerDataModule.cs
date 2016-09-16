using Cosmo.Utils;
using System.Data;
using System.Data.SqlClient;

namespace Cosmo.Data.Connection.Impl
{
   /// <summary>
   /// Representa una conexión a base de datos.
   /// </summary>
   public class SqlServerDataModule : DataModule
   {
      // Internal data declarations
      private string _dsn;
      private SqlConnection _dbconn;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="SqlServerDataModule"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public SqlServerDataModule(Workspace workspace, Plugin plugin) 
         : base(workspace, plugin)
      {
         this.ServerName =  plugin.GetString(DataModule.SETTING_DB_SERVER);
         this.DatabaseName = plugin.GetString(DataModule.SETTING_DB_SCHEMA);
         this.AccountLogin = plugin.GetString(DataModule.SETTING_DB_LOGIN);
         this.AccountPassword = plugin.GetString(DataModule.SETTING_DB_PASSWORD);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el nombre del servidor de base de datos.
      /// </summary>
      public string ServerName { get; set; }

      /// <summary>
      /// Gets or sets el nombre de la base de datos.
      /// </summary>
      public string DatabaseName { get; set; }

      /// <summary>
      /// Gets or sets el login de la cuenta de acceso.
      /// </summary>
      public string AccountLogin { get; set; }

      /// <summary>
      /// Gets or sets la contraseña de la cuenta de acceso.
      /// </summary>
      public string AccountPassword { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve la cadena de conexión a la base de datos del workspace
      /// </summary>
      /// <returns>La cadena de conexión.</returns>
      public string GetDSN()
      {
         return "Data Source=" + this.ServerName.Trim().ToUpper() + ";" +
                "Initial Catalog=" + this.DatabaseName.Trim().ToUpper() + ";" +
                "User ID=" + this.AccountLogin.Trim().ToUpper() + ";" +
                "Password=" + this.AccountPassword;
      }

      #endregion

      #region IDataModule Implementation

      /// <summary>
      /// Gets or sets la conexión a la base de datos.
      /// </summary>
      public override SqlConnection Connection
      {
         get { return _dbconn; }
         set { _dbconn = value; }
      }

      /// <summary>
      /// Inicializa una conexión con la base de datos del Workspace.
      /// </summary>
      public override void Connect()
      {
         try
         {
            // Comprueba si la conexión ya está abierta
            if (_dbconn != null)
            {
               if (_dbconn.State == ConnectionState.Open)
               {
                  return;
               }
            }

            // Inicializa la conexión
            if (string.IsNullOrEmpty(_dsn)) _dsn = this.GetDSN();
            _dbconn = new SqlConnection(_dsn);
            _dbconn.Open();
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Cierra la conexión a la base de datos del Workspace.
      /// </summary>
      public override void Disconnect()
      {
         try
         {
            if (_dbconn == null) return;
            if (_dbconn.State == ConnectionState.Open) _dbconn.Close();
            // _dbconn.Dispose();
         }
         catch
         {
            // Nothing to do
         }
      }

      /// <summary>
      /// Testea una conexión a base de datos.
      /// </summary>
      /// <returns><c>true</c> si la conexión se establece correctamente o <c>false</c> en cualquier otro caso.</returns>
      public override bool TestConnection()
      {
         try
         {
            // Genera una conexión
            SqlConnection conn = new SqlConnection(this.GetDSN());
            conn.Open();
            conn.Close();
            conn.Dispose();

            return true;
         }
         catch
         {
            return false;
         }
      }

      #endregion

   }
}
