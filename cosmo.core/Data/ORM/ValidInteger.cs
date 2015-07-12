using System;

namespace Cosmo.Data.ORM
{
   /// <summary>
   /// Implementa un mapeo de campo de formulario ORM.
   /// </summary>
   [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field, AllowMultiple = false)]
   public class ValidInteger : System.Attribute
   {
      // Internal data declarations
      private bool _isRequired;
      private Int64 _min;
      private Int64 _max;

      /// <summary>
      /// Gets a new instance of <see cref="ValidInteger"/>.
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
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _isRequired = false;
         _min = -99999999;
         _max = -99999999;
      }
   }
}
