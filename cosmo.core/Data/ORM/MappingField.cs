namespace Cosmo.Data.ORM
{
   /// <summary>
   /// Determina el tipo de dato de un campo de mapeo.
   /// </summary>
   public enum MappingDataType
   {
      /// <summary>String. Cadena de texto simple.</summary>
      String,
      /// <summary>String. Cadena de texto simple con múltiples líneas.</summary>
      MultilineString,
      /// <summary>Número entero.</summary>
      Integer,
      /// <summary>Número decimal.</summary>
      Decimal,
      /// <summary>Correo electrónico.</summary>
      Mail,
      /// <summary>Nombre de usuario.</summary>
      Login,
      /// <summary>Fecha.</summary>
      Date,
      /// <summary>Booleano (Sí/No).</summary>
      Boolean,
      /// <summary>Dirección URL.</summary>
      Url,
      /// <summary>Contraseña.</summary>
      Password,
      /// <summary>Campo oculto (no editable).</summary>
      Hidden
   }

   /// <summary>
   /// Implementa un mapeo de campo de formulario ORM.
   /// </summary>
   [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field, AllowMultiple = false)]
   public class MappingField : System.Attribute
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="MappingField"/>.
      /// </summary>
      public MappingField()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si el campo es índice de la tabla.
      /// </summary>
      public bool IsPrimaryKey { get; set; }

      /// <summary>
      /// Indica el nombre del campo (propiedad <em>name</em> del DOM).
      /// </summary>
      public string FieldName { get; set; }

      /// <summary>
      /// Gets or sets la etiqueta visible que aparecerá junto al campo.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Gets or sets el identificador del grupo de controles al que debe agregarse el campo.
      /// </summary>
      /// <remarks>
      /// Si no se establece, el control será representado al inicio del formulario fuera de los 
      /// grupos de controles.
      /// </remarks>
      public string GroupID { get; set; }

      /// <summary>
      /// Gets or sets el tipo de datos que contiene el campo.
      /// </summary>
      public MappingDataType DataType  { get; set; }

      /// <summary>
      /// Gets or sets el identificador de la lista que rellena la lista de posibles 
      /// valores del control.
      /// </summary>
      /// <remarks>
      /// Sólo aplicable a campos de tipo lista.
      /// </remarks>
      public string DataListID { get; set; }

      /// <summary>
      /// Indica si un campo es obligatorio.
      /// </summary>
      public bool Required { get; set; }

      /// <summary>
      /// Indica si el contenido del campo se pedirá dos veces para evitar errores de introducción.
      /// </summary>
      /// <remarks>
      /// Sólo aplicable en campos de tipo eMail o password.
      /// </remarks>
      public bool RewriteRequired { get; set; }

      /// <summary>
      /// Indica que el campo se establece manualmente justo antes de realizar una acción y no debe
      /// entrar en las validaciones ni aparecer como campo oculto.
      /// </summary>
      public bool ManualSet { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.IsPrimaryKey = false;
         this.FieldName = string.Empty;
         this.Label = string.Empty;
         this.Required = false;
         this.ManualSet = false;
         this.RewriteRequired = false;
         this.DataListID = string.Empty;
         this.DataType = MappingDataType.String;
         this.GroupID = string.Empty;
      }

      #endregion

   }
}
