using System;

namespace Cosmo.Security
{
   /// <summary>
   /// Excepción de error de sesión.
   /// </summary>
   public class SecurityException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="SecurityException"/>.
      /// </summary>
      public SecurityException() : base() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="SecurityException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public SecurityException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de <see cref="SecurityException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public SecurityException(string s, Exception ex) : base(s, ex) { }
   }
}
