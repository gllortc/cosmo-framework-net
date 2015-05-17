namespace Cosmo.Utils.Html
{

   /// <summary>
   /// Representa un atributo de un TAG Html.
   /// </summary>
   class HtmlAttribute
   {
      string _name;
      string _value;

      /// <summary>
      /// Devuelve una instancia de HtmlAttribute.
      /// </summary>
      public HtmlAttribute(string name)
      {
         _name = name;
         _value = "";
      }

      /// <summary>
      /// Devuelve una instancia de HtmlAttribute.
      /// </summary>
      public HtmlAttribute(string name, string value)
      {
         _name = name;
         _value = value;
      }

      /// <summary>
      /// Devuelve o establece el nombre del parámetro.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Devuelve o establece el valor del parámetro.
      /// </summary>
      public string Value
      {
         get { return _value; }
         set { _value = value; }
      }
   }

}
