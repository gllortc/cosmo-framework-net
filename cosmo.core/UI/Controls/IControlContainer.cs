using System;
using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Interface that exposes the methods shared with control containers.
   /// </summary>
   public interface IControlContainer
   {
      /// <summary>
      /// Gets all controls of a concrete type.
      /// </summary>
      /// <param name="controlType">Type of control.</param>
      /// <returns>A list of requested controls.</returns>
      List<Control> GetControlsByType(Type controlType);
   }
}
