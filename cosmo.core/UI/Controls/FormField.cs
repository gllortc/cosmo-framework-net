using Cosmo.Utils;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Interface que deben implementar todos los campos de formulario que pueden contener un valor.
   /// </summary>
   public abstract class FormField : Control
   {

      #region Enumerations

      /// <summary>
      /// Enumera los tipos de campo que se pueden implementar.
      /// </summary>
      public enum FieldTypes
      {
         /// <summary>Campos que recogen un valor del formulario (texto, números, fechas, etc.).</summary>
         Standard,
         /// <summary>Campos de archivo que implican subida de datos físicos.</summary>
         Upload,
         /// <summary>Campos de comprobación (capchas, etc.)</summary>
         Check
      }

      #endregion

      /// <summary>Cadena que debe concatenarse al final del DomID del campo original para obtener el campo de verificación.</summary>
      public const string FIELD_CHECK_POST_DOMID = "re";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormField"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      protected FormField(View parentView) : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormField"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      protected FormField(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el tipo de campo que implementa.
      /// </summary>
      public abstract FieldTypes FieldType { get; }

      /// <summary>
      /// Gets or sets el valor del campo de formulario.
      /// </summary>
      public abstract object Value { get; set; }

      /// <summary>
      /// Indica si el contenido del campo contiene errores.
      /// </summary>
      public bool IsValidContent { get; set; }

      /// <summary>
      /// Gets or sets el mensaje a mostrar cuando la validación del contenido del campo ha fallado.
      /// </summary>
      public string InvalidContentMessage { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Gets the field value from the request.
      /// </summary>
      /// <returns><c>true</c> si el valor obtenido es válido o <c>false</c> si el valor no puede ser aceptado.</returns>
      public abstract bool LoadValueFromRequest();

      /// <summary>
      /// Validate the field value.
      /// </summary>
      /// <c>true</c> if the field value is valid or <c>false</c> if the value is not valid.
      public abstract bool Validate();

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.IsValidContent = false;
         this.InvalidContentMessage = string.Empty;
      }

      #endregion

   }
}
