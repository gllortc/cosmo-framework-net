using Cosmo.Cms.Common;
using Cosmo.Cms.Model.Ads;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System.Reflection;

namespace Cosmo.Cms.Web
{
   [AuthenticationRequired]
   public class AdsEditor : PageView
   {
      // Declaración de nombres de parámetros
      private const string FIELD_TITLE = "tit";
      private const string FIELD_CONTENT = "con";
      private const string FIELD_PRICE = "pr";
      private const string FIELD_SECTION = "section";
      private const string FIELD_STATUS = "sta";
      private const string FIELD_CONTACT_PHONE = "cp";
      private const string FIELD_CONTACT_MAIL = "cm";

      #region PageView Implementation

      public override void InitPage()
      {
         Ad ad = null;

         //-------------------------------
         // Obtención de parámetros
         //-------------------------------

         // Obtiene los parámetros de llamada
         int docId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);
         int sectionId = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);

         //-------------------------------
         // Obtención de datos
         //-------------------------------

         // Inicializaciones
         AdsDAO adsDao = new AdsDAO(Workspace);

         if (docId > 0)
         {
            // Classified ad edition
            ad = adsDao.Item(docId);
         }
         else
         {
            // New classified ad
            ad = new Ad();
            ad.FolderID = sectionId;
            ad.Mail = Workspace.CurrentUser.User.MailAlternative;
            ad.Phone = Workspace.CurrentUser.User.Phone;
         }

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = AdsDAO.SERVICE_NAME + " | " + ad.Title;
         // ActiveMenuId = folder.MenuId;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = AdsDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_BOOK;
         MainContent.Add(header);

         // Formulario
         FormControl form = new FormControl(this, "frmCAd");
         form.Icon = "fa-edit";
         form.Caption = "Editar artículo";
         form.Action = GetType().Name;

         form.AddFormSetting(Cosmo.Workspace.PARAM_OBJECT_ID, ad.ID);

         FormFieldText txtTitle = new FormFieldText(this, FIELD_TITLE);
         txtTitle.Label = "Título";
         txtTitle.Value = ad.Title;
         txtTitle.Required = true;
         form.Content.Add(txtTitle);

         FormFieldEditor txtBody = new FormFieldEditor(this, FIELD_CONTENT);
         txtBody.Label = "Texto del anuncio";
         txtBody.Type = FormFieldEditor.FieldEditorType.HTML;
         txtBody.Value = ad.Body;
         txtBody.Required = true;
         form.Content.Add(txtBody);

         FormFieldText txtPrice = new FormFieldText(this, FIELD_PRICE);
         txtPrice.Label = "Precio (€)";
         txtPrice.Value = ad.Price;
         txtPrice.Type = FormFieldText.FieldDataType.Number;
         txtPrice.Required = false;
         txtPrice.Description = "Si no desea especificar precio, deje este campo a 0.";
         form.Content.Add(txtPrice);

         FormFieldList lstFolder = new FormFieldList(this, Cosmo.Workspace.PARAM_FOLDER_ID, "Categoria", FormFieldList.ListType.Single, ad.FolderID.ToString());
         lstFolder.LoadValuesFromDataList("ads-folders");
         lstFolder.Required = true;
         form.Content.Add(lstFolder);

         FormFieldList lstStatus = new FormFieldList(this, FIELD_STATUS, "Estado", FormFieldList.ListType.Single, ((int)ad.Status).ToString());
         lstStatus.Values.Add(new KeyValue("Despublicado (Borrador)", (int)CmsPublishStatus.PublishStatus.Unpublished));
         lstStatus.Values.Add(new KeyValue("Publicado", (int)CmsPublishStatus.PublishStatus.Published));
         form.Content.Add(lstStatus);

         CalloutControl callout = new CalloutControl(this);
         callout.Type = ComponentColorScheme.Information;
         callout.Title = "Formulario de contacto";
         callout.Text = @"Las personas interesadas en este anuncio pueden contactar con usted mediante el formulario 
                          de contacto. El siguiente campo muestra la cuenta (" + HtmlContentControl.BoldText("oculto a cualquier persona") + @") 
                          a la que recibirá las peticiones de contacto. Este dato no puede ser visto por ninguna persona." +
                          HtmlContentControl.HTML_NEW_LINE + HtmlContentControl.HTML_NEW_LINE +
                          "Mail de contacto: " + HtmlContentControl.BoldText(Workspace.CurrentUser.User.Mail);
         form.Content.Add(callout);

         DocumentHeaderControl contactSection = new DocumentHeaderControl(this);
         contactSection.Text = "Datos de contacto opcionales";
         contactSection.Description = @"Si usted lo desea puede agregar al anuncio una cuenta de correo y/o un teléfono de contacto " +
                                        HtmlContentControl.BoldText("que se mostrarán de forma pública") + ".";
         form.Content.Add(contactSection);

         FormFieldText txtContactPhone = new FormFieldText(this, FIELD_CONTACT_PHONE);
         txtContactPhone.Label = "Teléfono de contacto";
         txtContactPhone.Value = ad.Phone;
         txtContactPhone.Required = true;
         txtContactPhone.Type = FormFieldText.FieldDataType.Phone;
         form.Content.Add(txtContactPhone);

         FormFieldText txtContactMail = new FormFieldText(this, FIELD_CONTACT_MAIL);
         txtContactMail.Label = "Correo de contacto";
         txtContactMail.Value = ad.Mail;
         txtContactMail.Required = true;
         txtContactMail.Type = FormFieldText.FieldDataType.Email;
         form.Content.Add(txtContactMail);

         form.FormButtons.Add(new ButtonControl(this, "btnSave", "Guardar", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "btnCancel", "Cancelar", AdsView.GetURL(ad.ID), string.Empty));

         form.FormButtons[0].Color = ComponentColorScheme.Success;
         form.FormButtons[0].Icon = "fa-check";

         MainContent.Add(form);
      }

      /// <summary>
      /// Trata los datos recibidos de un formulario.
      /// </summary>
      public override void FormDataReceived(FormControl receivedForm)
      {
         Ad clsAd = new Ad();

         // Obtiene los datos del formulario
         clsAd.ID = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);
         clsAd.FolderID = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);
         clsAd.UserID = Workspace.CurrentUser.User.ID;
         clsAd.UserLogin = Workspace.CurrentUser.User.Login;

         clsAd.Title = Parameters.GetString(FIELD_TITLE);
         clsAd.Body = Parameters.GetString(FIELD_CONTENT);
         clsAd.Price = Parameters.GetInteger(FIELD_PRICE);
         clsAd.Status = CmsPublishStatus.ToPublishStatus(Parameters.GetInteger(FIELD_STATUS));
         clsAd.Phone = Parameters.GetString(FIELD_CONTACT_PHONE);
         clsAd.Mail = Parameters.GetString(FIELD_CONTACT_MAIL);

         // Realiza las operaciones de persistencia
         AdsDAO ads = new AdsDAO(Workspace);
         if (clsAd.ID <= 0)
         {
            ads.Add(clsAd);
         }
         else
         {
            ads.Update(clsAd);
         }

         // Redirige a la página del artículo
         Redirect(AdsView.GetURL(clsAd.ID));
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      public override void LoadPage()
      {
         // Nothing to do
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Gets an URL to create a new ad in a specified category of ads.
      /// </summary>
      /// <param name="folderId">Category unique identifier.</param>
      /// <returns>A string representing the requested URL.</returns>
      public static string GetURL(int folderId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_ADD);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, folderId);

         return url.ToString();
      }

      /// <summary>
      /// Gets an URL to edit an existing ad in a specified category of ads.
      /// </summary>
      /// <param name="folderId">Category unique identifier.</param>
      /// <param name="classifiedId">Ad unique identifier.</param>
      /// <returns>A string representing the requested URL.</returns>
      public static string GetURL(int folderId, int classifiedId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_EDIT);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, folderId);
         url.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, classifiedId);

         return url.ToString();
      }

      #endregion

   }
}
