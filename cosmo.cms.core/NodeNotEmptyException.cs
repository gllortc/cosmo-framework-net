using System;

namespace Cosmo.Cms
{
   public class NodeNotEmptyException : ApplicationException
   {
      public NodeNotEmptyException() : base() { }

      public NodeNotEmptyException(string message) : base(message) { }
   }
}
