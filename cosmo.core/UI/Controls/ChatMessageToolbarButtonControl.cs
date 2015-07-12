namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implemnenta un botón insertable en la barra de herramientas del editor de mensajes del chat.
   /// </summary>
   public class ChatMessageToolbarButtonControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ChatMessageToolbarButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public ChatMessageToolbarButtonControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ChatMessageToolbarButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="caption"></param>
      /// <param name="href"></param>
      public ChatMessageToolbarButtonControl(View parentView, string caption, string href)
         : base(parentView)
      {
         Initialize();

         this.Caption = caption;
         this.Href = href;
      }

      /// <summary>
      /// Gets a new instance of <see cref="ChatMessageToolbarButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="caption"></param>
      /// <param name="href"></param>
      /// <param name="icon"></param>
      public ChatMessageToolbarButtonControl(View parentView, string caption, string href, string icon)
         : base(parentView)
      {
         Initialize();

         this.Caption = caption;
         this.Href = href;
         this.Icon = icon;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets la etiqueta visible asociada al control.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Gets or sets la URL (o llamada JavaScript) correspondiente al link del botón.
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Gets or sets el código del icono a mostrar.
      /// </summary>
      public string Icon { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Caption = string.Empty;
         this.Href = string.Empty;
         this.Icon = string.Empty;
      }

      #endregion

   }
}
