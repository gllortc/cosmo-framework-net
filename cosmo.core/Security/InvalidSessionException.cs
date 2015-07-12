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
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      public InvalidSessionException() : base() { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      public InvalidSessionException(string s) : base(s) { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public InvalidSessionException(string s, Exception ex) : base(s, ex) { }
   }

   #endregion

}
