using System;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Excepción que indica que no se ha encontrado un determinado control
   /// </summary>
   public class ControlNotFoundException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="OrmException"/>.
      /// </summary>
      public ControlNotFoundException() : base() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="OrmException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public ControlNotFoundException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de <see cref="OrmException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public ControlNotFoundException(string s, Exception ex) : base(s, ex) { }
   }
}
