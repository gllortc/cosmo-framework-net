using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using System.Collections.Generic;

namespace Cosmo.UI.Modals
{
   /// <summary>
   /// Implementa una clase abstracta que deben implementar todas las ventanas modales.
   /// </summary>
   public abstract class IModalAjaxForm : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="IModalForm"/>.
      /// </summary>
      /// <param name="container"></param>
      /// <param name="domId"></param>
      protected IModalAjaxForm(ViewContainer container, string domId) 
         : base(container, domId)
      {
         Initialize(domId);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el título del formulario.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Devuelve o establece el código del icono que se mostrará junto al título.
      /// </summary>
      public string TitleIcon { get; set; }

      /// <summary>
      /// Indica si el formulario modal debe contener un botón para cerrar el diálogo.
      /// </summary>
      public bool Closeable { get; set; }

      /// <summary>
      /// Devuelve o establece la lista de controles que contiene la zona de contenidos de la página.
      /// </summary>
      public FormControl Form { get; set; }

      /// <summary>
      /// Devuelve una lista de los scripts <see cref="Script"/> que usará el modal para su representación.
      /// </summary>
      public List<Script> Scripts { get; set; }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Procesa el formulario y prepara todos los componentes para el renderizado. 
      /// </summary>
      public abstract void PreRenderForm();

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize(string domId)
      {
         // Inicializa variables
         Title = string.Empty;
         TitleIcon = string.Empty;
         Closeable = false;
         Form = new FormControl(Container, domId);
         Scripts = new List<Script>();
      }

      #endregion

   }
}
