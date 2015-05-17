using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Interface que deben cumplir los controles que contienen a su vez elementos contenedores,
   /// como Items, Carrouseles, etc.
   /// </summary>
   public interface IControlCollectionContainer
   {
      /// <summary>
      /// Colección de controles contenedores que son la base del control.
      /// </summary>
      List<IControlSingleContainer> NestedContainers { get; }
   }
}
