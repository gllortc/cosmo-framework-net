namespace Cosmo.UI
{
   /// <summary>
   /// Permite definir los parámetros de llamada de una vista.
   /// </summary>
   [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
   public class ViewParameter : System.Attribute
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ViewParameter"/>.
      /// </summary>
      public ViewParameter()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el nombre del parámetro.
      /// </summary>
      public string ParameterName { get; set; }

      /// <summary>
      /// Gets or sets el nombre de la propiedad a la que se debe asociar.
      /// </summary>
      public string PropertyName { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.ParameterName = string.Empty;
         this.PropertyName = string.Empty;
      }

      #endregion

   }
}
