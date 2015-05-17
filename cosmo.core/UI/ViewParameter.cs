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
      /// Devuelve una instancia de <see cref="ViewParameter"/>.
      /// </summary>
      public ViewParameter()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el nombre del parámetro.
      /// </summary>
      public string ParameterName { get; set; }

      /// <summary>
      /// Devuelve o establece el nombre de la propiedad a la que se debe asociar.
      /// </summary>
      public string PropertyName { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.ParameterName = string.Empty;
         this.PropertyName = string.Empty;
      }

      #endregion

   }
}
