namespace Cosmo.Cms.Model.Ads
{

   /// <summary>
   /// Implementa una carpeta que contiene anuncios clasificados en su interior.
   /// </summary>
   public class AdsSection
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="AdsSection"/>.
      /// </summary>
      public AdsSection()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del objeto.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Gets or sets el nombre de la carpeta.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets la descripción de la carpeta.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Indica si en las listas de selección sale seleccionada por defecto.
      /// </summary>
      public bool IsListDefault { get; set; }

      /// <summary>
      /// Indica si el elemento no es seleccionable en las listas de selección.
      /// </summary>
      public bool IsNotSelectable { get; set; }

      /// <summary>
      /// Gets or sets el estado de publicación del objeto.
      /// </summary>
      public bool Enabled { get; set; }

      /// <summary>
      /// Gets or sets el número de elementos que contiene la carpeta.
      /// </summary>
      public int Objects { get; set; }

      #endregion

      #region Private members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.ID = 0;
         this.Name = string.Empty;
         this.Description = string.Empty;
         this.Enabled = true;
         this.IsListDefault = false;
         this.IsNotSelectable = false;
         this.Objects = 0;
      }

      #endregion

   }
}