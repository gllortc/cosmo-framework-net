using System;
using System.Globalization;

namespace Cosmo.Data
{
   /// <summary>
   /// Exception is thrown when an the new value of a property of the <see cref="DataObjectBase{T}"/> class is invalid.
   /// </summary>
   public class InvalidPropertyValueException : Exception
   {
      /// <summary>
      /// Initializes a new instance of this exception.
      /// </summary>
      /// <param name="name">Name of the property that caused the exception.</param>
      /// <param name="expectedType">Expected type for the property.</param>
      /// <param name="actualType">Actual object value type.</param>
      public InvalidPropertyValueException(string name, Type expectedType, Type actualType)
         : base(string.Format(CultureInfo.InvariantCulture, "Expected a value of type '{0} instead of '{1}' for property '{2}'",
            expectedType, actualType, name)) { }
   }
}
