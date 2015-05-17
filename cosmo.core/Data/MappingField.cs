
namespace Cosmo.Data.ORM
{
   [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field, AllowMultiple = false)]
   public class MappingField : System.Attribute
   {
      private string _fieldName;

      public MappingField(string fieldName)
      {
         _fieldName = fieldName;
      }

      public string FieldName
      {
         get { return _fieldName; }
         set { _fieldName = value; }
      }
   }
}
