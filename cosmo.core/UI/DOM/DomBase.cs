namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Clase base para los elementos DOM.
   /// </summary>
   public abstract class DomBase
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="DomBase"/>.
      /// </summary>
      public DomBase() { }

      #region Methods

      /*
      /// <summary>
      /// Devuelve la instancia de <see cref="DomTemplate"/> usada para representar el contenido XHTML.
      /// </summary>
      internal DomTemplate Template
      {
         get { return _template; }
      }
      */

      #endregion

      #region Methods

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el elemento.</returns>
      abstract public string Render(DomTemplate template);

      /// <summary>
      /// Convierte una cadena de texto en un TAG reemplazable en las secciones de la plantilla de presentación.
      /// </summary>
      protected string GetTag(string key)
      {
         return "[@" + key.Trim().ToUpper() + "]";
      }

      #endregion

   }

}
