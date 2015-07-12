using Cosmo.Utils;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente Panel.
   /// Detalles del componente: http://getbootstrap.com/components/#panels
   /// </summary>
   public class PanelControl : Control, IControlSingleContainer
   {
      // Internal data declarations
      private string _caption;
      private string _captionIcon;
      private ControlCollection _controls;
      private string _contents;
      private string _contentId;
      private ControlCollection _footer;
      private ButtonGroupControl _buttons;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PanelControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public PanelControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="PanelControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public PanelControl(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region properties

      /// <summary>
      /// Gets or sets el título del control.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Gets or sets el código del icono que se desea mostrar al inicio del título.
      /// </summary>
      public string CaptionIcon
      {
         get { return _captionIcon; }
         set { _captionIcon = value; }
      }

      /// <summary>
      /// Gets or sets el contenido XHTML del panel.
      /// </summary>
      public string ContentXhtml
      {
         get { return _contents; }
         set { _contents = value; }
      }

      /// <summary>
      /// Gets or sets el ID (DOM) del contenido del panel.
      /// </summary>
      public string ContentDomId
      {
         get { return _contentId; }
         set { _contentId = value; }
      }

      /// <summary>
      /// Gets or sets la lista de controles contenidos dentro del control.
      /// </summary>
      public ControlCollection Content
      {
         get { return _controls; }
         set { _controls = value; }
      }

      /// <summary>
      /// Gets or sets la lista de componentes contenidos en el pie.
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
            if (_buttons == null) _buttons = new ButtonGroupControl(this.ParentView);
            return _buttons;
         }
         set { _buttons = value; }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
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

      #endregion

   }
}
