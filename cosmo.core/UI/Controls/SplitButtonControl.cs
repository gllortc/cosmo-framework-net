﻿using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un botón que al ser pulsado muestra un menú de opciones.
   /// </summary>
   public class SplitButtonControl : ButtonControl
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="SplitButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      public SplitButtonControl(View parentView) 
         : base(parentView)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Contiene la lista de opciones que se presentarán al desplegar el menú.
      /// </summary>
      public List<ButtonControl> MenuOptions { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.MenuOptions = new List<ButtonControl>();
      }

      #endregion

   }
}
