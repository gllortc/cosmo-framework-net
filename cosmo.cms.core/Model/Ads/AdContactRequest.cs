using Cosmo.Data.ORM;
using Cosmo.UI.Controls;

namespace Cosmo.Cms.Model.Ads
{
   /// <summary>
   /// Implementa una petición de contacto a un usuario que ha publicado un anuncio clasificado.
   /// </summary>
   [MappingObject(Caption = "Formulario de contacto",
                  CaptionIcon = IconControl.ICON_ENVELOPE,
                  Description = "Si desea contactar con el autor del anuncio por correo electrónico use este formulario. El autor recibirá su mensaje y podrá contestarle a la cuenta de correo electrónico que proporcione.",
                  FormStyle = OrmEngine.OrmFormStyle.Standard)]
   public class AdContactRequest
   {
      // Internal data declarations
      private string _name;
      private string _email;
      private string _message;
      private string _ip;
      private int _objId;

      /// <summary>
      /// Gets a new instance of <see cref="AdContactRequest"/>.
      /// </summary>
      public AdContactRequest()
      {
         Initialize();
      }

      /// <summary>
      /// Gets or sets el nombre de la persona que desea establecer el contacto.
      /// </summary>
      [MappingField(FieldName = "na",
                    Label = "Nombre",
                    DataType = MappingDataType.String,
                    Required = true)]
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Gets or sets la cuenta de correo electrónico de la persona que desea establecer el contacto.
      /// </summary>
      [MappingField(FieldName = "em",
                    Label = "Correo electrónico",
                    DataType = MappingDataType.Mail,
                    Required = true)]
      public string Mail
      {
         get { return _email; }
         set { _email = value; }
      }

      /// <summary>
      /// Gets or sets el mensaje (en texto plano) a enviar.
      /// </summary>
      [MappingField(FieldName = "mg",
                    Label = "Mensaje",
                    DataType = MappingDataType.MultilineString,
                    Required = true)]
      public string Message
      {
         get { return _message; }
         set { _message = value; }
      }

      /// <summary>
      /// Gets or sets el identificador del anuncio para el que se desea mandar el mensaje de contacto.
      /// </summary>
      [MappingField(FieldName = Workspace.PARAM_OBJECT_ID,
                    DataType = MappingDataType.Hidden,
                    Required = false)]
      public int ClassifiedAdId
      {
         get { return _objId; }
         set { _objId = value; }
      }

      /// <summary>
      /// Gets or sets la dirección IP desde la que se envió el mensaje.
      /// </summary>
      [MappingField(FieldName = "ip",
                    DataType = MappingDataType.String,
                    ManualSet = true)]
      public string IpAddress
      {
         get { return _ip; }
         set { _ip = value; }
      }

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _name = string.Empty;
         _email = string.Empty;
         _message = string.Empty;
         _objId = 0;
         _ip = string.Empty;
      }
   }
}
