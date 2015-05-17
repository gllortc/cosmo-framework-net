using Cosmo.Utils;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una pestaña que puede contener controles.
   /// </summary>
   public class TabItemControl : Control, IControlSingleContainer
   {
      // Declaración de variables internas
      private bool _active;
      private string _caption;
      private string _icon;
      private ControlCollection _controls;
      private ComponentColorScheme _color;

      /// <summary>
      /// Devuelve una instancia de <see cref="TabbedContainerControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public TabItemControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="TabbedContainerControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id"></param>
      /// <param name="caption"></param>
      public TabItemControl(ViewContainer container, string id, string caption)
         : base(container)
      {
         Initialize();

         this.DomID = id;
         this.Caption = caption;
      }

      /// <summary>
      /// Devuelve o establece el texto visible de la pestaña.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Indica si es la pestaña activa.
      /// </summary>
      public bool Active
      {
         get { return _active; }
         set { _active = value; }
      }

      /// <summary>
      /// Devuelve o establece el color de la pestaña.
      /// </summary>
      public ComponentColorScheme Color
      {
         get { return _color; }
         set { _color = value; }
      }

      /// <summary>
      /// Lista de controles que contiene la pestaña.
      /// </summary>
      public ControlCollection Content
      {
         get { return _controls; }
         set { _controls = value; }
      }

      /// <summary>
      /// Devuelve o establece código del icono que aparecerá junto al título de la pestaña.
      /// </summary>
      public string Icon
      {
         get { return _icon; }
         set { _icon = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _active = false;
         _caption = string.Empty;
         _icon = string.Empty;
         _controls = new ControlCollection();
         _color = ComponentColorScheme.Normal;
      }
   }
}
