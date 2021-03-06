﻿using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Representa un elemento insertable en un componente Navbar de Bootstrap.
   /// </summary>
   public class NavbarIButtonControl : Control
   {
      // Internal data declarations
      private string _icon;
      private string _title;
      private string _href;
      private bool _isActive;
      private NavbarItemPosition _position;
      private NavbarItemType _type;
      private List<NavbarIButtonControl> _subItems;

      #region Enumerations

      /// <summary>
      /// Describe los sitintos tipos de elementos que puede contener una barra de navegación.
      /// </summary>
      public enum NavbarItemType
      {
         /// <summary>Botón normal (enlace) o menú desplegable.</summary>
         Button,
         /// <summary>Línea divisoria entre distintos elementos.</summary>
         Divider,
         /// <summary>Botón de inicio de sesión.</summary>
         Login
      }

      /// <summary>
      /// Describe los sitintos tipos de elementos que puede contener una barra de navegación.
      /// </summary>
      public enum NavbarItemPosition
      {
         /// <summary>Alineado a la izquierda.</summary>
         Left,
         /// <summary>Alineado a la derecha.</summary>
         Right
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="NavbarIButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      public NavbarIButtonControl(View parentView)
         : base(parentView)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="NavbarIButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="caption">Título visible del elemento.</param>
      /// <param name="href">Dirección URL que va asociado al elemento.</param>
      public NavbarIButtonControl(View parentView, string caption, string href)
         : base(parentView)
      {
         Initialize();

         _title = caption;
         _href = href;
      }

      /// <summary>
      /// Gets a new instance of <see cref="NavbarIButtonControl"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="isDivider">Indica si el elemento es un divisor.</param>
      public NavbarIButtonControl(View parentView, bool isDivider)
         : base(parentView)
      {
         Initialize();

         _type = NavbarItemType.Divider;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets el título visible del elemento.
      /// </summary>
      public string Caption
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Gets or sets la URL que se invocará al hacer clic en el elemento.
      /// </summary>
      public string Href
      {
         get { return _href; }
         set { _href = value; }
      }

      /// <summary>
      /// Gets or sets el icono a mostrar en el elemento de menú superior.
      /// </summary>
      public string Icon
      {
         get { return _icon; }
         set { _icon = value; }
      }

      /// <summary>
      /// Indica si el elemento es el activo.
      /// </summary>
      public bool Active
      {
         get { return _isActive; }
         set { _isActive = value; }
      }

      /// <summary>
      /// Gets or sets el tipo de elemento al que representa.
      /// </summary>
      public NavbarItemType Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Gets or sets la posición del elemento en la barra de botones.
      /// </summary>
      public NavbarItemPosition Position
      {
         get { return _position; }
         set { _position = value; }
      }

      /// <summary>
      /// Lista de elementos contenidos en el elemento actual (para generar desplegables).
      /// </summary>
      public List<NavbarIButtonControl> SubItems
      {
         get { return _subItems; }
         set { _subItems = value; }
      }

      /// <summary>
      /// Agrega un subelemento (para menús desplegables).
      /// </summary>
      public void AddSubitem(NavbarIButtonControl subitem)
      {
         if (subitem == null) return;
         if (_subItems == null) _subItems = new List<NavbarIButtonControl>();

         _subItems.Add(subitem);
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _isActive = false;
         _title = string.Empty;
         _href = "#";
         _subItems = null;
         _type = NavbarItemType.Button;
         _icon = string.Empty;
         _position = NavbarItemPosition.Left;
      }

      #endregion

   }
}
