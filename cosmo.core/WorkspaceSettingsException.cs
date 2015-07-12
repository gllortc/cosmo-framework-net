using System;

namespace Cosmo
{
   /// <summary>
   /// Se produce cuando un módulo de renderizado no puede renderizar un determinado control.
   /// </summary>
   public class WorkspaceSettingsException : ApplicationException
   {
      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      public WorkspaceSettingsException() : base() { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      public WorkspaceSettingsException(string s) : base(s) { }

      /// <summary>
      /// Gets a new instance of InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public WorkspaceSettingsException(string s, Exception ex) : base(s, ex) { }
   }
}
