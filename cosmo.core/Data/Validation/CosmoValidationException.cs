using System;

namespace Cosmo.Data.Validation
{

   /// <summary>
   /// Implementa la excepción que se lanza cuando una validación no cumple con los requisitos establecidos.
   /// </summary>
   public class CosmoValidationException : Exception
   {
      private string _msg = string.Empty;

      /// <summary>
      /// Devuelve una instancia de CosmoValidationException.
      /// </summary>
      public CosmoValidationException() { }

      /// <summary>
      /// Devuelve una instancia de CosmoValidationException.
      /// </summary>
      /// <param name="message">Descripción del error.</param>
      public CosmoValidationException(string message)
      {
         _msg = message;
      }

      /// <summary>
      /// Devuelve el mensaje descriptivo del error.
      /// </summary>
      public new string Message
      {
         get { return _msg; }
      }
   }

}
