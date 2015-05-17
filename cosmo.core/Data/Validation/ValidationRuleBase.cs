namespace Cosmo.Data.Validation
{

   /// <summary>
   /// Implementa una regla de validación básica. Sirve para implementar reglas de validación específicas.
   /// </summary>
   public abstract class ValidationRuleBase
   {
      bool _required = false;

      #region Settings

      /// <summary>
      /// Indica si el campo es obligatorio.
      /// </summary>
      public bool Required
      {
         get { return _required; }
         set { _required = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Chequea la regla a partir del valor proporcionado.
      /// </summary>
      /// <param name="value">Valor a validar obtenido del formulario.</param>
      /// <returns><c>true</c> si el valor es válido o <c>false</c> en cualquier otro caso.</returns>
      public abstract bool CheckRule(string value);

      /// <summary>
      /// Devuelve una cadena de configuración de la regla para un script que use jQuery Validation..
      /// </summary>
      public abstract string GetValiadtionRulesScript(string name);

      /// <summary>
      /// Devuelve una cadena de configuración los mensajes a mostrar para un script que use jQuery Validation..
      /// </summary>
      public abstract string GetValiadtionMessagesScript(string name);

      #endregion

   }

}
