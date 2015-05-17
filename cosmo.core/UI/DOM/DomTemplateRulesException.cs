using System;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Representa una excepción que se produce a no poder aplicar ninuga plantilla de presentación según las reglas especificadas.
   /// </summary>
   public class DomTemplateRulesException : Exception
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="DomRenderException"/>.
      /// </summary>
      public DomTemplateRulesException() : base() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomRenderException"/>.
      /// </summary>
      public DomTemplateRulesException(string message) : base(message) { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomRenderException"/>.
      /// </summary>
      public DomTemplateRulesException(string message, Exception innerException) : base(message, innerException) { }
   }
}
