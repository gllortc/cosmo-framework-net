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
      /// Devuelve una instancia de <see cref="FormFieldAutocomplete"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador único del componente dentro de la vista (DOM).</param>
      public FormFieldAutocomplete(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el nombre del control.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Devuelve o establece el texto que aparecerá en la etiqueta.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Devuelve o establece el texto de ayuda que aparecerá dentro del cuadro de texto.
      /// </summary>
      public string Placeholder { get; set; }

      /// <summary>
      /// Devuelve o establece la URL de consulta de datos.
      /// Para especificar dónde se sitúa el valor a consultar se debe usar la constante <c>TOKEN_QUERY</c>.
      /// </summary>
      public string SearchUrl { get; set; }

      /// <summary>
      /// Devuelve o establece el número mínimo de carácteres a partir de los cuales empieza a enviar peticiones.
      /// </summary>
      public int MinLength { get; set; }

      /// <summary>
      /// Devuelve o establece la lista de botones de comando asociados al componente.
      /// </summary>
      public List<ButtonControl> Buttons { get; set; }

      /// <summary>
      /// Indica si el campo es requerido.
      /// </summary>
      public bool Required { get; set; }

      /// <summary>
      /// Devuelve o establece el valor del campo.
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
      /// Obtiene el valor del campo a partir de los datos recibidos mediante GET o POST.
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
      /// Valida el valor del campo.
      /// </summary>
      /// <returns><c>true</c> si el valor és aceptable o <c>false</c> si el valor no es válido.</returns>
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
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.DomID = "usrSearch";
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
