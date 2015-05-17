using Cosmo.Utils;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Interface que deben cumplir los controles contenedores simples (una zona).
   /// </summary>
   public interface IControlSingleContainer
   {
      /// <summary>
      /// Devuelve o establece el contenido del contenedor.
      /// </summary>
      ControlCollection Content { get; set; }
   }
}
