using System;
using System.Collections.Generic;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un botón insertable en un control <see cref="SidebarControl"/>.
   /// </summary>
   public class SidebarButton : Control
   {

      #region Constructors

      /// <summary>
      /// Gets an instance of <see cref="SidebarButton"/>.
      /// </summary>
      /// <param name="parentViewport">Página o contenedor dónde se representará el control.</param>
      public SidebarButton(View parentViewport)
         : base(parentViewport)
      {
         Initialize();
      }

      /// <summary>
      /// Gets an instance of <see cref="SidebarButton"/>.
      /// </summary>
      /// <param name="parentViewport">Página o contenedor dónde se representará el control.</param>
      /// <param name="caption"></param>
      /// <param name="href"></param>
      public SidebarButton(View parentViewport, string caption, string href)
         : base(parentViewport)
      {
         Initialize();

         this.Text = caption;
         this.Href = href;
      }

      /// <summary>
      /// Gets an instance of <see cref="SidebarButton"/>.
      /// </summary>
      /// <param name="parentViewport">Página o contenedor dónde se representará el control.</param>
      /// <param name="caption"></param>
      /// <param name="href"></param>
      /// <param name="icon"></param>
      public SidebarButton(View parentViewport, string caption, string href, string icon)
         : base(parentViewport)
      {
         Initialize();

         this.Text = caption;
         this.Href = href;
         this.Icon = icon;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si el elemento actual de la barra es el activo.
      /// </summary>
      public bool Active { get; set; }

      /// <summary>
      /// Gets or sets el texto visible del control.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or sets la URL de destino (enlace).
      /// </summary>
      public string Href { get; set; }

      /// <summary>
      /// Gets or sets el código del icono a mostrar junto al texto.
      /// </summary>
      public string Icon { get; set; }

      /// <summary>
      /// Gets or sets el texto que se mostrará en la etiqueta (<c>badge</c>).
      /// </summary>
      public string BadgeText { get; set; }

      /// <summary>
      /// Gets or sets el texto que se mostrará en la etiqueta (<c>badge</c>).
      /// </summary>
      public List<SidebarButton> SubItems { get; set; }

      /// <summary>
      /// Gets or sets the roles list allowed to view this element.
      /// </summary>
      public string[] Roles { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Set the roles list from a comma separated values list.
      /// </summary>
      /// <param name="rolesCsvList">Vomma separated values list of roles.</param>
      public void SetRoles(string rolesCsvList)
      {
         if (string.IsNullOrWhiteSpace(rolesCsvList))
         {
            this.Roles = new string[0];
         }
         else
         {
            this.Roles = rolesCsvList.Split(new string[1] { "," }, 
                                            StringSplitOptions.RemoveEmptyEntries);
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
         this.Href = string.Empty;
         this.Icon = string.Empty;
         this.BadgeText = string.Empty;
         this.SubItems = new List<SidebarButton>();
         this.Roles = new string[0];
      }

      #endregion

   }
}
