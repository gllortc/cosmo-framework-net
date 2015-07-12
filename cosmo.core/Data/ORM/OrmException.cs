using System;

namespace Cosmo.Data.ORM
{
   /// <summary>
   /// Excepción de error en procesos de modelado ORM.
   /// </summary>
   public class OrmException : ApplicationException
   {
      /// <summary>
      /// Gets a new instance of <see cref="OrmException"/>.
      /// </summary>
      public OrmException() : base() { }

      /// <summary>
      /// Gets a new instance of <see cref="OrmException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      public OrmException(string s) : base(s) { }

      /// <summary>
      /// Gets a new instance of <see cref="OrmException"/>.
      /// </summary>
      /// <param name="s">Mensaje</param>
      /// <param name="ex">Excepción</param>
      public OrmException(string s, Exception ex) : base(s, ex) { }
   }
}
