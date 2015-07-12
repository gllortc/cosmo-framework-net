namespace Cosmo.Services
{
   /// <summary>
   /// Implementa una clase que define un pais assignable a un usuario.
   /// </summary>
   public class Country
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="Country"/>.
      /// </summary>
      public Country()
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="Country"/>.
      /// </summary>
      /// <param name="id">Identificador único del pais</param>
      /// <param name="name">Nombre del pais</param>
      /// <param name="defaultSelected">Indica si será el pais seleccionado por defecto en los listados</param>
      public Country(int id, string name, bool defaultSelected)
      {
         this.ID = id;
         this.Name = name;
         this.Default = defaultSelected;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Identificador único del pais
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// Nombre del pais
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Indica si será el pais seleccionado por defecto en los listados
      /// </summary>
      public bool Default { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.ID = 0;
         this.Name = string.Empty;
         this.Default = false;
      }

      #endregion

   }
}
