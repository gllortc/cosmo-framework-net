namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Representa un elemento de la barra de navegación específico para el inicio de sesión y el acceso a la cuenta de los usuarios.
   /// </summary>
   public class NavbarLoginItem : NavbarIButtonControl
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="NavbarLoginItem"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public NavbarLoginItem(View parentView) //, Workspace workspace) 
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="NavbarLoginItem"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="modalFormId">El ID (DOM) del formulario modal de login definido en el componente <see cref="LoginFormControl"/>.</param>
      public NavbarLoginItem(View parentView, string modalFormId)
         : base(parentView)
      {
         Initialize();

         this.ModalFormId = modalFormId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el ID (DOM) del formulario modal de login definido en el componente <see cref="LoginFormControl"/>. 
      /// Si se deja en blanco (valor por defecto) el enlace de este elemento redireccionará al usuario
      /// a la página de <em>login</em>. Si se define, invocará al formulario modal con el ID especificado.
      /// </summary>
      public string ModalFormId { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.ModalFormId = string.Empty;
      }

      #endregion

   }
}
