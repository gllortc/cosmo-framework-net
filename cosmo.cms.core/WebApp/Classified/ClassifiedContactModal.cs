using Cosmo.Cms.Classified;
using Cosmo.Communications;
using Cosmo.Data.ORM;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;

namespace Cosmo.Cms.WebApp.Classified
{
   /// <summary>
   /// Implementa un formulario de contacto con el autor de un anuncio clasificado.
   /// </summary>
   [ViewParameter(ParameterName = Workspace.PARAM_OBJECT_ID)]
   public class ClassifiedContactModal : ModalViewContainer
   {
      // Declaración de variables internas
      FormControl contactForm = null;

      /// <summary>
      /// Devuelve una instancia de <see cref="ClassifiedContactModal"/>.
      /// </summary>
      /// <param name="classifiedAdID">Identificador del anuncio clasificado.</param>
      public ClassifiedContactModal(string domId, int classifiedAdID) 
         : base(domId)
      {
         this.ClassifiedAdID = classifiedAdID;
      }

      /// <summary>
      /// Devuelve o establece el identificador del anuncio para el que se desea generar el 
      /// formulario de contacto.
      /// </summary>
      private int ClassifiedAdID { get; set; }

      /// <summary>
      /// Método invocado al iniciar la carga de la página, antes de procesar los datos recibidos.
      /// </summary>
      public override void InitPage()
      {
         this.Title = "Contactar con el autor del anuncio";
         this.Icon = Cosmo.UI.Controls.IconControl.ICON_ENVELOPE;

         ClassifiedContactRequest request = new ClassifiedContactRequest();
         request.ClassifiedAdId = this.ClassifiedAdID;

         OrmEngine orm = new OrmEngine();
         contactForm = orm.CreateForm(this, Request, true);
         contactForm.Action = "ClassifiedContactModal";

         Content.Add(contactForm);
      }

      /// <summary>
      /// Método invocado al recibir datos de un formulario.
      /// </summary>
      /// <param name="receivedForm">Una instancia de <see cref="FormControl"/> que representa el formulario recibido. El formulario está actualizado con los datos recibidos.</param>
      public override void FormDataReceived(UI.Controls.FormControl receivedForm)
      {
         OrmEngine orm = new OrmEngine();
         ClassifiedContactRequest request = new ClassifiedContactRequest();

         try
         {
            if (orm.ProcessForm(request, contactForm, Parameters))
            {
               request.IpAddress = Request.UserHostAddress;

               // Si el objeto es válido se realiza la acción
               ClassifiedAdsDAO ads = new ClassifiedAdsDAO(Workspace);
               ads.SendContactRequest(request);
            }
         }
         catch (CommunicationsException)
         {
            Content.Add(new AlertControl(this, "Se ha producido un problema al enviar el mensaje de contacto.", ComponentColorScheme.Error));
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
   }
}
