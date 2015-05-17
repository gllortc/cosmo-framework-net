/**
* Basic sample plugin inserting abbreviation elements into CKEditor editing area.
*/

// Register the plugin with the editor.
// http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.plugins.html
CKEDITOR.plugins.add('youtube',
{
    // The plugin initialization logic goes inside this method.
    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.pluginDefinition.html#init
    init: function(editor) {
        // Define an editor command that inserts an abbreviation. 
        // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.editor.html#addCommand
        editor.addCommand('youtubeDialog', new CKEDITOR.dialogCommand('youtubeDialog'));
        // Create a toolbar button that executes the plugin command. 
        // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.html#addButton
        editor.ui.addButton('YouTube',
		{
		    // Toolbar button tooltip.
		    label: 'Insertar video YouTube',
		    // Reference to the plugin command name.
		    command: 'youtubeDialog',
		    // Button's icon file path.
		    icon: this.path + 'images/icon.png'
		});
        // Add a dialog window definition containing all UI elements and listeners.
        // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.html#.add
        CKEDITOR.dialog.add('youtubeDialog', function(editor) {
            return {
                // Basic properties of the dialog window: title, minimum size.
                // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.dialogDefinition.html
                title: 'Insertar video YouTube',
                minWidth: 400,
                minHeight: 200,
                // Dialog window contents.
                // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.definition.content.html
                contents:
				[
					{
					    // Definition of the Basic Settings dialog window tab (page) with its id, label, and contents.
					    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.contentDefinition.html
					    id: 'tab1',
					    label: 'Propiedades del video',
					    elements:
						[
							{
							    // Dialog window UI element: a text input field for the abbreviation text.
							    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.textInput.html
							    type: 'text',
							    id: 'url',
							    // Text that labels the field.
							    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.ui.dialog.labeledElement.html#constructor
							    label: 'URL del video',
							    // Validation checking whether the field is not empty.
							    validate: CKEDITOR.dialog.validate.notEmpty("Debe especificar la URL del video")
							}
						]
					}
				],
                // This method is invoked once a user closes the dialog window, accepting the changes.
                // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.dialogDefinition.html#onOk
                onOk: function() {
                    // A dialog window object.
                    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.html 
                    var dialog = this;
                    // Create a new abbreviation element and an object that will hold the data entered in the dialog window.
                    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dom.document.html#createElement
                    var abbr = editor.document.createElement('span');

                    // Retrieve the value of the "title" field from the "tab1" dialog window tab.
                    // Send it to the created element as the "title" attribute.
                    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dom.element.html#setAttribute
                    // abbr.setAttribute('title', dialog.getValueOf('tab1', 'url'));
                    // Set the element's text content to the value of the "abbr" dialog window field.
                    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dom.element.html#setText
                    abbr.setText('[video]' + dialog.getValueOf('tab1', 'url') + '[/video]');

                    // Retrieve the value of the "id" field from the "tab2" dialog window tab.
                    // If it is not empty, send it to the created abbreviation element. 
                    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.dialog.html#getValueOf
                    // var id = dialog.getValueOf('tab2', 'id');
                    // if (id) abbr.setAttribute('id', id);

                    // Insert the newly created abbreviation into the cursor position in the document.					
                    // http://docs.cksource.com/ckeditor_api/symbols/CKEDITOR.editor.html#insertElement
                    editor.insertElement(abbr);
                }
            };
        });
    }
});