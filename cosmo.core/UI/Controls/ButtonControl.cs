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
         /// <summary>Send form data. In a form send data using JavaScript/jQuery.</summary>
         SubmitJS,
         /// <summary>Open Modal. Abre un formulario modal habilitado en la página.</summary>
         OpenModalForm,
         /// <summary>Close Modal. Cierra el formulario modal dónde se encuentra el botón.</summary>
         CloseModalForm,
         /// <summary>Open Modal view. Abre un formulario modal habilitado en la página.</summary>
         OpenModalView
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public ButtonControl(View parentView) 
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="type">Tipo de botón a representar.</param>
      public ButtonControl(View parentView, string domId, string caption, ButtonTypes type)
         : base(parentView, domId)
      {
         Initialize();

         Text = caption;
         Type = type;
      }

      /// <summary>
      /// Gets a new instance of <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="icon">Código del icono a mostrar.</param>
      /// <param name="type">Tipo de botón a representar.</param>
      public ButtonControl(View parentView, string domId, string caption, string icon, ButtonTypes type)
         : base(parentView, domId)
      {
         Initialize();

         Text = caption;
         Icon = icon;
         Type = type;
      }

      /// <summary>
      /// Gets a new instance of <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="modal">Formulario modal que abrirá el botón.</param>
      public ButtonControl(View parentView, string domId, string caption, ModalView modal)
         : base(parentView, domId)
      {
         Initialize();

         Text = caption;
         ModalDomId = modal.DomID;
         Type = ButtonTypes.OpenModalView;
         JavaScriptAction = modal.GetInvokeCall();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="modal">Formulario modal que abrirá el botón.</param>
      public ButtonControl(View parentView, string domId, string caption, string icon, ModalView modal)
         : base(parentView, domId)
      {
         Initialize();

         Text = caption;
         Icon = icon;
         ModalDomId = modal.DomID;
         Type = ButtonTypes.OpenModalView;
         JavaScriptAction = modal.GetInvokeCall();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="href">Enlace dónde se redirige al hacer clic.</param>
      /// <param name="jsAction">Acción JS que se ejecutará al hacer clic.</param>
      public ButtonControl(View parentView, string domId, string caption, string href, string jsAction)
         : base(parentView, domId)
      {
         Initialize();

         Text = caption;
         Type = ButtonTypes.Normal;
         Href = href;
         JavaScriptAction = jsAction;
      }

      /// <summary>
      /// Gets a new instance of <see cref="ButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="caption">Texto visible para el botón.</param>
      /// <param name="icon">Código del icono a mostrar.</param>
      /// <param name="href">Enlace dónde se redirige al hacer clic.</param>
      /// <param name="jsAction">Acción JS que se ejecutará al hacer clic.</param>
      public ButtonControl(View parentView, string domId, string caption, string icon, string href, string jsAction)
         : base(parentView, domId)
      {
         Initialize();

         Text = caption;
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
      /// Gets or sets el texto visible que mostrará el componente.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or sets el código del icono que se mostrará en el componente.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Gets or sets la llamada a una acción JavaScript.
      /// </summary>
      /// <example>
      /// <c>...onclick="javascript:<b>navigateToUrl();</b>"...</c>
      /// </example>
      public string JavaScriptAction { get; set; }

      /// <summary>
      /// Gets or sets la URL a la que se invocará si se hace click..
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Gets or sets el identificador del formulario modal que debe abrir el botón.
      /// </summary>
      public string ModalDomId { get; set; }

      /// <summary>
      /// Gets or sets el color que se aplicará al componente.
      /// </summary>
      public ComponentColorScheme Color { get; set; }

      /// <summary>
      /// Gets or sets el tamaño que debe adoptar el componente.
      /// </summary>
      public ButtonSizes Size { get; set; }

      /// <summary>
      /// Gets or sets el tipo de botón.
      /// </summary>
      public ButtonTypes Type { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         Enabled = true;
         IsBlock = false;
         Text = string.Empty;
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
