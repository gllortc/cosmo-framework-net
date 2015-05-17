using System;
using System.Globalization;

namespace Cosmo.Data
{
   /// <summary>
   /// Exception is thrown when a property is added to the <see cref="DataObjectBase{T}"/> class that is
   /// already registered by the object.
   /// </summary>
   public class PropertyAlreadyRegisteredException : Exception
   {
      /// <summary>
      /// Initializes a new instance of this exception.
      /// </summary>
      /// <param name="name">Name of the property that caused the exception.</param>
      /// <param name="type">Type of the object that is trying to register the property.</param>
      public PropertyAlreadyRegisteredException(string name, Type type)
         : base(string.Format(CultureInfo.InvariantCulture, "Property '{0}' is already registered on type '{1}'", name, type)) { }
   }
}
