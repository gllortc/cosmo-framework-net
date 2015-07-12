using System;

namespace Cosmo.Security
{
   /// <summary>
   /// Excepción de error de sesión.
   /// </summary>
   public class SecurityException : ApplicationException
   {
      /// <summary>
      /// Gets a new instance of <see cref="SecurityException"/>.
      /// </summary>
      public SecurityException() : base() { }

      /// <summary>
      /// Gets a new instance of <see cref="SecurityException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public SecurityException(string s) : base(s) { }

      /// <summary>
      /// Gets a new instance of <see cref="SecurityException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public SecurityException(string s, Exception ex) : base(s, ex) { }
   }
}
