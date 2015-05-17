using System;
using System.Globalization;

namespace Cosmo.Data
{
   /// <summary>
   /// Exception is thrown when a property is used by the <see cref="DataObjectBase{T}"/> class that is
   /// not registered by the object.
   /// </summary>
   public class PropertyNotRegisteredException : Exception
   {
      /// <summary>
      /// Initializes a new instance of this exception.
      /// </summary>
      /// <param name="name">Name of the property that caused the exception.</param>
      /// <param name="type">Type of the object that is trying to register the property.</param>
      public PropertyNotRegisteredException(string name, Type type)
         : base(string.Format(CultureInfo.InvariantCulture, "Property '{0}' is not registered on type '{1}'", name, type)) { }
   }
}
