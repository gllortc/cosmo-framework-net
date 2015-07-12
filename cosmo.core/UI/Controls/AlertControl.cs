using System.Text;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente Alert.
   /// </summary>
   public class AlertControl : Control
   {
      // Internal data declarations
      private ComponentColorScheme _type;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public AlertControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="text"></param>
      /// <param name="type"></param>
      public AlertControl(View parentView, string text, ComponentColorScheme type)
         : base(parentView)
      {
         Initialize();

         this.Text = text;
         this.Type = type;
      }

      /// <summary>
      /// Gets a new instance of <see cref="AlertControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="text"></param>
      /// <param name="type"></param>
      /// <param name="closeable"></param>
      public AlertControl(View parentView, string text, ComponentColorScheme type, bool closeable)
         : base(parentView)
      {
         Initialize();

         this.Text = text;
         this.Type = type;
         this.Closeable = closeable;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el texto visible del elemento.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Indica si el cuadro de se puede cerrar.
      /// </summary>
      public bool Closeable { get; set; }

      /// <summary>
      /// Gets or sets el tipo de alerta que se va a mostrar.
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
      /// Initializes the instance data.
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
