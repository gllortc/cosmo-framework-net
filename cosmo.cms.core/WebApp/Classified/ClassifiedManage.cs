using Cosmo.Cms.Classified;
using Cosmo.Net;
using Cosmo.Security;
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
   [AuthenticationRequired]
   public class ClassifiedManage : PageView
   {
      public override void LoadPage()
      {
         Title = ClassifiedAdsDAO.SERVICE_NAME;

         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = ClassifiedAdsDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_SHOPPING_CART;
         MainContent.Add(header);

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Agrega la meta-información de la página
         Title = ClassifiedAdsDAO.SERVICE_NAME + " - Mia anuncios";

         //--------------------------------------------------------------
         // Lista de carpetas
         //--------------------------------------------------------------

         // Panel de herramientas administrativas

         /*Button btnTool;

         Panel adminPanel = new Panel(this);

         btnTool = new Button(this);
         btnTool.Icon = "fa-plus-square-o";
         btnTool.Caption = "Nuevo anuncio";
         btnTool.Color = ComponentColorScheme.Success;
         btnTool.IsBlock = true;
         btnTool.Href = "ContentEdit?" + Cosmo.Workspace.PARAM_FOLDER_ID + "=" + 0 + "&" +
                                         Cosmo.Workspace.PARAM_COMMAND + "=" + Cosmo.Workspace.COMMAND_ADD;
         btnTool.Enabled = Workspace.CurrentUser.CheckAuthorization(DocumentDAO.ROLE_CONTENT_EDITOR);

         adminPanel.Content.Add(btnTool);

         btnTool = new Button(this);
         btnTool.Icon = "fa-plus-square-o";
         btnTool.Caption = "Mis anuncios";
         btnTool.Color = ComponentColorScheme.Success;
         btnTool.IsBlock = true;
         btnTool.Href = "ContentEdit?" + Cosmo.Workspace.PARAM_FOLDER_ID + "=" + 0 + "&" +
                                         Cosmo.Workspace.PARAM_COMMAND + "=" + Cosmo.Workspace.COMMAND_ADD;
         btnTool.Enabled = Workspace.CurrentUser.CheckAuthorization(DocumentDAO.ROLE_CONTENT_EDITOR);

         adminPanel.Content.Add(btnTool);

         RightContent.Add(adminPanel);*/

         //--------------------------------------------------------------
         // Genera la lista de anuncios a mostrar
         //--------------------------------------------------------------

         ClassifiedAdsDAO ads = new ClassifiedAdsDAO(Workspace);
         List<ClassifiedAd> adlist = ads.Items(false, Workspace.CurrentUser.User.ID);

         if (adlist.Count > 0)
         {
            TableRow row;

            TableControl table = new TableControl(this);
            table.Hover = true;
            table.Header = new TableRow("row-head", "Anuncio", "Precio", "Añadido", "Estado", "Acciones");

            object status;
            ButtonGroupControl tbrAd;

            foreach (ClassifiedAd ad in adlist)
            {
               // Calcula el porcentaje de publicación
               long pc = (100 * ad.RemainingDays) / 30;

               if (pc == 0)
               {
                  status = new BadgeControl(this, "Despublicado", ComponentColorScheme.Error, true);
               }
               else
               {
                  status = new ProgressBarControl(this, (int)pc, ComponentColorScheme.Success, "Quedan " + ad.RemainingDays + " días");
               }

               // Establece las opciones para cada anuncio
               tbrAd = new ButtonGroupControl(this);
               tbrAd.Size = ButtonControl.ButtonSizes.ExtraSmall;
               tbrAd.Buttons.Add(new ButtonControl(this, "cmdDel" + ad.Id, "Eliminar", IconControl.ICON_REMOVE ,ButtonControl.ButtonTypes.Normal));
               if (pc <= 0) tbrAd.Buttons.Add(new ButtonControl(this, "cmdRepub" + ad.Id, "Republicar", IconControl.ICON_REFRESH, ButtonControl.ButtonTypes.Normal));

               // Genera el elemento de la lista
               row = new TableRow("row-ad-" + ad.Id,
                                  IconControl.GetIcon(this, IconControl.ICON_TAG) + " " + HtmlContentControl.Link(ClassifiedAdsDAO.GetClassifiedAdsEditURL(ad.Id), ad.Title, false),
                                  ad.Price <= 0 ? IconControl.GetIcon(this, IconControl.ICON_MINUS) : string.Format("{0:C}", ad.Price),
                                  ad.Updated.ToString(Formatter.FORMAT_DATE),
                                  status,
                                  tbrAd);

               table.Rows.Add(row);
            }

            HtmlContentControl html = new HtmlContentControl(this);
            html.AppendParagraph("Puede gestionar en esta página sus anuncios clasificados. Puede crear nuevos anuncios, editar los anuncios, republicar los anuncios caducados o eliminar anuncios obsoletos.");

            PanelControl panel = new PanelControl(this);
            panel.Caption = "Mis anuncios clasificados";
            panel.CaptionIcon = IconControl.ICON_BOOKMARK;
            panel.Content.Add(html);
            panel.Content.Add(table);

            panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnAddClassified", "Crear nuevo anuncio", IconControl.ICON_PLUS, ClassifiedAdsDAO.GetClassifiedAdsEditURL(), string.Empty));

            MainContent.Add(panel);
         }
         else
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Type = ComponentColorScheme.Information;
            callout.Title = "Sin anuncios";
            callout.Text = "Actualmente no dispone de ningún anuncio clasificado.";

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