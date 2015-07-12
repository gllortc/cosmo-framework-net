using Cosmo.Data.Validation;
using System;

namespace Cosmo.Data.ORM
{
   /// <summary>
   /// Implementa un mapeo de campo de formulario ORM.
   /// </summary>
   [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
   public class MappingFieldGroup : System.Attribute
   {
      // Internal data declarations
      // private string _id;
      // private string _title;
      // private string _description;

      /// <summary>
      /// Gets a new instance of <see cref="MappingField"/>.
      /// </summary>
      public MappingFieldGroup()
      {
         Initialize();
      }

      public string ID { get; set; }
      /*{
         get { return _id; }
         set { _id = value; }
      }*/

      public string Title { get; set; }
      /*{
         get { return _title; }
         set { _title = value; }
      }*/

      public string Description { get; set; }
      /*{
         get { return _description; }
         set { _description = value; }
      }*/

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         // _id = string.Empty;
         // _title = string.Empty;
         // _description = string.Empty;

         this.ID = string.Empty;
         this.Title = string.Empty;
         this.Description = string.Empty;
      }
   }
}
