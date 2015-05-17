using System.Xml;

namespace Cosmo.Utils
{
   /// <summary>
   /// Clase <em>helper</em> con diversasutilidades para tratar documentos XML.
   /// </summary>
   public class XmlUtilities
   {
      private const string TAG_MACHINE_NAME = "[%MACHINENAME%]";

      private const string XML_ATTR_PLUGIN_DEFAULT = "default";

      /// <summary>
      /// Obtiene el valor de un parámetro de un TAG determinado.
      /// </summary>
      public static string ReadTagParameter(XmlDocument xmlDoc, string tagName, string defaultValue)
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

         return ReplaceTags(nodes[0].Attributes[XML_ATTR_PLUGIN_DEFAULT] == null ? defaultValue : nodes[0].Attributes[XML_ATTR_PLUGIN_DEFAULT].Value);
      }

      /// <summary>
      /// Reemplaza los valores variables en un valor obtenido de un nodo o atributo XML.
      /// </summary>
      /// <param name="value">Valor original a analizar.</param>
      /// <returns>Valor original con las variables transformadas.</returns>
      public static string ReplaceTags(string value)
      {
         if (value.Contains(TAG_MACHINE_NAME))
         {
            value = value.Replace(TAG_MACHINE_NAME, System.Environment.MachineName.Trim().ToLower());
         }

         return value;
      }
   }
}
