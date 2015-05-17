using System;

namespace Cosmo.Communications
{
   /// <summary>
   /// Excepción que se lanza cuando la cuenta de correo electrónico no existe o no está asociada a ninguna cuenta de usuario.
   /// </summary>
   public class CommunicationsException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      public CommunicationsException() : base() { }

      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      public CommunicationsException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public CommunicationsException(string s, Exception ex) : base(s, ex) { }
   }
}
