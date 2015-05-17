﻿namespace Cosmo.UI.Controls
{

   #region Enumerations

   /// <summary>
   /// Enumera los tipos de slide de un carousel.
   /// </summary>
   public enum SlideType
   {
      /// <summary>Slide de tipo imagen.</summary>
      ImageSlide,
      /// <summary>Slide de tipo Xhtml.</summary>
      HtmlSlide,
      /// <summary>Slide de tipo Adobe Flash.</summary>
      FlashSlide
   }

   /// <summary>
   /// Enumera las posiciones dónde se pueden mostrar los caption en un slide.
   /// </summary>
   public enum SlideCaptionPositions
   {
      /// <summary>Izquierda.</summary>
      Left,
      /// <summary>Derecha.</summary>
      Right,
      /// <summary>Arriba.</summary>
      Top,
      /// <summary>Abajo.</summary>
      Bottom
   }

   #endregion

   /// <summary>
   /// Implementa un slide del carousel.
   /// </summary>
   public class CarrouselSlide
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de CSCarrouselSlide.
      /// </summary>
      public CarrouselSlide() 
      {
         Initialize();
      }

      #endregion

      #region Settings

      /// <summary>
      /// Devuelve o establece el nombre (con la ruta virtual) del archivo de imágen o SWF que se mostrará en el slide.
      /// </summary>
      public string FileName { get; set; }

      /// <summary>
      /// Devuelve o establece el nombre (con la ruta física) del archivo de imágen o SWF que se mostrará en el slide.
      /// </summary>
      public string FullName { get; set; }

      /// <summary>
      /// Devuelve o establece el tipo de slide a mostrar.
      /// </summary>
      public SlideType Type { get; set; }

      /// <summary>
      /// Devuelve o establece la posición dónde debe aparecer el caption del slide.
      /// </summary>
      public SlideCaptionPositions CaptionPosition { get; set; }

      /// <summary>
      /// Devuelve o establece el ancho del slide. Debe coincidir con el ancho del 
      /// banner especificado en la propiedad <code>FileName</code>.
      /// </summary>
      public int Width { get; set; }

      /// <summary>
      /// Devuelve o establece la altura del slide. Debe coincidir con el ancho del 
      /// banner especificado en la propiedad <code>FileName</code>.
      /// </summary>
      public int Height { get; set; }

      /// <summary>
      /// Devuelve o establece la URL de destino. Si no se establece el slide no será un enlace.
      /// </summary>
      public string Link { get; set; }

      /// <summary>
      /// Devuelve o establece el título del slide.
      /// </summary>
      public string Caption { get; set; }

      /// <summary>
      /// Devuelve o establece el nombre identificador del slide.
      /// Básicamente es usado para como texto alternativo de la imagen.
      /// </summary>
      public string Name { get; set; }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Width = 0;
         this.Height = 0;
         this.FileName = string.Empty;
         this.FullName = string.Empty;
         this.Name = string.Empty;
         this.Link = string.Empty;
         this.Caption = string.Empty;
         this.CaptionPosition = SlideCaptionPositions.Bottom;
         this.Type = SlideType.ImageSlide;
      }

      #endregion

   }
}