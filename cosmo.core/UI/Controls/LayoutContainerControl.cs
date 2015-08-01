using System;
using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implements a page layout container.
   /// </summary>
   public class LayoutContainerControl : Control, IControlContainer
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="LayoutContainerControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public LayoutContainerControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si se debe mostrar el fondo del <em>layout</em> descolorido (o en un color alternativo).
      /// Esto permite, por ejemplo, mostrar páginas con un solo control como <em>Login</em>, <em>Registro</em>, etc.
      /// </summary>
      public bool FadeBackground { get; set; }

      /// <summary>
      /// Gets or sets la lista de controles que contiene la cabecera.
      /// </summary>
      public ControlCollection Header { get; set; }

      /// <summary>
      /// Gets or sets la lista de controles que contiene el pie.
      /// </summary>
      public ControlCollection Footer { get; set; }

      /// <summary>
      /// Gets or sets la lista de controles que contiene la columna izquierda.
      /// </summary>
      public ControlCollection LeftContent { get; set; }

      /// <summary>
      /// Gets or sets la lista de controles que contiene la zona de contenidos de la página.
      /// </summary>
      public ControlCollection MainContent { get; set; }

      /// <summary>
      /// Gets or sets la lista de controles que contiene la columna derecha.
      /// </summary>
      public ControlCollection RightContent { get; set; }

      /// <summary>
      /// Indica si se trata de una página que sólo muestra un control.
      /// Las páginas de un sólo control permiten, por ejemplo, implementar formularios, login, registro, etc.
      /// </summary>
      public bool IsSingleControlLayout
      {
         get 
         {
            int ctrls = 0;

            ctrls += Header.Count;
            ctrls += Footer.Count;
            ctrls += LeftContent.Count;
            ctrls += MainContent.Count;
            ctrls += RightContent.Count;

            return (ctrls == 1);
         }
      }

      #endregion

      #region IControlContainer Implementation

      /// <summary>
      /// Gets all controls of a concrete type.
      /// </summary>
      /// <param name="controlType">Type of control.</param>
      /// <returns>A list of requested controls.</returns>
      public List<Control> GetControlsByType(Type controlType)
      {
         List<Control> controls = new List<Control>();

         // Obtiene el formulario
         controls.AddRange(MainContent.GetControlsByType(controlType));
         controls.AddRange(LeftContent.GetControlsByType(controlType));
         controls.AddRange(RightContent.GetControlsByType(controlType));

         return controls;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.FadeBackground = false;
         this.Header = new ControlCollection();
         this.LeftContent = new ControlCollection();
         this.RightContent = new ControlCollection();
         this.MainContent = new ControlCollection();
         this.Footer = new ControlCollection();
      }

      #endregion

   }
}
