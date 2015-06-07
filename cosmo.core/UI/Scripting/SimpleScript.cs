using System.Text;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Representa un script que debe ser adjuntado a una página (o control).
   /// </summary>
   public class SimpleScript : Script
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="SimpleScript"/>.
      /// </summary>
      /// <param name="viewport">Instancia de <see cref="View"/> que actúa de contenedor del control actual.</param>
      public SimpleScript(View viewport)
         : base(viewport)
      {

      }

      /// <summary>
      /// Devuelve una instancia de <see cref="SimpleScript"/>.
      /// </summary>
      /// <param name="viewport">Instancia de <see cref="View"/> que actúa de contenedor del control actual.</param>
      /// <param name="source">Una cadena que contiene el código JavaScript.</param>
      public SimpleScript(View viewport, string source)
         : base(viewport, source)
      { }

      /// <summary>
      /// Devuelve una instancia de <see cref="SimpleScript"/>.
      /// </summary>
      /// <param name="viewport">Instancia de <see cref="ICosmoViewport"/> que actúa de contenedor del control actual.</param>
      /// <param name="source"´>Una instancia de <see cref="StringBuilder"/> que contiene código JavaScript.</param>
      public SimpleScript(View viewport, StringBuilder source)
         : base(viewport, source)
      { }

      #endregion

      #region IScript Implementation

      /// <summary>
      /// Devuelve el código JavaScript que se debe incorporar en la página.
      /// </summary>
      /// <returns>Una cadena que contiene código JavaScript.</returns>
      public override string GetSource()
      {
         return Source.ToString();
      }

      #endregion

   }
}
