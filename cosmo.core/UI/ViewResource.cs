namespace Cosmo.UI
{
   /// <summary>
   /// Representa un recurso necesario para representar una página XHTML de Cosmo.
   /// </summary>
   public class ViewResource
   {

      #region Enumerations

      /// <summary>
      /// Enumera los tipos de recurso admitidos.
      /// </summary>
      public enum ResourceType
      {
         /// <summary>Hoja de estilos CSS.</summary>
         CSS,
         /// <summary>Archivo de código JavaScript.</summary>
         JavaScript
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ViewResource"/>.
      /// </summary>
      public ViewResource()
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ViewResource"/>.
      /// </summary>
      /// <param name="type">Tipo de recurso.</param>
      /// <param name="filePath">Ruta al recurso.</param>
      public ViewResource(ResourceType type, string filePath)
      {
         Initialize();

         this.Type = type;
         this.FilePath = filePath;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el tipo de recurso.
      /// </summary>
      public ResourceType Type { get; set; }

      /// <summary>
      /// Gets or sets el nombre del archivo (y la ruta de acceso).
      /// </summary>
      public string FilePath { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Type = ResourceType.CSS;
         this.FilePath = string.Empty;
      }

      #endregion

   }
}
