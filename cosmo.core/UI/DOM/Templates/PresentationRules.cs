using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Cosmo.UI.DOM.Templates
{

   /// <summary>
   /// Implementa una clase de servicio para la gestión de las reglas de presentación
   /// </summary>
   public class PresentationRules
   {
      private Workspace _workspace = null;

      /// <summary>
      /// Devuelve un a instancia de PresentationRules
      /// </summary>
      /// <param name="workspace">Una instancia del workspace</param>
      public PresentationRules(Workspace workspace)
      {
          _workspace = workspace;
      }

      #region Methods

      /// <summary>
      /// Genera una lista de los objetos.
      /// </summary>
      /// <param name="folderid">Identificador de la carpeta para la que se desea obtener la lista.</param>
      public List<PresentationRule> List(int folderid)
      {
         SqlDataReader reader = null;
         SqlCommand cmd = null;
         List<PresentationRule> devices = new List<PresentationRule>();

         try
         {
            // Abre una conexión a la BBDD
            _workspace.DataSource.Connect();

            string sql = "SELECT id,agent,devicetype,formatid,priority,redirecturl,frdesc,frdownload,frupload,frjavascript,frcookies,frdefault " +
                         "FROM sysformatrules " +
                         "WHERE devicetype=@devicetype " +
                         "ORDER BY priority ASC, agent ASC";

            cmd = new SqlCommand(sql, _workspace.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@devicetype", folderid));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               PresentationRule rule = new PresentationRule();
               rule.ID = reader.GetInt32(0);
               rule.Agent = reader.IsDBNull(1) ? "" : reader.GetString(1);
               rule.Type = reader.IsDBNull(2) ? PresentationRule.DeviceTypes.Unknown : (PresentationRule.DeviceTypes)reader.GetInt32(2);
               rule.TemplateID = reader.GetInt32(3);
               rule.Priority = reader.GetInt32(4);
               rule.RedirectToURL = reader.IsDBNull(5) ? "" : reader.GetString(5);
               rule.Name = reader.IsDBNull(6) ? "" : reader.GetString(6);
               rule.CanDownloadFiles = reader.GetBoolean(7);
               rule.CanUploadFiles = reader.GetBoolean(8);
               rule.CanExecuteJavaScript = reader.GetBoolean(9);
               rule.CanAcceptCookies = reader.GetBoolean(10);
               rule.Default = reader.GetBoolean(11);

               devices.Add(rule);
            }
            reader.Close();

            return devices;
         }
         catch
         {
            throw;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Permite recuperar los datos de una regla de presentación.
      /// </summary>
      /// <param name="id">Identificador de la regla.</param>
      /// <returns>Una instáncia de WSDevice.</returns>
      public PresentationRule Item(int id)
      {
         PresentationRule rule = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            // Abre una conexión a la BBDD del workspace
            _workspace.DataSource.Connect();

            string sql = "SELECT agent,devicetype,formatid,priority,redirecturl,frdesc,frdownload,frupload,frjavascript,frcookies,frdefault " +
                         "FROM sysformatrules " +
                         "WHERE id=@id";
            cmd = new SqlCommand(sql, _workspace.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters["@id"].Value = id;

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               rule = new PresentationRule();
               rule.ID = id;
               rule.Agent = reader.IsDBNull(0) ? "" : (string)reader[0];
               rule.Type = reader.IsDBNull(1) ? PresentationRule.DeviceTypes.Unknown : (PresentationRule.DeviceTypes)reader[1];
               rule.TemplateID = (int)reader[2];
               rule.Priority = (int)reader[3];
               rule.RedirectToURL = reader.IsDBNull(4) ? "" : (string)reader[4];
               rule.Name = reader.IsDBNull(5) ? "" : (string)reader[5];
               rule.CanDownloadFiles = (bool)reader[6];
               rule.CanUploadFiles = (bool)reader[7];
               rule.CanExecuteJavaScript = (bool)reader[8];
               rule.CanAcceptCookies = (bool)reader[9];
               rule.Default = (bool)reader[10];
            }
            reader.Close();

            return rule;
         }
         catch
         {
            throw;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Crea una nueva regla de presentación.
      /// </summary>
      /// <param name="rule">Instáncia de PresentationRule que contiene los detalles de la regla.</param>
      public void Add(PresentationRule rule)
      {
          string sql;
          SqlCommand cmd = null;
          SqlTransaction transaction = null;

          rule.Agent = rule.Agent.Trim().ToLower();
          rule.Name = rule.Name.Trim();
          rule.RedirectToURL = rule.RedirectToURL.Trim().ToLower();

          try
          {
             // Abre una conexión a la BBDD del workspace
             _workspace.DataSource.Connect();
             transaction = _workspace.DataSource.Connection.BeginTransaction();

             // Averigua si ya existe una regla con el mismo agente
             sql = "SELECT Count(*) AS nregs FROM sysformatrules WHERE Lower(agent)=@agent";
             cmd = new SqlCommand(sql, _workspace.DataSource.Connection, transaction);
             cmd.Parameters.Add(new SqlParameter("@agent", rule.Agent));

             if ((int)cmd.ExecuteScalar() > 0)
                 throw new Exception("Ya existe una regla de presentación que contiene el identificador \"" + rule.Agent + "\".");

             // Si es una regla por defecto, se asegura que no habrá otra activada
             if (rule.Default)
             {
                 sql = "UPDATE sysformatrules SET frdefault=0";
                 cmd = new SqlCommand(sql, _workspace.DataSource.Connection, transaction);
                 cmd.ExecuteNonQuery();
             }

             // Añade el registro a la tabla SYSFORMATRULES
             sql = "INSERT INTO sysformatrules (agent,devicetype,formatid,priority,redirecturl,frdesc,frdownload,frupload,frjavascript,frcookies,frdefault) " +
                   "VALUES (@agent,@devicetype,@formatid,@priority,@redirecturl,@frdesc,@frdownload,@frupload,@frjavascript,@frcookies,@frdefault)";
             cmd = new SqlCommand(sql, _workspace.DataSource.Connection, transaction);

             cmd.Parameters.Add(new SqlParameter("@agent", rule.Agent));
             cmd.Parameters.Add(new SqlParameter("@devicetype", (int)rule.Type));
             cmd.Parameters.Add(new SqlParameter("@formatid", rule.TemplateID));
             cmd.Parameters.Add(new SqlParameter("@priority", rule.Priority));
             cmd.Parameters.Add(new SqlParameter("@redirecturl", rule.RedirectToURL));
             cmd.Parameters.Add(new SqlParameter("@frdesc", rule.Name));
             cmd.Parameters.Add(new SqlParameter("@frdownload", rule.CanDownloadFiles));
             cmd.Parameters.Add(new SqlParameter("@frupload", rule.CanUploadFiles));
             cmd.Parameters.Add(new SqlParameter("@frjavascript", rule.CanExecuteJavaScript));
             cmd.Parameters.Add(new SqlParameter("@frcookies", rule.CanAcceptCookies));
             cmd.Parameters.Add(new SqlParameter("@frdefault", rule.Default));
             cmd.ExecuteNonQuery();

             // Averigua el ID del usuario
             sql = "SELECT id FROM sysformatrules WHERE Lower(agent)=@agent";
             cmd = new SqlCommand(sql, _workspace.DataSource.Connection, transaction);
             cmd.Parameters.Add(new SqlParameter("@agent", rule.Agent));
             rule.ID = (int)cmd.ExecuteScalar();

             // Cierra la conexión a la BBDD
             transaction.Commit();
          }
          catch
          {
             if (transaction != null) transaction.Rollback();
             throw;
          }
          finally
          {
             cmd.Dispose();
             transaction.Dispose();
             _workspace.DataSource.Disconnect();
          }
      }

      /// <summary>
      /// Actualiza los datos de una regla de presentación.
      /// </summary>
      /// <param name="rule">Instáncia de PresentationRule que contiene los datos actualizados de la regla.</param>
      public void Update(PresentationRule rule)
      {
         string sql;
         SqlCommand cmd = null;
         SqlTransaction transaction = null;

         rule.Agent = rule.Agent.Trim().ToLower();
         rule.Name = rule.Name.Trim();
         rule.RedirectToURL = rule.RedirectToURL.Trim().ToLower();

         try
         {
            // Abre una conexión a la BBDD del workspace
            _workspace.DataSource.Connect();
            transaction = _workspace.DataSource.Connection.BeginTransaction();

            // Averigua si ya existe una regla con el mismo agente
            sql = "SELECT Count(*) AS nregs FROM sysformatrules WHERE Lower(agent)=@agent And id<>@id";

            cmd = new SqlCommand(sql, _workspace.DataSource.Connection, transaction);
            cmd.Parameters.Add(new SqlParameter("@agent", rule.Agent));
            cmd.Parameters.Add(new SqlParameter("@id", rule.ID));
            if ((int)cmd.ExecuteScalar() > 0)
                throw new Exception("Ya existe una regla de presentación que contiene el identificador \"" + rule.Agent + "\".");

            // Si es una regla por defecto, se asegura que no habrá otra activada
            if (rule.Default)
            {
               sql = "UPDATE sysformatrules SET frdefault=0";
               cmd = new SqlCommand(sql, _workspace.DataSource.Connection, transaction);
               cmd.ExecuteNonQuery();
            }

            // Añade el registro a la tabla USERS
            sql = "UPDATE sysformatrules " +
                  "SET agent=@agent," +
                      "devicetype=@devicetype," +
                      "formatid=@formatid," +
                      "priority=@priority," +
                      "redirecturl=@redirecturl," +
                      "frdesc=@frdesc," +
                      "frdownload=@frdownload," +
                      "frupload=@frupload," +
                      "frjavascript=@frjavascript," +
                      "frcookies=@frcookies," +
                      "frdefault=@frdefault " +
                  "WHERE id=@id";
            cmd = new SqlCommand(sql, _workspace.DataSource.Connection, transaction);

            cmd.Parameters.Add(new SqlParameter("@agent", rule.Agent));
            cmd.Parameters.Add(new SqlParameter("@devicetype", (int)rule.Type));
            cmd.Parameters.Add(new SqlParameter("@formatid", rule.TemplateID));
            cmd.Parameters.Add(new SqlParameter("@priority", rule.Priority));
            cmd.Parameters.Add(new SqlParameter("@redirecturl", rule.RedirectToURL));
            cmd.Parameters.Add(new SqlParameter("@frdesc", rule.Name));
            cmd.Parameters.Add(new SqlParameter("@frdownload", rule.CanDownloadFiles));
            cmd.Parameters.Add(new SqlParameter("@frupload", rule.CanUploadFiles));
            cmd.Parameters.Add(new SqlParameter("@frjavascript", rule.CanExecuteJavaScript));
            cmd.Parameters.Add(new SqlParameter("@frcookies", rule.CanAcceptCookies));
            cmd.Parameters.Add(new SqlParameter("@frdefault", rule.Default));
            cmd.Parameters.Add(new SqlParameter("@id", rule.ID));
            cmd.ExecuteNonQuery();

            transaction.Commit();
         }
         catch
         {
            transaction.Rollback();
            throw;
         }
         finally
         {
            cmd.Dispose();
            transaction.Dispose();
            _workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina una regla de presentación.
      /// </summary>
      /// <param name="id">Identificador de la regla.</param>
      public void Delete(int id)
      {
          SqlCommand cmd = null;

          try
          {
             // Abre una conexión a la BBDD del workspace
             _workspace.DataSource.Connect();

             // Elimina el registro
             string sql = "DELETE FROM sysformatrules WHERE id=@id";

             cmd = new SqlCommand(sql, _workspace.DataSource.Connection);
             cmd.Parameters.Add(new SqlParameter("@id", id));
             cmd.ExecuteNonQuery();
          }
          catch
          {
             throw;
          }
          finally
          {
             cmd.Dispose();
             _workspace.DataSource.Disconnect();
          }
      }

      #endregion

   }
}

