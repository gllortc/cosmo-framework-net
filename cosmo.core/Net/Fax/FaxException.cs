using System;
using System.Runtime.Serialization;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Exception throwed in the Fax .NET library.
   /// </summary>
   [Serializable]
   public class FaxException : Exception
   {
      /// <summary>
      /// Create an exception instance with a message.
      /// </summary>
      /// <param name="message">Message of the exception.</param>
      public FaxException(string message) : base(message) { }

      /// <summary>
      /// Create an exception instance containing an inner exception.
      /// </summary>
      /// <param name="message">Message of the exception.</param>
      /// <param name="innerException">Inner exception attached with the FaxException.</param>
      /// <remarks>Lot of exceptions are generate by a error in native function calls.
      /// The <paramref name="innerException"/> is often filled with a <see cref="System.ComponentModel.Win32Exception"/>.</remarks>
      public FaxException(string message, Exception innerException) : base(message, innerException) { }

      /// <overloads></overloads>
      protected FaxException(SerializationInfo info, StreamingContext context) : base(info, context) { }
   }

}
