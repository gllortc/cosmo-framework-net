using System;
using System.Web.UI;

namespace Cosmo.UI.Aspx
{
   /// <summary>
   /// Extiende la clase <see cref="System.Web.UI.MasterPage"/> para agregar las funcionalidades de Cosmo.
   /// </summary>
   public class CosmoMasterPage : System.Web.UI.MasterPage
   {
      // Declaracción de variables internas
      private string _menuId;
      private Workspace _ws;

      /// <summary>
      /// Devuelve una instancia de <see cref="CosmoMasterPage"/>.
      /// </summary>
      public CosmoMasterPage()
      {
         Initialize();
      }

      /// <summary>
      /// Evento que se invoca antes de iniciar el renderizado ASP.
      /// </summary>
      protected void Page_Init(object sender, EventArgs e)
      {
         InitializePage();

         // Obtiene el menú activo
         string menuId = Request.Params[Cosmo.Workspace.PARAM_MENU_ACTIVE];
         if (menuId != null)
         {
            Session.Add("Cosmo.UI.Menu.ActiveID", menuId);
         }
      }

      /// <summary>
      /// Devuelve el workspace actual.
      /// </summary>
      public Workspace Workspace
      {
         get { return _ws; }
      }

      /// <summary>
      /// Devuelve o establece el identificador del elemento de menú activo.
      /// </summary>
      public string ActiveMenuId
      {
         get { return _menuId; }
         set { _menuId = value; }
      }

      #region Private Methods

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         // Inicializa variables
         _ws = null;
         _menuId = string.Empty;
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void InitializePage()
      {
         // Inicializa variables
         _ws = Workspace.GetWorkspace(Context);
      }

      #endregion

   }
}
