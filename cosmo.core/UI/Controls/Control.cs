﻿using Cosmo.UI.Scripting;

namespace Cosmo.UI.Controls
{

   #region Enumerations

   /// <summary>
   /// Enumera los esquemas de color para representar determinados elementos.
   /// </summary>
   public enum ComponentColorScheme
   {
      /// <summary>Esquema de colores normal.</summary>
      Normal,
      /// <summary>Información.</summary>
      Information,
      /// <summary>Aviso.</summary>
      Warning,
      /// <summary>Error.</summary>
      Error,
      /// <summary>Acción completada correctamente.</summary>
      Success,
      /// <summary>Destaca el elemento como principal.</summary>
      Primary
   }

   /// <summary>
   /// Enumera los colores de fondo de determinados componentes.
   /// </summary>
   public enum ComponentBackgroundColor
   {
      /// <summary>Sin color definido: se usa el color por defecto.</summary>
      None,
      /// <summary>Color de fondo rojo (depende de la plantilla y/o el renderizador).</summary>
      Red,
      /// <summary>Color de fondo amarillo (depende de la plantilla y/o el renderizador).</summary>
      Yellow,
      /// <summary>Color de fondo azul agua (depende de la plantilla y/o el renderizador).</summary>
      Aqua,
      /// <summary>Color de fondo azul (depende de la plantilla y/o el renderizador).</summary>
      Blue,
      /// <summary>Color de fondo azul claro (depende de la plantilla y/o el renderizador).</summary>
      LightBlue,
      /// <summary>Color de fondo verde (depende de la plantilla y/o el renderizador).</summary>
      Green,
      /// <summary>Color de fondo azul marino (depende de la plantilla y/o el renderizador).</summary>
      Navy,
      /// <summary>Color de fondo marron claro (depende de la plantilla y/o el renderizador).</summary>
      Teal,
      /// <summary>Color de fondo verde claro (depende de la plantilla y/o el renderizador).</summary>
      Olive,
      /// <summary>Color de fondo lima (depende de la plantilla y/o el renderizador).</summary>
      Lime,
      /// <summary>Color de fondo naranja (depende de la plantilla y/o el renderizador).</summary>
      Orange,
      /// <summary>Color de fondo fucsia (depende de la plantilla y/o el renderizador).</summary>
      Fuchsia,
      /// <summary>Color de fondo morado (depende de la plantilla y/o el renderizador).</summary>
      Purple,
      /// <summary>Color de fondo marrón (depende de la plantilla y/o el renderizador).</summary>
      Maroon,
      /// <summary>Color de fondo negro (depende de la plantilla y/o el renderizador).</summary>
      Black,
      /// <summary>Color de fondo gris (depende de la plantilla y/o el renderizador).</summary>
      Gray
   }

   #endregion

   /// <summary>
   /// Clase abstracta que deben implementar todos los componentes de Bootstrap.
   /// </summary>
   public abstract class Control
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="Control"/>.
      /// </summary>
      /// <param name="container">Instancia de <see cref="ViewContainer"/> que contiene el control.</param>
      protected Control(ViewContainer container)
      {
         Initialize();

         this.Container = container;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="Control"/>.
      /// </summary>
      /// <param name="container">Instancia de <see cref="ViewContainer"/> que contiene el control.</param>
      /// <param name="domId">Identificador único del componente dentro de la vista (DOM).</param>
      protected Control(ViewContainer container, string domId)
      {
         Initialize();

         this.Container = container;
         this.DomID = domId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador del componente en el DOM.
      /// Sólo se debe establecer este valor si se desea acceder al componente a través de JavaScript.
      /// </summary>
      public string DomID { get; set; }

      /// <summary>
      /// Devuelve o establece la instancia de <see cref="ViewContainer"/> que contiene el control.
      /// </summary>
      public ViewContainer Container { get; set; }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega un script necesario para la correcta visualización del control.
      /// </summary>
      /// <param name="script">Una instancia de <see cref="Script"/> que representa el script a incoporar.</param>
      public void AddScript(Script script)
      {
         this.Container.Scripts.Add(script);
      }

      /// <summary>
      /// Convierte la instancia a una cadena que contiene el código XHTML necesario para representar el control.
      /// </summary>
      public string ToXhtml()
      {
         return Container.Workspace.UIService.Render(this);
      }

      /// <summary>
      /// Devuelve el parámetro ID para incrustar en un TAG XHTML.
      /// </summary>
      internal string GetIdParameter()
      {
         if (string.IsNullOrWhiteSpace(this.DomID))
         {
            return string.Empty;
         }
         else
         {
            return " id=\"" + this.DomID + "\" ";
         }
      }

      /// <summary>
      /// Devuelve el parámetro NAME para incrustar en un TAG XHTML.
      /// </summary>
      internal string GetNameParameter()
      {
         if (string.IsNullOrWhiteSpace(this.DomID))
         {
            return string.Empty;
         }
         else
         {
            return " name=\"" + this.DomID + "\" ";
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         // Inicializa los valores
         this.DomID = string.Empty;
         this.Container = null;
      }

      #endregion

   }
}
