﻿using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el componente <strong>Navbar</strong> de <em>Bootstrap</em>.
   /// </summary>
   public class NavbarControl : Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="NavbarControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      public NavbarControl(ViewContainer container)
         : base(container)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="NavbarControl"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="fixedTop">Indica si la barra debe permanecer fija en la parte superior de la pantalla.</param>
      public NavbarControl(ViewContainer container, bool fixedTop)
         : base(container)
      {
         Initialize();

         this.IsFixedTop = fixedTop;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Cabecera de la barra de navegación. Contiene básicamente el nombre (o logo) del site y un enlñace a la página de inicio.
      /// </summary>
      public NavbarHeaderControl Header { get; set; }

      /// <summary>
      /// Indica si la barra debe permanecer fija en la parte superior de la pantalla.
      /// </summary>
      public bool IsFixedTop { get; set; }

      /// <summary>
      /// Lista de elementos que contiene la barra de navegación.
      /// </summary>
      public List<NavbarIButtonControl> Items { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega un nuevo elemento en la barra de navegación.
      /// </summary>
      /// <param name="item">Una instancia de <see cref="NavbarIButtonControl"/> que representa la cabecera de la barra.</param>
      public void AddItem(NavbarIButtonControl item)
      {
         if (item == null) return;
         if (this.Items == null) this.Items = new List<NavbarIButtonControl>();

         this.Items.Add(item);
      }

      /// <summary>
      /// Agrega un nuevo elemento en la barra de navegación.
      /// </summary>
      /// <param name="item">Una instancia de <see cref="NavbarIButtonControl"/> que representa la cabecera de la barra.</param>
      /// <param name="align">Posición en que se deve alinear el elemento en la barra.</param>
      public void AddItem(NavbarIButtonControl item, NavbarItemPosition align)
      {
         if (item == null) return;
         if (this.Items == null) this.Items = new List<NavbarIButtonControl>();

         item.Position = align;
         this.Items.Add(item);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.IsFixedTop = false;
         this.Header = null;
         this.Items = new List<NavbarIButtonControl>();
      }

      #endregion

   }
}
