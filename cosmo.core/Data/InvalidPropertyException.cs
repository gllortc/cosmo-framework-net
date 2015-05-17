using System;
using System.Globalization;

namespace Cosmo.Data
{
   /// <summary>
   /// Exception is thrown when an invalid property is added to the <see cref="DataObjectBase{T}"/> class.
   /// </summary>
   public class InvalidPropertyException : Exception
   {
      /// <summary>
      /// Initializes a new instance of this exception.
      /// </summary>
      /// <param name="name">Name of the property that caused the exception.</param>
      public InvalidPropertyException(string name)
         : base(string.Format(CultureInfo.InvariantCulture, "Property '{0}' is invalid (not serializable?)",
            string.IsNullOrEmpty(name) ? "null reference property" : name)) { }
   }
}
