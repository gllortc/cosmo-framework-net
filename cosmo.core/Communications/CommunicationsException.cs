using System;

namespace Cosmo.Communications
{
   /// <summary>
   /// Excepción que se lanza cuando la cuenta de correo electrónico no existe o no está asociada a ninguna cuenta de usuario.
   /// </summary>
   public class CommunicationsException : ApplicationException
   {
      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      public CommunicationsException() : base() { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      public CommunicationsException(string s) : base(s) { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public CommunicationsException(string s, Exception ex) : base(s, ex) { }
   }
}
