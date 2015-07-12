using System;

namespace Cosmo.Security.Auth
{

   /// <summary>
   /// Excepción que se lanza cuando la cuenta de usuario está inhabilitada.
   /// </summary>
   public class AuthenticationException : ApplicationException
   {
      /// <summary>
      /// Gets a new instance of <see cref="AuthenticationException"/>.
      /// </summary>
      public AuthenticationException() 
         : base() { }

      /// <summary>
      /// Gets a new instance of <see cref="AuthenticationException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public AuthenticationException(string s) 
         : base(s) { }

      /// <summary>
      /// Gets a new instance of <see cref="AuthenticationException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public AuthenticationException(string s, Exception ex) 
         : base(s, ex) { }
   }

}
