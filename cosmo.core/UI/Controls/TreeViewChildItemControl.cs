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
      /// Devuelve una instancia de <see cref="ListItem"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public TreeViewChildItemControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ListItem"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Identificador del componente en una vista (DOM).</param>
      public TreeViewChildItemControl(ViewContainer container, string domId)
         : base(container, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el título visible del elemento.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Devuelve o establece el texto descriptivo del elemento.
      /// No soporta XHTML, sólo texto.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Devuelve o establece la URL asociada al elemento.
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Devuelve o establece el código del icono a mostrar junto al título del elemento.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Devuelve o establece el texto que aparecerá como <em>badge</em> (usualmente para indicar, por ejemplo, el número de elementos en un categoria).
      /// </summary>
      public string BadgeText { get; set; }

      /// <summary>
      /// Indica si el elemento debe mostrarse como activo (resaltado).
      /// </summary>
      public bool IsActive { get; set; }

      /// <summary>
      /// Devuelve o establece el estilo de marcado del elemento.
      /// </summary>
      public ComponentColorScheme Type { get; set; }

      internal ListGroupControl.ListGroupStyle Style { get; set; }

      /// <summary>
      /// Devuelve o establece la lista de hijos de la rama actual.
      /// </summary>
      public List<TreeViewChildItemControl> ChildItems { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
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
