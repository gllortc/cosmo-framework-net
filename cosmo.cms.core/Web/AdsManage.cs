using Cosmo.Cms.Model.Ads;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Cms.Web
{
   /// <summary>
   /// ANUNCIOS CLASIFICADOS.
   /// Muestra el contenido de una categoria.
   /// </summary>
   [AuthenticationRequired]
   public class AdsManage : PageView
   {

      #region PageView Implementation

      public override void LoadPage()
      {
         Title = AdsDAO.SERVICE_NAME;

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = AdsDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_SHOPPING_CART;
         MainContent.Add(header);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Agrega la meta-información de la página
         Title = AdsDAO.SERVICE_NAME + " - Mia anuncios";

         //--------------------------------------------------------------
         // Genera la lista de anuncios a mostrar
         //--------------------------------------------------------------

         AdsManageListPartialView adsList = new AdsManageListPartialView();
         PartialViewContainerControl adsListView = new PartialViewContainerControl(this, adsList);
         MainContent.Add(adsListView);

         Scripts.Add(adsList.GetInvokeScriptWithParameters(Script.ScriptExecutionMethod.OnDocumentReady));
      }

      public override void InitPage()
      {
         // Nothing to do
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Gets an URL to manage current user ads.
      /// </summary>
      /// <returns>A string representing the requested URL.</returns>
      public static string GetURL()
      {
         return MethodBase.GetCurrentMethod().DeclaringType.Name;
      }

      #endregion

   }
}