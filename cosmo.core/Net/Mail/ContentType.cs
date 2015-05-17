using System;
using System.Collections.Generic;

namespace Cosmo.Net
{

   /// <summary>
   /// Represent Content-Type as class.
   /// </summary>
   public class ContentType : InternetTextMessage.Field
   {
      private List<InternetTextMessage.Field> zFields = new List<InternetTextMessage.Field>();

      /// <summary>
      /// Devuelve una instancia de ContentType.
      /// </summary>
      public ContentType(String inValue) : base("Content-Type", inValue)
      {
         this.Value = inValue;
      }

      /// <summary>
      /// Devuelve una instancia de ContentType.
      /// </summary>
      public ContentType(String inValue, InternetTextMessage.Field[] inFields) : base("Content-Type", inValue)
      {
         this.Value = inValue;
         for (int i = 0; i < inFields.Length; i++)
         {
            this.zFields.Add(inFields[i]);
         }
      }

      /// <summary>
      /// Get field collection.
      /// </summary>
      public List<InternetTextMessage.Field> Fields
      {
         get { return this.zFields; }
      }

      /// <summary>
      /// Get or set name.
      /// </summary>
      public String Name
      {
         get
         {
            InternetTextMessage.Field f = InternetTextMessage.Field.FindField(this.zFields, "Name");
            if (f == null)
            {
               return string.Empty;
            }
            return InternetTextMessage.DecodeHeader(f.Value);
         }
         set
         {
            InternetTextMessage.Field f = InternetTextMessage.Field.FindField(this.zFields, "Name");
            if (f == null)
            {
               f = new InternetTextMessage.Field("Name", value);
               this.zFields.Add(f);
            }
            else
            {
               f.Value = value;
            }
         }
      }

      /// <summary>
      /// Get or set boundary.
      /// </summary>
      public String Boundary
      {
         get
         {
            InternetTextMessage.Field f = InternetTextMessage.Field.FindField(this.zFields, "Boundary");
            if (f == null)
            {
               return string.Empty;
            }
            return f.Value;
         }
         set
         {
            InternetTextMessage.Field f = InternetTextMessage.Field.FindField(this.zFields, "Boundary");
            if (f == null)
            {
               f = new InternetTextMessage.Field("Boundary", value);
               this.zFields.Add(f);
            }
            else
            {
               f.Value = value;
            }
         }
      }

   }

}
