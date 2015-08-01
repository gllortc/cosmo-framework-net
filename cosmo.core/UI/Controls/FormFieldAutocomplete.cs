using Cosmo.Net;
using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un control Typeahead para campos de autocompletado.
   /// </summary>
   /// <remarks>
   /// https://github.com/twitter/typeahead.js
   /// </remarks>
   public class FormFieldAutocomplete : FormField
   {
      /// <summary>
      /// Toquen para indicar dónde se sitúa el texto de consulta en la URL de consulta del componente.
      /// </summary>
      public const string TOKEN_QUERY = "%QUERY";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldAutocomplete"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldAutocomplete(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el nombre del control.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets el texto que aparecerá en la etiqueta.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Gets or sets el texto de ayuda que aparecerá dentro del cuadro de texto.
      /// </summary>
      public string Placeholder { get; set; }

      /// <summary>
      /// Gets or sets la URL de consulta de datos.
      /// Para especificar dónde se sitúa el valor a consultar se debe usar la constante <c>TOKEN_QUERY</c>.
      /// </summary>
      public string SearchUrl { get; set; }

      /// <summary>
      /// Gets or sets el número mínimo de carácteres a partir de los cuales empieza a enviar peticiones.
      /// </summary>
      public int MinLength { get; set; }

      /// <summary>
      /// Gets or sets la lista de botones de comando asociados al componente.
      /// </summary>
      public List<ButtonControl> Buttons { get; set; }

      /// <summary>
      /// Indica si el campo es requerido.
      /// </summary>
      public bool Required { get; set; }

      /// <summary>
      /// Gets or sets el valor del campo.
      /// </summary>
      public override object Value { get; set; }

      /// <summary>
      /// Devuelve el tipo de campo implementado.
      /// </summary>
      public override FieldTypes FieldType
      {
         get { return FieldTypes.Standard; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Gets the field value from the request.
      /// </summary>
      public override bool LoadValueFromRequest()
      {
         try
         {
            this.Value = Url.GetString(ParentView.Workspace.Context.Request.Params, this.DomID);
         }
         catch
         {
            this.Value = string.Empty;
         }

         return Validate();
      }

      /// <summary>
      /// Validate the field value.
      /// </summary>
      /// <c>true</c> if the field value is valid or <c>false</c> if the value is not valid.
      public override bool Validate()
      {
         if (this.Required)
         {
            return !string.IsNullOrWhiteSpace(this.Value.ToString());
         }

         return true;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.DomID = string.Empty;
         this.Name = "autocomplete";
         this.Label = string.Empty;
         this.SearchUrl = string.Empty;
         this.Placeholder = string.Empty;
         this.MinLength = 4;
         this.Buttons = new List<ButtonControl>();
      }

      #endregion

      #region Disabled Code

      /*
      /// <summary>
      /// Convierte el componente en código XHTML.
      /// </summary>
      /// <returns>Una cadena que contiene el código XHTML necesario para representar el componente en un navegador.</returns>
      public string ToXhtml()
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<modal role=\"modal\">");
         xhtml.AppendLine("  <div class=\"modal-group\">");
         if (string.IsNullOrWhiteSpace(_label))
         {
            xhtml.AppendLine("    <label for=\"" + GetVisibleId() + "\">" + HttpUtility.HtmlDecode(_label) + "</label>");
         }

         if (_buttons.Count > 0) xhtml.AppendLine("    <div class=\"input-group\">");
         xhtml.AppendLine("      <input id=\"" + GetVisibleId() + "\" type=\"text\" class=\"modal-control typeahead\" placeholder=\"" + HttpUtility.HtmlDecode(_placeholder) + "\">");
         xhtml.AppendLine("      <input type=\"hidden\" id=\"" + this.DomID + "\" name=\"" + this.DomID + "\" value=\"\" \">");
         if (_buttons.Count > 0) xhtml.AppendLine("      <span class=\"input-group-btn\">");
         foreach (Button btn in _buttons)
         {
            // xhtml.AppendLine(btn.ToXhtml());
         }
         if (_buttons.Count > 0) xhtml.AppendLine("      </span>");
         if (_buttons.Count > 0) xhtml.AppendLine("    </div>");
         
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</modal>");

         xhtml.AppendLine("<script type=\"text/javascript\">");
         xhtml.AppendLine("jQuery(function($) {");
         xhtml.AppendLine("  var usrs = new Bloodhound({");
         xhtml.AppendLine("    name: 'usersData',");
         xhtml.AppendLine("    remote: '" + _searchUrl + "',");
         xhtml.AppendLine("    datumTokenizer: function(d) {");
         xhtml.AppendLine("      return Bloodhound.tokenizers.obj.whitespace(d.Value);");
         xhtml.AppendLine("    },");
         xhtml.AppendLine("    queryTokenizer: Bloodhound.tokenizers.whitespace,");
         xhtml.AppendLine("  });");
         xhtml.AppendLine("  usrs.initialize();");
         xhtml.AppendLine("  $('#" + GetVisibleId() + "').typeahead(null, {");
         xhtml.AppendLine("    name: '" + _name + "',");
         xhtml.AppendLine("    displayKey: 'Label',");
         xhtml.AppendLine("    minLength: " + _minLength + ",");
         xhtml.AppendLine("    source: usrs.ttAdapter()");
         xhtml.AppendLine("  }).on('typeahead:selected', function (object, datum) {");
         xhtml.AppendLine("    console.log(datum);");
         xhtml.AppendLine("    $('#" + this.DomID + "').val(datum.Value);");
         xhtml.AppendLine("  });");

         xhtml.AppendLine("});");
         xhtml.AppendLine("</script>");

         return xhtml.ToString();
      }
      */

      #endregion

   }
}
