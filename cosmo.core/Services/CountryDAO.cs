using Cosmo.Data.Connection;
using Cosmo.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Cosmo.Services
{
   /// <summary>
   /// Gestor de la lista de paises.
   /// </summary>
   public class CountryDAO
   {
      // Internal data declarations
      private Workspace workspace;

      // SQL definitions
      private const string SQL_COUNTRY_TABLE = "country";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="CountryDAO"/>.
      /// </summary>
      /// <param name="workspace">An instance of current Cosmo workspace</param>
      public CountryDAO(Workspace workspace)
      {
         Initialize();

         workspace = workspace;
      }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve una lista de paises
      /// </summary>
      public List<Country> GetCountryList()
      {
         string sql = string.Empty;
         Country country = null;
         SqlCommand cmd = null;
         List<Country> countries = new List<Country>();

         try
         {
            // Abre una conexión a la BBDD
            workspace.DataSource.Connect();

            sql = @"SELECT    countryid, countryname, countrylstdef 
                    FROM      " + SQL_COUNTRY_TABLE + @" 
                    ORDER BY  countryname ASC";

            cmd = new SqlCommand(sql, workspace.DataSource.Connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               while (reader.Read())
               {
                  country = new Country();
                  country.ID = reader.GetInt32(0);
                  country.Name = reader.GetString(1);
                  country.Default = reader.GetBoolean(2);

                  countries.Add(country);
               }
            }

            return countries;
         }
         catch (Exception ex)
         {
            workspace.Logger.Add(new LogEntry(this.GetType().Name + ".ListCountry", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));

            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Rellena un control DropDown con una lista de paises
      /// </summary>
      /// <param name="list">Control a rellenar</param>
      /// <param name="selectedId">Identificador del pais seleccionado por defecto</param>
      /// <returns>Un valor booleano que indica el resultado de la operación</returns>
      public void CreateCountryDropDownList(DropDownList list, int selectedId)
      {
         int id = 0;
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            workspace.DataSource.Connect();

            // Rellena el control
            sql = @"SELECT    * 
                    FROM      " + SQL_COUNTRY_TABLE + @" 
                    ORDER BY  countryname ASC";

            cmd = new SqlCommand(sql, workspace.DataSource.Connection);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
               list.DataSource = reader;
               list.DataTextField = "countryname";
               list.DataValueField = "countryid";
               list.DataBind();
            }

            // Preselecciona el elemento
            if (selectedId <= 0)
            {
               sql = @"SELECT Top 1 countryid 
                       FROM   " + SQL_COUNTRY_TABLE + @" 
                       WHERE  countrylstdef = 1";

               cmd = new SqlCommand(sql, workspace.DataSource.Connection);
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  if (reader.Read())
                  {
                     id = (int)reader["countryid"];
                     if (id > 0) list.Items.FindByValue(id.ToString()).Selected = true;
                  }
               }
            }
            else
            {
               list.Items.FindByValue(selectedId.ToString()).Selected = true;
            }
         }
         catch (Exception ex)
         {
            workspace.Logger.Add(new LogEntry(this.GetType().Name + ".CreateCountryDropDownList(DropDownList, int)", 
                                        ex.Message, 
                                        LogEntry.LogEntryType.EV_ERROR));

            throw ex;
         }
         finally
         {
            IDataModule.CloseAndDispose(cmd);
            workspace.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Rellena un control DropDown con una lista de paises.
      /// </summary>
      /// <param name="list">Control a rellenar.</param>
      /// <returns>Un valor booleano que indica el resultado de la operación.</returns>
      public void CreateCountryDropDownList(DropDownList list)
      {
         this.CreateCountryDropDownList(list, -1);
      }

      /// <summary>
      /// Obtiene el nombre de un pais a partir de su identificador
      /// </summary>
      /// <param name="countryId">Identificador del pais</param>
      /// <returns>Una cadena que contiene el nombre del pais</returns>
      public string GetCountryName(int countryId)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            workspace.DataSource.Connect();

            // Rellena el control
            sql = @"SELECT countryname 
                    FROM   " + SQL_COUNTRY_TABLE + @" 
                    WHERE countryid = @countryid";

            cmd = new SqlCommand(sql, workspace.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@countryid", countryId));

            return (string)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            workspace.Logger.Add(new LogEntry(this.GetType().Name + ".GetCountryName(int)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            workspace.DataSource.Disconnect();
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance.
      /// </summary>
      public void Initialize()
      {
         workspace = null;
      }

      #endregion

   }
}
