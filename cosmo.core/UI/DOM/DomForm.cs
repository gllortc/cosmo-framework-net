using System.Collections.Generic;
using System.Text;
using Cosmo.Data.Validation;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un menú DOM.
   /// </summary>
   public class DomForm : DomContentComponentBase
   {

      #region Enumerations

      /// <summary>
      /// Describe los tipos de codificación de los datos enviados.
      /// </summary>
      public enum FormEncodingTypes
      {
         /// <summary>Texto plano</summary>
         PlainText,
         /// <summary>Múltiples partes (para envío de archivos)</summary>
         MultiPartData
      }

      /// <summary>
      /// Describe los métodos de envio para el formulario.
      /// </summary>
      public enum FormMethodTypes
      {
         /// <summary>GET</summary>
         Get,
         /// <summary>POST</summary>
         Post
      }

      #endregion

      bool _validate;
      string _title;
      string _description;
      string _action;
      FormMethodTypes _method;
      FormEncodingTypes _encType;
      NavigationTarget _target;
      List<DomFormFieldset> _fieldsets;
      List<DomFormButton> _buttons;

      #region Constants

      /// <summary>Cabecera del formulario</summary>
      internal const string SECTION_HEAD = "form-head";
      /// <summary>Cabecera de un grupo de controles</summary>
      internal const string SECTION_FIELDSET_HEAD = "form-fieldset-head";
      /// <summary>Control del formulario (TextBox, ComboBox)</summary>
      internal const string SECTION_CONTROL_TEXT = "form-control";
      /// <summary>Control del formulario (Checkbox, Radio button)</summary>
      internal const string SECTION_CONTROL_OPTION = "form-option";
      /// <summary>Control del formulario (TextArea)</summary>
      internal const string SECTION_CONTROL_TEXTAREA = "form-textarea";
      /// <summary>Control del formulario (Captcha)</summary>
      internal const string SECTION_CONTROL_CAPTCHA = "form-captcha";
      /// <summary>Pié de un grupo de controles</summary>
      internal const string SECTION_FIELDSET_FOOT = "form-fieldset-footer";
      /// <summary>Zona de botones del formulario.</summary>
      internal const string SECTION_BUTTONS = "form-buttons";
      /// <summary>Pié del formulario</summary>
      internal const string SECTION_FOOT = "form-footer";

      /// <summary>Identificador del bloque XHTML generado.</summary>
      public const string TAG_HTML_ID = "oid";
      /// <summary>Tag: ID de plantilla</summary>
      public const string TAG_TEMPLATE_ID = "tid";
      /// <summary>Tag: Posición de la apertura del TAG Form (lo situa la clase</summary>
      public const string TAG_FORM_OPENTAG = "fopentag";
      /// <summary>Tag: Título</summary>
      public const string TAG_FORM_TITLE = "ftitle";
      /// <summary>Tag: Descripción del menú</summary>
      public const string TAG_FORM_DESCRIPTION = "fdesc";
      /// <summary>Tag: Etiqueta del campo</summary>
      public const string TAG_FIELD_LABEL = "flabel";
      /// <summary>Tag: Control del campo</summary>
      public const string TAG_FIELD_CONTROL = "fcontrol";
      /// <summary>Tag: Descripción del campo</summary>
      public const string TAG_FIELD_DESCRIPTION = "desc";
      /// <summary>Tag: Control del campo</summary>
      public const string TAG_FIELD_CAPTCHA_IMAGE = "fcaptchaimg";
      /// <summary>Tag: Botones del formulario</summary>
      public const string TAG_FORM_BUTTONS = "fbuttons";

      /// <summary>Atributo: Separador de botones</summary>
      public const string TAG_ATTR_SEPARATOR = "separator";
      /// <summary>Atributo: Clase CSS para los botones</summary>
      public const string TAG_ATTR_BUTTON_CLASS = "css-class";
      /// <summary>Atributo: Clase CSS para los votones resaltados</summary>
      public const string TAG_ATTR_BUTTON_CLASS_ACTIVE = "css-class-active";

      /// <summary>Nombre del campo de control de acciones</summary>
      internal const string PARAM_FORM_ACTION = "cd__fa";
      /// <summary>Nombre del campo de control de ID de formulario</summary>
      internal const string PARAM_FORM_ID = "cd__fi";

      /// <summary>Acción: Send Form</summary>
      internal const string ACTION_SEND = "_send_";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomMenu"/>.
      /// </summary>
      public DomForm() : base()
      {
         Clear();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomMenu"/>.
      /// </summary>
      /// <param name="id">Identificador único del formulario en la página dónde debe mostrarse.</param>
      public DomForm(string id) : base()
      {
         Clear();

         this.ID = id;
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "form"; }
      }

      /// <summary>
      /// Nombre del script que recibirá los datos del formulario.
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Nombre del script que recibirá los datos del formulario.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Nombre del script que recibirá los datos del formulario.
      /// </summary>
      public string Action
      {
         get { return _action; }
         set { _action = value; }
      }

      /// <summary>
      /// Método de envio de datos del formulario.
      /// </summary>
      public FormMethodTypes Method
      {
         get { return _method; }
         set { _method = value; }
      }

      /// <summary>
      /// Método de envio de datos del formulario.
      /// </summary>
      public FormEncodingTypes EncodingType
      {
         get { return _encType; }
         set { _encType = value; }
      }

      /// <summary>
      /// Método de envio de datos del formulario.
      /// </summary>
      public NavigationTarget Target
      {
         get { return _target; }
         set { _target = value; }
      }

      /// <summary>
      /// Grupos de opciones contenidos en el menú.
      /// </summary>
      public List<DomFormFieldset> Fieldsets
      {
         get { return _fieldsets; }
         set { _fieldsets = value; }
      }

      /// <summary>
      /// Botones del formulario.
      /// </summary>
      public List<DomFormButton> Buttons
      {
         get { return _buttons; }
         set { _buttons = value; }
      }

      /// <summary>
      /// Indica si se debe validar el contenido en el cliente.
      /// </summary>
      /// <remarks>
      /// Por defecto, el valor de esta propiedad es <c>true</c>.
      /// </remarks>
      public bool ClientValidation
      {
         get { return _validate; }
         set { _validate = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Reinicializa el menú y lo deja listo para crear uno nuevo.
      /// </summary>
      public void Clear()
      {
         this.ID = "frm1";
         _title = string.Empty;
         _description = string.Empty;
         _action = string.Empty;
         _method = FormMethodTypes.Get;
         _encType = FormEncodingTypes.PlainText;
         _target = NavigationTarget.Self;
         _title = string.Empty;
         _description = string.Empty;
         _fieldsets = new List<DomFormFieldset>();
         _buttons = new List<DomFormButton>();
         _validate = true;
      }

      /// <summary>
      /// Valida el formulario usando las reglas especificadas.
      /// </summary>
      /// <returns><c>true</c> si los valores son correctos o <c>false</c> en cualquier otro caso.</returns>
      public bool Validate()
      {
         bool isValid = false;
         bool ruleRes = false;

         // Recupera los datos de los campos
         foreach (DomFormFieldset fieldset in this.Fieldsets)
         {
            foreach (DomFormField field in fieldset.Fields)
            {
               if (field.ValidationRule is ValidationRuleBase)
               {
                  ruleRes = ((ValidationRuleBase)field.ValidationRule).CheckRule(field.Value);

                  isValid = isValid | ruleRes;
               }
            }
         }

         return isValid;
      }

      /// <summary>
      /// Devuelve el valor de un determinado campo.
      /// </summary>
      /// <param name="name">Nombre del campo.</param>
      public string GetFieldValue(string name)
      {
         name = name.Trim().ToLower();

         // Recupera los datos de los campos
         foreach (DomFormFieldset fieldset in this.Fieldsets)
         {
            foreach (DomFormField field in fieldset.Fields)
            {
               if (field.Name.ToLower().Equals(name))
               {
                  return field.Value;
               }
            }
         }

         return string.Empty;
      }

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el elemento.</returns>
      public override string Render(DomTemplate template, DomPage.ContentContainer container)
      {
         string ctrl = string.Empty;
         string img = string.Empty;
         string xhtml = string.Empty;
         string part = string.Empty;

         // Agrega una referencia obligatoria
         template.Scripts.Add(new DomPageScript("include/plugins/jquery/jquery.validate.min.js"));

         try
         {
            // Obtiene la plantilla del componente
            DomTemplateComponent component = template.GetContentComponent(this.ELEMENT_ROOT, container);
            if (component == null) return string.Empty;

            // Genera el TAG Form del formulario
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<form id=\"{0}\" name=\"{1}\" method=\"{2}\" action=\"{3}\">\n" +
                            "  <input type=\"hidden\" name=\"{4}\" value=\"{5}\" />\n" +
                            "  <input type=\"hidden\" name=\"{6}\" value=\"{7}\" />\n", 
                            this.ID,
                            this.ID,
                            _method == FormMethodTypes.Get ? "get" : "post",
                            this.Action,
                            DomForm.PARAM_FORM_ACTION, 
                            DomForm.ACTION_SEND,
                            DomForm.PARAM_FORM_ID,
                            this.ID);

            // Cabecera del formulario
            part = component.GetFragment(DomForm.SECTION_HEAD);
            part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FORM_OPENTAG, sb.ToString());
            part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FORM_TITLE, this.Title);
            part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FORM_DESCRIPTION, this.Description);
            xhtml = string.Format("{0}{1}", xhtml, part);

            // Grupos de controles
            foreach (DomFormFieldset fieldset in _fieldsets)
            {
               // Cabecera del grupo de controles
               part = component.GetFragment(DomForm.SECTION_FIELDSET_HEAD);
               part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FORM_TITLE, fieldset.Title);
               part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FORM_DESCRIPTION, fieldset.Description);
               xhtml = string.Format("{0}{1}", xhtml, part);

               foreach (DomFormField field in fieldset.Fields)
               {
                  // Genera el control
                  switch (field.Type)
                  {

                     // Controles ocultos
                     case DomFormField.FormFieldTypes.Hidden:
                     {
                        part = string.Format("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />\n", field.Name,
                                                                                                       field.Value);
                        break;
                     }

                     // Controles TextArea
                     case DomFormField.FormFieldTypes.LargeText:
                     {
                        ctrl = string.Format("<textarea id=\"{0}\" name=\"{1}\"{2}{3}{4}>{5}</textarea>", field.Name, 
                                                                                                          field.Name,
                                                                                                          (field.LargeTextColumns > 0 ? " cols=\"" + field.LargeTextColumns + "\"" : string.Empty),
                                                                                                          (field.LargeTextRows > 0 ? " rows=\"" + field.LargeTextRows + "\"" : string.Empty),
                                                                                                          (field.ReadOnly ? " readonly=\"readonly\"" : string.Empty), 
                                                                                                          field.Value);
                        // Genera el XHTML referente al control
                        part = component.GetFragment(DomForm.SECTION_CONTROL_TEXTAREA);
                        part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FIELD_CONTROL, ctrl);

                        break;
                     }

                     case DomFormField.FormFieldTypes.Boolean:
                     {
                        ctrl = string.Format("<input type=\"checkbox\" id=\"{0}\" name=\"{1}\" {2} {3} />", field.Name,
                                                                                                            field.Name,
                                                                                                            (field.Checked ? "checked=\"checked\"" : string.Empty),
                                                                                                            (field.ReadOnly ? "readonly=\"readonly\"" : string.Empty));

                        // Genera el XHTML referente al control
                        part = component.GetFragment(DomForm.SECTION_CONTROL_OPTION);
                        part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FIELD_CONTROL, ctrl);

                        break;
                     }

                     // Control Captcha
                     case DomFormField.FormFieldTypes.Captcha:
                     {
                        img = string.Format("<img id=\"{0}\" src=\"captcha.do\" alt=\"{1}\" />", "img_" + field.Name,
                                                                                                 (string.IsNullOrEmpty(field.Label) ? "Código de verificación" : field.Label));
                        ctrl = string.Format("<input type=\"text\" id=\"{0}\" name=\"{1}\" value=\"\" />", field.Name,
                                                                                                           field.Name);

                        // Genera el XHTML referente al control
                        part = component.GetFragment(DomForm.SECTION_CONTROL_CAPTCHA);
                        part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FIELD_CAPTCHA_IMAGE, img);
                        part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FIELD_CONTROL, ctrl);

                        break;
                     }

                     default:
                     {
                        ctrl = string.Format("<input type=\"text\" id=\"{0}\" name=\"{1}\" value=\"{2}\" {3} />", field.Name, 
                                                                                                                  field.Name, 
                                                                                                                  field.Value,
                                                                                                                  (field.ReadOnly ? "readonly=\"readonly\"" : string.Empty));

                        // Genera el XHTML referente al control
                        part = component.GetFragment(DomForm.SECTION_CONTROL_TEXT);
                        part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FIELD_CONTROL, ctrl);

                        break;
                     }
                  }

                  part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FIELD_DESCRIPTION, field.Description);
                  part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FIELD_LABEL, field.Label);
                  xhtml = string.Format("{0}{1}", xhtml, part);
               }

               // Pie del formulario
               xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomForm.SECTION_FIELDSET_FOOT));
            }

            // Botones del formulario
            part = component.GetFragment(DomForm.SECTION_BUTTONS);
            ctrl = string.Empty;
            foreach (DomFormButton button in _buttons)
            {
               switch (button.Type)
               {
                  case DomFormButton.FormControlTypes.Submit:
                     ctrl += string.Format("<input type=\"submit\" name=\"{0}\" value=\"{1}\" {2} />{3}", button.Name,
                                                                                                          button.Text,
                                                                                                          DomFormButton.GetCssClass(button.Active, component.GetAttribute(DomForm.TAG_ATTR_BUTTON_CLASS), component.GetAttribute(DomForm.TAG_ATTR_BUTTON_CLASS_ACTIVE)),
                                                                                                          component.GetAttribute(DomForm.TAG_ATTR_SEPARATOR));
                     break;

                  case DomFormButton.FormControlTypes.Reset:
                     ctrl += string.Format("<input type=\"reset\" name=\"{0}\" value=\"{1}\" {2} />{3}", button.Name,
                                                                                                         button.Text,
                                                                                                         DomFormButton.GetCssClass(button.Active, component.GetAttribute(DomForm.TAG_ATTR_BUTTON_CLASS), component.GetAttribute(DomForm.TAG_ATTR_BUTTON_CLASS_ACTIVE)),
                                                                                                         component.GetAttribute(DomForm.TAG_ATTR_SEPARATOR));
                     break;
               }
            }
            part = DomContentComponentBase.ReplaceTag(part, DomForm.TAG_FORM_BUTTONS, ctrl);
            xhtml = string.Format("{0}{1}", xhtml, part);

            // Pie del formulario
            xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomForm.SECTION_FOOT));

            // Reemplaza los TAGs comunes del elemento
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomForm.TAG_TEMPLATE_ID, template.ID.ToString());
            xhtml = DomContentComponentBase.ReplaceTag(xhtml, DomForm.TAG_HTML_ID, this.ID);

            // Agrega el código JavaScript de validación
            if (_validate)
               xhtml += GetValidationJS();

            return xhtml;
         }
         catch
         {
            throw;
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve un formulario guardado en una sesión.
      /// </summary>
      /// <param name="page">Una instancia de <see cref="DomPage"/> que representa la página que se está generando..</param>
      public static DomForm GetSubmitedForm(DomPage page)
      {
         DomForm form = null;

         // Comprueba que exista un formulario DOM enviado.
         if (page.Parameters == null) return null;
         if (!page.Parameters.ContainsKey(DomForm.PARAM_FORM_ID))
            return null;

         // Recupera el formulario
         // form = (DomForm)page.Session[GetFormCacheKey(page.Parameters. [DomForm.PARAM_FORM_ID])];
         
         if (form == null)
            return null;

         // Recupera los datos de los campos
         foreach (DomFormFieldset fieldset in form.Fieldsets)
         {
            foreach (DomFormField field in fieldset.Fields)
            {
               // field.Value = page.Parameters[field.Name];
            }
         }

         return form;
      }

      /// <summary>
      /// Elimina todos los formularios Cosmo de la sesión de usuario.
      /// </summary>
      /// <remarks>
      /// Este método se debe llamar siempre después de recibir un formulario para no ocupar memória de forma innecesária.
      /// </remarks>
      internal static void ClearCaheForms(DomPage page)
      {
         foreach (object cache in page.Session.Keys)
         {

         }
      }

      /// <summary>
      /// Devuelve una clave de caché correspondiente a un formulario.
      /// </summary>
      internal static string GetFormCacheKey(string formName)
      {
         return "cosmo.dom.form." + formName.Trim().ToLower();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Genera el código JavaScript que permite validar el formulario en el cliente.
      /// </summary>
      private string GetValidationJS()
      {
         bool first;
         StringBuilder js = new StringBuilder();

         try
         {
            js.AppendLine("<script type=\"text/javascript\">");
            js.AppendFormat("$(document).ready(function(){{\n");
            js.AppendFormat("  var validator = $(\"#{0}\").validate({{\n", this.ID);

            // Configuración de las reglas
            js.AppendFormat("    rules: {{\n");
            first = true;
            foreach (DomFormFieldset fieldset in this.Fieldsets)
            {
               foreach (DomFormField field in fieldset.Fields)
               {
                  if (field.ValidationRule is ValidationRuleBase)
                  {
                     if (!first) js.AppendFormat(",\n");
                     js.AppendFormat("{0}", ((ValidationRuleBase)field.ValidationRule).GetValiadtionRulesScript(field.Name));
                     first = false;
                  }
               }
            }
            js.AppendFormat("\n    }},\n");

            // Configuración de los mensajes de error
            js.AppendFormat("    messages: {{\n");
            first = true;
            foreach (DomFormFieldset fieldset in this.Fieldsets)
            {
               foreach (DomFormField field in fieldset.Fields)
               {
                  if (field.ValidationRule is ValidationRuleBase)
                  {
                     if (!first) js.AppendFormat(",\n");
                     js.AppendFormat("{0}", ((ValidationRuleBase)field.ValidationRule).GetValiadtionMessagesScript(field.Name));
                     first = false;
                  }
               }
            }
            js.AppendFormat("\n    }},\n");

            js.AppendFormat("    submitHandler: function() {{\n");
            js.AppendFormat("      alert(\"submitted!\");\n");
            js.AppendFormat("    }},\n");
            js.AppendFormat("    success: function(label) {{\n");
            js.AppendFormat("      label.html(\"&nbsp;\").addClass(\"checked\");\n");
            js.AppendFormat("    }}\n");

            js.AppendFormat("  }});\n");
            js.AppendFormat("}});\n");
            js.AppendLine("</script>\n");

            return js.ToString();
         }
         catch
         {
            // Si se produce un error no se activa la validación en cliente. 
            return string.Empty;
         }
      }

      #endregion

   }

}
