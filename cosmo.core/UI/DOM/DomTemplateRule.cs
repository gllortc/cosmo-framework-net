namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa una regla de aplicación de plantillas.
   /// </summary>
   public class DomTemplateRule
   {
      int _template;
      string _ruleText;
      string _regexp;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplateRule"/>.
      /// </summary>
      public DomTemplateRule() 
      {
         _template = 0;
         _ruleText = string.Empty;
         _regexp = string.Empty;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador de la plantilla a aplicar.
      /// </summary>
      public int Template
      {
         get { return _template; }
         set { _template = value; }
      }

      /// <summary>
      /// Devuelve o establece el texto de la regla.
      /// </summary>
      public string ContainText
      {
         get { return _ruleText; }
         set { _ruleText = value; }
      }

      /// <summary>
      /// Devuelve o establece la expresión regular.
      /// </summary>
      public string RegularExpression
      {
         get { return _regexp; }
         set { _regexp = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// 
      /// </summary>
      /// <param name="UserAgent"></param>
      /// <returns></returns>
      public bool Check(string UserAgent)
      {
         if (UserAgent.ToLower().Contains(_ruleText.ToLower()))
            return true;

         return false;
      }

      #endregion

   }

}
