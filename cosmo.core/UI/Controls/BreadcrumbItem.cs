﻿namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un elemento insertable en el componente <see cref="BreadcrumbControl"/>.
   /// </summary>
   public class BreadcrumbItem
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="BreadcrumbItem"/>.
      /// </summary>
      public BreadcrumbItem()
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="BreadcrumbItem"/>.
      /// </summary>
      /// <param name="caption">Texto visible que se mostrará en el elemento.</param>
      /// <param name="href">URL asociada al enlace del elemento.</param>
      public BreadcrumbItem(string caption, string href)
      {
         Initialize();

         Caption = caption;
         Href = href;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="BreadcrumbItem"/>.
      /// </summary>
      /// <param name="caption">Texto visible que se mostrará en el elemento.</param>
      /// <param name="href">URL asociada al enlace del elemento.</param>
      /// <param name="icon">Código del icono a mostrar junto al elemento.</param>
      public BreadcrumbItem(string caption, string href, string icon)
      {
         Initialize();

         Caption = caption;
         Href = href;
         Icon = icon;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="BreadcrumbItem"/>.
      /// </summary>
      /// <param name="caption">Texto visible que se mostrará en el elemento.</param>
      /// <param name="href">URL asociada al enlace del elemento.</param>
      /// <param name="isActive">Indica si es el elemento activo.</param>
      public BreadcrumbItem(string caption, string href, bool isActive)
      {
         Initialize();

         Caption = caption;
         Href = href;
         IsActive = isActive;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si el elemento es o no el activo.
      /// </summary>
      public bool IsActive { get; set; }

      /// <summary>
      /// Devuelve o establece el texto visible que se mostrará en el elemento.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Devuelve o establece la URL asociada al enlace del elemento.
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Devuelve o establece el código del icono a mostrar junto al elemento.
      /// </summary>
      public string Icon { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         IsActive = false;
         Caption = string.Empty;
         Href = string.Empty;
         Icon = string.Empty;
      }

      #endregion

   }
}
