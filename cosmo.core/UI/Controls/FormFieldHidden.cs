using Cosmo.Net;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo oculto.
   /// </summary>
   public class FormFieldHidden : FormField
   {
      // Declara variables internas
      private string _value;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del campo en el formulario.</param>
      public FormFieldHidden(ViewContainer parentViewport, string domId)
         : base(parentViewport, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldText"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del campo en el formulario.</param>
      /// <param name="value">Valor inicial del campo.</param>
      public FormFieldHidden(ViewContainer parentViewport, string domId, string value)
         : base(parentViewport, domId)
      {
         Initialize();

         this.Value = value;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el tipo de campo implementado.
      /// </summary>
      public override FieldTypes FieldType
      {
         get { return FieldTypes.Standard; }
      }

      /// <summary>
      /// Devuelve o establece el valor del campo.
      /// </summary>
      public override object Value 
      {
         get { return _value; }
         set { _value = value.ToString(); }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Valida el valor del campo.
      /// </summary>
      /// <returns><c>true</c> si el valor és aceptable o <c>false</c> si el valor no es válido.</returns>
      public override bool Validate()
      {
         return true;
      }

      /// <summary>
      /// Obtiene el valor del campo a partir de los datos recibidos mediante GET o POST.
      /// </summary>
      /// <returns><c>true</c> si el valor obtenido es válido o <c>false</c> si el valor no puede ser aceptado.</returns>
      public override bool LoadValueFromRequest()
      {
         try
         {
            _value = Url.GetString(Container.Workspace.Context.Request.Params, this.DomID);
            return true;
         }
         catch
         {
            _value = string.Empty;
            return false;
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _value = string.Empty;
      }

      #endregion

   }
}
