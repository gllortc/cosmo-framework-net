using System;

namespace Cosmo.UI.DOM.Templates
{

   /// <summary>
   /// Excepción de error aplicación de plantilla
   /// </summary>
   public class InvalidBrowserAgentException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de InvalidBrowserAgentException.
      /// </summary>
      public InvalidBrowserAgentException() : base() { }

      /// <summary>
      /// Devuelve una instancia de InvalidBrowserAgentException.
      /// </summary>
      public InvalidBrowserAgentException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de InvalidBrowserAgentException.
      /// </summary>
      public InvalidBrowserAgentException(string s, Exception ex) : base(s, ex) { }
   }
}