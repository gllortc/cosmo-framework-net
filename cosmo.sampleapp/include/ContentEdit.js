//=======================================================
// JS correspondiente a la página ContentEdit
//=======================================================

function deleteFile(rowId, objId, fileName)
{
   // Conformación previa antes del borrado
   bootbox.confirm("¿Realmente desea eliminar el archivo " + fileName + " de forma permanente?", function (result)
   {
      if (result == true)
      {
         $.ajax({
            url: 'APIFileSystem',
            data: {
               _cmd_: '_del_',
               _fn_: fileName,
               oid: objId
            },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
               if (data.response = "ok") {
                  console.log("ContentEdit: File [" + fileName + "] deleted!");
                  bootbox.alert("Archivo eliminado correctamente!");
                  $('#' + rowId).fadeOut(400, function () {
                     $('#' + rowId).remove();
                  });
               }
               else {
                  console.log("ContentEdit: Server Error deleting file!");
                  bootbox.alert("Se ha producido un error al intentar eliminar el archivo.");
               }
            },
            error: function (jqXHR, textStatus, errorThrown) {
               bootbox.alert("Se ha producido un error al intentar eliminar el archivo.<br /><br />ERROR:" + textStatus);
            }
         });
      }
   });
}

function CopyFileUrl(url)
{
   bootbox.dialog({
      message: "URL del archivo para usar con el editor de contenido (Ctrl + C):<br/><br/><strong>" + url + "</strong>",
      title:   "Copiar URL del archivo",
      buttons: {
         success: {
            label: "Cerrar",
            className: "btn-primary"
         }
      }
   });
}