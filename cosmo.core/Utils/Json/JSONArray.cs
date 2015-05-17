using System.Collections.Generic;
using System.Data.SqlClient;

namespace Cosmo.Utils.Json
{

   #region class JSONArray : List<object>

   /// <summary>
   /// Implementa un array de valores en foprmato JSON.
   /// </summary>
   /// <remarks>
   /// La lista puede contener objetos del tipo:
   ///  - JSONValue
   ///  - JSONArray
   /// </remarks>
   public class JSONArray : List<object>
   {
      /// <summary>
      /// Devuelve una cadena en formato JSON que representa el array.
      /// </summary>
      public string Writer()
      {
         string json = "";

         json += "[";
         foreach (JSONValue value in this)
         {
            json += value.Writer() + ",";
         }
         json = json.Substring(0, json.Length - 1);
         json += "]";

         return json;
      }

      /// <summary>
      /// Rellena un array JSON con el resultado de una consulta SQL.
      /// </summary>
      /// <param name="reader">Los datos devueltos por una consulta.</param>
      public void Fill(SqlDataReader reader)
      {
         while (reader.Read())
         {
            JSONArray row = new JSONArray();

            for (int i = 0; i < reader.VisibleFieldCount; i++)
            {
               JSONValue value = new JSONValue();
               value.Name = reader.GetName(i);
               value.Value = reader.GetValue(i);
            }

            this.Add(row);
         }
      }
   }

   #endregion

   #region class JSONObject : List<object>

   /// <summary>
   /// Implementa un array de valores en formato JSON.
   /// </summary>
   /// <remarks>
   /// La lista puede contener objetos del tipo:
   ///  - JSONValue
   ///  - JSONArray
   /// </remarks>
   public class JSONObject : List<object>
   {
      /// <summary>
      /// Devuelve una cadena en formato JSON que representa el array.
      /// </summary>
      public string Writer()
      {
         string json = "";

         json += "{";
         foreach (JSONValue value in this)
         {
            json += value.Writer() + ",";
         }
         json = json.Substring(0, json.Length - 1);
         json += "}";

         return json;
      }
   }

   #endregion

}
