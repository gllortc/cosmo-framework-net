using System;

namespace Cosmo.UI.Render
{
   /// <summary>
   /// Se produce cuando un módulo de renderizado no puede renderizar un determinado control.
   /// </summary>
   public class NoRenderModuleAvailableException : ApplicationException
   {
      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      public NoRenderModuleAvailableException() : base() { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      public NoRenderModuleAvailableException(string s) : base(s) { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public NoRenderModuleAvailableException(string s, Exception ex) : base(s, ex) { }
   }
}
