using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Cosmo.Utils.Web
{

   /// <summary>
   /// Implementa una clase para la gestión de los parámetros pasados por Query String
   /// </summary>
   [Obsolete]
   public class PageParameters : NameValueCollection
   {
      private string _document;
      private string _anchorName;

      const string PARAM_ENCODED_INDICATOR = "ei";
      const string PARAM_ENCODED_DATA = "ed";
      const string KEY_CHAR_SLASH = "--";

      /// <summary>
      /// Devuelve una instancia de <see cref="PageParameters"/>.
      /// </summary>
      public PageParameters() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="PageParameters"/>.
      /// </summary>
      public PageParameters(NameValueCollection clone) : base(clone) 
      {
         // Detecta si dispone de parámetros encriptados
         if (this[PARAM_ENCODED_INDICATOR] == null || this[PARAM_ENCODED_INDICATOR] == "") return;

         // Decodifica los parámetros
         try
         {
            // Decodifica los parámetros
            string oparams = this[PARAM_ENCODED_DATA];
            oparams = Encryption.DeHex(oparams);

            // Carga los parámetros en el objeto actual
            PageParameters oqs = PageParameters.FromUrl(oparams);

            // Los añade a la colección de valores
            foreach (string key in oqs.AllKeys)
               this.Add(key, oqs[key].Replace(KEY_CHAR_SLASH, "\\"));
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="PageParameters"/>.
      /// </summary>
      /// <param name="document">Nombre del archivo (URL).</param>
      public PageParameters(string document)
      {
         _document = document;
         _anchorName = string.Empty;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="PageParameters"/>.
      /// </summary>
      /// <param name="document">Nombre del archivo (URL).</param>
      /// <param name="anchorName">Nombre del ancla.</param>
      public PageParameters(string document, string anchorName)
      {
         _document = document;
         _anchorName = anchorName;
      }

      #region Properties

      /// <summary>
      /// Nombre del documento para el que se evaluan los parámetros.
      /// </summary>
      public string Document
      {
         get { return _document; }
         set { _document = value; }
      }

      /// <summary>
      /// Devuelve o establece el nombre del ancla.
      /// </summary>
      public string AnchorName
      {
         get { return _anchorName; }
         set { _anchorName = value; }
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
         foreach (string pkey in this.AllKeys)
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
         if (this[name] == null) return defaultvalue;
         return this[name];
      }

      /// <summary>
      /// Obtiene el valor en formato texto de un parámetro
      /// </summary>
      /// <param name="name">Nombre del parámetro</param>
      /// <returns>El contenido String del parámetro</returns>
      /// <remarks>En caso de no existir el parámetro, devuelve una cadena vacía</remarks>
      public string GetString(string name)
      {
         return GetString(name, "");
      }

      /// <summary>
      /// Obtiene el valor entero (int) de un parámetro
      /// </summary>
      /// <param name="name">Nombre del parámetro</param>
      /// <param name="defaultvalue">Valor que devolverá en caso de no existir o no podre interpretarlo</param>
      /// <returns>El valor numérico entero del parámetro</returns>
      public int GetInteger(string name, int defaultvalue)
      {
         if (this[name] == null) return defaultvalue;

         int value = 0;
         if (!int.TryParse(this[name], out value)) value = 0;

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
         if (this[name] == null) return defaultvalue;

         if (this[name].Trim().ToLower().Equals("false") || this[name].Trim().Equals("0")) return false;
         if (this[name].Trim().ToLower().Equals("true") || this[name].Trim().Equals("1")) return true;

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
      /// Elimina todos los parámetros excepto los indicados como parámetro.
      /// </summary>
      /// <param name="except">Un array con los nombres de los parámetros a mantener.</param>
      public void ClearAllExcept(string[] except)
      {
         ArrayList toRemove = new ArrayList();
         foreach (string s in this.AllKeys)
         {
            foreach (string e in except)
            {
               if (s.ToLower() == e.ToLower())
                  if (!toRemove.Contains(s)) 
                     toRemove.Add(s);
            }
         }

         foreach (string s in toRemove)
            this.Remove(s);
      }

      /// <summary>
      /// Elimina todos los parámetros excepto el indicado como parámetro.
      /// </summary>
      /// <param name="except">Nombre del parámetro a mantener.</param>
      public void ClearAllExcept(string except)
      {
         ClearAllExcept(new string[] { except });
      }

      /// <summary>
      /// Agrega un nuevo parámetro a la cadena QueryString.
      /// </summary>
      /// <param name="name">Nombre del parámetro.</param>
      /// <param name="value">Valor del parámetro.</param>
      public override void Add(string name, string value)
      {
         if (this[name] != null)
            this[name] = value;
         else
            base.Add(name, value.Replace("\\", KEY_CHAR_SLASH));
      }

      /// <summary>
      /// Agrega un nuevo parámetro numérico (entero) a la cadena QueryString.
      /// </summary>
      /// <param name="name">Nombre del parámetro.</param>
      /// <param name="value">Valor del parámetro.</param>
      public void Add(string name, int value)
      {
         if (this[name] != null)
            this[name] = value.ToString();
         else
            base.Add(name, value.ToString());
      }

      /// <summary>
      /// Agrega un nuevo parámetro booleano a la cadena QueryString.
      /// </summary>
      /// <param name="name">Nombre del parámetro.</param>
      /// <param name="value">Valor del parámetro.</param>
      public void Add(string name, bool value)
      {
         if (this[name] != null)
            this[name] = (value ? "1" : "0");
         else
            base.Add(name, (value ? "1" : "0"));
      }

      /// <summary>
      /// Convierte los parámetros en una cadena QueryString usable en una Url.
      /// </summary>
      /// <returns>La cadena de texto.</returns>
      public string GetUrl()
      {
         return GetUrl(false, false);
      }

      /// <summary>
      /// Convierte los parámetros en una cadena QueryString usable en una Url.
      /// </summary>
      /// <param name="includeUrl">Indica si debe incluir el nombre del archivo o sólo devolver los parámetros.</param>
      /// <returns>La cadena de texto.</returns>
      public string GetUrl(bool includeUrl)
      {
         return GetUrl(includeUrl, false);
      }

      /// <summary>
      /// Convierte los parámetros en una cadena QueryString usable en una Url añadiendo la Url de dominio al inicio.
      /// </summary>
      /// <param name="includeUrl">Indica si debe incluir el nombre del archivo o sólo devolver los parámetros.</param>
      /// <param name="htmlEncoded">Indica si debe codificar la URL para su uso en XHTML.</param>
      /// <returns>La cadena de texto.</returns>
      public string GetUrl(bool includeUrl, bool htmlEncoded)
      {
         string[] parts = new string[this.Count];
         string[] keys = this.AllKeys;

         for (int i = 0; i < keys.Length; i++)
            parts[i] = keys[i] + "=" + HttpContext.Current.Server.UrlEncode(this[keys[i]]);

         string url = String.Join("&", parts);

         // Agrega el script si se ha especificado
         if (includeUrl)
            url = _document + "?" + (url != null ? url : string.Empty);

         if (!_anchorName.Equals(""))
            url += "#" + _anchorName.Trim();

         if (htmlEncoded)
            url = url.Replace("&", "&amp;");

         return url;
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
         string href = Encryption.Hex(this.GetUrl());

         // Limpia el objeto si es necesario
         if (clearAfetr) this.Clear();

         return script + "?" + PARAM_ENCODED_INDICATOR + "=1&" + PARAM_ENCODED_DATA + "=" + href;
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
      /// <returns>Una instancia de <see cref="PageParameters"/>.</returns>
      /// <remarks>Para recojer la URL actual: Request.Url.AbsoluteUri</remarks>
      public static PageParameters FromUrl(string url)
      {
         // Contempla la posibilidad de pasar únicamente el QueryString sin la Uri
         if (!url.Contains("?"))
            url = "data@data.com?" + url;

         string[] parts = url.Split("?".ToCharArray());

         PageParameters qs = new PageParameters();
         qs._document = parts[0];

         if (parts.Length == 1)
            return qs;

         string[] keys = parts[1].Split("&".ToCharArray());
         foreach (string key in keys)
         {
            string[] part = key.Split("=".ToCharArray());
            if (part.Length == 1) qs.Add(part[0], string.Empty);
            qs.Add(part[0], part[1]);
         }

         // Decodifica los parámetros
         try
         {
            // Detecta si dispone de parámetros encriptados
            if (qs[PARAM_ENCODED_INDICATOR] == null || qs[PARAM_ENCODED_INDICATOR] == string.Empty) return qs;

            // Decodifica los parámetros
            string oparams = qs[PARAM_ENCODED_DATA];
            oparams = Encryption.DeHex(oparams);

            // Carga los parámetros en el objeto actual
            PageParameters oqs = PageParameters.FromUrl(oparams);

            // Los añade a la colección de valores
            foreach (string key in oqs.AllKeys)
               qs.Add(key, oqs[key].Replace(KEY_CHAR_SLASH, "\\"));

            return qs;
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// POST data and Redirect to the specified url using the specified page.
      /// </summary>
      /// <param name="page">Una referencia a la página desde dónde se desea mandar el formulario.</param>
      /// <param name="destinationUrl">La URL de destino dónde se mandarán los datos del formulario.</param>
      /// <param name="data">Los datos asociados al formulario que serán enviados.</param>
      public static void RedirectAndPost(Page page, string destinationUrl, NameValueCollection data)
      {
         // Set a name for the form
         string formID = "frmPostRedir";

         // Build the form using the specified data to be posted.
         StringBuilder strForm = new StringBuilder();
         strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + destinationUrl + "\" method=\"POST\">\n");
         foreach (string key in data)
         {
            strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">\n");
         }
         strForm.Append("</form>\n");

         // Build the JavaScript which will do the Posting operation.
         StringBuilder strScript = new StringBuilder();
         strScript.Append("<script type=\"text/javascript\">\n");
         strScript.Append("var v" + formID + " = document." + formID + ";\n");
         strScript.Append("v" + formID + ".submit();\n");
         strScript.Append("</script>\n");

         // Return the form and the script concatenated. (The order is important, Form then JavaScript)
         string html = strForm.ToString() + strScript.ToString();

         // Add a literal control the specified page holding the Post Form, this is to submit the Posting form with the request.
         page.Controls.Add(new LiteralControl(html));
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
            string ret = String.Empty;
            StringBuilder sb = new StringBuilder(hexstring.Length / 2);

            for (int i = 0; i <= hexstring.Length - 1; i = i + 2)
            {
               sb.Append((char)int.Parse(hexstring.Substring(i, 2), NumberStyles.HexNumber));
            }
            return sb.ToString();
         }

         public static string Hex(string sData)
         {
            string temp = String.Empty; ;
            string newdata = String.Empty;
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
                     sb.Append(String.Format("{0:X2}", (int)(sData.ToCharArray())[i]));
                     i--;
                  }
               }
               else
               {
                  sb.Append(String.Format("{0:X2}", (int)(sData.ToCharArray())[i]));
               }
               i++;
            }
            return sb.ToString();
         }
      }

      #endregion

   }
}
