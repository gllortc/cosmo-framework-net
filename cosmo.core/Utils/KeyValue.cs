namespace Cosmo.Utils
{
   /// <summary>
   /// Implementa un contenedor de valores para usar en controles UI tipo lista (u cualquier otro ámbito).
   /// </summary>
   public class KeyValue
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="KeyValue"/>.
      /// </summary>
      public KeyValue()
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="KeyValue"/>.
      /// </summary>
      /// <param name="label"></param>
      /// <param name="value"></param>
      public KeyValue(string label, string value)
      {
         Initialize();

         Label = label;
         Value = value;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece la etiqueta visible que verá el usuario.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Devuelve o establece el valor asociado a la etiqueta y que recogerá el control.
      /// </summary>
      public string Value { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         Label = string.Empty;
         Value = string.Empty;
      }

      #endregion

   }
}
