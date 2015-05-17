using System;
using System.Collections.Generic;

namespace Cosmo.Utils.Json
{

   /// <summary>
   /// JSONArrayCollection is an ordered collection of values. An array begins with 
   /// "[" (left bracket) and ends with "]" (right bracket). Array elements are 
   /// separated by "," (comma).
   /// </summary>
   public class MWJsonArrayCollection : MWJsonValueCollection
   {
      /// <summary>
      /// Internal generic list of JSONValue objects that comprise the elements of the JSONArrayCollection.
      /// </summary>
      protected List<MWJsonValue> Values;

      /// <summary>
      /// Public constructor that accepts a generic list of JSONValue objects.
      /// </summary>
      /// <param name="values">Generic list of JSONValue objects.</param>
      public MWJsonArrayCollection(List<MWJsonValue> values) : base()
      {
         this.Values = values;
      }

      /// <summary>
      /// Empty public constructor. Use this method in conjunction with
      /// the Add method to populate the internal array of elements.
      /// </summary>
      public MWJsonArrayCollection() : base()
      {
         this.Values = new List<MWJsonValue>();
      }

      /// <summary>
      /// Adds a JSONValue to the internal object array.  Values are checked to 
      /// ensure no duplication occurs in the internal array.
      /// </summary>
      /// <param name="value">JSONValue to add to the internal array</param>
      public void Add(MWJsonValue value)
      {
         if (!this.Values.Contains(value))
            this.Values.Add(value);
      }

      /// <summary>
      /// Required override of the CollectionToPrettyPrint() method.
      /// </summary>
      /// <returns>the entire collection as a string in JSON-compliant format, with indentation for readability</returns>
      protected override string CollectionToPrettyPrint()
      {
         MWJsonValue.CURRENT_INDENT++;
         List<string> output = new List<string>();
         List<string> nvps = new List<string>();
         foreach (MWJsonValue jv in this.Values)
            nvps.Add("".PadLeft(MWJsonValue.CURRENT_INDENT, Convert.ToChar(base.HORIZONTAL_TAB)) + jv.PrettyPrint());
         output.Add(string.Join(base.JSONVALUE_SEPARATOR + Environment.NewLine, nvps.ToArray()));
         MWJsonValue.CURRENT_INDENT--;

         return string.Join("", output.ToArray());
      }

      /// <summary>
      /// Required override of the CollectionToString() method.
      /// </summary>
      /// <returns>the entire collection as a string in JSON-compliant format</returns>
      protected override string CollectionToString()
      {
         List<string> output = new List<string>();
         List<string> nvps = new List<string>();
         foreach (MWJsonValue jv in this.Values)
            nvps.Add(jv.ToString());

         output.Add(string.Join(base.JSONVALUE_SEPARATOR, nvps.ToArray()));
         return string.Join("", output.ToArray());
      }

      /// <summary>
      /// Required override of the BeginMarker property
      /// </summary>
      protected override string BeginMarker
      {
         get { return "["; }
      }

      /// <summary>
      /// Required override of the EndMarker property
      /// </summary>
      protected override string EndMarker
      {
         get { return "]"; }
      }
   }

}
