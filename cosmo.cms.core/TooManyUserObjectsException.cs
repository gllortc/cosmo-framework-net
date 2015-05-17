using System;

namespace Cosmo.Cms
{
   public class TooManyUserObjectsException : ApplicationException
   {
      public TooManyUserObjectsException() : base() { }

      public TooManyUserObjectsException(string message) : base(message) { }
   }
}
