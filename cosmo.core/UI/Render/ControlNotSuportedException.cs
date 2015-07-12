using System;

namespace Cosmo.UI.Render
{
   /// <summary>
   /// Se produce cuando un módulo de renderizado no puede renderizar un determinado control.
   /// </summary>
   public class ControlNotSuportedException : ApplicationException
   {
      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      public ControlNotSuportedException() : base() { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      public ControlNotSuportedException(string s) : base(s) { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public ControlNotSuportedException(string s, Exception ex) : base(s, ex) { }
   }
}
