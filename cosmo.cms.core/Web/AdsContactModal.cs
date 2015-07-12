using Cosmo.Cms.Model.Ads;
using Cosmo.Communications;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;

namespace Cosmo.Cms.Web
{
   /// <summary>
   /// Implementa un formulario de contacto con el autor de un anuncio clasificado.
   /// </summary>
   [ViewParameter(ParameterName = Workspace.PARAM_OBJECT_ID, 
                  PropertyName = "ClassifiedAdID")]
   public class AdsContactModal : ModalView
   {
      // Modal element unique identifier
      private const string DOM_ID = "frm-ads-contact";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ClassifiedContactModal"/>.
      /// </summary>
      public AdsContactModal()
         : base()
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ClassifiedContactModal"/>.
      /// </summary>
      /// <param name="classifiedAdID">Identificador del anuncio clasificado.</param>
      public AdsContactModal(int classifiedAdID)
         : base()
      {
         Initialize();

         this.ClassifiedAdID = classifiedAdID;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del anuncio para el que se desea generar el 
      /// formulario de contacto.
      /// </summary>
      public int ClassifiedAdID { get; set; }

      #endregion

      #region ModalView Implementation

      /// <summary>
      /// Método invocado al iniciar la carga de la página, antes de procesar los datos recibidos.
      /// </summary>
      public override void InitPage()
      {
         this.Title = "Contactar con el autor del anuncio";
         this.Icon = Cosmo.UI.Controls.IconControl.ICON_ENVELOPE;

         // Gets the call parameters
         ClassifiedAdID = Parameters.GetInteger(Workspace.PARAM_OBJECT_ID);
         if (ClassifiedAdID <= 0)
         {
            ShowError("Anuncio no encontrado",
                      "El anuncio especificado no existe o no se encuentra disponible en estos momentos.");
            return;
         }

         // Genera el formulario para objetos del tipo User
         FormControl form = new FormControl(this, "frmAdContact");
         form.Caption = "Contacto para anuncio clasificado";
         form.Icon = IconControl.ICON_ENVELOPE;
         form.UsePanel = false;
         form.SendDataMethod = FormControl.FormSendDataMethod.JSSubmit;

         form.AddFormSetting(Cosmo.Workspace.PARAM_OBJECT_ID, ClassifiedAdID);

         FormFieldText txtName = new FormFieldText(this, "txtName", "Nombre", FormFieldText.FieldDataType.Text);
         txtName.Description = "Especifica tu nombre para que el autor sepa como te llamas.";
         txtName.Required = true;
         form.Content.Add(txtName);

         FormFieldText txtMail = new FormFieldText(this, "txtMail", "Correo electrónico", FormFieldText.FieldDataType.Email);
         txtMail.Description = "Este correo será proporcionado al autor para que se ponga en contacto contigo.";
         txtMail.Required = true;
         form.Content.Add(txtMail);

         FormFieldEditor txtMsg = new FormFieldEditor(this, "txtMsg", "Mensaje", FormFieldEditor.FieldEditorType.Simple);
         txtMsg.Required = true;
         form.Content.Add(txtMsg);

         FormFieldCaptcha txtCaptcha = new FormFieldCaptcha(this, "txtCaptcha", "Código de seguridad");
         txtCaptcha.Description = "Este campo se usa para evitar envíos automatizados y/o fraudulentos.";
         form.Content.Add(txtCaptcha);

         form.FormButtons.Add(new ButtonControl(this, "cmdSend", "Enviar", IconControl.ICON_SEND, ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdCancel", "Cancelar", IconControl.ICON_REPLY, ButtonControl.ButtonTypes.CloseModalForm));

         Content.Add(form);
      }

      /// <summary>
      /// Método invocado al recibir datos de un formulario.
      /// </summary>
      /// <param name="receivedForm">Una instancia de <see cref="FormControl"/> que representa el formulario recibido. El formulario está actualizado con los datos recibidos.</param>
      public override void FormDataReceived(UI.Controls.FormControl receivedForm)
      {
         Content.Clear();

         try
         {
            AdContactRequest request = new AdContactRequest();
            request.ClassifiedAdId = Parameters.GetInteger(Cosmo.Workspace.PARAM_OBJECT_ID);
            request.Name = Parameters.GetString("txtName");
            request.Mail = Parameters.GetString("txtMail");
            request.Message = Parameters.GetString("txtMsg");
            request.IpAddress = Request.UserHostAddress;

            // Si el objeto es válido se realiza la acción
            AdsDAO ads = new AdsDAO(Workspace);
            ads.SendContactRequest(request);

            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Petición de contacto enviada con éxito";
            callout.Icon = IconControl.ICON_CHECK;
            callout.Text = "La petición de contacto se ha enviado con éxito al autor del anuncio.";
            callout.Type = ComponentColorScheme.Success;

            Content.Add(callout);
         }
         catch (CommunicationsException)
         {
            Content.Add(new AlertControl(this, "Se ha producido un problema al enviar el mensaje de contacto.", ComponentColorScheme.Error));

            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Uuupppsss! Se produjo un error...";
            callout.Icon = IconControl.ICON_WARNING;
            callout.Text = "Se ha producido un problema al enviar el mensaje de contacto por correo electrónico y no ha sido posible realizar el envío.";
            callout.Type = ComponentColorScheme.Error;

            Content.Add(callout);
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }
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
      /// Método invocado durante la carga de la página.
      /// </summary>
      public override void LoadPage()
      {
         // Nothing to do here
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance.
      /// </summary>
      private void Initialize()
      {
         this.DomID = DOM_ID;
         this.ClassifiedAdID = 0;
      }

      #endregion

   }
}
