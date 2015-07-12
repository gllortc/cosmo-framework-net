using System;

namespace Cosmo.Security
{
   /// <summary>
   /// Excepción que se lanza cuando la cuenta de usuario está inhabilitada.
   /// </summary>
   public class UserDisabledException : ApplicationException
   {
      /// <summary>
      /// Gets a new instance of <see cref="UserDisabledException"/>.
      /// </summary>
      public UserDisabledException() : base() { }

      /// <summary>
      /// Gets a new instance of <see cref="UserDisabledException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public UserDisabledException(string s) : base(s) { }

      /// <summary>
      /// Gets a new instance of <see cref="UserDisabledException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public UserDisabledException(string s, Exception ex) : base(s, ex) { }
   }
}
