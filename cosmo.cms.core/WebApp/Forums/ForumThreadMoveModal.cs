﻿using Cosmo.Cms.Forums;
using Cosmo.UI;
using Cosmo.UI.Controls;

namespace Cosmo.WebApp.Forums
{
   /// <summary>
   /// Formulario modal que permite mover un determinado thread a un canal 
   /// distinto al que se encuentra actualmente.
   /// </summary>
   [ViewParameter(ParameterName = ForumsDAO.PARAM_THREAD_ID,
                  PropertyName = "ThreadID")]
   [ViewParameter(ParameterName = ForumsDAO.PARAM_CHANNEL_ID,
                  PropertyName = "ChannelID")]
   public class ForumThreadMoveModal : ModalViewContainer
   {
      // Modal element unique identifier
      private const string DOM_ID = "forum-thread-move-modal";

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="ForumThreadMoveModal"/>.
      /// </summary>
      public ForumThreadMoveModal() : base() 
      {
         this.DomID = ForumThreadMoveModal.DOM_ID;
      }

      /// <summary>
      /// Gets an instance of <see cref="ForumThreadMoveModal"/>.
      /// </summary>
      /// <param name="threadId">Thread identifier.</param>
      /// <param name="channelId">Current thread channel identifier.</param>
      public ForumThreadMoveModal(int threadId, int channelId) : base() 
      {
         this.DomID = ForumThreadMoveModal.DOM_ID;
         this.ChannelID = channelId;
         this.ThreadID = threadId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del thread que se desea eliminar.
      /// </summary>
      public int ThreadID { get; set; }

      /// <summary>
      /// Devuelve o establece el identificador del thread que se desea eliminar.
      /// </summary>
      public int ChannelID { get; set; } 

      #endregion

      #region ModalViewContainer Implementation

      public override void InitPage()
      {
         Title = "Mover thread a otro canal";
         Closeable = true;
         Icon = IconControl.ICON_FOLDER_OPEN;

         FormControl form = new FormControl(this, "frmMoveThread");
         form.UsePanel = false;
         form.SendDataMethod = FormControl.FormSendDataMethod.JSSubmit;
         form.AddFormSetting(ForumsDAO.PARAM_THREAD_ID, this.ThreadID);
         form.FormButtons.Add(new ButtonControl(this, "cmdAccept", "Mover", ButtonControl.ButtonTypes.Submit));
         form.FormButtons.Add(new ButtonControl(this, "cmdClose", "Cancelar", ButtonControl.ButtonTypes.CloseModalForm));

         ForumsDAO fdao = new ForumsDAO(Workspace);
         FormFieldList lstChannels = new FormFieldList(this, ForumsDAO.PARAM_CHANNEL_ID, "Canal de destino", FormFieldList.ListType.Single);
         lstChannels.Required = true;
         lstChannels.Values = ForumsDAO.ConvertToKeyValueList(fdao.GetForums());
         form.Content.Add(lstChannels);

         Content.Add(form);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         // Obtiene los parámetros de la llamada.
         int threadId = receivedForm.GetIntFieldValue(ForumsDAO.PARAM_THREAD_ID);
         int channelId = receivedForm.GetIntFieldValue(ForumsDAO.PARAM_CHANNEL_ID);

         Content.Clear();

         try
         {
            // Ejecuta el método para mover el hilo
            ForumsDAO fdao = new ForumsDAO(Workspace);
            fdao.MoveThread(threadId, channelId);

            CalloutControl callout = new CalloutControl(this);
            callout.Title = "Operación completada con éxito";
            callout.Icon = IconControl.ICON_CHECK;
            callout.Text = "El hilo se ha movido con éxito.";
            callout.Type = ComponentColorScheme.Success;

            Content.Add(callout);
         }
         catch
         {
            CalloutControl callout = new CalloutControl(this);
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

   }
}
