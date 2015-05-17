using System.Collections.Generic;
using System.Collections.Specialized;

namespace Cosmo.Utils
{
   /// <summary>
   /// Clase <em>helper</em> para la manipulación de colecciones.
   /// </summary>
   public static class Collections
   {
      /// <summary>
      /// Indica si existe una determinada clave en una colección.
      /// </summary>
      /// <param name="collection">Colección a analizar.</param>
      /// <param name="key">Clave que se desea rastrear.</param>
      /// <returns><c>true</c> si la colección contiene la clave proporcionada o <c>false</c> en cualquier otro caso.</returns>
      public static bool ContainsKey(NameObjectCollectionBase collection, string key)
      {
         foreach (string ckey in collection.Keys)
         {
            if (ckey.Equals(key))
            {
               return true;
            }
         }
         return false;
      }

      /*public static List<T> AppendList(List<T> list, List<T> listToAppend)
      {
         return null;
      }*/
   }
}
