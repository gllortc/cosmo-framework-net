using Cosmo.Cms.Classified;
using Cosmo.Net;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils.Html;
using System;
using System.Collections.Generic;

namespace Cosmo.WebApp.Classified
{
   /// <summary>
   /// ANUNCIOS CLASIFICADOS.
   /// Muestra el contenido de una categoria.
   /// </summary>
   public class ClassifiedFolder : PageViewContainer
   {
      public override void LoadPage()
      {
         // Declaraciones
         ClassifiedAdsDAO ads = null;
         ClassifiedAdsSection folder = null;

         Title = ClassifiedAdsDAO.SERVICE_NAME;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene los parámetros
         int folderid = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID, 0);

         // Obtiene las propiedades particulares de la carpeta actual
         ads = new ClassifiedAdsDAO(Workspace);
         folder = ads.GetFolder(folderid);
         if (folder == null)
         {
            ShowError("Categoria no encontrada",
                      "La categoria de anuncios clasificados solicitada no existe o bien no se encuentra disponible.");
            return;
         }

         PageHeaderControl header = new PageHeaderControl(this);
         header.Icon = IconControl.ICON_SHOPPING_CART;
         header.Title = ClassifiedAdsDAO.SERVICE_NAME;
         header.SubTitle = folder.Name;
         MainContent.Add(header);

         // Agrega la meta-información de la página
         Title = ClassifiedAdsDAO.SERVICE_NAME + " - " + folder.Name;

         /*
         //--------------------------------------------------------------
         // Lista de carpetas
         //--------------------------------------------------------------

         Panel panelFolders = new Panel(this);
         panelFolders.Caption = "Secciones";
         panelFolders.Content.Add(LayoutAdapter.ClassifiedAds.ConvertSectionsToListGroup(this, ads.GetFolders(true), folder.ID));

         RightContent.Add(panelFolders);

         // Panel de herramientas administrativas

         Button btnTool;
         

         Panel adminPanel = new Panel(this);

         url = new Url("ClassifiedEdit");
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_ADD);

         btnTool = new Button(this);
         btnTool.Icon = "fa-plus-square-o";
         btnTool.Caption = "Nuevo anuncio";
         btnTool.Color = ComponentColorScheme.Success;
         btnTool.IsBlock = true;
         btnTool.Href = url.ToString(true);

         adminPanel.Content.Add(btnTool);

         url = new Url("ClassifiedManage");

         btnTool = new Button(this);
         btnTool.Icon = "fa-plus-square-o";
         btnTool.Caption = "Mis anuncios";
         btnTool.Color = ComponentColorScheme.Success;
         btnTool.IsBlock = true;
         btnTool.Href = url.ToString(true);

         adminPanel.Content.Add(btnTool);

         RightContent.Add(adminPanel);
         */
         //--------------------------------------------------------------
         // Genera la lista de anuncios a mostrar
         //--------------------------------------------------------------

         Url url;
         List<ClassifiedAd> adlist = ads.Items(folder.ID, true);

         if (adlist.Count > 0)
         {
            TableRow row;

            TableControl table = new TableControl(this);
            table.Hover = true;
            table.Header = new TableRow("row-head", "Anuncio", "Precio", "Añadido", "Autor");

            foreach (ClassifiedAd ad in adlist)
            {
               // Genera el elemento de la lista
               row = new TableRow("row-ad-" + ad.Id,
                                  IconControl.GetIcon(this, IconControl.ICON_TAG) + " " + HtmlContentControl.Link(ClassifiedAdsDAO.GetClassifiedAdsViewURL(ad.Id), ad.Title, false),
                                  ad.Price <= 0 ? IconControl.GetIcon(this, IconControl.ICON_MINUS) : string.Format("{0:C}", ad.Price),
                                  ad.Updated.ToString(Formatter.FORMAT_DATE),
                                  IconControl.GetIcon(this, IconControl.ICON_USER) + " " + ad.UserLogin);

               table.Rows.Add(row);
            }

            HtmlContentControl html = new HtmlContentControl(this);
            html.AppendParagraph(folder.Description);

            PanelControl panel = new PanelControl(this);
            panel.Caption = folder.Name;
            panel.CaptionIcon = IconControl.ICON_FOLDER_OPEN;
            panel.Content.Add(html);
            panel.Content.Add(table);

            url = new Url("ClassifiedEdit");
            url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_ADD);
            panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnAddClassified", "Añadir anuncio", "fa-plus", url.ToString(true), string.Empty));

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
   }
}