using System;

namespace Cosmo
{
   /// <summary>
   /// Se produce cuando un módulo de renderizado no puede renderizar un determinado control.
   /// </summary>
   public class WorkspaceSettingsException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      public WorkspaceSettingsException() : base() { }

      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      public WorkspaceSettingsException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de InvalidSessionException
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public WorkspaceSettingsException(string s, Exception ex) : base(s, ex) { }
   }
}
