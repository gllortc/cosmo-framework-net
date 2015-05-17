using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Cosmo.Net
{

   /// <summary>
   /// Implementa una clase para la gestión de URLs.
   /// </summary>
   public class Url
   {
      const string PARAM_ENCODED_INDICATOR = "ei";
      const string PARAM_ENCODED_DATA = "ed";
      const string KEY_CHAR_SLASH = "--";

      private string _url = string.Empty;
      private string _file = string.Empty;
      private string _anchorName = string.Empty;
      private NameValueCollection _params = null;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="Url"/>.
      /// </summary>
      public Url() 
      {
         _params = new NameValueCollection();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="Url"/>.
      /// </summary>
      /// <param name="url">La dirección URL.</param>
      public Url(string url)
      {
         _url = url;
         _params = new NameValueCollection();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="Url"/>.
      /// </summary>
      /// <param name="url">La dirección URL.</param>
      /// <param name="anchorName">Nombre del enlace interno.</param>
      public Url(string url, string anchorName)
      {
         _url = url;
         _params = new NameValueCollection();
         _anchorName = anchorName;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="Url"/>.
      /// </summary>
      public Url(NameValueCollection clone) 
      {
         _params = clone;

         // Detecta si dispone de parámetros encriptados
         if (_params[PARAM_ENCODED_INDICATOR] == null || _params[PARAM_ENCODED_INDICATOR] == string.Empty) return;

         // Decodifica los parámetros
         try
         {
            // Decodifica los parámetros
            string oparams = _params[PARAM_ENCODED_DATA];
            oparams = Encryption.DeHex(oparams);

            // Carga los parámetros en el objeto actual
            Url oqs = Url.FromUrl(oparams);

            // Los añade a la colección de valores
            foreach (string key in oqs.Parameters.AllKeys)
            {
               _params.Add(key, oqs.Parameters[key].Replace(KEY_CHAR_SLASH, "\\"));
            }
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="Url"/>.
      /// </summary>
      /// <param name="request">Una instancia de <see cref="HttpRequest"/></param>
      public Url(HttpRequest request)
      {
         _file = Path.GetFileName(request.PhysicalPath);
         _params = request.Params;

         // Detecta si dispone de parámetros encriptados
         if (_params[PARAM_ENCODED_INDICATOR] == null || _params[PARAM_ENCODED_INDICATOR] == string.Empty) return;

         // Decodifica los parámetros
         try
         {
            // Decodifica los parámetros
            string oparams = _params[PARAM_ENCODED_DATA];
            oparams = Encryption.DeHex(oparams);

            // Carga los parámetros en el objeto actual
            Url oqs = Url.FromUrl(oparams);

            // Los añade a la colección de valores
            foreach (string key in oqs.Parameters.AllKeys)
            {
               _params.Add(key, oqs.Parameters[key].Replace(KEY_CHAR_SLASH, "\\"));
            }
         }
         catch
         {
            throw;
         }
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el nombre del ancla.
      /// </summary>
      public string AnchorName
      {
         get { return _anchorName; }
         set { _anchorName = value; }
      }

      /// <summary>
      /// Devuelve o establece el nombre del archivo.
      /// </summary>
      public string Filename
      {
         get { return _file; }
         set { _file = value; }
      }

      /// <summary>
      /// Contiene la lista de parámetros de la Url.
      /// </summary>
      public NameValueCollection Parameters
      {
         get { return _params; }
         set { _params = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Indica si una clave existe entre los parámetros recibidos.
      /// </summary>
      /// <param name="key">Una cadena de texto que representa la clave a comprobar.</param>
      /// <returns><c>true</c> si la colección contiene la clave o <c>false</c> en cualquier otro caso.</returns>
      public bool ContainsKey(string key)
      {
         foreach (string pkey in _params.AllKeys)
            if (pkey.Equals(key))
               return true;

         return false;
      }

      /// <summary>
      /// Obtiene el valor en formato texto de un parámetro
      /// </summary>
      /// <param name="name">Nombre del parámetro</param>
      /// <param name="defaultvalue">Valor que devolverá en caso de no existir o no podre interpretarlo</param>
      /// <returns>El contenido String del parámetro</returns>
      public string GetString(string name, string defaultvalue)
      {
         if (_params[name] == null) return defaultvalue;
         return (_params[name]).ToString().Trim();
      }

      /// <summary>
      /// Obtiene el valor en formato texto de un parámetro
      /// </summary>
      /// <param name="name">Nombre del parámetro</param>
      /// <returns>El contenido String del parámetro</returns>
      /// <remarks>En caso de no existir el parámetro, devuelve una cadena vacía</remarks>
      public string GetString(string name)
      {
         return GetString(name, string.Empty);
      }

      /// <summary>
      /// Obtiene el valor entero (int) de un parámetro
      /// </summary>
      /// <param name="name">Nombre del parámetro</param>
      /// <param name="defaultvalue">Valor que devolverá en caso de no existir o no podre interpretarlo</param>
      /// <returns>El valor numérico entero del parámetro</returns>
      public int GetInteger(string name, int defaultvalue)
      {
         if (_params[name] == null) return defaultvalue;

         int value = 0;
         if (!int.TryParse(_params[name], out value)) value = 0;

         return value;
      }

      /// <summary>
      /// Obtiene el valor entero (int) de un parámetro
      /// </summary>
      /// <param name="name">Nombre del parámetro</param>
      /// <returns>El valor numérico entero del parámetro</returns>
      /// <remarks>En caso de no existir el parámetro, devuelve una valor 0</remarks>
      public int GetInteger(string name)
      {
         return GetInteger(name, 0);
      }

      /// <summary>
      /// Obtiene el valor booleano de un parámetro
      /// </summary>
      /// <param name="name">Nombre del parámetro</param>
      /// <param name="defaultvalue">Valor que devolverá en caso de no existir o no podre interpretarlo</param>
      /// <returns>El valor booleano del parámetro</returns>
      public bool GetBoolean(string name, bool defaultvalue)
      {
         if (_params[name] == null) return defaultvalue;

         if (_params[name].Trim().ToLower().Equals("false") || _params[name].Trim().Equals("0")) return false;
         if (_params[name].Trim().ToLower().Equals("true") || _params[name].Trim().Equals("1")) return true;

         return defaultvalue;
      }

      /// <summary>
      /// Obtiene el valor booleano de un parámetro
      /// </summary>
      /// <param name="name">Nombre del parámetro</param>
      /// <returns>El valor booleano del parámetro</returns>
      /// <remarks>En caso de no existir el parámetro, devuelve una valor false</remarks>
      public bool GetBoolean(string name)
      {
         return GetBoolean(name, false);
      }

      /// <summary>
      /// Elimina los parámetros de la Url.
      /// </summary>
      public void ClearParameters()
      {
         _params.Clear();
      }

      /// <summary>
      /// Elimina los parámetros de la Url.
      /// </summary>
      /// <param name="except">Nombre del parámetro a mantener.</param>
      public void ClearParameters(string except)
      {
         ClearParameters(new string[] { except });
      }

      /// <summary>
      /// Elimina los parámetros de la Url.
      /// </summary>
      /// <param name="except">Un array con los nombres de los parámetros a mantener.</param>
      public void ClearParameters(string[] except)
      {
         ArrayList toRemove = new ArrayList();
         foreach (string s in _params.AllKeys)
         {
            foreach (string e in except)
            {
               if (s.ToLower() == e.ToLower())
                  if (!toRemove.Contains(s))
                     toRemove.Add(s);
            }
         }

         foreach (string s in toRemove)
         {
            _params.Remove(s);
         }
      }

      /// <summary>
      /// Agrega un nuevo parámetro a la Url.
      /// </summary>
      /// <param name="name">Nombre del parámetro.</param>
      /// <param name="value">Valor del parámetro.</param>
      public Url AddParameter(string name, string value)
      {
         if (_params[name] != null)
         {
            _params[name] = value;
         }
         else
         {   
            _params.Add(name, value.Replace("\\", KEY_CHAR_SLASH));
         }

         return this;
      }

      /// <summary>
      /// Agrega un nuevo parámetro numérico (entero) a la Url.
      /// </summary>
      /// <param name="name">Nombre del parámetro.</param>
      /// <param name="value">Valor del parámetro.</param>
      public Url AddParameter(string name, int value)
      {
         if (_params[name] != null)
         {
            _params[name] = value.ToString();
         }
         else
         {
            _params.Add(name, value.ToString());
         }

         return this;
      }

      /// <summary>
      /// Agrega un nuevo parámetro booleano a la Url.
      /// </summary>
      /// <param name="name">Nombre del parámetro.</param>
      /// <param name="value">Valor del parámetro.</param>
      public Url AddParameter(string name, bool value)
      {
         if (_params[name] != null)
         {
            _params[name] = (value ? "1" : "0");
         }
         else
         {
            _params.Add(name, (value ? "1" : "0"));
         }

         return this;
      }

      /// <summary>
      /// Convierte una URL o un eMail en un enlace Html.
      /// </summary>
      /// <param name="text">Texto que aparecerá en el enlace.</param>
      /// <returns>Un enlace XHTML.</returns>
      public string ToLink(string text)
      {
         string address = _url.Trim().ToLower();

         if (address.Equals(string.Empty)) return string.Empty;

         if (address.Contains("@") && !address.StartsWith("mailto:"))
            // Dirección de correo electrónico
            address = "mailto:" + address;
         else if (!address.StartsWith("http://") && !address.StartsWith("https://"))
            // Una dirección web
            address = "http://" + address;

         address = Cosmo.Net.Mail.Obfuscate(address);

         return "<a href=\"" + address + "\" target=\"_blank\">" + text + "</a>";
      }

      /// <summary>
      /// Convierte una URL o un eMail en un enlace Html.
      /// </summary>
      /// <returns>Un enlace XHTML.</returns>
      public string ToLink()
      {
         return ToLink(_url);
      }

      /// <summary>
      /// Indica si la Url es válida.
      /// </summary>
      /// <returns><c>true</c> si la Url es válida o <c>false</c> en cualquier otro caso.</returns>
      public bool IsValid()
      {
         return Url.IsValid(_url);
      }

      /// <summary>
      /// Obtiene la URL resultante con los parámetros codificados.
      /// </summary>
      /// <param name="script">Nombre del script.</param>
      /// <returns>Una cadena de texto que representa la URL.</returns>
      public string GetEncodedURL(string script)
      {
         return this.GetEncodedURL(script, false);
      }

      /// <summary>
      /// Obtiene la URL resultante con los parámetros codificados.
      /// </summary>
      /// <param name="script">Nombre del script.</param>
      /// <param name="clearAfetr">Indica si debe eliminar los parámetros después de devolver la URL.</param>
      /// <returns>Una cadena de texto que representa la URL.</returns>
      public string GetEncodedURL(string script, bool clearAfetr)
      {
         // Devuelve la URL codificada
         string href = Encryption.Hex(this.ToString());

         // Limpia el objeto si es necesario
         if (clearAfetr) this.ClearParameters();

         return script + "?" + PARAM_ENCODED_INDICATOR + "=1&" + PARAM_ENCODED_DATA + "=" + href;
      }

      /// <summary>
      /// Transforma la Url a una cadena de texto.
      /// </summary>
      /// <returns>La cadena de texto.</returns>
      public override string ToString()
      {
         return ToString(true, false);
      }

      /// <summary>
      /// Transforma la Url a una cadena de texto.
      /// </summary>
      /// <param name="includeUrl">Indica si debe incluir el nombre del archivo o sólo devolver los parámetros.</param>
      /// <returns>La cadena de texto.</returns>
      public string ToString(bool includeUrl)
      {
         return ToString(includeUrl, false);
      }

      /// <summary>
      /// Transforma la Url a una cadena de texto.
      /// </summary>
      /// <param name="includeUrl">Indica si debe incluir el nombre del archivo o sólo devolver los parámetros.</param>
      /// <param name="htmlEncoded">Indica si debe codificar la URL para su uso en XHTML.</param>
      /// <returns>La cadena de texto.</returns>
      public string ToString(bool includeUrl, bool htmlEncoded)
      {
         string[] parts = new string[_params.Count];
         string[] keys = _params.AllKeys;

         for (int i = 0; i < keys.Length; i++)
            parts[i] = keys[i] + "=" + HttpContext.Current.Server.UrlEncode(_params[keys[i]]);

         string url = string.Join("&", parts);

         // Agrega el script si se ha especificado
         if (includeUrl)
            url = _url + "?" + (url != null ? url : string.Empty);

         if (!_anchorName.Equals(""))
            url += "#" + _anchorName.Trim();

         if (htmlEncoded)
            url = url.Replace("&", "&amp;");

         return url;
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Permite recuperar un parámetro String del QueryString
      /// </summary>
      /// <param name="parameters">Parámetros (Request.Params).</param>
      /// <param name="name">Nombre del parámetro a recuperar.</param>
      /// <param name="defaultvalue">Valor por defecto en caso de no poder obtener dicho valor.</param>
      /// <returns>El valor resultante.</returns>
      public static string GetString(NameValueCollection parameters, string name, string defaultvalue)
      {
         // Verifica el caso que no haya parámetros
         if (parameters == null) return defaultvalue;
         if (parameters.Count <= 0) return defaultvalue;

         // Verifica que exista el parámetro solicitado
         if (parameters[name] == null) return defaultvalue;

         return HttpUtility.UrlDecode(parameters[name].ToString());
      }

      /// <summary>
      /// Permite recuperar un parámetro String del QueryString
      /// </summary>
      /// <param name="parameters">Parámetros (Request.Params).</param>
      /// <param name="name">Nombre del parámetro a recuperar.</param>
      /// <returns>El valor resultante.</returns>
      public static string GetString(NameValueCollection parameters, string name)
      {
         return GetString(parameters, name, string.Empty);
      }

      /// <summary>
      /// Permite recuperar un parámetro Integer del QueryString
      /// </summary>
      /// <param name="parameters">Parámetros (Request.Params).</param>
      /// <param name="name">Nombre del parámetro a recuperar.</param>
      /// <param name="defaultvalue">Valor por defecto en caso de no poder obtener dicho valor.</param>
      /// <returns>El valor resultante.</returns>
      public static int GetInteger(NameValueCollection parameters, string name, int defaultvalue)
      {
         int value = 0;

         // Verifica el caso que no haya parámetros
         if (parameters == null) return defaultvalue;
         if (parameters.Count <= 0) return defaultvalue;

         // Verifica que exista el parámetro solicitado
         if (parameters[name] == null) return defaultvalue;

         if (!int.TryParse(parameters[name].ToString(), out value)) value = (int)defaultvalue;

         return value;
      }

      /// <summary>
      /// Permite recuperar un parámetro Integer del QueryString
      /// </summary>
      /// <param name="parameters">Parámetros (Request.Params).</param>
      /// <param name="name">Nombre del parámetro a recuperar.</param>
      /// <returns>El valor resultante.</returns>
      public static int GetInteger(NameValueCollection parameters, string name)
      {
         return GetInteger(parameters, name, 0);
      }

      /// <summary>
      /// Obtiene el valor booleano de un parámetro
      /// </summary>
      /// <param name="parameters">La colección de parámetros recibidos.</param>
      /// <param name="name">Nombre del parámetro</param>
      /// <param name="defaultvalue">Valor que devolverá en caso de no existir o no podre interpretarlo</param>
      /// <returns>El valor booleano del parámetro</returns>
      public static bool GetBoolean(NameValueCollection parameters, string name, bool defaultvalue)
      {
         if (parameters == null) return defaultvalue;
         if (parameters[name] == null) return defaultvalue;

         if (parameters[name].Trim().ToLower().Equals("false") || parameters[name].Trim().Equals("0")) return false;
         if (parameters[name].Trim().ToLower().Equals("true") || parameters[name].Trim().Equals("1")) return true;

         return defaultvalue;
      }

      /// <summary>
      /// Obtiene el valor booleano de un parámetro.
      /// </summary>
      /// <param name="parameters">La colección de parámetros recibidos.</param>
      /// <param name="name">Nombre del parámetro.</param>
      /// <returns>El valor booleano del parámetro.</returns>
      public static bool GetBoolean(NameValueCollection parameters, string name)
      {
         return GetBoolean(parameters, name, false);
      }

      /// <summary>
      /// Obtiene la fecha/hora de un parámetro.
      /// </summary>
      /// <param name="parameters">La colección de parámetros recibidos.</param>
      /// <param name="name">Nombre del parámetro</param>
      /// <param name="defaultvalue">Valor que devolverá en caso de no existir o no podre interpretarlo</param>
      /// <returns>El valor <see cref="DateTime"/> del parámetro.</returns>
      public static DateTime GetDateTime(NameValueCollection parameters, string name, DateTime defaultvalue)
      {
         if (parameters == null) return defaultvalue;
         if (parameters[name] == null) return defaultvalue;

         DateTime date = defaultvalue;
         DateTime.TryParse(parameters[name].Trim(), out date);

         return date;
      }

      /// <summary>
      /// Obtiene la fecha/hora de un parámetro.
      /// </summary>
      /// <param name="parameters">La colección de parámetros recibidos.</param>
      /// <param name="name">Nombre del parámetro.</param>
      /// <returns>El valor <see cref="DateTime"/> del parámetro.</returns>
      public static DateTime GetDateTime(NameValueCollection parameters, string name)
      {
         return GetDateTime(parameters, name, DateTime.MinValue);
      }

      /// <summary>
      /// Obtiene los parámetros desde una URL (con o sin el nombre de archivo).
      /// </summary>
      /// <param name="url">URL a tratar.</param>
      /// <returns>Una instancia de <see cref="Url"/>.</returns>
      /// <remarks>Para recojer la URL actual: Request.Url.AbsoluteUri</remarks>
      public static Url FromUrl(string url)
      {
         // Contempla la posibilidad de pasar únicamente el QueryString sin la Uri
         if (!url.Contains("?"))
            url = "data@data.com?" + url;

         string[] parts = url.Split("?".ToCharArray());

         Url qs = new Url();
         qs.Filename = parts[0];

         if (parts.Length == 1)
            return qs;

         string[] keys = parts[1].Split("&".ToCharArray());
         foreach (string key in keys)
         {
            string[] part = key.Split("=".ToCharArray());
            if (part.Length == 1) qs.AddParameter(part[0], string.Empty);
            qs.AddParameter(part[0], part[1]);
         }

         // Decodifica los parámetros
         try
         {
            // Detecta si dispone de parámetros encriptados
            if (qs.Parameters[PARAM_ENCODED_INDICATOR] == null || qs.Parameters[PARAM_ENCODED_INDICATOR] == string.Empty) return qs;

            // Decodifica los parámetros
            string oparams = qs.Parameters[PARAM_ENCODED_DATA];
            oparams = Encryption.DeHex(oparams);

            // Carga los parámetros en el objeto actual
            Url oqs = Url.FromUrl(oparams);

            // Los añade a la colección de valores
            foreach (string key in oqs.Parameters.AllKeys)
               qs.AddParameter(key, oqs.Parameters[key].Replace(KEY_CHAR_SLASH, "\\"));

            return qs;
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Compina dos fragmentos de URL
      /// </summary>
      /// <returns>Los fragmentos combinados.</returns>
      public static string Combine(string Url1, string Url2)
      {
         Url1 = Url1.Trim();
         Url2 = Url2.Trim();

         if (Url1.EndsWith("/"))
            return Url1 + Url2;

         return Url1 + "/" + Url2;
      }

      /// <summary>
      /// Compina dos o más fragmentos de URL
      /// </summary>
      /// <returns>Los fragmentos combinados.</returns>
      public static string Combine(params string[] Urls)
      {
         if (Urls.Length == 0) return string.Empty;
         if (Urls.Length == 1) return Urls[0].Trim().Replace("\r\n", string.Empty);

         string url = Urls[0].Trim().Replace("\r\n", string.Empty);

         for (int i = 1; i < Urls.Length; i++)
         {
            url = Url.Combine(url, Urls[i].Trim().Replace("\r\n", string.Empty));
         }

         return url;
      }

      /// <summary>
      /// Permite verificar si una URL es válida.
      /// </summary>
      /// <param name="url">URL a verificar.</param>
      /// <returns>Un valor booleano indicando la validez.</returns>
      public static bool IsValid(string url)
      {
         //const string REGEX_VERIFY_URL = @"(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&%_\./-~-]*)?";
         const string REGEX_VERIFY_URL = @"(([\w]+:)?//)?(([\d\w]|%[a-fA-formatter\d]{2,2})+(:([\d\w]|%[a-fA-formatter\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(/([-+_~.\d\w]|%[a-fA-formatter\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-formatter\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-formatter\d]{2,2})*)?";

         Regex regex = new Regex(REGEX_VERIFY_URL, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
         return regex.IsMatch(url);
      }

      /// <summary>
      /// Convierte una URL o un ex-Mail en un enlace Html
      /// </summary>
      /// <param name="address">Dirección de mail o URL</param>
      /// <param name="text">Texto que se mostrará en el enlace.</param>
      /// <returns>Un enlace XHTML</returns>
      public static string ConvertToLink(string address, string text)
      {
         address = address.Trim().ToLower();

         if (address.Equals(string.Empty)) return string.Empty;

         if (address.Contains("@") && !address.StartsWith("mailto:"))
            // Dirección de correo electrónico
            address = "mailto:" + address;
         else if (!address.StartsWith("http://") && !address.StartsWith("https://"))
            // Una dirección web
            address = "http://" + address;

         address = Cosmo.Net.Mail.Obfuscate(address);

         return "<a href=\"" + address + "\" target=\"_blank\">" + text + "</a>";
      }

      /// <summary>
      /// Convierte una URL o un ex-Mail en un enlace Html
      /// </summary>
      /// <param name="address">Dirección de mail o URL</param>
      /// <returns>Un enlace XHTML</returns>
      public static string ConvertToLink(string address)
      {
         return ConvertToLink(address, Cosmo.Net.Mail.Obfuscate(address));
      }

      /// <summary>
      /// Navega hacia una determinada URL usando el navegador por defecto.
      /// </summary>
      /// <param name="url">URL de destino.</param>
      public static void Browse(string url)
      {
         System.Diagnostics.Process.Start(url);
      }

      #endregion

      #region class Encryption

      /// <summary>
      /// Clase privada para encriptación/desencriptación del QueryString
      /// </summary>
      class Encryption
      {
         public static string DeHex(string hexstring)
         {
            string ret = string.Empty;
            StringBuilder sb = new StringBuilder(hexstring.Length / 2);

            for (int i = 0; i <= hexstring.Length - 1; i = i + 2)
            {
               sb.Append((char)int.Parse(hexstring.Substring(i, 2), NumberStyles.HexNumber));
            }
            return sb.ToString();
         }

         public static string Hex(string sData)
         {
            string temp = string.Empty; ;
            string newdata = string.Empty;
            StringBuilder sb = new StringBuilder(sData.Length * 2);

            for (int i = 0; i < sData.Length; i++)
            {
               if ((sData.Length - (i + 1)) > 0)
               {
                  temp = sData.Substring(i, 2);
                  if (temp == @"\n") newdata += "0A";
                  else if (temp == @"\b") newdata += "20";
                  else if (temp == @"\reader") newdata += "0D";
                  else if (temp == @"\c") newdata += "2C";
                  else if (temp == @"\\") newdata += "5C";
                  else if (temp == @"\0") newdata += "00";
                  else if (temp == @"\t") newdata += "07";
                  else
                  {
                     sb.Append(string.Format("{0:X2}", (int)(sData.ToCharArray())[i]));
                     i--;
                  }
               }
               else
               {
                  sb.Append(string.Format("{0:X2}", (int)(sData.ToCharArray())[i]));
               }
               i++;
            }
            return sb.ToString();
         }
      }

      #endregion

   }
}
