using System;
using System.Globalization;

namespace Cosmo.Data
{
   /// <summary>
   /// Exception is thrown when a property value is set to null but when the type does not support
   /// null values.
   /// </summary>
   public class PropertyNotNullableException : Exception
   {
      /// <summary>
      /// Initializes a new instance of this exception.
      /// </summary>
      /// <param name="name">Name of the property that caused the exception.</param>
      /// <param name="type">Type of the object that is trying to register the property.</param>
      public PropertyNotNullableException(string name, Type type)
         : base(string.Format(CultureInfo.InvariantCulture, "Property '{0}' on type '{1}' does not support null-values", name, type)) { }
   }
}
