//
// http://www.codeproject.com/KB/cs/DataObjectBase.aspx
//
// Enable support for collections (requires .NET framework 3.5)
// #define SUPPORT_COLLECTIONS

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System.Diagnostics;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using Cosmo.Diagnostics;
using Cosmo.Utils;

// #if SUPPORT_COLLECTIONS
using System.Collections.Specialized;
// #endif

//////////////////////////////////////////////////////////
//
// Usage example
//
// Below is a class that shows how to use the DataObjectBase class.
// The example is a simple object that represents an ini-file entry. 
//
// [Serializable]
// internal class IniData : DataObjectBase<IniData>
// {
//    /// <summary>
//    /// Initializes a new object from scratch.
//    /// </summary>
//    public IniData() : base(null, new StreamingContext()) { }
//
//    /// <summary>
//    /// Initializes a new object based on <see cref="SerializationInfo"/>.
//    /// </summary>
//    /// <param name="info"><see cref="SerializationInfo"/> that contains the information.</param>
//    /// <param name="context"><see cref="StreamingContext"/>.</param>
//    public IniData(SerializationInfo info, StreamingContext context) : base(info, context) { }
//
//    /// <summary>
//    /// Gets or sets the filename of the ini file to modify.
//    /// </summary>
//    public string FileName
//    {
//        get { return GetValue<string>(FileNameProperty); }
//        set { SetValue(FileNameProperty, value); }
//    }
//
//    /// <summary>
//    /// Register the property so it is known in the class.
//    /// </summary>
//    public readonly PropertyData FileNameProperty = RegisterProperty("FileName", typeof(string), string.Empty);
//
//    /// <summary>
//    /// Gets or sets the group inside the ini file to modify.
//    /// </summary>
//    public string Group
//    {
//        get { return GetValue<string>(GroupProperty); }
//        set { SetValue(GroupProperty, value); }
//    }
//
//    /// <summary>
//    /// Register the property so it is known in the class.
//    /// </summary>
//    public readonly PropertyData GroupProperty = RegisterProperty("Group", typeof(string), string.Empty);
//
//    /// <summary>
//    /// Gets or sets the key to modify.
//    /// </summary>
//    public string Key
//    {
//        get { return GetValue<string>(KeyProperty); }
//        set { SetValue(KeyProperty, value); }
//    }
//
//    /// <summary>
//    /// Register the property so it is known in the class.
//    /// </summary>
//    public readonly PropertyData KeyProperty = RegisterProperty("Key", typeof(string), string.Empty);
//
//    /// <summary>
//    /// Gets or sets the new value of the key.
//    /// </summary>
//    public string Value
//    {
//        get { return GetValue<string>(ValueProperty); }
//        set { SetValue(ValueProperty, value); }
//    }
//
//    /// <summary>
//    /// Register the property so it is known in the class.
//    /// </summary>
//    public readonly PropertyData ValueProperty = RegisterProperty("Value", typeof(string), string.Empty);
//
//    /// <summary>
//    /// Retrieves the actual data from the serialization info.
//    /// </summary>
//    /// <param name="info"><see cref="SerializationInfo"/>.</param>
//    protected override void GetDataFromSerializationInfo(SerializationInfo info)
//    {
//        // Check if deserialization succeeded
//        if (DeserializationSucceeded) return;
//
//        // Perform any custom serialization here, or if you wish to 
//        // support older style serialization, you can do it here
//    }
//}
//
//////////////////////////////////////////////////////////

namespace Cosmo.Data
{
   /// <summary>
   /// Abstract class that serves as a base class for serializable objects.
   /// </summary>
   /// <typeparam name="T">Type that the class should hold (same as the defined type).</typeparam>
   [Serializable]
   public abstract class DataObjectBase<T> : ISerializable, INotifyPropertyChanged, IDataErrorInfo, IDataWarningInfo, ICloneable, IDeserializationCallback, IEditableObject
      where T : class
   {

      #region Internal classes

      /// <summary>
      /// Class containing backup information.
      /// </summary>
      private class BackupData
      {
         private const string IsDirty = "IsDirty";

         private readonly DataObjectBase<T> _object;
         private Dictionary<string, object> _propertyValuesBackup;
         private Dictionary<string, object> _objectValuesBackup;

         /// <summary>
         /// Initializes a new backup data object.
         /// </summary>
         /// <param name="obj">Object to backup.</param>
         public BackupData(DataObjectBase<T> obj)
         {
            // Store values
            _object = obj;

            // Create backup
            CreateBackup();
         }

         #region Methods

         /// <summary>
         /// Creates a backup of the object property values.
         /// </summary>
         private void CreateBackup()
         {
            // Backup property values
            using (MemoryStream stream = new MemoryStream())
            {
               // Binary formatter
               BinaryFormatter binaryFormatter = new BinaryFormatter();
               binaryFormatter.Serialize(stream, ConvertDictionaryToList(_object._propertyValues));

               // Move to the beginning of the stream
               stream.Seek(0, 0);

               // Store backup
               _propertyValuesBackup = _object.ConvertListToDictionary((List<KeyValuePair<string, object>>)binaryFormatter.Deserialize(stream));
            }

            // Backup object values
            _objectValuesBackup = new Dictionary<string, object>();
            _objectValuesBackup.Add(IsDirty, _object.IsDirty);
         }

         /// <summary>
         /// Restores the backup to the object.
         /// </summary>
         public void RestoreBackup()
         {
            // Restore property values
            foreach (KeyValuePair<string, object> propertyValue in _propertyValuesBackup)
            {
               // Set value so the PropertyChanged event is invoked
               _object.SetValue(propertyValue.Key, propertyValue.Value);
            }

            // Restore object values
            _object.IsDirty = (bool)_objectValuesBackup[IsDirty];
         }

         #endregion

      }

      #endregion

      private const string WarningMessageProperty = "IDataWarningInfo.Message";
      private const string ErrorMessageProperty = "IDataErrorInfo.Message";

      private static readonly Dictionary<string, PropertyData> _propertyInfo = new Dictionary<string, PropertyData>();

      private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();
      private readonly object _propertyValuesLock = new object();
      private BackupData _backup;

      private Dictionary<string, string> _fieldWarnings = new Dictionary<string, string>();
      private List<string> _businessWarnings = new List<string>();
      private Dictionary<string, string> _fieldErrors = new Dictionary<string, string>();
      private List<string> _businessErrors = new List<string>();

      private readonly SerializationInfo _serializationInfo;

      /// <summary>
      /// Initializes a new instance of the <see cref="DataObjectBase&lt;T&gt;"/> class.
      /// </summary>
      protected DataObjectBase() : this(null, new StreamingContext())
      {
         // Set default values
         AlwaysInvokeNotifyChanged = true;
      }

      #region Events

      /// <summary>
      /// Invoked when a property of this object has changed.
      /// </summary>
      [field: NonSerialized]
      public event PropertyChangedEventHandler PropertyChanged;

      #endregion

      #region Operators

      /// <summary>
      /// Implements the operator ==.
      /// </summary>
      /// <param name="a">A.</param>
      /// <param name="b">The b.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(DataObjectBase<T> a, DataObjectBase<T> b)
      {
         // If both are null, or both are the same instance, return true
         if (ReferenceEquals(a, b)) return true;

         // If one is null, but not both, return false
         if (((object)a == null) || ((object)b == null)) return false;

         // Now loop all registered properties in a
         foreach (KeyValuePair<string, object> propertyValue in a._propertyValues)
         {
            // Get values
            object valueA = propertyValue.Value;
            object valueB = b.GetValue(propertyValue.Key);

            // Check references
            if (!ReferenceEquals(valueA, valueB))
            {
               if ((valueA == null) || (valueB == null)) return false;

               // Is this an IEnumerable (but not a string)?
               if ((valueA is IEnumerable) && !(valueA is string))
               {
                  // Yes, loop all sub items and check them
                  if (!CollectionHelper.IsEqualTo((IEnumerable)valueA, (IEnumerable)valueB)) return false;
               }
               else
               {
                  // No, check objects via equals method
                  if (!valueA.Equals(valueB)) return false;
               }
            }
         }

         // Objects are equal
         return true;
      }

      /// <summary>
      /// Implements the operator !=.
      /// </summary>
      /// <param name="a">A.</param>
      /// <param name="b">The b.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(DataObjectBase<T> a, DataObjectBase<T> b)
      {
         return (!(a == b));
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets a value indicating whether this instance is initialized.
      /// </summary>
      /// <value>
      /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
      /// </value>
      [Browsable(false)]
      [XmlIgnore]
      private static bool IsInitialized { get; set; }

      /// <summary>
      /// Gets or sets whether this object is subscribed to all childs.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      private bool SubscribedToEvents { get; set; }

      /// <summary>
      /// Gets or sets whether the deserialized data is available, which means that
      /// OnDeserialized is invoked.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      private bool IsDeserializedDataAvailable { get; set; }

      /// <summary>
      /// Gets or sets whether the object is fully deserialized.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      private bool IsDeserialized { get; set; }

      /// <summary>
      /// Gets or sets whether this object is validated or not.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      private bool IsValidated { get; set; }

      /// <summary>
      /// Gets or sets whether this object should always invoke the <see cref="PropertyChanged"/> event,
      /// even when the actual value of a property has not changed.
      /// 
      /// Enabling this property is useful when using this class in a WPF environment.
      /// </summary>
      /// <remarks>
      /// By default, this property is true. Disable it to gain a very, very small performance improvement but
      /// to loose stable WPF compatibility.
      /// </remarks>
      [Browsable(false)]
      protected bool AlwaysInvokeNotifyChanged { get; set; }

      /// <summary>
      /// Gets or sets whether this object should handle (thus invoke the specific events) when
      /// a property of collection value has changed.
      /// </summary>
      [Browsable(false)]
      protected bool HandlePropertyAndCollectionChanges { get; set; }

      /// <summary>
      /// Gets or sets whether this object should automatically validate itself when a property value
      /// has changed.
      /// </summary>
      [Browsable(false)]
      protected bool AutomaticallyValidateOnPropertyChanged { get; set; }

      /// <summary>
      /// Gets or sets the <see cref="SerializationMode"/> of this object.
      /// </summary>
      /// <remarks>
      /// This member is a field to enable support for Xml-Rpc (see http://www.xml-rpc.net/).
      /// </remarks>
      [Browsable(false)]
      [field: NonSerialized]
      [XmlIgnore]
      public SerializationMode Mode;

      /// <summary>
      /// Gets whether this object is dirty (contains unsaved data).
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      public bool IsDirty { get; protected set; }

      /// <summary>
      /// Gets whether this object contains any field or business warnings.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      public bool HasWarnings
      {
         get { return ((_fieldWarnings.Count + _businessWarnings.Count) > 0); }
      }

      /// <summary>
      /// Gets the number of field warnings.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      public int FieldWarningCount
      {
         get { return _fieldErrors.Count; }
      }

      /// <summary>
      /// Gets the number of business rule warnings.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      public int BusinessRuleWarningCount
      {
         get { return _businessWarnings.Count; }
      }

      /// <summary>
      /// Gets whether this object contains any field or business errors.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      public bool HasErrors
      {
         get { return ((_fieldErrors.Count + _businessErrors.Count) > 0); }
      }

      /// <summary>
      /// Gets the number of field errors.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      public int FieldErrorCount
      {
         get { return _fieldErrors.Count; }
      }

      /// <summary>
      /// Gets the number of business rule errors.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      public int BusinessRuleErrorCount
      {
         get { return _businessErrors.Count; }
      }

      /// <summary>
      /// Gets whether the deserialization has succeeded. If automatic deserialization fails, the object
      /// should try to deserialize manually.
      /// </summary>
      [Browsable(false)]
      [XmlIgnore]
      protected bool DeserializationSucceeded { get; private set; }

      #endregion

      #region Methods

      /// <summary>
      /// Loads the object from a file using binary formatting.
      /// </summary>
      /// <param name="fileName">Filename of the file that contains the serialized data of this object.</param>
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      public static T Load(string fileName)
      {
         // Load
         return Load(fileName, false);
      }

      /// <summary>
      /// Loads the object from a file using binary formatting.
      /// </summary>
      /// <param name="fileName">Filename of the file that contains the serialized data of this object.</param>
      /// <param name="enableRedirects">if set to <c>true</c>, redirects will be enabled.</param>
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      /// <remarks>
      /// When enableRedirects is enabled, loading will take more time. Only set
      /// the parameter to <c>true</c> when the deserialization without redirects fails.
      /// </remarks>
      public static T Load(string fileName, bool enableRedirects)
      {
         // Load
         return Load(fileName, SerializationMode.Binary, enableRedirects);
      }

      /// <summary>
      /// Loads the object from a file using a specific formatting.
      /// </summary>
      /// <param name="fileName">Filename of the file that contains the serialized data of this object.</param>
      /// <param name="mode"><see cref="SerializationMode"/> to use.</param>
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      public static T Load(string fileName, SerializationMode mode)
      {
         // Load
         return Load(fileName, mode, false);
      }

      /// <summary>
      /// Loads the object from a file using a specific formatting.
      /// </summary>
      /// <param name="fileName">Filename of the file that contains the serialized data of this object.</param>
      /// <param name="mode"><see cref="SerializationMode"/> to use.</param>
      /// <param name="enableRedirects">if set to <c>true</c>, redirects will be enabled.</param>
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      /// <remarks>
      /// When enableRedirects is enabled, loading will take more time. Only set
      /// the parameter to <c>true</c> when the deserialization without redirects fails.
      /// </remarks>
      public static T Load(string fileName, SerializationMode mode, bool enableRedirects)
      {
         // Create stream
         using (Stream stream = new FileStream(fileName, System.IO.FileMode.Open))
         {
            return Load(stream, mode, enableRedirects);
         }
      }

      /// <summary>
      /// Loads the object from an XmlDocument object.
      /// </summary>
      /// <param name="xmlDocument">The XML document.</param>
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      public static T Load(XmlDocument xmlDocument)
      {
         // Create memory stream
         using (MemoryStream memoryStream = new MemoryStream())
         {
            // Write xml to memory
            xmlDocument.Save(memoryStream);

            // Restore memory stream
            memoryStream.Position = 0L;

            // Load the document
            return Load(memoryStream, SerializationMode.Xml, false);
         }
      }

      /// <summary>
      /// Loads the object from a stream using binary formatting.
      /// </summary>
      /// <param name="bytes">The byte array.</param>
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      public static T Load(byte[] bytes)
      {
         // Load
         return Load(bytes, SerializationMode.Binary);
      }

      /// <summary>
      /// Loads the object from a stream.
      /// </summary>
      /// <param name="bytes">The byte array.</param>
      /// <param name="mode"><see cref="SerializationMode"/> to use.</param> 
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      public static T Load(byte[] bytes, SerializationMode mode)
      {
         // Load bytes into stream
         using (MemoryStream memoryStream = new MemoryStream())
         {
            // Write to memory stream
            memoryStream.Write(bytes, 0, bytes.Length);

            // Restore memory stream
            memoryStream.Position = 0L;

            // Load
            return Load(memoryStream, SerializationMode.Binary);
         }
      }

      /// <summary>
      /// Loads the object from a stream using binary formatting.
      /// </summary>
      /// <param name="stream">Stream that contains the serialized data of this object.</param>
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      public static T Load(Stream stream)
      {
         // Load
         return Load(stream, false);
      }

      /// <summary>
      /// Loads the specified stream.
      /// </summary>
      /// <param name="stream">The stream.</param>
      /// <param name="enableRedirects">if set to <c>true</c>, redirects will be enabled.</param>
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      /// <remarks>
      /// When enableRedirects is enabled, loading will take more time. Only set
      /// the parameter to <c>true</c> when the deserialization without redirects fails.
      /// </remarks>
      public static T Load(Stream stream, bool enableRedirects)
      {
         // Load
         return Load(stream, SerializationMode.Binary, enableRedirects);
      }

      /// <summary>
      /// Loads the object from a stream using a specific formatting.
      /// </summary>
      /// <param name="stream">Stream that contains the serialized data of this object.</param>
      /// <param name="mode"><see cref="SerializationMode"/> to use.</param> 
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      public static T Load(Stream stream, SerializationMode mode)
      {
         // Load
         return Load(stream, mode, false);
      }

      /// <summary>
      /// Loads the object from a stream using a specific formatting.
      /// </summary>
      /// <param name="stream">Stream that contains the serialized data of this object.</param>
      /// <param name="mode"><see cref="SerializationMode"/> to use.</param> 
      /// <param name="enableRedirects">if set to <c>true</c>, redirects will be enabled.</param>
      /// <returns>Deserialized instance of the object. If the deserialization fails, <c>null</c> is returned.</returns>
      /// <remarks>
      /// When enableRedirects is enabled, loading will take more time. Only set
      /// the parameter to <c>true</c> when the deserialization without redirects fails.
      /// </remarks>
      public static T Load(Stream stream, SerializationMode mode, bool enableRedirects)
      {
         // Declare variables
         object result = null;

         // Create right formatter
         switch (mode)
         {
            case SerializationMode.Binary:

               try
               {
                  // Declare formatter
                  BinaryFormatter binaryFormatter = null;

                  // Check how we should load
                  if (!enableRedirects)
                  {
                     // Create binary formatter
                     binaryFormatter = new BinaryFormatter();
                  }
                  else
                  {
                     // Create custom binary formatter with custom binder
                     binaryFormatter = new BinaryFormatter();
                     binaryFormatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                     binaryFormatter.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                     binaryFormatter.Binder = new RedirectDeserializationBinder();

                     // Reset stream position
                     stream.Position = 0L;
                  }

                  // Deserialize
                  result = binaryFormatter.Deserialize(stream);
               }
               catch (Exception ex)
               {
                  // Trace
                  ExceptionHelper.TraceExceptionAsError(ex, "Failed to deserialize the binary object");

                  // Failed
                  return default(T);
               }
               break;

            case SerializationMode.Xml:

               // Xml does not support custom bindings, thus no redirects
               try
               {
                  // Declare formatter
                  XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                  // Deserialize
                  result = xmlSerializer.Deserialize(stream);
               }
               catch (Exception ex)
               {
                  // Trace
                  ExceptionHelper.TraceExceptionAsError(ex, "Failed to deserialize the xml object");

                  // Failed
                  return default(T);
               }
               break;
         }

         // Store mode
         if ((result != null) && (result is DataObjectBase<T>))
         {
            // Store mode
            ((DataObjectBase<T>)result).Mode = mode;
         }

         // Return result
         return (T)result;
      }

      /// <summary>
      /// Saves the object to a file using the default formatting.
      /// </summary>
      /// <param name="fileName">Filename of the file that will contain the serialized data of this object.</param>
      public void Save(string fileName)
      {
         // Save using the default mode
         Save(fileName, Mode);
      }

      /// <summary>
      /// Saves the object to a file using a specific formatting.
      /// </summary>
      /// <param name="mode"><see cref="SerializationMode"/> to use.</param>
      /// <param name="fileName">Filename of the file that will contain the serialized data of this object.</param>
      public void Save(string fileName, SerializationMode mode)
      {
         // Get the directory and make sure it exists
         FileInfo fileInfo = new FileInfo(fileName);
         if ((fileInfo.DirectoryName != null) && (!Directory.Exists(fileInfo.DirectoryName)))
         {
            Directory.CreateDirectory(fileInfo.DirectoryName);
         }

         // Create stream
         using (Stream stream = new FileStream(fileName, FileMode.Create))
         {
            // Use stream save method
            Save(stream, mode);
         }
      }

      /// <summary>
      /// Saves the object to a stream using the default formatting.
      /// </summary>
      /// <param name="stream">Stream that will contain the serialized data of this object.</param>
      public void Save(Stream stream)
      {
         // Save using the default mode
         Save(stream, Mode);
      }

      /// <summary>
      /// Saves the object to a stream using a specific formatting.
      /// </summary>
      /// <param name="stream">Stream that will contain the serialized data of this object.</param>
      /// <param name="mode"><see cref="SerializationMode"/> to use.</param>
      public void Save(Stream stream, SerializationMode mode)
      {
         // Create right formatter
         switch (mode)
         {
            case SerializationMode.Binary:
               // Create binary formatter
               BinaryFormatter binaryFormatter = new BinaryFormatter();

               // Serialize
               binaryFormatter.Serialize(stream, this);
               break;

            case SerializationMode.Xml:
               // Create serializer
               XmlSerializer xmlSerializer = new XmlSerializer(GetType());

               // Serialize
               xmlSerializer.Serialize(stream, this);
               break;
         }

         // Not dirty any longer
         IsDirty = false;
      }

      /// <summary>
      /// Registers a property that will be automatically handled by this object.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <param name="type">Type of the property.</param>
      /// <param name="defaultValue">Default value of the property.</param>
      /// <returns>
      /// 	<see cref="PropertyData"/> containing the property information.
      /// </returns>
      protected static PropertyData RegisterProperty(string name, Type type, object defaultValue)
      {
         // Invoke overload
         return RegisterProperty(name, type, defaultValue, null);
      }

      /// <summary>
      /// Registers a property that will be automatically handled by this object.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <param name="type">Type of the property.</param>
      /// <param name="defaultValue">Default value of the property.</param>
      /// <param name="propertyChangedEventHandler">The property changed event handler.</param>
      /// <returns>
      /// 	<see cref="PropertyData"/> containing the property information.
      /// </returns>
      protected static PropertyData RegisterProperty(string name, Type type, object defaultValue, PropertyChangedEventHandler propertyChangedEventHandler)
      {
         // Check if property is serializable
         if (!type.IsSerializable) throw new InvalidPropertyException(name);

         // Create the property
         PropertyData property = new PropertyData(name, type, defaultValue, propertyChangedEventHandler);

         // Return the data
         return property;
      }

      /// <summary>
      /// Initializes all the properties for this object.
      /// </summary>
      private void InitializeProperties()
      {
         // Get the current type
         Type type = GetType();

         // Loop all fields
         FieldInfo[] fields = type.GetFields();
         foreach (FieldInfo field in fields)
         {
            if (field.FieldType == typeof(PropertyData))
            {
               // Get field value
               PropertyData propertyValue = ((field.IsStatic) ? field.GetValue(null) : field.GetValue(this)) as PropertyData;
               if (propertyValue != null)
               {
                  // Initialize this property
                  InitializeProperty(propertyValue);
               }
            }
         }

         // Loop all property data properties
         PropertyInfo[] properties = type.GetProperties();
         foreach (PropertyInfo property in properties)
         {
            if (property.PropertyType == typeof(PropertyData))
            {
               // Get the property value
               PropertyData propertyValue = property.GetValue(null, null) as PropertyData;
               if (propertyValue != null)
               {
                  // Inititialize this property
                  InitializeProperty(propertyValue);
               }
            }
         }

         // Initialized
         IsInitialized = true;
      }

      /// <summary>
      /// Initializes a specific property for this object.
      /// </summary>
      /// <param name="property"><see cref="PropertyData"/> of the property.</param>
      /// <exception cref="InvalidPropertyException">Thrown when the name of the property is invalid.</exception>
      /// <exception cref="PropertyAlreadyRegisteredException">Thrown when the property is already registered.</exception>
      private void InitializeProperty(PropertyData property)
      {
         // Invoke override
         InitializeProperty(property.Name, property.Type, property.GetDefaultValue(), property.PropertyChangedEventHandler);
      }

      /// <summary>
      /// Initializes a specific property for this object.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <param name="type">Type of the property.</param>
      /// <param name="defaultValue">Default value of the property.</param>
      /// <param name="propertyChangedEventHandler">The property changed event handler.</param>
      /// <exception cref="InvalidPropertyException">Thrown when the name of the property is invalid.</exception>
      /// <exception cref="PropertyAlreadyRegisteredException">Thrown when the property is already registered.</exception>
      private void InitializeProperty(string name, Type type, object defaultValue, PropertyChangedEventHandler propertyChangedEventHandler)
      {
         // Check if the property name is valid
         if (string.IsNullOrEmpty(name)) throw new InvalidPropertyException(name);

         // Check if the type is nullable
         if ((defaultValue == null) && !IsTypeNullable(type)) throw new PropertyNotNullableException(name, GetType());

         // If not initialized, register property
         if (!IsInitialized)
         {
            // First, check if this property is already in use
            if (IsPropertyRegistered(name)) throw new PropertyAlreadyRegisteredException(name, GetType());

            // Create property data
            PropertyData propertyData = new PropertyData(name, type, defaultValue, propertyChangedEventHandler);
            _propertyInfo.Add(name, propertyData);
         }

         // Lock
         lock (_propertyValuesLock)
         {
            // Add property value
            _propertyValues.Add(name, defaultValue);
         }
      }

      /// <summary>
      /// Returns whether a specific property is registered.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <returns>True if the property is registered, otherwise false.</returns>
      protected bool IsPropertyRegistered(string name)
      {
         return _propertyInfo.ContainsKey(name);
      }

      /// <summary>
      /// Validates the field values of this object. Override this method to enable
      /// validation of field values.
      /// </summary>
      protected virtual void ValidateFields() { }

      /// <summary>
      /// Validates the business rules of this object. Override this method to enable
      /// validation of business rules.
      /// </summary>
      protected virtual void ValidateBusinessRules() { }

      /// <summary>
      /// Sets the value of a specific property.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <param name="value">Value of the property</param>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      protected void SetValue(string name, object value)
      {
         // Set value
         SetValue(name, value, true);
      }

      /// <summary>
      /// Sets the value of a specific property.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <param name="value">Value of the property</param>
      /// <param name="notifyOnChange"></param>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>/// 
      private void SetValue(string name, object value, bool notifyOnChange)
      {
         // Check if we have the property
         if (!IsPropertyRegistered(name)) throw new PropertyNotRegisteredException(name, GetType());

         // Retrieve the information
         PropertyData property = _propertyInfo[name];

         // Check if the type is nullable
         if ((value == null) && !IsTypeNullable(property.Type)) throw new PropertyNotNullableException(name, GetType());

         // Check if the type is right
         if ((value != null) && (!property.Type.IsAssignableFrom(value.GetType()))) throw new InvalidPropertyValueException(name, property.Type, value.GetType());

         // Set value
         lock (_propertyValuesLock)
         {
            // Check if we should invoke PropertyChanged event
            bool notify = false;

            // Check if the values are different
            if (GetValue(name) != value)
            {
               // Update the value
               _propertyValues[name] = value;

               // Data has changed
               IsValidated = false;

               // Notify
               notify = true;
            }

            // Invoke NotifyChanged event if required
            if (notifyOnChange && (AlwaysInvokeNotifyChanged || notify))
            {
               OnPropertyChanged(name);
            }
         }
      }

      /// <summary>
      /// Sets the value of a specific property.
      /// </summary>
      /// <param name="property"><see cref="PropertyData"/> of the property.</param>
      /// <param name="value">Value of the property</param>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      protected void SetValue(PropertyData property, object value)
      {
         // Check for null references
         if (property == null) throw new NullReferenceException("Property may not be null");

         // Call method
         SetValue(property.Name, value);
      }

      /// <summary>
      /// Gets the value of a specific property.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <returns>Object value of the property</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      protected object GetValue(string name)
      {
         // Check if we have the property
         if (!IsPropertyRegistered(name)) throw new PropertyNotRegisteredException(name, GetType());

         // Lock properties
         lock (_propertyValuesLock)
         {
            // Return data
            return _propertyValues[name];
         }
      }

      /// <summary>
      /// Gets the typed value of a specific property.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <returns>Object value of the property</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      protected T1 GetValue<T1>(string name)
      {
         // Get value
         object obj = GetValue(name);

         // Return right value
         return ((obj != null) && (obj is T1)) ? (T1)obj : default(T1);
      }

      /// <summary>
      /// Gets the value of a specific property.
      /// </summary>
      /// <param name="property"><see cref="PropertyData"/> of the property.</param>
      /// <returns>Object value of the property</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      protected object GetValue(PropertyData property)
      {
         // Check for null references
         if (property == null) throw new NullReferenceException("Property may not be null");

         // Call method
         return GetValue(property.Name);
      }

      /// <summary>
      /// Gets the typed value of a specific property.
      /// </summary>
      /// <param name="property"><see cref="PropertyData"/> of the property.</param>
      /// <returns>Object value of the property</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      protected T1 GetValue<T1>(PropertyData property)
      {
         // Get value
         object obj = GetValue(property);

         // Return right value
         return ((obj != null) && (obj is T1)) ? (T1)obj : default(T1);
      }

      /// <summary>
      /// Returns the default value of a specific property.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <returns>Default value of the property.</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      public object GetDefaultValue(string name)
      {
         // Check if we have the property
         if (!IsPropertyRegistered(name)) throw new PropertyNotRegisteredException(name, GetType());

         // Return value
         return _propertyInfo[name].GetDefaultValue();
      }

      /// <summary>
      /// Returns the typed default value of a specific property.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <returns>Default value of the property.</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      public T1 GetDefaultValue<T1>(string name)
      {
         // Get value
         object obj = GetDefaultValue(name);

         // Return right value
         return ((obj != null) && (obj is T1)) ? (T1)obj : default(T1);
      }

      /// <summary>
      /// Returns the default value of a specific property.
      /// </summary>
      /// <param name="property"><see cref="PropertyData"/> of the property.</param>
      /// <returns>Default value of the property.</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      public object GetDefaultValue(PropertyData property)
      {
         // Check for null references
         if (property == null) throw new NullReferenceException("Property may not be null");

         // Return default value
         return GetDefaultValue(property.Name);
      }

      /// <summary>
      /// Returns the typed default value of a specific property.
      /// </summary>
      /// <param name="property"><see cref="PropertyData"/> of the property.</param>
      /// <returns>Default value of the property.</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      public T1 GetDefaultValue<T1>(PropertyData property)
      {
         // Get value
         object obj = GetDefaultValue(property);

         // Return right value
         return ((obj != null) && (obj is T1)) ? (T1)obj : default(T1);
      }

      /// <summary>
      /// Returns the type of a specific property.
      /// </summary>
      /// <param name="name">Name of the property.</param>
      /// <returns>Type of the property.</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      public Type GetType(string name)
      {
         // Check if we have the property
         if (!IsPropertyRegistered(name)) throw new PropertyNotRegisteredException(name, GetType());

         // Return value
         return _propertyInfo[name].Type;
      }

      /// <summary>
      /// Returns the type of a specific property.
      /// </summary>
      /// <param name="property"><see cref="PropertyData"/> of the property.</param>
      /// <returns>Type of the property.</returns>
      /// <exception cref="PropertyNotRegisteredException">Thrown when the property is not registered.</exception>
      public Type GetType(PropertyData property)
      {
         return GetType(property.Name);
      }

      /// <summary>
      /// Returns whether a type is nullable or not.
      /// </summary>
      /// <param name="type">Type to check.</param>
      /// <returns>True if the type is nullable, otherwise false.</returns>
      private static bool IsTypeNullable(Type type)
      {
         // Check input
         if (type == null) return false;

         // Check if this type is a reference type (thus nullable)
         if (!type.IsValueType) return true;

         // Check if this type is nullable
         if (Nullable.GetUnderlyingType(type) != null) return true;

         // Not nullable (value-type)
         return false;
      }

      /// <summary>
      /// Validates the current object for field and business rule errors.
      /// </summary>
      /// <remarks>
      /// To check wether this object contains any errors, use the <see cref="HasErrors"/> property.
      /// 
      /// This method does not force validation. This means that when the object is already validated,
      /// and no properties have been changed, no validation actually occurs since there is no reason
      /// for any values to have changed.
      /// </remarks>
      public void Validate()
      {
         // Validate (but do not force by default)
         Validate(false);
      }

      /// <summary>
      /// Validates the current object for field and business rule errors.
      /// </summary>
      /// <param name="force">if set to <c>true</c>, a validation is forced.</param>
      /// <remarks>
      /// To check wether this object contains any errors, use the <see cref="HasErrors"/> property.
      /// </remarks>
      public void Validate(bool force)
      {
         // Check if the object is already validated
         if (IsValidated && !force) return;

         // Make sure we have a valid data errors dictionary
         IDictionary<string, string> previousFieldWarnings = _fieldWarnings;
         IDictionary<string, string> previousFieldErrors = _fieldErrors;

         // Clear all warnings & errors
         _fieldWarnings = new Dictionary<string, string>();
         _fieldErrors = new Dictionary<string, string>();

         // Validate the field errors
         ValidateFields();

         IDictionary<string, bool> changedFields = new Dictionary<string, bool>();
         if (previousFieldWarnings != null)
         {
            foreach (string propertyName in previousFieldWarnings.Keys)
            {
               changedFields.Add(propertyName, !_fieldWarnings.ContainsKey(propertyName) || !string.Equals(_fieldWarnings[propertyName], previousFieldWarnings[propertyName], StringComparison.Ordinal));
            }
         }
         if (previousFieldErrors != null)
         {
            foreach (string propertyName in previousFieldErrors.Keys)
            {
               changedFields.Add(propertyName, !_fieldErrors.ContainsKey(propertyName) || !string.Equals(_fieldErrors[propertyName], previousFieldErrors[propertyName], StringComparison.Ordinal));
            }
         }

         foreach (string propertyName in _fieldWarnings.Keys)
         {
            // Was not in previousCollection, new error thus also changed.
            if (!changedFields.ContainsKey(propertyName)) changedFields.Add(propertyName, true);
         }
         foreach (string propertyName in _fieldErrors.Keys)
         {
            // Was not in previousCollection, new error thus also changed.
            if (!changedFields.ContainsKey(propertyName)) changedFields.Add(propertyName, true);
         }

         // Fire the property changed event for all changed fields
         foreach (string propertyName in changedFields.Keys)
         {
            if (changedFields[propertyName]) OnPropertyChanged(propertyName);
         }

         // Get the previous count
         int previousBusinessWarningsCount = _businessWarnings.Count;
         int previousBusinessErrorsCount = _businessErrors.Count;

         // Clear all warnings & errors
         _businessWarnings = new List<string>();
         _businessErrors = new List<string>();

         // Validate business rules
         ValidateBusinessRules();

         // Object is validated (do this before updating the business rules)
         IsValidated = true;

         // Update properties if required
         if ((_businessWarnings.Count > 0) || (previousBusinessWarningsCount > 0))
         {
            OnPropertyChanged(WarningMessageProperty);
         }
         if ((_businessErrors.Count > 0) || (previousBusinessErrorsCount > 0))
         {
            OnPropertyChanged(ErrorMessageProperty);
         }
      }

      /// <summary>
      /// Returns a message that contains all the current errors and automatically determines the name of the object.
      /// </summary>
      /// <returns>
      /// Error string or empty in case of no errors.
      /// </returns>
      public string GetErrorMessage()
      {
         return GetErrorMessage(null);
      }

      /// <summary>
      /// Returns a message that contains all the current errors.
      /// </summary>
      /// <param name="userFriendlyObjectName">Name of the user friendly object.</param>
      /// <returns>
      /// Error string or empty in case of no errors.
      /// </returns>
      public string GetErrorMessage(string userFriendlyObjectName)
      {
         // Check if we even have errors
         if (!HasErrors) return string.Empty;

         // Check the user friendly entity name
         if (string.IsNullOrEmpty(userFriendlyObjectName))
         {
            // Use the real entity name (stupid developer that passes a useless value)
            userFriendlyObjectName = GetType().Name;
         }

         // Set up message
         StringBuilder messageBuilder = new StringBuilder();
         messageBuilder.AppendLine(string.Format(CultureInfo.CurrentUICulture, Properties.Resources.ErrorsFound, userFriendlyObjectName));
         messageBuilder.Append(GetListMessages(_fieldErrors, _businessErrors));

         // Return message
         return messageBuilder.ToString();
      }

      /// <summary>
      /// Returns a message that contains all the current warnings and automatically determines the name of the object.
      /// </summary>
      /// <returns>
      /// Warning string or empty in case of no warnings.
      /// </returns>
      public string GetWarningMessage()
      {
         return GetWarningMessage(null);
      }

      /// <summary>
      /// Returns a message that contains all the current warnings.
      /// </summary>
      /// <param name="userFriendlyObjectName">Name of the user friendly object.</param>
      /// <returns>
      /// Warning string or empty in case of no warnings.
      /// </returns>
      public string GetWarningMessage(string userFriendlyObjectName)
      {
         // Check if we even have errors
         if (!HasWarnings) return string.Empty;

         // Check the user friendly entity name
         if (string.IsNullOrEmpty(userFriendlyObjectName))
         {
            // Use the real entity name (stupid developer that passes a useless value)
            userFriendlyObjectName = GetType().Name;
         }

         // Set up message
         StringBuilder messageBuilder = new StringBuilder();
         messageBuilder.AppendLine(string.Format(CultureInfo.CurrentUICulture, Properties.Resources.WarningsFound, userFriendlyObjectName));
         messageBuilder.Append(GetListMessages(_fieldWarnings, _businessWarnings));

         // Return message
         return messageBuilder.ToString();
      }

      /// <summary>
      /// Gets the list messages.
      /// </summary>
      /// <param name="fields">The field warnings or errors.</param>
      /// <param name="business">The business warnings or errors.</param>
      /// <returns>String representing the output of all items in the fields an business object.</returns>
      /// <remarks>
      /// This method is used to create a message string for field warnings or errors and business warnings
      /// or errors. Just pass the right dictionary and list to this method.
      /// </remarks>
      private static string GetListMessages(Dictionary<string, string> fields, IEnumerable<string> business)
      {
         // Set up message
         StringBuilder messageBuilder = new StringBuilder();

         // Add fields
         foreach (KeyValuePair<string, string> field in fields)
         {
            messageBuilder.AppendLine(string.Format("* {0}", field.Value));
         }
         foreach (string businessItem in business)
         {
            messageBuilder.AppendLine(string.Format("* {0}", businessItem));
         }

         // Return message
         return messageBuilder.ToString();
      }

      /// <summary>
      /// Sets a specific field warning.
      /// </summary>
      /// <param name="property"><see cref="PropertyData"/> of the property.</param>
      /// <param name="warning">Warning message.</param>
      protected void SetFieldWarning(PropertyData property, string warning)
      {
         // Call other method
         SetFieldWarning(property.Name, warning);
      }

      /// <summary>
      /// Sets a specific field warning.
      /// </summary>
      /// <param name="property">Name of the property.</param>
      /// <param name="warning">Warning message.</param>
      protected void SetFieldWarning(string property, string warning)
      {
         // Store the warning
         _fieldWarnings[property] = warning;
      }

      /// <summary>
      /// Sets a specific business rule warning.
      /// </summary>
      /// <param name="warning">Warning message</param>
      protected void SetBusinessRuleWarning(string warning)
      {
         // Make sure to list the warning only once
         if (_businessWarnings.Contains(warning)) return;

         // Store the warning
         _businessWarnings.Add(warning);
      }

      /// <summary>
      /// Sets a specific field error.
      /// </summary>
      /// <param name="property"><see cref="PropertyData"/> of the property.</param>
      /// <param name="error">Error message.</param>
      protected void SetFieldError(PropertyData property, string error)
      {
         // Call other method
         SetFieldError(property.Name, error);
      }

      /// <summary>
      /// Sets a specific field error.
      /// </summary>
      /// <param name="property">Name of the property.</param>
      /// <param name="error">Error message.</param>
      protected void SetFieldError(string property, string error)
      {
         // Store the error
         _fieldErrors[property] = error;
      }

      /// <summary>
      /// Sets a specific business rule error.
      /// </summary>
      /// <param name="error">Error message</param>
      protected void SetBusinessRuleError(string error)
      {
         // Make sure to list the error only once
         if (_businessErrors.Contains(error)) return;

         // Store the error
         _businessErrors.Add(error);
      }

      /// <summary>
      /// Subscribes to all property events.
      /// </summary>
      public void SubscribeToEvents()
      {
         // Check for a valid object
         if (_propertyValues == null) return;

         // Check if we are already subscribed
         if (SubscribedToEvents) return;

         // Lock
         lock (_propertyValuesLock)
         {
            // Loop all properties
            foreach (KeyValuePair<string, object> property in _propertyValues)
            {
               // Check if we have a valid value
               if (property.Value != null)
               {
                  // Check via reflection whether this object supports INotifyPropertyChanged
                  INotifyPropertyChanged propertyChangedValue = property.Value as INotifyPropertyChanged;
                  if (propertyChangedValue != null)
                  {
                     // Subscribe
                     propertyChangedValue.PropertyChanged += OnPropertyChanged;
                  }

#if SUPPORT_COLLECTIONS

                        // Check via reflection whether this object supports INotifyCollectionChanged
                        INotifyCollectionChanged collectionChangedValue = property.Value as INotifyCollectionChanged;
                        if (collectionChangedValue != null)
                        {
                            // Subscribe
                            collectionChangedValue.CollectionChanged += OnCollectionChanged;
                        }

#endif

               }
            }
         }

         // We are now subscribed to all events
         SubscribedToEvents = true;
      }

      /// <summary>
      /// Invoked when a property value has changed.
      /// </summary>
      /// <param name="sender">The sender.</param>
      /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
      private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         // Is this a warning or error message?
         if ((string.Compare(e.PropertyName, WarningMessageProperty) != 0) &&
            (string.Compare(e.PropertyName, ErrorMessageProperty) != 0))
         {
            // Object is dirty
            IsDirty = true;

            // Not validated
            IsValidated = false;
         }

         // Check if we should handle events
         if (!HandlePropertyAndCollectionChanges) return;

         // Should a special property changed event be invoked?
         if (_propertyInfo.ContainsKey(e.PropertyName))
         {
            // Get property data
            PropertyData propertyData = _propertyInfo[e.PropertyName];

            // Invoke special event if required
            if (propertyData.PropertyChangedEventHandler != null) propertyData.PropertyChangedEventHandler(sender, e);
         }

         // Invoke event if available)
         if (PropertyChanged != null) PropertyChanged(sender, e);

         // Automatically validate if needed
         if (AutomaticallyValidateOnPropertyChanged) Validate();
      }

      /// <summary>
      /// Invoked when a property value has changed.
      /// </summary>
      /// <param name="propertyName">Name of the property that has changed.</param>
      protected virtual void OnPropertyChanged(string propertyName)
      {
         OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }

#if SUPPORT_COLLECTIONS

        /// <summary>
        /// Invoked when a collection value has changed.
        /// </summary>
        /// <param name="sender">The object that contains the changed collection value.</param>
        /// <param name="e"><see cref="NotifyCollectionChangedEventArgs"/> containing all information about the changed collection.</param>
        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Object is dirty
            IsDirty = true;

            // Not validated
            IsValidated = false;

            // Check if we should handle events
            if (!HandlePropertyAndCollectionChanges) return;

            // Lock
            lock (_propertyValuesLock)
            {
                // Check the property that has changed
                foreach (KeyValuePair<string, object> property in _propertyValues)
                {
                    // Compare
                    if ((property.Value != null) && (property.Value == sender))
                    {
                        // We know which property has changed
                        OnPropertyChanged(property.Key);

                        // Exit
                        return;
                    }
                }
            }
        }

#endif

      /// <summary>
      /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
      /// </summary>
      /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
      /// <returns>
      /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
      /// </returns>
      /// <exception cref="T:System.NullReferenceException">
      /// The <paramref name="obj"/> parameter is null.
      /// </exception>
      public override bool Equals(object obj)
      {
         // Check for null
         if ((object)obj == null) return false;

         // Check type
         if (!(obj is DataObjectBase<T>)) return false;

         // Now use the == operator
         return (DataObjectBase<T>)this == (DataObjectBase<T>)obj;
      }

      /// <summary>
      /// Returns a hash code for this instance.
      /// </summary>
      /// <returns>
      /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
      /// </returns>
      public override int GetHashCode()
      {
         return base.GetHashCode();
      }

      /// <summary>
      /// Returns a <see cref="System.String"/> that represents this instance.
      /// </summary>
      /// <returns>
      /// A <see cref="System.String"/> that represents this instance.
      /// </returns>
      public override string ToString()
      {
         return base.ToString();
      }

      /// <summary>
      /// Serializes the object to and xml object.
      /// </summary>
      /// <returns><see cref="XmlDocument"/> containing the serialized data.</returns>
      public XmlDocument ToXml()
      {
         // Create xml document
         XmlDocument xmlDocument = new XmlDocument();

         // Create serializer
         XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

         // Write data to string
         StringBuilder xmlContent = new StringBuilder();
         using (XmlWriter xmlWriter = XmlWriter.Create(xmlContent))
         {
            // Serialize
            xmlSerializer.Serialize(xmlWriter, this);
         }

         // Now push the text into the xml document
         xmlDocument.LoadXml(xmlContent.ToString());

         // Return document
         return xmlDocument;
      }

      /// <summary>
      /// Serializes the object to a byte array.
      /// </summary>
      /// <returns>Byte array containing the serialized data.</returns>
      public byte[] ToByteArray()
      {
         // Create memory stream
         using (MemoryStream memoryStream = new MemoryStream())
         {
            // Save to memory stream
            Save(memoryStream, SerializationMode.Binary);

            // Return memory stream as array
            return memoryStream.ToArray();
         }
      }

      #endregion

      #region Serialization

      /// <summary>
      /// Only constructor for the DataObjectBase.
      /// </summary>
      /// <param name="info">SerializationInfo object, null if this is the first time construction.</param>
      /// <param name="context">StreamingContext object, simple pass a default new StreamingContext() if this is the first time construction.</param>
      /// <remarks>
      /// Call this method, even when constructing the object for the first time (thus not deserializing).
      /// </remarks>
      protected DataObjectBase(SerializationInfo info, StreamingContext context)
      {
         // Initialize the properties
         InitializeProperties();

         // Set default values
         DeserializationSucceeded = false;
         HandlePropertyAndCollectionChanges = true;
         AutomaticallyValidateOnPropertyChanged = true;
         Mode = SerializationMode.Binary;
         IsDirty = false;

         // Make sure this is not a first time call
         if (info == null)
         {
            // Yes, subscribe to any events
            SubscribeToEvents();

            // Exit
            return;
         }

         // Store the serialization info
         _serializationInfo = info;

         // Deserialize properties (for checking purposes only)
         List<KeyValuePair<string, object>> properties = (List<KeyValuePair<string, object>>)SerializationHelper.GetObject(info,
            "Properties", typeof(List<KeyValuePair<string, object>>), new List<KeyValuePair<string, object>>());

         // Get data from serialization info if possible
         GetDataFromSerializationInfoInternal(_serializationInfo);

         // Set whether deserialization succeeded
         DeserializationSucceeded = ((properties != null) && (properties.Count > 0));
      }

      /// <summary>
      /// Retrieves the actual data from the serialization info.
      /// </summary>
      /// <param name="info"><see cref="SerializationInfo"/>.</param>
      /// <remarks>
      /// This method is called from the OnDeserialized method, thus all child objects
      /// are serialized and available at the time this method is called.
      /// 
      /// Only use this method to support older serialization techniques. When using this class
      /// for new objects, all serialization is handled automatically.
      /// </remarks>
      protected virtual void GetDataFromSerializationInfo(SerializationInfo info) { }

      /// <summary>
      /// Retrieves the actual data from the serialization info for the properties registered
      /// on this object.
      /// </summary>
      /// <param name="info"><see cref="SerializationInfo"/>.</param>
      protected void GetDataFromSerializationInfoInternal(SerializationInfo info)
      {
         // Declare variables
         List<KeyValuePair<string, object>> properties;

         // Check if the data is already deserialized
         if (IsDeserialized) return;

         // Check if there is data available
         if (!IsDeserializedDataAvailable) return;

         // Exit if there is no serialization info
         if (info == null) return;

         try
         {
            // Deserialize properties
            properties = (List<KeyValuePair<string, object>>)SerializationHelper.GetObject(info,
               "Properties", typeof(List<KeyValuePair<string, object>>), new List<KeyValuePair<string, object>>());

            // Now load all properties that were serialized
            foreach (KeyValuePair<string, object> property in properties)
            {
               // Check if the property is registered
               if (IsPropertyRegistered(property.Key))
               {
                  // Check if the property is null
                  if (property.Value != null)
                  {
                     // Check if the property is a collection
                     if ((property.Value is ICollection) && (property.Value is IDeserializationCallback))
                     {
                        // Get the deserialization callback
                        IDeserializationCallback propertyDeserializationCallback = property.Value as IDeserializationCallback;

                        // Call it since collections need this call to contain valid items
                        propertyDeserializationCallback.OnDeserialization(this);
                     }
                  }

                  // Lock
                  lock (_propertyValuesLock)
                  {
                     // Store the value (since deserialized values always override default values)
                     _propertyValues[property.Key] = property.Value;
                  }
               }
            }
         }
         catch (Exception ex)
         {
            // Trace
            Trace.TraceError("An error occurred while deserializing object '{1}'.{0}{0}Details:{0}'{2}'",
                             Environment.NewLine, GetType().Name, ex.ToString());
         }

         // Allow developers to support backwards compatibility
         GetDataFromSerializationInfo(info);

         // Data is now considered deserialized
         IsDeserialized = true;

         try
         {
            // Now serialization of all child objects has finished, subscribe to events
            SubscribeToEvents();
         }
         catch (Exception)
         {
            // Trace
            Trace.TraceWarning("Failed to subscribe to events in the OnDeserialized method");
         }

         // Solution is not yet dirty
         IsDirty = false;
      }

      /// <summary>
      /// Writes all the data that must be serialized into the <see cref="SerializationInfo"/> object.
      /// </summary>
      /// <param name="info"><see cref="SerializationInfo"/>.</param>
      /// <param name="context">Not used.</param>
      public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         // Declare variables
         List<KeyValuePair<string, object>> properties;

         // Lock
         lock (_propertyValuesLock)
         {
            // Convert dictionary to list
            properties = ConvertDictionaryToList(_propertyValues);
         }

         // Serialize properties
         info.AddValue("Properties", properties, typeof(List<KeyValuePair<string, object>>));
      }

      /// <summary>
      /// Invoked when the deserialization of the object graph is complete.
      /// </summary>
      /// <param name="context">StreamingContext.</param>
      [OnDeserialized]
      private void OnDeserialized(StreamingContext context)
      {
         // The object is deserialized
         IsDeserializedDataAvailable = true;

         // Get data from serialization info if possible
         GetDataFromSerializationInfoInternal(_serializationInfo);
      }

      /// <summary>
      /// Invoked when the deserialization of the object graph is complete.
      /// </summary>
      /// <param name="sender">Sender.</param>
      public void OnDeserialization(object sender)
      {
         try
         {
            // Lock
            lock (_propertyValuesLock)
            {
               // Call the IDeserializationCallback for all childs
               foreach (KeyValuePair<string, object> property in _propertyValues)
               {
                  // Call the deserialization callback
                  CallOnDeserializationCallback(property.Value);

                  // Is the property value a collection?
                  ICollection collection = property.Value as ICollection;
                  if (collection != null)
                  {
                     foreach (object item in collection)
                     {
                        // Set item explicitly
                        CallOnDeserializationCallback(item);
                     }
                  }
               }
            }
         }
         catch (Exception)
         {
            // Trace
            Trace.TraceWarning("Failed to call IDeserializationCallback.OnDeserialization for child objects");
         }
      }

      /// <summary>
      /// Calls the <see cref="IDeserializationCallback.OnDeserialization"/> method on the object if possible.
      /// </summary>
      /// <param name="obj"></param>
      private void CallOnDeserializationCallback(object obj)
      {
         // Make sure we have a value
         if (obj == null) return;

         // Get the value
         IDeserializationCallback propertyCallback = obj as IDeserializationCallback;
         if (propertyCallback != null)
         {
            // Call the OnDeserialization method
            propertyCallback.OnDeserialization(this);
         }
      }

      /// <summary>
      /// Converts a dictionary to a list for serialization purposes.
      /// </summary>
      /// <param name="dictionary">Dictionary to convert.</param>
      /// <returns>List that contains all the values of the dictionary.</returns>
      /// <remarks>
      /// This method is required because Dictionary can't be serialized.
      /// </remarks>
      private static List<KeyValuePair<string, object>> ConvertDictionaryToList(Dictionary<string, object> dictionary)
      {
         // Declare variables
         List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

         // Loop all items
         foreach (KeyValuePair<string, object> dictionaryItem in dictionary)
         {
            result.Add(new KeyValuePair<string, object>(dictionaryItem.Key, dictionaryItem.Value));
         }

         // Return result
         return result;
      }

      /// <summary>
      /// Converts a list to a dictionary for serialization purposes.
      /// </summary>
      /// <param name="list">List to convert.</param>
      /// <returns>Dictionary that contains all the values of the list.</returns>
      private Dictionary<string, object> ConvertListToDictionary(IEnumerable<KeyValuePair<string, object>> list)
      {
         // Declare variables
         Dictionary<string, object> result = new Dictionary<string, object>();

         // Loop all items
         foreach (KeyValuePair<string, object> listItem in list)
         {
            // Check if the property is registered
            if (IsPropertyRegistered(listItem.Key))
            {
               // Check if the property is null
               if (listItem.Value != null)
               {
                  // Check if the property is a collection
                  if ((listItem.Value is ICollection) && (listItem.Value is IDeserializationCallback))
                  {
                     // Get the deserialization callback
                     IDeserializationCallback propertyDeserializationCallback = listItem.Value as IDeserializationCallback;

                     // Call it since collections need this call to contain valid items
                     propertyDeserializationCallback.OnDeserialization(this);
                  }
               }

               // Store the value (since deserialized values always override default values)
               result[listItem.Key] = listItem.Value;
            }
         }

         // Return result
         return result;
      }

      #endregion

      #region ICloneable Members

      /// <summary>
      /// Clones the current object.
      /// </summary>
      /// <returns>Clone of the object.</returns>
      public object Clone()
      {
         // Declare variables
         Object clone = null;

         try
         {
            // Create memory stream & formatter
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            // Serialize to memory
            formatter.Serialize(stream, this);

            // Move to the beginning of the stream
            stream.Seek(0, 0);

            // Deserialize from memory
            clone = formatter.Deserialize(stream);

            // Close memory stream
            stream.Close();
         }
         catch (Exception ex)
         {
            // Trace
            Trace.TraceError(ex.ToString());
         }

         // Return clone
         return clone;
      }

      #endregion

      #region IDataWarningInfo Members

      /// <summary>
      /// Gets the current warning.
      /// </summary>
      string IDataWarningInfo.Warning
      {
         get
         {
            // Declare variables
            string warning = string.Empty;

            // If not validated, validate
            if (!IsValidated) Validate();

            // Check if any warnings occurred
            if ((_businessWarnings != null) && (_businessWarnings.Count > 0))
            {
               // Yes, retrieve the first one
               warning = _businessWarnings[0];
            }

            // Return result
            return warning;
         }
      }

      /// <summary>
      /// Gets a warning for a specific column.
      /// </summary>
      /// <param name="columnName">Column name.</param>
      /// <returns>Warning.</returns>
      string IDataWarningInfo.this[string columnName]
      {
         get
         {
            // Declare variables
            string warning = string.Empty;

            // If not validated, validate
            if (!IsValidated) Validate();

            // Check if the warning is available (and thus occurred)
            if ((_fieldWarnings != null) && _fieldWarnings.ContainsKey(columnName))
            {
               // Yes, retrieve the warning
               warning = _fieldWarnings[columnName];
            }

            // Return result
            return warning;
         }
      }

      #endregion

      #region IDataErrorInfo Members

      /// <summary>
      /// Gets the current error.
      /// </summary>
      string IDataErrorInfo.Error
      {
         get
         {
            // Declare variables
            string error = string.Empty;

            // If not validated, validate
            if (!IsValidated) Validate();

            // Check if any errors occurred
            if ((_businessErrors != null) && (_businessErrors.Count > 0))
            {
               // Yes, retrieve the first one
               error = _businessErrors[0];
            }

            // Return result
            return error;
         }
      }

      /// <summary>
      /// Gets an error for a specific column.
      /// </summary>
      /// <param name="columnName">Column name.</param>
      /// <returns>Error.</returns>
      string IDataErrorInfo.this[string columnName]
      {
         get
         {
            // Declare variables
            string error = string.Empty;

            // If not validated, validate
            if (!IsValidated) Validate();

            // Check if the error is available (and thus occurred)
            if ((_fieldErrors != null) && _fieldErrors.ContainsKey(columnName))
            {
               // Yes, retrieve the error
               error = _fieldErrors[columnName];
            }

            // Return result
            return error;
         }
      }

      #endregion

      #region IEditableObject Members

      /// <summary>
      /// Begins an edit on an object.
      /// </summary>
      public void BeginEdit()
      {
         // Check if there already is a backup
         if (_backup != null)
         {
            throw new InvalidOperationException("BeginEdit cannot be invoked twice. A call to BeginEdit must always be closed with a call to CancelEdit or EndEdit.");
         }

         // Create backup
         _backup = new BackupData(this);
      }

      /// <summary>
      /// Discards changes since the last <see cref="IEditableObject.BeginEdit()"/> call.
      /// </summary>
      public void CancelEdit()
      {
         // Check if there is a backup
         if (_backup == null) return;

         // Restore the backup
         _backup.RestoreBackup();
         _backup = null;
      }

      /// <summary>
      /// Pushes changes since the last <see cref="IEditableObject.BeginEdit()"/> call.
      /// </summary>
      public void EndEdit()
      {
         // Just clear the backup
         _backup = null;
      }

      #endregion

   }
}
