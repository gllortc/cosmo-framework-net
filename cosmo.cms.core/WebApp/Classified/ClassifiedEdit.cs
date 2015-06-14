using Cosmo.Cms.Classified;
using Cosmo.Cms.Content;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System.Reflection;

namespace Cosmo.WebApp.Classified
{
   [AuthenticationRequired]
   public class ClassifiedEdit : PageView
   {
      // Declaración de nombres de parámetros
      private const string FIELD_TITLE = "tit";
      private const string FIELD_DESCRIPTION = "des";
      private const string FIELD_CONTENT = "con";
      private const string FIELD_THUMBNAIL = "thb";
      private const string FIELD_ATTACHMENT = "att";
      private const string FIELD_STATUS = "sta";
      private const string FIELD_HIGHLIGHT = "hgl";

      #region PageView Implementation

      public override void InitPage()
      {
         string cmd;
         ClassifiedAd doc = null;
         ClassifiedAdsSection folder = null;

         // Agrega los recursos necesarios para representar la página actual
         Resources.Add(new ViewResource(ViewResource.ResourceType.JavaScript, "include/ContentEdit.js"));

         //-------------------------------
         // Obtención de parámetros
         //-------------------------------

         // Obtiene los parámetros de llamada
         int docId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);

         //-------------------------------
         // Obtención de datos
         //-------------------------------

         // Inicializaciones
         ClassifiedAdsDAO docs = new ClassifiedAdsDAO(Workspace);

         cmd = Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND);
         switch (cmd)
         {
            case Cosmo.Workspace.COMMAND_EDIT:
               doc = docs.Item(docId);
               break;

            case Cosmo.Workspace.COMMAND_ADD:
               doc = new ClassifiedAd();
               doc.FolderID = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);
               break;

            default:
               ShowError("Llamada incorrecta!",
                         "Hemos detectado una llamada incorrecta al editor de anuncios. No es posible abrir el editor en estas condiciones.");
               break;
         }

         // Obtiene el documento y la carpeta
         folder = docs.GetFolder(doc.FolderID);
         if (folder == null)
         {
            ShowError("Categoria no encontrada",
                      "No se ha podido determinar la categoria a la que desea añadir el anuncio. No es posible abrir el editor en estas condiciones.");
            return;
         }

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = ClassifiedAdsDAO.SERVICE_NAME + " | " + doc.Title;
         // ActiveMenuId = folder.MenuId;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = DocumentDAO.SERVICE_NAME;
         header.Icon = IconControl.ICON_BOOK;
         MainContent.Add(header);

         // Formulario
         FormControl form = new FormControl(this);
         form.Icon = "fa-edit";
         form.Caption = "Editar artículo";
         form.Action = GetType().Name;

         form.AddFormSetting(Cosmo.Workspace.PARAM_COMMAND, Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND));
         form.AddFormSetting(Cosmo.Workspace.PARAM_OBJECT_ID, doc.ID);
         form.AddFormSetting(Cosmo.Workspace.PARAM_FOLDER_ID, doc.FolderID);

         form.Content.Add(new FormFieldText(this, FIELD_TITLE, "Título", FormFieldText.FieldDataType.Text, doc.Title));
         form.Content.Add(new FormFieldEditor(this, FIELD_CONTENT, "Anuncio", FormFieldEditor.FieldEditorType.HTML, doc.Body));

         FormFieldList lstStatus = new FormFieldList(this, FIELD_STATUS, "Estado", FormFieldList.ListType.Single, (doc.Status == Cms.Common.PublishStatus.Published ? "1" : "0"));
         lstStatus.Values.Add(new KeyValue("Despublicado (Borrador)", "0"));
         lstStatus.Values.Add(new KeyValue("Publicado", "1"));
         form.Content.Add(lstStatus);

         form.FormButtons.Add(new ButtonControl(this, "btnSave", "Guardar", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "btnCancel", "Cancelar", ClassifiedView.GetURL(doc.ID), string.Empty));

         form.FormButtons[0].Color = ComponentColorScheme.Success;
         form.FormButtons[0].Icon = "fa-check";

         MainContent.Add(form);
      }

      public override void LoadPage()
      {
         // Nothing to do
      }

      /// <summary>
      /// Trata los datos recibidos de un formulario.
      /// </summary>
      public override void FormDataReceived(FormControl receivedForm)
      {
         Document clsAd = new Document();

         // Obtiene los datos del formulario
         clsAd.ID = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);
         clsAd.FolderId = Parameters.GetInteger(Cosmo.Workspace.PARAM_FOLDER_ID);
         clsAd.Title = Parameters.GetString(FIELD_TITLE);
         clsAd.Description = Parameters.GetString(FIELD_DESCRIPTION);
         clsAd.Content = Parameters.GetString(FIELD_CONTENT);
         clsAd.Published = Parameters.GetBoolean(FIELD_STATUS);
         clsAd.Hightlight = Parameters.GetBoolean(FIELD_HIGHLIGHT);
         if (Collections.ContainsKey(Request.Files, FIELD_THUMBNAIL)) clsAd.Thumbnail = Request.Files.Get(FIELD_THUMBNAIL).FileName;
         if (Collections.ContainsKey(Request.Files, FIELD_ATTACHMENT)) clsAd.Attachment = Request.Files.Get(FIELD_ATTACHMENT).FileName;

         // Realiza las operaciones de persistencia
         DocumentDAO docs = new DocumentDAO(Workspace);
         switch (Parameters.GetString(Cosmo.Workspace.PARAM_COMMAND))
         {
            case Cosmo.Workspace.COMMAND_EDIT:
               docs.Update(clsAd);
               break;

            case Cosmo.Workspace.COMMAND_ADD:
               docs.Add(clsAd);
               break;

            default: 
               break;
         }

         // Tratamiento de los archivos adjuntos
         // TODO!

         // Redirige a la página del artículo
         Redirect(ClassifiedView.GetURL(clsAd.ID));
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
