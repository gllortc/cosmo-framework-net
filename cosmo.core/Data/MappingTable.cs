﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmo.Data.ORM
{
   [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
   public class MappingTable : System.Attribute
   {
      public MappingTable()
      {

      }
   }
}
