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

      // Internal data declaration
      private string _defaultId;
      private List<Plugin> _plugins;

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
      public string DefaultPluginId
      {
         get { return _defaultId; }
         set { _defaultId = value; }
      }

      /// <summary>
      /// Contiene la lista 
      /// </summary>
      public List<Plugin> Plugins
      {
         get { return _plugins; }
         set { _plugins = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega un nuevo plugin de forma segura a la colección evitando duplicados en los identificadores.
      /// </summary>
      /// <param name="plugin">La instancia de <see cref="Plugin"/> que se desea agregar.</param>
      public void AddPlugin(Plugin plugin)
      {
         if (GetPlugin(plugin.ID) == null)
         {
            _plugins.Add(plugin);
         }
         else
         {
            throw new ArgumentException("Ya existe un módulo con el ID [" + plugin.ID + "]");
         }
      }

      /// <summary>
      /// Devuelve el plugin seleccionado por defecto.
      /// </summary>
      /// <returns>Una instancia de <see cref="Plugin"/> seleccionado por defecto o <c>null</c> si no se encuentra o no está definido.</returns>
      public Plugin GetDefaultPlugin()
      {
         if (string.IsNullOrWhiteSpace(_defaultId))
         {
            return null;
         }
         else
         {
            return GetPlugin(_defaultId);
         }
      }

      /// <summary>
      /// Obtiene un plugin a partir de su identificador único en la colección.
      /// </summary>
      /// <param name="pluginId">Identificador único del plugin.</param>
      /// <returns>La instancia de <see cref="Plugin"/> solicitada o <c>null</c> si no existe el identificador en la colección.</returns>
      public Plugin GetPlugin(string pluginId)
      {
         foreach (Plugin plugin in _plugins)
         {
            if (plugin.ID.Equals(pluginId))
            {
               return plugin;
            }
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
            this.DefaultPluginId = XmlUtilities.ReadTagParameter(xmlDoc, defaultAttributeName, string.Empty);
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _defaultId = string.Empty;
         _plugins = new List<Plugin>();
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
                                XmlUtilities.ReplaceTags(node.Attributes[XML_ATTR_PARAM_VALUE].Value));
         }

         return plugin;
      }

      #endregion

   }
}
