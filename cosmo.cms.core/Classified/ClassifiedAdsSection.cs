namespace Cosmo.Cms.Classified
{

   /// <summary>
   /// Implementa una carpeta que contiene anuncios clasificados en su interior.
   /// </summary>
   public class ClassifiedAdsSection
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ClassifiedAdsSection"/>.
      /// </summary>
      public ClassifiedAdsSection()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del objeto.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Devuelve o establece el nombre de la carpeta.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Devuelve o establece la descripción de la carpeta.
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
      /// Devuelve o establece el estado de publicación del objeto.
      /// </summary>
      public bool Enabled { get; set; }

      /// <summary>
      /// Devuelve o establece el número de elementos que contiene la carpeta.
      /// </summary>
      public int Objects { get; set; }

      #endregion

      #region Private members

      /// <summary>
      /// Inicializa la instancia.
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