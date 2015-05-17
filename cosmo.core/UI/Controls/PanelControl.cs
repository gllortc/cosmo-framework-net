using Cosmo.Utils;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente Panel.
   /// Detalles del componente: http://getbootstrap.com/components/#panels
   /// </summary>
   public class PanelControl : Control, IControlSingleContainer
   {
      // Declaración de variables internas
      private string _caption;
      private string _captionIcon;
      private ControlCollection _controls;
      private string _contents;
      private string _contentId;
      private ControlCollection _footer;
      private ButtonGroupControl _buttons;

      /// <summary>
      /// Devuelve una instancia de <see cref="PanelControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public PanelControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="PanelControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador único del componente dentro de la vista.</param>
      public PanelControl(ViewContainer container, string domId)
         : base(container, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve o establece el título del control.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Devuelve o establece el código del icono que se desea mostrar al inicio del título.
      /// </summary>
      public string CaptionIcon
      {
         get { return _captionIcon; }
         set { _captionIcon = value; }
      }

      /// <summary>
      /// Devuelve o establece el contenido XHTML del panel.
      /// </summary>
      public string ContentXhtml
      {
         get { return _contents; }
         set { _contents = value; }
      }

      /// <summary>
      /// Devuelve o establece el ID (DOM) del contenido del panel.
      /// </summary>
      public string ContentDomId
      {
         get { return _contentId; }
         set { _contentId = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de controles contenidos dentro del control.
      /// </summary>
      public ControlCollection Content
      {
         get { return _controls; }
         set { _controls = value; }
      }

      /// <summary>
      /// Devuelve o establece la lista de componentes contenidos en el pie.
      /// </summary>
      public ControlCollection Footer
      {
         get { return _footer; }
         set { _footer = value; }
      }

      /// <summary>
      /// Barra de herramientas de la cabecera.
      /// </summary>
      public ButtonGroupControl ButtonBar
      {
         get 
         {
            if (_buttons == null) _buttons = new ButtonGroupControl(this.Container);
            return _buttons;
         }
         set { _buttons = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _caption = string.Empty;
         _captionIcon = string.Empty;
         _contents = string.Empty;
         _controls = new ControlCollection();
         _footer = new ControlCollection();
         _buttons = null;
      }
   }
}
