namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Representa un elemento de la barra de navegación específico para el inicio de sesión y el acceso a la cuenta de los usuarios.
   /// </summary>
   public class NavbarLoginItem : NavbarIButtonControl
   {
      // Declaración de variables internas.
      private Workspace _ws;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="NavbarLoginItem"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      public NavbarLoginItem(ViewContainer container, Workspace workspace) 
         : base(container)
      {
         Initialize();

         _ws = workspace;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="NavbarLoginItem"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      /// <param name="modalFormId">El ID (DOM) del formulario modal de login definido en el componente <see cref="LoginFormControl"/>.</param>
      public NavbarLoginItem(ViewContainer container, Workspace workspace, string modalFormId)
         : base(container)
      {
         Initialize();

         _ws = workspace;
         this.ModalFormId = modalFormId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el ID (DOM) del formulario modal de login definido en el componente <see cref="LoginFormControl"/>. 
      /// Si se deja en blanco (valor por defecto) el enlace de este elemento redireccionará al usuario
      /// a la página de <em>login</em>. Si se define, invocará al formulario modal con el ID especificado.
      /// </summary>
      public string ModalFormId { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.ModalFormId = string.Empty;
         _ws = null;
      }

      #endregion

   }
}
