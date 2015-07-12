using System;

namespace Cosmo.Security
{
   /// <summary>
   /// Excepción que se lanza cuando la cuenta de usuario no está verificada.
   /// </summary>
   public class UserNotVerifiedException : ApplicationException
   {
      /// <summary>
      /// Gets a new instance of <see cref="UserNotVerifiedException"/>.
      /// </summary>
      public UserNotVerifiedException() : base() { }

      /// <summary>
      /// Gets a new instance of <see cref="UserNotVerifiedException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public UserNotVerifiedException(string s) : base(s) { }

      /// <summary>
      /// Gets a new instance of <see cref="UserNotVerifiedException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public UserNotVerifiedException(string s, Exception ex) : base(s, ex) { }
   }
}
