using Cosmo.UI.Modals;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un botón de comando.
   /// </summary>
   public class ButtonControl : Control
   {

      #region Enumerations

      /// <summary>
      /// Enumera los distintos tamaños de botón.
      /// </summary>
      public enum ButtonSizes
      {
         /// <summary>Grande</summary>
         Large,
         /// <summary>Normal</summary>
         Default,
         /// <summary>Pequeño</summary>
         Small,
         /// <summary>Miniatura</summary>
         ExtraSmall
      }

      /// <summary>
      /// Enumera los distintos tipos de botón.
      /// </summary>
      public enum ButtonTypes
      {
         /// <summary>Normal. Se controla el botón mediante eventos de JavaScript.</summary>
         Normal,
         /// <summary>Envío. Situado en un formulario envia los datos del mismo.</summary>
         Submit,
         /// <summary>Open Modal. Abre un formulario modal habilitado en la página.</summary>
         OpenModalForm,
         /// <summary>Close Modal. Cierra el formulario modal dónde se encuentra el botón.</summary>
         CloseModalForm
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public ButtonControl(ViewContainer container) 
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador del control en la página (DOM ID).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="type">Tipo de botón a representar.</param>
      public ButtonControl(ViewContainer container, string domId, string caption, ButtonTypes type)
         : base(container, domId)
      {
         Initialize();

         Caption = caption;
         Type = type;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador del control en la página (DOM ID).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="icon">Código del icono a mostrar.</param>
      /// <param name="type">Tipo de botón a representar.</param>
      public ButtonControl(ViewContainer container, string domId, string caption, string icon, ButtonTypes type)
         : base(container, domId)
      {
         Initialize();

         Caption = caption;
         Icon = icon;
         Type = type;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador del control en la página (DOM ID).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="modal">Formulario modal que abrirá el botón.</param>
      public ButtonControl(ViewContainer container, string domId, string caption, IModalForm modal)
         : base(container, domId)
      {
         Initialize();

         Caption = caption;
         ModalDomId = modal.DomID;
         Type = ButtonTypes.OpenModalForm;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador del control en la página (DOM ID).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="icon">Código del icono a mostrar.</param>
      /// <param name="modal">Formulario modal que abrirá el botón.</param>
      public ButtonControl(ViewContainer container, string domId, string caption, string icon, IModalForm modal)
         : base(container, domId)
      {
         Initialize();

         Caption = caption;
         Icon = icon;
         ModalDomId = modal.DomID;
         Type = ButtonTypes.OpenModalForm;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador del control en la página (DOM ID).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="href">Enlace dónde se redirige al hacer clic.</param>
      /// <param name="jsAction">Acción JS que se ejecutará al hacer clic.</param>
      public ButtonControl(ViewContainer container, string domId, string caption, string href, string jsAction)
         : base(container, domId)
      {
         Initialize();

         Caption = caption;
         Type = ButtonTypes.Normal;
         Href = href;
         JavaScriptAction = jsAction;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador del control en la página (DOM ID).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="icon">Código del icono a mostrar.</param>
      /// <param name="href">Enlace dónde se redirige al hacer clic.</param>
      /// <param name="jsAction">Acción JS que se ejecutará al hacer clic.</param>
      public ButtonControl(ViewContainer container, string domId, string caption, string icon, string href, string jsAction)
         : base(container, domId)
      {
         Initialize();

         Caption = caption;
         Icon = icon;
         Type = ButtonTypes.Normal;
         Href = href;
         JavaScriptAction = jsAction;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si el botó está habilitado (<c>true</c>) o deshabilitado (<c>false</c>).
      /// </summary>
      public bool Enabled { get; set; }

      /// <summary>
      /// Indica si es un botón <em>block</em> (si ocupa todo el ancho disponible en el contenedor).
      /// </summary>
      public bool IsBlock { get; set; }

      /// <summary>
      /// Devuelve o establece el texto visible que mostrará el componente.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Devuelve o establece el código del icono que se mostrará en el componente.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Devuelve o establece la llamada a una acción JavaScript.
      /// </summary>
      /// <example>
      /// <c>...onclick="javascript:<b>navigateToUrl();</b>"...</c>
      /// </example>
      public string JavaScriptAction { get; set; }

      /// <summary>
      /// Devuelve o establece la URL a la que se invocará si se hace click..
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Devuelve o establece el identificador del formulario modal que debe abrir el botón.
      /// </summary>
      public string ModalDomId { get; set; }

      /// <summary>
      /// Devuelve o establece el color que se aplicará al componente.
      /// </summary>
      public ComponentColorScheme Color { get; set; }

      /// <summary>
      /// Devuelve o establece el tamaño que debe adoptar el componente.
      /// </summary>
      public ButtonSizes Size { get; set; }

      /// <summary>
      /// Devuelve o establece el tipo de botón.
      /// </summary>
      public ButtonTypes Type { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         Enabled = true;
         IsBlock = false;
         Caption = string.Empty;
         Icon = string.Empty;
         JavaScriptAction = string.Empty;
         Href = string.Empty;
         ModalDomId = string.Empty;
         Size = ButtonSizes.Default;
         Type = ButtonTypes.Normal;
         Color = ComponentColorScheme.Normal;
      }

      #endregion

   }
}
