using System.Text;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente Alert.
   /// </summary>
   public class AlertControl : Control
   {
      // Declaración de variables internas
      private ComponentColorScheme _type;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public AlertControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="text"></param>
      /// <param name="type"></param>
      public AlertControl(ViewContainer container, string text, ComponentColorScheme type)
         : base(container)
      {
         Initialize();

         this.Text = text;
         this.Type = type;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="text"></param>
      /// <param name="type"></param>
      /// <param name="closeable"></param>
      public AlertControl(ViewContainer container, string text, ComponentColorScheme type, bool closeable)
         : base(container)
      {
         Initialize();

         this.Text = text;
         this.Type = type;
         this.Closeable = closeable;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el texto visible del elemento.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Indica si el cuadro de se puede cerrar.
      /// </summary>
      public bool Closeable { get; set; }

      /// <summary>
      /// Devuelve o establece el tipo de alerta que se va a mostrar.
      /// </summary>
      public ComponentColorScheme Type
      {
         get { return _type; }
         set
         {
            if (value == ComponentColorScheme.Normal)
            {
               _type = ComponentColorScheme.Information;
            }
            else
            {
               _type = value;
            }
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Text = string.Empty;
         this.Closeable = false;
         _type = ComponentColorScheme.Information;
      }

      #endregion

   }
}
