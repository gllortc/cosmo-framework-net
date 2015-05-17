namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa un campo de formulario del modelo de documentos DOM.
   /// </summary>
   public class DomFormButton
   {

      #region Enumerations

      /// <summary>
      /// Etiquetas que se pueden mostrar en los controles 
      /// </summary>
      public enum FormControlActions : int
      {
         /// <summary>Se muestra una imagen personalizada para el botón.</summary>
         CustomImage = 0,
         /// <summary>Se muestra la etiqueta Aceptar.</summary>
         Accept = 1,
         /// <summary>Se muestra la etiqueta Cancelar.</summary>
         Cancel = 2,
         /// <summary>Se muestra la etiqueta Volver.</summary>
         Return = 4,
         /// <summary>Se muestra la etiqueta Enviar.</summary>
         Send = 8,
         /// <summary>Se muestra la etiqueta Imprimir.</summary>
         Print = 16,
         /// <summary>Se muestra la etiqueta Limpiar.</summary>
         Reset = 32,
         /// <summary>Se muestra la etiqueta Descargar.</summary>
         Download = 64,
         /// <summary>Se muestra la etiqueta Eliminar.</summary>
         Delete = 128,
         /// <summary>Se muestra la etiqueta Ir.</summary>
         GoTo = 256,
         /// <summary>Se muestra la etiqueta Ver.</summary>
         View = 512,
         /// <summary>Se muestra la etiqueta Menu.</summary>
         Menu = 1024
      }

      /// <summary>
      /// Tipos de control
      /// </summary>
      public enum FormControlTypes : int
      {
         /// <summary>Botón de enlace</summary>
         Link = 1,
         /// <summary>Botón de envío de formulario</summary>
         Submit = 2,
         /// <summary>Botón de limpiar campios del formulario</summary>
         Reset = 3
      }

      #endregion

      bool _active;
      string _name;
      string _text;
      FormControlTypes _type;
      string _href;
      FormControlActions _action;
      string _image;

      /// <summary>
      /// Devuelve una instancia de la clase <see cref="DomFormButton"/>.
      /// </summary>
      public DomFormButton()
      {
         _active = false;
         _name = "";
         _text = string.Empty;
         _type = FormControlTypes.Submit;
         _action = FormControlActions.Accept;
         _href = "";
         _image = "";
      }

      /// <summary>
      /// Devuelve una instancia de la clase <see cref="DomFormButton"/>.
      /// </summary>
      /// <param name="type">Tipo de control.</param>
      /// <param name="name">Nombre del control.</param>
      public DomFormButton(FormControlTypes type, string name)
      {
         _active = false;
         _name = name;
         _type = type;
         _text = string.Empty;
         _action = FormControlActions.View;
         _href = string.Empty;
         _image = string.Empty;
      }

      /// <summary>
      /// Devuelve una instancia de la clase <see cref="DomFormButton"/>.
      /// </summary>
      /// <param name="type">Tipo de control.</param>
      /// <param name="name">Nombre del control.</param>
      /// <param name="text">Texto visible del control.</param>
      public DomFormButton(FormControlTypes type, string name, string text)
      {
         _active = false;
         _name = name;
         _text = text;
         _type = type;
         _action = FormControlActions.View;
         _href = string.Empty;
         _image = string.Empty;
      }

      /// <summary>
      /// Devuelve una instancia de la clase <see cref="DomFormButton"/>.
      /// </summary>
      /// <param name="type">Tipo de control.</param>
      /// <param name="name">Nombre del control.</param>
      /// <param name="text">Texto visible del control.</param>
      /// <param name="active">Indica si se debe resaltar el botón.</param>
      public DomFormButton(FormControlTypes type, string name, string text, bool active)
      {
         _active = active;
         _name = name;
         _text = text;
         _type = type;
         _action = FormControlActions.View;
         _href = string.Empty;
         _image = string.Empty;
      }

      #region Properties

      /// <summary>
      /// Indica si se debe resaltar el botón.
      /// </summary>
      public bool Active
      {
         get { return _active; }
         set { _active = value; }
      }

      /// <summary>
      /// Nombre del control.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Texto visible del control.
      /// </summary>
      public string Text
      {
         get { return _text; }
         set { _text = value; }
      }

      /// <summary>
      /// Tipo de control.
      /// </summary>
      public FormControlTypes Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Enlace (Href) para los controles de tipo enlace.
      /// </summary>
      public string Href
      {
         get { return _href; }
         set { _href = value; }
      }

      /// <summary>
      /// Acción que mostrará la etiqueta del control.
      /// </summary>
      public FormControlActions Action
      {
         get { return _action; }
         set { _action = value; }
      }

      /// <summary>
      /// En caso de ser una imagen personalizada, contiene la Url de acceso a la imagen usada para mostrar el control
      /// </summary>
      public string CustomImageUrl
      {
         get { return _image; }
         set { _image = value; }
      }

      #endregion

      #region Static Members

      internal static string GetCssClass(bool active, string css, string cssActive)
      {
         if (active)
         {
            if (string.IsNullOrEmpty(cssActive)) return string.Empty;
            return "class=\"" + cssActive + "\"";
         }
         else
         {
            if (string.IsNullOrEmpty(css)) return string.Empty;
            return "class=\"" + css + "\"";
         }
      }

      #endregion

   }

}
