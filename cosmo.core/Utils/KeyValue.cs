namespace Cosmo.Utils
{
   /// <summary>
   /// Implementa un contenedor de valores para usar en controles UI tipo lista (u cualquier otro ámbito).
   /// </summary>
   /// <remarks>
   /// This class should be substituted by <see cref="KeyValuePair"/> in a short future.
   /// </remarks>
   public class KeyValue
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="KeyValue"/>.
      /// </summary>
      public KeyValue()
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="KeyValue"/>.
      /// </summary>
      /// <param name="label"></param>
      /// <param name="value"></param>
      public KeyValue(string label, string value)
      {
         Initialize();

         Label = label;
         Value = value;
      }

      /// <summary>
      /// Gets a new instance of <see cref="KeyValue"/>.
      /// </summary>
      /// <param name="label"></param>
      /// <param name="value"></param>
      public KeyValue(string label, int value)
      {
         Initialize();

         Label = label;
         Value = value.ToString();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets la etiqueta visible que verá el usuario.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Gets or sets el valor asociado a la etiqueta y que recogerá el control.
      /// </summary>
      public string Value { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         Label = string.Empty;
         Value = string.Empty;
      }

      #endregion

   }
}
