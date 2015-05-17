using System;

namespace Cosmo.Security
{
   /// <summary>
   /// Excepción que se lanza cuando la cuenta de usuario no está verificada.
   /// </summary>
   public class UserNotVerifiedException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="UserNotVerifiedException"/>.
      /// </summary>
      public UserNotVerifiedException() : base() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="UserNotVerifiedException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public UserNotVerifiedException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de <see cref="UserNotVerifiedException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public UserNotVerifiedException(string s, Exception ex) : base(s, ex) { }
   }
}
