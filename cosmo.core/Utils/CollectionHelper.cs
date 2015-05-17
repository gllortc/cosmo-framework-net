using System.Collections;

namespace Cosmo.Utils
{
   /// <summary>
   /// Collection helper class.
   /// </summary>
   internal static class CollectionHelper
   {
      /// <summary>
      /// Checks whether a collection is the same as another collection.
      /// </summary>
      /// <param name="listA">The list A.</param>
      /// <param name="listB">The list B.</param>
      /// <returns>
      /// True if the two collections contain all the same items in the same order.
      /// </returns>
      public static bool IsEqualTopp(IEnumerable listA, IEnumerable listB)
      {
         // Check if the objects are equal
         if (listA == listB) return true;

         // Check if one of the values is null
         if ((listA == null) || (listB == null)) return false;

         // Get enumerators
         IEnumerator enumeratorA = listA.GetEnumerator();
         IEnumerator enumeratorB = listB.GetEnumerator();

         // Get first value
         bool enumAHasValue = enumeratorA.MoveNext();
         bool enumBHasValue = enumeratorB.MoveNext();

         // Loop until one of the enumerations reaches its end
         while (enumAHasValue && enumBHasValue)
         {
            // Compare
            if (!enumeratorA.Current.Equals(enumeratorB.Current)) return false;

            // Move to next object
            enumAHasValue = enumeratorA.MoveNext();
            enumBHasValue = enumeratorB.MoveNext();
         }

         // If we get here, and both enumerables don't have any value left,
         // they are equal
         return !(enumAHasValue || enumBHasValue);
      }
   }
}
