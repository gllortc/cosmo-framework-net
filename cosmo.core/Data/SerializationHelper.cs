using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Cosmo.Data
{

   #region Enumerations

   /// <summary>
   /// Enumeration containing all the available serialization modes for the <see cref="DataObjectBase{T}"/> class.
   /// </summary>
   public enum SerializationMode
   {
      /// <summary>
      /// Serialize using the XmlSerializer.
      /// </summary>
      Xml,

      /// <summary>
      /// Serialize using the BinaryFormatter.
      /// </summary>
      Binary
   }

   #endregion

   /// <summary>
   /// Class that makes serialization much easier and safer.
   /// </summary>
   public static class SerializationHelper
   {
      /// <summary>
      /// Retrieves a string from a SerializationInfo object.
      /// </summary>
      /// <param name="info">SerializationInfo object.</param>
      /// <param name="name">Name of the value to retrieve.</param>
      /// <param name="defaultValue">Default value when value does not exist.</param>
      /// <returns>String value.</returns>
      public static string GetString(SerializationInfo info, string name, string defaultValue)
      {
         // Get object
         return GetObject(info, name, defaultValue);
      }

      /// <summary>
      /// Retrieves an integer from a SerializationInfo object.
      /// </summary>
      /// <param name="info">SerializationInfo object</param>
      /// <param name="name">Name of the value to retrieve.</param>
      /// <param name="defaultValue">Default value when value does not exist.</param>
      /// <returns>Integer value.</returns>
      public static int GetInt(SerializationInfo info, string name, int defaultValue)
      {
         // Get object
         return GetObject(info, name, defaultValue);
      }

      /// <summary>
      /// Retrieves a boolean from a SerializationInfo object.
      /// </summary>
      /// <param name="info">SerializationInfo object.</param>
      /// <param name="name">Name of the value to retrieve.</param>
      /// <param name="defaultValue">Default value when value does not exist.</param>
      /// <returns>Boolean value.</returns>
      public static bool GetBool(SerializationInfo info, string name, bool defaultValue)
      {
         // Get object
         return GetObject(info, name, defaultValue);
      }

      /// <summary>
      /// Retrieves an object from a SerializationInfo object.
      /// </summary>
      /// <typeparam name="T">Type of the value to read from the serialization information.</typeparam>
      /// <param name="info">SerializationInfo object.</param>
      /// <param name="name">Name of the value to retrieve.</param>
      /// <param name="defaultValue">Default value when value does not exist.</param>
      /// <returns>Object value.</returns>
      public static T GetObject<T>(SerializationInfo info, string name, T defaultValue)
      {
         // Get the type
         Type type = typeof(T);

         // Get the value
         object value = GetObject(info, name, type, defaultValue);

         // Return result
         return ((value != null) && (value is T)) ? (T)value : defaultValue;
      }

      /// <summary>
      /// Retrieves an object from a SerializationInfo object.
      /// </summary>
      /// <param name="info">SerializationInfo object.</param>
      /// <param name="name">Name of the value to retrieve.</param>
      /// <param name="type">Type of the object to retrieve.</param>
      /// <param name="defaultValue">Default value when value does not exist.</param>
      /// <returns>Object value.</returns>
      public static object GetObject(SerializationInfo info, string name, Type type, object defaultValue)
      {
         try
         {
            object obj = info.GetValue(name, type);
            return obj ?? defaultValue;
         }
         catch (Exception)
         {
            return defaultValue;
         }
      }
   }
}
