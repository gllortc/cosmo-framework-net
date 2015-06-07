namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implemnenta un botón insertable en la barra de herramientas del editor de mensajes del chat.
   /// </summary>
   public class ChatMessageToolbarButtonControl : Control
   {
      // Declaración de variables internas
      private string _caption;
      private string _href;
      private string _icon;

      /// <summary>
      /// Devuelve una instancia de <see cref="ChatMessageToolbarButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public ChatMessageToolbarButtonControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ChatMessageToolbarButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="caption"></param>
      /// <param name="href"></param>
      public ChatMessageToolbarButtonControl(View parentView, string caption, string href)
         : base(parentView)
      {
         Initialize();

         _caption = caption;
         _href = href;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="ChatMessageToolbarButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="caption"></param>
      /// <param name="href"></param>
      /// <param name="icon"></param>
      public ChatMessageToolbarButtonControl(View parentView, string caption, string href, string icon)
         : base(parentView)
      {
         Initialize();

         _caption = caption;
         _href = href;
         _icon = icon;
      }

      /// <summary>
      /// Devuelve o establece la etiqueta visible asociada al control.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Devuelve o establece la URL (o llamada JavaScript) correspondiente al link del botón.
      /// </summary>
      public string Href
      {
         get { return _href; }
         set { _href = value; }
      }

      /// <summary>
      /// Devuelve o establece el código del icono a mostrar.
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
         _caption = string.Empty;
         _href = string.Empty;
         _icon = string.Empty;
      }
   }
}
