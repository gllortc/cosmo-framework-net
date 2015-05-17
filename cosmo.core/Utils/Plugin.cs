using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;

namespace Cosmo.Utils
{
   /// <summary>
   /// Implementa un plugin para extender distintas funcionalidades de Cosmo.
   /// </summary>
   public class Plugin
   {
      private bool _default;
      private string _id;
      private string _class;
      private NameValueCollection _settings;
      private Object _instance;
      private XmlNode _configXml;

      /// <summary>
      /// Devuelve una instancia de <see cref="Plugin"/>.
      /// </summary>
      public Plugin()
      {
         Initialize();
      }

      /// <summary>
      /// Permite almacenar la instancia del plugin.
      /// </summary>
      public Object Instance
      {
         get { return _instance; }
         set { _instance = value; }
      }

      /// <summary>
      /// Devuelve o establece el identificador del plugin.
      /// </summary>
      public string ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Devuelve o establece el identificador del plugin.
      /// </summary>
      public string Class
      {
         get { return _class; }
         set { _class = value; }
      }

      /// <summary>
      /// Devuelve o establece el identificador del plugin.
      /// </summary>
      public bool Default
      {
         get { return _default; }
         set { _default = value; }
      }

      /// <summary>
      /// Devuelve o establece el nodo XML que contiene la definición del plugin.
      /// </summary>
      public XmlNode XmlPluginNode
      {
         get { return _configXml; }
         set { _configXml = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de parámetros de configuración del plugin.
      /// </summary>
      public NameValueCollection Settings
      {
         get { return _settings; }
         set { _settings = value; }
      }

      /// <summary>
      /// Obtiene el valor en formato texto de un parámetro
      /// </summary>
      /// <param name="name">Nombre del parámetro</param>
      /// <param name="defaultvalue">Valor que devolverá en caso de no existir o no podre interpretarlo</param>
      /// <returns>El contenido String del parámetro</returns>
      public string GetString(string name, string defaultvalue)
      {
         if (_settings[name] == null) return defaultvalue;
         return _settings[name];
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
         if (_settings[name] == null) return defaultvalue;

         int value = 0;
         if (!int.TryParse(_settings[name], out value)) value = 0;

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
         if (_settings[name] == null) return defaultvalue;

         if (_settings[name].Trim().ToLower().Equals("false") || _settings[name].Trim().Equals("0")) return false;
         if (_settings[name].Trim().ToLower().Equals("true") || _settings[name].Trim().Equals("1")) return true;

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
      /// Comprueba si una lista de <em>plugins</em> contiene un determinado <em>plugin</em>.
      /// </summary>
      /// <param name="id">Identificador del <em>plugin</em> a buscar.</param>
      /// <param name="pluginList">Lista de <em>plugins</em> disponibles.</param>
      /// <returns><c>true</c> si la lista contiene el ID buscado o <c>false</c> en cualquier otro caso.</returns>
      public static bool ContainsPlugin(string id, List<Plugin> pluginList)
      {
         foreach (Plugin plugin in pluginList)
         {
            if (plugin.ID.Equals(id))
            {
               return true;
            }
         }
         return false;
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _default = false;
         _id = string.Empty;
         _class = string.Empty;
         _settings = new NameValueCollection();
         _instance = null;
         _configXml = null;
      }
   }
}
