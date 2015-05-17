using System;

namespace Cosmo.Security
{
   /// <summary>
   /// Excepción que se lanza cuando la cuenta de usuario está inhabilitada.
   /// </summary>
   public class UserDisabledException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="UserDisabledException"/>.
      /// </summary>
      public UserDisabledException() : base() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="UserDisabledException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public UserDisabledException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de <see cref="UserDisabledException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public UserDisabledException(string s, Exception ex) : base(s, ex) { }
   }
}
