using System.Text;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Representa un elemento de la barra de navegación específico para el inicio de sesión y el acceso a la cuenta de los usuarios.
   /// </summary>
   public class NavbarBrand : NavbarIButton
   {
      // Declaración de variables internas
      private Workspace _ws;

      /// <summary>
      /// Devuelve una instancia de <see cref="NavbarBrand"/>.
      /// </summary>
      /// <param name="parentViewport">Página o contenedor dónde se representará el control.</param>
      public NavbarBrand(ICosmoViewport parentViewport)
         : base(parentViewport)
      {
      }

      /// <summary>
      /// Convierte el componente en código XHTML.
      /// </summary>
      /// <returns>Una cadena que contiene el código XHTML necesario para representar el componente en un navegador.</returns>
      public new string ToXhtml()
      {
         StringBuilder xhtml = new StringBuilder();

         return xhtml.ToString();
      }
   }
}
