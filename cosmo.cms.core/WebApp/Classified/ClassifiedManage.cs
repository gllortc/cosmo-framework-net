﻿using Cosmo.Cms.Classified;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.WebApp.Classified
{
   /// <summary>
   /// ANUNCIOS CLASIFICADOS.
   /// Muestra el contenido de una categoria.
   /// </summary>
   [AuthenticationRequired]
   public class ClassifiedManage : PageView
   {

      #region PageView Implementation

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
         // Genera la lista de anuncios a mostrar
         //--------------------------------------------------------------

         ClassifiedAdsDAO ads = new ClassifiedAdsDAO(Workspace);
         List<ClassifiedAd> adlist = ads.Items(false, Workspace.CurrentUser.User.ID);

         HtmlContentControl html = new HtmlContentControl(this);
         html.AppendParagraph("Puede gestionar en esta página sus anuncios clasificados. Puede crear nuevos anuncios, editar los anuncios, republicar los anuncios caducados o eliminar anuncios obsoletos.");

         PanelControl panel = new PanelControl(this);
         panel.Caption = "Mis anuncios clasificados";
         panel.CaptionIcon = IconControl.ICON_BOOKMARK;
         panel.Content.Add(html);

         panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnAddClassified", "Crear nuevo anuncio", IconControl.ICON_PLUS, ClassifiedEdit.GetURL(0), string.Empty));

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
               tbrAd.Buttons.Add(new ButtonControl(this, "cmdDel" + ad.ID, "Eliminar", IconControl.ICON_REMOVE ,ButtonControl.ButtonTypes.Normal));
               if (pc <= 0) tbrAd.Buttons.Add(new ButtonControl(this, "cmdRepub" + ad.ID, "Republicar", IconControl.ICON_REFRESH, ButtonControl.ButtonTypes.Normal));

               // Genera el elemento de la lista
               row = new TableRow("row-ad-" + ad.ID,
                                  IconControl.GetIcon(this, IconControl.ICON_TAG) + " " + HtmlContentControl.Link(ClassifiedEdit.GetURL(ad.ID), ad.Title, false),
                                  ad.Price <= 0 ? IconControl.GetIcon(this, IconControl.ICON_MINUS) : string.Format("{0:C}", ad.Price),
                                  ad.Updated.ToString(Formatter.FORMAT_DATE),
                                  status,
                                  tbrAd);

               table.Rows.Add(row);
            }

            panel.Content.Add(table);
         }
         else
         {
            CalloutControl callout = new CalloutControl(this);
            callout.Type = ComponentColorScheme.Information;
            callout.Title = "Sin anuncios";
            callout.Text = "Actualmente no dispone de ningún anuncio clasificado.";

            panel.Content.Add(callout);
         }

         MainContent.Add(panel);
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