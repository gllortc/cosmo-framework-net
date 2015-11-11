using Cosmo.UI.Controls;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Implements a script that allow a control acts as a autocomplete control.
   /// </summary>
   public class AutocompleteFormFieldScript : Script
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="AutocompleteFormFieldScript"/>.
      /// </summary>
      /// <param name="formField">The field which acts as a autocomplete control.</param>
      public AutocompleteFormFieldScript(FormFieldAutocomplete formField)
         : base(formField.ParentView) 
      {
         Initialize();

         this.FormField = formField;
         this.ExecutionType = ScriptExecutionMethod.Standalone;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the control which acts as a autocomplete.
      /// </summary>
      public FormFieldAutocomplete FormField { get; set; }

      /// <summary>
      /// Gets or sets the string that indicates where is located the query text in URL.
      /// </summary>
      /// <remarks>
      /// Example: The url for query will be: '../data/films/queries/%QUERY'
      /// </remarks>
      public string QueryWildcard { get; set; }

      #endregion

      #region Script Implementation

      public override void BuildSource()
      {
         string bhName = "bh" + this.FormField.DomID;
         string tahName = "tah" + this.FormField.DomID;

         //Source.AppendLine(@"var " + bhName + " = new Bloodhound({");
         //Source.AppendLine(@"  datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),");
         //Source.AppendLine(@"  queryTokenizer: Bloodhound.tokenizers.whitespace,");
         //Source.AppendLine(@"  identify: function(obj) { return obj.Data.Label; },");
         //Source.AppendLine(@"  remote: {");
         //Source.AppendLine(@"    url: '" + Web.Handlers.SecurityRestHandler.GetUserSearchUrl(this.QueryWildcard) + "',");
         //Source.AppendLine(@"    filter: function (parsedResponse) {");
         //Source.AppendLine(@"              console.log(parsedResponse);");
         //Source.AppendLine(@"              return parsedResponse;");
         //Source.AppendLine(@"    },");
         //Source.AppendLine(@"    wildcard: '" + this.QueryWildcard + "'");
         //Source.AppendLine(@"  }");
         //Source.AppendLine(@"});");

         Source.AppendLine(@"$('#" + this.FormField.DomID + "').typeahead(null, {");
         Source.AppendLine(@"  name: '" + tahName + "',");
         Source.AppendLine(@"  minLength: 4,");
         Source.AppendLine(@"  display: 'Data.Label',");
         // Source.AppendLine(@"  source: " + bhName);
         Source.AppendLine(@"  remote: {");
         Source.AppendLine(@"    url: '" + Web.Handlers.SecurityRestHandler.GetUserSearchUrl(this.QueryWildcard) + "',");
         Source.AppendLine(@"    filter: function (parsedResponse) {");
         Source.AppendLine(@"              var resultList = data.aaData.map(function (item) {");
         Source.AppendLine(@"                return item.name;");
         Source.AppendLine(@"              });");
         Source.AppendLine(@"            }");
         Source.AppendLine(@"  }");
         Source.AppendLine(@"});");
      }

      ///// <summary>
      ///// Make the JavaScript source code of script.
      ///// </summary>
      ///// <returns>A string containing the requestes source code of script.</returns>
      //public override string GetSource()
      //{
      //   string bhName = "bh" + this.FormField.DomID;
      //   string tahName = "tah" + this.FormField.DomID;

      //   //Source.AppendLine(@"var " + bhName + " = new Bloodhound({");
      //   //Source.AppendLine(@"  datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),");
      //   //Source.AppendLine(@"  queryTokenizer: Bloodhound.tokenizers.whitespace,");
      //   //Source.AppendLine(@"  identify: function(obj) { return obj.Data.Label; },");
      //   //Source.AppendLine(@"  remote: {");
      //   //Source.AppendLine(@"    url: '" + Web.Handlers.SecurityRestHandler.GetUserSearchUrl(this.QueryWildcard) + "',");
      //   //Source.AppendLine(@"    filter: function (parsedResponse) {");
      //   //Source.AppendLine(@"              console.log(parsedResponse);");
      //   //Source.AppendLine(@"              return parsedResponse;");
      //   //Source.AppendLine(@"    },");
      //   //Source.AppendLine(@"    wildcard: '" + this.QueryWildcard + "'");
      //   //Source.AppendLine(@"  }");
      //   //Source.AppendLine(@"});");

      //   Source.AppendLine(@"$('#" + this.FormField.DomID + "').typeahead(null, {");
      //   Source.AppendLine(@"  name: '" + tahName + "',");
      //   Source.AppendLine(@"  minLength: 4,");
      //   Source.AppendLine(@"  display: 'Data.Label',");
      //   // Source.AppendLine(@"  source: " + bhName);
      //   Source.AppendLine(@"  remote: {");
      //   Source.AppendLine(@"    url: '" + Web.Handlers.SecurityRestHandler.GetUserSearchUrl(this.QueryWildcard) + "',");
      //   Source.AppendLine(@"    filter: function (parsedResponse) {");
      //   Source.AppendLine(@"              var resultList = data.aaData.map(function (item) {");
      //   Source.AppendLine(@"                return item.name;");
      //   Source.AppendLine(@"              });");
      //   Source.AppendLine(@"            }");
      //   Source.AppendLine(@"  }");
      //   Source.AppendLine(@"});");

      //   return Source.ToString();
      //}

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.FormField = null;
         this.QueryWildcard = FormFieldAutocomplete.TOKEN_QUERY;
      }

      #endregion

   }
}
