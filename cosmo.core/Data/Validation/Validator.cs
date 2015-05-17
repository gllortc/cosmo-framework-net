using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cosmo.Data.Validation
{

   /// <summary>
   /// Implementa una clase para la validación de datos proporcionados en un formulario web.
   /// </summary>
   public class Validator
   {
      bool _isValid;
      bool _useException;
      string _lastErrMsg;
      List<string> _errMsgs = null;

      private const string PATTERN_TAG_MINLEN = "%MIN_LEN%";
      private const string PATTERN_TAG_MAXLEN = "%MAX_LEN%";
      private const string REGEX_PASSWORD_PATTERN = @"(?=.*[A-Z])(?=.*[a-z])(?=.*[@#$%^&+=]|.*\d)^$";
      private const string REGEX_MAIL_PATTERN = @"^(([^<>()[\]\\.,;:\s@\""]+" +
                                                @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@" +
                                                @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}" +
                                                @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+" +
                                                @"[a-zA-Z]{2,}))$";

      /// <summary>
      /// Devuelve una instancia de Validator.
      /// </summary>
      public Validator() 
      {
         _isValid = true;
         _useException = false;
         _lastErrMsg = string.Empty;
         _errMsgs = new List<string>();
      }

      /// <summary>
      /// Devuelve una instancia de Validator.
      /// </summary>
      /// <param name="exceptionOnValidationError">Indica si se debe lanzar una excepción cuando falle una validación.</param>
      public Validator(bool exceptionOnValidationError) : this()
      {
         _useException = exceptionOnValidationError;
      }

      /// <summary>
      /// Indica si se debe lanzar una excepción cuando falle una validación.
      /// </summary>
      public bool LaunchExceptionOnValidationError
      {
         get { return _useException; }
         set { _useException = value; }
      }

      /// <summary>
      /// Indica si los campos validados son correctos o no.
      /// </summary>
      public bool IsValid
      {
         get { return _isValid; }
      }

      /// <summary>
      /// Devuelve el mensaje de error.
      /// </summary>
      public string ErrorMessage
      {
         get 
         {
            string error = string.Empty;
            foreach (string msg in _errMsgs)
            {
               error += "- " + msg + "\n";
            }
            return error; 
         }
      }

      /// <summary>
      /// Devuelve el mensaje de error en formato XHTML.
      /// </summary>
      public string ErrorXhtmlMessage
      {
         get
         {
            string error = string.Empty;
            foreach (string msg in _errMsgs)
            {
               error += "<li>" + msg + "</li>\n";
            }
            return "<ul>\n" + error + "\n</ul>\n";
         }
      }

      /// <summary>
      /// Devuelve el mensaje de error en formato strijng para usar en JavaScript.
      /// </summary>
      public string ErrorJSMessage
      {
         get
         {
            string error = string.Empty;
            foreach (string msg in _errMsgs)
            {
               error += "<li>" + msg + "</li>";
            }
            return "<ul>" + error + "</ul>";
         }
      }

      /// <summary>
      /// Permite agregar un error de forma manual para validaciones manuales.
      /// </summary>
      /// <param name="message">Mensaje de error.</param>
      public void AddError(string message)
      {
         _isValid = false;
         _errMsgs.Add(message);
      }

      /// <summary>
      /// Validación de cadenas de texto.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <param name="lengthMin">Longitud mínima.</param>
      /// <param name="lengthMax">Longitud máxima.</param>
      /// <returns>Una cadena de texto que contiene el valor validado o una cadena de texto vacía en cualquier otro caso.</returns>
      public string StringValidator(string value, string title, bool required, int lengthMin, int lengthMax)
      {
         // Formatea parámetros
         value = value.Trim();

         // Validación: la obligatoriedad
         if (required && string.IsNullOrEmpty(value))
         {
            _lastErrMsg = "El campo " + title + " es obligatorio.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }
         else if (!required && string.IsNullOrEmpty(value.Trim()))
         {
            return string.Empty;
         }

         // Validación: longitud mínima
         if (lengthMin > 0 && value.Length < lengthMin)
         {
            _lastErrMsg = "El campo " + title + " debe tener una longitud entre " + lengthMin + " y " + lengthMax + " carácteres.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         // Validación: longitud máxima
         if (lengthMax > 0 && value.Length > lengthMax)
         {
            _lastErrMsg = "El campo " + title + " debe tener una longitud entre " + lengthMin + " y " + lengthMax + " carácteres.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         return value;
      }

      /// <summary>
      /// Validación de cadenas de texto.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <returns>Una cadena de texto que contiene el valor validado o una cadena de texto vacía en cualquier otro caso.</returns>
      public string StringValidator(string value, string title, bool required)
      {
         return StringValidator(value, title, required, 0, 0);
      }

      /// <summary>
      /// Validación de cadenas de texto.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <param name="min">Valor mínimo.</param>
      /// <param name="max">Valor máximo.</param>
      /// <param name="defaultValue">Valor por defecto a devolver en caso de fallar la validación.</param>
      /// <returns>Una cadena de texto que contiene el valor validado o una cadena de texto vacía en cualquier otro caso.</returns>
      public int IntegerValidator(string value, string title, bool required, int min, int max, int defaultValue)
      {
         int retvalue = defaultValue;

         // Validación: la obligatoriedad
         if (required && string.IsNullOrEmpty(value))
         {
            _lastErrMsg = "El campo " + title + " es obligatorio.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return defaultValue;
         }
         else if (!required && string.IsNullOrEmpty(value.Trim()))
         {
            return defaultValue;
         }

         // Convierte el valor en formato texto a entero
         if (!int.TryParse(value, out retvalue)) retvalue = 0;

         // Validación: longitud mínima
         if (min > 0 && retvalue < min)
         {
            _lastErrMsg = "El campo " + title + " debe tener un valor entre " + min + " y " + max + ".";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return defaultValue;
         }

         // Validación: longitud máxima
         if (max > 0 && retvalue > max)
         {
            _lastErrMsg = "El campo " + title + " debe tener un valor entre " + min + " y " + max + ".";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return defaultValue;
         }

         return retvalue;
      }

      /// <summary>
      /// Validación de cadenas de texto.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <param name="min">Valor mínimo.</param>
      /// <param name="max">Valor máximo.</param>
      /// <returns>Una cadena de texto que contiene el valor validado o una cadena de texto vacía en cualquier otro caso.</returns>
      public int IntegerValidator(string value, string title, bool required, int min, int max)
      {
         return IntegerValidator(value, title, required, min, max, 0);
      }

      /// <summary>
      /// Validación de cadenas de texto.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <returns>Una cadena de texto que contiene el valor validado o una cadena de texto vacía en cualquier otro caso.</returns>
      public int IntegerValidator(string value, string title, bool required)
      {
         return IntegerValidator(value, title, required, 0, 0, 0);
      }

      /// <summary>
      /// Validación de valors monetarios.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <param name="min">Valor mínimo.</param>
      /// <param name="max">Valor máximo.</param>
      /// <param name="defaultValue">Valor por defecto a devolver en caso de fallar la validación.</param>
      /// <returns>Un valor <see cref="System.Decimal"/> que contiene el valor validado o el valor por defecto en cualquier otro caso.</returns>
      public decimal CurrencyValidator(string value, string title, bool required, decimal min, decimal max, decimal defaultValue)
      {
         decimal retvalue = defaultValue;

         // Validación: la obligatoriedad
         if (required && string.IsNullOrEmpty(value))
         {
            _lastErrMsg = "El campo " + title + " es obligatorio.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return defaultValue;
         }
         else if (!required && string.IsNullOrEmpty(value.Trim()))
         {
            return defaultValue;
         }

         // Convierte el valor en formato texto a entero
         if (!decimal.TryParse(value, out retvalue)) retvalue = 0;

         // Validación: longitud mínima
         if (min > 0 && retvalue < min)
         {
            _lastErrMsg = "El campo " + title + " debe tener un valor entre " + min + " y " + max + ".";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return defaultValue;
         }

         // Validación: longitud máxima
         if (max > 0 && retvalue > max)
         {
            _lastErrMsg = "El campo " + title + " debe tener un valor entre " + min + " y " + max + ".";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return defaultValue;
         }

         return retvalue;
      }

      /// <summary>
      /// Validación de valors monetarios.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <returns>Un valor <see cref="System.Decimal"/> que contiene el valor validado o el valor por defecto en cualquier otro caso.</returns>
      public decimal CurrencyValidator(string value, string title, bool required)
      {
         return this.CurrencyValidator(value, title, required, decimal.Zero, decimal.Zero, decimal.Zero);
      }

      /// <summary>
      /// Validación de contraseñas.
      /// </summary>
      /// <param name="value">Contraseña.</param>
      /// <param name="value2">Verificación de la contraseña.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <param name="lengthMin">Longitud mínima.</param>
      /// <param name="lengthMax">Longitud máxima.</param>
      /// <returns></returns>
      public string PasswordValidator(string value, string value2, string title, bool required, int lengthMin, int lengthMax)
      {
         // Controla el campo vacío
         if (required && string.IsNullOrEmpty(value.Trim()))
         {
            _lastErrMsg = "El campo " + title + " es obligatorio.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }
         else if (!required && string.IsNullOrEmpty(value.Trim()))
         {
            return string.Empty;
         }

         // Verifica que los dos campos contengan la misma cadena
         if (!value.Equals(value2))
         {
            _lastErrMsg = "El campo " + title + " y la verificación no contienen la misma contraseña.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         // Comprueba que se trate de una cuenta de correo electrónico
         Regex reStrict = new Regex(REGEX_PASSWORD_PATTERN);
         if (!reStrict.IsMatch(value))
         {
            _lastErrMsg = "El campo " + title + " no contiene una contraseña válida.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         // Validación: longitud mínima
         if (lengthMin > 0 && value.Length < lengthMin)
         {
            _lastErrMsg = "El campo " + title + " debe tener una longitud entre " + lengthMin + " y " + lengthMax + " carácteres.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         // Validación: longitud máxima
         if (lengthMax > 0 && value.Length > lengthMax)
         {
            _lastErrMsg = "El campo " + title + " debe tener una longitud entre " + lengthMin + " y " + lengthMax + " carácteres.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         return value;
      }

      /// <summary>
      /// Validación de cuentas de correo electrónico.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <param name="lengthMin">Longitud mínima.</param>
      /// <param name="lengthMax">Longitud máxima.</param>
      /// <returns>Una cadena de texto que contiene la cuenta de correo o una cadena de texto vacía en cualquier otro caso.</returns>
      public string MailValidator(string value, string title, bool required, int lengthMin, int lengthMax)
      {
         // Formatea parámetros
         value = value.Trim();

         // Controla el campo vacío
         if (required && string.IsNullOrEmpty(value))
         {
            _lastErrMsg = "El campo " + title + " es obligatorio.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }
         else if (!required && string.IsNullOrEmpty(value.Trim()))
         {
            return string.Empty;
         }

         // Comprueba que se trate de una cuenta de correo electrónico
         Regex reStrict = new Regex(REGEX_MAIL_PATTERN);
         if (!reStrict.IsMatch(value))
         {
            _lastErrMsg = "El campo " + title + " no contiene una cuenta de email válida.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         // Validación: longitud mínima
         if (lengthMin > 0 && value.Length < lengthMin)
         {
            _lastErrMsg = "El campo " + title + " debe tener una longitud mínima de " + lengthMin + " carácteres.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         // Validación: longitud máxima
         if (lengthMax > 0 && value.Length > lengthMax)
         {
            _lastErrMsg = "El campo " + title + " debe tener una longitud máxima de " + lengthMax + " carácteres.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         return value;
      }

      /// <summary>
      /// Validación de cuentas de correo electrónico.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <returns>Una cadena de texto que contiene la cuenta de correo o una cadena de texto vacía en cualquier otro caso.</returns>
      public string MailValidator(string value, string title, bool required)
      {
         return this.MailValidator(value, title, required, 0, 0);
      }

      /// <summary>
      /// Validación de direcciones URL.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <param name="lengthMin">Longitud mínima.</param>
      /// <param name="lengthMax">Longitud máxima.</param>
      /// <returns>Una cadena de texto que contiene la URL o una cadena de texto vacía en cualquier otro caso.</returns>
      public string UrlValidator(string value, string title, bool required, int lengthMin, int lengthMax)
      {
         // Formatea parámetros
         value = value.Trim();

         // Controla el campo vacío
         if (required && string.IsNullOrEmpty(value))
         {
            _lastErrMsg = "El campo " + title + " es obligatorio.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }
         else if (!required && string.IsNullOrEmpty(value.Trim()))
         {
            return string.Empty;
         }

         // Comprueba que se trate de una URL valida
         string strRegex = "^(https?://)" + 
                           "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" + //user@ 
                           @"(([0-9]{1,3}\.){3}[0-9]{1,3}" +                           // IP- 199.194.52.184 
                           "|" +                                                       // allows either IP or domain 
                           @"([0-9a-z_!~*'()-]+\.)*" +                                 // tertiary domain(s)- www. 
                           @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." +                   // second level domain 
                           "[a-z]{2,6})" +                                             // first level domain- .com or .museum 
                           "(:[0-9]{1,4})?" +                                          // port number- :80 
                           "((/?)|" +                                                  // a slash isn't required if there is no file name 
                           "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
         Regex re = new Regex(strRegex);
         if (!re.IsMatch(value))
         {
            _lastErrMsg = "El campo " + title + " no contiene una dirección URL válida.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         // Validación: longitud mínima
         if (lengthMin > 0 && value.Length < lengthMin)
         {
            _lastErrMsg = "El campo " + title + " debe tener una longitud mínima de " + lengthMin + " carácteres.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         // Validación: longitud máxima
         if (lengthMax > 0 && value.Length > lengthMax)
         {
            _lastErrMsg = "El campo " + title + " debe tener una longitud máxima de " + lengthMax + " carácteres.";
            if (_useException) throw new CosmoValidationException(_lastErrMsg);
            _errMsgs.Add(_lastErrMsg);
            _isValid = false;
            return string.Empty;
         }

         return value;
      }

      /// <summary>
      /// Validación de direcciones URL.
      /// </summary>
      /// <param name="value">Valor a validar.</param>
      /// <param name="title">Título del campo.</param>
      /// <param name="required">Indica si es un campo requerido.</param>
      /// <returns>Una cadena de texto que contiene la URL o una cadena de texto vacía en cualquier otro caso.</returns>
      public string UrlValidator(string value, string title, bool required)
      {
         return this.UrlValidator(value, title, required, 0, 0);
      }
   }
}
