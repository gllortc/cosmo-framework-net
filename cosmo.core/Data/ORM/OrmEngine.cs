using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace Cosmo.Data.ORM
{
   /// <summary>
   /// Implementa el motor ORM de Cosmo.
   /// </summary>
   public class OrmEngine
   {

      #region Enumerations

      /// <summary>
      /// Determina el estilo de generación de formularios para el objeto.
      /// </summary>
      public enum OrmFormStyle
      {
         /// <summary>Formulario estándar, colocando todos los grupos de forma consecutiva.</summary>
         Standard,
         /// <summary>Formulario tabular, colocando todos los grupos en pestañas.</summary>
         Tabbed
      }

      #endregion

      /// <summary>TAG para incluir el nombre del workspace</summary>
      public const string TAG_WORKSPACE_NAME = "<%WORKSPACE-NAME%>";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="OrmEngine"/>.
      /// </summary>
      public OrmEngine()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Lista de campos que no se deben tener en cuenta a la hora de generar y/o procesar los formularios.
      /// </summary>
      public List<String> DiscardFields { get; set; }

      /// <summary>
      /// Devuelve o establece la forma de representar los grupos de controles en los formularios generados.
      /// </summary>
      public OrmEngine.OrmFormStyle FormStyle { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Inicializa una determinada instancia con los datos de un formulario. Si la validación de los datos
      /// falla marca los campos como error de datos para su posterior representación.
      /// </summary>
      /// <param name="obj">Una instancia del tipo que se desea rellenar a partir de los datos recibidos desde el formulario.</param>
      /// <param name="form">Una instancia de <see cref="FormControl"/> que contiene el formulario necesario para recoger los datos de la instancia.</param>
      /// <param name="parameters">Una instancia de <see cref="Url"/> que contiene los datos proporcionados por el usuario en el formulario.</param>
      /// <returns><c>true</c> si la validación del formulario es correcta o <c>false</c> en cualquier otro caso.</returns>
      public bool ProcessForm(Object obj, FormControl form, Url parameters)
      {
         bool isValid = true;
         object[] attrs;
         MappingField fld;
         FormField formField;

         // Obtiene todos los campos del formulario
         List<FormField> fields = form.Content.GetFormFields();

         foreach (PropertyInfo property in obj.GetType().GetProperties())
         {
            fld = null;

            attrs = property.GetCustomAttributes(typeof(MappingField), false);
            if (attrs.Length > 0)
            {
               fld = (MappingField)attrs[0];
            }

            if (fld != null)
            {
               if (parameters.ContainsKey(fld.FieldName))
               {
                  formField = form.GetField(fld.FieldName);
                  if (formField == null)
                  {
                     return false;
                  }
                  else
                  {
                     // Establece el valor en el formulario
                     form.SetFieldValue(fld.FieldName, parameters.Parameters[fld.FieldName]);

                     // Si el campo requiere re-escritura 
                     if (fld.RewriteRequired)
                     {

                     }

                     // Valida el contenido del campo
                     isValid = isValid & formField.Validate();

                     // Establece el valor en la instancia
                     property.SetValue(obj, ConvertProperty(property, parameters.Parameters[fld.FieldName]), null);
                  }
               }
            }
         }

         // Verifica el CAPTCHA si existe en el formulario
         FormField captcha = form.GetField(FormFieldCaptcha.CAPTCHA_FIELD_NAME);
         if (captcha != null)
         {
            form.SetFieldValue(FormFieldCaptcha.CAPTCHA_FIELD_NAME, parameters.Parameters[FormFieldCaptcha.CAPTCHA_FIELD_NAME]);
            isValid = isValid & captcha.Validate();
         }

         return isValid;
      }

      /// <summary>
      /// Obtiene los valores procedentes de un formulario y los establece en una instancia.
      /// </summary>
      /// <param name="obj">Instancia a rellenar.</param>
      /// <param name="parameters">Parámetros recibidos del cliente.</param>
      /// <returns></returns>
      public Object GetObjectData(Object obj, Url parameters)
      {
         object[] attrs;
         MappingField fld;

         foreach (PropertyInfo property in obj.GetType().GetProperties())
         {
            fld = null;

            attrs = property.GetCustomAttributes(typeof(MappingField), false);
            if (attrs.Length > 0)
            {
               fld = (MappingField)attrs[0];
            }

            if (fld != null)
            {
               if (parameters.ContainsKey(fld.FieldName))
               {
                  property.SetValue(obj, ConvertProperty(property, parameters.Parameters[fld.FieldName]), null);
               }
            }
         }

         return obj;
      }

      /// <summary>
      /// Genera el formulario correspondiente a un determinado objeto anotado mediante Cosmo ORM Annotations.
      /// </summary>
      /// <param name="parentView">Contenedor de destino del formulario.</param>
      /// <param name="domId">Control unique identifier in DOM.</param>
      /// <param name="instance">Instancia del objeto para el que se desea crear un formulario.</param>
      /// <param name="enableHumanCheck">Habilita un campo <em>CAPTCHA</em> para impedir envios automatizados en formularios de carácter público.</param>
      /// <returns>Una instancia de <see cref="FormControl"/> que representa el formulario correspondiente al objeto.</returns>
      /// <remarks>
      /// Los campos del formulario tomarán como valor el que se obtenga de las propiedades de la instancia.
      /// </remarks>
      public FormControl CreateForm(View parentView, string domId, Object instance, bool enableHumanCheck)
      {
         FormControl form = null;
         Type type = instance.GetType();

         // Crea el formulario
         form = CreateForm(parentView, domId, instance.GetType(), enableHumanCheck);

         // Para cada propiedad, establece el valor de los campos
         foreach (PropertyInfo property in type.GetProperties())
         {
            foreach (object field in property.GetCustomAttributes(typeof(MappingField), false))
            {
               if (!((MappingField)field).ManualSet && !this.DiscardFields.Contains(((MappingField)field).FieldName))
               {
                  // TODO: Asegurar que no debe tratar diferentes tipos de datos antes de insertar el valor
                  form.SetFieldValue(((MappingField)field).FieldName, property.GetValue(instance, null).ToString());
               }
            }
         }

         return form;
      }

      /// <summary>
      /// Genera el formulario correspondiente a un determinado objeto anotado mediante Cosmo ORM Annotations.
      /// </summary>
      /// <param name="parentView">Contenedor de destino del formulario.</param>
      /// <param name="domId">Control unique identifier in DOM.</param>
      /// <param name="type">Tipo correspondiente al objeto para el qual se desea representar el formulario.</param>
      /// <param name="enableHumanCheck">Habilita un campo <em>CAPTCHA</em> para impedir envios automatizados en formularios de carácter público.</param>
      /// <returns>Una instancia de <see cref="FormControl"/> que representa el formulario correspondiente al objeto.</returns>
      public FormControl CreateForm(View parentView, string domId, Type type, bool enableHumanCheck)
      {
         FormControl form = new FormControl(parentView, domId);
         form.Method = "post";

         // Agrega la decoración del formulario
         MappingObject objInfo = GetMappingObject(type);
         if (objInfo == null)
         {
            throw new OrmException("El tipo " + type.ToString() + " no contiene la información de mapeado necesaria.");
         }

         if (!string.IsNullOrWhiteSpace(objInfo.Caption))
         {
            form.Caption = objInfo.Caption;
         }

         if (!string.IsNullOrWhiteSpace(objInfo.CaptionIcon))
         {
            form.Icon = objInfo.CaptionIcon;
         }

         if (!string.IsNullOrWhiteSpace(objInfo.Description))
         {
            form.Content.Add(new HtmlContentControl(parentView, objInfo.Description));
         }

         // Agrega los controles no agrupados
         AddGroupFields(null, form, type);

         // Agrega los grupos de controles
         foreach (MappingFieldGroup group in GetFieldGroups(type))
         {
            AddGroupFields(group, form, type);
         }

         // Agrega comprobación captcha
         if (enableHumanCheck)
         {
            AddHumanCheck(form);
         }

         // Agrega el botón de envio
         form.FormButtons.Add(new ButtonControl(parentView, "cmdSend", "Aceptar", ButtonControl.ButtonTypes.Submit));

         return form;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.FormStyle = OrmFormStyle.Standard;
         this.DiscardFields = new List<string>();
      }

      /// <summary>
      /// Agrega campos mapeados al formulario.
      /// </summary>
      /// <param name="group">
      /// Una instancia de <see cref="MappingFieldGroup"/> que indica el grupo de controles dónde agregar los campos. 
      /// Si es <c>null</c> se agregarán desagrupados al principio del formulario.
      /// </param>
      /// <param name="modal">Formulario dónde se agregarán los controles.</param>
      /// <param name="type">Tipo de objeto para el que se está creando el formulario.</param>
      private void AddGroupFields(MappingFieldGroup group, FormControl form, Type type)
      {
         MappingField field;

         // Determina si el grupo tiene controles
         if (CountGroupFields(group, type) <= 0)
         {
            return;
         }

         // Genera la cabecera del grupo
         if (group != null)
         {
            HtmlContentControl adsContent = new HtmlContentControl(form.ParentView);
            adsContent.AppendParagraph(group.Description);

            form.Content.Add(new HtmlContentControl(form.ParentView, "<h2 class=\"page-header\">" + HttpUtility.HtmlDecode(group.Title) + "</h2>"));
            form.Content.Add(adsContent);
         }

         // Agrega los controles
         foreach (PropertyInfo property in type.GetProperties())
         {
            field = GetMappingField(property);
            if (field == null) continue;

            if (group == null)
            {
               if (string.IsNullOrWhiteSpace(field.GroupID))
               {
                  form.Content.Add(CreateField(form, field));
               }
            }
            else
            {
               if (group.ID.Equals(field.GroupID) && !this.DiscardFields.Contains(field.FieldName))
               {
                  form.Content.Add(CreateField(form, field));
               }
            }
         }
      }

      /// <summary>
      /// Devuelve el número de campos de un determinado grupo.
      /// </summary>
      /// <param name="group">
      /// Una instancia de <see cref="MappingFieldGroup"/> que indica el grupo para el que se desea 
      /// obtener el número de campos. Si es <c>null</c> se contabilizarán los campos desagrupados.
      /// </param>
      /// <param name="type">Tipo de objeto para el que se está creando el formulario.</param>
      /// <returns>Un número entero que indica el número de campos en un determinado grupo.</returns>
      private int CountGroupFields(MappingFieldGroup group, Type type)
      {
         int num = 0;
         MappingField field;

         // Agrega los controles
         foreach (PropertyInfo property in type.GetProperties())
         {
            field = GetMappingField(property);
            if (field == null) continue;

            if (group == null)
            {
               if (string.IsNullOrWhiteSpace(field.GroupID))
               {
                  num++;
               }
            }
            else
            {
               if (group.ID.Equals(field.GroupID) && !this.DiscardFields.Contains(field.FieldName))
               {
                  num++;
               }
            }
         }

         return num;
      }

      /// <summary>
      /// Agrega una verificación de seguridad basada en <em>CAPTCHA</em>.
      /// </summary>
      /// <param name="modal">La instancia de <see cref="FormControl"/> dónde se agregará el control.</param>
      private void AddHumanCheck(FormControl form)
      {
         FormFieldCaptcha captcha = new FormFieldCaptcha(form.ParentView);
         captcha.Label = "Código de verificación";
         captcha.Placeholder = "Copie aquí el código que aparece a la izquierda";

         // Genera la cabecera del grupo
         form.Content.Add(new HtmlContentControl(form.ParentView, "<h2 class=\"page-header\">Verificación</h2>"));
         form.Content.Add(new HtmlContentControl(form.ParentView).AppendParagraph("Para evitar envios automatizados (<em>spam</em>), debe copiar el código en el campo de texto, respetando mayúsculas y minúsculas."));
         form.Content.Add(captcha);
      }

      /// <summary>
      /// Genera un campo a partir de una propiedad mapeada.
      /// </summary>
      /// <param name="modal">La instancia de <see cref="FormControl"/> dónde se agregará el nuevo campo.</param>
      /// <param name="field">Campo mapeado.</param>
      /// <returns>Un control que representa el campo generado.</returns>
      private Control CreateField(FormControl form, MappingField field)
      {
         if (field.ManualSet)
         {
            // No realiza ninguna acción
         }
         else if (string.IsNullOrWhiteSpace(field.DataListID))
         {
            switch (field.DataType)
            {
               case MappingDataType.Hidden:
                  form.AddFormSetting(field.FieldName, string.Empty);
                  break;

               case MappingDataType.Boolean:
                  FormFieldBoolean boolField = new FormFieldBoolean(form.ParentView, field.FieldName, field.Label);
                  boolField.Required = field.Required;
                  return boolField;

               case MappingDataType.Date:
                  FormFieldDate dateField = new FormFieldDate(form.ParentView, field.FieldName, field.Label, FormFieldDate.FieldDateType.Date);
                  dateField.Required = field.Required;
                  return dateField;

               case MappingDataType.Decimal:
                  // TODO
                  break;

               case MappingDataType.Integer:
                  // TODO
                  break;

               case MappingDataType.Login:
                  FormFieldText loginField = new FormFieldText(form.ParentView, field.FieldName, field.Label, FormFieldText.FieldDataType.Text);
                  loginField.Required = field.Required;
                  return loginField;

               case MappingDataType.Mail:
                  FormFieldText mailField = new FormFieldText(form.ParentView, field.FieldName, field.Label, FormFieldText.FieldDataType.Email);
                  mailField.Required = field.Required;
                  return mailField;

               case MappingDataType.MultilineString:
                  FormFieldEditor txaField = new FormFieldEditor(form.ParentView, field.FieldName, field.Label, FormFieldEditor.FieldEditorType.Simple);
                  txaField.Required = field.Required;
                  return txaField;

               case MappingDataType.Password:
                  FormFieldPassword pwdField = new FormFieldPassword(form.ParentView, field.FieldName, field.Label);
                  pwdField.Required = field.Required;
                  pwdField.RewriteRequired = field.RewriteRequired;
                  pwdField.RewriteFieldPlaceholder = "Vuelva a escribir aquí la contraseña";
                  return pwdField;

               case MappingDataType.Url:
                  FormFieldText urlField = new FormFieldText(form.ParentView, field.FieldName, field.Label, FormFieldText.FieldDataType.Url);
                  urlField.Required = field.Required;
                  return urlField;

               default:
                  FormFieldText txtField = new FormFieldText(form.ParentView, field.FieldName, field.Label, FormFieldText.FieldDataType.Text);
                  txtField.Required = field.Required;
                  return txtField;
            }
         }
         else
         {
            FormFieldList list = new FormFieldList(form.ParentView, field.FieldName, field.Label, FormFieldList.ListType.Single);
            list.Values = form.ParentView.Workspace.DataService.GetDataList(field.DataListID).Values;
            list.Value = form.ParentView.Workspace.DataService.GetDataList(field.DataListID).DefaultValue;

            return list;
         }

         return null;
      }

      /// <summary>
      /// Obtiene el <see cref="MappingObject"/> de una clase determinada.
      /// </summary>
      /// <param name="type">Tipo para el que se desea obtener la información ORM.</param>
      /// <returns>La instancia de <see cref="MappingObject"/> solicitada o <c>null</c> si la clase no contiene la anotación.</returns>
      private MappingObject GetMappingObject(Type type)
      {
         // Obtiene el mapping de la propiedad
         object[] attrs = type.GetCustomAttributes(typeof(MappingObject), false);

         if (attrs.Length > 0)
         {
            return (MappingObject)attrs[0];
         }
         else
         {
            return null;
         }
      }

      /// <summary>
      /// Obtiene el <see cref="MappingField"/> de una determinada propiedad.
      /// </summary>
      /// <param name="property">Propiedad para la que se desea obtener la información ORM.</param>
      /// <returns>La instancia de <see cref="MappingField"/> solicitada o <c>null</c> si la propiedad no contiene la anotación.</returns>
      private MappingField GetMappingField(PropertyInfo property)
      {
         // Obtiene el mapping de la propiedad
         object[] attrs = property.GetCustomAttributes(typeof(MappingField), false);
         
         if (attrs.Length > 0)
         {
            return (MappingField)attrs[0];
         }
         else
         {
            return null;
         }
      }

      /// <summary>
      /// Obtiene una lista de los grupos de controles que tiene el formulario.
      /// </summary>
      /// <param name="type">Tipo correspondiente al objeto para el qual se desea representar el formulario.</param>
      /// <returns>Una lista de instancias de <see cref="MappingFieldGroup"/>.</returns>
      private List<MappingFieldGroup> GetFieldGroups(Type type)
      {
         List<MappingFieldGroup> lst = new List<MappingFieldGroup>();

         foreach (object grp in type.GetCustomAttributes(typeof(MappingFieldGroup), false))
         {
            lst.Add((MappingFieldGroup)grp);
         }

         lst.Sort(new MappingFieldGroupComparator());

         return lst;
      }

      /// <summary>
      /// Convierte un valor obtenido como parámetro de llamada (string) al tipo indicado por una propiedad.
      /// </summary>
      private object ConvertProperty(PropertyInfo property, string value)
      {
         if (Cosmo.Utils.Number.IsIntegerType(property.PropertyType))
         {
            if (string.IsNullOrWhiteSpace(value))
            {
               return 0;
            }
            else
            {
               return Cosmo.Utils.Number.Val(value);
            }
         }
         else if (Cosmo.Utils.Number.IsDecimalType(property.PropertyType))
         {
            if (string.IsNullOrWhiteSpace(value))
            {
               return 0.0D;
            }
            else
            {
               return float.Parse(value);
            }
         }
         else if (Cosmo.Utils.Calendar.IsDateType(property.PropertyType))
         {
            if (string.IsNullOrWhiteSpace(value))
            {
               return null;
            }
            else
            {
               return DateTime.Parse(value);
            }
         }
         else
         {
            return value;
         }
      }

      #endregion

      #region Child Classes

      /// <summary>
      /// Comparador que permite ordenar alfabéticamente los grupos de controles.
      /// </summary>
      class MappingFieldGroupComparator : Comparer<MappingFieldGroup>
      {
         public override int Compare(MappingFieldGroup x, MappingFieldGroup y)
         {
            return x.Title.CompareTo(y.Title);
         }
      }

      #endregion

   }
}
