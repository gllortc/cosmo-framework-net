namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un control para incrustar en la cabecera de un documento o página y que permite mostrar
   /// el título del contenido, un breadcrumb, etc.
   /// </summary>
   public class PageHeaderControl : Control
   {
      // Declaración de variables internas
      private string _title;
      private string _subtitle;
      private string _icon;
      private BreadcrumbControl _breadcrumb;

      /// <summary>
      /// Devuelve una instancia de <see cref="PageHeaderControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public PageHeaderControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve o establece el título de la página.
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Devuelve o establece el subtítulo de la página.
      /// </summary>
      public string SubTitle
      {
         get { return _subtitle; }
         set { _subtitle = value; }
      }

      /// <summary>
      /// Devuelve o establece el icono que acompaña el título.
      /// </summary>
      public string Icon
      {
         get { return _icon; }
         set { _icon = value; }
      }

      /// <summary>
      /// Devuelve o establece el componente que permite integrar una ruta a la cabecera de página.
      /// </summary>
      public BreadcrumbControl Breadcrumb
      {
         get { return _breadcrumb; }
         set { _breadcrumb = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _title = string.Empty;
         _subtitle = string.Empty;
         _icon = string.Empty;
         _breadcrumb = null;
      }
   }
}
