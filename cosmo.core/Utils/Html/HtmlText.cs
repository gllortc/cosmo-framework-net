namespace Cosmo.Utils.Html
{

   /// <summary>
   /// Representa un fragmento de texto HTML.
   /// </summary>
   public class HtmlText
   {
      private string _text = "";

      /// <summary>
      /// Devuelve una instancia de HtmlText.
      /// </summary>
      public HtmlText() { }

      /// <summary>
      /// Devuelve una instancia de HtmlText.
      /// </summary>
      public HtmlText(string text)
      {
         _text = text;
      }

      /// <summary>
      /// Devuelve o establece el fragmento de texto.
      /// </summary>
      public string Text
      {
         get { return _text; }
         set { _text = value.Trim().ToLower(); }
      }

      /// <summary>
      /// Convierte el objeto a una cadena de texto agregable al documento HTML.
      /// </summary>
      /// <returns>Una cadena de texto presentable en un documento HTML.</returns>
      public string ToXhtml()
      {
         return _text + " ";
      }
   }

}
