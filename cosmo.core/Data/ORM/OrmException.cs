using System;

namespace Cosmo.Data.ORM
{
   /// <summary>
   /// Excepción de error en procesos de modelado ORM.
   /// </summary>
   public class OrmException : ApplicationException
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="OrmException"/>.
      /// </summary>
      public OrmException() : base() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="OrmException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public OrmException(string s) : base(s) { }

      /// <summary>
      /// Devuelve una instancia de <see cref="OrmException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public OrmException(string s, Exception ex) : base(s, ex) { }
   }
}
