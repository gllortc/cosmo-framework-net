using Cosmo.Cms.Model.Ads;
using Cosmo.Cms.Model.Forum;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;

namespace Cosmo.Cms.Web
{
   [AuthenticationRequired]
   [ViewParameter(ParameterName = Cosmo.Workspace.PARAM_OBJECT_ID,
                  PropertyName = "AdID")]
   public class AdsDeleteModal : ModalView
   {
      // Modal element unique identifier 
      private const string DOM_ID = "ads-delete-modal";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="AdsDeleteModal"/>.
      /// </summary>
      public AdsDeleteModal()
         : base()
      {
         Initialize();

         this.DomID = AdsDeleteModal.DOM_ID;
      }

      /// <summary>
      /// Gets an instance of <see cref="AdsDeleteModal"/>.
      /// </summary>
      /// <param name="adId">Ad identifier.</param>
      public AdsDeleteModal(int adId)
         : base()
      {
         Initialize();

         this.DomID = AdsDeleteModal.DOM_ID;
         this.AdID = adId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the ad unique identifier.
      /// </summary>
      public int AdID { get; set; } 

      #endregion

      #region ModalViewContainer Implementation

      public override void InitPage()
      {
         Title = "Eliminar anuncio clasificado";
         Icon = IconControl.ICON_DELETE;
         Closeable = true;

         FormControl form = new FormControl(this, "frmAdDelete");
         form.UsePanel = false;
         form.SendDataMethod = FormControl.FormSendDataMethod.JSSubmit;
         form.AddFormSetting(Cosmo.Workspace.PARAM_OBJECT_ID, this.AdID);

         CalloutControl callout = new CalloutControl(this);
         callout.Type = ComponentColorScheme.Information;
         callout.Title = "Información sobre la acción";
         callout.Icon = IconControl.ICON_WARNING;
         callout.Text = "La eliminación del anuncio es permanente y no se puede deshacer.";
         form.Content.Add(callout);

         // Agrega un mensaje de confirmación
         HtmlContentControl confirm = new HtmlContentControl(this);
         confirm.AppendParagraph("¿Está seguro/a que desea eliminar el anuncio clasificado?");
         form.Content.Add(confirm);

         form.FormButtons.Add(new ButtonControl(this, "cmdAccept", "Aceptar", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdClose", "Cancelar", ButtonControl.ButtonTypes.CloseModalForm));

         Content.Add(form);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         CalloutControl callout = null;

         // Obtiene los parámetros de la llamada.
         int adId = receivedForm.GetIntFieldValue(Cosmo.Workspace.PARAM_OBJECT_ID);

         Content.Clear();

         try
         {
            // Initialization
            AdsDAO adao = new AdsDAO(Workspace);

            // Check the ad is from current user
            Ad ad = adao.Item(adId);

            if (ad.UserID == Workspace.CurrentUser.User.ID)
            {
               // Delete the ad
               adao.Delete(adId, true);

               callout = new CalloutControl(this);
               callout.Title = "Operación completada con éxito";
               callout.Icon = IconControl.ICON_CHECK;
               callout.Text = "El anuncio ha sido eliminado.";
               callout.Type = ComponentColorScheme.Success;
            }
            else
            {
               callout = new CalloutControl(this);
               callout.Title = "Operación fraudulenta detectada!";
               callout.Icon = IconControl.ICON_WARNING;
               callout.Text = "Usted no es el autor del anuncio que está tratando de eliminar. " +
                              "Este intento fraudulento ha sido reportado al departamento IT para su análisis.";
               callout.Type = ComponentColorScheme.Error;

               // Audit illegal delete request
               Workspace.Logger.Add("User " + Workspace.CurrentUser.User.Login + " try to delete ad #" + adId + " without permission.",
                                    Diagnostics.LogEntry.LogEntryType.EV_SECURITY,
                                    GetType().Name);
            }

            Content.Add(callout);
         }
         catch
         {
            callout = new CalloutControl(this);
            callout.Title = "ERROR";
            callout.Icon = IconControl.ICON_WARNING;
            callout.Text = "Se ha producido un error al intentar completar la operación.";
            callout.Type = ComponentColorScheme.Error;

            Content.Add(callout);
         }

         ButtonControl cmdClose = new ButtonControl(this, "cmdClose", "Cerrar", ButtonControl.ButtonTypes.CloseModalForm);
         Content.Add(cmdClose);
      }

      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      public override void LoadPage()
      {
         // Nothing to do
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.AdID = 0;
      }

      #endregion

   }
}
