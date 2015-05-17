using System.Data.SqlClient;

namespace Cosmo.Data
{
   /// <summary>
   /// Define una clase que deben extender los drivers específicos para cada SGBD.
   /// </summary>
   public abstract class DataAgent
   {
      // Declaración de variables internas
      private SqlConnection _dbconn;

      /// <summary>
      /// Devuelve una instancia se <see cref="DataAgent"/>.
      /// </summary>
      public DataAgent()
      {
         _dbconn = null;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece la conexión a base de datos.
      /// </summary>
      public SqlConnection Connection
      {
         get { return _dbconn; }
         set { _dbconn = value; }
      }

      /// <summary>
      /// Indica si la conexión está abierta (<c>true</c>) o no (<c>false</c>).
      /// </summary>
      public bool IsConnected
      {
         get 
         {
            if (_dbconn == null)
            {
               return false;
            }

            if (_dbconn.State == System.Data.ConnectionState.Broken || _dbconn.State == System.Data.ConnectionState.Closed)
            {
               return false;
            }

            return true;
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Abre una conexión con la base de datos usando el mecanismo implementado por el driver específico.
      /// <br /><br />
      /// Este método lo implementa cada <em>driver</em> dado que cada tipo de SGBD puede tener formas de conexión distintas. 
      /// Los datos de la conexión deben estar contenidos en las propiedades específicas del agente.
      /// </summary>
      public abstract void Connect();

      /// <summary>
      /// Cierra la conexión con la base de datos.
      /// <br /><br />
      /// Este método lo implementa cada <em>driver</em> dado que cada tipo de SGBD puede tener formas de desconexión distintas. 
      /// Los datos de la conexión deben estar contenidos en las propiedades específicas del agente.
      /// </summary>
      public abstract void Disconnect();

      #endregion

   }
}
