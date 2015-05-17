
namespace Cosmo.Utils.Json
{

   #region class JSONValue

   /// <summary>
   /// Representa un valor genérico.
   /// </summary>
   public class JSONValue
   {
      private string _name;
      private object _value;

      /// <summary>
      /// Devuelve una instancia de JSONValue.
      /// </summary>
      public JSONValue() { }

      /// <summary>
      /// Devuelve una instancia de JSONValue.
      /// </summary>
      public JSONValue(string name, object value) 
      {
         _name = name;
         _value = value;
      }

      /// <summary>
      /// Nombre del valor.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Valor.
      /// </summary>
      public object Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Devuelve el valor en formato JSON.
      /// </summary>
      /// <returns>Una cadena en formato JSON que representa el valor.</returns>
      public string Writer()
      {
         string json = "";

         json += JSONWriter.Enquote(_name) + ":";

         if (_value.GetType() == typeof(bool))
            json += "'" + ((bool)_value).ToString().ToLower() + "'";
         else if (_value.GetType() == typeof(int))
            json += "'" + ((int)_value).ToString() + "'";
         else if (_value.GetType() == typeof(string))
            json += "'" + JSONWriter.Enquote((string)_value) + "'";

         return json;
      }
   }

   #endregion

   #region class JSONStringValue

   /// <summary>
   /// Representa un valor de cadena de texto.
   /// </summary>
   public class JSONStringValue
   {
      private string _name;
      private string _value;

      /// <summary>
      /// Nombre del valor.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Valor.
      /// </summary>
      public string Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Devuelve el valor en formato JSON.
      /// </summary>
      /// <returns>Una cadena en formato JSON que representa el valor.</returns>
      public string Writer()
      {
         return JSONWriter.Enquote(_name) + ":'" + JSONWriter.Enquote(_value) + "'";
      }
   }

   #endregion

   #region class JSONIntegerValue

   /// <summary>
   /// Representa un valor de cadena de texto.
   /// </summary>
   public class JSONIntegerValue
   {
      private string _name;
      private int _value;

      /// <summary>
      /// Nombre del valor.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Valor.
      /// </summary>
      public int Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Devuelve el valor en formato JSON.
      /// </summary>
      /// <returns>Una cadena en formato JSON que representa el valor.</returns>
      public string Writer()
      {
         return JSONWriter.Enquote(_name) + ":'" + _value + "'";
      }
   }

   #endregion

   #region class JSONBooleanValue

   /// <summary>
   /// Representa un valor booleano.
   /// </summary>
   public class JSONBooleanValue
   {
      private string _name;
      private bool _value;

      /// <summary>
      /// Nombre del valor.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Valor.
      /// </summary>
      public bool Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Devuelve el valor en formato JSON.
      /// </summary>
      /// <returns>Una cadena en formato JSON que representa el valor.</returns>
      public string Writer()
      {
         return JSONWriter.Enquote(_name) + ":'" + _value.ToString().ToLower() + "'";
      }
   }

   #endregion

}
