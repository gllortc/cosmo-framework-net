using System;

namespace Cosmo.Security
{

   #region InvalidSessionException

   /// <summary>
   /// Excepción de error de sesión
   /// </summary>
   public class InvalidSessionException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      public InvalidSessionException() : base() { }

      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      public InvalidSessionException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public InvalidSessionException(string s, Exception ex) : base(s, ex) { }
   }

   #endregion

}
