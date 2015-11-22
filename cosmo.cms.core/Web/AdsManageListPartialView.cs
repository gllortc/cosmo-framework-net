using Cosmo.Cms.Model.Ads;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using System.Collections.Generic;

namespace Cosmo.Cms.Web
{
   /// <summary>
   /// Generates a list with all user ads and show options for all ads.
   /// </summary>
   [AuthenticationRequired]
   public class AdsManageListPartialView : PartialView
   {

      // Modal element unique identifier
      private const string DOM_ID = "ads-list";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="AdsManageListPartialView"/>.
      /// </summary>
      public AdsManageListPartialView()
         : base(AdsManageListPartialView.DOM_ID)
      { }

      #endregion

      #region PageView Implementation

      public override void LoadPage()
      {
         //--------------------------------------
         // DELETE ADS MODAL
         //--------------------------------------

         // Attach the delete ad modal
         AdsDeleteModal deleteModal = new AdsDeleteModal();

         // Generate script to refresh partial view on delete modal close
         SimpleScript deleteReload = new SimpleScript(this);
         deleteReload.AppendSourceLine(this.GetInvokeCall());
         deleteReload.ExecutionType = Script.ScriptExecutionMethod.OnEvent;
         deleteReload.AttachToEvent(deleteModal.DomID, Script.Events.EVENT_ON_MODAL_CLOSE);
         
         Scripts.Add(deleteReload);
         Modals.Add(deleteModal);

         //--------------------------------------
         // REPUBLISH ADS MODAL
         //--------------------------------------

         // Attach the republish ad modal
         AdsRepublishModal republishModal = new AdsRepublishModal();

         // Generate script to refresh partial view on republish modal close
         SimpleScript repReload = new SimpleScript(this);
         repReload.AppendSourceLine(this.GetInvokeCall());
         repReload.ExecutionType = Script.ScriptExecutionMethod.OnEvent;
         repReload.AttachToEvent(republishModal.DomID, Script.Events.EVENT_ON_MODAL_CLOSE);

         Scripts.Add(repReload);
         Modals.Add(republishModal);

         //--------------------------------------
         // UNPUBLISH ADS MODAL
         //--------------------------------------

         // Attach the republish ad modal
         AdsUnpublishModal unpublishModal = new AdsUnpublishModal();

         // Generate script to refresh partial view on republish modal close
         SimpleScript unpReload = new SimpleScript(this);
         unpReload.AppendSourceLine(this.GetInvokeCall());
         unpReload.ExecutionType = Script.ScriptExecutionMethod.OnEvent;
         unpReload.AttachToEvent(unpublishModal.DomID, Script.Events.EVENT_ON_MODAL_CLOSE);

         Scripts.Add(unpReload);
         Modals.Add(unpublishModal);

         //--------------------------------------
         // ADS LIST
         //--------------------------------------

         AdsDAO ads = new AdsDAO(Workspace);
         List<Ad> adlist = ads.GetByUser(Workspace.CurrentUser.User.ID, false);

         HtmlContentControl html = new HtmlContentControl(this);
         html.AppendParagraph("Puede gestionar en esta página sus anuncios clasificados. Puede crear nuevos anuncios, editar los anuncios, republicar los anuncios caducados o eliminar anuncios obsoletos.");

         PanelControl panel = new PanelControl(this);
         panel.Text = "Mis anuncios clasificados";
         panel.CaptionIcon = IconControl.ICON_BOOKMARK;
         panel.Content.Add(html);

         panel.ButtonBar.Buttons.Add(new ButtonControl(this, "btnAddClassified", "Crear nuevo anuncio", IconControl.ICON_PLUS, AdsEditor.GetURL(0), string.Empty));

         if (adlist.Count > 0)
         {
            TableRow row;

            TableControl table = new TableControl(this);
            table.Hover = true;
            table.Header = new TableRow("row-head", "Anuncio", "Precio", "Publicado", "Estado", "Acciones");

            object status;
            ButtonGroupControl tbrAd;

            foreach (Ad ad in adlist)
            {
               // Calcula el porcentaje de publicación
               long pc = (100 * ad.RemainingDays) / 30;

               if (!ad.IsPublished)
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

               tbrAd.Buttons.Add(new ButtonControl(this, "cmdEdit" + ad.ID, "Editar", IconControl.ICON_EDIT, AdsEditor.GetURL(ad.FolderID, ad.ID), string.Empty));

               if (!ad.IsPublished)
               {
                  republishModal.AdID = ad.ID;
                  tbrAd.Buttons.Add(new ButtonControl(this, "cmdRepub" + ad.ID, "Republicar", IconControl.ICON_EYE, republishModal));
               }
               else
               {
                  unpublishModal.AdID = ad.ID;
                  tbrAd.Buttons.Add(new ButtonControl(this, "cmdUnpub" + ad.ID, "Despublicar", IconControl.ICON_EYE_SLASH, unpublishModal));
               }

               deleteModal.AdID = ad.ID;
               tbrAd.Buttons.Add(new ButtonControl(this, "cmdDel" + ad.ID, "Eliminar", IconControl.ICON_REMOVE, deleteModal));

               // Genera el elemento de la lista
               row = new TableRow("row-ad-" + ad.ID,
                                  IconControl.GetIcon(this, IconControl.ICON_TAG) + " " + HtmlContentControl.Link(AdsView.GetURL(ad.ID), ad.Title, false),
                                  ad.Price <= 0 ? IconControl.GetIcon(this, IconControl.ICON_MINUS) : string.Format("{0:C}", ad.Price),
                                  ad.Updated.ToString(Cosmo.Utils.Calendar.FORMAT_DATE),
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

         Content.Add(panel);
      }

      #endregion

   }
}