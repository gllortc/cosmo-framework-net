using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Representa un elemento insertable en un control <see cref="TimelineControl"/>.
   /// </summary>
   public class TimelineItem
   {

      #region Enumerations

      /// <summary>
      /// Enumera los tipos de elemento que se pueden insertar en un control <see cref="TimelineControl"/>.
      /// </summary>
      public enum TimelineItemType
      {
         /// <summary>Etiqueta.</summary>
         Label,
         /// <summary>Entrada en el diario.</summary>
         Entry
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="TimelineItem"/>.
      /// </summary>
      public TimelineItem()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del elemento.
      /// Algunos renderizadores usan esta propiedad para identificar al elemento de forma única y 
      /// acceder a és mediante JavaScript.
      /// </summary>
      public string ID { get; set; }

      /// <summary>
      /// Gets or sets el título del elemento.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or sets the control which will be rendered as the title.
      /// </summary>
      public Control TitleControl { get; set; }

      /// <summary>
      /// Gets or sets el texto que indica el tiempo de publicación.
      /// </summary>
      public string Time { get; set; }

      /// <summary>
      /// Gets or sets el contenido del elemento.
      /// </summary>
      public string Body { get; set; }

      /// <summary>
      /// Gets or sets el contenido del elemento.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Gets or sets el código del icono a mostrar junto al elemento.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Gets or sets el color del icono a mostrar junto al elemento.
      /// </summary>
      public ComponentColorScheme Color { get; set; }

      /// <summary>
      /// Gets or sets el color de fondo del elemento.
      /// </summary>
      public ComponentBackgroundColor BackgroundColor { get; set; }

      /// <summary>
      /// Gets or sets el tipo de elemento a representar.
      /// </summary>
      public TimelineItemType Type { get; set; }

      /// <summary>
      /// Permite acceder a la barra de botones que se situa al lado del marcador de tiempo.
      /// </summary>
      public List<ButtonControl> Buttons { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.ID = string.Empty;
         this.Title = string.Empty;
         this.Time = string.Empty;
         this.Body = string.Empty;
         this.Label = string.Empty;
         this.Icon = Cosmo.UI.Controls.IconControl.ICON_USER;
         this.Color = ComponentColorScheme.Information;
         this.BackgroundColor = ComponentBackgroundColor.None;
         this.Type = TimelineItemType.Entry;
         this.Buttons = new List<ButtonControl>();
         this.TitleControl = null;
      }

      #endregion

   }
}
