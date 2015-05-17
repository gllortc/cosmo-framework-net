using Cosmo.Cms.Photos;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils.Html;
using System;
using System.Collections.Generic;

namespace Cosmo.WebApp.Photos
{
   [AuthenticationRequired]
   public class PhotosUpload : PageViewContainer
   {
      int folderId;
      FormControl form = null;
      PhotoFolder folder = null;
      PhotoDAO pictureDao = null;

      public override void InitPage()
      {
         Title = PhotoDAO.SERVICE_NAME;
         ActiveMenuId = "photos";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Obtiene los parámetros
         folderId = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID, 0);

         // Obtiene las propiedades particulares de la carpeta actual
         pictureDao = new PhotoDAO(Workspace);
         folder = pictureDao.GetFolder(folderId, false);
         if (folder == null)
         {
            ShowError("Categoria no encontrada",
                      "La categoria especificada no existe o no se encuentra disponible en estos momentos.");
            return;
         }

         PageHeaderControl header = new PageHeaderControl(this);
         header.Icon = IconControl.ICON_CAMERA;
         header.Title = PhotoDAO.SERVICE_NAME;
         header.SubTitle = folder.Name;
         MainContent.Add(header);

         // Agrega la meta-información de la página
         Title = PhotoDAO.SERVICE_NAME + " - Agregar fotografia";

         // Genera el formulario para objetos del tipo User
         form = new FormControl(this);
         form.IsMultipart = true;
         form.DomID = "frmUploadPhoto";
         form.Caption = "Publicar nueva fotografia";
         form.Icon = IconControl.ICON_UPLOAD;

         FormFieldHidden parentid = new FormFieldHidden(this, Cosmo.Workspace.PARAM_FOLDER_ID, folderId.ToString());
         form.Content.Add(parentid);

         FormFieldFile picFile = new FormFieldFile(this, "file", "Archivo", "Formato JPG o PNG. Tamaño máximo: 1Mb", string.Empty);
         picFile.Required = true;
         picFile.AllowedExtensions.Add("jpg");
         picFile.AllowedExtensions.Add("jpeg");
         picFile.AllowedExtensions.Add("png");
         picFile.FileName = GetFileName();
         picFile.DowloadPath = Workspace.FileSystemService.GetServicePath(PhotoDAO.SERVICE_FOLDER);
         picFile.ThumbnailMaxHeight = 200;
         picFile.ThumbnailMaxWith = 100;
         picFile.CreateThumbnail = true;
         form.Content.Add(picFile);

         FormFieldEditor body = new FormFieldEditor(this, "body", "Texto descriptivo", FormFieldEditor.FieldEditorType.Simple);
         body.Description = "Solo texto, sin saltos de línea (no tienen efecto) ni enlaces (<em>links</em>).";
         body.Required = true;
         form.Content.Add(body);

         FormFieldText site = new FormFieldText(this, "site", "Lugar", FormFieldText.FieldDataType.Text);
         site.Description = "Use el formato: <em>Ciudad (Provincia)</em> o en su defecto <em>Ciudad (Pais)</em>.";
         site.Required = true;
         form.Content.Add(site);

         FormFieldDate date = new FormFieldDate(this, "date", "Fecha de captura", FormFieldDate.FieldDateType.Date);
         date.Required = true;
         form.Content.Add(date);

         FormFieldText author = new FormFieldText(this, "author", "Autor de la fotografia", FormFieldText.FieldDataType.Text);
         author.Required = true;
         author.Value = Workspace.CurrentUser.User.GetDisplayName();
         form.Content.Add(author);

         FormFieldText link = new FormFieldText(this, "link", "Enlace (<em>link</em>)", FormFieldText.FieldDataType.Text);
         link.Description = "Esta dirección puede ser su correo electrónico o una dirección web.";
         link.Required = false;
         form.Content.Add(link);

         form.FormButtons.Add(new ButtonControl(this, "cmdSend", "Enviar", IconControl.ICON_SEND, ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdCancel", "Cancelar", IconControl.ICON_REPLY, "", "history.back(-1);"));

         MainContent.Add(form);

         // Extracto de las normas de publicación
         List<String> rulesText = new List<String>();
         rulesText.Add("Publique sólo imágenes suyas o de las cuales disponga de permiso del autor o propietario. Respete los derechos de autor.");
         rulesText.Add("No publique fotografias de baja calidad, con poca resolución o borrosas si no tienen un interés excepcional.");
         rulesText.Add("Sólo se aceptan imágenes directamente relacionadas con la temática del portal. Cualquier otra temática será borrada sin previo aviso.");
         rulesText.Add("Tamaño máximo:" + pictureDao.FileMaxLength + " Kb.");

         HtmlContentControl rules = new HtmlContentControl(this);
         rules.AppendUnorderedList(rulesText);

         PanelControl rulesPanel = new PanelControl(this);
         rulesPanel.Caption = "Normas para la publicación de fotografias";
         rulesPanel.CaptionIcon = IconControl.ICON_BELL;
         rulesPanel.Content.Add(rules);

         MainContent.Add(rulesPanel);
      }

      public override void LoadPage()
      {
         // throw new System.NotImplementedException();
      }

      public override void FormDataReceived(UI.Controls.FormControl receivedForm)
      {
         // Genera la imagen
         Photo picture = new Photo();
         picture.FolderId = receivedForm.GetIntFieldValue(Cosmo.Workspace.PARAM_FOLDER_ID);
         picture.PictureFile = receivedForm.GetFileFieldValue("file").Name;
         picture.UserID = Workspace.CurrentUser.User.ID;
         picture.Description = receivedForm.GetStringFieldValue("body") + "<br />" +
                               "<em>" + receivedForm.GetStringFieldValue("site") + ", " + 
                               receivedForm.GetDateFieldValue("date").ToString(Formatter.FORMAT_DATE) + "</em>";
         // picture.Author = (chkCopyright.Checked ? "&copy; " : string.Empty) +
         //                  (txtAuthorLink.Text.Trim().Equals(string.Empty) ? txtAuthor.Text.Trim() : Cosmo.Net.Url.ConvertToLink(txtAuthorLink.Text.Trim().ToLower(), txtAuthor.Text.Trim()));
         picture.Created = DateTime.Now;

         // Obtiene el nombre de la miniatura
         FormFieldFile fileField = (FormFieldFile)receivedForm.GetField("file");
         picture.ThumbnailFile = fileField.Thumbnail.Name;

         // Genera las inatancias necesarias
         PhotoDAO pictures = new PhotoDAO(Workspace);
         pictures.Add(picture, false);

         // Redirige al usuario a la vista de carpeta
         Redirect("PhotosByFolder?fid=" + picture.FolderId);
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      /// <summary>
      /// Devuelve el nombre que debe recibir el archivo.
      /// </summary>
      private string GetFileName()
      {
         if (this.IsFormReceived)
         {
            return pictureDao.GetFileName(folder.ID,
                                          Workspace.Context.Request.Files["file"].FileName);
         }

         return string.Empty;
      }
   }
}
