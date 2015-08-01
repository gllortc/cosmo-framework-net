using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un elemento del componente <see cref="TreeViewControl"/>.
   /// </summary>
   public class TreeViewChildItemControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ListItem"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public TreeViewChildItemControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ListItem"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public TreeViewChildItemControl(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el título visible del elemento.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Gets or sets el texto descriptivo del elemento.
      /// No soporta XHTML, sólo texto.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets la URL asociada al elemento.
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Gets or sets el código del icono a mostrar junto al título del elemento.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Gets or sets el texto que aparecerá como <em>badge</em> (usualmente para indicar, por ejemplo, el número de elementos en un categoria).
      /// </summary>
      public string BadgeText { get; set; }

      /// <summary>
      /// Indica si el elemento debe mostrarse como activo (resaltado).
      /// </summary>
      public bool IsActive { get; set; }

      /// <summary>
      /// Gets or sets el estilo de marcado del elemento.
      /// </summary>
      public ComponentColorScheme Type { get; set; }

      internal ListGroupControl.ListGroupStyle Style { get; set; }

      /// <summary>
      /// Gets or sets la lista de hijos de la rama actual.
      /// </summary>
      public List<TreeViewChildItemControl> ChildItems { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Caption = string.Empty;
         this.Description = string.Empty;
         this.BadgeText = string.Empty;
         this.Href = string.Empty;
         this.Icon = string.Empty;
         this.IsActive = false;
         this.Type = ComponentColorScheme.Normal;
         this.Style = ListGroupControl.ListGroupStyle.Simple;
         this.ChildItems = new List<TreeViewChildItemControl>();
      }

      #endregion

   }
}
