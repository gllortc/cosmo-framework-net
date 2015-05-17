using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmo.Data.Validation
{
   public interface IValidator
   {
      bool Validate(string value);
   }
}
