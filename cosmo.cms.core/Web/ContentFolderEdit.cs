using Cosmo.Cms.Model;
using Cosmo.Cms.Model.Content;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Menu;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using Cosmo.Web;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Cms.Web
{
   [AuthorizationRequired(DocumentDAO.ROLE_CONTENT_EDITOR)]
   public class ContentFolderEdit : PageView
   {
      // Field name declarations
      private const string FIELD_PARENTID = "opi";
      private const string FIELD_TITLE = "tit";
      private const string FIELD_CONTENT = "con";
      private const string FIELD_STATUS = "sta";
      private const string FIELD_MENUITEM = "mi";
      private const string FIELD_ORDER = "or";

      #region PageView Implementation

      public override void InitPage()
      {
         string cmd;
         DocumentFolder folder = null;
         Dictionary<string, object> jsViewParams;

         // Add needed resources
         Resources.Add(new ViewResource(ViewResource.ResourceType.JavaScript, "include/ContentEdit.js"));

         //-------------------------------
         // Parameters
         //-------------------------------

         // Get call parameters
         int folderId = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);
         DocumentFSID fsId = new DocumentFSID(folderId, true);

         //-------------------------------
         // Obtención de datos
         //-------------------------------

         // Inicializaciones
         DocumentDAO docs = new DocumentDAO(Workspace);

         cmd = Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND);
         switch (cmd)
         {
            case Cosmo.Workspace.COMMAND_EDIT:
               folder = docs.GetFolder(folderId, true);
               break;

            case Cosmo.Workspace.COMMAND_ADD:
               folder = new DocumentFolder();
               folder.ParentID = folderId;
               break;

            default:
               ShowError("ILLEGAL CALL!",
                         "An illegal call was detected: We can not open the editor and/or load the requested content.");
               break;
         }

         //-------------------------------
         // Habilita formularios modales
         //-------------------------------
         MediaUpload frmUpload = new MediaUpload(new DocumentFSID(folder.ID, true));
         Modals.Add(frmUpload);

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = folder.Name + " | " + DocumentDAO.SERVICE_NAME;
         ActiveMenuId = folder.MenuId;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_BOOK;
         MainContent.Add(header);

         TabbedContainerControl tabs = new TabbedContainerControl(this);

         // Content form tab

         TabItemControl frmData = new TabItemControl(this, "tabData", "Content");
         frmData.Content.Add(new FormFieldText(this, FIELD_TITLE, "Main title", FormFieldText.FieldDataType.Text, folder.Name));
         frmData.Content.Add(new FormFieldEditor(this, FIELD_CONTENT, "Content", FormFieldEditor.FieldEditorType.HTML, folder.Description));
         frmData.Content.Add(new FormFieldText(this, FIELD_ORDER, "Sort order", FormFieldText.FieldDataType.Number, folder.Order.ToString()));

         FormFieldList lstFolder = new FormFieldList(this, FIELD_PARENTID, "Parent folder", FormFieldList.ListType.Single, folder.ParentID);
         foreach (DocumentFolder dfolder in DocumentDAO.ConvertFolderTreeToList(docs.GetAll()))
         {
            lstFolder.Values.Add(new KeyValue(dfolder.Name, dfolder.ID));
         }
         frmData.Content.Add(lstFolder);

         FormFieldList lstStatus = new FormFieldList(this, FIELD_STATUS, "Status", FormFieldList.ListType.Single, (int)folder.Status);
         lstStatus.Values.Add(new KeyValue("Unpublished (Draft)", ((int)CmsPublishStatus.PublishStatus.Unpublished).ToString()));
         lstStatus.Values.Add(new KeyValue("Published", ((int)CmsPublishStatus.PublishStatus.Published).ToString()));
         frmData.Content.Add(lstStatus);

         FormFieldList lstMenu = new FormFieldList(this, FIELD_MENUITEM, "Menu", FormFieldList.ListType.Single, folder.MenuId);
         foreach (MenuItem menuItem in Workspace.UIService.GetMenu("sidebar").MenuItems)
         {
            lstMenu.Values.Add(new KeyValue(menuItem.Name, menuItem.ID));
         }
         frmData.Content.Add(lstMenu);

         tabs.TabItems.Add(frmData);

         // Files (media and content attachments) tab

         TabItemControl tabFiles = new TabItemControl(this, "tabFiles", "Attachments");

         HtmlContentControl filesContent = new HtmlContentControl(this);
         filesContent.AppendParagraph("The following list contains all content attachments.");
         tabFiles.Content.Add(filesContent);

         jsViewParams = new Dictionary<string, object>();
         jsViewParams.Add(Cosmo.Workspace.PARAM_FOLDER_ID, fsId.ToFolderName());

         ButtonGroupControl btnFiles = new ButtonGroupControl(this);
         btnFiles.Size = ButtonControl.ButtonSizes.Small;
         btnFiles.Buttons.Add(new ButtonControl(this, 
                                                "cmdAddFiles", 
                                                "Add files", 
                                                string.Empty,
                                                frmUpload.GetInvokeCall(jsViewParams)));
         btnFiles.Buttons.Add(new ButtonControl(this, "cmdRefresh", "Refresh list", IconControl.ICON_REFRESH, "#", "cosmoUIServices.loadTemplate();"));
         tabFiles.Content.Add(btnFiles);

         MediaFileList fileList = new MediaFileList(new DocumentFSID(folder.ID, true));
         PartialViewContainerControl fileListView = new PartialViewContainerControl(this, fileList);
         tabFiles.Content.Add(fileListView);
         Scripts.Add(fileList.GetInvokeScript(Script.ScriptExecutionMethod.OnDocumentReady, cmd, folder.ID));

         btnFiles.Buttons[1].Href = string.Empty;
         btnFiles.Buttons[1].JavaScriptAction = fileList.GetInvokeCall(jsViewParams);

         tabs.TabItems.Add(tabFiles);

         // Content properties tab

         HtmlContentControl infoContent = new HtmlContentControl(this);
         infoContent.AppendParagraph("Current content properties (before save).");

         TableControl tableInfo = new TableControl(this);
         tableInfo.Bordered = false;
         tableInfo.Condensed = true;
         tableInfo.Header = new TableRow(string.Empty, "Property", "Value");
         tableInfo.Rows.Add(new TableRow(string.Empty, "Creation date", folder.Created.ToString("dd/MM/yyyy hh:mm")));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Last modification", folder.Updated.ToString("dd/MM/yyyy hh:mm")));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Owner", folder.Owner));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Current status", folder.Status == CmsPublishStatus.PublishStatus.Published ? "Publicado" : "Despublicado (Borrador)"));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Views", "Not available"));

         TabItemControl frmInfo = new TabItemControl(this, "tabInfo", "Information");
         frmInfo.Content.Add(infoContent);
         frmInfo.Content.Add(tableInfo);
         tabs.TabItems.Add(frmInfo);

         // Form generation

         FormControl form = new FormControl(this, "frmCEdit");
         form.IsMultipart = true;
         form.Icon = "fa-edit";
         form.Text = "Edit content";
         form.Action = GetType().Name;
         form.AddFormSetting(Cosmo.Workspace.PARAM_COMMAND, Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND));
         form.AddFormSetting(Cosmo.Workspace.PARAM_FOLDER_ID, folder.ID);
         //form.AddFormSetting(FIELD_PARENTID, folder.ParentID);
         form.Content.Add(tabs);
         form.FormButtons.Add(new ButtonControl(this, "btnSave", "Save", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "btnCancel", "Cancel", ContentByFolder.GetURL(folder.ID), string.Empty));

         form.FormButtons[0].Color = ComponentColorScheme.Success;
         form.FormButtons[0].Icon = "fa-check";

         MainContent.Add(form);
      }

      /// <summary>
      /// Trata los datos recibidos de un formulario.
      /// </summary>
      public override void FormDataReceived(FormControl receivedForm)
      {
         DocumentFolder folder = new DocumentFolder();

         // Obtiene los datos del formulario
         folder.ID = receivedForm.GetIntFieldValue(Cosmo.Workspace.PARAM_FOLDER_ID);
         folder.ParentID = receivedForm.GetIntFieldValue(FIELD_PARENTID);
         folder.Name = receivedForm.GetStringFieldValue(FIELD_TITLE);
         folder.Description = receivedForm.GetStringFieldValue(FIELD_CONTENT);
         folder.Order = receivedForm.GetIntFieldValue(FIELD_ORDER);
         folder.MenuId = receivedForm.GetStringFieldValue(FIELD_MENUITEM);
         folder.Status = CmsPublishStatus.ToPublishStatus(receivedForm.GetIntFieldValue(FIELD_STATUS));

         // Realiza las operaciones de persistencia
         DocumentDAO docs = new DocumentDAO(Workspace);
         switch (Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND))
         {
            case Cosmo.Workspace.COMMAND_EDIT:
               docs.UpdateFolder(folder);
               break;

            case Cosmo.Workspace.COMMAND_ADD:
               docs.AddFolder(folder);
               break;

            default: 
               break;
         }

         // Redirige a la página del artículo
         Redirect(ContentByFolder.GetURL(folder.ID));
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Gets the URL for editing a content folder.
      /// </summary>
      /// <param name="folderId">Content folder unique identifier (DB).</param>
      public static string GetEditURL(int folderId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, folderId);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_EDIT);

         return url.ToString();
      }

      /// <summary>
      /// Gets the URL for creating a content folder.
      /// </summary>
      /// <param name="parentId">Parent content folder unique identifier (DB). If you want to create a top level
      /// folder, this parameter must set to 0.</param>
      public static string GetAddURL(int parentId)
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, parentId);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_ADD);

         return url.ToString();
      }

      #endregion

   }
}
