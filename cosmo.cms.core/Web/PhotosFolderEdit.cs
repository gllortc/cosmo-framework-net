using Cosmo.Cms.Model;
using Cosmo.Cms.Model.Content;
using Cosmo.Cms.Model.Photos;
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
   public class PhotosFolderEdit : PageView
   {
      // Field name declarations
      private const string FIELD_PARENTID = "fpi";
      private const string FIELD_NAME = "name";
      private const string FIELD_CONTENT = "con";
      private const string FIELD_STATUS = "sta";
      private const string FIELD_PATTERN = "ptt";
      private const string FIELD_ORDER = "or";
      private const string FIELD_ISCONTAINER = "isc";

      #region PageView Implementation

      public override void InitPage()
      {
         string cmd;
         PhotoFolder folder = null;
         Dictionary<string, object> jsViewParams;

         // Add needed resources
         Resources.Add(new ViewResource(ViewResource.ResourceType.JavaScript, "include/ContentEdit.js"));

         //-------------------------------
         // Parameters
         //-------------------------------

         // Get call parameters
         int folderId = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);

         //-------------------------------
         // Obtención de datos
         //-------------------------------

         // Inicializaciones
         PhotoDAO photos = new PhotoDAO(Workspace);

         cmd = Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND);
         switch (cmd)
         {
            case Cosmo.Workspace.COMMAND_EDIT:
               folder = photos.GetFolder(folderId, true);
               break;

            case Cosmo.Workspace.COMMAND_ADD:
               folder = new PhotoFolder();
               folder.ParentID = folderId;
               break;

            default:
               ShowError("Llamada incorrecta!",
                         "Hemos detectado una llamada incorrecta al editor de artículos. No es posible abrir el editor en estas condiciones.");
               break;
         }

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = folder.Name + " | " + PhotoDAO.SERVICE_NAME;
         ActiveMenuId = "photo-browse";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = PhotoDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_CAMERA;
         MainContent.Add(header);

         TabbedContainerControl tabs = new TabbedContainerControl(this);

         // Content form tab

         TabItemControl frmData = new TabItemControl(this, "tabData", "Contenido");
         frmData.Content.Add(new FormFieldText(this, FIELD_NAME, "Título", FormFieldText.FieldDataType.Text, folder.Name));
         frmData.Content.Add(new FormFieldEditor(this, FIELD_CONTENT, "Contenido", FormFieldEditor.FieldEditorType.HTML, folder.Description));
         frmData.Content.Add(new FormFieldText(this, FIELD_PATTERN, "Patrón para nombres de archivo", FormFieldText.FieldDataType.Text, folder.FilePattern));
         frmData.Content.Add(new FormFieldText(this, FIELD_ORDER, "Orden", FormFieldText.FieldDataType.Number, folder.Order.ToString()));
         frmData.Content.Add(new FormFieldBoolean(this, FIELD_ISCONTAINER, "Puede contener imágenes", folder.IsContainer));

         FormFieldList lstStatus = new FormFieldList(this, FIELD_STATUS, "Estado", FormFieldList.ListType.Single, ((int)folder.Status).ToString());
         lstStatus.Values.Add(new KeyValue("Despublicado (Borrador)", ((int)CmsPublishStatus.PublishStatus.Unpublished).ToString()));
         lstStatus.Values.Add(new KeyValue("Publicado", ((int)CmsPublishStatus.PublishStatus.Published).ToString()));
         frmData.Content.Add(lstStatus);

         tabs.TabItems.Add(frmData);

         // Content properties tab

         HtmlContentControl infoContent = new HtmlContentControl(this);
         infoContent.AppendParagraph("Propiedades del contenido en el momento actual (antes de guardar y actualizar las propiedades).");

         TableControl tableInfo = new TableControl(this);
         tableInfo.Bordered = false;
         tableInfo.Condensed = true;
         tableInfo.Header = new TableRow(string.Empty, "Propiedad", "Valor");
         tableInfo.Rows.Add(new TableRow(string.Empty, "Fecha de creación", "<no disponible>"));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Última actualización", "<no disponible>"));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Propietario", folder.Owner));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Estado actual", folder.Status == CmsPublishStatus.PublishStatus.Published ? "Publicado" : "Despublicado (Borrador)"));
         tableInfo.Rows.Add(new TableRow(string.Empty, "Visitas", "<no disponible>"));

         TabItemControl frmInfo = new TabItemControl(this, "tabInfo", "Información");
         frmInfo.Content.Add(infoContent);
         frmInfo.Content.Add(tableInfo);
         tabs.TabItems.Add(frmInfo);

         // Form generation

         FormControl form = new FormControl(this, "frmCEdit");
         form.IsMultipart = true;
         form.Icon = "fa-edit";
         form.Text = "Editar carpeta de imágenes";
         form.Action = GetType().Name;
         form.AddFormSetting(Cosmo.Workspace.PARAM_COMMAND, Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND));
         form.AddFormSetting(Cosmo.Workspace.PARAM_FOLDER_ID, folder.ID);
         form.AddFormSetting(FIELD_PARENTID, folder.ParentID);
         form.Content.Add(tabs);
         form.FormButtons.Add(new ButtonControl(this, "btnSave", "Guardar", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "btnCancel", "Cancelar", ContentByFolder.GetURL(folder.ID), string.Empty));

         form.FormButtons[0].Color = ComponentColorScheme.Success;
         form.FormButtons[0].Icon = "fa-check";

         MainContent.Add(form);
      }

      /// <summary>
      /// Trata los datos recibidos de un formulario.
      /// </summary>
      public override void FormDataReceived(FormControl receivedForm)
      {
         PhotoFolder folder = new PhotoFolder();

         // Retrieve the object data from form data
         folder.ID = receivedForm.GetIntFieldValue(Cosmo.Workspace.PARAM_FOLDER_ID);
         folder.ParentID = receivedForm.GetIntFieldValue(FIELD_PARENTID);
         folder.Name = receivedForm.GetStringFieldValue(FIELD_NAME);
         folder.Description = receivedForm.GetStringFieldValue(FIELD_CONTENT);
         folder.Order = receivedForm.GetIntFieldValue(FIELD_ORDER);
         folder.FilePattern = receivedForm.GetStringFieldValue(FIELD_PATTERN);
         folder.IsContainer = receivedForm.GetBoolFieldValue(FIELD_ISCONTAINER);
         folder.Status = CmsPublishStatus.ToPublishStatus(receivedForm.GetIntFieldValue(FIELD_STATUS));

         // Create or update the folder in database
         PhotoDAO docs = new PhotoDAO(Workspace);
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

         // Redirect to browse page
         Redirect(PhotosBrowse.GetURL());
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

      /// <summary>
      /// Gets the URL for creating a content folder.
      /// </summary>
      /// <remarks>This method create the folder into the root level.</remarks>
      public static string GetAddURL()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         url.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, 0);
         url.AddParameter(Cosmo.Workspace.PARAM_COMMAND, Cosmo.Workspace.COMMAND_ADD);

         return url.ToString();
      }

      #endregion

   }
}
