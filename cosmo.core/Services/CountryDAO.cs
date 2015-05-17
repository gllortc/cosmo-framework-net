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
      private Workspace _ws;

      /// <summary>
      /// Devuelve una instancia de <see cref="CountryDAO"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      public CountryDAO(Workspace workspace)
      {
         Initialize();

         _ws = workspace;
      }

      /// <summary>
      /// Devuelve una lista de paises
      /// </summary>
      public List<Country> GetCountryList()
      {
         Country country = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         List<Country> countries = new List<Country>();

         try
         {
            // Abre una conexión a la BBDD
            _ws.DataSource.Connect();

            string sql = "SELECT countryid,countryname,countrylstdef FROM country ORDER BY countryname ASC";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               country = new Country();
               country.ID = reader.GetInt32(0);
               country.Name = reader.GetString(1);
               country.Default = reader.GetBoolean(2);

               countries.Add(country);
            }
            reader.Close();

            return countries;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(this.GetType().Name + ".ListCountry", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            if (!reader.IsClosed) reader.Close();

            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
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
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            // Rellena el control
            string sql = "SELECT * FROM country ORDER BY countryname ASC";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            reader = cmd.ExecuteReader();
            list.DataSource = reader;
            list.DataTextField = "countryname";
            list.DataValueField = "countryid";
            list.DataBind();
            reader.Close();

            // Preselecciona el elemento
            if (selectedId <= 0)
            {
               sql = "SELECT Top 1 countryid FROM country WHERE countrylstdef=1";
               cmd = new SqlCommand(sql, _ws.DataSource.Connection);
               reader = cmd.ExecuteReader();
               if (reader.Read())
               {
                  id = (int)reader["countryid"];
                  if (id > 0) list.Items.FindByValue(id.ToString()).Selected = true;
               }
               reader.Close();
            }
            else
            {
               list.Items.FindByValue(selectedId.ToString()).Selected = true;
            }
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CSWebsite.CreateCountryDropDownList(DropDownList, int)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            if (!reader.IsClosed) reader.Close();
            reader.Dispose();
            cmd.Dispose();

            _ws.DataSource.Disconnect();
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
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            // Rellena el control
            string sql = "SELECT countryname FROM country WHERE countryid=@countryid";
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@countryid", countryId));

            return (string)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry("CSWebsite.GetCountryName(int)", ex.Message, LogEntry.LogEntryType.EV_ERROR));
            throw;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      public void Initialize()
      {
         _ws = null;
      }
   }
}
