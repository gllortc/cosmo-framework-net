namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa una clase útil para usar como tipo de datos del DataTable que se usa como base del objeto MWDomTable.
   /// </summary>
   /// <remarks>
   /// Usando este tipo se pueden aplicar clases y estilos CSS distintos por cada celda o columna.
   /// </remarks>
   public class DomTableCell
   {
      private object _value;
      private string _cssStyle;
      private string _cssClass;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTableCell"/>.
      /// </summary>
      public DomTableCell() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTableCell"/>.
      /// </summary>
      /// <param name="value">Valor de la celda.</param>
      public DomTableCell(string value) 
      {
         _value = value;
      }

      #region Properties

      /// <summary>
      /// Valor de la celda de datos.
      /// </summary>
      public object Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Estilos CSS individuales.
      /// </summary>
      /// <remarks>
      /// Su contenido coincide con el atributo STYLE del tag TD.
      /// </remarks>
      public string CSSStyle
      {
         get { return _cssStyle; }
         set { _cssStyle = value; }
      }

      /// <summary>
      /// Classe/s CSS a aplicar a la celda.
      /// </summary>
      public string CSSClass
      {
         get { return _cssClass; }
         set { _cssClass = value; }
      }

      #endregion

   }

}
