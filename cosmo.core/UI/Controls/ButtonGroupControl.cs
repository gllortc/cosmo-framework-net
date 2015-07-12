﻿using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa una barra de herramientas.
   /// http://getbootstrap.com/components/#btn-groups
   /// </summary>
   public class ButtonGroupControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ButtonGroupControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public ButtonGroupControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si la barra está alineada verticalmente.
      /// </summary>
      public bool Vertical { get; set; }

      /// <summary>
      /// Gets or sets el tamaño que deben tener los botones de la barra.
      /// </summary>
      public ButtonControl.ButtonSizes Size { get; set; }

      /// <summary>
      /// Contiene la lista de botones de la barra de herramientas.
      /// </summary>
      public List<ButtonControl> Buttons { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Vertical = false;
         this.Size = ButtonControl.ButtonSizes.Default;
         this.Buttons = new List<ButtonControl>();
      }

      #endregion

   }
}
