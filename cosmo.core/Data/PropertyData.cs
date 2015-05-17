using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Cosmo.Data
{
   /// <summary>
   /// Object that contains all the property data that is used by the <see cref="DataObjectBase{T}"/> class.
   /// </summary>
   public class PropertyData
   {
      [field: NonSerialized]
      private Type _type;

      /// <summary>
      /// Initializes a new instance of this object.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <param name="type">Type of the property.</param>
      /// <param name="defaultValue">Default value of the property.</param>
      /// <param name="propertyChangedEventHandler">The property changed event handler.</param>
      internal PropertyData(string name, Type type, object defaultValue, PropertyChangedEventHandler propertyChangedEventHandler)
      {
         // Store values
         Name = name;
         Type = type;
         DefaultValue = defaultValue;
         PropertyChangedEventHandler = propertyChangedEventHandler;
      }

      #region Properties

      /// <summary>
      /// Gets the name of the property.
      /// </summary>
      public string Name { get; private set; }

      /// <summary>
      /// Gets the type of the property.
      /// </summary>
      [XmlIgnore]
      public Type Type
      {
         get { return _type ?? typeof(object); }
         private set { _type = value; }
      }

      /// <summary>
      /// Gets or sets the default value of the property.
      /// </summary>
      private object DefaultValue { get; set; }

      /// <summary>
      /// Gets or sets the property changed event handler.
      /// </summary>
      /// <value>The property changed event handler.</value>
      [XmlIgnore]
      internal PropertyChangedEventHandler PropertyChangedEventHandler { get; private set; }

      #endregion

      #region Methods

      /// <summary>
      /// Returns the default value of the property.
      /// </summary>
      /// <returns>Default value of the property.</returns>
      public object GetDefaultValue()
      {
         return DefaultValue;
      }

      /// <summary>
      /// Returns the typed default value of the property.
      /// </summary>
      /// <returns>Default value of the property.</returns>
      public T GetDefaultValue<T>()
      {
         return ((DefaultValue != null) && (DefaultValue is T)) ? (T)DefaultValue : default(T);
      }

      #endregion

   }
}
