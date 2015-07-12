using Cosmo.Utils;
using System.Data.SqlClient;

namespace Cosmo.Data.Connection
{
   /// <summary>
   /// Declara el interface que deben implementar los DataSources.
   /// </summary>
   public abstract class IDataModule
   {
      /// <summary>Clave de la propiedad de configuración del plugin que contiene el servidor SQL Server.</summary>
      public static string SETTING_DB_SERVER = "db.server";
      /// <summary>Clave de la propiedad de configuración del plugin que contiene el esquema de base de datos.</summary>
      public static string SETTING_DB_SCHEMA = "db.schema";
      /// <summary>Clave de la propiedad de configuración del plugin que contiene el login de la cuenta de acceso.</summary>
      public static string SETTING_DB_LOGIN = "db.login";
      /// <summary>Clave de la propiedad de configuración del plugin que contiene el la contraseña de la cuenta de acceso.</summary>
      public static string SETTING_DB_PASSWORD = "db.password";
      
      // Internal data declarations
      private Plugin _plugin;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="IDataModule"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      protected IDataModule(Workspace workspace, Plugin plugin)
      {
         Initialize();

         _plugin = plugin;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el nombre (ID) del módulo de datos.
      /// </summary>
      public string ID
      {
         get { return _plugin.ID; }
      }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Gets or sets la conexión a la base de datos.
      /// </summary>
      public abstract SqlConnection Connection  { get; set; }

      /// <summary>
      /// Inicializa una conexión con la base de datos del Workspace.
      /// </summary>
      public abstract void Connect();

      /// <summary>
      /// Cierra la conexión a la base de datos del Workspace.
      /// </summary>
      public abstract void Disconnect();
 
      /// <summary>
      /// Testea una conexión a base de datos.
      /// </summary>
      /// <returns><c>true</c> si la conexión se establece correctamente o <c>false</c> en cualquier otro caso.</returns>
      public abstract bool TestConnection();

      #endregion

      #region Static Members

      /// <summary>
      /// Cierra i dispone un objeto de tipo <see cref="SqlDataReader"/>.
      /// </summary>
      public static void CloseAndDispose(SqlDataReader reader)
      {
         try
         {
            reader.Close();
         }
         catch
         {
            // Nothing to do
         }

         try
         {
            reader.Dispose();
         }
         catch
         {
            // Nothing to do
         }
      }

      /// <summary>
      /// Cierra i dispone un objeto de tipo <see cref="SqlCommand"/>.
      /// </summary>
      public static void CloseAndDispose(SqlCommand command)
      {
         try
         {
            command.Dispose();
         }
         catch
         {
            // Nothing to do
         }
      }

      /// <summary>
      /// Close and dispose a <see cref="SqlDataAdapter"/> instance.
      /// </summary>
      public static void CloseAndDispose(SqlDataAdapter dataAdapter)
      {
         try
         {
            dataAdapter.Dispose();
         }
         catch
         {
            // Nothing to do
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _plugin = null;
      }

      #endregion

   }
}
