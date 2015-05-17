using System;

namespace Cosmo.Communications
{

   #region class InvalidMailException : ApplicationException

   /// <summary>
   /// Excepción que se lanza cuando la cuenta de correo electrónico no existe o no está asociada a ninguna cuenta de usuario.
   /// </summary>
   public class InvalidMailException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      public InvalidMailException() : base() { }

      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      public InvalidMailException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public InvalidMailException(string s, Exception ex) : base(s, ex) { }
   }

   #endregion

}
