tinyMCEPopup.requireLangPack();

var ExampleDialog = {
   init: function() {
      var f = document.forms[0];

      // Get the selected contents as text and place it in the input
      f.someval.value = tinyMCEPopup.editor.selection.getContent({ format: 'text' });
      // f.somearg.value = tinyMCEPopup.getWindowArg('some_custom_arg');
   },

   insert: function() {
      var video_url = document.forms[0].someval.value; ;
      // var url = document.forms[0].someval.value;
      // video_url = url.replace("/(?<=\?v=)([a-zA-Z0-9_-])+/g", "$1");

      // Insert the contents from the input into the document
      tinyMCEPopup.editor.execCommand('mceInsertContent', true, '[video]' + video_url + '[/video] ');
      tinyMCEPopup.close();
   }
};

tinyMCEPopup.onInit.add(ExampleDialog.init, ExampleDialog);
