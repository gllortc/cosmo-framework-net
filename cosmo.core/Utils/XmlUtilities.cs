using System.Xml;

namespace Cosmo.Utils
{
   /// <summary>
   /// Clase <em>helper</em> con diversasutilidades para tratar documentos XML.
   /// </summary>
   public class XmlUtilities
   {

      private const string XML_ATTR_PLUGIN_DEFAULT = "default";

      /// <summary>
      /// Obtiene el valor de un parámetro de un TAG determinado.
      /// </summary>
      private static string ReadTagParameter(XmlDocument xmlDoc, string tagName, string defaultValue)
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
   }
}
