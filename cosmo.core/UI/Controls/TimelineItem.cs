using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Representa un elemento insertable en un control <see cref="TimelineControl"/>.
   /// </summary>
   public class TimelineItem
   {
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

      // Declaración de variables internas
      private string _id;
      private string _title;
      private string _time;
      private string _body;
      private string _icon;
      private string _label;
      private ComponentColorScheme _color;
      private ComponentBackgroundColor _bgColor;
      private TimelineItemType _type;
      private List<ButtonControl> _buttons;

      /// <summary>
      /// Devuelve una instancia de <see cref="TimelineItem"/>.
      /// </summary>
      public TimelineItem()
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve o establece el identificador del elemento.
      /// Algunos renderizadores usan esta propiedad para identificar al elemento de forma única y 
      /// acceder a és mediante JavaScript.
      /// </summary>
      public string ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Devuelve o establece el título del elemento.
      /// </summary>
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Gets or sets the control which will be rendered as the title.
      /// </summary>
      public Control TitleControl { get; set; }

      /// <summary>
      /// Devuelve o establece el texto que indica el tiempo de publicación.
      /// </summary>
      public string Time
      {
         get { return _time; }
         set { _time = value; }
      }

      /// <summary>
      /// Devuelve o establece el contenido del elemento.
      /// </summary>
      public string Body
      {
         get { return _body; }
         set { _body = value; }
      }

      /// <summary>
      /// Devuelve o establece el contenido del elemento.
      /// </summary>
      public string Label
      {
         get { return _label; }
         set { _label = value; }
      }

      /// <summary>
      /// Devuelve o establece el código del icono a mostrar junto al elemento.
      /// </summary>
      public string Icon
      {
         get { return _icon; }
         set { _icon = value; }
      }

      /// <summary>
      /// Devuelve o establece el color del icono a mostrar junto al elemento.
      /// </summary>
      public ComponentColorScheme Color
      {
         get { return _color; }
         set { _color = value; }
      }

      /// <summary>
      /// Devuelve o establece el color de fondo del elemento.
      /// </summary>
      public ComponentBackgroundColor BackgroundColor
      {
         get { return _bgColor; }
         set { _bgColor = value; }
      }

      /// <summary>
      /// Devuelve o establece el tipo de elemento a representar.
      /// </summary>
      public TimelineItemType Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Permite acceder a la barra de botones que se situa al lado del marcador de tiempo.
      /// </summary>
      public List<ButtonControl> Buttons
      {
         get { return _buttons; }
         set { _buttons = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _title = string.Empty;
         _time = string.Empty;
         _body = string.Empty;
         _icon = Cosmo.UI.Controls.IconControl.ICON_USER;
         _color = ComponentColorScheme.Information;
         _bgColor = ComponentBackgroundColor.None;
         _type = TimelineItemType.Entry;
         _buttons = new List<ButtonControl>();

         this.TitleControl = null;
      }
   }
}
