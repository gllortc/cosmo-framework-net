using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.Net
{

   /// <summary>
   /// Represent Content-Disposition as class.
   /// </summary>
   public class ContentDisposition : InternetTextMessage.Field
   {
      private List<InternetTextMessage.Field> zFields = new List<InternetTextMessage.Field>();

      /// <summary>
      /// Devuelve una instancia de ContentDisposition.
      /// </summary>
      public ContentDisposition(String inValue) : base("Content-Disposition", inValue)
      {
         this.Value = inValue;
      }

      /// <summary>
      /// Devuelve una instancia de ContentDisposition.
      /// </summary>
      public ContentDisposition(String inValue, InternetTextMessage.Field[] inFields) : base("Content-Disposition", inValue)
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
      /// Get or set filename.
      /// </summary>
      public String FileName
      {
         get
         {
            InternetTextMessage.Field f = InternetTextMessage.Field.FindField(this.zFields, "FileName");
            if (f == null)
            {
               return string.Empty;
            }
            return InternetTextMessage.DecodeHeader(f.Value);
         }
         set
         {
            InternetTextMessage.Field f = InternetTextMessage.Field.FindField(this.zFields, "FileName");
            if (f == null)
            {
               f = new InternetTextMessage.Field("FileName", value);
               this.zFields.Add(f);
            }
            else
            {
               f.Value = value;
            }
            this.Value = "attachment";
         }
      }
   }

}
