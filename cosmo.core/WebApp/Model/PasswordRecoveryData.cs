using Cosmo.Data.ORM;

namespace Cosmo.WebApp.Common.Model
{
   [MappingObject(FormStyle = OrmEngine.OrmFormStyle.Standard)]
   public class PasswordRecoveryData
   {
      private string _mail;

      /// <summary>
      /// Devuelve una instancia de <see cref="PasswordRecoveryData"/>.
      /// </summary>
      public PasswordRecoveryData()
      {
         Initialize();
      }

      /// <summary>
      /// Cuenta de correo electrónico principal
      /// </summary>
      [MappingField(FieldName = "ml",
                    Label = "Correo electrónico",
                    DataType = MappingDataType.Mail,
                    Required = true)]
      public string Mail
      {
         get { return _mail; }
         set { _mail = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _mail = string.Empty;
      }
   }
}
