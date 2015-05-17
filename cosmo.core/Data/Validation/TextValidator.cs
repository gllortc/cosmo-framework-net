using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmo.Data.Validation
{
   public class TextValidator : IValidator
   {
      private bool _required;

      public TextValidator(bool required)
      {
         Initialize();

         _required = required;
      }

      public bool Validate(string value)
      {
         bool valid = true;

         valid = valid & string.IsNullOrWhiteSpace(value.Trim());

         return valid;
      }

      private void Initialize()
      {
         _required = false;
      }
   }
}
