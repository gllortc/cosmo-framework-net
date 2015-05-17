using System;

namespace Cosmo.Data.ORM
{
   /// <summary>
   /// Implementa un mapeo de campo de formulario ORM.
   /// </summary>
   [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field, AllowMultiple = false)]
   public class ValidInteger : System.Attribute
   {
      // Declaración de variables internas
      private bool _isRequired;
      private Int64 _min;
      private Int64 _max;

      /// <summary>
      /// Devuelve una instancia de <see cref="ValidInteger"/>.
      /// </summary>
      public ValidInteger()
      {
         Initialize();
      }

      public bool Required
      {
         get { return _isRequired; }
         set { _isRequired = value; }
      }

      public Int64 Min
      {
         get { return _min; }
         set { _min = value; }
      }

      public Int64 Max
      {
         get { return _max; }
         set { _max = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _isRequired = false;
         _min = -99999999;
         _max = -99999999;
      }
   }
}
