using System;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Representa una excepción que se produce al renderizar una página de DOM.
   /// </summary>
   public class DomRenderException : Exception
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="DomRenderException"/>.
      /// </summary>
      public DomRenderException() : base() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomRenderException"/>.
      /// </summary>
      public DomRenderException(string message) : base(message) { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomRenderException"/>.
      /// </summary>
      public DomRenderException(string message, Exception innerException) : base(message, innerException) { }
   }
}
