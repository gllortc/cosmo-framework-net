//---------------------------------------------------------------------------------
// COSMO SERVICES / JavaScript Client
//---------------------------------------------------------------------------------
// Version : 4.0.0
// Autor   : Gerard Llort
//---------------------------------------------------------------------------------
// Requires:
// - Cosmo.Core 3.5+ (HTML Bootstrap Components)
// - jQuery 2.0+
//---------------------------------------------------------------------------------

//---------------------------------------------------------------------------------
// UI Services
//---------------------------------------------------------------------------------

var cosmoUIServices = {

   //==================================================================
   // Carga una plantilla en un determinado contenedor del DOM.
   //------------------------------------------------------------------
   // url:       URL de llamada a la plantilla.
   // elementId: ID (DOM) del elemento que debe contener la vista.
   //==================================================================
   loadTemplate: function (url, elementId) {
      $('#' + elementId).load(url, function (response, status, xhr) 
      {
         // Load complete!
      });
   },

   sendForm: function (formId) 
   {
      $("#" + formId).submit(function(e)
      {
         var postData   = $(this).serializeArray();
         var formURL    = $(this).attr("action");

         $.ajax(
         {
            url :    formURL,
            type:    "POST",
            data :   postData,
            success: function(data, textStatus, jqXHR) 
            {
               //data: return data from server
            },
            error:   function(jqXHR, textStatus, errorThrown) 
            {
               //if fails      
            }
         });
         e.preventDefault(); //STOP default action
         e.unbind(); //unbind. to stop multiple form submit.
      });
   },

   sendMultipartForm: function (formId)
   {
      $('#' + formId).submit(function (e) 
      {
         var formObj    = $(this);
         var formURL    = formObj.attr("action");
         var formData   = new FormData(this);

         $.ajax({
            url:           formURL,
            type:          'POST',
            data:          formData,
            mimeType:      "multipart/form-data",
            contentType:   false,
            cache:         false,
            processData:   false,
            success:       function(data, textStatus, jqXHR)
            {
 
            },
            error:         function(jqXHR, textStatus, errorThrown) 
            {
            }
         });
         e.preventDefault(); //Prevent Default action. 
         e.unbind();
      });
   }

};

//---------------------------------------------------------------------------------
// Communication Services
//---------------------------------------------------------------------------------

var cosmoCommServices = {

   //==================================================================
   // Envia un mensaje privado a un usuario determinado.
   //------------------------------------------------------------------
   // formId:  ID (DOM) del formulario de envio de mensajes.
   // sendBtn: ID (DOM) del elemento que recibe el clic de enviar (usualmente un botón).
   // usrId:   Identificador del usuario destinatario del mensaje.
   //==================================================================
   sendPMsg: function(formId,sendBtn,usrId)
   {
      $('#' + sendBtn).click(function () {
         var sendBtn = $(this);
         sendBtn.button('loading');
         console.log("cosmoCommServices: Sending PM message...");
         var postData = $('#' + formId).serializeArray();
         $.ajax({
            url:      'CommService',
            type:     'post',
            dataType: 'json',
            data: postData,
            success: function (data, textStatus, jqXHR) {
               if (data.response = "ok") {
                  console.log("cosmoCommServices: PM message sended successfully!");           
                  console.log("cosmoCommServices: Reloading thread...");
                  $("#pmChatMsgs").load('CommService?cmd=umgth&usrid=' + usrId);
                  // $('#pmChatMsgs').slimScroll({ scrollBy: '300px' });
                  $("#body").val('');
                  console.log("cosmoCommServices: Reload complete. Process finished.");
               }
               else {
                  console.log("cosmoCommServices: Server error sending PM message.");
                  BootstrapDialog.show({
                     type: BootstrapDialog.TYPE_DANGER,
                     title: 'Mensajes Privados',
                     message: 'Se ha producido un error durante la petición de borrado al servidor. Vuelva a intentar la operación en unos instantes.',
                     buttons: [{
                        label: 'Aceptar',
                        action: function (dialog) {
                           dialog.close();
                        }
                     }]
                  });
               }
            },
            error: function (jqXHR, textStatus, errorThrown) {
               alert("error: " + textStatus);
            },
            complete: function () {
               sendBtn.button('reset');
               document.getElementById("pmChatMsgs").scrollTop = document.getElementById("pmChatMsgs").scrollHeight;
            }
         });
         document.getElementById("pmChatMsgs").scrollTop = document.getElementById("pmChatMsgs").scrollHeight;
         return false;
      });
      document.getElementById("pmChatMsgs").scrollTop = document.getElementById("pmChatMsgs").scrollHeight;
   },

   //==================================================================
   // Elimina un determinado mensaje privado del usuario autenticado.
   //------------------------------------------------------------------
   // siteUrl: URL del comando REST (con parámetros) de eliminación.
   // msgId:   Identificador del mensaje a eliminar.
   //==================================================================
   deletePMsg: function(siteUrl, msgId)
   {
      BootstrapDialog.show({
         type: BootstrapDialog.TYPE_WARNING,
         title: 'Mensajes Privados',
         message: '¿Realmente desea eliminar el mensaje de forma permanente?',
         buttons: [{
            label: 'Eliminar',
            action: function(dialog) {
               console.log("cosmoCommServices: Deleting PM message #" + msgId + "...");
               var jqxhr = jQuery.getJSON(siteUrl, function (data) {
                  if (data.response = "ok") {
                     console.log("cosmoCommServices: PM message #" + msgId + " deleted successfully!");
                     jQuery('#chatMsg' + msgId).remove();
                  }
                  else {
                     console.log("cosmoCommServices: Server error deleting PM message #" + msgId + ".");
                     BootstrapDialog.show({
                        type: BootstrapDialog.TYPE_DANGER,
                        title: 'Mensajes Privados',
                        message: 'Se ha producido un error durante la petición de borrado al servidor. Vuelva a intentar la operación en unos instantes.',
                        buttons: [{
                           label: 'Aceptar',
                           action: function (dialog) {
                              dialog.close();
                           }
                        }]
                     });
                  }
                  dialog.close();
               })
               .fail(function (jqxhr, textStatus, error) {
                  var err = textStatus + ', ' + error;
                  console.log("cosmoCommServices: [ERROR] deleting PM message #" + msgId + " (getJSON failed): " + err);
                  BootstrapDialog.show({
                     type: BootstrapDialog.TYPE_DANGER,
                     title: 'Mensajes Privados',
                     message: 'Se ha producido un error durante la petición de borrado al servidor. Vuelva a intentar la operación en unos instantes.',
                     buttons: [{
                        label: 'Aceptar',
                        action: function (dialog) {
                           dialog.close();
                        }
                     }]
                  });
               });
            }
         }, {
            label: 'Cancelar',
            action: function(dialog) {
               dialog.close();
            }
         }]
      });
   },

   //==================================================================
   // Abre una conversa con el ID de usuario especificado.
   //------------------------------------------------------------------
   // elementId: ID (DOM) del elemento que contiene el ID de usuario.
   //==================================================================
   openPMsgThread: function(elementId)
   {
      window.location.href = 'usrmsg.aspx?th=' + $('#' + elementId).val();
   }

};