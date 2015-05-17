namespace Cosmo.UI
{
   /// <summary>
   /// Clase que deben implementar los <em>layouts</em>.
   /// </summary>
   public abstract class ILayout
   {
      /// <summary>
      /// Convierte el componente en código XHTML.
      /// </summary>
      /// <returns>Una cadena que contiene el código XHTML necesario para representar el componente en un navegador.</returns>
      public abstract string ToXhtml();

      /// <summary>
      /// Convierte el componente en código XHTML.
      /// </summary>
      /// <returns>Una cadena que contiene el código XHTML necesario para representar el componente en un navegador.</returns>
      public override string ToString()
      {
         return this.ToXhtml();
      }
   }
}
