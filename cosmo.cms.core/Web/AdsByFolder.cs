using Cosmo.Cms.Model.Ads;
using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Cms.Web
{
   /// <summary>
   /// ANUNCIOS CLASIFICADOS.
   /// Muestra el contenido de una categoria.
   /// </summary>
   public class AdsByFolder : PageView
   {

      #region PageView Implementation

      public override void LoadPage()
      {
         // Declaraciones
         AdsDAO ads = null;
         AdsSection folder = null;

         Title = AdsDAO.SERVICE_NAME;
         ActiveMenuId = "mnuAds" + Parameters.GetString(Cosmo.Workspace.PARAM_FOLDER_ID);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene los parámetros
         int folderid = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);

         // Obtiene las propiedades particulares de la carpeta actual
         ads = new AdsDAO(Workspace);
         folder = ads.GetFolder(folderid);
         if (folder == null)
         {
            ShowError("Categoria no encontrada",
                      "La categoria de anuncios clasificados solicitada no existe o bien no se encuentra disponible.");
            return;
         }

         PageHeaderControl header = new PageHeaderControl(this);
         header.Icon = IconControl.ICON_SHOPPING_CART;
         header.Title = AdsDAO.SERVICE_NAME;
         header.SubTitle = folder.Name;
         MainContent.Add(header);

         // Agrega la meta-información de la página
         Title = AdsDAO.SERVICE_NAME + " - " + folder.Name;

         // Insert a modal to show user data
         Cosmo.Web.UserDataModal userData = new Cosmo.Web.UserDataModal();
         Modals.Add(userData);

         //--------------------------------------------------------------
         // Genera la lista de anuncios a mostrar
         //--------------------------------------------------------------

         Url url;
         List<Ad> adlist = ads.Items(folder.ID, true);

         if (adlist.Count > 0)
         {
            TableRow row;

            TableControl table = new TableControl(this);
            table.Hover = true;
            table.Header = new TableRow("row-head", "Anuncio", "Precio", "Añadido", "Autor");

            foreach (Ad ad in adlist)
            {
               // Genera el elemento de la lista
               row = new TableRow("row-ad-" + ad.ID,
                                  IconControl.GetIcon(this, IconControl.ICON_TAG) + " " + HtmlContentControl.Link(AdsView.GetURL(ad.ID), ad.Title, false),
                                  ad.Price <= 0 ? IconControl.GetIcon(this, IconControl.ICON_MINUS) : string.Format("{0:C}", ad.Price),
                                  ad.Updated.ToString(Formatter.FORMAT_DATE),
                                  new UserLinkControl(this, ad.UserID, ad.UserLogin, userData));

               table.Rows.Add(row);
            }

            HtmlContentControl html = new HtmlContentControl(this);
            html.AppendParagraph(folder.Description);

            PanelControl panel = new PanelControl(this);
            panel.Caption = folder.Name;
            panel.CaptionIcon = IconControl.ICON_FOLDER_OPEN;
            panel.Content.Add(html);
            panel.Content.Add(table);

            panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnAddClassified", "Añadir anuncio", "fa-plus", AdsEditor.GetURL(folderid), string.Empty));

            url = new Url("ClassifiedManage");
            panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnManageAds", "Mis anuncios", "fa-bookmark", url.ToString(true), string.Empty));

            MainContent.Add(panel);
         }
         else
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Type = ComponentColorScheme.Information;
            callout.Title = "No hay ningún anuncio en esta categoria";
            callout.Text = "Actualmente no hay ningún anuncio en esta categoria. Vuelve regularmente para comprobar si hay novedades.";

            MainContent.Add(callout);
         }
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

      public static string GetURL(int folderId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_ADD);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, folderId);

         return url.ToString();
      }

      #endregion

   }
}