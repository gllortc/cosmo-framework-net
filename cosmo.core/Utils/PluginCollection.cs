using System;
using System.Collections.Generic;
using System.Xml;

namespace Cosmo.Utils
{
   /// <summary>
   /// Implementa una colección de plugins.
   /// </summary>
   public class PluginCollection
   {
      // Declaración del espacio de nombres XML
      private const string XML_NODE_PARAMETER = "param";
      private const string XML_ATTR_PLUGIN_ID = "id";
      private const string XML_ATTR_PLUGIN_DRIVER = "driver";
      private const string XML_ATTR_PLUGIN_DEFAULT = "default";
      private const string XML_ATTR_PARAM_KEY = "key";
      private const string XML_ATTR_PARAM_VALUE = "value";

      private const string TAG_MACHINE_NAME = "[%MACHINENAME%]";

      // Internal data declaration
      private Dictionary<String, Plugin> _plugins;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PluginCollection"/>.
      /// </summary>
      public PluginCollection()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del plugin seleccionado por defecto.
      /// </summary>
      public string DefaultPluginId { get; private set; }

      /// <summary>
      /// Gets the number of plugin modules in the collection.
      /// </summary>
      public int Count
      {
         get { return _plugins.Count; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Gets the list of all modules.
      /// </summary>
      /// <returns></returns>
      public Dictionary<String, Plugin>.ValueCollection GetList()
      {
         return this._plugins.Values;
      }

      /// <summary>
      /// Check if a plugin contains certain module.
      /// </summary>
      /// <param name="pluginId">Module unique identifier.</param>
      /// <returns><c>true</c> if the module exists or <c>false</c> in all other cases.</returns>
      public bool ContainsPlugin(string pluginId)
      {
         return _plugins.ContainsKey(pluginId);
      }

      /// <summary>
      /// Agrega un nuevo plugin de forma segura a la colección evitando duplicados en los identificadores.
      /// </summary>
      /// <param name="plugin">La instancia de <see cref="Plugin"/> que se desea agregar.</param>
      public void AddPlugin(Plugin plugin)
      {
         if (_plugins.ContainsKey(plugin.ID))
         {
            _plugins[plugin.ID] = plugin;
         }
         else
         {
            _plugins.Add(plugin.ID, plugin);
         }
      }

      /// <summary>
      /// Devuelve el plugin seleccionado por defecto.
      /// </summary>
      /// <returns>Una instancia de <see cref="Plugin"/> seleccionado por defecto o <c>null</c> si no se encuentra o no está definido.</returns>
      public Plugin GetDefaultPlugin()
      {
         if (string.IsNullOrWhiteSpace(this.DefaultPluginId))
         {
            return null;
         }
         else
         {
            return GetPlugin(this.DefaultPluginId);
         }
      }

      /// <summary>
      /// Obtiene un plugin a partir de su identificador único en la colección.
      /// </summary>
      /// <param name="pluginId">Identificador único del plugin.</param>
      /// <returns>La instancia de <see cref="Plugin"/> solicitada o <c>null</c> si no existe el identificador en la colección.</returns>
      public Plugin GetPlugin(string pluginId)
      {
         if (_plugins.ContainsKey(pluginId))
         {
            return _plugins[pluginId];
         }

         return null;
      }

      /// <summary>
      /// Devuelve una lista de <em>plugins</em> del mismo tipo.
      /// </summary>
      /// <param name="xmlDoc">Documento XML que contiene los TAGs XML que definen a un tipo de <em>plugins</em>.</param>
      /// <param name="pluginTagName">Nombre del TAG que contiene el <em>plugin</em>.</param>
      /// <returns></returns>
      public void LoadPluginCollection(XmlDocument xmlDoc, string pluginTagName)
      {
         LoadPluginCollection(xmlDoc, string.Empty, pluginTagName);
      }

      /// <summary>
      /// Devuelve una lista de <em>plugins</em> del mismo tipo.
      /// </summary>
      /// <param name="xmlDoc">Documento XML que contiene los TAGs XML que definen a un tipo de <em>plugins</em>.</param>
      /// <param name="defaultAttributeName"></param>
      /// <param name="pluginTagName">Nombre del TAG que contiene el <em>plugin</em>.</param>
      /// <returns></returns>
      public void LoadPluginCollection(XmlDocument xmlDoc, string defaultAttributeName, string pluginTagName)
      {
         Plugin plugin = null;
         XmlNodeList nodeList = null;

         Initialize();

         nodeList = xmlDoc.GetElementsByTagName(pluginTagName);
         foreach (XmlNode node in nodeList)
         {
            plugin = ReadPlugin(node);

            if (plugin != null)
            {
               this.AddPlugin(plugin);
            }
         }

         if (!string.IsNullOrWhiteSpace(defaultAttributeName))
         {
            this.DefaultPluginId = ReadTagParameter(xmlDoc, defaultAttributeName, string.Empty);

            // Replace ID tags
            this.DefaultPluginId = ReplaceTags(this.DefaultPluginId);
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.DefaultPluginId = string.Empty;
         _plugins = new Dictionary<String, Plugin>();
      }

      /// <summary>
      /// Lee las propiedades de un <em>plugin</em>.
      /// </summary>
      /// <param name="xmlNode">Nodo que contiene la definición del <em>plugin</em>.</param>
      private Plugin ReadPlugin(XmlNode xmlNode)
      {
         Plugin plugin = null;
         XmlNodeList nodeList = null;

         plugin = new Plugin();
         plugin.ID = xmlNode.Attributes[XML_ATTR_PLUGIN_ID].Value;
         plugin.Class = xmlNode.Attributes[XML_ATTR_PLUGIN_DRIVER].Value;
         plugin.XmlPluginNode = xmlNode;

         if (xmlNode.Attributes[XML_ATTR_PLUGIN_DEFAULT] != null)
         {
            plugin.Default = xmlNode.Attributes[XML_ATTR_PLUGIN_DEFAULT].Value.Equals("1") ? true : false;
         }

         nodeList = ((XmlElement)xmlNode).GetElementsByTagName(XML_NODE_PARAMETER);
         foreach (XmlNode node in nodeList)
         {
            plugin.Settings.Add(node.Attributes[XML_ATTR_PARAM_KEY].Value, 
                                ReplaceTags(node.Attributes[XML_ATTR_PARAM_VALUE].Value));
         }

         return plugin;
      }

      /// <summary>
      /// Obtiene el valor de un parámetro de un TAG determinado.
      /// </summary>
      private string ReadTagParameter(XmlDocument xmlDoc, string tagName, string defaultValue)
      {
         XmlNodeList nodes = xmlDoc.GetElementsByTagName(tagName);
         if (nodes.Count <= 0)
         {
            return defaultValue;
         }

         if (nodes[0].Attributes.Count <= 0)
         {
            return defaultValue;
         }

         return nodes[0].Attributes[XML_ATTR_PLUGIN_DEFAULT].Value;
      }

      /// <summary>
      /// Replace ID tags from element plugin ID.
      /// </summary>
      private string ReplaceTags(string elementId)
      {
         // Replace ID tags
         if (elementId.Contains(TAG_MACHINE_NAME))
         {
            elementId = elementId.Replace(TAG_MACHINE_NAME, Environment.MachineName.Trim().ToLower());
         }

         return elementId;
      }

      #endregion

   }
}
